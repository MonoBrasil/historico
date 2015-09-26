using System;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;


namespace PHP.Core {


	// visitor interface
	public interface Visitor {
		void Visit(AST ast);
	}


	// build symbol table
	public class SymbolTableVisitor : Visitor {

		public SymbolTableVisitor() { }

		public void Visit(AST ast) {
			// start with emtpy symbol table
			SymbolTable.reset();
			// build symbol table and at the same time check references
			foreach (Statement stmt in ast.stmt_list)
				Visit(stmt);
		}

		protected void Visit(ASTNode node) {
			if (node == null)
				return;
			else if (node is CLASS_DECLARATION) {
				CLASS_DECLARATION cd = (CLASS_DECLARATION)node;
				// process statements of class
				SymbolTable.getInstance().cur_scope = cd.scope;
				foreach (Statement stmt in cd.stmt_list)
					Visit(stmt);
			}
			else if (node is CLASS_VARIABLE_DECLARATION) {
				CLASS_VARIABLE_DECLARATION cvd = (CLASS_VARIABLE_DECLARATION)node;
				// process all values assigned
				foreach (Expression expr in cvd.values)
					Visit(expr);
			}
			else if (node is FUNCTION_DECLARATION) {
				FUNCTION_DECLARATION fd = (FUNCTION_DECLARATION)node;
				// process parameters and statements of function
				SymbolTable.getInstance().cur_scope = fd.scope;
				foreach (PARAMETER_DECLARATION pd in fd.parameters)
					Visit(pd);
				foreach (Statement stmt in fd.stmt_list)
					Visit(stmt);
			}
			else if (node is PARAMETER_DECLARATION) {
				PARAMETER_DECLARATION pd = (PARAMETER_DECLARATION)node;
				// process default value
				Visit(pd.default_value);
			}
			else if (node is IF) {
				// process if statement
				IF i = (IF)node;
				Visit(i.expr);
				Visit(i.stmt);
				// process else if statements
				foreach (ELSEIF e in i.elseif_list)
					Visit(e);
				// process else statement
				Visit(i.else_stmt);
			}
			else if (node is ELSEIF) {
				// process if statement
				ELSEIF e = (ELSEIF)node;
				Visit(e.expr);
				Visit(e.stmt);
			}
			else if (node is WHILE) {
				WHILE w = (WHILE)node;
				// process while expression and statement
				Visit(w.expr);
				Visit(w.stmt);
			}
			else if (node is DO) {
				DO d = (DO)node;
				// process do expression and statement
				Visit(d.stmt);
				Visit(d.expr);
			}
			else if (node is FOR) {
				FOR f = (FOR)node;
				// process for expressions and statement
				foreach (Expression e in f.expr_list1)
					Visit(e);
				foreach (Expression e2 in f.expr_list2)
					Visit(e2);
				Visit(f.stmt);
				foreach (Expression e3 in f.expr_list3)
					Visit(e3);
			}
			else if (node is FOREACH) {
				FOREACH f = (FOREACH)node;
				// process foreach expressions and statement
				if (f.key != null && f.key is FUNCTION_CALL)
					Report.Warn(406, ((FUNCTION_CALL)f.key).function_name, f.key.line, f.key.column);
				if (f.value is FUNCTION_CALL)
					Report.Warn(406, ((FUNCTION_CALL)f.value).function_name, f.value.column, f.value.line);
				Visit(f.stmt);
			}
			else if (node is BLOCK) {
				BLOCK b = (BLOCK)node;
				// process statements of block
				foreach (Statement stmt in b.stmt_list)
					Visit(stmt);
			}
			else if (node is StatementList) {
				StatementList s = (StatementList)node;
				// process statements of block
				foreach (Statement stmt in s)
					Visit(stmt);
			}
			else if (node is ECHO) {
				ECHO e = (ECHO)node;
				// process echo expressions
				foreach (Expression e2 in e.expr_list)
					Visit(e2);
			}
			else if (node is BREAK) {
				BREAK b = (BREAK)node;
				// process expression
				Visit(b.expr);
			}
			else if (node is CONTINUE) {
				CONTINUE c = (CONTINUE)node;
				// process expression
				Visit(c.expr);
			}
			else if (node is RETURN) {
				RETURN r = (RETURN)node;
				// process expression
				Visit(r.expr);
			}
			else if (node is EXPRESSION_AS_STATEMENT) {
				EXPRESSION_AS_STATEMENT eas = (EXPRESSION_AS_STATEMENT)node;
				// process expression
				Visit(eas.expr);
			}
			else if (node is CLONE) {
				CLONE c = (CLONE)node;
				// process expression
				Visit(c.expr);
			}
			else if (node is PAAMAYIM_NEKUDOTAYIM) {
				PAAMAYIM_NEKUDOTAYIM pn = (PAAMAYIM_NEKUDOTAYIM)node;
				// process expression
				Visit(pn.expr);
			}
			else if (node is OBJECT_OPERATOR) {
				OBJECT_OPERATOR oo = (OBJECT_OPERATOR)node;
				// process left part
				Visit(oo.expr1);
				// process right part
				Visit(oo.expr2);
			}
			else if (node is EQUALS) {
				EQUALS e = (EQUALS)node;
				// process expressions
				Visit(e.expr1);
				Visit(e.expr2);
			}
			else if (node is VARIABLE) {
				VARIABLE var = (VARIABLE)node;
				// process offset, if available
				if (var.offset != null)
					Visit(var.offset);
			}
			else if (node is OFFSET) {
				OFFSET o = (OFFSET)node;
				// process offset expression
				Visit(o.value);
			}
			else if (node is FUNCTION_CALL) {
				FUNCTION_CALL fc = (FUNCTION_CALL)node;
				// process parameters
				foreach (Expression expr in fc.parameters)
					Visit(expr);
			}
			else if (node is INSTANCEOF) {
				INSTANCEOF i = (INSTANCEOF)node;
				// process expression
				Visit(i.expr);
			}
			else if (node is ARRAY) {
				ARRAY a = (ARRAY)node;
				// process array pairs
				foreach (ARRAY_PAIR ap in a.array_pair_list) {
					Visit(ap.key);
					Visit(ap.value);
				}
			}
			else if (node is UnaryExpression) {
				UnaryExpression ue = (UnaryExpression)node;
				// process expression
				Visit(ue.expr);
			}
			else if (node is BinaryExpression) {
				BinaryExpression be = (BinaryExpression)node;
				// process expressions
				Visit(be.expr1);
				Visit(be.expr2);
			}
			else if (node is TernaryExpression) {
				TernaryExpression te = (TernaryExpression)node;
				// process expressions
				Visit(te.expr1);
				Visit(te.expr2);
				Visit(te.expr3);
			}
			else if (node is Expression) {
				Expression e = (Expression)node;
				if (e is VARIABLE)
					Visit((VARIABLE)e);
				else if (e is FUNCTION_CALL)
					Visit((FUNCTION_CALL)e);
				else if (e is ARRAY)
					Visit((ARRAY)e);
				else if (e is UnaryExpression)
					Visit((UnaryExpression)e);
				else if (e is BinaryExpression)
					Visit((BinaryExpression)e);
				else if (e is TernaryExpression)
					Visit((TernaryExpression)e);
			}
		}
	}


	// collects all delcarations which may be called before declared
	// (this is class, function and class variable declarations)
	public class DeclarationsVisitor : Visitor {

		public Assembly mPHPRuntime;
		public Type PHPCoreLang;
		public Type PHPCoreConvert;
		public Type PHPCoreRuntime;
		public Type PHPMixed;
		public Type PHPBoolean;
		public Type PHPInteger;
		public Type PHPDouble;
		public Type PHPString;
		public Type PHPArray;
		public Type PHPObject;
		public Type PHPNull;

		public CLASS_DECLARATION cd;
		public FUNCTION_DECLARATION fd;
		public Hashtable typesAlreadyProcessed;
		public MethodInfo mainMethod;

		public DeclarationsVisitor() {
			mPHPRuntime = Assembly.LoadFrom("mPHPRuntime.dll");
			PHPCoreLang = mPHPRuntime.GetType("PHP.Core.Lang");
			PHPCoreConvert = mPHPRuntime.GetType("PHP.Core.Convert");
			PHPCoreRuntime = mPHPRuntime.GetType("PHP.Core.Runtime");
			PHPMixed = mPHPRuntime.GetType("PHP.Mixed");
			PHPBoolean = mPHPRuntime.GetType("PHP.Boolean");
			PHPInteger = mPHPRuntime.GetType("PHP.Integer");
			PHPDouble = mPHPRuntime.GetType("PHP.Double");
			PHPString = mPHPRuntime.GetType("PHP.String");
			PHPArray = mPHPRuntime.GetType("PHP.Array");
			PHPObject = mPHPRuntime.GetType("PHP.Object");
			PHPNull = mPHPRuntime.GetType("PHP.Null");

			typesAlreadyProcessed = new Hashtable();
		}

		public void Visit(AST ast) {
			// create module
			PEmitter.BeginModule();
			// process each statement of php script recursively
			foreach (Statement stmt in ast.stmt_list)
				Visit(stmt);
			// set entry point at __MAIN.__MAIN()
			if (PEmitter.target == PEmitter.EXE)
				PEmitter.asmBld.SetEntryPoint(mainMethod, PEFileKinds.ConsoleApplication);

		}

