using System;
using System.Collections.Specialized;
using ExpertCoder.Templates;
using UML = ExpertCoder.Uml2;
using TT = MonoUML.Generators.SqlGeneration.TemplateTree;

namespace MonoUML.Generators.SqlGeneration.TemplateTree
{
	public class Table: TraceableTemplate
	{
		
		public Table(object relatedModelElement):
			base("Table.txt",System.Reflection.Assembly.GetExecutingAssembly(),_placeHolders, relatedModelElement)
		{
			string nl = System.Environment.NewLine;
			_primaryKeys = new StringCollection( );
			//_primaryKeys.Tabs = 1;
			//_primaryKeys.FirstLineTabs = 0;
			_columnsName = new TemplateCollection(nl);
			_columnsName.Tabs = 1;
			_columnsName.FirstLineTabs = 0;
		}
		
		
		public bool IsAbstract
		{
			get { return _isAbstract; }
			set { _isAbstract = value; }
		}
		
		public bool IsNew
		{
			get { return _isNew; }
			set { _isNew = value; }
		}
		
		public bool IsSealed
		{
			get { return _isSealed; }
			set { _isSealed = value; }
		}
		

		public string SchemaName
		{
			get { return _schemaName; }
			set { _schemaName = value; }
		}
		
		public string TableName
		{
			get { return _tableName; }
			set { _tableName = value; }
		}
		
		public StringCollection PrimaryKeys
		{
			get { return _primaryKeys; }
		}
		
		public TemplateCollection ColumnsName
		{
			get { return _columnsName; }
		}
		
		protected override void SetPlaceHoldersValues()
		{
			base[_tableNamePH] = _tableName.ToString();
			string[] pks = new string[_primaryKeys.Count];
			_primaryKeys.CopyTo(pks, 0);
			base[_primaryKeysPH] = String.Join(", ", pks);
			string modifiers = _isAbstract ? "abstract " : String.Empty;
			modifiers += _isNew ? "new " : String.Empty;
			modifiers += _isSealed ? "sealed " : String.Empty;
			base[_schemaNamePH] = _schemaName.ToString();
			base[_columnsNamePH] = _columnsName.ToString();
		}
		
		private string _tableName;
		private bool _isAbstract;
		private bool _isNew;
		private bool _isSealed;
		private string _schemaName;
		private StringCollection _primaryKeys;
		private TemplateCollection _columnsName;

		private static string _tableNamePH = "$TableName!";
		private static string _primaryKeysPH = "$PrimaryKey*";		
		private static string _schemaNamePH = "$SchemaName!";		
		private static string _columnsNamePH = "$ColumsName*";		
		
		private static string[] _placeHolders = new string[]
		{
			_tableNamePH, _primaryKeysPH,_schemaNamePH, _columnsNamePH
			
		};
	}
}
