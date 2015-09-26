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
using System.Collections.Specialized;

using Gtk;

using Mono.Posix;

public class CatalogPropertiesDialog: Dialog
{
	private Catalog catalog;
	private ListDictionary columns;
	private Hashtable checks;

	private Button cancelButton;
	private Button okButton;
	public CatalogPropertiesDialog (Catalog catalog)
	{
		this.catalog = catalog;
		this.Title = String.Format (Mono.Posix.Catalog.GetString ("{0} properties"), catalog.Name);
		this.HasSeparator = false;

		cancelButton = (Button)this.AddButton (Stock.Cancel, 0);
		okButton     = (Button)this.AddButton (Stock.Ok, 1);
		cancelButton.Clicked += OnCancelButtonClicked;
		okButton.Clicked     += OnOkButtonClicked;
		VBox vBox = this.VBox;

		HBox titleBox = new HBox ();
		Gtk.Frame frame = new Frame ("<b>"+Mono.Posix.Catalog.GetString ("Columns to show")+"</b>");
		((Label)(frame.LabelWidget)).UseMarkup = true;
		titleBox.PackEnd (frame);
		vBox.PackStart (titleBox);

		VBox columnsBox = new VBox ();

		columns = catalog.Columns;
		Hashtable columnsToShow = catalog.ColumnsToShow;
		checks = new Hashtable ();
		foreach (string colName in columns.Keys) {
			if (colName.Equals ("id")) continue;
			CheckButton check = new CheckButton ((string)columns[colName]);
			check.Active = (bool)columnsToShow[colName];
			checks.Add (columns[colName], check);
			columnsBox.PackStart (check);
		}

		frame.Add (columnsBox);

		this.ShowAll();
	}

	private void OnCancelButtonClicked (object o, EventArgs args)
	{
		this.Destroy();
	}

	private void OnOkButtonClicked (object o, EventArgs args)
	{
		Hashtable columnsToShow = new Hashtable ();
		foreach (string s in checks.Keys) {
			CheckButton cb = (CheckButton)checks[s];
			foreach (string key in columns.Keys) {
				if (columns[key].Equals(s)) {
					columnsToShow.Add (key, cb.Active);
					break;
				}
			}
		}

		catalog.ColumnsToShow = columnsToShow;        
		this.Destroy();
	}
}
