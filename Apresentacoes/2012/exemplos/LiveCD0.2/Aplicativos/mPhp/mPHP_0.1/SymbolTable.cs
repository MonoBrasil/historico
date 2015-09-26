using System;
using System.Collections;


namespace PHP.Core {


	public class SymbolTable {

		private static SymbolTable sym_tab;
		public SymbolTableScope cur_scope;

		public const int RESERVED_WORD = 0;
		public const int CLASS = 1;
		public const int CLASS_VARIABLE = 2;
		public const int FUNCTION = 4;

		public static SymbolTable getInstance() {
			if (sym_tab == null)
				sym_tab = new SymbolTable();
			return sym_tab;
		}

		private SymbolTable() {
			cur_scope = new SymbolTableScope(null);
			// insert reserved words
			insertGlobal("__MAIN", RESERVED_WORD);
			insertGlobal("and", RESERVED_WORD);
			insertGlobal("or", RESERVED_WORD);
			insertGlobal("xor", RESERVED_WORD);
			insertGlobal("__FILE__", RESERVED_WORD);
			insertGlobal("exception", RESERVED_WORD);
			insertGlobal("__LINE__", RESERVED_WORD);
			insertGlobal("array", RESERVED_WORD);
			insertGlobal("as", RESERVED_WORD);
			insertGlobal("break", RESERVED_WORD);
			insertGlobal("case", RESERVED_WORD);
			insertGlobal("class", RESERVED_WORD);
			insertGlobal("const", RESERVED_WORD);
			insertGlobal("continue", RESERVED_WORD);
			insertGlobal("declare", RESERVED_WORD);
			insertGlobal("default", RESERVED_WORD);
			insertGlobal("die", RESERVED_WORD);
			insertGlobal("do", RESERVED_WORD);
			insertGlobal("echo", RESERVED_WORD);
			insertGlobal("else", RESERVED_WORD);
			insertGlobal("elseif", RESERVED_WORD);
			insertGlobal("empty", RESERVED_WORD);
			insertGlobal("enddeclare", RESERVED_WORD);
			insertGlobal("endfor", RESERVED_WORD);
			insertGlobal("endforeach", RESERVED_WORD);
			insertGlobal("endif", RESERVED_WORD);
			insertGlobal("endswitch", RESERVED_WORD);
			insertGlobal("endwhile", RESERVED_WORD);
			insertGlobal("eval", RESERVED_WORD);
			insertGlobal("exit", RESERVED_WORD);
			insertGlobal("for", RESERVED_WORD);
			insertGlobal("foreach", RESERVED_WORD);
			insertGlobal("function", RESERVED_WORD);
			insertGlobal("global", RESERVED_WORD);
			insertGlobal("if", RESERVED_WORD);
			insertGlobal("include", RESERVED_WORD);
			insertGlobal("include_once", RESERVED_WORD);
			insertGlobal("isset", RESERVED_WORD);
			insertGlobal("list", RESERVED_WORD);
			insertGlobal("new", RESERVED_WORD);
			insertGlobal("print", RESERVED_WORD);
			insertGlobal("require", RESERVED_WORD);
			insertGlobal("require_once", RESERVED_WORD);
			insertGlobal("return", RESERVED_WORD);
			insertGlobal("static", RESERVED_WORD);
			insertGlobal("switch", RESERVED_WORD);
			insertGlobal("unset", RESERVED_WORD);
			insertGlobal("use", RESERVED_WORD);
			insertGlobal("var", RESERVED_WORD);
			insertGlobal("while", RESERVED_WORD);
			insertGlobal("__FUNCTION__", RESERVED_WORD);
			insertGlobal("__CLASS__", RESERVED_WORD);
			insertGlobal("__METHOD__", RESERVED_WORD);
			insertGlobal("final", RESERVED_WORD);
			insertGlobal("php_user_filter", RESERVED_WORD);
			insertGlobal("interface", RESERVED_WORD);
			insertGlobal("implements", RESERVED_WORD);
			insertGlobal("extends", RESERVED_WORD);
			insertGlobal("public", RESERVED_WORD);
			insertGlobal("private", RESERVED_WORD);
			insertGlobal("protected", RESERVED_WORD);
			insertGlobal("abstract", RESERVED_WORD);
			insertGlobal("clone", RESERVED_WORD);
			insertGlobal("try", RESERVED_WORD);
			insertGlobal("catch", RESERVED_WORD);
			insertGlobal("throw", RESERVED_WORD);
		}

