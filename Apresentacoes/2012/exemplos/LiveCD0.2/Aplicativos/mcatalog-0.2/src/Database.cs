/*
 * Copyright (C) 2004 Cesar Garcia Tapia <tapia@mcatalog.net>
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as
 * published by the Free Software Foundation; either version 2 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public
 * License along with this program; if not, write to the
 * Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 */

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Reflection;
using Mono.Data.SqliteClient;

using System.Collections;
using System.Collections.Specialized;

using Gdk;
using Mono.Posix;

// Database tables are defined in Database.sql

public class Database
{
	SqliteConnection connection;
	SqliteCommand command;
	string dbVersion;

	bool debug;

	public Database (string file)
	{
		bool create = false;
		if (!File.Exists (file)) {
			create = true;
		}

		try {
			StreamReader srVersion = new StreamReader (file+".version");
			dbVersion = srVersion.ReadToEnd ();
			if (dbVersion != null) {
				dbVersion = dbVersion.Trim();
			}
			srVersion.Close();
		}
		catch {
			dbVersion = null;
		}

		string connectionString = "URI=file:"+file;
		connection = new SqliteConnection(connectionString);
		connection.Open();
		command = connection.CreateCommand();

		if (create) {
			Conf.EmptyCache();
			Assembly thisAssembly = Assembly.GetEntryAssembly ();
			Stream stream = thisAssembly.GetManifestResourceStream("Database.sql");
			if (stream != null) {
				StreamReader sr = new StreamReader (stream);
				string sql = sr.ReadToEnd();
				command.CommandText = sql;
				command.ExecuteNonQuery();

				StreamWriter swVersion = new StreamWriter (file+".version", false);
				swVersion.Write (Defines.VERSION);
				swVersion.Close();

				dbVersion = Defines.VERSION;
			}
			else {
				System.Console.WriteLine("Error creating the database");
			}
		}

		if (dbVersion == null || !dbVersion.Equals (Defines.VERSION)) {
			UpdateDatabase(file, dbVersion);
		}
	}

	public void Close ()
	{
		command.Dispose();
		connection.Close();
	}

	public bool Debug {
		get {
			return debug;
		}
		set {
			debug = value;
		}
	}

	private static string SqlString (string s)
	{
		return s.Replace ("'", "''");
	}

	public ArrayList Catalogs ()
	{
		if (debug)
			Console.WriteLine ("DB:Catalogs");
		ArrayList array = new ArrayList();

		command.CommandText = "SELECT * FROM catalogs ORDER BY weight";
		IDataReader reader = command.ExecuteReader();
		while(reader.Read()) {
			Catalog catalog = new Catalog (this,
					reader["name"].ToString(),
					reader["table_name"].ToString(),
					reader["short_description"].ToString(),
					reader["long_description"].ToString(),
					reader["image"].ToString(),
					Int32.Parse(reader["weight"].ToString()));
			array.Add (catalog);
		}
		reader.Close();

		return array;
	}

	public View GetCatalogView (string catalog)
	{
		if (debug)
			Console.WriteLine ("DB:GetCatalogView: "+catalog);
		try {
			string sql = "SELECT view FROM catalogs WHERE name='"+catalog+"'";
			command.CommandText = sql;
			IDataReader reader = command.ExecuteReader();
			reader.Read();
			return (View)Int32.Parse (reader["view"].ToString());
		}
		catch {
			return (View)1;
		}
	}

	public void SetCatalogView (string catalog, View view)
	{
		if (debug)
			Console.WriteLine ("DB:SetCatalogView "+catalog);
		string sql = "UPDATE catalogs SET view="+(int)view+" WHERE name='"+catalog+"'";
		command.CommandText = sql;
		command.ExecuteNonQuery();
	}

	public Order GetCatalogOrder (string catalog)
	{
		if (debug)
			Console.WriteLine ("DB:GetCatalogOrder: "+catalog);
		string sql ="SELECT order_by, sort FROM catalogs WHERE name='"+catalog+"'";
		command.CommandText = sql;
		IDataReader reader = command.ExecuteReader();
		reader.Read();
		Order order = new Order ();
		order.Field = reader["order_by"].ToString();
		order.Ascending = reader["sort"].ToString().Equals("ascending")?true:false;

		return order;
	}
	
