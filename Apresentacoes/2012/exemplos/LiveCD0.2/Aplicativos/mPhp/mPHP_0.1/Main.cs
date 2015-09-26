using TUVienna.CS_CUP.Runtime;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Globalization;


namespace PHP.Core {


	public class Compiler {

		public static string info;
		public static string help;
		public static ArrayList possibleOptions;

		static Compiler() {
			// info message
			StringBuilder infoSB = new StringBuilder();
			infoSB.Append("Mono PHP Compiler v0.1 by Raphael Romeikat" + Environment.NewLine);
			infoSB.Append("Licensed under GNU General Public License" + Environment.NewLine);
			infoSB.Append("http://sourceforge.net/projects/php4mono" + Environment.NewLine);
			info = infoSB.ToString();
			// help message
			StringBuilder helpSB = new StringBuilder();
			helpSB.Append("Usage: mono mPHP.exe [options] <source file>" + Environment.NewLine);
			helpSB.Append("Options:" + Environment.NewLine);
			helpSB.Append("-out:<file>      Specifies output file" + Environment.NewLine);
			helpSB.Append("                 (no directories allowed, just filename)" + Environment.NewLine);
			helpSB.Append("-target:<kind>   Specifies the target (short: -t:)" + Environment.NewLine);
			helpSB.Append("                 <kind> is one of: exe, library" + Environment.NewLine);
			helpSB.Append("-nowarn          Disables warnings" + Environment.NewLine);
			helpSB.Append("-help            Displays this usage message (short: -?)" + Environment.NewLine);
			help = helpSB.ToString();
			// possible options
			possibleOptions = new ArrayList();
			possibleOptions.Add("out");
			possibleOptions.Add("target");
			possibleOptions.Add("t");
			possibleOptions.Add("nowarn");
			possibleOptions.Add("?");
			possibleOptions.Add("help");
		}

		public static void Main(string[] args) {
			try {
				// display info message
				Console.WriteLine(info);

				// is there any parameter?
				if (args.Length == 0) {
					Report.Error(000);
					Console.WriteLine(help);
					return;
				}

				// process parameters for options
				ArrayList desiredOptions = new ArrayList();
				string sourceFilename = null;
				for (int i = 0; i < args.Length; i++) {
					string option = args[i];
					// determine which option it is
					string pureOption = option.ToLower();
					if (pureOption.StartsWith("/"))
						pureOption = pureOption.Remove(0, 1);
					else if (pureOption.StartsWith("--"))
						pureOption = pureOption.Remove(0, 2);
					else if (pureOption.StartsWith("-"))
						pureOption = pureOption.Remove(0, 1);
					string desiredOption = pureOption;
					if (pureOption.StartsWith("out:"))
						desiredOption = "out";
					else if (pureOption.StartsWith("target:"))
						desiredOption = "target";
					else if (pureOption.StartsWith("t:"))
						desiredOption = "t";
					// is option valid?
					if (possibleOptions.Contains(desiredOption))
						desiredOptions.Add(pureOption);
					else if (i == args.Length - 1)
						sourceFilename = option;
					else {
						Report.Error(004, desiredOption);
						return;
					}
				}
				// help option
				if (desiredOptions.Contains("?") || desiredOptions.Contains("help")) {
					Console.WriteLine(help);
					return;
				}
				// determine source file
				if (sourceFilename == null) {
					Report.Error(000);
					Console.WriteLine(help);
					return;
				}
				FileInfo sourceFile = null;
				StreamReader sourceFileStreamReader = null;
				string outputFilename = null;
				string directory = null;
				try {
					sourceFile = new FileInfo(sourceFilename);
					FileStream sourceFileStream = new FileStream(sourceFile.FullName, FileMode.Open, FileAccess.Read);
					sourceFileStreamReader = new StreamReader(sourceFileStream, new UTF8Encoding());
					directory = sourceFile.DirectoryName + Path.DirectorySeparatorChar;
				}
				catch (Exception) {
					Report.Error(001, sourceFile.FullName);
					return;
				}
				// other options
				int outputTarget = 0;
				foreach (string option in desiredOptions) {
					// out option
					if (option.StartsWith("out:")) {
						outputFilename = option.Remove(0, 4);
						if (outputFilename.IndexOf('\\') != -1 || outputFilename.IndexOf('/') != -1) {
							Report.Error(003, outputFilename);
							return;
						}
						if (!outputFilename.EndsWith(".exe"))
							outputFilename = outputFilename + ".exe";
					}
					// target option
					if (option.StartsWith("target:") || option.StartsWith("t:")) {
						string desiredTarget;
						if (option.StartsWith("target:"))
							desiredTarget = option.Remove(0, 7);
						else
							desiredTarget = option.Remove(0, 2);
						if (desiredTarget == "exe")
							outputTarget = PEmitter.EXE;
						else if (desiredTarget == "library")
							outputTarget = PEmitter.LIBRARY;
						else {
							Report.Error(005, desiredTarget);
							return;
						}
						PEmitter.target = outputTarget;
					}
					// nowarn option
					if (option == "nowarn") {
						Report.warningsEnabled = false;
					}
				}
				// if no output file specified, use and modify souce file name
				if (outputFilename == null) {
					string extension = null;
					if (outputTarget == PEmitter.EXE)
						extension = ".exe";
					else if (outputTarget == PEmitter.LIBRARY)
						extension = ".dll";
					int indexLastDot = sourceFile.Name.LastIndexOf('.');
					if (indexLastDot == -1)
						outputFilename = sourceFile.Name + extension;
					else
						outputFilename = sourceFile.Name.Substring(0, indexLastDot) + extension;
				}

				// set formatting to US Amiercan (needed e.g. for type Double)
				System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
				// initialize parser
				Console.WriteLine("Parsing from file " + sourceFile.FullName + "...");
				Parser yy = new Parser(new Scanner(sourceFileStreamReader));
				// parse input stream
				AST ast = (AST)yy.parse().value;
				// set output filename
				PEmitter.fileName = outputFilename;
				// transform AST to create class __MAIN and function __MAIN()
				//Console.WriteLine("Working on class __MAIN and function __MAIN()...");
				new MainMethodVisitor().Visit(ast);
				// ensure every class (except __MAIN) has a constructor
				//Console.WriteLine("Working on constructors...");
				new ConstructorVisitor().Visit(ast);
				// ensure every method has a return statement and cut unreachable statements after a return
				//Console.WriteLine("Working on return statements...");
				new ReturnStatementVisitor().Visit(ast);
				// ensure there is no break/continue without a loop
				//Console.WriteLine("Working on loops...");
				new LoopVisitor().Visit(ast);
				// reorder class declarations by inheritance
				//Console.WriteLine("Working on interitance...");
				new InheritanceVisitor().Visit(ast);
				// collect all delcarations which may be called before being declared
				//Console.WriteLine("Working on declarations...");
				new DeclarationsVisitor().Visit(ast);
				// build symbol table
				//Console.WriteLine("Building symbol table...");
				new SymbolTableVisitor().Visit(ast);
				// emit CIL code
				//Console.WriteLine("Emitting CIL code...");
				new EmitterVisitor().Visit(ast);
				// report success
				string success = "Compiling to file " + Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + outputFilename + " succeeded";
				if (Report.numberOfWarnings > 1)
					success += " with " + Report.numberOfWarnings + " warnings";
				else if (Report.numberOfWarnings == 1)
					success += " with 1 warning";
				Console.WriteLine(success);
			}
			catch (System.Exception e) {
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
			}
		}

	}


}