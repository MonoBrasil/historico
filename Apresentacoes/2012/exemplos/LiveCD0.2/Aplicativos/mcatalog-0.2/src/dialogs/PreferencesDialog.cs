using System;
using System.IO;

using Gtk;
using Glade;

using Mono.Posix;

public class PreferencesDialog: Dialog
{
	TreeStore themeStore;
	ItemShelf itemShelf;
	
	public PreferencesDialog(ItemShelf itemShelf): base ()
	{
		this.itemShelf = itemShelf;

		this.Title = Mono.Posix.Catalog.GetString ("Preferences");
		this.HasSeparator = false;
		this.SetDefaultSize (300, 200);

		Notebook notebook = new Notebook ();

		Glade.XML gxml = new Glade.XML (null, "themeselection.glade", "hbox1", null);
		HBox hBox = (HBox)gxml["hbox1"];
		ScrolledWindow scrolledwindow = (ScrolledWindow)gxml["scrolledwindow1"];
		TreeView themeTreeview = CreateThemeTreeView ();
		themeTreeview.Selection.Changed += OnThemeTreeViewSelectionChanged;

		scrolledwindow.Add (themeTreeview);
		
		notebook.AppendPage (hBox, new Label (Mono.Posix.Catalog.GetString ("Theme")));

		this.VBox.Add (notebook);
		
		Button closeButton = (Button)this.AddButton (Gtk.Stock.Close, 1);
		closeButton.Clicked += OnCloseButtonClicked;
		
		this.ShowAll();
	}
	
	private TreeView CreateThemeTreeView ()
	{
		PopulateStore ();
		TreeView tv = new TreeView (themeStore);
		tv.HeadersVisible = false;
		CellRendererText ct = new CellRendererText ();
		tv.AppendColumn (Mono.Posix.Catalog.GetString ("Theme"), ct, new TreeCellDataFunc (CellDataFunc));

		tv.Visible = true;
		
		return tv;
	}

	private void PopulateStore ()
	{
		themeStore = new TreeStore (typeof(string));
                foreach (string dir in Directory.GetDirectories (Defines.IMAGE_DATADIR)) {
                        themeStore.AppendValues (new DirectoryInfo(dir).Name);
                }
	}
	
	private void OnThemeTreeViewSelectionChanged (object o, EventArgs args)
	{
		TreeIter iter;
		TreeModel model;

		if (((TreeSelection)o).GetSelected (out model, out iter))
		{
			string val = (string) model.GetValue (iter, 0);
			itemShelf.ChangeTheme (val);
		}
	}
		
	private void CellDataFunc (Gtk.TreeViewColumn tree_column,
					Gtk.CellRenderer cell,
					Gtk.TreeModel tree_model,
					Gtk.TreeIter iter)
        {
                string val = (string) themeStore.GetValue (iter, 0);
                ((CellRendererText) cell).Text = val;
        }
	
	private void OnCloseButtonClicked (object o, EventArgs args)
	{
		this.Destroy();
	}
}
