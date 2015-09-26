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
using System.Collections;

using Gtk;
using Gnome;

using Amazon;

public class ExportDialog: Gtk.Dialog
{
	private Catalog catalog;
	private string template;
	private Hashtable templates;
	private bool searchOn;
	
	private Gtk.Button cancelButton;
	private Gtk.Button okButton;

	[Glade.Widget] private ComboBox templateComboBox;
	[Glade.Widget] private FileEntry outputFileEntry;
	[Glade.Widget] private Image previewImage;

	[Glade.Widget] private RadioButton radioButtonSearch;
	[Glade.Widget] private RadioButton radioButtonActive;

	public ExportDialog (Catalog catalog, bool searchOn):
		base (Mono.Posix.Catalog.GetString ("Export"), null, DialogFlags.NoSeparator | DialogFlags.Modal)
	{
		this.catalog = catalog;
		this.templates = new Hashtable ();
		this.searchOn = searchOn;
		
		Glade.XML gxml = new Glade.XML (null, "dialogexport.glade", "hbox1", null);
		gxml.Autoconnect(this);

		cancelButton = (Button)this.AddButton (Gtk.Stock.Cancel, 0);
		okButton     = (Button)this.AddButton (Gtk.Stock.Ok, 1);
		cancelButton.Clicked += OnCancelButtonClicked;
		okButton.Clicked     += OnOkButtonClicked;

		VBox vBox = this.VBox;
		vBox.Add ((Box)gxml["hbox1"]);

		PopulateComboBox ();

		if (!searchOn) {
			radioButtonSearch.Sensitive = false;
		}

		radioButtonActive.Label = String.Format (Mono.Posix.Catalog.GetString ("Export the whole {0} catalog"),catalog.ShortDescription);

		this.ShowAll();
	}

	private void PopulateComboBox ()
	{
		ComboBox aux = ComboBox.NewText ();
		aux.Changed += ChangeComboBox;
		Box box = (Box) templateComboBox.Parent;
		box.Remove (templateComboBox);
		templateComboBox = aux;
		box.PackEnd (templateComboBox);
		
		FileInfo[] files = new DirectoryInfo(Defines.TEMPLATES_DATADIR).GetFiles("*.rc");
		foreach (FileInfo fileInfo in files)
		{
			StreamReader sr = fileInfo.OpenText();
			string line;
			string path = "";
			string templateName = "";
			char[] delimiter= {'='};
			while (true) {
				line = sr.ReadLine ();
				if (line == null)
					break;

				string[] split = line.Split (delimiter);
				if (split[0] == null || split[1] == null) 
					continue;

				switch (split[0].ToUpper()) {
					case "NAME":
						templateName = split[1];
						path = System.IO.Path.ChangeExtension (fileInfo.FullName, "html");
						break;
					case "CATALOG":
						if (catalog.Name.Equals (split[1])) {
							templateComboBox.AppendText (templateName);
							templates.Add (templateName, path);
						}
						break;
				}
			}
			sr.Close ();
		}
		templateComboBox.Active = 0;
	}

	private void ChangeComboBox (object o, EventArgs args)
	{
		TreeIter iter;

		if (templateComboBox.GetActiveIter (out iter)) {
			template = (string) templates[(string) templateComboBox.Model.GetValue (iter, 0)];
			string imageFileName = template.Replace (".html", ".png");
			if (File.Exists (imageFileName)) {
				if (previewImage != null) {
					previewImage.File = imageFileName;
				}
			}

		}
	}

	private void OnCancelButtonClicked (object o, EventArgs args)
	{
		this.Destroy();
	}

	private void OnOkButtonClicked (object o, EventArgs args)
	{
		Exporter exporter = new Exporter (catalog);
		exporter.Template = template;
		if (exporter.Export (outputFileEntry.Filename)) {
			Gtk.Dialog dialog = new MessageDialog (null,
					DialogFlags.Modal,
					MessageType.Info,
					ButtonsType.Ok,
					Mono.Posix.Catalog.GetString ("Catalog succesfully exported"));
			dialog.Run();

			dialog.Destroy ();
		}
		else {
			Gtk.Dialog dialog = new MessageDialog (null,
					DialogFlags.Modal,
					MessageType.Error,
					ButtonsType.Close,
					Mono.Posix.Catalog.GetString ("Some error while exporting the catalog"));
			dialog.Run();

			dialog.Destroy ();
		}
		
		this.Destroy();
	}
}
