using System;
using System.Reflection;
using ExpertCoder.Templates;

namespace MonoUML.Generators.SqlGeneration.TemplateTree
{
 	public class PrimaryKey: TraceableTemplate
	{
		public PrimaryKey(object relatedModelElement):
			base("Primarykey.txt", Assembly.GetExecutingAssembly(), _placeHolders, relatedModelElement)
		{
		}

		public string PrimaryKeyName
		{
			get { return _primaryKeyName; }
			set { _primaryKeyName = value; }
		}
		
		public string Type
		{
			get { return _type; }
			set { _type = value; }
		}
		
		public string SchemaName
		{
			get {return _schemaName; }
			set {_schemaName = value; }
		}
		public string TableName
		{
			get {return _tableName; }
			set {_tableName = value; }
		}
		protected override void SetPlaceHoldersValues()
		{
			base[_primaryKeyNamePH] = _primaryKeyName;
			base[_typePH] = _type;
			base[_schemaNamePH] = _schemaName;
			base[_tableNamePH] = _tableName;
		}
		
		private string _primaryKeyName;
		private string _type;
		private string _schemaName;
		private string _tableName;

		private static string _primaryKeyNamePH = "$PrimaryKey!";
		private static string _typePH = "$Type!";
		private static string _schemaNamePH = "$SchemaName!";
		private static string _tableNamePH = "$TableName!";
		
		private static string[] _placeHolders = new string[]
			{ _primaryKeyNamePH, _typePH, _schemaNamePH, _tableNamePH };
	}
}