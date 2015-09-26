using System;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;


namespace PHP.Core {


	public class PEmitter {

		public const int EXE = 0;
		public const int LIBRARY = 1;

		public static string fileName = null;
		public static int target = EXE;
		public static string moduleName;
		public static AssemblyName asmNam;
		public static AssemblyBuilder asmBld;
		public static ModuleBuilder modBld;

		public static void BeginModule() {
			if (fileName == null)
				fileName = "output.exe";
			moduleName = fileName.Substring(0, fileName.Length - 4);
			asmNam = new AssemblyName();
			asmNam.Name = moduleName;
			asmNam.Version = new Version(0, 1, 0, 0);
			asmBld = System.AppDomain.CurrentDomain.DefineDynamicAssembly(asmNam, AssemblyBuilderAccess.Save);
			modBld = asmBld.DefineDynamicModule(moduleName, fileName);
		}

		public static void EndModule() {
			//Console.WriteLine("Saving to file \"" + fileName + "\"...");
			try {
				asmBld.Save(fileName);
			}
			catch (Exception e) {
				Report.Error(002, e.Message);
			}
		}
	}


}