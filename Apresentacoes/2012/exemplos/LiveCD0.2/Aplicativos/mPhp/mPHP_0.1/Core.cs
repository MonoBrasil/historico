using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Reflection;
using PHP.Core;


namespace PHP.Core {


	public class Runtime {

		// used for switch statement
		public static bool switchInProgress;
		// used for calls of a static function from a non-static context
		public static Mixed thisForStaticContext;

		public static ArrayList functionCallTrace;

		public static int maxPlaceOnHeap;
		public static Hashtable heap;

		public static Hashtable variablePool;

		public static Hashtable caseSensitiveConstantPool;
		public static Hashtable caseInsensitiveConstantPool;

		public static int test = 0;

		static Runtime() {
			switchInProgress = false;
			thisForStaticContext = new Null();
			functionCallTrace = new ArrayList();
			functionCallTrace.Add("__MAIN->__MAIN");
			maxPlaceOnHeap = 0;
			heap = new Hashtable();
			variablePool = new Hashtable();
			caseSensitiveConstantPool = new Hashtable();
			caseInsensitiveConstantPool = new Hashtable();
		}

		public static void StoreToVariable(Mixed value, Mixed variableName) {
			// if a static variable is available, store there
			if (LoadFromStaticVariable(variableName) != null)
				StoreToStaticVariable(value, variableName);
			// otherwise store as a local variable in current scope
			else
				StoreToLocalVariable(value, variableName);
		}

		public static void StoreToLocalVariable(Mixed value, Mixed variableName) {
			string scope = FunctionCallTraceAsString();
			StoreToVariable(scope, value, variableName);
		}

		public static void StoreToStaticVariable(Mixed value, Mixed variableName) {
			string scope = (string)functionCallTrace[functionCallTrace.Count - 1];
			// remove the local version of the variable and adopt place on heap
			Mixed localVar = LoadFromLocalVariable(variableName);
			if (localVar != null) {
				value.__placeOnHeap = localVar.__placeOnHeap;
				UnsetVariable(variableName);
			}
			// store to static variable
			StoreToVariable(scope, value, variableName);
		}

		public static void StoreToGlobalVariable(Mixed value, Mixed variableName) {
			string scope = "__MAIN->__MAIN";
			StoreToVariable(scope, value, variableName);
		}

		private static void StoreToVariable(string scope, Mixed value, Mixed variableName) {
			string variableNameString = Convert.ToString(variableName).value;
			// use scope delivered
			Hashtable variablePoolOfScope;
			if (variablePool[scope] == null) {
				variablePoolOfScope = new Hashtable();
				variablePool[scope] = variablePoolOfScope;
			}
			else
				variablePoolOfScope = (Hashtable)variablePool[scope];
			// if no value passed, use Null
			if (value == null)
				value = new Null();
			// determine place on heap
			if (value.__placeOnHeap == 0) {
				if (variablePoolOfScope[variableNameString] == null)
					value.__placeOnHeap = ++maxPlaceOnHeap;
				else
					value.__placeOnHeap = (int)variablePoolOfScope[variableNameString];
			}
			// add variable to variable pool
			variablePoolOfScope[variableNameString] = value.__placeOnHeap;
			// store value to heap
			heap[value.__placeOnHeap] = value;
		}

		public static Mixed LoadFromVariable(Mixed variableName) {
			string scope = FunctionCallTraceAsString();
			// if a local variable is available, take it
			Mixed result = LoadFromLocalVariable(variableName);
			// otherwise look for a static variable
			if (result == null)
				result = LoadFromStaticVariable(variableName);
			// anything found?
			if (result == null) {
				string variableNameString = Convert.ToString(variableName).value;
				Report.Warn(218, variableNameString);
				return new Null();
			}
			else
				return result;
		}

		private static Mixed LoadFromLocalVariable(Mixed variableName) {
			string scope = FunctionCallTraceAsString();
			return LoadFromVariable(scope, variableName);
		}

		private static Mixed LoadFromStaticVariable(Mixed variableName) {
			string scope = (string)functionCallTrace[functionCallTrace.Count - 1];
			return LoadFromVariable(scope, variableName);
		}