		protected void Visit(ASTNode node) {
			if (node == null)
				return;
			else if (node is CLASS_DECLARATION) {
				cd = (CLASS_DECLARATION)node;
				// check if class extends a final class
				if (cd.extends != null) {
					SymbolTableEntry parentCdEntry = SymbolTable.getInstance().lookupGlobal(cd.extends.ToLower(), SymbolTable.CLASS);
					CLASS_DECLARATION parentCd = (CLASS_DECLARATION)parentCdEntry.node;
					if (parentCd.modifier == Modifiers.FINAL)
						Report.Error(101, parentCd.name, cd.line, cd.column);
				}
				// create type builder
				TypeAttributes modifier = TypeAttributes.Class;
				if (cd.modifier == PHP.Core.Modifiers.PUBLIC)
					modifier |= TypeAttributes.Public;
				//else if (cd.modifier == PHP.Core.Modifiers.ABSTRACT)
				//	modifier |= TypeAttributes.Abstract;
				else if (cd.modifier == PHP.Core.Modifiers.FINAL)
					modifier |= TypeAttributes.Sealed;
				Type parent;
				if (cd.extends == null || typesAlreadyProcessed[cd.extends] == null) {
					if (cd.name == "__MAIN")
						parent = typeof(object);
					else
						parent = PHPObject;
				}
				else
					parent = (Type)typesAlreadyProcessed[cd.extends];
				cd.typBld = PEmitter.modBld.DefineType(cd.name, modifier, parent);
				typesAlreadyProcessed.Add(cd.name, cd.typBld);
				// insert class symbol (which is case insensitive)
				SymbolTable.getInstance().insertGlobal(cd.name.ToLower(), SymbolTable.CLASS, cd);
				// process each statement within class
				SymbolTable.getInstance().openScope();
				cd.scope = SymbolTable.getInstance().cur_scope;
				foreach (Statement stmt in cd.stmt_list)
					Visit(stmt);
				SymbolTable.getInstance().closeScope();
				cd = null;
			}
			else if (node is CLASS_VARIABLE_DECLARATION) {
				CLASS_VARIABLE_DECLARATION cvd = (CLASS_VARIABLE_DECLARATION)node;
				// create field builders
				FieldAttributes modifiers = 0;
				if (cvd.modifiers.Count == 0)
					modifiers = FieldAttributes.Public | FieldAttributes.Static;
				else if (cvd.modifiers.Contains(Modifiers.CONST))
					modifiers = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.InitOnly;
				else if (!cvd.modifiers.Contains(PHP.Core.Modifiers.PUBLIC) && !cvd.modifiers.Contains(PHP.Core.Modifiers.PROTECTED) && !cvd.modifiers.Contains(PHP.Core.Modifiers.PRIVATE))
					modifiers = FieldAttributes.Public;
				else {
					ArrayList tmpModifiers = (ArrayList)cvd.modifiers.Clone();
					if (cvd.modifiers.Contains(PHP.Core.Modifiers.PUBLIC)) {
						modifiers = FieldAttributes.Public;
						tmpModifiers.Remove(PHP.Core.Modifiers.PUBLIC);
					}
					else if (cvd.modifiers.Contains(PHP.Core.Modifiers.PROTECTED)) {
						modifiers = FieldAttributes.Family;
						tmpModifiers.Remove(PHP.Core.Modifiers.PROTECTED);
					}
					else if (cvd.modifiers.Contains(PHP.Core.Modifiers.PRIVATE)) {
						modifiers = FieldAttributes.Private;
						tmpModifiers.Remove(PHP.Core.Modifiers.PRIVATE);
					}
					if (tmpModifiers.Contains(PHP.Core.Modifiers.PUBLIC) || tmpModifiers.Contains(PHP.Core.Modifiers.PROTECTED) || tmpModifiers.Contains(PHP.Core.Modifiers.PRIVATE))
						Report.Error(105, cvd.line, cvd.column);
				}
				foreach (int modifier in cvd.modifiers) {
					switch (modifier) {
						case PHP.Core.Modifiers.STATIC: modifiers |= FieldAttributes.Static; break;
						//case PHP.Core.Modifiers.ABSTRACT: Report.Error(103, "abstract", cvd.line, cvd.column); break;
						case PHP.Core.Modifiers.FINAL: Report.Error(103, "abstract", cvd.line, cvd.column); break;
					}
				}
				cvd.fieldBuilders = new ArrayList();
				foreach (string name in cvd.names) {
					cvd.fieldBuilders.Add(cd.typBld.DefineField(name, PHPMixed, modifiers));
					// insert member symbol
					SymbolTable.getInstance().insertLocal(name, SymbolTable.CLASS_VARIABLE, cvd);
				}
			}
			else if (node is FUNCTION_DECLARATION) {
				fd = (FUNCTION_DECLARATION)node;
				// check if function overrides a final function with the same name
				CLASS_DECLARATION tmpCd = cd;
				while (tmpCd.extends != null) {
					SymbolTableEntry parentCdEntry = SymbolTable.getInstance().lookupGlobal(cd.extends.ToLower(), SymbolTable.CLASS);
					tmpCd = (CLASS_DECLARATION)parentCdEntry.node;
					SymbolTableEntry superFdEntry = tmpCd.scope.lookup(fd.name.ToLower(), SymbolTable.FUNCTION);
					if (superFdEntry != null) {
						FUNCTION_DECLARATION superFd = (FUNCTION_DECLARATION)superFdEntry.node;
						if (superFd.modifiers.Contains(Modifiers.FINAL)) {
							StringBuilder parameters = new StringBuilder();
							foreach (PARAMETER_DECLARATION pd in superFd.parameters) {
								if (pd.type != null) {
									parameters.Append(pd.type);
									parameters.Append(" ");
								}
								parameters.Append(pd.name);
								parameters.Append(", ");
							}
							if (parameters.Length > 0)
								parameters.Remove(parameters.Length - 2, 2);
							Report.Error(102, tmpCd.name + "::" + superFd.name + "(" + parameters.ToString() + ")", fd.line, fd.column);
						}
					}
				}
				// create constructor and method builders
				MethodAttributes modifiers = 0;
				if (fd.modifiers.Count == 0)
					modifiers = MethodAttributes.Public;
				else if (!fd.modifiers.Contains(PHP.Core.Modifiers.PUBLIC) && !fd.modifiers.Contains(PHP.Core.Modifiers.PROTECTED) && !fd.modifiers.Contains(PHP.Core.Modifiers.PRIVATE))
					modifiers = MethodAttributes.Public;
				else {
					ArrayList tmpModifiers = (ArrayList)fd.modifiers.Clone();
					if (fd.modifiers.Contains(PHP.Core.Modifiers.PUBLIC)) {
						modifiers = MethodAttributes.Public;
						tmpModifiers.Remove(PHP.Core.Modifiers.PUBLIC);
					}
					else if (fd.modifiers.Contains(PHP.Core.Modifiers.PROTECTED)) {
						modifiers = MethodAttributes.Family;
						tmpModifiers.Remove(PHP.Core.Modifiers.PROTECTED);
					}
					else if (fd.modifiers.Contains(PHP.Core.Modifiers.PRIVATE)) {
						modifiers = MethodAttributes.Private;
						tmpModifiers.Remove(PHP.Core.Modifiers.PRIVATE);
					}
					if (tmpModifiers.Contains(PHP.Core.Modifiers.PUBLIC) || tmpModifiers.Contains(PHP.Core.Modifiers.PROTECTED) || tmpModifiers.Contains(PHP.Core.Modifiers.PRIVATE))
						Report.Error(105, fd.line, fd.column);
				}
				foreach (int modifier in fd.modifiers) {
					if (fd.name == "__construct")
						switch (modifier) {
							case PHP.Core.Modifiers.STATIC: Report.Error(104, "static", fd.line, fd.column); break;
							//case PHP.Core.Modifiers.ABSTRACT: Report.Error(104, "abstract", fd.line, fd.column); break;
							case PHP.Core.Modifiers.FINAL: modifiers |= MethodAttributes.Final; break;
						}
					else if (fd.name == "__constructStatic")
						modifiers |= MethodAttributes.Static;
					else
						switch (modifier) {
							case PHP.Core.Modifiers.STATIC: modifiers |= MethodAttributes.Static; break;
							//case PHP.Core.Modifiers.ABSTRACT: modifiers |= MethodAttributes.Abstract; break;
							case PHP.Core.Modifiers.FINAL: modifiers |= MethodAttributes.Final; break;
						}
				}
				Type[] parameterTypes = new Type[fd.parameters.Count];
				for (int i = 0; i < parameterTypes.Length; i++)
					parameterTypes[i] = PHPMixed;
				Type returnType;
				if (fd.name == "__MAIN")
					returnType = typeof(void);
				else
					returnType = PHPMixed;
				if (fd.name == "__construct" || fd.name == "__constructStatic")
					fd.ctrBld = cd.typBld.DefineConstructor(modifiers, CallingConventions.Standard, parameterTypes);
				else
					fd.mthBld = cd.typBld.DefineMethod(fd.name, modifiers, returnType, parameterTypes);
				// at beginning of script
				if (fd.name == "__MAIN") {
					mainMethod = fd.mthBld;
					// disable warnings, if desired
					if (!Report.warningsEnabled) {
						mPHPRuntime = Assembly.LoadFrom("mPHPRuntime.dll");
						Type report = mPHPRuntime.GetType("PHP.Core.Report");
						fd.mthBld.GetILGenerator().Emit(OpCodes.Ldc_I4_0);
						fd.mthBld.GetILGenerator().Emit(OpCodes.Stsfld, report.GetField("warningsEnabled"));
					}
					// initialize local settings to en-US
					fd.mthBld.GetILGenerator().Emit(OpCodes.Call, PHPCoreLang.GetMethod("Init", Type.EmptyTypes));
				}
				// insert function symbol (which is case insensitive)
				SymbolTable.getInstance().insertLocal(fd.name.ToLower(), SymbolTable.FUNCTION, fd);
				// remember scope
				SymbolTable.getInstance().openScope();
				fd.scope = SymbolTable.getInstance().cur_scope;
				SymbolTable.getInstance().closeScope();
				fd = null;
			}
		}

	}


	// transform AST to create class __MAIN and function __MAIN()
	public class MainMethodVisitor : Visitor {

		public CLASS_DECLARATION cd__MAIN;
		public FUNCTION_DECLARATION fd__MAIN;
		public StatementList modifiedStmtList;

		public MainMethodVisitor() {
			// create public class __MAIN
			cd__MAIN = new CLASS_DECLARATION(PHP.Core.Modifiers.PUBLIC, "__MAIN", null, new StatementList(), 0, 0);
			// create public function __MAIN()
			ArrayList modifiers = new ArrayList();
			modifiers.Add(PHP.Core.Modifiers.PUBLIC);
			modifiers.Add(PHP.Core.Modifiers.STATIC);
			fd__MAIN = new FUNCTION_DECLARATION(modifiers, false, "__MAIN", new ArrayList(), new StatementList(), 0, 0);
			// add function __MAIN() to class __MAIN
			cd__MAIN.stmt_list.Add(fd__MAIN);
			// create modified statement list
			modifiedStmtList = new StatementList();
		}

		public void Visit(AST ast) {
			// put class __MAIN at beginning of modified statement list
			modifiedStmtList.Add(cd__MAIN);
			// leave top class declarations, move top function declarations to class __MAIN, move everything else to function __MAIN()
			foreach (Statement stmt in ast.stmt_list)
				Visit(stmt);
			// replace AST's old statement list by modified one
			ast.stmt_list = modifiedStmtList;
		}

		protected void Visit(ASTNode node) {
			if (node == null)
				return;
			else if (node is CLASS_DECLARATION)
				modifiedStmtList.Add((Statement)node);
			else if (node is FUNCTION_DECLARATION) {
				FUNCTION_DECLARATION fd = (FUNCTION_DECLARATION)node;
				ArrayList modifiers = new ArrayList();
				modifiers.Add(PHP.Core.Modifiers.PUBLIC);
				modifiers.Add(PHP.Core.Modifiers.STATIC);
				fd.modifiers = modifiers;
				cd__MAIN.stmt_list.Add(fd);
			}
			else
				fd__MAIN.stmt_list.Add((Statement)node);
		}

	}


	// reorder class declarations by inheritance
	public class InheritanceVisitor : Visitor {

		public InheritanceVisitor() { }

		public void Visit(AST ast) {
			StatementList originalClassDeclarations = new StatementList();
			StatementList reorderedClassDeclarations = new StatementList();
			// move class declarations from ast.stmt_list to originalClassDeclarations
			for (int i = 0; i < ast.stmt_list.Count(); i++) {
				Statement stmt = ast.stmt_list.Get(i);
				if (stmt is CLASS_DECLARATION) {
					ast.stmt_list.Remove(stmt);
					i--;
					originalClassDeclarations.Add(stmt);
				}
			}
			// reorder class declarations
			ArrayList processedInPreviousIteration;
			ArrayList toBeProcessedInNextIteration = new ArrayList();
			// start with classes with no parent specified
			for (int i = 0; i < originalClassDeclarations.Count(); i++) {
				CLASS_DECLARATION cd = (CLASS_DECLARATION)originalClassDeclarations.Get(i);
				if (cd.extends == null) {
					originalClassDeclarations.Remove(cd);
					i--;
					reorderedClassDeclarations.Add(cd);
					toBeProcessedInNextIteration.Add(cd.name);
				}
			}
			processedInPreviousIteration = toBeProcessedInNextIteration;
			// in each iteration, add those class delcarations which inherit from class declarations processed in the previous iteration
			while (originalClassDeclarations.Count() > 0) {
				toBeProcessedInNextIteration = new ArrayList();
				for (int i = 0; i < originalClassDeclarations.Count(); i++) {
					CLASS_DECLARATION cd = (CLASS_DECLARATION)originalClassDeclarations.Get(i);
					if (processedInPreviousIteration.Contains(cd.extends)) {
						originalClassDeclarations.Remove(cd);
						i--;
						reorderedClassDeclarations.Add(cd);
						toBeProcessedInNextIteration.Add(cd.name);
					}
				}
				// if there are still classes left, but nothing happened in this iteration, a parent wasn't declared or there is a cycle in inheritance
				if (processedInPreviousIteration.Count == 0)
					Report.Error(100);
				processedInPreviousIteration = toBeProcessedInNextIteration;
			}
			// add reordered class declarations to ast.stmt_list
			ast.stmt_list.AddRange(reorderedClassDeclarations);
		}

	}


	// ensure every method has a return statement and cut unreachable statements after a return
	public class ReturnStatementVisitor : Visitor {

		public ReturnStatementVisitor() { }

		public void Visit(AST ast) {
			// process every statement
			foreach (Statement stmt in ast.stmt_list)
				Visit(stmt);
		}

		protected void Visit(ASTNode node) {
			if (node == null)
				return;
			else if (node is CLASS_DECLARATION) {
				CLASS_DECLARATION cd = (CLASS_DECLARATION)node;
				foreach (Statement stmt in cd.stmt_list)
					Visit(stmt);
			}
			else if (node is FUNCTION_DECLARATION) {
				FUNCTION_DECLARATION fd = (FUNCTION_DECLARATION)node;
				bool returnReached = false;
				StatementList newStmtList = new StatementList();
				// copy each statement to newStmtList until top return is reached
				foreach (Statement stmt in fd.stmt_list) {
					if (stmt is RETURN) {
						newStmtList.Add(stmt);
						returnReached = true;
						break;
					}
					else
						newStmtList.Add(stmt);
				}
				// if there was no return statement, add one
				if (!returnReached)
					newStmtList.Add(new RETURN(null, 0, 0));
				// replace statement list of function with new one
				fd.stmt_list = newStmtList;
			}
		}

	}


	// ensure there is no break/continue without a loop
	public class LoopVisitor : Visitor {

		public int level;

		public LoopVisitor() {
			level = 0;
		}

		public void Visit(AST ast) {
			// process every statement
			foreach (Statement stmt in ast.stmt_list)
				Visit(stmt);
		}

		protected void Visit(ASTNode node) {
			if (node == null)
				return;
			else if (node is CLASS_DECLARATION) {
				CLASS_DECLARATION cd = (CLASS_DECLARATION)node;
				foreach (Statement stmt in cd.stmt_list)
					Visit(stmt);
			}
			else if (node is FUNCTION_DECLARATION) {
				FUNCTION_DECLARATION fd = (FUNCTION_DECLARATION)node;
				foreach (Statement stmt in fd.stmt_list)
					Visit(stmt);
			}
			else if (node is BLOCK) {
				BLOCK b = (BLOCK)node;
				foreach (Statement stmt in b.stmt_list)
					Visit(stmt);
			}
			else if (node is IF) {
				IF i = (IF)node;
				Visit(i.stmt);
			}
			else if (node is ELSEIF) {
				ELSEIF e = (ELSEIF)node;
				Visit(e.stmt);
			}
			else if (node is WHILE) {
				WHILE w = (WHILE)node;
				level++;
				Visit(w.stmt);
				level--;
			}
			else if (node is DO) {
				DO d = (DO)node;
				level++;
				Visit(d.stmt);
				level--;
			}
			else if (node is FOR) {
				FOR f = (FOR)node;
				level++;
				Visit(f.stmt);
				level--;
			}
			else if (node is FOREACH) {
				FOREACH f = (FOREACH)node;
				level++;
				Visit(f.stmt);
				level--;
			}
			else if (node is SWITCH) {
				SWITCH s = (SWITCH)node;
				level++;
				foreach (ASTNode node2 in s.switch_case_list) {
					if (node2 is CASE)
						Visit((CASE)node2);
					else if (node2 is DEFAULT)
						Visit((DEFAULT)node2);
				}
				level--;
			}
			else if (node is CASE) {
				CASE c = (CASE)node;
				Visit(c.stmt);
			}
			else if (node is DEFAULT) {
				DEFAULT d = (DEFAULT)node;
				Visit(d.stmt);
			}
			else if (node is BREAK) {
				if (level == 0)
					Report.Error(501, node.line, node.column);
			}
			else if (node is CONTINUE) {
				if (level == 0)
					Report.Error(501, node.line, node.column);
			}
		}

	}

	// ensure every class (except __MAIN) has a constructor
	public class ConstructorVisitor : Visitor {

		public bool constructorFound;

		public ConstructorVisitor() { }

		public void Visit(AST ast) {
			// process every statement
			foreach (Statement stmt in ast.stmt_list)
				Visit(stmt);
		}

