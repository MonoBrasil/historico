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
using System.Reflection;

using Gtk;
using Gnome;

public abstract class AddDialog: Gtk.Dialog
{
	protected Catalog catalog;
	protected ArrayList values;
	protected string searchCategory;

	protected Gtk.Button cancelButton;
	protected Gtk.Button okButton;
	protected RatingWidget ratingWidget;
	protected ComboBox comboBoxSearchEngine;

	[Glade.Widget] protected Gtk.Button buttonSearch;
	[Glade.Widget] protected Gtk.Entry entryTitle;
	[Glade.Widget] protected Gtk.HBox hboxRating;
	[Glade.Widget] protected Gtk.HBox hboxSearch;

	protected int Update = -1;

	protected SearchEngine searchEngine;
	protected ArrayList searchEngines = null;
	protected ProgressDialog progressDialog;
	protected ArrayList list;
	protected SearchResults results;
	protected string image;

	protected Thread thread;
	protected ThreadNotify notify;

	public AddDialog (Catalog catalog,
			  string gladeFile,
			  string title,
			  string searchCategory):
		base (title, null, DialogFlags.NoSeparator | DialogFlags.Modal)
	{
		Glade.XML gxml = new Glade.XML (null, gladeFile, "vbox1", null);
		gxml.Autoconnect(this);

		this.catalog = catalog;
		this.values = new ArrayList();

		this.searchCategory = searchCategory;

		cancelButton = (Button)this.AddButton (Gtk.Stock.Cancel, 0);
		okButton     = (Button)this.AddButton (Gtk.Stock.Ok, 1);
		cancelButton.Clicked += OnCancelButtonClicked;
		okButton.Clicked     += OnOkButtonClicked;
		okButton.Sensitive = false;

		VBox vBox = this.VBox;
		vBox.Add ((VBox)gxml["vbox1"]);

		entryTitle.Changed += OnEntryTitleChanged;
		entryTitle.IsFocus = true;

		ratingWidget = new RatingWidget();
		ratingWidget.Visible = true;
		hboxRating.Add (ratingWidget);

		buttonSearch.Clicked += OnSearchClicked;

		comboBoxSearchEngine = CreateComboBoxSearchEngine ();
		hboxSearch.PackEnd (comboBoxSearchEngine);
		
		this.ShowAll();
	}

	public abstract void EditItem (Item item);
	protected abstract void FillDialogFromSearch (SearchResults results);
        public abstract void OnOkButtonClicked (object o, EventArgs args);

	public string GetImage (string url)
	{
		WebClient client = new WebClient ();
		string tempFile = System.IO.Path.GetTempFileName();
		File.Delete (tempFile);
		tempFile = Conf.DownloadedImagesDir+"/"+System.IO.Path.GetFileName(tempFile);
		client.DownloadFile (url, tempFile);
		return tempFile;
	}

	public void OnEntryTitleChanged (object sender, EventArgs e)
	{
		if (!entryTitle.Text.Trim().Equals ("")) {
			okButton.Sensitive = true;
		}
		else {
			okButton.Sensitive = false;
		}
	}

	public void OnCancelButtonClicked (object o, EventArgs args)
	{
		this.Destroy();
	}

	protected ComboBox CreateComboBoxSearchEngine ()
	{
		bool isApplicable = false;
		searchEngines = new ArrayList ();

		ComboBox combo = ComboBox.NewText ();

		Assembly assembly = Assembly.GetCallingAssembly();
		Type type = typeof (SearchEngine);
		foreach (Type t in assembly.GetTypes()) {
			
			isApplicable = false;


			if (t.IsSubclassOf (type)) {
				object[] attributes = t.GetCustomAttributes(typeof(SearchEngineAttribute), false);
				foreach(object attribute in attributes)
				{
					SearchEngineAttribute sea = (SearchEngineAttribute) attribute;
					if (sea.Category.Equals (searchCategory)) {
						isApplicable = true;
					}
				}

				if (isApplicable) {
					SearchEngine searchEngine = (SearchEngine)Activator.CreateInstance (t);
					combo.AppendText (searchEngine.Name);
					searchEngines.Add (searchEngine);
				}
			}
		}

		combo.Active = 0;
		return combo;
	}	

	protected void OnSearchClicked (object o, EventArgs args)
	{
		searchEngine = (SearchEngine)searchEngines [comboBoxSearchEngine.Active];

		if (entryTitle.Text.Trim().Equals("")) {
			string message = (Mono.Posix.Catalog.GetString ("You must write something to search."));

			MessageDialog dialog = new MessageDialog (null,
					DialogFlags.Modal | DialogFlags.DestroyWithParent,
					MessageType.Warning,
					ButtonsType.Close,
					message);
			dialog.Run ();
			dialog.Destroy ();

		}
		else {
			progressDialog = new ProgressDialog(searchEngine.Name);
			progressDialog.ShowAll();
			progressDialog.Response += OnProgressDialogResponse;

			thread = new Thread(new ThreadStart (doQuery));
			notify = new ThreadNotify (new ReadyEvent (CreateSelectionDialog));

			thread.Start();
		}
	}

	protected void CreateSelectionDialog ()
	{
		progressDialog.CloseDialog ();
	}

	protected void OnProgressDialogResponse (object o, EventArgs args)
	{
		if (list != null && list.Count > 0) {
			SelectDialog listDialog = new SelectDialog (list, searchCategory);
			int selection = listDialog.Run();

			if (selection != -1) {
				results = (SearchResults)list[selection];
				FillDialogFromSearch (results);
			}
		}
		else {
			string message = (Mono.Posix.Catalog.GetString ("No matches for your query"));

			MessageDialog dialog = new MessageDialog (null,
					DialogFlags.Modal | DialogFlags.DestroyWithParent,
					MessageType.Warning,
					ButtonsType.Close,
					message);
			dialog.Run ();
			dialog.Destroy ();
		}
	}

	protected void doQuery ()
	{
		list = searchEngine.Query (searchCategory, entryTitle.Text);
		notify.WakeupMain ();
	}
}
