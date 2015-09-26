using System;
using System.Collections;
using System.Reflection.Emit;


namespace PHP.Core {


	public class AST {
		public StatementList stmt_list;
		public AST(StatementList stmt_list) {
			this.stmt_list = stmt_list;
		}
	}

	public abstract class ASTNode {
		public int line;
		public int column;
		public ASTNode(int line, int column) {
			this.line = line;
			this.column = column;
		}
	}

	public abstract class Statement : ASTNode {
		public Statement(int line, int column)
			: base(line, column) { }
	}

	public class CLASS_DECLARATION : Statement {
		public int modifier;
		public string name;
		public string extends;
		public ArrayList implements;
		public StatementList stmt_list;
		public SymbolTableScope scope;
		public TypeBuilder typBld;
		public Type typ;
		public CLASS_DECLARATION(int modifier, string name, string extends, StatementList stmt_list, int line, int column)
			: base(line, column) {
			this.modifier = modifier;
			this.name = name;
			this.extends = extends;
			this.stmt_list = stmt_list;
		}
	}

	public class CLASS_VARIABLE_DECLARATION : Statement {
		public ArrayList modifiers;
		public ArrayList names;
		public ExpressionList values;
		public ArrayList fieldBuilders;
		public CLASS_VARIABLE_DECLARATION(ArrayList modifiers, ArrayList names, ExpressionList values, int line, int column)
			: base(line, column) {
			this.modifiers = modifiers;
			this.names = names;
			this.values = values;
		}
	}

	public class FUNCTION_DECLARATION : Statement {
		public ArrayList modifiers;
		public bool return_by_reference;
		public string name;
		public ArrayList parameters;
		public StatementList stmt_list;
		public SymbolTableScope scope;
		public ConstructorBuilder ctrBld;
		public MethodBuilder mthBld;
		public FUNCTION_DECLARATION(ArrayList modifiers, bool return_by_reference, string name, ArrayList parameters, StatementList stmt_list, int line, int column)
			: base(line, column) {
			this.modifiers = modifiers;
			this.return_by_reference = return_by_reference;
			this.name = name;
			this.parameters = parameters;
			this.stmt_list = stmt_list;
		}
	}

	public class PARAMETER_DECLARATION : ASTNode {
		public string type;
		public bool by_reference;
		public string name;
		public Expression default_value;
		public PARAMETER_DECLARATION(string type, bool by_reference, string name, Expression default_value, int line, int column)
			: base(line, column) {
			this.type = type;
			this.by_reference = by_reference;
			this.name = name;
			this.default_value = default_value;
		}
	}

	public class GLOBAL : Statement {
		public ExpressionList var_list;
		public GLOBAL(ExpressionList var_list, int line, int column)
			: base(line, column) {
			this.var_list = var_list;
		}
	}

	public class STATIC_DECLARATION : Statement {
		public ExpressionList expr_list;
		public STATIC_DECLARATION(ExpressionList expr_list, int line, int column)
			: base(line, column) {
			this.expr_list = expr_list;
		}
	}

	public class ECHO : Statement {
		public ExpressionList expr_list;
		public ECHO(ExpressionList expr_list, int line, int column)
			: base(line, column) {
			this.expr_list = expr_list;
		}
	}

	public class BLOCK : Statement {
		public StatementList stmt_list;
		public BLOCK(StatementList stmt_list, int line, int column)
			: base(line, column) {
			this.stmt_list = stmt_list;
		}
	}

	public class IF : Statement {
		public Expression expr;
		public Statement stmt;
		public ArrayList elseif_list;
		public Statement else_stmt;
		public IF(Expression expr, Statement stmt, ArrayList elseif_list, Statement else_stmt, int line, int column)
			: base(line, column) {
			this.expr = expr;
			this.stmt = stmt;
			this.elseif_list = elseif_list;
			this.else_stmt = else_stmt;
		}
	}

	public class ELSEIF : ASTNode {
		public Expression expr;
		public Statement stmt;
		public ELSEIF(Expression expr, Statement stmt, int line, int column)
			: base(line, column) {
			this.expr = expr;
			this.stmt = stmt;
		}
	}

	public class WHILE : Statement {
		public Expression expr;
		public Statement stmt;
		public WHILE(Expression expr, Statement stmt, int line, int column)
			: base(line, column) {
			this.expr = expr;
			this.stmt = stmt;
		}
	}