	public void SetCatalogOrder (string catalog, Order order)
	{
		if (debug)
			Console.WriteLine ("DB:SetCatalogOrder "+catalog);
		string sort;
		if (order.Ascending) {
			sort = "ascending";
		}
		else {
			sort = "descending";
		}
		string sql = "UPDATE catalogs SET ";
		
		if (order.Field != null && !order.Field.Trim().Equals("")) {
			sql += "order_by='"+order.Field+"', ";
		}
		
		sql += "sort='"+sort+"' WHERE name='"+catalog+"'";

		command.CommandText = sql;
		command.ExecuteNonQuery();
	}

	public double GetCatalogZoom (string catalog)
	{
		if (debug)
			Console.WriteLine ("DB:GetCatalogZoom: "+catalog);
		try {
			string sql = "SELECT zoom FROM catalogs WHERE name='"+catalog+"'";
			command.CommandText = sql;
			IDataReader reader = command.ExecuteReader();
			reader.Read();
			return Double.Parse (reader["zoom"].ToString(), System.Globalization.NumberFormatInfo.InvariantInfo);
		}
		catch {
			return (double)1;
		}
	}

	public void SetCatalogZoom (string catalog, double zoom)
	{
		if (debug)
			Console.WriteLine ("DB:SetCatalogZoom: "+catalog);
		string zoomString = zoom.ToString (System.Globalization.NumberFormatInfo.InvariantInfo);
		string sql = "UPDATE catalogs SET zoom="+zoomString+" WHERE name='"+catalog+"'";
		command.CommandText = sql;
		command.ExecuteNonQuery();
	}
	
	public ItemList LoadItemList (Catalog catalog, Presentation presentation)
	{
		if (debug)
			Console.WriteLine ("DB:LoadItemList");
		ListDictionary columns = GetColumns (catalog.Table);
		Hashtable columnsToShow = GetColumnsToShow (catalog.Table);
		ItemList list = new ItemList (catalog, columns, columnsToShow, presentation);
		return list;
	}

	public ArrayList GetItems (string table, string catalog)
	{
		if (debug)
			Console.WriteLine ("DB:GetItems: "+catalog);
		Order order = this.GetCatalogOrder (catalog);
		ArrayList list = new ArrayList ();
		
		string sql = "SELECT * FROM "+table;
		
		if (order.Field!=null && !order.Field.Trim().Equals("")) {
			sql+=" ORDER BY "+order.Field;
			if (order.Ascending) {
				sql += " ASC";
			}
			else {
				sql += " DESC";
			}
		}
		
		command.CommandText = sql;
		IDataReader reader = command.ExecuteReader();

		while (reader.Read()) {
			ListDictionary columns = new ListDictionary ();
			int i = 0;
			while (i < reader.FieldCount) {
				string campo = reader.GetName(i).ToString();
				if (reader[i] != null) {
					columns.Add (campo, reader[i].ToString());
				}
				else {
					columns.Add (campo, null);
				}
				i++;
			}
			list.Add (new Item (table, columns, false));
		}

		reader.Close();

		return list;
	}

	public ArrayList Search (string searchString, string table)
	{
		return Search (searchString, table, null);
	}

	public ArrayList Search (string searchString, string table, string field)
	{
		if (debug)
			Console.WriteLine ("DB:Search");
		StringBuilder sql = new StringBuilder ();
		sql = sql.Append ("SELECT * FROM ");
		sql = sql.Append (table);
		sql = sql.Append (" WHERE ");
		
		ListDictionary columnNames = this.GetColumns (table);
		int i = 0;
		foreach (string s in columnNames.Keys) {
			if (i++!=0) {
				sql = sql.Append (" OR ");
			}

			sql = sql.Append (s);
			sql = sql.Append (" LIKE '%");
			sql = sql.Append (SqlString(searchString));
			sql = sql.Append ("%'");
		}

		if (field!=null)
			sql = sql.Append (" ORDER BY "+field);

		command.CommandText = sql.ToString();
		IDataReader reader = command.ExecuteReader();

		ArrayList list = new ArrayList ();
		while (reader.Read()) {
			ListDictionary columns = new ListDictionary ();
			i = 0;
			while (i < reader.FieldCount) {
				string campo = reader.GetName(i).ToString();
				if (reader[i] != null) {
					columns.Add (campo, reader[i].ToString());
				}
				else {
					columns.Add (campo, null);
				}
				i++;
			}
			list.Add (new Item (table, columns, false));
		}

		reader.Close();

		return list;
	}
	