		public static Mixed LoadFromGlobalVariable(Mixed variableName) {
			string scope = "__MAIN->__MAIN";
			// look for variable in global scope
			Mixed result = LoadFromVariable(scope, variableName);
			// anything found?
			if (result == null) {
				string variableNameString = Convert.ToString(variableName).value;
				Report.Warn(218, variableNameString);
				return new Null();
			}
			else
				return result;
		}

		private static Mixed LoadFromVariable(string scope, Mixed variableName) {
			// use scope delivered
			Hashtable variablePoolOfScope = (Hashtable)variablePool[scope];
			if (variablePoolOfScope == null)
				return null;
			// fetch desired variable
			string variableNameString = Convert.ToString(variableName).value;
			if (variablePoolOfScope[variableNameString] == null)
				return null;
			int id = (int)variablePoolOfScope[variableNameString];
			return LoadFromHeap(id);
		}

		public static Mixed LoadFromHeap(int id) {
			if (heap[id] == null)
				return new Null();
			else
				return (Mixed)heap[id];
		}

		public static void AddFunctionCallToTrace(string functionCall) {
			functionCallTrace.Add(functionCall);
		}

		public static void RemoveFunctionCallFromTrace() {
			// unset local variables defined in current scope
			variablePool[FunctionCallTraceAsString()] = null;
			// remove function call from trace
			functionCallTrace.RemoveAt(functionCallTrace.Count - 1);
		}

		public static string FunctionCallTraceAsString() {
			StringBuilder result = new StringBuilder();
			for (int i = 0; i < functionCallTrace.Count; i++) {
				string functionCall = (string)functionCallTrace[i];
				result.Append(functionCall);
				result.Append("=>");
			}
			if (result.Length > 0)
				result.Remove(result.Length - 2, 2);
			return result.ToString();
		}

		public static void UnsetVariable(Mixed variableName) {
			string variableNameString = Convert.ToString(variableName).value;
			// use current scope
			string scope = FunctionCallTraceAsString();
			// unset variable
			Hashtable variablePoolOfScope = (Hashtable)variablePool[scope];
			if (variablePoolOfScope != null)
				variablePoolOfScope[variableNameString] = null;
		}

		public static Boolean DefineConstant(Mixed name, Mixed value, Mixed caseInsensitive) {
			// ensure value is scalar
			bool valueIsScalar = value is Boolean || value is Integer || value is Double || value is String;
			if (!valueIsScalar) {
				Report.Warn(400);
				return new Boolean(false);
			}
			// add to constant pool
			string nameString = Convert.ToString(name).value;
			// as case insensitive
			if (Convert.ToBoolean(caseInsensitive).value) {
				if (caseInsensitiveConstantPool[nameString.ToLower()] == null) {
					caseInsensitiveConstantPool[nameString.ToLower()] = value;
					return new Boolean(true);
				}
				else {
					Report.Warn(219);
					return new Boolean(false);
				}
			}
			// as case sensitive
			else {
				if (caseSensitiveConstantPool[nameString] == null) {
					caseSensitiveConstantPool[nameString] = value;
					return new Boolean(true);
				}
				else {
					Report.Warn(219);
					return new Boolean(false);
				}
			}
		}

		public static Mixed GetConstant(Mixed name) {
			string nameString = Convert.ToString(name).value;
			// if the desired constant exists as case sensitive, take it
			if (caseSensitiveConstantPool[nameString] != null)
				return (Mixed)caseSensitiveConstantPool[nameString];
			// else if is exists as case insensitive, take it
			else if (caseInsensitiveConstantPool[nameString.ToLower()] != null)
				return (Mixed)caseInsensitiveConstantPool[nameString.ToLower()];
			// nothing found
			else
				return new Null();
		}

		public static void StoreToClassMember(Mixed classObject, Mixed value, Mixed classMemberName) {
			string classMemberNameString = Convert.ToString(classMemberName).value;
			// check if an Object was passed
			if (!(classObject is Object)) {
				Report.Warn(403);
				return;
			}
			// store
			FieldInfo fi = classObject.GetType().GetField(classMemberNameString);
			if (fi == null) {
				Report.Warn(207, classMemberNameString);
				return;
			}
			else {
				// determine place on heap
				if (value.__placeOnHeap == 0) {
					Mixed oldObject = (Mixed)fi.GetValue(classObject);
					if (oldObject.__placeOnHeap == 0)
						value.__placeOnHeap = ++maxPlaceOnHeap;
					else
						value.__placeOnHeap = (int)oldObject.__placeOnHeap;
				}
				// add variable to variable pool
				fi.SetValue(classObject, value);
				// store value to heap
				heap[value.__placeOnHeap] = value;
			}
		}