	public class DO : Statement {
		public Statement stmt;
		public Expression expr;
		public DO(Statement stmt, Expression expr, int line, int column)
			: base(line, column) {
			this.stmt = stmt;
			this.expr = expr;
		}
	}

	public class FOR : Statement {
		public ExpressionList expr_list1;
		public ExpressionList expr_list2;
		public ExpressionList expr_list3;
		public Statement stmt;
		public FOR(ExpressionList expr_list1, ExpressionList expr_list2, ExpressionList expr_list3, Statement stmt, int line, int column)
			: base(line, column) {
			this.expr_list1 = expr_list1;
			this.expr_list2 = expr_list2;
			this.expr_list3 = expr_list3;
			this.stmt = stmt;
		}
	}

	public class FOREACH : Statement {
		public Expression array;
		public Expression key;
		public Expression value;
		public Statement stmt;
		public FOREACH(Expression array, Expression key, Expression value, Statement stmt, int line, int column)
			: base(line, column) {
			this.array = array;
			this.key = key;
			this.value = value;
			this.stmt = stmt;
		}
	}

	public class SWITCH : Statement {
		public Expression expr;
		public ArrayList switch_case_list;
		public SWITCH(Expression expr, ArrayList switch_case_list, int line, int column)
			: base(line, column) {
			this.expr = expr;
			this.switch_case_list = switch_case_list;
		}
	}

	public class CASE : ASTNode {
		public Expression expr;
		public Statement stmt;
		public CASE(Expression expr, Statement stmt, int line, int column)
			: base(line, column) {
			this.expr = expr;
			this.stmt = stmt;
		}
	}

	public class DEFAULT : ASTNode {
		public Statement stmt;
		public DEFAULT(Statement stmt, int line, int column)
			: base(line, column) {
			this.stmt = stmt;
		}
	}

	public class BREAK : Statement {
		public Expression expr;
		public BREAK(Expression expr, int line, int column)
			: base(line, column) {
			this.expr = expr;
		}
	}

	public class CONTINUE : Statement {
		public Expression expr;
		public CONTINUE(Expression expr, int line, int column)
			: base(line, column) {
			this.expr = expr;
		}
	}

	public class RETURN : Statement {
		public Expression expr;
		public RETURN(Expression expr, int line, int column)
			: base(line, column) {
			this.expr = expr;
		}
	}

	public class UNSET : Statement {
		public ExpressionList var_list;
		public UNSET(ExpressionList var_list, int line, int column)
			: base(line, column) {
			this.var_list = var_list;
		}
	}

	public class EXPRESSION_AS_STATEMENT : Statement {
		public Expression expr;
		public EXPRESSION_AS_STATEMENT(Expression expr, int line, int column)
			: base(line, column) {
			this.expr = expr;
		}
	}

	public abstract class Expression : ASTNode {
		public Expression(int line, int column) : base(line, column) { }
	}

	public abstract class UnaryExpression : Expression {
		public Expression expr;
		public UnaryExpression(Expression expr, int line, int column)
			: base(line, column) {
			this.expr = expr;
		}
	}

	public abstract class BinaryExpression : Expression {
		public Expression expr1;
		public Expression expr2;
		public BinaryExpression(Expression expr1, Expression expr2, int line, int column)
			: base(line, column) {
			this.expr1 = expr1;
			this.expr2 = expr2;
		}
	}

	public abstract class TernaryExpression : Expression {
		public Expression expr1;
		public Expression expr2;
		public Expression expr3;
		public TernaryExpression(Expression expr1, Expression expr2, Expression expr3, int line, int column)
			: base(line, column) {
			this.expr1 = expr1;
			this.expr2 = expr2;
			this.expr3 = expr3;
		}
	}

	public class VARIABLE : Expression {
		public string name;
		public OFFSET offset;
		public VARIABLE(string name, int line, int column) : this(name, null, line, column) { }
		public VARIABLE(string name, OFFSET offset, int line, int column)
			: base(line, column) {
			this.name = name;
			this.offset = offset;
		}
	}

	public class REFERENCE : Expression {
		public Expression variable;
		public REFERENCE(Expression variable, int line, int column)
			: base(line, column) {
			this.variable = variable;
		}
	}