	public ArrayList GetBorrowers ()
	{
		if (debug)
			Console.WriteLine ("DB:GetBorrowers");
		string sql = "SELECT * FROM borrowers";
		command.CommandText = sql;

		SqliteDataReader reader = command.ExecuteReader();
		
		Hashtable names = new Hashtable();
		while (reader.Read()) {
			names.Add (Int32.Parse(reader["id"].ToString()), reader["name"].ToString());
		}
		reader.Close();
		
		ArrayList list = new ArrayList ();
		foreach (int id in names.Keys) {
			sql = "SELECT * FROM lends WHERE borrower="+id;
			command.CommandText = sql;
			reader = command.ExecuteReader();
			
			ArrayList items = new ArrayList ();
			while (reader.Read()) {
				items.Add (Int32.Parse(reader["id"].ToString()));
			}
			list.Add (new Borrower (id, (string)names[id], items));
			reader.Close();
		}
		
		return list;
	}

	public Item GetBorrowerItem (int id)
	{
		if (debug)
			Console.WriteLine ("DB:GetBorrowerItem");
		string sql = "SELECT * FROM lends WHERE id="+id;
		command.CommandText = sql;
		SqliteDataReader reader = command.ExecuteReader();
		if (reader.Read()) {
			if (reader["item_id"] != null && !reader["item_id"].ToString().Equals("")) {
				string table = (string)reader["table_name"];
				int itemId = Int32.Parse(reader["item_id"].ToString());
				reader.Close();
				return this.GetItem (table, itemId);
			}
			else {
				return null;
			}
		}
		else {
			return null;
		}
	}

	public ArrayList LoadBorrowerItems (Borrower borrower, string tableName)
	{
		if (debug)
			Console.WriteLine ("DB:LoadBorrowerItems");

		ArrayList list = new ArrayList ();
//		string sql = "SELECT * from lends WHERE table_name='"+tableName+"' AND borrower="+borrower.Id;
		string sql = "SELECT * from lends WHERE borrower="+borrower.Id;
		command.CommandText = sql;
		SqliteDataReader reader = command.ExecuteReader ();
		while (reader.Read ()) {
			if (reader["item_id"] != null && !reader["item_id"].ToString().Equals("")) {
				string table = (string)reader["table_name"];
				int itemId = Int32.Parse(reader["item_id"].ToString());
				list.Add (this.GetItem (table, itemId));
			}
		}
		reader.Close();
		return list;
	}

	public int CountBorrowerItems(int id)
	{
		if (debug)
			Console.WriteLine ("DB:GetBorrowerItem");
		string sql = "SELECT count(*) FROM lends WHERE borrower="+id;
		command.CommandText = sql;
		SqliteDataReader reader = command.ExecuteReader();
		if (reader.Read()) {
			return Int32.Parse (reader[0].ToString());
		}
		else {
			return 0;
		}
	}


	public int LendItem (Item item, Borrower borrower)
	{
		if (debug)
			Console.WriteLine ("DB:LendItem");
		StringBuilder sql = new StringBuilder (200);
		sql = sql.Append ("INSERT INTO lends VALUES (NULL, ");
		sql = sql.Append (borrower.Id);
		sql = sql.Append (", '");
		sql = sql.Append (item.Table);
		sql = sql.Append ("', ");
		sql = sql.Append (item.Id);
		sql = sql.Append (")");
		
		command.CommandText = sql.ToString();
		command.ExecuteNonQuery();

		return ((SqliteCommand)command).LastInsertRowID();
	}
	
