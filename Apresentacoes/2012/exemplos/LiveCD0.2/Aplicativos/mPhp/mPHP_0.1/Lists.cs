using System;
using System.Collections;
//using System.Collections.Generic;


namespace PHP.Core {


	public class StatementList : Statement {

		private ArrayList list;

		public StatementList() : base(0, 0) {
			list = new ArrayList();
		}

		public StatementList(Statement s)
			: this() {
			list.Add(s);
		}

		public void Add(Statement s) {
			list.Add(s);
		}

		public void AddRange(StatementList stmt_list) {
			foreach (Statement stmt in stmt_list)
				list.Add(stmt);
		}

		public void Remove(Statement stmt) {
			list.Remove(stmt);
		}

		public void RemoveRange(StatementList stmt_list) {
			foreach (Statement stmt in stmt_list)
				list.Remove(stmt);
		}

		public IEnumerator GetEnumerator() {
			return list.GetEnumerator();
		}

		public int Count() {
			return list.Count;
		}

		public Statement Get(int i) {
			object result = list[i];
			if (result == null)
				return null;
			else
				return (Statement)result;
		}

	}



	public class ExpressionList {

		private ArrayList list;

		public ExpressionList() {
			list = new ArrayList();
		}

		public ExpressionList(Expression e)
			: this() {
			list.Add(e);
		}

		public void Add(Expression e) {
			list.Add(e);
		}

		public void AddRange(ExpressionList expr_list) {
			foreach (Expression expr in expr_list)
				list.Add(expr);
		}

		public void Remove(Expression expr) {
			list.Remove(expr);
		}

		public void RemoveRange(ExpressionList expr_list) {
			foreach (Expression expr in expr_list)
				list.Remove(expr);
		}

		public IEnumerator GetEnumerator() {
			return list.GetEnumerator();
		}

		public int Count() {
			return list.Count;
		}

		public Expression Get(int i) {
			object result = list[i];
			if (result == null)
				return null;
			else
				return (Expression)result;
		}

	}


}