		public void openScope() {
			SymbolTableScope new_scope = new SymbolTableScope(cur_scope);
			cur_scope = new_scope;
		}

		public void closeScope() {
			cur_scope = cur_scope.parent;
		}

		public void insertLocal(string name, int kind) {
			insertLocal(name, kind, null);
		}

		public void insertLocal(string name, int kind, ASTNode node) {
			cur_scope.insert(name, kind, node);
		}

		public void insertGlobal(string name, int kind) {
			insertGlobal(name, kind, null);
		}

		public void insertGlobal(string name, int kind, ASTNode node) {
			getTopScope().insert(name, kind, node);
		}

		public SymbolTableEntry lookup(string name, int kind) {
			return cur_scope.lookup(name, kind);
		}

		public SymbolTableEntry lookupGlobal(string name, int kind) {
			return getTopScope().lookup(name, kind);
		}

		public static void reset() {
			sym_tab = new SymbolTable();
		}

		private SymbolTableScope getTopScope() {
			SymbolTableScope tmp_scope = cur_scope;
			while (tmp_scope.parent != null)
				tmp_scope = tmp_scope.parent;
			return tmp_scope;
		}

	}


	public class SymbolTableScope {

		public SymbolTableScope parent;
		public Hashtable entries;

		public ArrayList classMembers;
		public ArrayList globalVariables;

		public SymbolTableScope(SymbolTableScope parent) {
			this.parent = parent;
			entries = new Hashtable();
			classMembers = new ArrayList();
			globalVariables = new ArrayList();
		}

		public void insert(string name, int kind) {
			insert(name, kind, null);
		}

		public void insert(string name, int kind, ASTNode node) {
			SymbolTableEntry entry = new SymbolTableEntry(name, kind, node);
			// a new class member?
			if (kind == SymbolTable.CLASS_VARIABLE) {
				if (classMembers.Contains(name))
					Report.Error(306, name);
				classMembers.Add(name);
			}
			// no symbol with this name exists, so add it
			if (entries[name] == null) {
				Hashtable value = new Hashtable();
				value[kind] = entry;
				entries[name] = value;
			}
			// a symbol with this name already exists
			else {
				Hashtable value = (Hashtable)entries[name];
				// but with another kind, so add it
				if (value[kind] == null)
					value[kind] = entry;
				// with kind reserved word, class, class variable or function, so report error
				else
					switch (kind) {
						case SymbolTable.RESERVED_WORD: Report.Error(201, name, node.line, node.column); break;
						case SymbolTable.CLASS: Report.Error(202, name, node.line, node.column); break;
						case SymbolTable.CLASS_VARIABLE: Report.Error(204, name, node.line, node.column); break;
						case SymbolTable.FUNCTION: Report.Error(211, name, node.line, node.column); break;
					}
			}

		}

		public SymbolTableEntry lookup(string name, int kind) {
			Hashtable entry = (Hashtable)entries[name];
			// no entry with this name and kind found in current scope
			if (entry == null || entry[kind] == null) {
				// no parent scope, so we are at top scope
				if (parent == null)
					return null;
				// parent scope available, so lookup there
				else
					return parent.lookup(name, kind);
			}
			// entry found with matching kind
			else
				return (SymbolTableEntry)entry[kind];
		}

		public ArrayList lookup(int kind) {
			ArrayList result = new ArrayList();
			// check all entries
			foreach (Hashtable entry in entries.Values) {
				// if entry contains specified kind, add to result
				if (entry[kind] != null)
					result.Add((SymbolTableEntry)entry[kind]);
			}
			// done
			return result;
		}

	}


	public class SymbolTableEntry {

		public string name;
		public int kind;
		public ASTNode node;

		public SymbolTableEntry(string name, int kind, ASTNode node) {
			this.name = name;
			this.kind = kind;
			this.node = node;
		}

	}


}