		public static Mixed LoadFromClassMember(Mixed classObject, Mixed classMemberName) {
			string classMemberNameString = Convert.ToString(classMemberName).value;
			// check if an Object was passed
			if (!(classObject is Object)) {
				Report.Warn(403);
				return new Null();
			}
			// load
			FieldInfo fi = classObject.GetType().GetField(classMemberNameString);
			if (fi == null) {
				Report.Warn(207, classMemberNameString);
				return new Null();
			}
			else {
				Mixed result = (Mixed)fi.GetValue(classObject);
				return Refresh(result);
			}
		}

		public static Mixed Refresh(Mixed m) {
			// if object is available on heap, load newly from there
			if (m.__placeOnHeap > 0)
				return LoadFromHeap(m.__placeOnHeap);
			// else return passed object as it is
			else
				return m;
		}

		public static Mixed InvokeFunction(Mixed classObject, ArrayList parametersSupplied, Mixed functionName) {
			string functionNameString = Convert.ToString(functionName).value;
			// check if an Object was passed
			if (!(classObject is Object)) {
				Report.Warn(405);
				return new Null();
			}
			// invoke
			MethodInfo mi = classObject.GetType().GetMethod(functionNameString);
			if (mi == null) {
				Report.Warn(214, functionNameString);
				return new Null();
			}
			else {
				// pass parameters (only as many as needed)
				int parametersNeeded = mi.GetParameters().Length;
				object[] parametersPassed = new object[parametersNeeded];
				int i = 0;
				for (; i < Math.Min(parametersNeeded, parametersSupplied.Count); i++)
					parametersPassed[i] = parametersSupplied[i];
				// if less parameters actually passed then necessary, pass Null objects instead
				for (; i < parametersNeeded; i++)
					parametersPassed[i] = new Null();
				// add function call to trace
				AddFunctionCallToTrace(classObject.GetType().Name + "->" + functionNameString);
				// invoke
				Mixed result = (Mixed)mi.Invoke(classObject, parametersPassed);
				// remove function call to trace
				RemoveFunctionCallFromTrace();
				// push return value
				return result;
			}
		}

	}


	public class Lang {

		public static TextWriter stdOut;
		public static TextWriter nowhere;

		static Lang() {
			stdOut = Console.Out;
			nowhere = new StringWriter();
		}

		public static void Init() {
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
		}

		public static void CheckTypeHint(Mixed m, Type t, int paramIndex) {
			bool typeHintOk = ((Boolean)Instanceof(m, t)).value;
			if (!(typeHintOk))
				Report.Error(302, System.Convert.ToString(paramIndex));
		}

		public static Mixed Offset(Mixed m1, Mixed m2, int kind) {
			// if empty offset passed, return Null
			if (m2 == null)
				return new Null();
			// else process offset
			if (kind == OFFSET.SQUARE) {
				if (m1 is Array) {
					Array a = (Array)m1;
					return a.Get(m2);
				}
				// ignore offset
				else {
					if (!(m1 is Null))
						Report.Warn(401);
					return m1;
				}
			}
			else
				return null;
		}

		public static bool CurrentIsValid(Array a) {
			return a.current >= 0 && a.current < a.keys.Count;
		}

		public static void Echo(Mixed m) {
			if (m is Boolean) {
				if (((Boolean)m).value)
					Console.Write(1);
			}
			else if (m is Integer)
				Console.Write(((Integer)m).value);
			else if (m is Double)
				Console.Write(((Double)m).value);
			else if (m is String)
				Console.Write(((String)m).value);
			else if (m is Array)
				Console.Write("Array");
			else if (m is Object)
				Console.Write("Object id #" + ((Object)m).__id);
			else if (m is Null) { /* do nothing */ }

		}

		public static Mixed Instanceof(Mixed m, Type t) {
			return new Boolean(t.IsInstanceOfType(m));
		}

		public static Mixed BooleanNot(Mixed m) {
			Boolean b = Convert.ToBoolean(m);
			bool result = b.value;
			return new Boolean(!result);
		}

