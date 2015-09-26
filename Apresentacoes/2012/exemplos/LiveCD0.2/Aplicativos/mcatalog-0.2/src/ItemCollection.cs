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
using System.Collections;

public delegate void CollectionChangedHandler();

public class ItemCollection: IEnumerable, IEnumerator
{
	private Database database;
	private string table;
	private string catalog;
	
	private ArrayList items;
	private int position;

	public event CollectionChangedHandler OnChanged;
	
	public ItemCollection (Database db, string table, string catalog)
	{
		this.database = db;
		this.table = table;
		this.catalog = catalog;
		this.items = new ArrayList ();
		position = -1;
	}
	
	public string Table
	{
		get {
			return table;
		}
	}
	
	public ArrayList GetSelectedItems ()
	{
		ArrayList list = new ArrayList ();
		foreach (Item item in items) {
			if (item.IsSelected) {
				list.Add (item);
			}
		}
		return list;
	}
	
	public void UnselectAll ()
	{
		foreach (Item item in items) {
			item.IsSelected = false;
		}
	}
	
	public void Unselect (Item item)
	{
		item.IsSelected = false;
	}
	
	public void Add (Item item)
	{
		Insert (item);
		Changed();
	}
	
	private void Insert (Item item)
	{
		items.Add (item);
	}
	
	public void Remove (Item item)
	{
		items.Remove (item);
		database.RemoveItem (table, item);
		Changed();
	}
	
	public void Update (Item item)
	{
		for (int i=0; i<items.Count; i++) {
			if (((Item)items[i]).Id == item.Id) {
				items[i] = item;
				break;
			} 
		}
		Changed();
	}

	public void LoadAll ()
	{
		items.Clear ();
		ArrayList newItems = database.GetItems (this.table, this.catalog);
		foreach (Item item in newItems) {
			this.Insert (item);
		}
		Changed ();
	}
	
	public void Search (string searchString)
	{
		string field = Conf.Get ("ui/shelf_order", "0");
		if (field.Equals ("0")) {
			field = null;
		}
		
		ArrayList list = database.Search (searchString, table, field);
		
		items.Clear();

		foreach (Item item in list) {
			this.Insert (item);
		}
		Changed();
	}

	public void LoadBorrowerList (Borrower borrower)
	{
		ArrayList list = database.LoadBorrowerItems (borrower, table);
		
		items.Clear();

		foreach (Item item in list) {
			this.Insert (item);
		}
		Changed();
	}
	
	public int Count ()
	{
		return items.Count;
	}

	private void Changed ()
	{
		if (this.OnChanged != null) {
			this.OnChanged();
		}
	}
		
	public IEnumerator GetEnumerator()
	{
		return (IEnumerator)this;
	}
	
	public bool MoveNext()
	{
		if(position<items.Count-1) {
			position++;
			return true;
		}
		position=-1;
		return false;
	}
	
	public void Reset()
	{
		position=-1;
	}
	
	public object Current
	{
		get {
			return items[position];
		}
	}
}