	public class FUNCTION_CALL : Expression {
		public string function_name;
		public ExpressionList parameters;
		public FUNCTION_CALL(string function_name, ExpressionList parameters, int line, int column)
			: base(line, column) {
			this.function_name = function_name;
			this.parameters = parameters;
		}
	}

	public class NEW : Expression {
		internal string type;
		internal ExpressionList ctor_args;
		public NEW(string type, ExpressionList ctor_args, int line, int column)
			: base(line, column) {
			this.type = type;
			this.ctor_args = ctor_args;
		}
	}

	public class INSTANCEOF : Expression {
		internal Expression expr;
		internal string type;
		public INSTANCEOF(Expression expr, string type, int line, int column)
			: base(line, column) {
			this.expr = expr;
			this.type = type;
		}
	}

	public class ARRAY : Expression {
		public ArrayList array_pair_list;
		public ARRAY(ArrayList array_pair_list, int line, int column)
			: base(line, column) {
			this.array_pair_list = array_pair_list;
		}
	}

	public class ARRAY_PAIR : ASTNode {
		public Expression key;
		public Expression value;
		public ARRAY_PAIR(Expression key, Expression value, int line, int column)
			: base(line, column) {
			this.key = key;
			this.value = value;
		}
	}

	public class OFFSET : ASTNode {
		public const int SQUARE = 0;
		public const int CURLY = 1;
		public int kind;
		public Expression value;
		public OFFSET(int kind, Expression value, int line, int column)
			: base(line, column) {
			this.kind = kind;
			this.value = value;
		}
	}

	public class INC : UnaryExpression {
		public int kind;
		public INC(Expression expr, int kind, int line, int column)
			: base(expr, line, column) {
			this.kind = kind;
		}
	}

	public class DEC : UnaryExpression {
		public int kind;
		public DEC(Expression expr, int kind, int line, int column) : base(expr, line, column) {
			this.kind = kind;
		}
	}

	public class BOOLEAN_NOT : UnaryExpression {
		public BOOLEAN_NOT(Expression expr, int line, int column) : base(expr, line, column) { }
	}

	public class NOT : UnaryExpression {
		public NOT(Expression expr, int line, int column) : base(expr, line, column) { }
	}

	public class EXIT : UnaryExpression {
		public EXIT(Expression expr, int line, int column) : base(expr, line, column) { }
	}

	public class PRINT : UnaryExpression {
		public PRINT(Expression expr, int line, int column) : base(expr, line, column) { }
	}

	public class BOOL_CAST : UnaryExpression {
		public BOOL_CAST(Expression expr, int line, int column) : base(expr, line, column) { }
	}

	public class INT_CAST : UnaryExpression {
		public INT_CAST(Expression expr, int line, int column) : base(expr, line, column) { }
	}

	public class DOUBLE_CAST : UnaryExpression {
		public DOUBLE_CAST(Expression expr, int line, int column) : base(expr, line, column) { }
	}

	public class STRING_CAST : UnaryExpression {
		public STRING_CAST(Expression expr, int line, int column) : base(expr, line, column) { }
	}

	public class ARRAY_CAST : UnaryExpression {
		public ARRAY_CAST(Expression expr, int line, int column) : base(expr, line, column) { }
	}

	public class OBJECT_CAST : UnaryExpression {
		public OBJECT_CAST(Expression expr, int line, int column) : base(expr, line, column) { }
	}

	public class CLONE : UnaryExpression {
		public CLONE(Expression expr, int line, int column) : base(expr, line, column) { }
	}

	public class PAAMAYIM_NEKUDOTAYIM : UnaryExpression {
		public string type;
		public PAAMAYIM_NEKUDOTAYIM(string type, Expression expr, int line, int column) : base(expr, line, column) {
			this.type = type;
		}
	}