		protected void Visit(ASTNode node) {
			if (node == null)
				return;
			else if (node is CLASS_DECLARATION) {
				CLASS_DECLARATION cd = (CLASS_DECLARATION)node;
				if (cd.name == "__MAIN")
					return;
				constructorFound = false;
				foreach (Statement stmt in cd.stmt_list)
					Visit(stmt);
				// if no user defined constructor is found, add one
				ArrayList modifiers, parameters;
				StatementList stmt_list;
				Statement ctorDecl;
				if (!constructorFound) {
					modifiers = new ArrayList();
					modifiers.Add(PHP.Core.Modifiers.PUBLIC);
					parameters = new ArrayList();
					stmt_list = new StatementList();
					ctorDecl = new FUNCTION_DECLARATION(modifiers, false, "__construct", parameters, stmt_list, 0, 0);
					cd.stmt_list.Add(ctorDecl);
				}
				// in every case, add a static constructor
				modifiers = new ArrayList();
				modifiers.Add(PHP.Core.Modifiers.PUBLIC);
				modifiers.Add(PHP.Core.Modifiers.STATIC);
				parameters = new ArrayList();
				stmt_list = new StatementList();
				ctorDecl = new FUNCTION_DECLARATION(modifiers, false, "__constructStatic", parameters, stmt_list, 0, 0);
				cd.stmt_list.Add(ctorDecl);
			}
			else if (node is FUNCTION_DECLARATION) {
				FUNCTION_DECLARATION fd = (FUNCTION_DECLARATION)node;
				if (fd.name == "__construct")
					constructorFound = true;
			}
		}

	}


	// emit CIL code
	public class EmitterVisitor : Visitor {

		public Assembly mPHPRuntime;
		public Type PHPCoreLang;
		public Type PHPCoreConvert;
		public Type PHPCoreRuntime;
		public Type PHPMixed;
		public Type PHPBoolean;
		public Type PHPInteger;
		public Type PHPDouble;
		public Type PHPString;
		public Type PHPArray;
		public Type PHPObject;
		public Type PHPNull;

		public CLASS_DECLARATION cd;
		public FUNCTION_DECLARATION fd;
		public ILGenerator ilGen;

		public bool objectOperatorInProgress; // used for OBJECT_OPERATOR
		public bool processingObjectThis; // used for OBJECT_OPERATOR
		public Label enterLoop, exitLoop; // used for FOR, FOREACH, WHILE, DO and SWITCH

		public EmitterVisitor() {
			mPHPRuntime = Assembly.LoadFrom("mPHPRuntime.dll");
			PHPCoreLang = mPHPRuntime.GetType("PHP.Core.Lang");
			PHPCoreConvert = mPHPRuntime.GetType("PHP.Core.Convert");
			PHPCoreRuntime = mPHPRuntime.GetType("PHP.Core.Runtime");
			PHPMixed = mPHPRuntime.GetType("PHP.Mixed");
			PHPBoolean = mPHPRuntime.GetType("PHP.Boolean");
			PHPInteger = mPHPRuntime.GetType("PHP.Integer");
			PHPDouble = mPHPRuntime.GetType("PHP.Double");
			PHPString = mPHPRuntime.GetType("PHP.String");
			PHPArray = mPHPRuntime.GetType("PHP.Array");
			PHPObject = mPHPRuntime.GetType("PHP.Object");
			PHPNull = mPHPRuntime.GetType("PHP.Null");
			objectOperatorInProgress = false;
			processingObjectThis = false;
		}

		public void Visit(AST ast) {
			// process each statement of php script recursively
			foreach (Statement stmt in ast.stmt_list)
				Visit(stmt);

			// save module
			PEmitter.EndModule();
		}