		public static Mixed Not(Mixed m) {
			if (m is String) {
				StringBuilder result = new StringBuilder();
				string s = Convert.ToString(m).value;
				// process on ASCII value of characters
				for (int i = 0; i < s.Length; i++)
					result.Append((char)(~s[i]));
				// done
				return new String(result.ToString());
			}
			else {
				Integer i = Convert.ToInteger(m);
				return new Integer(~i.value);
			}
		}

		public static Mixed Exit(Mixed m) {
			int exitCode = 0;
			if (m is Integer)
				exitCode = ((Integer)m).value;
			Environment.Exit(exitCode);
			return null;
		}

		public static Mixed Print(Mixed m) {
			Echo(m);
			return new Integer(1);
		}

		public static Mixed BooleanAnd(Mixed m1, Mixed m2) {
			Boolean b1 = Convert.ToBoolean(m1);
			Boolean b2 = Convert.ToBoolean(m2);
			bool result = b1.value && b2.value;
			return new Boolean(result);
		}

		public static Mixed BooleanOr(Mixed m1, Mixed m2) {
			Boolean b1 = Convert.ToBoolean(m1);
			Boolean b2 = Convert.ToBoolean(m2);
			bool result = b1.value || b2.value;
			return new Boolean(result);
		}

		public static Mixed LogicalAnd(Mixed m1, Mixed m2) {
			Boolean b1 = Convert.ToBoolean(m1);
			Boolean b2 = Convert.ToBoolean(m2);
			bool result = b1.value && b2.value;
			return new Boolean(result);
		}

		public static Mixed LogicalOr(Mixed m1, Mixed m2) {
			Boolean b1 = Convert.ToBoolean(m1);
			Boolean b2 = Convert.ToBoolean(m2);
			bool result = b1.value || b2.value;
			return new Boolean(result);
		}

		public static Mixed LogicalXor(Mixed m1, Mixed m2) {
			Boolean b1 = Convert.ToBoolean(m1);
			Boolean b2 = Convert.ToBoolean(m2);
			bool result = b1.value ^ b2.value;
			return new Boolean(result);
		}

		public static Mixed Concat(Mixed m1, Mixed m2) {
			String s1 = Convert.ToString(m1);
			String s2 = Convert.ToString(m2);
			string result = s1.value + s2.value;
			return new String(result);
		}

		public static Mixed Plus(Mixed m1, Mixed m2) {
			if (m1 is Array && !(m2 is Array) || m2 is Array && !(m1 is Array))
				Report.Error(500);
			// if one operand is an array, perform array union
			if (m1 is Array || m2 is Array) {
				Array a1 = (Array)m1;
				Array a2 = (Array)m2;
				Array result = new Array();
				result.keys.AddRange(a1.keys);
				result.values.AddRange(a1.values);
				// only add a key (and its object) if it wasn't in a1
				for (int i = 0; i < a2.keys.Count; i++) {
					Mixed key = (Mixed)a2.keys[i];
					int value = (int)a2.values[i];
					if (!result.keys.Contains(key)) {
						result.keys.Add(key);
						result.values.Add(value);
					}
				}
				return result;
			}
			// else perform regular additions
			else {
				Double f1 = Convert.ToDouble(m1);
				Double f2 = Convert.ToDouble(m2);
				double result = f1.value + f2.value;
				if (result % 1 == 0)
					return new Integer((int)result);
				else
					return new Double(result);
			}
		}

		public static Mixed Minus(Mixed m1, Mixed m2) {
			if (m1 is Array || m2 is Array)
				Report.Error(500);
			Double f1 = Convert.ToDouble(m1);
			Double f2 = Convert.ToDouble(m2);
			double result = f1.value - f2.value;
			if (result % 1 == 0)
				return new Integer((int)result);
			else
				return new Double(result);
		}

		public static Mixed Times(Mixed m1, Mixed m2) {
			if (m1 is Array || m2 is Array)
				Report.Error(500);
			Double f1 = Convert.ToDouble(m1);
			Double f2 = Convert.ToDouble(m2);
			double result = f1.value * f2.value;
			if (result % 1 == 0)
				return new Integer((int)result);
			else
				return new Double(result);
		}

