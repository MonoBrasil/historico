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

using Gtk;
using Gdk;
using GLib;

using Mono.Posix;

using System;
using System.Collections;
using System.Collections.Specialized;

public class ItemList: Gtk.TreeView
{
	public event EventHandler OnEditItemRequest;
	public event EventHandler OnLendItemRequest;
	public event EventHandler OnReturnItemRequest;
	public event EventHandler OnItemSelected;
	public event EventHandler OnItemDeleted;

	private ScrolledWindow swPresentation;

	private Catalog catalog;

	private ArrayList order;
	private Hashtable types;
	private Hashtable columnsToShow;
	private ListDictionary columnNames;
	public Hashtable items;

	private int idColumnPosition;
	private int imageColumnPosition;

	private Presentation presentation;
	
	public ItemList (Catalog catalog, ListDictionary columnNames, Hashtable columnsToShow, Presentation presentation)
	{
		this.presentation = presentation;
		this.catalog = catalog;
		catalog.OnItemCollectionChanged += Refresh;
		this.columnNames = columnNames;
		this.items = new Hashtable ();

		order = new ArrayList ();

		if (this.columnNames != null) {
			int columnIndex = 0;
			this.columnsToShow = columnsToShow;

			// create the types array for the treeview
			types = new Hashtable ();

			foreach (string columnName in this.columnNames.Keys) {
				TreeViewColumn column = new TreeViewColumn();
				if (columnName.Equals ("id")) {

					// Let's fill the Type array for the Model and create the columns.
					// Id Column
					types.Add (columnName, typeof (string));
					order.Add (columnName);
					column.Title = "Id";
					CellRendererText idCellRenderer = new CellRendererText ();
					column.PackStart (idCellRenderer, false);
					RenderInfo idRenderInfo = new RenderInfo (this, columnIndex);
					column.SetCellDataFunc (idCellRenderer, new TreeCellDataFunc (idRenderInfo.GenericCellDataFunc));
					column.SortColumnId = columnIndex;
					this.InsertColumn (column, columnIndex);
					idColumnPosition = columnIndex;
					column.Visible = false;
				}
				else if (columnName.Equals ("image")) {

					types.Add (columnName, typeof (Gdk.Pixbuf));
					order.Add (columnName);
					column.Title = Mono.Posix.Catalog.GetString ("Cover");
					CellRendererPixbuf imageCellRenderer = new CellRendererPixbuf ();
					column.PackStart (imageCellRenderer, false);
					column.SetCellDataFunc (imageCellRenderer, new TreeCellDataFunc (ImageCellDataFunc));
					this.InsertColumn (column, columnIndex);
					imageColumnPosition = columnIndex;
					column.Visible = (bool)this.columnsToShow["image"];
				}
				else {
					types.Add (columnName, typeof(string));
					order.Add (columnName);
					column.Resizable = true;
					column.Title = columnName;
					RenderInfo renderInfo = new RenderInfo (this, columnIndex);
					CellRendererText genericCellRenderer = new CellRendererText();
					column.PackStart (genericCellRenderer, false);
					column.SetCellDataFunc (genericCellRenderer, new TreeCellDataFunc (renderInfo.GenericCellDataFunc));
					column.SortColumnId = columnIndex;
					this.InsertColumn (column, columnIndex);
					column.Visible = (bool)this.columnsToShow[columnName];
				}
				columnIndex++;
			}

			Type[] typeArray = new Type[types.Count];
			for (int i=0; i<order.Count; i++) {
				typeArray[i] = (Type)types[order[i]];
			}
			this.Model = new ListStore (typeArray);
			this.Selection.Mode = SelectionMode.Multiple;
			this.Selection.Changed += OnSelectionChanged;
			this.ButtonReleaseEvent += OnButtonReleased;
			this.RowActivated += OnRowActivated;
			this.EnableSearch = true;
			this.HeadersVisible = true;
			this.RulesHint = true;
			this.Visible = true;

			LoadAll();
		}
	}

	public Catalog Catalog
	{
		get {
			return catalog;
		}
	}

	public ScrolledWindow PresentationWidget
	{
		get {
			return this.swPresentation;
		}
		set {
			this.swPresentation = value;
		}
	}

	public void LoadAll ()
	{
		foreach (Item item in catalog.ItemCollection) {
			AddItem (item);
		}
	}

	public void Clear ()
	{
		((ListStore)this.Model).Clear();
		items.Clear();
	}

	private void Refresh ()
	{
		Clear();
		LoadAll();
	}

