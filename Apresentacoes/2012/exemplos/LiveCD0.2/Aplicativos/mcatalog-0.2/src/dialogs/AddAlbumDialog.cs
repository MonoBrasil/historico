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
using System.Net;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;

using Gtk;
using Gnome;

using Mono.Posix;

using Amazon;

public class AddAlbumDialog: AddDialog
{
	[Glade.Widget] private Gtk.Entry entryArtists;
	[Glade.Widget] private Gtk.Entry entryLabel;
	[Glade.Widget] private Gtk.TextView textviewTracks;
	[Glade.Widget] private Gtk.Entry entryDate;
	[Glade.Widget] private Gtk.Entry entryRuntime;
	[Glade.Widget] private Gtk.Entry entryStyle;
	[Glade.Widget] private Gtk.Entry entryAsin;
	[Glade.Widget] private Gtk.Entry entryMedium;
	[Glade.Widget] private Gnome.PixmapEntry entryImage;
	[Glade.Widget] private Gtk.TextView textviewComments;

	public AddAlbumDialog (Catalog catalog) : base (catalog,
							"dialogaddalbum.glade",
							Mono.Posix.Catalog.GetString ("Add a new Album"),
							"albums")
	{
	}

	public override void EditItem (Item item)
	{
		this.Title = Mono.Posix.Catalog.GetString ("Edit");
		this.Update = item.Id;
		
		entryImage.Filename = item.Columns["image"]!=null?item.Columns["image"].ToString():"";
		ratingWidget.Value = Int32.Parse (item.Columns["rating"]!=null?item.Columns["rating"].ToString():"1");
		entryTitle.Text = item.Columns["title"]!=null?item.Columns["title"].ToString():"";
		entryArtists.Text = item.Columns["author"]!=null?item.Columns["author"].ToString():"";
		entryLabel.Text = item.Columns["label"]!=null?item.Columns["label"].ToString():"";
		entryDate.Text = item.Columns["date"]!=null?item.Columns["date"].ToString():"";
		entryStyle.Text = item.Columns["style"]!=null?item.Columns["style"].ToString():"";
		entryAsin.Text = item.Columns["asin"]!=null?item.Columns["asin"].ToString():"";
		textviewTracks.Buffer.Text = item.Columns["tracks"]!=null?item.Columns["tracks"].ToString():"";
		entryMedium.Text = item.Columns["medium"]!=null?item.Columns["medium"].ToString():"";
		entryRuntime.Text = item.Columns["runtime"]!=null?item.Columns["runtime"].ToString():"";
		textviewComments.Buffer.Text = item.Columns["comments"]!=null?item.Columns["comments"].ToString():"";
	}
	
	protected override void FillDialogFromSearch (SearchResults genericResults)
	{
		SearchResultsAlbum results = (SearchResultsAlbum) genericResults;

		entryImage.Filename = results.Image!=null?GetImage(results.Image):"";
		
		entryTitle.Text = results.Name!=null?results.Name:"";
		
		entryArtists.Text = "";
		if (results.Artists != null) {
			for (int i = 0; i < results.Artists.Length; i++) {
				if (i > 0) entryArtists.Text += ", ";
				entryArtists.Text += results.Artists[i];
			}
		}
		
		entryLabel.Text = results.Label!=null?results.Label:"";
		
		textviewTracks.Buffer.Text = "";
		if (results.Tracks != null) {
			for (int i = 0; i < results.Tracks.Length; i++) {
				textviewTracks.Buffer.Text += results.Tracks[i];
				if (i+1 < results.Tracks.Length) {
					textviewTracks.Buffer.Text += "\n";
				}
			}
		}

		entryDate.Text = results.Date!=null?results.Date:"";
		entryRuntime.Text = results.Runtime!=null?results.Runtime:"";
		entryStyle.Text = results.Style;
		entryAsin.Text = results.ASIN!=null?results.ASIN:""; 
		entryMedium.Text = results.Medium;
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
		columns.Add ("author", entryArtists.Text);
		columns.Add ("label", entryLabel.Text);
		columns.Add ("date", entryDate.Text);
		columns.Add ("style", entryStyle.Text);
		columns.Add ("asin", entryAsin.Text);
		columns.Add ("tracks", textviewTracks.Buffer.Text);
		columns.Add ("medium", entryMedium.Text);
		columns.Add ("runtime", entryRuntime.Text);
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
