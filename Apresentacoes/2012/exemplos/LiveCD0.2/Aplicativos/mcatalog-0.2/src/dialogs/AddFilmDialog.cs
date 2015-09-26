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

public class AddFilmDialog: AddDialog
{
	[Glade.Widget] private Gtk.Entry entryOriginalTitle;
	[Glade.Widget] private Gtk.Entry entryDirector;
	[Glade.Widget] private Gtk.Entry entryDate;
	[Glade.Widget] private Gtk.Entry entryRuntime;
	[Glade.Widget] private Gtk.Entry entryGenre;
	[Glade.Widget] private Gtk.Entry entryDistributor;
	[Glade.Widget] private Gtk.Entry entryMedium;
	[Glade.Widget] private Gtk.Entry entryCountry;
	[Glade.Widget] private Gtk.Entry entryLanguage;
	[Glade.Widget] private Gnome.PixmapEntry entryImage;
	[Glade.Widget] private Gtk.TextView textviewComments;
	[Glade.Widget] private Gtk.TextView textviewStarring;

	public AddFilmDialog (Catalog catalog) : base (catalog,
						       "dialogaddfilm.glade",
						       Mono.Posix.Catalog.GetString ("Add a new film"),
						       "films")
	{
	}
	
	public override void EditItem (Item item)
	{
		this.Title = Mono.Posix.Catalog.GetString ("Edit");
		this.Update = item.Id;
		
		entryImage.Filename = item.Columns["image"]!=null?item.Columns["image"].ToString():"";
		ratingWidget.Value = Int32.Parse (item.Columns["rating"]!=null?item.Columns["rating"].ToString():"1");
		entryTitle.Text = item.Columns["title"]!=null?item.Columns["title"].ToString():"";
		entryOriginalTitle.Text = item.Columns["original_title"]!=null?item.Columns["original_title"].ToString():"";
		entryDirector.Text = item.Columns["director"]!=null?item.Columns["director"].ToString():"";
		textviewStarring.Buffer.Text = item.Columns["starring"]!=null?item.Columns["starring"].ToString():"";
		entryDate.Text = item.Columns["date"]!=null?item.Columns["date"].ToString():"";
		entryGenre.Text = item.Columns["genre"]!=null?item.Columns["genre"].ToString():"";
		entryRuntime.Text = item.Columns["runtime"]!=null?item.Columns["runtime"].ToString():"";
		entryCountry.Text = item.Columns["country"]!=null?item.Columns["country"].ToString():"";
		entryLanguage.Text = item.Columns["language"]!=null?item.Columns["language"].ToString():"";
		entryDistributor.Text = item.Columns["distributor"]!=null?item.Columns["distributor"].ToString():"";
		entryMedium.Text = item.Columns["medium"]!=null?item.Columns["medium"].ToString():"";
		textviewComments.Buffer.Text = item.Columns["comments"]!=null?item.Columns["comments"].ToString():"";
	}
	
	protected override void FillDialogFromSearch (SearchResults genericResults) 
	{
		SearchResultsFilm results = (SearchResultsFilm)genericResults;
		
		entryImage.Filename = results.Image!=null?GetImage(results.Image):"";
		ratingWidget.Value = results.Rating;
		
		entryTitle.Text = results.Name!=null?results.Name:"";
		
		entryDirector.Text = "";
		if (results.Directors != null) {
			for (int i = 0; i < results.Directors.Length; i++) {
				if (i > 0) entryDirector.Text += ", ";
				entryDirector.Text += results.Directors[i];
			}
		}
		
		if (results.Date != null) {
			entryDate.Text = results.Date.Substring (0, 4);
		}
		
		entryGenre.Text = results.Genre;
		entryRuntime.Text = results.RunningTime!=null?results.RunningTime:"";
		entryCountry.Text = results.Country; 
		entryLanguage.Text = results.Language;
		entryDistributor.Text = results.Manufacturer!=null?results.Manufacturer:"";
		entryMedium.Text = results.Medium;
		
		if (results.Comments != null) {
			textviewComments.Buffer.Text = results.Comments;
		}
		
		textviewStarring.Buffer.Text = "";
		if (results.Starring != null) {
			for (int i = 0; i < results.Starring.Length; i++) {
				textviewStarring.Buffer.Text += results.Starring[i];
				if (i+1 < results.Starring.Length) {
					textviewStarring.Buffer.Text += "\n";
				}
			}
		}
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
		columns.Add ("director", entryDirector.Text);
		columns.Add ("starring", textviewStarring.Buffer.Text);
		columns.Add ("date", entryDate.Text);
		columns.Add ("genre", entryGenre.Text);
		columns.Add ("runtime", entryRuntime.Text);
		columns.Add ("country", entryCountry.Text);
		columns.Add ("language", entryLanguage.Text);
		columns.Add ("distributor", entryDistributor.Text);
		columns.Add ("medium", entryMedium.Text);
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
