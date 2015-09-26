using System;
using System.Reflection;
using ExpertCoder.Templates;

namespace MonoUML.Generators.SqlGeneration.TemplateTree
{
 	public class Column: TraceableTemplate
	{
		public Column(object relatedModelElement):
			base("Column.txt", Assembly.GetExecutingAssembly(), _placeHolders, relatedModelElement)
		{
		}

		public string ColumnName
		{
			get { return _columnName; } 
			set { _columnName = value; }
		}
		
		public string Type
		{
			get { return _type; }
			set { _type = value; }
		}
		
		public string MoreColumn
		{
			get {return _moreColumn;}
			set {_moreColumn = value;}
		}
		
		protected override void SetPlaceHoldersValues()
		{
			base[_columnNamePH] = _columnName;
			base[_typePH] = _type;
			base[_moreColumnPH] = _moreColumn;
		}
		
		private string _columnName;
		private string _type;
		private string _moreColumn;

		private static string _columnNamePH = "$Column!";
		private static string _typePH = "$Type!";
		private static string _moreColumnPH = "$MoreColumn!";
		
		private static string[] _placeHolders = new string[]
			{ _columnNamePH, _typePH,_moreColumnPH };
	}
}