		public static Mixed Div(Mixed m1, Mixed m2) {
			if (m1 is Array || m2 is Array)
				Report.Error(500);
			Double f1 = Convert.ToDouble(m1);
			Double f2 = Convert.ToDouble(m2);
			double result = f1.value / f2.value;
			if (result % 1 == 0)
				return new Integer((int)result);
			else
				return new Double(result);
		}

		public static Mixed Mod(Mixed m1, Mixed m2) {
			Double f1 = Convert.ToDouble(m1);
			Double f2 = Convert.ToDouble(m2);
			double result = f1.value % f2.value;
			if (result % 1 == 0)
				return new Integer((int)result);
			else
				return new Double(result);
		}

		public static Mixed And(Mixed m1, Mixed m2) {
			if (m1 is String && m2 is String) {
				StringBuilder result = new StringBuilder();
				string s1 = Convert.ToString(m1).value;
				string s2 = Convert.ToString(m2).value;
				// cut strings to same length
				if (s1.Length > s2.Length)
					s1 = s1.Substring(0, s2.Length);
				else if (s2.Length > s1.Length)
					s2 = s2.Substring(0, s1.Length);
				// process on ASCII value of characters
				for (int i = 0; i < s1.Length; i++)
					result.Append((char)(s1[i] & s2[i]));
				// done
				return new String(result.ToString());
			}
			else {
				Integer i1 = Convert.ToInteger(m1);
				Integer i2 = Convert.ToInteger(m2);
				return new Integer(i1.value & i2.value);
			}
		}

		public static Mixed Or(Mixed m1, Mixed m2) {
			if (m1 is String && m2 is String) {
				StringBuilder result = new StringBuilder();
				string s1 = Convert.ToString(m1).value;
				string s2 = Convert.ToString(m2).value;
				// cut strings to same length
				if (s1.Length > s2.Length)
					s1 = s1.Substring(0, s2.Length);
				else if (s2.Length > s1.Length)
					s2 = s2.Substring(0, s1.Length);
				// process on ASCII value of characters
				for (int i = 0; i < s1.Length; i++)
					result.Append((char)(s1[i] | s2[i]));
				// done
				return new String(result.ToString());
			}
			else {
				Integer i1 = Convert.ToInteger(m1);
				Integer i2 = Convert.ToInteger(m2);
				return new Integer(i1.value | i2.value);
			}
		}

		public static Mixed Xor(Mixed m1, Mixed m2) {
			if (m1 is String && m2 is String) {
				StringBuilder result = new StringBuilder();
				string s1 = Convert.ToString(m1).value;
				string s2 = Convert.ToString(m2).value;
				// cut strings to same length
				if (s1.Length > s2.Length)
					s1 = s1.Substring(0, s2.Length);
				else if (s2.Length > s1.Length)
					s2 = s2.Substring(0, s1.Length);
				// process on ASCII value of characters
				for (int i = 0; i < s1.Length; i++)
					result.Append((char)(s1[i] ^ s2[i]));
				// done
				return new String(result.ToString());
			}
			else {
				Integer i1 = Convert.ToInteger(m1);
				Integer i2 = Convert.ToInteger(m2);
				return new Integer(i1.value ^ i2.value);
			}
		}

		public static Mixed Sl(Mixed m1, Mixed m2) {
			if (m1 is String && m2 is String) {
				StringBuilder result = new StringBuilder();
				string s1 = Convert.ToString(m1).value;
				string s2 = Convert.ToString(m2).value;
				// cut strings to same length
				if (s1.Length > s2.Length)
					s1 = s1.Substring(0, s2.Length);
				else if (s2.Length > s1.Length)
					s2 = s2.Substring(0, s1.Length);
				// process on ASCII value of characters
				for (int i = 0; i < s1.Length; i++)
					result.Append((char)(s1[i] << s2[i]));
				// done
				return new String(result.ToString());
			}
			else {
				Integer i1 = Convert.ToInteger(m1);
				Integer i2 = Convert.ToInteger(m2);
				return new Integer(i1.value << i2.value);
			}
		}

		public static Mixed Sr(Mixed m1, Mixed m2) {
			if (m1 is String && m2 is String) {
				StringBuilder result = new StringBuilder();
				string s1 = Convert.ToString(m1).value;
				string s2 = Convert.ToString(m2).value;
				// cut strings to same length
				if (s1.Length > s2.Length)
					s1 = s1.Substring(0, s2.Length);
				else if (s2.Length > s1.Length)
					s2 = s2.Substring(0, s1.Length);
				// process on ASCII value of characters
				for (int i = 0; i < s1.Length; i++)
					result.Append((char)(s1[i] >> s2[i]));
				// done
				return new String(result.ToString());
			}
			else {
				Integer i1 = Convert.ToInteger(m1);
				Integer i2 = Convert.ToInteger(m2);
				return new Integer(i1.value >> i2.value);
			}
		}

