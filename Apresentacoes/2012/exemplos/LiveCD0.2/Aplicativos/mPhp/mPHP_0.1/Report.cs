using System;
using System.Collections;


namespace PHP.Core {


	public class Report {

		public static bool warningsEnabled = true;
		public static int numberOfErrors = 0;
		public static int numberOfWarnings = 0;
		public const int ERROR = 0;
		public const int WARNING = 1;

		public static void Error(string options) {
			Process(ERROR, -1, "", -1, -1);
		}

		public static void Error(int nr) {
			Process(ERROR, nr, "", -1, -1);
		}

		public static void Error(int nr, string options) {
			Process(ERROR, nr, options, -1, -1);
		}

		public static void Error(int nr, int line, int column) {
			Process(ERROR, nr, "", line, column);
		}

		public static void Error(int nr, string options, int line, int column) {
			Process(ERROR, nr, options, line, column);
		}

		public static void Warn(string options) {
			Process(WARNING, -1, "", -1, -1);
		}

		public static void Warn(int nr) {
			Process(WARNING, nr, "", -1, -1);
		}

		public static void Warn(int nr, string options) {
			Process(WARNING, nr, options, -1, -1);
		}

		public static void Warn(int nr, int line, int column) {
			Process(WARNING, nr, "", line, column);
		}

		public static void Warn(int nr, string options, int line, int column) {
			Process(WARNING, nr, options, line, column);
		}

		public static void Process(int kind, int nr, string options, int line, int column) {
			// count errors and warnings
			if (kind == Report.ERROR)
				numberOfErrors++;
			if (kind == Report.WARNING)
				numberOfWarnings++;
			// create text about kind
			string kindString = null;
			if (kind == ERROR)
				kindString = "Error ";
			else if (kind == WARNING)
				kindString = "Warning ";
			// create text about line/column, if supplied
			string location = "";
			if (line != -1 && column != -1)
				location = " (" + line + "," + column + ")";
			// create message
			string msg;
			switch (nr) {
				case -1: msg = "Unknown error occured"; break;
				case 000: msg = kindString + nr + location + ": No file to compile was specified"; break;
				case 001: msg = kindString + nr + location + ": Cannot open source file " + options; break;
				case 002: msg = kindString + nr + location + ": Cannot write to output file " + options; break;
				case 003: msg = kindString + nr + location + ": No directories allowed in output file " + options; break;
				case 004: msg = kindString + nr + location + ": The option " + options + " is unknown"; break;
				case 005: msg = kindString + nr + location + ": The target " + options + " is unknown"; break;
				case 100: msg = kindString + nr + location + ": Error in inheritance - missing parent declaration or cycle"; break;
				case 101: msg = kindString + nr + location + ": Cannot extend final class " + options; break;
				case 102: msg = kindString + nr + location + ": Cannot override final function " + options; break;
				case 103: msg = kindString + nr + location + ": The modifier " + options + " is not allowed for class members"; break;
				case 104: msg = kindString + nr + location + ": The modifier " + options + " is not allowed for constructors"; break;
				case 105: msg = kindString + nr + location + ": Multiple access type modifiers are not allowed"; break;
				case 200: msg = kindString + nr + location + ": Syntax error in input: " + options; break;
				case 201: msg = kindString + nr + location + ": The symbol " + options + " is a reserved word and cannot be declared"; break;
				case 202: msg = kindString + nr + location + ": The class " + options + " has already been declared"; break;
				case 203: msg = kindString + nr + location + ": The class " + options + " has not been declared"; break;
				case 204: msg = kindString + nr + location + ": The class member " + options + " has already been declared"; break;
				case 205: msg = kindString + nr + location + ": The class member " + options + " has not been declared"; break;
				case 206: msg = kindString + nr + location + ": The class member " + options + " is not visible"; break;
				case 207: msg = kindString + nr + location + ": The class member " + options + " has not been declared or is not visible"; break;
				case 208: msg = kindString + nr + location + ": The class member " + options + " is not static"; break;
				case 209: msg = kindString + nr + location + ": The static class member " + options + " has not been declared"; break;
				case 210: msg = kindString + nr + location + ": The static class member " + options + " is not visible"; break;
				case 211: msg = kindString + nr + location + ": The function " + options + " has already been declared"; break;
				case 212: msg = kindString + nr + location + ": The function " + options + " has not been declared"; break;
				case 213: msg = kindString + nr + location + ": The function " + options + " is not visible"; break;
				case 214: msg = kindString + nr + location + ": The function " + options + " has not been declared or is not visible"; break;
				case 215: msg = kindString + nr + location + ": The function " + options + " is not static"; break;
				case 216: msg = kindString + nr + location + ": The static function " + options + " has not been declared"; break;
				case 217: msg = kindString + nr + location + ": The static function " + options + " is not visible"; break;
				case 218: msg = kindString + nr + location + ": The variable " + options + " has not been defined"; break;
				case 219: msg = kindString + nr + location + ": This constant has already been defined"; break;
				case 300: msg = kindString + nr + location + ": Missing parameter " + options; break;
				case 301: msg = kindString + nr + location + ": Cannot pass parameter " + options + " by reference"; break;
				case 302: msg = kindString + nr + location + ": Parameter " + options + " has the wrong type"; break;
				case 303: msg = kindString + nr + location + ": Assigning the return value of new by reference is deprecated and will be ignored"; break;
				case 400: msg = kindString + nr + location + ": Constants may only evaluate to scalar values"; break;
				case 401: msg = kindString + nr + location + ": The specified data type is not an array and therefore cannot be accessed by offset"; break;
				case 402: msg = kindString + nr + location + ": Illegal key type when accessing array"; break;
				case 403: msg = kindString + nr + location + ": Trying to store to class member of a non-object type"; break;
				case 404: msg = kindString + nr + location + ": Trying to get class member of a non-object type"; break;
				case 405: msg = kindString + nr + location + ": Trying to invoke from class member of a non-object type"; break;
				case 406: msg = kindString + nr + location + ": A function call cannot be used as an argument for foreach: " + options; break;
				case 407: msg = kindString + nr + location + ": The argument supplied for foreach is invalid as it is not of array type"; break;
				case 408: msg = kindString + nr + location + ": Can't use a function return value in write context"; break;
				case 500: msg = kindString + nr + location + ": Unsupported operand types"; break;
				case 501: msg = kindString + nr + location + ": Break/Continue without a loop"; break;
				case 502: msg = kindString + nr + location + ": Using $this in a static function will only succeed when called from a non-static context"; break;
				case 503: msg = kindString + nr + location + ": Cannot access parent:: because current class doesn't inherit from a class"; break;
				case 900: msg = kindString + nr + location + ": The language element \"" + options + "\" is not supported in the current version of mPHP"; break;
				default: msg = "Unknown " + kindString.ToLower() + nr + location + " occured"; break;
			}
			if (kind == ERROR) {
				Console.Out.WriteLine(msg);
				// report fail and exit, if it is not a usage error
				if (nr == -1 || nr >= 100) {
					string fail = "Compiling failed: ";
					if (Report.numberOfErrors == 1)
						fail += Report.numberOfErrors + " error, ";
					else
						fail += Report.numberOfErrors + " errors, ";
					if (Report.numberOfWarnings == 1)
						fail += Report.numberOfWarnings + " warning";
					else
						fail += Report.numberOfWarnings + " warnings";
					Console.WriteLine(fail);
					Environment.Exit(0);
				}
			}
			else if (kind == WARNING && warningsEnabled)
				Console.WriteLine(msg);
		}

	}


}