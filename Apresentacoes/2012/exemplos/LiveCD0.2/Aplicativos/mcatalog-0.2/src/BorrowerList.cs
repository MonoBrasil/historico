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
using System.Text;
using System.Collections;
using Gtk;
using Gdk;

public class BorrowerList: TreeView
{
	private Database database;
	private TreeStore store;

	public event EventHandler OnBorrowerSelected;

	public BorrowerList (Database database)
	{
		this.database = database;

		store = new TreeStore (typeof(Borrower));
		this.Model = store;

		this.AppendColumn ("Borrowers", new CellRendererText (), new TreeCellDataFunc (ItemCellDataFunc));

		this.HeadersVisible = false;
		this.RulesHint = false;
		this.Visible = true;

		this.Selection.Changed += OnSelectionChanged;

		Fill ();
		this.ShowAll();
	}

	public int Count () {
		ArrayList borrowers = database.GetBorrowers();
		if (borrowers == null) {
			return 0;
		}
		else {
			return borrowers.Count;
		}
	}

	private void Fill ()
	{
		foreach (Borrower borrower in database.GetBorrowers()) {
			store.AppendValues(borrower);
		}
	}

	private void Refresh (object o, EventArgs args)
	{
		((TreeStore)this.Model).Clear();
		Fill();
	}

	private void ItemCellDataFunc(TreeViewColumn col,
			CellRenderer   cell,
			TreeModel      model,
			TreeIter       iter)
	{
		Borrower borrower = (Borrower)model.GetValue(iter, 0);
		StringBuilder sb = new StringBuilder (borrower.Name);
		sb = sb.Append ("(");
		sb = sb.Append (database.CountBorrowerItems (borrower.Id));
		sb = sb.Append (")");
		((CellRendererText)cell).Text = sb.ToString();
	}

	private void OnSelectionChanged (object o, EventArgs args)
	{
		if (this.Selection.CountSelectedRows() == 1) {
			TreeModel model;
			TreeIter iter;
			if (this.Selection.GetSelected (out model, out iter)) {
				Borrower borrower = (Borrower)model.GetValue (iter, 0);
				OnBorrowerSelected (borrower, null);
			}
		}
	}
}

