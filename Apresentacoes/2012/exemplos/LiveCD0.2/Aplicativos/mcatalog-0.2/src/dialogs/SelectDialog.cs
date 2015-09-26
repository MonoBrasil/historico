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
using System.Text;
using System.Collections;

using Gtk;

using Mono.Posix;

using Amazon;

public class SelectDialog: Dialog
{
	private ArrayList list;
	private ListStore store;
	private TreeView tv;
	
	private class Pair {
		public int Index;
		public SearchResults SearchResults;
		public Pair (int a, SearchResults b) {
			this.Index = a;
			this.SearchResults = b;
		}
	}
	
	public SelectDialog (ArrayList list, string category): base ()
	{
		this.Modal = true;
		this.HasSeparator = false;
		this.Resizable = false;
		this.list = list;
		this.Title = Mono.Posix.Catalog.GetString ("Select your choice");
		
		VBox vBox = this.VBox;
			
		Button cancelButton = (Button)this.AddButton (Gtk.Stock.Cancel, 0);
		Button okButton     = (Button)this.AddButton (Gtk.Stock.Ok, 1);
		cancelButton.Clicked += OnCancelButtonClicked;
		okButton.Clicked     += OnOkButtonClicked;
		
		PopulateStore ();
		tv = new TreeView (store);
		tv.HeadersVisible = true;
		tv.RowActivated += OnRowActivated;
		CellRendererText ct = new CellRendererText ();
		tv.AppendColumn (Mono.Posix.Catalog.GetString ("Title"), ct, new TreeCellDataFunc (CellDataTitle));
		switch (category) {
			case "films":
				tv.AppendColumn (Mono.Posix.Catalog.GetString ("Director"), ct, new TreeCellDataFunc (CellDataDirector));
				break;
			case "books":
				tv.AppendColumn (Mono.Posix.Catalog.GetString ("Author"), ct, new TreeCellDataFunc (CellDataAuthor));
				break;
			case "albums":
				tv.AppendColumn (Mono.Posix.Catalog.GetString ("Artists"), ct, new TreeCellDataFunc (CellDataArtists));
				break;
		}
		vBox.Add (tv);
		vBox.ShowAll();
	}
			
	private void PopulateStore ()
	{
		store = new ListStore (typeof (Pair));
		for (int i = 0; i < list.Count; i++) {
			store.AppendValues (new Pair (i, (SearchResults)list[i]));
		}
	}
	
	private void CellDataTitle (Gtk.TreeViewColumn tree_column,
								Gtk.CellRenderer cell,
								Gtk.TreeModel tree_model,
								Gtk.TreeIter iter)
	{
		Pair val = (Pair) store.GetValue (iter, 0);		
		string s = val.SearchResults.Name;
		if (s.Length > 50) {
			s = s.Substring (0, 50)+"...";
		}

		((CellRendererText) cell).Text = s;
	}
	
	private void CellDataDirector (Gtk.TreeViewColumn tree_column,
								Gtk.CellRenderer cell,
								Gtk.TreeModel tree_model,
								Gtk.TreeIter iter)
	{
		Pair val = (Pair) store.GetValue (iter, 0);
		
		SearchResultsFilm searchResults = (SearchResultsFilm)val.SearchResults;
		if (searchResults != null && searchResults.Directors != null) {
			StringBuilder s = new StringBuilder ("");
			for (int i = 0; i < searchResults.Directors.Length; i++) {
				s.Append (searchResults.Directors[i]);
				if (i+1 < searchResults.Directors.Length) {
					s.Append (", ");
				}
			}

			string str = s.ToString();
			if (str.Length > 50) {
				str = str.Substring (0, 50)+"...";
			}
		
			((CellRendererText) cell).Text = str;
		}
	}
	
	private void CellDataAuthor (Gtk.TreeViewColumn tree_column,
								Gtk.CellRenderer cell,
								Gtk.TreeModel tree_model,
								Gtk.TreeIter iter)
	{
		Pair val = (Pair) store.GetValue (iter, 0);

		SearchResultsBook searchResults = (SearchResultsBook)val.SearchResults;
		
		if (searchResults != null && searchResults.Authors != null) {
			StringBuilder s = new StringBuilder ("");
			for (int i = 0; i < searchResults.Authors.Length; i++) {	
				s.Append (searchResults.Authors[i]);
				if (i+1 < searchResults.Authors.Length) {
					s.Append (", ");
				}
			}
		
			string str = s.ToString();
			if (str.Length > 50) {
				str = str.Substring (0, 50)+"...";
			}
			
			((CellRendererText) cell).Text = str;
		}
	}

	private void CellDataArtists (Gtk.TreeViewColumn tree_column,
								Gtk.CellRenderer cell,
								Gtk.TreeModel tree_model,
								Gtk.TreeIter iter)
	{
		Pair val = (Pair) store.GetValue (iter, 0);

		SearchResultsAlbum searchResults = (SearchResultsAlbum)val.SearchResults;
		
		if (searchResults != null && searchResults.Artists != null) {
			StringBuilder s = new StringBuilder ("");
			for (int i = 0; i < searchResults.Artists.Length; i++) {	
				s.Append (searchResults.Artists[i]);
				if (i+1 < searchResults.Artists.Length) {
					s.Append (", ");
				}
			}
		
			string str = s.ToString();
			if (str.Length > 50) {
				str = str.Substring (0, 50)+"...";
			}
			
			((CellRendererText) cell).Text = str;
		}
	}
		
	private void OnCancelButtonClicked (object o, EventArgs args)
	{
		this.Respond (-1);
		this.Destroy();
	}

	private void OnRowActivated (object o, RowActivatedArgs args) {
		OnOkButtonClicked (null, null);
	}
	
	private void OnOkButtonClicked (object o, EventArgs args)
	{
		if (tv.Selection.CountSelectedRows() > 0) {
			TreeModel model;
			TreeIter iter;
			tv.Selection.GetSelected (out model, out iter);
			Pair pair = (Pair)store.GetValue (iter, 0);
			this.Respond (pair.Index);
		}
		else {
			this.Respond (-1);
		}
		this.Destroy();
	}
}
