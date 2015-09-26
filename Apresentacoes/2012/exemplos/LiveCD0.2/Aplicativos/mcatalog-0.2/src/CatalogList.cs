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
using Gtk;

public class CatalogList: TreeView
{
	private int count;

	public CatalogList ()
	{
		this.count = 0;

		this.AppendColumn ("", new CellRendererPixbuf(), new TreeCellDataFunc (ImageCellDataFunc));
		this.AppendColumn ("Catalogs", new CellRendererText(), new TreeCellDataFunc (CatalogCellDataFunc));
		this.Model = new ListStore (typeof(Catalog));
		this.HeadersVisible = false;
		this.RulesHint = false;
		this.Visible = true;

		this.PopupMenu += OnPopupMenu;
		this.ButtonReleaseEvent += OnButtonReleased;
		this.RowActivated += OnRowActivated;
	}

	public void AddCatalog (Catalog catalog)
	{
		((ListStore)this.Model).AppendValues(catalog);
		count++;
	}

	public Catalog GetSelectedCatalog ()
	{
		TreeIter      iter;
		TreeModel     model;
		Catalog       catalog;

		if (!this.Selection.GetSelected(out model, out iter)) {
			return null;
		}

		catalog = (Catalog)model.GetValue(iter, 0);

		return catalog;
	}

	public void SetSelectedCatalog (int catalog)
	{
		this.Selection.SelectPath (new TreePath (catalog.ToString()));
	}

	public int Count ()
	{
		return count;
	}

	public void Search (string searchString)
	{
		Catalog catalog = GetSelectedCatalog ();
		catalog.ItemCollection.Search (searchString);
	}

	private void OnPopupMenu (object o, Gtk.PopupMenuArgs args)
	{
		if (this.Selection.CountSelectedRows() > 0) {
			ShowPopup ();
		}
	}

	private void OnButtonReleased (object o, Gtk.ButtonReleaseEventArgs args)
	{
		if (this.Selection.CountSelectedRows() > 0) {
			if (args.Event.Button == 3)
				ShowPopup ();
		}
	}

	private void ShowPopup ()
	{
		Gtk.Menu menu = new Gtk.Menu ();
		menu.AccelGroup = new AccelGroup ();
		Gtk.ImageMenuItem properties = new Gtk.ImageMenuItem (Gtk.Stock.Properties, menu.AccelGroup);
		properties.Activated += OnPropertiesMenuItemClicked;

		menu.Append (properties);

		menu.Popup (null, null, null, IntPtr.Zero, 3, Gtk.Global.CurrentEventTime);
		menu.ShowAll ();
	}

	private void OnRowActivated (object o, EventArgs args)
	{
		OnPropertiesMenuItemClicked (null, null);
	}

	private void OnPropertiesMenuItemClicked (object o, EventArgs args)
	{
		CatalogPropertiesDialog dialog = new CatalogPropertiesDialog (GetSelectedCatalog());
		dialog.Run();
	}

	private void ImageCellDataFunc(TreeViewColumn col,
			CellRenderer   cell,
			TreeModel      model,
			TreeIter       iter)
	{
		Catalog catalog = (Catalog)model.GetValue(iter, 0);

		CellRendererPixbuf cr = (CellRendererPixbuf) cell;
		cr.Pixbuf = catalog.Image.Pixbuf;
	}

	private void CatalogCellDataFunc(TreeViewColumn col,
			CellRenderer   cell,
			TreeModel      model,
			TreeIter       iter)
	{
		Catalog catalog = (Catalog)model.GetValue(iter, 0);
		((CellRendererText)cell).Text = catalog.ShortDescription;
	}
}