	public void ReturnItem (Item item)
	{
		string sql = "DELETE FROM lends WHERE item_id="+item.Id;
		command.CommandText = sql;
		command.ExecuteNonQuery();
	}
	
	public bool IsBorrowed (Item item)
	{
		StringBuilder sql = new StringBuilder (100);
		sql = sql.Append ("SELECT * FROM lends WHERE item_id=");
		sql = sql.Append (item.Id);
		sql = sql.Append (" AND table_name='");
		sql = sql.Append (item.Table);
		sql = sql.Append ("'");
		command.CommandText = sql.ToString();

		SqliteDataReader reader = command.ExecuteReader();
		if (reader.Read()) {
			reader.Close();
			return true;
		}
		else {
			reader.Close();
			return false;
		}
	}

	public int AddBorrower (string name)
	{
		string sql = "INSERT INTO borrowers VALUES (NULL, '"+name+"')";
		command.CommandText = sql;
		command.ExecuteNonQuery();

		return ((SqliteCommand)command).LastInsertRowID();
	}
	
	public ListDictionary GetColumns (string table)
	{
		if (debug)
			Console.WriteLine ("DB:GetColumns");

		return InternationalizeDataBaseFields (table);

		/*
		string sql = "SELECT * FROM "+table+"_columns";
		command.CommandText = sql;

		SqliteDataReader reader = command.ExecuteReader();
		if (reader.Read()) {
			ArrayList list = new ArrayList ();
			Hashtable names = InternationalizeDataBaseFields (table);
			// Extract the column names for constructing the Item List
			for (int i=0; i<reader.FieldCount; i++) {
				string s = reader.GetName(i);
				if (!s.Equals("aux")) {
					list.Add (names[s]);
				}
			}
			return list;
		}
		else {
			return null;
		}
		*/
	}

	public Hashtable GetColumnsToShow (string table)
	{
		if (debug)
			Console.WriteLine ("DB:GetColumnsToShow");
		string sql = "SELECT * FROM "+table+"_columns";
		command.CommandText = sql;

		IDataReader reader = command.ExecuteReader();
		if (reader.Read()) {
			Hashtable columnsToShow = new Hashtable ();
			int i = 0;
			while (i < reader.FieldCount) {
				string campo = reader.GetName(i).ToString();
				if (!campo.Equals("aux")) {
					columnsToShow.Add (campo, reader[campo].ToString().Equals("Y")?true:false);
				}
				i++;
			}
			return columnsToShow;
		}
		else {
			return null;
		}
	}

	public void SetColumnsToShow (string table, Hashtable hashtable)
	{
		if (debug)
			Console.WriteLine ("DB:SetColumnsToShow");
		StringBuilder sql = new StringBuilder (1000);
		sql = sql.Append ("BEGIN; ");
		foreach (string s in hashtable.Keys) {
			sql = sql.Append ("UPDATE ");
			sql = sql.Append (table);
			sql = sql.Append ("_columns SET ");
			sql = sql.Append (s);
			sql = sql.Append ("='");
			if ((bool)hashtable[s]) {
				sql = sql.Append ("Y");
			}
			else {
				sql = sql.Append ("N");
			}
			sql = sql.Append ("' WHERE aux='1'; ");
		}
		sql = sql = sql.Append ("COMMIT;");
		command.CommandText = sql.ToString();
		command.ExecuteNonQuery();
	}