		public static Boolean IsEqual(Mixed m1, Mixed m2) {
			// if both operands are arrays, check equality on array pairs
			if (m1 is Array && m2 is Array) {
				Array a1 = (Array)m1;
				Array a2 = (Array)m2;
				return new Boolean(a1.Equals(a2));
			}
			// if both operands are objects, check identity on instances
			else if (m1 is Object && m2 is Object) {
				Object o1 = (Object)m1;
				Object o2 = (Object)m2;
				return new Boolean(o1.Equals(o2));
			}
			// otherwise perform regular equality check
			else {
				if (m1 is Boolean) {
					Boolean b1 = (Boolean)m1;
					Boolean b2 = Convert.ToBoolean(m2);
					return new Boolean(b1.value == b2.value);
				}
				else if (m2 is Boolean) {
					Boolean b1 = Convert.ToBoolean(m1);
					Boolean b2 = (Boolean)m2;
					return new Boolean(b1.value == b2.value);
				}
				else if (m1 is Integer) {
					Integer i1 = (Integer)m1;
					Integer i2 = Convert.ToInteger(m2);
					return new Boolean(i1.value == i2.value);
				}
				else if (m2 is Integer) {
					Integer i1 = Convert.ToInteger(m1);
					Integer i2 = (Integer)m2;
					return new Boolean(i1.value == i2.value);
				}
				else if (m1 is Double) {
					Double i1 = (Double)m1;
					Double i2 = Convert.ToDouble(m2);
					return new Boolean(i1.value == i2.value);
				}
				else if (m2 is Double) {
					Double i1 = Convert.ToDouble(m1);
					Double i2 = (Double)m2;
					return new Boolean(i1.value == i2.value);
				}
				else if (m1 is String) {
					String i1 = (String)m1;
					String i2 = Convert.ToString(m2);
					return new Boolean(i1.value == i2.value);
				}
				else if (m2 is String) {
					String i1 = Convert.ToString(m1);
					String i2 = (String)m2;
					return new Boolean(i1.value == i2.value);
				}
				else if (m1 is Null) {
					return new Boolean(m2 is Null);
				}
				else if (m2 is Null) {
					return new Boolean(m1 is Null);
				}
				else
					return new Boolean(false);
			}
		}

		public static Boolean IsNotEqual(Mixed m1, Mixed m2) {
			Boolean isEqual = IsEqual(m1, m2);
			return new Boolean(!isEqual.value);
		}

		public static Boolean IsIdentical(Mixed m1, Mixed m2) {
			// if both operands are arrays, check identity on array pairs
			if (m1 is Array && m2 is Array) {
				Array a1 = (Array)m1;
				Array a2 = (Array)m2;
				if (a1.keys.Count != a2.keys.Count)
					return new Boolean(false);
				for (int i = 0; i < a1.keys.Count; i++) {
					if (!IsIdentical((Mixed)a1.keys[i], (Mixed)a2.keys[i]).value)
						return new Boolean(false);
					if ((int)a1.values[i] != (int)a2.values[i])
						return new Boolean(false);
				}
				return new Boolean(true);
			}
			// if both operands are objects, check identity on instances
			else if (m1 is Object && m2 is Object) {
				int id1 = ((Object)m1).__id;
				int id2 = ((Object)m2).__id;
				return new Boolean(id1 == id2);
			}
			// otherwise perform regular identity check
			else {
				if (m1.GetType() == m2.GetType())
					return IsEqual(m1, m2);
				else
					return new Boolean(false);
			}
		}

		public static Boolean IsNotIdentical(Mixed m1, Mixed m2) {
			Boolean isIdentical = IsIdentical(m1, m2);
			return new Boolean(!isIdentical.value);
		}

		public static Boolean Lower(Mixed m1, Mixed m2) {
			Double f1 = Convert.ToDouble(m1);
			Double f2 = Convert.ToDouble(m2);
			return new Boolean(f1.value < f2.value);
		}