	public void AddItem (Item item)
	{
		items.Add (item.Id, item);
		TreeIter iter = ((ListStore)this.Model).Append();

		for (int i = 0; i < order.Count; i++) {
			if (types[order[i]] == typeof (Gdk.Pixbuf)) {
				if (item.SmallCover !=null && !item.SmallCover.Equals("")) {
					Gdk.Pixbuf cover = new Gdk.Pixbuf (item.SmallCover);
					if (cover != null) {
						((ListStore)this.Model).SetValue (iter, i, cover);
					}
				}
			}
			else {
				if (item.Columns.Contains (order[i].ToString())) {
					string aux = item.Columns[order[i].ToString()].ToString();
					if (aux != null) {
						((ListStore)this.Model).SetValue (iter, i, aux);
					}
				}
				else {
					((ListStore)this.Model).SetValue (iter, i, "");
				}
			}
		}
	}

	public void ImageCellDataFunc(TreeViewColumn col,
			CellRenderer   cell,
			TreeModel      model,
			TreeIter       iter)
	{
		CellRendererPixbuf cr = (CellRendererPixbuf) cell;
		Gdk.Pixbuf image;
		try {
			image = (Gdk.Pixbuf)(((ListStore)model).GetValue(iter, imageColumnPosition));

			if (image != null) {
				cr.Pixbuf = image;
			}
			else {
				cr.Pixbuf = Pixbuf.LoadFromResource ("empty.png");
			}
		}
		catch {
			cr.Pixbuf = Pixbuf.LoadFromResource ("empty.png");
		}
	}

	private class RenderInfo
	{
		private int column;

		public RenderInfo (ItemList tv, int column)
		{
			this.column = column;
		}

		public void GenericCellDataFunc (TreeViewColumn col,
				CellRenderer cell,
				TreeModel model,
				TreeIter iter)
		{
			object s = ((ListStore)model).GetValue (iter, column);

			if (s != null) {
				((CellRendererText)cell).Text = s.ToString();
			}
		}
	}

	private void OnButtonReleased (object o, ButtonReleaseEventArgs args)
	{
		if (args.Event.Button == 3) {
			ShowPopup ();
		}
	}

	private void OnRowActivated (object o, RowActivatedArgs args)
	{
		OnEditItemRequest (null, null);
	}

	private void ShowPopup ()
	{
		Gtk.Menu menu = new Gtk.Menu ();
		menu.AccelGroup = new AccelGroup ();
		Gtk.MenuItem editMenuItem = new Gtk.MenuItem (Mono.Posix.Catalog.GetString ("Edit"));
		
		ArrayList selectedItems = catalog.ItemCollection.GetSelectedItems();
		Gtk.MenuItem lendMenuItem = null;
		if (selectedItems.Count == 1) {
			if (catalog.IsBorrowed ((Item)selectedItems[0])) {
				lendMenuItem = new Gtk.MenuItem (Mono.Posix.Catalog.GetString ("Return"));
				lendMenuItem.Activated  += OnReturnMenuItemClicked;
			}
			else {
				lendMenuItem = new Gtk.MenuItem (Mono.Posix.Catalog.GetString ("Lend"));
			 	lendMenuItem.Activated  += OnLendMenuItemClicked;
			}
		}
		
		Gtk.ImageMenuItem removeMenuItem = new Gtk.ImageMenuItem (Gtk.Stock.Remove, menu.AccelGroup);
		Gtk.SeparatorMenuItem separator = new Gtk.SeparatorMenuItem();

		editMenuItem.Activated += OnEditMenuItemClicked;
		removeMenuItem.Activated += OnRemoveMenuItemClicked;

		menu.Append (editMenuItem);
		if (lendMenuItem != null) {
			menu.Append (lendMenuItem);
		}
		menu.Append (separator);
		menu.Append (removeMenuItem);

		menu.Popup (null, null, null, IntPtr.Zero, 3, Gtk.Global.CurrentEventTime);	

		menu.ShowAll ();
	}

	private void OnEditMenuItemClicked (object o, EventArgs args)
	{
		OnEditItemRequest (null, null);
	}
	
	private void OnLendMenuItemClicked (object o, EventArgs args)
	{
		OnLendItemRequest (null, null);
	}
	
	private void OnReturnMenuItemClicked (object o, EventArgs args)
	{
		OnReturnItemRequest (null, null);
	}

	private void OnRemoveMenuItemClicked (object o, EventArgs args)
	{
		OnItemDeleted (null, null);
	}

	private void OnSelectionChanged (object o, EventArgs args)
	{
		catalog.ItemCollection.UnselectAll ();

		TreeSelectionForeachFunc func = new TreeSelectionForeachFunc (OnSelectedForeach);
		this.Selection.SelectedForeach (func );
		this.OnItemSelected(o, args);
	}

	private void OnSelectedForeach (TreeModel model, TreePath path, TreeIter iter)
	{
		int id = Int32.Parse (model.GetValue(iter, idColumnPosition).ToString());
		Item item = (Item)items[id];
		item.IsSelected = true;

		if (this.Selection.CountSelectedRows() == 1) {
			presentation.Load (catalog.Table, item);
		}
	}
}
