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
 * General Public License for more results.
 *
 * You should have received a copy of the GNU General Public
 * License along with this program; if not, write to the
 * Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 */

using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;

using Gtk;
using Gnome;

using Mono.Posix;

using Amazon;

public class AddBookDialog: AddDialog
{
	[Glade.Widget] private Gtk.Entry entryOriginalTitle;
	[Glade.Widget] private Gtk.Entry entryAuthor;
	[Glade.Widget] private Gtk.Entry entryDate;
	[Glade.Widget] private Gtk.Entry entryPages;
	[Glade.Widget] private Gtk.Entry entryGenre;
	[Glade.Widget] private Gtk.Entry entryISBN;
	[Glade.Widget] private Gtk.Entry entryPublisher;
	[Glade.Widget] private Gtk.Entry entryCountry;
	[Glade.Widget] private Gtk.Entry entryLanguage;
	[Glade.Widget] private Gnome.PixmapEntry entryImage;
	[Glade.Widget] private Gtk.TextView textviewComments;

	public AddBookDialog (Catalog catalog) : base (catalog,
						       "dialogaddbook.glade",
						       Mono.Posix.Catalog.GetString ("Add a new book"),
						       "books")
	{
	}

	public override void EditItem (Item item)
	{
		this.Title = Mono.Posix.Catalog.GetString ("Editing");
		this.Update = item.Id;
		
		entryTitle.Text = item.Columns["title"]!=null?item.Columns["title"].ToString():"";
		entryOriginalTitle.Text = item.Columns["original_title"]!=null?item.Columns["original_title"].ToString():"";
		entryAuthor.Text = item.Columns["author"]!=null?item.Columns["author"].ToString():"";
		entryDate.Text = item.Columns["date"]!=null?item.Columns["date"].ToString():"";
		entryGenre.Text = item.Columns["genre"]!=null?item.Columns["genre"].ToString():"";
		entryPages.Text = item.Columns["pages"]!=null?item.Columns["pages"].ToString():"";
		entryPublisher.Text = item.Columns["publisher"]!=null?item.Columns["publisher"].ToString():"";
		entryISBN.Text = item.Columns["isbn"]!=null?item.Columns["isbn"].ToString():"";
		entryCountry.Text = item.Columns["country"]!=null?item.Columns["country"].ToString():"";
		entryLanguage.Text = item.Columns["language"]!=null?item.Columns["language"].ToString():"";
		entryImage.Filename = item.Columns["image"]!=null?item.Columns["image"].ToString():"";
		textviewComments.Buffer.Text = item.Columns["comments"]!=null?item.Columns["comments"].ToString():"";
		ratingWidget.Value = Int32.Parse (item.Columns["rating"]!=null?item.Columns["rating"].ToString():"1");
	}
	
	protected override void FillDialogFromSearch (SearchResults genericResults)
	{
		SearchResultsBook results = (SearchResultsBook)genericResults;

		entryTitle.Text = results.Name!=null?results.Name:"";
		
		entryAuthor.Text = "";
		for (int i = 0; i < results.Authors.Length; i++) {
			entryAuthor.Text += results.Authors[i];
			if (i+1 < results.Authors.Length) {
				entryAuthor.Text += ", ";
			}
		}
		
		entryDate.Text = results.Date!=null?results.Date:"";
		entryPages.Text = results.Pages!=null?results.Pages:"";
		entryISBN.Text = results.ISBN!=null?results.ISBN:"";
		entryPublisher.Text = results.Publisher!=null?results.Publisher:"";
		entryImage.Filename = results.Image!=null?GetImage(results.Image):"";
		textviewComments.Buffer.Text = results.Comments!=null?results.Comments:"";
	}
	
	public override void OnOkButtonClicked (object o, EventArgs args)
	{
		ListDictionary columns = new ListDictionary ();
		if (this.Update == -1) { 
			columns.Add ("id", null);
		}
		else {
			columns.Add ("id", this.Update.ToString());
		}
		
		if (entryImage.Filename != null) {
			FileInfo fileInfo = new FileInfo (entryImage.Filename);
			if (fileInfo.Length == 807) {
				columns.Add ("image", null);
			}
			else {
				columns.Add ("image", entryImage.Filename);
			}
		}
		else {
			columns.Add ("image", null);
		}
		
		columns.Add ("rating", ratingWidget.Value.ToString());
		columns.Add ("title", entryTitle.Text);
		columns.Add ("original_title", entryOriginalTitle.Text);
		columns.Add ("author", entryAuthor.Text);
		columns.Add ("date", entryDate.Text);
		columns.Add ("genre", entryGenre.Text);
		columns.Add ("pages", entryPages.Text);
		columns.Add ("publisher", entryPublisher.Text);
		columns.Add ("isbn", entryISBN.Text);
		columns.Add ("country", entryCountry.Text);
		columns.Add ("language", entryLanguage.Text);
		columns.Add ("comments", textviewComments.Buffer.Text);
		
		if (this.Update == -1) {
			catalog.NewItem (new Item (catalog.Table, columns));
		}
		else {
			catalog.UpdateItem (new Item (catalog.Table, columns));
		}
		
		this.Destroy();
	}
}