		public static Boolean IsLowerOrEqual(Mixed m1, Mixed m2) {
			Double f1 = Convert.ToDouble(m1);
			Double f2 = Convert.ToDouble(m2);
			return new Boolean(f1.value <= f2.value);
		}

		public static Boolean Greater(Mixed m1, Mixed m2) {
			Double f1 = Convert.ToDouble(m1);
			Double f2 = Convert.ToDouble(m2);
			return new Boolean(f1.value > f2.value);
		}

		public static Boolean IsGreaterOrEqual(Mixed m1, Mixed m2) {
			Double f1 = Convert.ToDouble(m1);
			Double f2 = Convert.ToDouble(m2);
			return new Boolean(f1.value >= f2.value);
		}

		public static Mixed Clone(Mixed m) {
			if (m is Boolean)
				return new Boolean(((Boolean)m).value);
			else if (m is Integer)
				return new Integer(((Integer)m).value);
			else if (m is Double)
				return new Double(((Double)m).value);
			else if (m is String)
				return new String(((String)m).value);
			else if (m is Array) {
				Array a = (Array)m;
				Mixed clonedKey;
				int clonedValue;
				ArrayList clonedKeys = new ArrayList();
				ArrayList clonedValues = new ArrayList();
				for (int i = 0; i < a.keys.Count; i++) {
					clonedKey = Clone((Mixed)a.keys[i]);
					clonedValue = (int)a.values[i];
					clonedKeys.Add(clonedKey);
					clonedValues.Add(clonedValue);
				}
				return new Array(clonedKeys, clonedValues);
			}
			else if (m is Object) {
				// find constructor of object to be cloned
				ConstructorInfo ctor = null;
				foreach(ConstructorInfo ci in m.GetType().GetConstructors()) {
					if (!ci.IsStatic)
						ctor = ci;
				}
				int parameterCount = ctor.GetParameters().Length;
				object[] parameters = new object[parameterCount];
				for (int i = 0; i < parameters.Length; i++)
					parameters[i] = new Null();
				// set the standard out to nowhere to avoid output when calling the constructor right now
				Console.SetOut(nowhere);
				// create new instance
				Object result = (Object)Activator.CreateInstance(m.GetType(), parameters);
				// set field values
				foreach (FieldInfo f in m.GetType().GetFields()) {
					// don't use the internal fields __id and __placeOnHeap
					if (f.Name != "__id" && f.Name != "__placeOnHeap")
						f.SetValue(result, f.GetValue(m));
				}
				// if a __clone function is available, invoke
				MethodInfo clone = m.GetType().GetMethod("__clone", Type.EmptyTypes);
				if (clone != null) {
					clone.Invoke(result, null);
				}
				// reset the standard output
				Console.SetOut(stdOut);
				return result;
			}
			else if (m is Null)
				return new Null();
			else
				return new Null();
		}

	}

	public class Convert {

		public static Boolean ToBoolean(Mixed m) {
			if (m is Boolean)
				return (Boolean)m;
			else if (m is Integer) {
				if (((Integer)m).value == 0)
					return new Boolean(false);
				else
					return new Boolean(true);
			}
			else if (m is Double) {
				if (((Double)m).value == 0)
					return new Boolean(false);
				else
					return new Boolean(true);
			}
			else if (m is String) {
				if (((String)m).value == "" || ((String)m).value == "0")
					return new Boolean(false);
				else
					return new Boolean(true);
			}
			else if (m is Array) {
				if (((Array)m).keys.Count == 0)
					return new Boolean(false);
				else
					return new Boolean(true);
			}
			else if (m is Object)
				return new Boolean(true);
			else if (m is Null)
				return new Boolean(false);
			else
				return new Boolean(false);
		}

		public static Integer ToInteger(Mixed m) {
			if (m is Boolean) {
				if (((Boolean)m).value)
					return new Integer(1);
				else
					return new Integer(0);
			}
			else if (m is Integer)
				return (Integer)m;
			else if (m is Double)
				return new Integer((int)Math.Floor(((Double)m).value));
			else if (m is String) {
				string toDoubleShortenedString = ShortenToDoubleString(((String)m).value);
				double d = System.Convert.ToDouble(toDoubleShortenedString);
				return new Integer((int)Math.Floor(d));
			}
			else if (m is Null)
				return new Integer(0);
			else
				return ToInteger(ToBoolean(m));
		}

