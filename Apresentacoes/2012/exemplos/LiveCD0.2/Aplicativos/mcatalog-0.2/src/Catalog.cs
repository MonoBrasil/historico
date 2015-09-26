/* Copyright (C) 2004 Cesar Garcia Tapia <tapia@mcatalog.net>
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
using System.Collections.Specialized;

using Gtk;
using Gdk;
using Mono.Posix;

public class Catalog 
{
	private string name;
	private string table;
	private string shortDescription;
	private string longDescription;
	private Gtk.Image  image;
	private int    weight;
	private Database database;
	private ItemCollection itemCollection;

	public event CollectionChangedHandler OnItemCollectionChanged;
	public event CollectionChangedHandler OnBorrowerListChanged;

	public Catalog (Database db,
			string name,
			string table,
			string shortDescription,
			string longDescription,
			string image,
			int weight)
	{
		this.database = db;
		this.name   = name;
		this.table  = table;
		this.itemCollection = new ItemCollection (db, table, name);
		itemCollection.OnChanged += ItemCollectionChanged;

		this.shortDescription = shortDescription;
		this.longDescription = longDescription;
		this.weight = weight;

		this.image = new Gtk.Image();
		try {
			this.image.FromPixbuf = Pixbuf.LoadFromResource (image);
		}
		catch {
			this.image = null;
		}
	}

	public void LoadAll ()
	{
		itemCollection.LoadAll();
	}

	private void ItemCollectionChanged ()
	{
		if (this.OnItemCollectionChanged != null) {
			this.OnItemCollectionChanged();
		}
	}
	
	public void BorrowerListChanged ()
	{
		if (this.OnBorrowerListChanged != null) {
			this.OnBorrowerListChanged();
		}
	}

	public ItemCollection ItemCollection
	{
		get {
			return itemCollection;
		}
	}

	public string Name
	{
		get {
			return name;
		}
		set {
			name = value;
		}
	}

	public double Zoom
	{
		get {
			return database.GetCatalogZoom (name);
		}
		set {
			database.SetCatalogZoom (name, value);
		}
	}

	public ListDictionary Columns
	{
		get {
			return database.GetColumns(table);
		}
	}

	public Hashtable ColumnsToShow
	{
		get {
			return database.GetColumnsToShow(table);
		}
		set {
			database.SetColumnsToShow (table, value);
		}
	}

	public string Table
	{
		get {
			return table;
		}
	}

	public string ShortDescription
	{
		get {
			return shortDescription;
		}
		set {
			shortDescription = value;
		}
	}

	public string LongDescription
	{
		get {
			return longDescription;
		}
		set {
			longDescription = value;
		}
	}

	public Gtk.Image Image
	{
		get {
			return image;
		}
		set {
			image = value;
		}
	}

	public int Weight
	{
		get {
			return weight;
		}
		set {
			weight = value;
		}
	}

	public Order Order
	{
		get {
			return database.GetCatalogOrder (name);
		}
		set {
			database.SetCatalogOrder (name, value);
		}
	}

	public void NewItem (Item item)
	{
		int id = database.AddItem (table, item);
		itemCollection.Add (database.GetItem (table, id));
	}

	public void UpdateItem (Item item)
	{
		database.UpdateItem (table, item);
		itemCollection.Update (database.GetItem (table, item.Id));
	}

	public void RemoveItem (Item item)
	{
		database.RemoveItem (table, item);
		itemCollection.Remove (item);
	}
	
	public void ReturnItem (Item item)
	{
		database.ReturnItem (item);
	}
	
	public void OpenAddItemDialog ()
	{
		OpenAddItemDialog (null);
	}

	public void EditSelectedItem()
	{
		foreach (Item item in itemCollection.GetSelectedItems()) {
			OpenAddItemDialog (item);
		}
	}
	
	public void LendSelectedItem()
	{
		foreach (Item item in itemCollection.GetSelectedItems()) {
			OpenLendItemDialog (item);
		}
	}
	
	public void ReturnSelectedItem()
	{
		foreach (Item item in itemCollection.GetSelectedItems()) {
			OpenReturnItemDialog (item);
		}
	}

	public bool IsBorrowed (Item item)
	{
		return database.IsBorrowed (item);
	}
	
	private void OpenAddItemDialog (Item item)
	{
		Dialog dialog = null;
		switch (table) {
			case "items_books":
				dialog = new AddBookDialog (this);
				if (item != null) {
					((AddBookDialog)dialog).EditItem (item);
				}
				break;
			case "items_films":
				dialog = new AddFilmDialog (this);
				if (item != null) {
					((AddFilmDialog)dialog).EditItem (item);
				}
				break;
			case "items_albums":
				dialog = new AddAlbumDialog (this);
				if (item != null) {
					((AddAlbumDialog)dialog).EditItem (item);
				}
				break;
		}
	}
	
	private void OpenLendItemDialog (Item item)
	{
		LendItemDialog dialog = new LendItemDialog (item, database);
		if (dialog.Run() == 1) {
			BorrowerListChanged();
		}
	}
	
	private void OpenReturnItemDialog (Item item)
	{
		Dialog dialog = new MessageDialog (null,
				DialogFlags.Modal,
				MessageType.Question,
				ButtonsType.OkCancel,
				String.Format (Mono.Posix.Catalog.GetString ("Return {0}?"),item.Title));
		ResponseType response = (ResponseType)dialog.Run();

		if (response == ResponseType.Ok) {
			ReturnItem (item);
			BorrowerListChanged();
		}

		dialog.Destroy();
	}
}