	public class OBJECT_OPERATOR : BinaryExpression {
		public OBJECT_OPERATOR(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class EQUALS : BinaryExpression {
		public EQUALS(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class PLUS_EQUAL : BinaryExpression {
		public PLUS_EQUAL(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class MINUS_EQUAL : BinaryExpression {
		public MINUS_EQUAL(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class MUL_EQUAL : BinaryExpression {
		public MUL_EQUAL(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class DIV_EQUAL : BinaryExpression {
		public DIV_EQUAL(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class CONCAT_EQUAL : BinaryExpression {
		public CONCAT_EQUAL(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class MOD_EQUAL : BinaryExpression {
		public MOD_EQUAL(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class AND_EQUAL : BinaryExpression {
		public AND_EQUAL(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class OR_EQUAL : BinaryExpression {
		public OR_EQUAL(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class XOR_EQUAL : BinaryExpression {
		public XOR_EQUAL(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class SL_EQUAL : BinaryExpression {
		public SL_EQUAL(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class SR_EQUAL : BinaryExpression {
		public SR_EQUAL(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class BOOLEAN_OR : BinaryExpression {
		public BOOLEAN_OR(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class BOOLEAN_AND : BinaryExpression {
		public BOOLEAN_AND(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class LOGICAL_OR : BinaryExpression {
		public LOGICAL_OR(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class LOGICAL_AND : BinaryExpression {
		public LOGICAL_AND(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class LOGICAL_XOR : BinaryExpression {
		public LOGICAL_XOR(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class CONCAT : BinaryExpression {
		public CONCAT(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class PLUS : BinaryExpression {
		public PLUS(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class MINUS : BinaryExpression {
		public MINUS(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class TIMES : BinaryExpression {
		public TIMES(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class DIV : BinaryExpression {
		public DIV(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class MOD : BinaryExpression {
		public MOD(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class AND : BinaryExpression {
		public AND(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class OR : BinaryExpression {
		public OR(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class XOR : BinaryExpression {
		public XOR(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class SL : BinaryExpression {
		public SL(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class SR : BinaryExpression {
		public SR(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class IS_EQUAL : BinaryExpression {
		public IS_EQUAL(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class IS_NOT_EQUAL : BinaryExpression {
		public IS_NOT_EQUAL(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class IS_IDENTICAL : BinaryExpression {
		public IS_IDENTICAL(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class IS_NOT_IDENTICAL : BinaryExpression {
		public IS_NOT_IDENTICAL(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class LOWER : BinaryExpression {
		public LOWER(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class IS_LOWER_OR_EQUAL : BinaryExpression {
		public IS_LOWER_OR_EQUAL(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class GREATER : BinaryExpression {
		public GREATER(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class IS_GREATER_OR_EQUAL : BinaryExpression {
		public IS_GREATER_OR_EQUAL(Expression expr1, Expression expr2, int line, int column) : base(expr1, expr2, line, column) { }
	}

	public class IF_EXPR : TernaryExpression {
		public IF_EXPR(Expression expr1, Expression expr2, Expression expr3, int line, int column) : base(expr1, expr2, expr3, line, column) { }
	}

	public abstract class SCALAR : Expression {
		public SCALAR(int line, int column) : base(line, column) { }
	}

	public class LNUMBER_SCALAR : SCALAR {
		public int value;
		public LNUMBER_SCALAR(int value, int line, int column)
			: base(line, column) {
			this.value = value;
		}
	}

	public class DNUMBER_SCALAR : SCALAR {
		public double value;
		public DNUMBER_SCALAR(double value, int line, int column)
			: base(line, column) {
			this.value = value;
		}
	}

	public class STRING_SCALAR : SCALAR {
		public string value;
		public STRING_SCALAR(string value, int line, int column)
			: base(line, column) {
			this.value = value;
		}
	}

	public class SINGLE_QUOTES : SCALAR {
		public ArrayList encaps_list;
		public SINGLE_QUOTES(ArrayList encaps_list, int line, int column)
			: base(line, column) {
			this.encaps_list = encaps_list;
		}
	}

	public class DOUBLE_QUOTES : SCALAR {
		public ArrayList encaps_list;
		public DOUBLE_QUOTES(ArrayList encaps_list, int line, int column)
			: base(line, column) {
			this.encaps_list = encaps_list;
		}
	}

	public class HEREDOC : SCALAR {
		public ArrayList encaps_list;
		public HEREDOC(ArrayList encaps_list, int line, int column)
			: base(line, column) {
			this.encaps_list = encaps_list;
		}
	}

	public class CONSTANT : SCALAR {
		public string name;
		public CONSTANT(string name, int line, int column)
			: base(line, column) {
			this.name = name;
		}
	}


}