		protected void Visit(ASTNode node) {
			if (node == null)
				return;
			else if (node is CLASS_DECLARATION) {
				cd = (CLASS_DECLARATION)node;
				// process each statement of class
				foreach (Statement stmt in cd.stmt_list)
					Visit(stmt);
				// bake the class
				cd.typ = cd.typBld.CreateType();
				cd = null;
			}
			else if (node is FUNCTION_DECLARATION) {
				fd = (FUNCTION_DECLARATION)node;
				// get ILGenerator and check if this is a static function or not
				bool staticFunction;
				if (fd.name == "__construct" || fd.name == "__constructStatic") {
					ilGen = fd.ctrBld.GetILGenerator();
					staticFunction = fd.ctrBld.IsStatic;
				}
				else {
					ilGen = fd.mthBld.GetILGenerator();
					staticFunction = fd.mthBld.IsStatic;
				}
				// in constructors, store initial class member values
				if (fd.name == "__construct" || fd.name == "__constructStatic") {
					CLASS_DECLARATION tmpCd = cd;
					ArrayList namesAlreadyAdded = new ArrayList();
					do { // fetch class variable declarations of current class
						ArrayList classVarDecls = tmpCd.scope.lookup(SymbolTable.CLASS_VARIABLE);
						foreach (SymbolTableEntry cvdEntry in classVarDecls) {
							CLASS_VARIABLE_DECLARATION cvd = (CLASS_VARIABLE_DECLARATION)cvdEntry.node;
							for (int i = 0; i < cvd.names.Count; i++) {
								FieldBuilder fb = (FieldBuilder)cvd.fieldBuilders[i];
								string name = (string)cvd.names[i];
								Expression value = cvd.values.Get(i);
								// only process names not already added
								if (namesAlreadyAdded.Contains(name))
									continue;
								bool cvdIsVisible = tmpCd == cd || !cvd.modifiers.Contains(Modifiers.PRIVATE);
								bool cvdIsStatic = cvd.modifiers.Contains(Modifiers.STATIC) || cvd.modifiers.Contains(Modifiers.CONST);
								// in a static constructor, store initial static class member values
								if (fd.name == "__constructStatic" && cvdIsVisible && cvdIsStatic) {
									if (value == null)
										ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
									else
										Visit(value);
									ilGen.Emit(OpCodes.Stsfld, fb);
									namesAlreadyAdded.Add(name);
								}
								// in a local constructor, store initial local class member values
								if (fd.name == "__construct" && cvdIsVisible && !cvdIsStatic) {
									ilGen.Emit(OpCodes.Ldarg_0);
									if (value == null)
										ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
									else
										Visit(value);
									ilGen.Emit(OpCodes.Stfld, fb);
									namesAlreadyAdded.Add(name);
								}
							}
						}
						// fetch parent class declaration
						if (tmpCd.extends == null)
							tmpCd = null;
						else {
							SymbolTableEntry parentCdEntry = SymbolTable.getInstance().lookupGlobal(tmpCd.extends.ToLower(), SymbolTable.CLASS);
							tmpCd = (CLASS_DECLARATION)parentCdEntry.node;
						}
					} while (tmpCd != null);

				}
				// store parameters passed as local variable
				PARAMETER_DECLARATION pd;
				for (int i = 0; i < fd.parameters.Count; i++) {
					pd = (PARAMETER_DECLARATION)fd.parameters[i];
					// push value passed
					ilGen.Emit(OpCodes.Ldarg, i + (staticFunction ? 0 : 1));
					// if value passed is Null, use default value, if available
					if (pd.default_value != null) {
						Label skip = ilGen.DefineLabel();
						ilGen.Emit(OpCodes.Dup);
						ilGen.Emit(OpCodes.Isinst, PHPNull);
						ilGen.Emit(OpCodes.Brfalse, skip);
						ilGen.Emit(OpCodes.Pop);
						Visit(pd.default_value);
						ilGen.MarkLabel(skip);
					}
					// check if class type hint is ok
					if (pd.type != null) {
						ilGen.Emit(OpCodes.Dup);
						ilGen.Emit(OpCodes.Ldstr, pd.type);
						ilGen.Emit(OpCodes.Ldc_I4_0);
						ilGen.Emit(OpCodes.Ldc_I4_1);
						ilGen.Emit(OpCodes.Call, typeof(Type).GetMethod("GetType", new Type[] { typeof(string), typeof(bool), typeof(bool) }));
						ilGen.Emit(OpCodes.Ldc_I4, i + (staticFunction ? 1 : 2));
						ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("CheckTypeHint", new Type[] { PHPMixed, typeof(Type), typeof(int) }));
					}
					// store
					StoreToVariable(pd.name);
				}
				// process statements
				if (fd.stmt_list.Count() == 0)
					ilGen.Emit(OpCodes.Nop);
				else
					foreach (Statement stmt in fd.stmt_list)
						Visit(stmt);
				ilGen = null;
				fd = null;
			}
			else if (node is GLOBAL) {
				GLOBAL g = (GLOBAL)node;
				foreach (VARIABLE var in g.var_list)
					fd.scope.globalVariables.Add(var.name);
			}
			else if (node is STATIC_DECLARATION) {
				STATIC_DECLARATION sd = (STATIC_DECLARATION)node;
				foreach (Expression expr in sd.expr_list) {
					if (expr is VARIABLE) {
						ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
						StoreToStaticVariable(((VARIABLE)expr).name);
					}
					if (expr is EQUALS) {
						Visit(((EQUALS)expr).expr2);
						StoreToStaticVariable(((VARIABLE)((EQUALS)expr).expr1).name);
					}
				}
			}
			else if (node is ECHO) {
				ECHO e = (ECHO)node;
				foreach (Expression e2 in e.expr_list) {
					Visit(e2);
					ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Echo", new Type[] { PHPMixed }));
				}
			}
			else if (node is BLOCK) {
				BLOCK b = (BLOCK)node;
				foreach (Statement stmt in b.stmt_list)
					Visit(stmt);
			}
			else if (node is StatementList) {
				StatementList s = (StatementList)node;
				// process statements of block
				foreach (Statement stmt in s)
					Visit(stmt);
			}
			else if (node is IF) {
				IF i = (IF)node;
				Label nextCheck = ilGen.DefineLabel();
				Label endIf = ilGen.DefineLabel();
				// process if statement
				Visit(i.expr);
				ilGen.Emit(OpCodes.Call, PHPCoreConvert.GetMethod("ToValueBool", new Type[] { PHPMixed }));
				ilGen.Emit(OpCodes.Brfalse, nextCheck);
				Visit(i.stmt);
				ilGen.Emit(OpCodes.Br, endIf);
				ilGen.MarkLabel(nextCheck);
				// process elseif statements
				foreach (ELSEIF e in i.elseif_list) {
					//Visit(e);
					ELSEIF ei = (ELSEIF)node;
					Label nextCheck2 = ilGen.DefineLabel();
					// process else statement
					Visit(ei.expr);
					ilGen.Emit(OpCodes.Call, PHPCoreConvert.GetMethod("ToValueBool", new Type[] { PHPMixed }));
					ilGen.Emit(OpCodes.Brfalse, nextCheck2);
					Visit(ei.stmt);
					ilGen.Emit(OpCodes.Br, endIf);
					ilGen.MarkLabel(nextCheck2);
				}
				// process else statement
				Visit(i.else_stmt);
				// done
				ilGen.MarkLabel(endIf);
				endIf = new Label();
			}
			else if (node is WHILE) {
				WHILE w = (WHILE)node;
				enterLoop = ilGen.DefineLabel();
				exitLoop = ilGen.DefineLabel();
				ilGen.MarkLabel(enterLoop);
				Visit(w.expr);
				ilGen.Emit(OpCodes.Call, PHPCoreConvert.GetMethod("ToValueBool", new Type[] { PHPMixed }));
				ilGen.Emit(OpCodes.Brfalse, exitLoop);
				Visit(w.stmt);
				ilGen.Emit(OpCodes.Br, enterLoop);
				ilGen.MarkLabel(exitLoop);
			}
			else if (node is DO) {
				DO d = (DO)node;
				enterLoop = ilGen.DefineLabel();
				exitLoop = ilGen.DefineLabel();
				ilGen.MarkLabel(enterLoop);
				Visit(d.stmt);
				Visit(d.expr);
				ilGen.Emit(OpCodes.Call, PHPCoreConvert.GetMethod("ToValueBool", new Type[] { PHPMixed }));
				ilGen.Emit(OpCodes.Brfalse, exitLoop);
				ilGen.Emit(OpCodes.Br, enterLoop);
				ilGen.MarkLabel(exitLoop);
			}
			else if (node is FOR) {
				FOR f = (FOR)node;
				enterLoop = ilGen.DefineLabel();
				exitLoop = ilGen.DefineLabel();
				foreach (Expression expr in f.expr_list1) {
					Visit(expr);
					// pop to treat for expression(s) 1 as statement(s)
					ilGen.Emit(OpCodes.Pop);
				}
				ilGen.MarkLabel(enterLoop);
				IEnumerator ienum = f.expr_list2.GetEnumerator();
				bool hasMore = ienum.MoveNext();
				while (hasMore) {
					Visit((Expression)ienum.Current);
					hasMore = ienum.MoveNext();
					if (hasMore)
						// if more than one expression 2, pop all except last one to only decide on last one for jumping
						ilGen.Emit(OpCodes.Pop);
				}
				// if for expression 2 is empty, the loop should run indefinitely
				if (f.expr_list2.Count() > 0) {
					ilGen.Emit(OpCodes.Call, PHPCoreConvert.GetMethod("ToValueBool", new Type[] { PHPMixed }));
					ilGen.Emit(OpCodes.Brfalse, exitLoop);
				}
				Visit(f.stmt);
				foreach (Expression expr in f.expr_list3) {
					Visit(expr);
					// pop to treat for expression(s) 3 as statement(s)
					ilGen.Emit(OpCodes.Pop);
				}
				ilGen.Emit(OpCodes.Br, enterLoop);
				ilGen.MarkLabel(exitLoop);
			}
			else if (node is FOREACH) {
				FOREACH f = (FOREACH)node;
				if (f.key != null && f.key is FUNCTION_CALL)
					Report.Error(408, f.key.line, f.key.column);
				if (f.value is FUNCTION_CALL)
					Report.Error(408, f.value.line, f.value.column);
				enterLoop = ilGen.DefineLabel();
				exitLoop = ilGen.DefineLabel();
				Label warn = ilGen.DefineLabel();
				// push array reference
				Visit(f.array);
				// if type is not array, jump to end
				ilGen.Emit(OpCodes.Dup);
				ilGen.Emit(OpCodes.Isinst, PHPArray);
				ilGen.Emit(OpCodes.Brfalse, warn);
				// if array is not referenced, clone to acheive value semantics
				if (!(f.value is REFERENCE))
					ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Clone", new Type[] { PHPMixed }));
				// declare key and value variable
				string valueName;
				if (f.value is VARIABLE)
					valueName = ((VARIABLE)f.value).name;
				else
					valueName = ((VARIABLE)((REFERENCE)f.value).variable).name;
				if (f.key != null) {
					ilGen.Emit(OpCodes.Ldnull);
					StoreToVariable(((VARIABLE)f.key).name);
				}
				ilGen.Emit(OpCodes.Ldnull);
				StoreToVariable(valueName);
				// reset public array pointer
				ilGen.Emit(OpCodes.Dup);
				ilGen.Emit(OpCodes.Callvirt, PHPArray.GetMethod("Reset", Type.EmptyTypes));
				ilGen.Emit(OpCodes.Pop);
				// enter loop
				ilGen.MarkLabel(enterLoop);
				// check if public array pointer is inside the array
				ilGen.Emit(OpCodes.Dup);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("CurrentIsValid", new Type[] { PHPArray }));
				// if not, exit loop
				ilGen.Emit(OpCodes.Brfalse, exitLoop);
				// if key variable declared, push current key of array and store to key variable
				if (f.key != null) {
					ilGen.Emit(OpCodes.Dup);
					ilGen.Emit(OpCodes.Callvirt, PHPArray.GetMethod("Key", Type.EmptyTypes));
					StoreToVariable(((VARIABLE)f.key).name);
				}
				// push current value of array and store to value variable
				ilGen.Emit(OpCodes.Dup);
				ilGen.Emit(OpCodes.Callvirt, PHPArray.GetMethod("Current", Type.EmptyTypes));
				StoreToVariable(valueName);
				// process foreach statement
				Visit(f.stmt);
				// load value variable and store as current value of array
				ilGen.Emit(OpCodes.Dup);
				ilGen.Emit(OpCodes.Dup);
				ilGen.Emit(OpCodes.Callvirt, PHPArray.GetMethod("Key", Type.EmptyTypes));
				LoadFromVariable(valueName);
				ilGen.Emit(OpCodes.Callvirt, PHPArray.GetMethod("Append", new Type[] { PHPMixed, PHPMixed }));
				// advance public array pointer
				ilGen.Emit(OpCodes.Dup);
				ilGen.Emit(OpCodes.Callvirt, PHPArray.GetMethod("Next", Type.EmptyTypes));
				ilGen.Emit(OpCodes.Pop);
				// and reenter loop
				ilGen.Emit(OpCodes.Br, enterLoop);
				// report invalid argument
				ilGen.MarkLabel(warn);
				ilGen.Emit(OpCodes.Ldc_I4, 407);
				ilGen.Emit(OpCodes.Ldc_I4, f.line);
				ilGen.Emit(OpCodes.Ldc_I4, f.column);
				ilGen.Emit(OpCodes.Call, mPHPRuntime.GetType("PHP.Core.Report").GetMethod("Warn", new Type[] { typeof(int), typeof(int), typeof(int) }));
				// pop array reference
				ilGen.MarkLabel(exitLoop);
				ilGen.Emit(OpCodes.Pop);
			}
			else if (node is SWITCH) {
				SWITCH s = (SWITCH)node;
				enterLoop = ilGen.DefineLabel();
				exitLoop = ilGen.DefineLabel();
				// push switch expression and enter loop
				Visit(s.expr);
				ilGen.MarkLabel(enterLoop);
				// process case and default statements
				foreach (ASTNode node2 in s.switch_case_list) {
					if (node2 is CASE)
						Visit((CASE)node2);
					else if (node2 is DEFAULT)
						Visit((DEFAULT)node2);
				}
				// done
				ilGen.MarkLabel(exitLoop);
				ilGen.Emit(OpCodes.Pop);
				ilGen.Emit(OpCodes.Ldc_I4_0);
				ilGen.Emit(OpCodes.Stsfld, PHPCoreRuntime.GetField("switchInProgress"));
			}
			else if (node is CASE) {
				CASE c = (CASE)node;
				Label processCase = ilGen.DefineLabel();
				Label nextCase = ilGen.DefineLabel();
				// if statements already processing, process this one as well
				ilGen.Emit(OpCodes.Ldsfld, PHPCoreRuntime.GetField("switchInProgress"));
				ilGen.Emit(OpCodes.Brtrue, processCase);
				// else if case expression doesn't equal switch expression, jump to next case
				ilGen.Emit(OpCodes.Dup);
				Visit(c.expr);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("IsEqual", new Type[] { PHPMixed, PHPMixed }));
				ilGen.Emit(OpCodes.Call, PHPCoreConvert.GetMethod("ToValueBool", new Type[] { PHPMixed }));
				ilGen.Emit(OpCodes.Brfalse, nextCase);
				// else start processing switch statements
				ilGen.Emit(OpCodes.Ldc_I4_1);
				ilGen.Emit(OpCodes.Stsfld, PHPCoreRuntime.GetField("switchInProgress"));
				// process case statement
				ilGen.MarkLabel(processCase);
				Visit(c.stmt);
				// mark start of next case
				ilGen.MarkLabel(nextCase);
			}
			else if (node is DEFAULT) {
				DEFAULT d = (DEFAULT)node;
				// process default statement
				ilGen.Emit(OpCodes.Ldc_I4_1);
				ilGen.Emit(OpCodes.Stsfld, PHPCoreRuntime.GetField("switchInProgress"));
				Visit(d.stmt);
			}
			else if (node is BREAK) {
				BREAK b = (BREAK)node;
				ilGen.Emit(OpCodes.Br, exitLoop);
			}
			else if (node is CONTINUE) {
				CONTINUE c = (CONTINUE)node;
				ilGen.Emit(OpCodes.Br, enterLoop);
			}
			else if (node is RETURN) {
				RETURN r = (RETURN)node;
				if (fd.name != "__MAIN" && fd.name != "__construct" && fd.name != "__constructStatic") {
					// if no return value specified, use Null as return value; otherwise use return value specified
					if (r.expr == null)
						ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
					else
						Visit(r.expr);
				}
				ilGen.Emit(OpCodes.Ret);
			}
			else if (node is UNSET) {
				UNSET u = (UNSET)node;
				// unset variables
				for (int i = 0; i < u.var_list.Count(); i++) {
					if (u.var_list.Get(i) is FUNCTION_CALL)
						Report.Error(408, u.var_list.Get(i).line, u.var_list.Get(i).column);
					VARIABLE var = (VARIABLE)u.var_list.Get(i);
					// regular variable, so unset
					if (var.offset == null) {
						ilGen.Emit(OpCodes.Ldstr, var.name);
						ilGen.Emit(OpCodes.Newobj, PHPString.GetConstructor(new Type[] { typeof(string) }));
						ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("UnsetVariable", new Type[] { PHPMixed }));
					}
					// array item, so remove from array
					else if (var.offset.kind == OFFSET.SQUARE) {
						LoadFromVariable(var.name);
						// convert to Array (in case the variable was unset)
						ilGen.Emit(OpCodes.Call, PHPCoreConvert.GetMethod("ToArray", new Type[] { PHPMixed }));
						if (var.offset.value == null)
							ilGen.Emit(OpCodes.Ldnull);
						else
							Visit(var.offset);
						ilGen.Emit(OpCodes.Callvirt, PHPArray.GetMethod("Remove", new Type[] { PHPMixed }));
					}
				}
			}
			else if (node is EXPRESSION_AS_STATEMENT) {
				EXPRESSION_AS_STATEMENT eas = (EXPRESSION_AS_STATEMENT)node;
				Visit(eas.expr);
				ilGen.Emit(OpCodes.Pop);
			}
			else if (node is VARIABLE) {
				VARIABLE var = (VARIABLE)node;
				// get desired variable value
				LoadFromVariable(var.name);
				// process offset, if available
				if (var.offset != null) {
					// this is an array
					if (var.offset.kind == OFFSET.SQUARE) {
						if (var.offset.value == null)
							ilGen.Emit(OpCodes.Ldnull);
						else
							Visit(var.offset);
						ilGen.Emit(OpCodes.Ldc_I4, OFFSET.SQUARE);
						ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Offset", new Type[] { PHPMixed, PHPMixed, typeof(int) }));
					}
				}
			}
			else if (node is REFERENCE) {
				REFERENCE r = (REFERENCE)node;
				// process enclosed variable or function call
				if (r.variable is VARIABLE)
					Visit((VARIABLE)r.variable);
				else if (r.variable is FUNCTION_CALL)
					Visit((FUNCTION_CALL)r.variable);
			}
			else if (node is OFFSET) {
				OFFSET o = (OFFSET)node;
				// process offset expression
				Visit(o.value);
			}
			else if (node is FUNCTION_CALL) {
				FUNCTION_CALL fc = (FUNCTION_CALL)node;
				// catch predefined functions
				if (fc.function_name.ToLower() == "define") {
					PushParameters(fc.parameters, 3);
					ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("DefineConstant", new Type[] { PHPMixed, PHPMixed, PHPMixed }));
				}
				// otherwise call user defined function
				else {
					// look in global scope as calls to functions in user defined classed are handled by object operator or paamayim nekudotayim
					SymbolTable st = SymbolTable.getInstance();
					SymbolTableEntry entry = cd.scope.lookup(fc.function_name.ToLower(), SymbolTable.FUNCTION);
					if (entry == null)
						Report.Error(212, fc.function_name, fc.line, fc.column);
					FUNCTION_DECLARATION fd = (FUNCTION_DECLARATION)entry.node;
					// pass parameters (only as many as needed)
					int parametersPassedActually = (int)Math.Min(fd.parameters.Count, fc.parameters.Count());
					for (int i = 0; i < parametersPassedActually; i++) {
						PARAMETER_DECLARATION pd = (PARAMETER_DECLARATION)fd.parameters[i];
						Expression expr = (Expression)fc.parameters.Get(i);
						Visit(expr);
						// clone if value semantics is desired
						if (!pd.by_reference && !(expr is REFERENCE))
							ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Clone", new Type[] { PHPMixed }));
					}
					// if less parameters actually passed then necessary, pass Null objects instead
					for (int i = parametersPassedActually; i < fd.parameters.Count; i++) {
						PARAMETER_DECLARATION pd = (PARAMETER_DECLARATION)fd.parameters[i];
						if (pd.default_value == null)
							Report.Warn(300, System.Convert.ToString(i + 1), fc.line, fc.column);
						ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
					}
					// ensure parameters passed by reference are variables
					for (int i = 0; i < parametersPassedActually; i++) {
						PARAMETER_DECLARATION pd = (PARAMETER_DECLARATION)fd.parameters[i];
						Expression variableSupplied = (Expression)fc.parameters.Get(i);
						if (pd.by_reference || variableSupplied is REFERENCE)
							if (!(variableSupplied is VARIABLE || variableSupplied is REFERENCE))
								Report.Error(301, System.Convert.ToString(i + 1), variableSupplied.line, variableSupplied.column);
					}
					// add function call to call trace
					ilGen.Emit(OpCodes.Ldstr, cd.name + "->" + fc.function_name);
					ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("AddFunctionCallToTrace", new Type[] { typeof(string) }));
					// if an object operator in progress, call instance function
					if (objectOperatorInProgress)
						ilGen.Emit(OpCodes.Call, fd.mthBld);
					// else call static function
					else
						ilGen.Emit(OpCodes.Call, fd.mthBld);
					// remove function call from call trace
					ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("RemoveFunctionCallFromTrace", Type.EmptyTypes));
				}
			}
			else if (node is NEW) {
				NEW n = (NEW)node;
				SymbolTableEntry cdEntry = SymbolTable.getInstance().lookupGlobal(n.type.ToLower(), SymbolTable.CLASS);
				if (cdEntry == null)
					Report.Error(203, n.type, n.line, n.column);
				CLASS_DECLARATION cd = (CLASS_DECLARATION)cdEntry.node;
				SymbolTableEntry ctorEntry = cd.scope.lookup("__construct", SymbolTable.FUNCTION);
				FUNCTION_DECLARATION ctorDecl = (FUNCTION_DECLARATION)ctorEntry.node;
				// pass parameters (only as many as needed)
				int parametersPassedActually = (int)Math.Min(ctorDecl.parameters.Count, n.ctor_args.Count());
				for (int i = 0; i < parametersPassedActually; i++) {
					Expression expr = (Expression)n.ctor_args.Get(i);
					Visit(expr);
				}
				// if less parameters actually passed then necessary, pass Null objects instead
				for (int i = parametersPassedActually; i < ctorDecl.parameters.Count; i++) {
					PARAMETER_DECLARATION pd = (PARAMETER_DECLARATION)ctorDecl.parameters[i];
					if (pd.default_value == null)
						Report.Warn(300, System.Convert.ToString(i + 1), n.line, n.column);
					ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
				}
				// ensure parameters passed by reference are vairables
				for (int i = 0; i < parametersPassedActually; i++) {
					PARAMETER_DECLARATION pd = (PARAMETER_DECLARATION)ctorDecl.parameters[i];
					Expression variableSupplied = (Expression)n.ctor_args.Get(i);
					if (pd.by_reference || variableSupplied is REFERENCE)
						if (!(variableSupplied is VARIABLE || variableSupplied is REFERENCE))
							Report.Error(301, System.Convert.ToString(i + 1), variableSupplied.line, variableSupplied.column);
				}
				// add constructor call to call trace
				ilGen.Emit(OpCodes.Ldstr, n.type + "->__construct");
				ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("AddFunctionCallToTrace", new Type[] { typeof(string) }));
				// call constructor
				ilGen.Emit(OpCodes.Newobj, ctorDecl.ctrBld);
				// remove constructor call from call trace
				ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("RemoveFunctionCallFromTrace", Type.EmptyTypes));
			}
			else if (node is INSTANCEOF) {
				INSTANCEOF i = (INSTANCEOF)node;
				Visit(i.expr);
				ilGen.Emit(OpCodes.Ldstr, i.type);
				ilGen.Emit(OpCodes.Ldc_I4_0);
				ilGen.Emit(OpCodes.Ldc_I4_1);
				ilGen.Emit(OpCodes.Call, typeof(Type).GetMethod("GetType", new Type[] { typeof(string), typeof(bool), typeof(bool) }));
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Instanceof", new Type[] { PHPMixed, typeof(Type) }));
			}
			else if (node is ARRAY) {
				ARRAY a = (ARRAY)node;
				// create new empty array
				ilGen.Emit(OpCodes.Newobj, PHPArray.GetConstructor(Type.EmptyTypes));
				// process array pairs
				foreach (ARRAY_PAIR ap in a.array_pair_list) {
					// duplicate reference to array (in order not to loose it after append)
					ilGen.Emit(OpCodes.Dup);
					// process key
					if (ap.key == null)
						ilGen.Emit(OpCodes.Ldnull);
					else
						Visit(ap.key);
					// process value
					if (ap.value is REFERENCE)
						if (((REFERENCE)ap.value).variable is FUNCTION_CALL)
							Report.Error(408, ap.line, ap.column);
					Visit(ap.value);
					ilGen.Emit(OpCodes.Callvirt, PHPArray.GetMethod("Append", new Type[] { PHPMixed, PHPMixed }));
				}
			}
			else if (node is INC) {
				INC i = (INC)node;
				if (i.expr is FUNCTION_CALL)
					Report.Error(408, i.expr.line, i.expr.column);
				LoadFromVariable(((VARIABLE)i.expr).name);
				if (i.kind == 1) {
					ilGen.Emit(OpCodes.Dup);
					ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Clone", new Type[] { PHPMixed }));
				}
				ilGen.Emit(OpCodes.Ldc_I4_1);
				ilGen.Emit(OpCodes.Newobj, PHPInteger.GetConstructor(new Type[] { typeof(int) }));
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Plus", new Type[] { PHPMixed, PHPMixed }));
				if (i.kind == 0) {
					ilGen.Emit(OpCodes.Dup);
					ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Clone", new Type[] { PHPMixed }));
				}
				StoreToVariable(((VARIABLE)i.expr).name);
			}
			else if (node is DEC) {
				DEC d = (DEC)node;
				if (d.expr is FUNCTION_CALL)
					Report.Error(408, d.expr.line, d.expr.column);
				LoadFromVariable(((VARIABLE)d.expr).name);
				if (d.kind == 1)
					ilGen.Emit(OpCodes.Dup);
				ilGen.Emit(OpCodes.Ldc_I4_1);
				ilGen.Emit(OpCodes.Newobj, PHPInteger.GetConstructor(new Type[] { typeof(int) }));
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Minus", new Type[] { PHPMixed, PHPMixed }));
				if (d.kind == 0)
					ilGen.Emit(OpCodes.Dup);
				StoreToVariable(((VARIABLE)d.expr).name);
			}
			else if (node is BOOLEAN_NOT) {
				BOOLEAN_NOT bn = (BOOLEAN_NOT)node;
				Visit(bn.expr);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("BooleanNot", new Type[] { PHPMixed }));
			}
			else if (node is NOT) {
				NOT n = (NOT)node;
				Visit(n.expr);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Not", new Type[] { PHPMixed }));
			}
			else if (node is EXIT) {
				EXIT e = (EXIT)node;
				if (e.expr == null)
					ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
				else
					Visit(e.expr);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Exit", new Type[] { PHPMixed }));
			}
			else if (node is PRINT) {
				PRINT p = (PRINT)node;
				Visit(p.expr);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Print", new Type[] { PHPMixed }));
			}
			else if (node is BOOL_CAST) {
				BOOL_CAST bc = (BOOL_CAST)node;
				Visit(bc.expr);
				ilGen.Emit(OpCodes.Call, PHPCoreConvert.GetMethod("ToBoolean", new Type[] { PHPMixed }));
			}
			else if (node is INT_CAST) {
				INT_CAST ic = (INT_CAST)node;
				Visit(ic.expr);
				ilGen.Emit(OpCodes.Call, PHPCoreConvert.GetMethod("ToInteger", new Type[] { PHPMixed }));
			}
			else if (node is DOUBLE_CAST) {
				DOUBLE_CAST dc = (DOUBLE_CAST)node;
				Visit(dc.expr);
				ilGen.Emit(OpCodes.Call, PHPCoreConvert.GetMethod("ToDouble", new Type[] { PHPMixed }));
			}
			else if (node is STRING_CAST) {
				STRING_CAST sc = (STRING_CAST)node;
				Visit(sc.expr);
				ilGen.Emit(OpCodes.Call, PHPCoreConvert.GetMethod("ToString", new Type[] { PHPMixed }));
			}
			else if (node is ARRAY_CAST) {
				ARRAY_CAST ac = (ARRAY_CAST)node;
				Visit(ac.expr);
				ilGen.Emit(OpCodes.Call, PHPCoreConvert.GetMethod("ToArray", new Type[] { PHPMixed }));
			}
			else if (node is OBJECT_CAST) {
				OBJECT_CAST oc = (OBJECT_CAST)node;
				Visit(oc.expr);
				ilGen.Emit(OpCodes.Call, PHPCoreConvert.GetMethod("ToObject", new Type[] { PHPMixed }));
			}
			else if (node is CLONE) {
				CLONE c = (CLONE)node;
				Visit(c.expr);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Clone", new Type[] { PHPMixed }));
			}
			else if (node is PAAMAYIM_NEKUDOTAYIM) {
				PAAMAYIM_NEKUDOTAYIM pn = (PAAMAYIM_NEKUDOTAYIM)node;
				LoadFromPaamayimNekudotayim(pn);
			}
			else if (node is OBJECT_OPERATOR) {
				OBJECT_OPERATOR oo = (OBJECT_OPERATOR)node;
				LoadFromObjectOperator(oo);
			}
			else if (node is EQUALS) {
				EQUALS e = (EQUALS)node;
				if (e.expr1 is FUNCTION_CALL)
					Report.Error(408, e.expr1.line, e.expr1.column);
				// push assigned value as result of this equals expression
				Visit(e.expr2);
				// store to an object operator expression
				if (e.expr1 is OBJECT_OPERATOR)
					StoreToObjectOperator((OBJECT_OPERATOR)e.expr1, e.expr2);
				// store to an object operator expression
				else if (e.expr1 is PAAMAYIM_NEKUDOTAYIM)
					StoreToPaamayimNekudotayim((PAAMAYIM_NEKUDOTAYIM)e.expr1, e.expr2);
				// store to a variable expression
				else if (e.expr1 is VARIABLE) {
					VARIABLE var = (VARIABLE)e.expr1;
					// determine value or reference semantis
					bool referenceSemantics = false;
					// if assigned expr is a reference
					if (e.expr2 is REFERENCE) {
						REFERENCE r = (REFERENCE)e.expr2;
						// and a variable is called, it's reference semantics
						if (r.variable is VARIABLE)
							referenceSemantics = true;
						// and a function not returnung a reference is called, it's reference semantics
						if (r.variable is FUNCTION_CALL) {
							SymbolTableEntry entry = SymbolTable.getInstance().lookup(((FUNCTION_CALL)r.variable).function_name.ToLower(), SymbolTable.FUNCTION);
							FUNCTION_DECLARATION fd = (FUNCTION_DECLARATION)entry.node;
							if (fd.return_by_reference)
								referenceSemantics = true;
						}
					}
					// if there is no offset, just store to that variable
					if (var.offset == null) {
						ilGen.Emit(OpCodes.Dup);
						if (!referenceSemantics)
							ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Clone", new Type[] { PHPMixed }));
						StoreToVariable(var.name);
					}
					// if there is an array offset, load array and store to specified place of that array
					else if (var.offset.kind == OFFSET.SQUARE) {
						LoadFromVariable(var.name);
						// if array loaded is null, create a new one and store
						Label skip = ilGen.DefineLabel();
						ilGen.Emit(OpCodes.Isinst, PHPNull);
						ilGen.Emit(OpCodes.Brfalse, skip);
						ilGen.Emit(OpCodes.Newobj, PHPArray.GetConstructor(Type.EmptyTypes));
						StoreToVariable(var.name);
						ilGen.MarkLabel(skip);
						LoadFromVariable(var.name);
						// convert to Array (in case the variable was unset)
						ilGen.Emit(OpCodes.Call, PHPCoreConvert.GetMethod("ToArray", new Type[] { PHPMixed }));
						// if value of offset is null, push null to automatically calculate key
						if (var.offset.value == null)
							ilGen.Emit(OpCodes.Ldnull);
						// if a value is given, push that value
						else
							Visit(var.offset.value);
						Visit(e.expr2);
						if (!referenceSemantics)
							ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Clone", new Type[] { PHPMixed }));
						ilGen.Emit(OpCodes.Callvirt, PHPArray.GetMethod("Append", new Type[] { PHPMixed, PHPMixed }));
					}
				}
			}
			else if (node is PLUS_EQUAL) {
				PLUS_EQUAL pe = (PLUS_EQUAL)node;
				if (pe.expr1 is FUNCTION_CALL)
					Report.Error(408, pe.expr1.line, pe.expr1.column);
				LoadFromVariable(((VARIABLE)pe.expr1).name);
				Visit(pe.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Plus", new Type[] { PHPMixed, PHPMixed }));
				ilGen.Emit(OpCodes.Dup);
				StoreToVariable(((VARIABLE)pe.expr1).name);
			}
			else if (node is MINUS_EQUAL) {
				MINUS_EQUAL me = (MINUS_EQUAL)node;
				if (me.expr1 is FUNCTION_CALL)
					Report.Error(408, me.expr1.line, me.expr1.column);
				LoadFromVariable(((VARIABLE)me.expr1).name);
				Visit(me.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Minus", new Type[] { PHPMixed, PHPMixed }));
				ilGen.Emit(OpCodes.Dup);
				StoreToVariable(((VARIABLE)me.expr1).name);
			}
			else if (node is MUL_EQUAL) {
				MUL_EQUAL me = (MUL_EQUAL)node;
				if (me.expr1 is FUNCTION_CALL)
					Report.Error(408, me.expr1.line, me.expr1.column);
				LoadFromVariable(((VARIABLE)me.expr1).name);
				Visit(me.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Mul", new Type[] { PHPMixed, PHPMixed }));
				ilGen.Emit(OpCodes.Dup);
				StoreToVariable(((VARIABLE)me.expr1).name);
			}
			else if (node is DIV_EQUAL) {
				DIV_EQUAL de = (DIV_EQUAL)node;
				if (de.expr1 is FUNCTION_CALL)
					Report.Error(408, de.expr1.line, de.expr1.column);
				LoadFromVariable(((VARIABLE)de.expr1).name);
				Visit(de.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Div", new Type[] { PHPMixed, PHPMixed }));
				ilGen.Emit(OpCodes.Dup);
				StoreToVariable(((VARIABLE)de.expr1).name);
			}
			else if (node is CONCAT_EQUAL) {
				CONCAT_EQUAL ce = (CONCAT_EQUAL)node;
				if (ce.expr1 is FUNCTION_CALL)
					Report.Error(408, ce.expr1.line, ce.expr1.column);
				LoadFromVariable(((VARIABLE)ce.expr1).name);
				Visit(ce.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Concat", new Type[] { PHPMixed, PHPMixed }));
				ilGen.Emit(OpCodes.Dup);
				StoreToVariable(((VARIABLE)ce.expr1).name);
			}
			else if (node is MOD_EQUAL) {
				MOD_EQUAL me = (MOD_EQUAL)node;
				if (me.expr1 is FUNCTION_CALL)
					Report.Error(408, me.expr1.line, me.expr1.column);
				LoadFromVariable(((VARIABLE)me.expr1).name);
				Visit(me.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Mod", new Type[] { PHPMixed, PHPMixed }));
				ilGen.Emit(OpCodes.Dup);
				StoreToVariable(((VARIABLE)me.expr1).name);
			}
			else if (node is AND_EQUAL) {
				AND_EQUAL ae = (AND_EQUAL)node;
				if (ae.expr1 is FUNCTION_CALL)
					Report.Error(408, ae.expr1.line, ae.expr1.column);
				LoadFromVariable(((VARIABLE)ae.expr1).name);
				Visit(ae.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("And", new Type[] { PHPMixed, PHPMixed }));
				ilGen.Emit(OpCodes.Dup);
				StoreToVariable(((VARIABLE)ae.expr1).name);
			}
			else if (node is OR_EQUAL) {
				OR_EQUAL oe = (OR_EQUAL)node;
				if (oe.expr1 is FUNCTION_CALL)
					Report.Error(408, oe.expr1.line, oe.expr1.column);
				LoadFromVariable(((VARIABLE)oe.expr1).name);
				Visit(oe.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Or", new Type[] { PHPMixed, PHPMixed }));
				ilGen.Emit(OpCodes.Dup);
				StoreToVariable(((VARIABLE)oe.expr1).name);
			}
			else if (node is XOR_EQUAL) {
				XOR_EQUAL xe = (XOR_EQUAL)node;
				if (xe.expr1 is FUNCTION_CALL)
					Report.Error(408, xe.expr1.line, xe.expr1.column);
				LoadFromVariable(((VARIABLE)xe.expr1).name);
				Visit(xe.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Xor", new Type[] { PHPMixed, PHPMixed }));
				ilGen.Emit(OpCodes.Dup);
				StoreToVariable(((VARIABLE)xe.expr1).name);
			}
			else if (node is SL_EQUAL) {
				SL_EQUAL se = (SL_EQUAL)node;
				if (se.expr1 is FUNCTION_CALL)
					Report.Error(408, se.expr1.line, se.expr1.column);
				LoadFromVariable(((VARIABLE)se.expr1).name);
				Visit(se.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Sl", new Type[] { PHPMixed, PHPMixed }));
				ilGen.Emit(OpCodes.Dup);
				StoreToVariable(((VARIABLE)se.expr1).name);
			}
			else if (node is SR_EQUAL) {
				SR_EQUAL se = (SR_EQUAL)node;
				if (se.expr1 is FUNCTION_CALL)
					Report.Error(408, se.expr1.line, se.expr1.column);
				LoadFromVariable(((VARIABLE)se.expr1).name);
				Visit(se.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Sr", new Type[] { PHPMixed, PHPMixed }));
				ilGen.Emit(OpCodes.Dup);
				StoreToVariable(((VARIABLE)se.expr1).name);
			}
			else if (node is BOOLEAN_AND) {
				BOOLEAN_AND ba = (BOOLEAN_AND)node;
				Visit(ba.expr1);
				Visit(ba.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("BooleanAnd", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is BOOLEAN_OR) {
				BOOLEAN_OR bo = (BOOLEAN_OR)node;
				Visit(bo.expr1);
				Visit(bo.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("BooleanOr", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is LOGICAL_AND) {
				LOGICAL_AND la = (LOGICAL_AND)node;
				Visit(la.expr1);
				Visit(la.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("LogicalAnd", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is LOGICAL_OR) {
				LOGICAL_OR lo = (LOGICAL_OR)node;
				Visit(lo.expr1);
				Visit(lo.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("LogicalOr", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is LOGICAL_XOR) {
				LOGICAL_XOR lx = (LOGICAL_XOR)node;
				Visit(lx.expr1);
				Visit(lx.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("LogicalXor", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is CONCAT) {
				CONCAT c = (CONCAT)node;
				Visit(c.expr1);
				Visit(c.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Concat", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is PLUS) {
				PLUS p = (PLUS)node;
				Visit(p.expr1);
				Visit(p.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Plus", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is MINUS) {
				MINUS m = (MINUS)node;
				Visit(m.expr1);
				Visit(m.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Minus", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is TIMES) {
				TIMES t = (TIMES)node;
				Visit(t.expr1);
				Visit(t.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Times", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is DIV) {
				DIV d = (DIV)node;
				Visit(d.expr1);
				Visit(d.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Div", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is MOD) {
				MOD m = (MOD)node;
				Visit(m.expr1);
				Visit(m.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Mod", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is AND) {
				AND a = (AND)node;
				Visit(a.expr1);
				Visit(a.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("And", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is OR) {
				OR o = (OR)node;
				Visit(o.expr1);
				Visit(o.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Or", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is XOR) {
				XOR x = (XOR)node;
				Visit(x.expr1);
				Visit(x.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Xor", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is SL) {
				SL s = (SL)node;
				Visit(s.expr1);
				Visit(s.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Sl", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is SR) {
				SL s = (SL)node;
				Visit(s.expr1);
				Visit(s.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Sr", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is IS_EQUAL) {
				IS_EQUAL ie = (IS_EQUAL)node;
				Visit(ie.expr1);
				Visit(ie.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("IsEqual", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is IS_NOT_EQUAL) {
				IS_NOT_EQUAL ine = (IS_NOT_EQUAL)node;
				Visit(ine.expr1);
				Visit(ine.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("IsNotEqual", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is IS_IDENTICAL) {
				IS_IDENTICAL ii = (IS_IDENTICAL)node;
				Visit(ii.expr1);
				Visit(ii.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("IsIdentical", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is IS_NOT_IDENTICAL) {
				IS_NOT_IDENTICAL ini = (IS_NOT_IDENTICAL)node;
				Visit(ini.expr1);
				Visit(ini.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("IsNotIdentical", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is LOWER) {
				LOWER l = (LOWER)node;
				Visit(l.expr1);
				Visit(l.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Lower", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is IS_LOWER_OR_EQUAL) {
				IS_LOWER_OR_EQUAL iloe = (IS_LOWER_OR_EQUAL)node;
				Visit(iloe.expr1);
				Visit(iloe.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("IsLowerOrEqual", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is GREATER) {
				GREATER g = (GREATER)node;
				Visit(g.expr1);
				Visit(g.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Greater", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is IS_GREATER_OR_EQUAL) {
				IS_GREATER_OR_EQUAL igoe = (IS_GREATER_OR_EQUAL)node;
				Visit(igoe.expr1);
				Visit(igoe.expr2);
				ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("IsGreaterOrEqual", new Type[] { PHPMixed, PHPMixed }));
			}
			else if (node is IF_EXPR) {
				IF_EXPR ie = (IF_EXPR)node;
				Label falseBranch = ilGen.DefineLabel();
				Label mergeBranches = ilGen.DefineLabel();
				Visit(ie.expr1);
				ilGen.Emit(OpCodes.Call, PHPCoreConvert.GetMethod("ToValueBool", new Type[] { PHPMixed }));
				ilGen.Emit(OpCodes.Brfalse, falseBranch);
				Visit(ie.expr2);
				ilGen.Emit(OpCodes.Br, mergeBranches);
				ilGen.MarkLabel(falseBranch);
				Visit(ie.expr3);
				ilGen.MarkLabel(mergeBranches);
			}
			else if (node is LNUMBER_SCALAR) {
				LNUMBER_SCALAR ls = (LNUMBER_SCALAR)node;
				ilGen.Emit(OpCodes.Ldc_I4, ls.value);
				ilGen.Emit(OpCodes.Newobj, PHPInteger.GetConstructor(new Type[] { typeof(int) }));
			}
			else if (node is DNUMBER_SCALAR) {
				DNUMBER_SCALAR ds = (DNUMBER_SCALAR)node;
				ilGen.Emit(OpCodes.Ldc_R8, ds.value);
				ilGen.Emit(OpCodes.Newobj, PHPDouble.GetConstructor(new Type[] { typeof(double) }));
			}
			else if (node is STRING_SCALAR) {
				STRING_SCALAR ss = (STRING_SCALAR)node;
				if (ss.value.ToLower() == "true") {
					ilGen.Emit(OpCodes.Ldc_I4_1);
					ilGen.Emit(OpCodes.Newobj, PHPBoolean.GetConstructor(new Type[] { typeof(bool) }));
				}
				else if (ss.value.ToLower() == "false") {
					ilGen.Emit(OpCodes.Ldc_I4_0);
					ilGen.Emit(OpCodes.Newobj, PHPBoolean.GetConstructor(new Type[] { typeof(bool) }));
				}
				else {
					ilGen.Emit(OpCodes.Ldstr, ss.value);
					ilGen.Emit(OpCodes.Newobj, PHPString.GetConstructor(new Type[] { typeof(string) }));
				}
			}
			else if (node is SINGLE_QUOTES) {
				SINGLE_QUOTES sq = (SINGLE_QUOTES)node;
				StringBuilder result = new StringBuilder();
				foreach (object o in sq.encaps_list) {
					if (o is string)
						result.Append((string)o);
					else if (o is VARIABLE) {
						result.Append('$');
						result.Append(((VARIABLE)o).name);
					}
				}
				ilGen.Emit(OpCodes.Ldstr, result.ToString());
				ilGen.Emit(OpCodes.Newobj, PHPString.GetConstructor(new Type[] { typeof(string) }));
			}
			else if (node is DOUBLE_QUOTES) {
				DOUBLE_QUOTES dq = (DOUBLE_QUOTES)node;
				StringBuilder output = new StringBuilder();
				bool concat = false;
				foreach (object o in dq.encaps_list) {
					if (o is string)
						output.Append((string)o);
					else {
						// push substring between last variable and current one and concat, if necessary
						if (output.Length > 0) {
							ilGen.Emit(OpCodes.Ldstr, output.ToString());
							ilGen.Emit(OpCodes.Newobj, PHPString.GetConstructor(new Type[] { typeof(string) }));
							if (concat)
								ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Concat", new Type[] { PHPMixed, PHPMixed }));
							concat = true;
							output = new StringBuilder();
						}
						// push variable value
						if (o is VARIABLE)
							Visit((VARIABLE)o);
						// or push object operator result
						else if (o is OBJECT_OPERATOR)
							Visit((OBJECT_OPERATOR)o);
						// or push Null
						else
							ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
						// concat, if necessary
						if (concat)
							ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Concat", new Type[] { PHPMixed, PHPMixed }));
						concat = true;
					}
				}
				// push substring after last variable and concat, if necessary
				if (output.Length > 0) {
					ilGen.Emit(OpCodes.Ldstr, output.ToString());
					ilGen.Emit(OpCodes.Newobj, PHPString.GetConstructor(new Type[] { typeof(string) }));
					if (concat)
						ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Concat", new Type[] { PHPMixed, PHPMixed }));
					output = null;
				}
			}
			else if (node is HEREDOC) {
				HEREDOC h = (HEREDOC)node;
				StringBuilder output = new StringBuilder();
				bool concat = false;
				foreach (object o in h.encaps_list) {
					if (o is string)
						output.Append((string)o);
					else {
						// push substring between last variable and current one and concat, if necessary
						if (output.Length > 0) {
							ilGen.Emit(OpCodes.Ldstr, output.ToString());
							ilGen.Emit(OpCodes.Newobj, PHPString.GetConstructor(new Type[] { typeof(string) }));
							if (concat)
								ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Concat", new Type[] { PHPMixed, PHPMixed }));
							concat = true;
							output = new StringBuilder();
						}
						// push variable value
						if (o is VARIABLE)
							Visit((VARIABLE)o);
						// or push object operator result
						else if (o is OBJECT_OPERATOR)
							Visit((OBJECT_OPERATOR)o);
						// or push Null
						else
							ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
						// concat, if necessary
						if (concat)
							ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Concat", new Type[] { PHPMixed, PHPMixed }));
						concat = true;
					}
				}
				// push substring after last variable and concat, if necessary
				if (output.Length > 0) {
					ilGen.Emit(OpCodes.Ldstr, output.ToString());
					ilGen.Emit(OpCodes.Newobj, PHPString.GetConstructor(new Type[] { typeof(string) }));
					if (concat)
						ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Concat", new Type[] { PHPMixed, PHPMixed }));
					output = null;
				}
			}
			else if (node is CONSTANT) {
				CONSTANT c = (CONSTANT)node;
				// push constant value
				ilGen.Emit(OpCodes.Ldstr, c.name);
				ilGen.Emit(OpCodes.Newobj, PHPString.GetConstructor(new Type[] { typeof(string) }));
				ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("GetConstant", new Type[] { PHPMixed }));
			}
			else if (node is UnaryExpression) {
				UnaryExpression ue = (UnaryExpression)node;
				// process expression
				Visit(ue.expr);
			}
			else if (node is BinaryExpression) {
				BinaryExpression be = (BinaryExpression)node;
				// process expressions
				Visit(be.expr1);
				Visit(be.expr2);
			}
			else if (node is TernaryExpression) {
				TernaryExpression te = (TernaryExpression)node;
				// process expressions
				Visit(te.expr1);
				Visit(te.expr2);
				Visit(te.expr3);
			}
			else if (node is Expression) {
				Expression e = (Expression)node;
				if (e is VARIABLE)
					Visit((VARIABLE)e);
				else if (e is FUNCTION_CALL)
					Visit((FUNCTION_CALL)e);
				else if (e is ARRAY)
					Visit((ARRAY)e);
				else if (e is UnaryExpression)
					Visit((UnaryExpression)e);
				else if (e is BinaryExpression)
					Visit((BinaryExpression)e);
				else if (e is TernaryExpression)
					Visit((TernaryExpression)e);
			}
		}

		public void PushParameters(ExpressionList parameters, int number) {
			int parametersPushed = 0;
			// push parameters available, until desired number is reached
			while (parametersPushed < number && parametersPushed < parameters.Count())
				Visit(parameters.Get(parametersPushed++));
			// if not enough parameters were available, fill up with Nulls
			while (parametersPushed < number) {
				ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
				parametersPushed++;
			}
		}

		public void LoadFromVariable(string variableName) {
			ilGen.Emit(OpCodes.Ldstr, variableName);
			ilGen.Emit(OpCodes.Newobj, PHPString.GetConstructor(new Type[] { typeof(string) }));
			if (fd.scope.globalVariables.Contains(variableName))
				ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("LoadFromGlobalVariable", new Type[] { PHPMixed }));
			else
				ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("LoadFromVariable", new Type[] { PHPMixed }));
		}

		public void StoreToVariable(string variableName) {
			ilGen.Emit(OpCodes.Ldstr, variableName);
			ilGen.Emit(OpCodes.Newobj, PHPString.GetConstructor(new Type[] { typeof(string) }));
			if (fd.scope.globalVariables.Contains(variableName))
				ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("StoreToGlobalVariable", new Type[] { PHPMixed, PHPMixed }));
			else
				ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("StoreToVariable", new Type[] { PHPMixed, PHPMixed }));
		}

		public void StoreToStaticVariable(string variableName) {
			ilGen.Emit(OpCodes.Ldstr, variableName);
			ilGen.Emit(OpCodes.Newobj, PHPString.GetConstructor(new Type[] { typeof(string) }));
			if (fd.scope.globalVariables.Contains(variableName))
				ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("StoreToGlobalVariable", new Type[] { PHPMixed, PHPMixed }));
			else
				ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("StoreToStaticVariable", new Type[] { PHPMixed, PHPMixed }));
		}

		public void StoreToObjectOperator(OBJECT_OPERATOR oo, Expression value) {
			// warn if left part is a function call
			if (oo.expr1 is FUNCTION_CALL) {
				FUNCTION_CALL fc = (FUNCTION_CALL)oo.expr1;
				Report.Warn(404, fc.function_name, fc.line, fc.column);
				if (objectOperatorInProgress) {
					ilGen.Emit(OpCodes.Pop);
					objectOperatorInProgress = false;
				}
				return;
			}
			// process left part
			objectOperatorInProgress = true;
			VARIABLE var = (VARIABLE)oo.expr1;
			// local variable
			// handle $this->
			if (var.name == "$this") {
				processingObjectThis = true;
				// if this is a static context, load object from which this static context was called
				if (fd.modifiers.Contains(Modifiers.STATIC)) {
					Report.Warn(502, var.line, var.column);
					ilGen.Emit(OpCodes.Ldsfld, PHPCoreRuntime.GetField("thisForStaticContext"));
				}
				// otherwise load current object
				else
					ilGen.Emit(OpCodes.Ldarg_0);
				if (var.offset != null)
					Report.Warn(401, var.offset.line, var.offset.column);
			}
			else if (var.name.StartsWith("$")) {
				processingObjectThis = false;
				Visit(var);
			}
			// class member
			else {
				// if processing a class member of $this (of a non-static context), load directly
				if (processingObjectThis && !fd.modifiers.Contains(Modifiers.STATIC))
					LoadFromClassMemberOfThis("$" + var.name, var.line, var.column);
				// else load at runtime by reflection
				else
					LoadFromClassMemberOfAnotherObject("$" + var.name);
				processingObjectThis = false;
				// process offset, if available
				if (var.offset != null) {
					// this is an array
					if (var.offset.kind == OFFSET.SQUARE) {
						if (var.offset.value == null)
							ilGen.Emit(OpCodes.Ldnull);
						else
							Visit(var.offset);
						ilGen.Emit(OpCodes.Ldc_I4, OFFSET.SQUARE);
						ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Offset", new Type[] { PHPMixed, PHPMixed, typeof(int) }));
					}
				}
			}
			// process right part
			if (oo.expr2 is OBJECT_OPERATOR)
				StoreToObjectOperator((OBJECT_OPERATOR)oo.expr2, value);
			else if (oo.expr2 is VARIABLE) {
				VARIABLE var2 = (VARIABLE)oo.expr2;
				// no offset available, so store to class member
				if (var2.offset == null) {
					Visit(value);
					// determine value or reference semantis
					bool referenceSemantics = false;
					// if assigned expr is a reference
					if (value is REFERENCE) {
						REFERENCE r = (REFERENCE)value;
						// and a variable is called, it's reference semantics
						if (r.variable is VARIABLE)
							referenceSemantics = true;
						// and a function not returnung a reference is called, it's reference semantics
						if (r.variable is FUNCTION_CALL) {
							SymbolTableEntry entry = SymbolTable.getInstance().lookup(((FUNCTION_CALL)r.variable).function_name.ToLower(), SymbolTable.FUNCTION);
							FUNCTION_DECLARATION fd = (FUNCTION_DECLARATION)entry.node;
							if (fd.return_by_reference)
								referenceSemantics = true;
						}
					}
					if (!referenceSemantics)
						ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Clone", new Type[] { PHPMixed }));
					// if processing a class member of $this, store directly
					if (processingObjectThis)
						StoreToClassMemberOfThis("$" + var2.name, var2.line, var2.column);
					// else store at runtime by reflection
					else
						StoreToClassMemberOfAnotherObject("$" + var2.name);
				}
				// array offset available, so store to specified place
				else if (var2.offset.kind == OFFSET.SQUARE) {
					ilGen.Emit(OpCodes.Dup);
					ilGen.Emit(OpCodes.Dup);
					// if processing a class member of $this, load directly
					if (processingObjectThis)
						LoadFromClassMemberOfThis("$" + var2.name, var2.line, var2.column);
					// else load at runtime by reflection
					else
						LoadFromClassMemberOfAnotherObject("$" + var2.name);
					// if array loaded is null, create a new one and store
					Label skip = ilGen.DefineLabel();
					Label join = ilGen.DefineLabel();
					ilGen.Emit(OpCodes.Isinst, PHPNull);
					ilGen.Emit(OpCodes.Brfalse, skip);
					ilGen.Emit(OpCodes.Newobj, PHPArray.GetConstructor(Type.EmptyTypes));
					// if processing a class member of $this, store directly
					if (processingObjectThis)
						StoreToClassMemberOfThis("$" + var2.name, var2.line, var2.column);
					// else store at runtime by reflection
					else
						StoreToClassMemberOfAnotherObject("$" + var2.name);
					ilGen.Emit(OpCodes.Br, join);
					ilGen.MarkLabel(skip);
					ilGen.Emit(OpCodes.Pop);
					ilGen.MarkLabel(join);
					// if processing a class member of $this, load directly
					if (processingObjectThis)
						LoadFromClassMemberOfThis("$" + var2.name, var2.line, var2.column);
					// else load at runtime by reflection
					else
						LoadFromClassMemberOfAnotherObject("$" + var2.name);
					// convert to Array (in case the variable was unset)
					ilGen.Emit(OpCodes.Callvirt, PHPCoreConvert.GetMethod("ToArray", new Type[] { PHPMixed }));
					// if value of offset is null, push null to automatically calculate key
					if (var2.offset.value == null)
						ilGen.Emit(OpCodes.Ldnull);
					// if a value is given, push that value
					else
						Visit(var2.offset.value);
					Visit(value);
					if (!(value is REFERENCE))
						ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Clone", new Type[] { PHPMixed }));
					ilGen.Emit(OpCodes.Callvirt, PHPArray.GetMethod("Append", new Type[] { PHPMixed, PHPMixed }));
				}
				objectOperatorInProgress = false;
			}
			else if (oo.expr2 is FUNCTION_CALL) {
				FUNCTION_CALL fc = (FUNCTION_CALL)oo.expr2;
				Report.Warn(403, fc.function_name, fc.line, fc.column);
				if (objectOperatorInProgress) {
					ilGen.Emit(OpCodes.Pop);
					objectOperatorInProgress = false;
				}
				return;
			}
		}

		public void LoadFromObjectOperator(OBJECT_OPERATOR oo) {
			// warn if left part is a function call
			if (oo.expr1 is FUNCTION_CALL) {
				FUNCTION_CALL fc = (FUNCTION_CALL)oo.expr1;
				Report.Warn(404, fc.function_name, fc.line, fc.column);
				if (objectOperatorInProgress) {
					ilGen.Emit(OpCodes.Pop);
					objectOperatorInProgress = false;
				}
				ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
				return;
			}
			// process left part
			objectOperatorInProgress = true;
			VARIABLE var = (VARIABLE)oo.expr1;
			// local variable
			// handle $this->
			if (var.name == "$this") {
				processingObjectThis = true;
				// if this is a static context, load object from which this static context was called
				if (fd.modifiers.Contains(Modifiers.STATIC)) {
					Report.Warn(502, var.line, var.column);
					ilGen.Emit(OpCodes.Ldsfld, PHPCoreRuntime.GetField("thisForStaticContext"));
				}
				// otherwise load current object
				else
					ilGen.Emit(OpCodes.Ldarg_0);
				if (var.offset != null)
					Report.Warn(401, var.offset.line, var.offset.column);
			}
			else if (var.name.StartsWith("$")) {
				processingObjectThis = false;
				Visit(var);
			}
			// class member
			else {
				// if processing a class member of $this, load directly
				if (processingObjectThis)
					LoadFromClassMemberOfThis("$" + var.name, var.line, var.column);
				// else load at runtime by reflection
				else
					LoadFromClassMemberOfAnotherObject("$" + var.name);
				processingObjectThis = false;
				// process offset, if available
				if (var.offset != null) {
					// this is an array
					if (var.offset.kind == OFFSET.SQUARE) {
						if (var.offset.value == null)
							ilGen.Emit(OpCodes.Ldnull);
						else
							Visit(var.offset);
						ilGen.Emit(OpCodes.Ldc_I4, OFFSET.SQUARE);
						ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Offset", new Type[] { PHPMixed, PHPMixed, typeof(int) }));
					}
				}
			}
			// process right part
			if (oo.expr2 is OBJECT_OPERATOR)
				LoadFromObjectOperator((OBJECT_OPERATOR)oo.expr2);
			else if (oo.expr2 is VARIABLE) {
				VARIABLE var2 = (VARIABLE)oo.expr2;
				// if processing a class member of $this (of a non-static context), load directly
				if (processingObjectThis && !fd.modifiers.Contains(Modifiers.STATIC))
					LoadFromClassMemberOfThis("$" + var2.name, var2.line, var2.column);
				// else load at runtime by reflection
				else
					LoadFromClassMemberOfAnotherObject("$" + var2.name);
				// process offset, if available
				if (var2.offset != null) {
					// this is an array
					if (var2.offset.kind == OFFSET.SQUARE) {
						if (var2.offset.value == null)
							ilGen.Emit(OpCodes.Ldnull);
						else
							Visit(var2.offset);
						ilGen.Emit(OpCodes.Ldc_I4, OFFSET.SQUARE);
						ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Offset", new Type[] { PHPMixed, PHPMixed, typeof(int) }));
					}
				}
				objectOperatorInProgress = false;
			}
			else if (oo.expr2 is FUNCTION_CALL) {
				FUNCTION_CALL fc = (FUNCTION_CALL)oo.expr2;
				// if processing a class member of $this, invoke directly
				if (processingObjectThis)
					InvokeFunctionOfThis(fc.function_name, fc.line, fc.column);
				// else invoke at runtime by reflection
				else {
					ilGen.Emit(OpCodes.Newobj, typeof(ArrayList).GetConstructor(Type.EmptyTypes));
					foreach (Expression expr in fc.parameters) {
						ilGen.Emit(OpCodes.Dup);
						Visit(expr);
						ilGen.Emit(OpCodes.Call, typeof(ArrayList).GetMethod("Add", new Type[] { typeof(object) }));
						ilGen.Emit(OpCodes.Pop);
					}
					InvokeFunctionOfAnotherObject(fc.function_name);
				}
				objectOperatorInProgress = false;
			}
		}

		public void StoreToPaamayimNekudotayim(PAAMAYIM_NEKUDOTAYIM pn, Expression value) {
			// handle self:: and parent::
			if (pn.type == "self")
				pn.type = cd.name;
			else if (pn.type == "parent") {
				if (cd.extends == null)
					Report.Error(503, pn.line, pn.column);
				pn.type = cd.extends;
			}
			// process right part
			if (pn.expr is VARIABLE) {
				VARIABLE var = (VARIABLE)pn.expr;
				// no offset available, so store to class member
				if (var.offset == null) {
					Visit(value);
					// determine value or reference semantis
					bool referenceSemantics = false;
					// if assigned expr is a reference
					if (value is REFERENCE) {
						REFERENCE r = (REFERENCE)value;
						// and a variable is called, it's reference semantics
						if (r.variable is VARIABLE)
							referenceSemantics = true;
						// and a function not returnung a reference is called, it's reference semantics
						if (r.variable is FUNCTION_CALL) {
							SymbolTableEntry entry = SymbolTable.getInstance().lookup(((FUNCTION_CALL)r.variable).function_name.ToLower(), SymbolTable.FUNCTION);
							FUNCTION_DECLARATION fd = (FUNCTION_DECLARATION)entry.node;
							if (fd.return_by_reference)
								referenceSemantics = true;
						}
					}
					if (!referenceSemantics)
						ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Clone", new Type[] { PHPMixed }));
					// store
					StoreToStaticClassMember(pn.type, var.name, var.line, var.column);
				}
				// array offset available, so store to specified place
				else if (var.offset.kind == OFFSET.SQUARE) {
					// load
					LoadFromStaticClassMember(pn.type, var.name, var.line, var.column);
					// if array loaded is null, create a new one and store
					Label skip = ilGen.DefineLabel();
					ilGen.Emit(OpCodes.Isinst, PHPNull);
					ilGen.Emit(OpCodes.Brfalse, skip);
					ilGen.Emit(OpCodes.Newobj, PHPArray.GetConstructor(Type.EmptyTypes));
					// store
					StoreToStaticClassMember(pn.type, var.name, var.line, var.column);
					ilGen.MarkLabel(skip);
					// load
					LoadFromStaticClassMember(pn.type, var.name, var.line, var.column);
					// convert to Array (in case the variable was unset)
					ilGen.Emit(OpCodes.Callvirt, PHPCoreConvert.GetMethod("ToArray", new Type[] { PHPMixed }));
					// if value of offset is null, push null to automatically calculate key
					if (var.offset.value == null)
						ilGen.Emit(OpCodes.Ldnull);
					// if a value is given, push that value
					else
						Visit(var.offset.value);
					Visit(value);
					if (!(value is REFERENCE))
						ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Clone", new Type[] { PHPMixed }));
					ilGen.Emit(OpCodes.Callvirt, PHPArray.GetMethod("Append", new Type[] { PHPMixed, PHPMixed }));
				}
			}
			else if (pn.expr is FUNCTION_CALL) {
				Report.Error(408, pn.line, pn.column);
				return;
			}
		}

		public void LoadFromPaamayimNekudotayim(PAAMAYIM_NEKUDOTAYIM pn) {
			// handle self:: and parent::
			if (pn.type == "self")
				pn.type = cd.name;
			else if (pn.type == "parent") {
				if (cd.extends == null)
					Report.Error(503, pn.line, pn.column);
				pn.type = cd.extends;
			}
			if (pn.expr is VARIABLE) {
				VARIABLE var = (VARIABLE)pn.expr;
				LoadFromStaticClassMember(pn.type, var.name, var.line, var.column);
				// process offset, if available
				if (var.offset != null) {
					// this is an array
					if (var.offset.kind == OFFSET.SQUARE) {
						if (var.offset.value == null)
							ilGen.Emit(OpCodes.Ldnull);
						else
							Visit(var.offset);
						ilGen.Emit(OpCodes.Ldc_I4, OFFSET.SQUARE);
						ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Offset", new Type[] { PHPMixed, PHPMixed, typeof(int) }));
					}
				}
			}
			else if (pn.expr is FUNCTION_CALL) {
				FUNCTION_CALL fc = (FUNCTION_CALL)pn.expr;
				// is this a non-static context?
				// if so, push current object to handle $this in static function
				if (!fd.modifiers.Contains(Modifiers.STATIC))
					ilGen.Emit(OpCodes.Ldarg_0);
				// if not, push Null
				else
					ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
				ilGen.Emit(OpCodes.Stsfld, PHPCoreRuntime.GetField("thisForStaticContext"));
				// invoke
				InvokeStaticFunction(pn.type, fc.function_name, fc.parameters, fc.line, fc.column);
			}
		}

		public void StoreToClassMemberOfThis(string classMemberName, int line, int column) {
			CLASS_DECLARATION tmpCD = cd;
			SymbolTableEntry cvdEntry = tmpCD.scope.lookup(classMemberName, SymbolTable.CLASS_VARIABLE);
			while (cvdEntry == null) {
				if (tmpCD.extends == null)
					break;
				SymbolTableEntry parentCdEntry = SymbolTable.getInstance().lookupGlobal(tmpCD.extends.ToLower(), SymbolTable.CLASS);
				tmpCD = (CLASS_DECLARATION)parentCdEntry.node;
				cvdEntry = tmpCD.scope.lookup(classMemberName, SymbolTable.CLASS_VARIABLE);
				if (cvdEntry != null && ((CLASS_VARIABLE_DECLARATION)cvdEntry.node).modifiers.Contains(Modifiers.PRIVATE)) {
					Report.Warn(206, classMemberName, line, column);
					ilGen.Emit(OpCodes.Pop);
					ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
					return;
				}
			}
			if (cvdEntry == null) {
				Report.Warn(205, classMemberName, line, column);
				ilGen.Emit(OpCodes.Pop);
				ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
				return;
			}
			CLASS_VARIABLE_DECLARATION cvd = (CLASS_VARIABLE_DECLARATION)cvdEntry.node;
			int index = cvd.names.IndexOf(classMemberName);
			FieldBuilder fb = (FieldBuilder)cvd.fieldBuilders[index];
			ilGen.Emit(OpCodes.Stfld, fb);
		}

		public void LoadFromClassMemberOfThis(string classMemberName, int line, int column) {
			CLASS_DECLARATION tmpCD = cd;
			SymbolTableEntry cvdEntry = tmpCD.scope.lookup(classMemberName, SymbolTable.CLASS_VARIABLE);
			while (cvdEntry == null) {
				if (tmpCD.extends == null)
					break;
				SymbolTableEntry parentCdEntry = SymbolTable.getInstance().lookupGlobal(tmpCD.extends.ToLower(), SymbolTable.CLASS);
				tmpCD = (CLASS_DECLARATION)parentCdEntry.node;
				cvdEntry = tmpCD.scope.lookup(classMemberName, SymbolTable.CLASS_VARIABLE);
				if (cvdEntry != null && ((CLASS_VARIABLE_DECLARATION)cvdEntry.node).modifiers.Contains(Modifiers.PRIVATE)) {
					Report.Warn(206, classMemberName, line, column);
					ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
					return;
				}
			}
			if (cvdEntry == null) {
				Report.Warn(205, classMemberName, line, column);
				ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
				return;
			}
			CLASS_VARIABLE_DECLARATION cvd = (CLASS_VARIABLE_DECLARATION)cvdEntry.node;
			int index = cvd.names.IndexOf(classMemberName);
			FieldBuilder fb = (FieldBuilder)cvd.fieldBuilders[index];
			ilGen.Emit(OpCodes.Ldfld, fb);
			// if object is available on heap, load newly from there
			ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("Refresh", new Type[] { PHPMixed }));
		}

		public void InvokeFunctionOfThis(string functionName, int line, int column) {
			CLASS_DECLARATION tmpCD = cd;
			SymbolTableEntry fdEntry = tmpCD.scope.lookup(functionName, SymbolTable.FUNCTION);
			while (fdEntry == null) {
				if (tmpCD.extends == null)
					break;
				SymbolTableEntry parentCdEntry = SymbolTable.getInstance().lookupGlobal(tmpCD.extends.ToLower(), SymbolTable.CLASS);
				tmpCD = (CLASS_DECLARATION)parentCdEntry.node;
				fdEntry = tmpCD.scope.lookup(functionName, SymbolTable.FUNCTION);
				if (fdEntry != null && ((FUNCTION_DECLARATION)fdEntry.node).modifiers.Contains(Modifiers.PRIVATE)) {
					Report.Warn(213, functionName, line, column);
					ilGen.Emit(OpCodes.Pop);
					ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
					return;
				}
			}
			if (fdEntry == null) {
				Report.Warn(212, functionName, line, column);
				ilGen.Emit(OpCodes.Pop);
				ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
				return;
			}
			FUNCTION_DECLARATION fd = (FUNCTION_DECLARATION)fdEntry.node;
			MethodBuilder mb = fd.mthBld;
			// add function call to call trace
			ilGen.Emit(OpCodes.Ldstr, cd.name + "->" + fd.name);
			ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("AddFunctionCallToTrace", new Type[] { typeof(string) }));
			ilGen.Emit(OpCodes.Call, mb);
			// remove function call from call trace
			ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("RemoveFunctionCallFromTrace", Type.EmptyTypes));
		}

		public void StoreToClassMemberOfAnotherObject(string classMemberName) {
			ilGen.Emit(OpCodes.Ldstr, classMemberName);
			ilGen.Emit(OpCodes.Newobj, PHPString.GetConstructor(new Type[] { typeof(string) }));
			ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("StoreToClassMember", new Type[] { PHPMixed, PHPMixed, PHPMixed }));
		}

		public void LoadFromClassMemberOfAnotherObject(string classMemberName) {
			ilGen.Emit(OpCodes.Ldstr, classMemberName);
			ilGen.Emit(OpCodes.Newobj, PHPString.GetConstructor(new Type[] { typeof(string) }));
			ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("LoadFromClassMember", new Type[] { PHPMixed, PHPMixed }));
		}

		public void InvokeFunctionOfAnotherObject(string functionName) {
			ilGen.Emit(OpCodes.Ldstr, functionName);
			ilGen.Emit(OpCodes.Newobj, PHPString.GetConstructor(new Type[] { typeof(string) }));
			ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("InvokeFunction", new Type[] { PHPMixed, typeof(ArrayList), PHPMixed }));
		}

		public void StoreToStaticClassMember(string className, string classMemberName, int line, int column) {
			SymbolTableEntry cdEntry = SymbolTable.getInstance().lookupGlobal(className.ToLower(), SymbolTable.CLASS);
			CLASS_DECLARATION cd = (CLASS_DECLARATION)cdEntry.node;
			CLASS_DECLARATION tmpCD = cd;
			SymbolTableEntry cvdEntry = tmpCD.scope.lookup(classMemberName, SymbolTable.CLASS_VARIABLE);
			while (cvdEntry == null) {
				if (tmpCD.extends == null)
					break;
				SymbolTableEntry parentCdEntry = SymbolTable.getInstance().lookupGlobal(tmpCD.extends.ToLower(), SymbolTable.CLASS);
				tmpCD = (CLASS_DECLARATION)parentCdEntry.node;
				cvdEntry = tmpCD.scope.lookup(classMemberName, SymbolTable.CLASS_VARIABLE);
				if (cvdEntry != null && !((CLASS_VARIABLE_DECLARATION)cvdEntry.node).modifiers.Contains(Modifiers.STATIC) && !((CLASS_VARIABLE_DECLARATION)cvdEntry.node).modifiers.Contains(Modifiers.CONST)) {
					Report.Warn(208, classMemberName, line, column);
					ilGen.Emit(OpCodes.Pop);
					ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
					return;
				}
				if (cvdEntry != null && ((CLASS_VARIABLE_DECLARATION)cvdEntry.node).modifiers.Contains(Modifiers.PRIVATE)) {
					Report.Warn(210, classMemberName, line, column);
					ilGen.Emit(OpCodes.Pop);
					ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
					return;
				}
			}
			if (cvdEntry == null) {
				Report.Warn(209, classMemberName, line, column);
				ilGen.Emit(OpCodes.Pop);
				ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
				return;
			}
			CLASS_VARIABLE_DECLARATION cvd = (CLASS_VARIABLE_DECLARATION)cvdEntry.node;
			if (!cvd.modifiers.Contains(Modifiers.STATIC) && !cvd.modifiers.Contains(Modifiers.CONST)) {
				Report.Warn(208, classMemberName, line, column);
				ilGen.Emit(OpCodes.Pop);
				ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
				return;
			}
			int index = cvd.names.IndexOf(classMemberName);
			FieldBuilder fb = (FieldBuilder)cvd.fieldBuilders[index];
			ilGen.Emit(OpCodes.Stsfld, fb);
		}

		public void LoadFromStaticClassMember(string className, string classMemberName, int line, int column) {
			SymbolTableEntry cdEntry = SymbolTable.getInstance().lookupGlobal(className.ToLower(), SymbolTable.CLASS);
			CLASS_DECLARATION cd = (CLASS_DECLARATION)cdEntry.node;
			CLASS_DECLARATION tmpCD = cd;
			SymbolTableEntry cvdEntry = tmpCD.scope.lookup(classMemberName, SymbolTable.CLASS_VARIABLE);
			while (cvdEntry == null) {
				if (tmpCD.extends == null)
					break;
				SymbolTableEntry parentCdEntry = SymbolTable.getInstance().lookupGlobal(tmpCD.extends.ToLower(), SymbolTable.CLASS);
				tmpCD = (CLASS_DECLARATION)parentCdEntry.node;
				cvdEntry = tmpCD.scope.lookup(classMemberName, SymbolTable.CLASS_VARIABLE);
				if (cvdEntry != null && !((CLASS_VARIABLE_DECLARATION)cvdEntry.node).modifiers.Contains(Modifiers.STATIC) && !((CLASS_VARIABLE_DECLARATION)cvdEntry.node).modifiers.Contains(Modifiers.CONST)) {
					Report.Warn(208, classMemberName, line, column);
					ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
					return;
				}
				if (cvdEntry != null && ((CLASS_VARIABLE_DECLARATION)cvdEntry.node).modifiers.Contains(Modifiers.PRIVATE)) {
					Report.Warn(210, classMemberName, line, column);
					ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
					return;
				}
			}
			if (cvdEntry == null) {
				Report.Warn(209, classMemberName, line, column);
				ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
				return;
			}
			CLASS_VARIABLE_DECLARATION cvd = (CLASS_VARIABLE_DECLARATION)cvdEntry.node;
			if (!cvd.modifiers.Contains(Modifiers.STATIC) && !cvd.modifiers.Contains(Modifiers.CONST)) {
				Report.Warn(208, classMemberName, line, column);
				ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
				return;
			}
			int index = cvd.names.IndexOf(classMemberName);
			FieldBuilder fb = (FieldBuilder)cvd.fieldBuilders[index];
			ilGen.Emit(OpCodes.Ldsfld, fb);
			// if object is available on heap, load newly from there
			ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("Refresh", new Type[] { PHPMixed }));
		}

		public void InvokeStaticFunction(string className, string functionName, ExpressionList parameters, int line, int column) {
			SymbolTableEntry cdEntry = SymbolTable.getInstance().lookupGlobal(className.ToLower(), SymbolTable.CLASS);
			CLASS_DECLARATION cd = (CLASS_DECLARATION)cdEntry.node;
			CLASS_DECLARATION tmpCD = cd;
			SymbolTableEntry fdEntry = tmpCD.scope.lookup(functionName, SymbolTable.FUNCTION);
			while (fdEntry == null) {
				if (tmpCD.extends == null)
					break;
				SymbolTableEntry parentCdEntry = SymbolTable.getInstance().lookupGlobal(tmpCD.extends.ToLower(), SymbolTable.CLASS);
				tmpCD = (CLASS_DECLARATION)parentCdEntry.node;
				fdEntry = tmpCD.scope.lookup(functionName, SymbolTable.FUNCTION);
				if (fdEntry != null && !((FUNCTION_DECLARATION)fdEntry.node).modifiers.Contains(Modifiers.STATIC)) {
					Report.Error(215, functionName, line, column);
					ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
					return;
				}
				if (fdEntry != null && ((FUNCTION_DECLARATION)fdEntry.node).modifiers.Contains(Modifiers.PRIVATE)) {
					Report.Error(217, functionName, line, column);
					ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
					return;
				}
			}
			if (fdEntry == null) {
				Report.Error(216, functionName, line, column);
				ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
				return;
			}
			FUNCTION_DECLARATION fd = (FUNCTION_DECLARATION)fdEntry.node;
			if (!fd.modifiers.Contains(Modifiers.STATIC)) {
				Report.Error(215, functionName, line, column);
				ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
				return;
			}
			// pass parameters (only as many as needed)
			int parametersPassedActually = (int)Math.Min(fd.parameters.Count, parameters.Count());
			for (int i = 0; i < parametersPassedActually; i++) {
				PARAMETER_DECLARATION pd = (PARAMETER_DECLARATION)fd.parameters[i];
				Expression expr = (Expression)parameters.Get(i);
				Visit(expr);
				// clone if value semantics is desired
				if (!pd.by_reference && !(expr is REFERENCE))
					ilGen.Emit(OpCodes.Call, PHPCoreLang.GetMethod("Clone", new Type[] { PHPMixed }));
			}
			// if less parameters actually passed then necessary, pass Null objects instead
			for (int i = parametersPassedActually; i < fd.parameters.Count; i++) {
				PARAMETER_DECLARATION pd = (PARAMETER_DECLARATION)fd.parameters[i];
				if (pd.default_value == null)
					Report.Warn(300, System.Convert.ToString(i + 1), line, column);
				ilGen.Emit(OpCodes.Newobj, PHPNull.GetConstructor(Type.EmptyTypes));
			}
			// ensure parameters passed by reference are variables
			for (int i = 0; i < parametersPassedActually; i++) {
				PARAMETER_DECLARATION pd = (PARAMETER_DECLARATION)fd.parameters[i];
				Expression variableSupplied = (Expression)parameters.Get(i);
				if (pd.by_reference || variableSupplied is REFERENCE)
					if (!(variableSupplied is VARIABLE || variableSupplied is REFERENCE))
						Report.Error(301, System.Convert.ToString(i + 1), line, column);
			}
			// add function call to call trace
			ilGen.Emit(OpCodes.Ldstr, cd.name + "->" + fd.name);
			ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("AddFunctionCallToTrace", new Type[] { typeof(string) }));
			// call
			MethodBuilder mb = fd.mthBld;
			ilGen.Emit(OpCodes.Call, mb);
			// remove function call from call trace
			ilGen.Emit(OpCodes.Call, PHPCoreRuntime.GetMethod("RemoveFunctionCallFromTrace", Type.EmptyTypes));
		}

	}


}