		public static Double ToDouble(Mixed m) {
			if (m is Boolean) {
				if (((Boolean)m).value)
					return new Double(1);
				else
					return new Double(0);
			}
			else if (m is Integer)
				return new Double(((Integer)m).value);
			else if (m is Double)
				return (Double)m;
			else if (m is String) {
				string toDoubleShortenedString = ShortenToDoubleString(((String)m).value);
				double d = System.Convert.ToDouble(toDoubleShortenedString);
				return new Double(d);
			}
			else if (m is Null)
				return new Double(0);
			else
				return ToDouble(ToInteger(m));
		}

		public static String ToString(Mixed m) {
			if (m is Boolean) {
				if (((Boolean)m).value)
					return new String("1");
				else
					return new String("");
			}
			else if (m is Integer)
				return new String(((Integer)m).value.ToString());
			else if (m is Double)
				return new String(((Double)m).value.ToString());
			else if (m is String)
				return (String)m;
			else if (m is Array)
				return new String("Array");
			else if (m is Object)
				return new String("Object id #" + ((Object)m).__id);
			else if (m is Null)
				return new String("");
			else
				return new String("");
		}

		public static Array ToArray(Mixed m) {
			if (m is Boolean) {
				Array a = new Array();
				a.Append(m);
				return a;
			}
			else if (m is Integer) {
				Array a = new Array();
				a.Append(m);
				return a;
			}
			else if (m is Double) {
				Array a = new Array();
				a.Append(m);
				return a;
			}
			else if (m is String) {
				Array a = new Array();
				a.Append(m);
				return a;
			}
			else if (m is Array)
				return (Array)m;
			else if (m is Object) {
				Array result = new Array();
				foreach (FieldInfo f in m.GetType().GetFields()) {
					// don't use the internal fields __id and __maxId
					if (f.Name != "__id" && f.Name != "__maxId") {
						String name;
						if (f.Name.StartsWith("$"))
							name = new String(f.Name.Substring(1, f.Name.Length - 1));
						else
							name = new String(f.Name);
						Mixed value = (Mixed)f.GetValue(m);
						result.Append(name, value);
					}
				}
				return result;
			}
			else if (m is Null)
				return new Array();
			else
				return new Array();
		}

		public static Object ToObject(Mixed m) {
			if (m is Object)
				return (Object)m;
			else
				return new stdClass(m);
		}

		public static bool ToValueBool(Mixed m) {
			return ToBoolean(m).value;
		}

		public static int ToValueInt(Mixed m) {
			return ToInteger(m).value;
		}

		public static double ToValueDouble(Mixed m) {
			return ToDouble(m).value;
		}

		public static string ToValueString(Mixed m) {
			return ToString(m).value;
		}

		public static string ShortenToDoubleString(string s) {
			int i = 0;
			// optional sign
			if (i < s.Length) {
				if (s[i] == '+' || s[i] == '-')
					i++;
			}
			else
				return "0";
			// if sign was the only input, return 0
			if (i == s.Length)
				return "0";
			// optional digits
			while (i < s.Length && s[i] >= '0' && s[i] <= '9')
				i++;
			// optional dot
			if (i < s.Length) {
				if (s[i] == '.')
					i++;
			}
			else
				return s.Substring(0, i);
			// optional digits
			while (i < s.Length && s[i] >= '0' && s[i] <= '9')
				i++;
			// optional e
			bool eFound = false;
			if (i < s.Length) {
				if (s[i] == 'e' || s[i] == 'E') {
					i++;
					eFound = true;
				}
			}
			else
				return s.Substring(0, i);
			// optional digits
			bool digitsFoundAfterE = false;
			while (i < s.Length && s[i] >= '0' && s[i] <= '9') {
				i++;
				digitsFoundAfterE = true;
			}
			// cut e if not followed by digits
			if (eFound && !digitsFoundAfterE)
				i--;
			// done
			return s.Substring(0, i);
		}

	}


	public class Modifiers {
		public const int PUBLIC = 0;
		public const int PROTECTED = 1;
		public const int PRIVATE = 2;
		public const int STATIC = 3;
		public const int ABSTRACT = 4;
		public const int FINAL = 5;
		public const int CONST = 6;
	}


}