	public int AddItem (string table, Item item)
	{
		if (debug)
			Console.WriteLine ("DB:AddItem");
		StringBuilder sql = new StringBuilder (1000);
		sql = sql.Append ("INSERT INTO ");
		sql = sql.Append (table);
		sql = sql.Append (" (id, ");

		int i = 0;
		Console.WriteLine ("FOREACH 1");
		foreach (string key in item.Columns.Keys) {
			sql = sql.Append (key);
			
			if (i!=item.Columns.Count-1) {
				sql = sql.Append (", ");
			}
			else {
				sql = sql.Append (")");
			}
			i++;
		}
		
		sql = sql.Append (" VALUES (NULL, ");

		i = 0;
		Console.WriteLine ("FOREACH 2");
		foreach (string key in item.Columns.Keys) {
			StringBuilder aux = new StringBuilder(100);
			if (item.Columns[key]!=null) {
				aux = aux.Append ("'");
				aux = aux.Append (SqlString(item.Columns[key].ToString()));
				aux = aux.Append ("'");
			}
			else {
				aux = aux.Append ("NULL");
			}
			
			sql = sql.Append (aux);
			
			if (i!=item.Columns.Count-1) {
				sql = sql.Append (", ");
			}
			else {
				sql = sql.Append (")");
			}
			i++;
		}

		if (debug)
			Console.WriteLine ("SQL: "+sql.ToString());
		command.CommandText = sql.ToString();
		command.ExecuteNonQuery();

		return ((SqliteCommand)command).LastInsertRowID();
	}

	public void UpdateItem (string table, Item item)
	{
		if (debug)
			Console.WriteLine ("DB:UpdateItem");
		ListDictionary columnNames = this.GetColumns (table);
		StringBuilder sql = new StringBuilder (1000);
		sql = sql.Append ("UPDATE ");
		sql = sql.Append (table);

		sql = sql.Append (" SET ");

		int i = 0;
		foreach (string key in columnNames.Keys) {
			sql = sql.Append (key);
			sql = sql.Append ("=");
			if (item.Columns[key] != null) {
				sql = sql.Append ("'");
				sql = sql.Append (SqlString(item.Columns[key].ToString()));
				sql = sql.Append ("'");
			}
			else {
				sql = sql.Append ("NULL");
			}

			if (i < columnNames.Count - 1) {
				sql = sql.Append (", ");
			}
			i++;
		}

		sql = sql.Append (" WHERE id=");
		sql = sql.Append (item.Id);

		if (debug)
			Console.WriteLine ("SQL: "+sql.ToString());
		command.CommandText = sql.ToString();
		command.ExecuteNonQuery();
	}

	public void RemoveItem (string table, Item item)
	{
		if (debug)
			Console.WriteLine ("DB:RemoveItem");
		StringBuilder sql = new StringBuilder (100);
		sql = sql.Append ("DELETE FROM ");
		sql = sql.Append (table);
		sql = sql.Append (" WHERE id=");
		sql = sql.Append (item.Id);
		command.CommandText = sql.ToString();
		command.ExecuteNonQuery();
		Item.RemoveCache (table, item.Id);
	}

	public Item GetItem (string table, int id)
	{
		if (debug)
			Console.WriteLine ("DB:GetItem");
		Item item = null;

		StringBuilder sql = new StringBuilder (100);
		sql = sql.Append ("SELECT * from ");
		sql = sql.Append (table);
		sql = sql.Append (" WHERE id=");
		sql = sql.Append (id);
		command.CommandText = sql.ToString();
		IDataReader reader = command.ExecuteReader();

		if (reader.Read()) {
			ListDictionary columns = new ListDictionary();
			int i = 0;
			while (i < reader.FieldCount) {
				string campo = reader.GetName(i).ToString();
				if (reader[i] != null) {
					columns.Add (campo, reader[i].ToString());
				}
				else {
					columns.Add (campo, null);
				}
				i++;
			}
			item = new Item (table, columns, false);
		}

		reader.Close();

		return item;
	}

	private void UpdateDatabase (string file, string dbVersion)
	{
		if (!dbVersion.Equals ("0.0.7")) {
			if (!dbVersion.Equals ("0.0.6")) {
				PatchDatabase ("0.0.5.sql");
			}
			PatchDatabase ("0.0.7.sql");
		}

		StreamWriter swVersion = new StreamWriter (file+".version", false);
		swVersion.Write (Defines.VERSION);
		swVersion.Close();
	}

	private void PatchDatabase (string resourceFile)
	{
		Assembly thisAssembly = Assembly.GetEntryAssembly ();
		Stream stream = thisAssembly.GetManifestResourceStream (resourceFile);
		if (stream != null) {
			StreamReader sr = new StreamReader (stream);
			string sql = sr.ReadToEnd ();
			command.CommandText = sql;
			command.ExecuteNonQuery ();
		}
	}

	private ListDictionary InternationalizeDataBaseFields (string table_name)
	{
		ListDictionary table = new ListDictionary ();
		switch (table_name) {
			case "items_films":
				table.Add ("id", Mono.Posix.Catalog.GetString ("Id"));
				table.Add ("image", Mono.Posix.Catalog.GetString ("Image"));
				table.Add ("rating", Mono.Posix.Catalog.GetString ("Rating"));
				table.Add ("title", Mono.Posix.Catalog.GetString ("Title"));
				table.Add ("original_title", Mono.Posix.Catalog.GetString ("Original Title"));
				table.Add ("director", Mono.Posix.Catalog.GetString ("Director"));
				table.Add ("starring", Mono.Posix.Catalog.GetString ("Starring"));
				table.Add ("date", Mono.Posix.Catalog.GetString ("Date"));
				table.Add ("genre", Mono.Posix.Catalog.GetString ("Genre"));
				table.Add ("runtime", Mono.Posix.Catalog.GetString ("Runtime"));
				table.Add ("country", Mono.Posix.Catalog.GetString ("Country"));
				table.Add ("language", Mono.Posix.Catalog.GetString ("Language"));
				table.Add ("distributor", Mono.Posix.Catalog.GetString ("Distributor"));
				table.Add ("medium", Mono.Posix.Catalog.GetString ("Medium"));
				table.Add ("comments", Mono.Posix.Catalog.GetString ("Comments"));

				break;

			case "items_books":
				table.Add ("id", Mono.Posix.Catalog.GetString ("Id"));
				table.Add ("image", Mono.Posix.Catalog.GetString ("Image"));
				table.Add ("rating", Mono.Posix.Catalog.GetString ("Rating"));
				table.Add ("title", Mono.Posix.Catalog.GetString ("Title"));
				table.Add ("original_title", Mono.Posix.Catalog.GetString ("Original Title"));
				table.Add ("author", Mono.Posix.Catalog.GetString ("Author"));
				table.Add ("date", Mono.Posix.Catalog.GetString ("Date"));
				table.Add ("genre", Mono.Posix.Catalog.GetString ("Genre"));
				table.Add ("pages", Mono.Posix.Catalog.GetString ("Pages"));
				table.Add ("publisher", Mono.Posix.Catalog.GetString ("Publisher"));
				table.Add ("isbn", Mono.Posix.Catalog.GetString ("ISBN"));
				table.Add ("country", Mono.Posix.Catalog.GetString ("Country"));
				table.Add ("language", Mono.Posix.Catalog.GetString ("Language"));
				table.Add ("comments", Mono.Posix.Catalog.GetString ("Comments"));

				break;

			case "items_albums":
				table.Add ("id", Mono.Posix.Catalog.GetString ("Id"));
				table.Add ("image", Mono.Posix.Catalog.GetString ("Image"));
				table.Add ("rating", Mono.Posix.Catalog.GetString ("Rating"));
				table.Add ("title", Mono.Posix.Catalog.GetString ("Title"));
				table.Add ("author", Mono.Posix.Catalog.GetString ("Author"));
				table.Add ("label", Mono.Posix.Catalog.GetString ("Label"));
				table.Add ("date", Mono.Posix.Catalog.GetString ("Date"));
				table.Add ("style", Mono.Posix.Catalog.GetString ("Style"));
				table.Add ("asin", Mono.Posix.Catalog.GetString ("ASIN"));
				table.Add ("tracks", Mono.Posix.Catalog.GetString ("Tracks"));
				table.Add ("medium", Mono.Posix.Catalog.GetString ("Medium"));
				table.Add ("runtime", Mono.Posix.Catalog.GetString ("Runtime"));
				table.Add ("comments", Mono.Posix.Catalog.GetString ("Comments"));

				break;
		}

		return table;
	}
}
