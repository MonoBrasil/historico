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
using System.Net;
using System.Collections;

using Gdk;
using Gtk;
using Glade;

using Mono.Posix;

public class GladeApp
{
	[Glade.Widget] private Gnome.App app1;
	private Gnome.Program program;
	private Database database;
	private CatalogList catalogList;
	private Catalog activeCatalog;
	private BorrowerList borrowerList;

	// Lists widgets
	[Glade.Widget] private Notebook notebook;
	[Glade.Widget] private ScrolledWindow swCatalogs;
	[Glade.Widget] private ScrolledWindow swBorrowers;
	[Glade.Widget] private ScrolledWindow swItemsList;
	[Glade.Widget] private ScrolledWindow swItemsShelf;
	[Glade.Widget] private ScrolledWindow swPresentation;
	[Glade.Widget] private HPaned itemListPaned;
	[Glade.Widget] private HPaned catalogListPaned;
	[Glade.Widget] private VPaned borrowersPaned;
	[Glade.Widget] private Frame borrowersFrame;

	private TitleWidget titleWidget;
	//	private TitleWidgetOrderChangedHandler titleWidgetOrderChangedHandler;
	private View activeView;

	private Presentation presentation;
	private ItemShelf itemShelf;
	private bool itemShelfInitialized = false;
	private ItemList itemList;

	// Buttons
	[Glade.Widget] private MenuItem menuItemAddItem;
	[Glade.Widget] private MenuItem menuItemRemoveItem;
	[Glade.Widget] private RadioMenuItem menuItemShelfView;
	[Glade.Widget] private RadioMenuItem menuItemListView;
	private ToggleButton buttonList;
	private ToggleButton buttonShelf;

	[Glade.Widget] private Button zoomInButton;
	[Glade.Widget] private Button zoomOutButton;
	[Glade.Widget] private Button addItemButton;
	[Glade.Widget] private Button removeItemButton;

	//	[Glade.Widget] private Button addBorrowerButton;
	//	[Glade.Widget] private Button removeBorrowerButton;

	[Glade.Widget] private Button editItemButton;
	[Glade.Widget] private Button lendItemButton;

	[Glade.Widget] private Gtk.Entry searchEntry;
	// searchOn: true if a search is active.
	private bool searchOn;

	public static void Main (string[] args)
	{
		new GladeApp (args);
	}

	public GladeApp (string[] args) 
	{
		program = new Gnome.Program ("mCatalog", "1.0", Gnome.Modules.UI, args);
		Mono.Posix.Catalog.Init ("mcatalog", Defines.GNOME_LOCALE_DIR);

		// Proxy Setup
		bool use_proxy = Conf.Get ("/system/http_proxy/use_http_proxy", false);

		if (use_proxy) {
			string proxy_host = Conf.Get ("/system/http_proxy/host", "");
			int proxy_port = Conf.Get ("/system/http_proxy/port", 8080);
			string proxy = string.Format("http://{0}:{1}/", proxy_host, proxy_port);
			WebProxy proxyObject = new WebProxy(proxy, true);
			System.Net.GlobalProxySelection.Select = proxyObject;
		}

		database = new Database (Conf.HomeDir+"/db.db");
		database.Debug = true;

		Glade.XML gxml = new Glade.XML (null, "mainwindow.glade", "app1", "mcatalog");
		gxml.Autoconnect (this);
		app1.DeleteEvent += OnWindowDeleteEvent;

		presentation = new Presentation ();
		itemShelf = new ItemShelf (presentation);
		swPresentation.AddWithViewport (presentation);
		presentation.Init ();

		searchEntry.Activated += OnSearchEntryActivated;

		itemListPaned.SizeRequested += OnItemListPanedResized;

		// Fill the list hbox
		HBox hBoxList = (HBox)gxml["hBoxList"];
		buttonList = new ToggleButton ();
		buttonList.Clicked += OnButtonListClicked;
		Gtk.Image image1 = new Gtk.Image (new Gdk.Pixbuf (null, "list.png"));
		image1.Visible = true;
		buttonList.Add (image1);
		buttonList.Relief = ReliefStyle.Half;
		hBoxList.PackStart (buttonList, false, true, 0);
		buttonShelf = new ToggleButton ();
		buttonShelf.Clicked += OnButtonShelfClicked;
		Gtk.Image image2 = new Gtk.Image (new Gdk.Pixbuf (null, "shelf.png"));
		image2.Visible = true;
		buttonShelf.Add (image2);
		buttonShelf.Relief = ReliefStyle.Half;
		hBoxList.PackStart (buttonShelf, false, true, 0);
		titleWidget = new TitleWidget ();
		titleWidget.OnOrderChanged += OnOrderChanged;
		hBoxList.PackStart (titleWidget, true, true, 4);
		hBoxList.ShowAll();

		// Get the menu items we need to handle
		menuItemAddItem.Sensitive = false;
		menuItemRemoveItem.Sensitive = false;
		menuItemShelfView.Data["view"] = View.Shelf;
		menuItemListView.Data["view"] = View.List;

		menuItemShelfView.Toggled += OnViewToggled;

		// Buttons
		addItemButton.Sensitive = false;
		removeItemButton.Sensitive = false;
		lendItemButton.Sensitive = false;
		editItemButton.Sensitive = false;

		lendItemButton.Clicked += LendOrReturnItem;
		editItemButton.Clicked += EditItem;

		// Populate the catalog tree
		PopulateCatalogs ();
		PopulateBorrowers ();

		app1.ShowAll();
		RestoreWindowState ();
		program.Run();
	}

	public void PopulateCatalogs ()
	{
		catalogList = new CatalogList();
		swItemsShelf.SizeAllocated    += itemShelf.SizeAllocate;
		catalogList.Selection.Changed += OnCatalogActivated;

		swCatalogs.Add (catalogList);

		int i=0;
		int sel = 0;
		string active = Conf.Get("active_catalog", "books");
		foreach (Catalog catalog in database.Catalogs()) {
			catalogList.AddCatalog (catalog);
			if (catalog.Name.Equals(active)) {
				sel = i;
			}
			i++;
		}    
		catalogList.SetSelectedCatalog(sel);
	}

	private void PopulateBorrowers ()
	{
		borrowerList = new BorrowerList (database);
		borrowerList.OnBorrowerSelected += BorrowerSelected;
		swBorrowers.Add (borrowerList);	
	}

	private void OnAddBorrowerButtonClicked (object o, EventArgs args)
	{
		Console.WriteLine ("ADD");
	}

	private void OnRemoveBorrowerButtonClicked (object o, EventArgs args)
	{
		Console.WriteLine ("REMOVE");
	}

	private void OnPreferencesActivated (object o, EventArgs args)
	{
		new PreferencesDialog (itemShelf);
	}

	// Change the view mode
	private void OnViewToggled (object o, EventArgs args)
	{
		ViewRadioMenuItemChanged((RadioMenuItem)o);
	}

	private void OnButtonShelfClicked (object o, EventArgs args)
	{
		if (buttonShelf.Active) {
			SetView (View.Shelf);
			buttonList.Active = false;
		}
	}

	private void OnButtonListClicked (object o, EventArgs args)
	{
		if (buttonList.Active) {
			SetView (View.List);
			buttonShelf.Active = false;
		}
	}

	private void ViewRadioMenuItemChanged (RadioMenuItem radio)
	{
		GLib.SList list = radio.Group;
		foreach (RadioMenuItem item in list) {
			if (item.Active) {
				if (activeView != (View)item.Data["view"]) {
					SetView ((View)item.Data["view"]);
				}
				break;
			}
		}
	}

	private void SetView (View view)
	{
		SetView (view, false);
	}

	private void SetView (View view, bool force)
	{
		if (activeView != view || force) {
			activeView = view;

			database.SetCatalogView (activeCatalog.Name, view);
			if (view == View.List) {
				if (swItemsList.Child != null) {
					swItemsList.Remove (swItemsList.Child);
				}
				swItemsList.Add (itemList);
				notebook.CurrentPage = 0;
				zoomInButton.Visible = false;
				zoomOutButton.Visible = false;
				menuItemListView.Activate();
				buttonList.Active = true;
			}
			if (view == View.Shelf) {
				notebook.CurrentPage = 1;
				zoomInButton.Visible = true;
				zoomOutButton.Visible = true;
				menuItemShelfView.Activate();
				buttonShelf.Active = true;
			}

			titleWidget.SetView (view);
			titleWidget.Table = activeCatalog.ShortDescription;
			titleWidget.Fields = database.GetColumns (activeCatalog.Table);
			titleWidget.Order = activeCatalog.Order;
		}
	}

	private void OnZoomInActivated (object o, EventArgs args)
	{
		itemShelf.Zoom += 0.2;
	}

	private void OnZoomOutActivated (object o, EventArgs args)
	{
		// Prevent Zoom == 0 badness
		if(itemShelf.Zoom > 0.2)
			itemShelf.Zoom -= 0.2;
	}

	private void OnNormalZoomActivated (object o, EventArgs args)
	{
		itemShelf.Zoom = 1;
	}

	// Event fired when a catalog has been selected
	private void OnCatalogActivated (object o, EventArgs args)
	{
		presentation.Init();
		Catalog newActiveCatalog = catalogList.GetSelectedCatalog();
		if (newActiveCatalog != null) {
			activeCatalog = newActiveCatalog;
			
			itemList = database.LoadItemList(activeCatalog, presentation);
			itemList.OnItemSelected      += OnItemSelectionChanged;
			itemList.OnLendItemRequest   += LendItem;
			itemList.OnReturnItemRequest += ReturnItem;
			itemList.OnEditItemRequest   += EditItem;
			itemList.OnItemDeleted       += DeleteItems;

			itemShelf.Catalog = activeCatalog;
			if (!itemShelfInitialized) {
				itemShelfInitialized = true;

				swItemsShelf.AddWithViewport (itemShelf);

				itemShelf.OnItemSelected      += OnItemSelectionChanged;
				itemShelf.OnLendItemRequest   += LendItem;
				itemShelf.OnReturnItemRequest += ReturnItem;
				itemShelf.OnEditItemRequest   += EditItem;
				itemShelf.OnNewItemRequest    += AddItem;
				itemShelf.OnItemDeleted       += DeleteItems;
			}

			menuItemAddItem.Sensitive = true;
			addItemButton.Sensitive = true;
			SetView (database.GetCatalogView (activeCatalog.Name), true);

			activeCatalog.LoadAll();

			Conf.Set("active_catalog", activeCatalog.Name);
			if (borrowerList!=null) {
				borrowerList.Selection.UnselectAll ();
			}
		}
		else {
			menuItemAddItem.Sensitive = false;
			addItemButton.Sensitive = false;
			titleWidget.Table = "";
		}
	}

	private void BorrowerSelected (object o, EventArgs args)
	{
		Borrower borrower = (Borrower)o;
		activeCatalog.ItemCollection.LoadBorrowerList(borrower);
		string title = String.Format (Mono.Posix.Catalog.GetString ("{0} borrower by {1}"),activeCatalog.ShortDescription, borrower.Name);
		titleWidget.Table = title; 
		catalogList.Selection.UnselectAll ();
	}

	private void OnOrderChanged (Order order)
	{
		activeCatalog.Order = order;
	}


	private void BorrowerListChanged (object o, EventArgs args)
	{
		if (borrowerList.Count() == 0) {
			borrowersFrame.Visible = false;
		}
		else {
			borrowersFrame.Visible = true;
		}

		OnItemSelectionChanged (o, args);
	}

	private void OnItemSelectionChanged (object o, EventArgs args)
	{
		ArrayList items = activeCatalog.ItemCollection.GetSelectedItems();
		if (items.Count > 0) {
			menuItemRemoveItem.Sensitive = true;
			removeItemButton.Sensitive = true;

			if (items.Count == 1) {
				lendItemButton.Sensitive = true;
				editItemButton.Sensitive = true;

				Item selectedItem = (Item)items[0];
				if (database.IsBorrowed (selectedItem)) {
					lendItemButton.Label = Mono.Posix.Catalog.GetString ("Return");
				}
				else {
					lendItemButton.Label = Mono.Posix.Catalog.GetString ("Lend");
				}
			}
			else {
				lendItemButton.Sensitive = false;
				editItemButton.Sensitive = false;
			}
		}
		else {
			menuItemRemoveItem.Sensitive = false;
			removeItemButton.Sensitive = false;
			lendItemButton.Sensitive = false;
			editItemButton.Sensitive = false;
		}
	}

	// Add a new item to the active catalog
	private void OnAddItemToolbarButtonClicked ()
	{
		AddItem (null, null);
	}

	private void OnAddItemButtonClicked (object o, EventArgs args)
	{
		AddItem (null, null);
	}

	private void AddItem (object o, EventArgs args)
	{
		activeCatalog.OpenAddItemDialog();
	}

	private void EditItem (object o, EventArgs args)
	{
		activeCatalog.EditSelectedItem();
	}

	private void LendOrReturnItem (object o, EventArgs args)
	{
		ArrayList items = activeCatalog.ItemCollection.GetSelectedItems();
		if (items.Count > 0) {
			Item selectedItem = (Item)items[0];
			if (database.IsBorrowed (selectedItem)) {
				ReturnItem (null, null);
			}
			else {
				LendItem (null, null);
			}
		}
	}

	private void LendItem (object o, EventArgs args)
	{
		activeCatalog.LendSelectedItem();
	}

	private void ReturnItem (object o, EventArgs args)
	{
		activeCatalog.ReturnSelectedItem();
	}

	// Delete the active item
	private void OnDeleteItemToolbarButtonClicked ()
	{
		DeleteItems (null, null);
	}

	private void OnDeleteItemButtonClicked (object o, EventArgs args)
	{
		DeleteItems (null, null); 
	}

	private void DeleteItems (object o, EventArgs args)
	{
		Gtk.Dialog dialog = new MessageDialog (null,
				DialogFlags.Modal,
				MessageType.Question,
				ButtonsType.OkCancel,
				Mono.Posix.Catalog.GetString ("Remove the selected items?"));
		ResponseType response = (ResponseType)dialog.Run();

		if (response == ResponseType.Ok) {
			ArrayList selected = activeCatalog.ItemCollection.GetSelectedItems ();
			foreach (Item item in selected) {
				if (database.IsBorrowed (item)) {
					database.ReturnItem (item);
					activeCatalog.BorrowerListChanged ();
				}
				activeCatalog.RemoveItem (item);
			}
		}

		dialog.Destroy();
	}

	private void OnExportActivate (object o, EventArgs args)
	{
		new ExportDialog (activeCatalog, searchOn);
	}

	private void OnSearchEntryActivated (object o, EventArgs args)
	{
		catalogList.Search (searchEntry.Text);
		searchOn = searchEntry.Text.Equals ("");
	}

	private void OnQuitActivateEvent (object o, EventArgs args)
	{
		Exit ();
	}

	private void OnWindowDeleteEvent (object o, DeleteEventArgs args) 
	{
		Exit ();
		args.RetVal = true;
	}

	private void OnItemListPanedResized (object o, SizeRequestedArgs args)
	{
		Gdk.Rectangle rect = ((Paned)o).Allocation;
		swItemsShelf.SetSizeRequest (rect.Width, rect.Height);
	}

	private void RestoreWindowState()
	{
		int width, height;

		width = Conf.Get ("ui/main_window_width", 600);
		height = Conf.Get("ui/main_window_height", 400);

		if (width == Int32.MaxValue) {
			app1.Maximize();
		}
		else {
			app1.Resize(width, height);
		}

		width = Conf.Get ("ui/catalog_list_width", 130);
		catalogListPaned.Position = width;

		width = Conf.Get ("ui/item_list_width", 360);
		itemListPaned.Position = width;

		height = Conf.Get ("ui/borrowers_list_height", 100);
		borrowersPaned.Position = height;

		if (borrowerList.Count() == 0) {
			borrowersFrame.Visible = false;
		}

		app1.WindowPosition = WindowPosition.CenterAlways;
	}

	private void SaveWindowState()
	{
		int height, width;

		if (app1.GdkWindow.State == Gdk.WindowState.Maximized) {
			Conf.Set("ui/main_window_width", Int32.MaxValue);
		}
		else {
			app1.GetSize(out width, out height);
			Conf.Set("ui/main_window_width", width);
			Conf.Set("ui/main_window_height", height);
		}

		Conf.Set("ui/catalog_list_width", catalogListPaned.Position);
		Conf.Set("ui/item_list_width", itemListPaned.Position);
		Conf.Set ("ui/borrowers_list_height", borrowersPaned.Position);

		Conf.Sync ();
	}

	private void ShowAboutDialog (System.Object o, EventArgs e)
	{
		string [] authors = new string [] {
			"Cesar Garcia Tapia (tapia@mcatalog.net)"
		};

		string [] documenters = new string [] {};
		string translaters = Mono.Posix.Catalog.GetString ("translator-credits");
		Pixbuf pixbuf = null;

		Gnome.About about = new Gnome.About ("MCatalog", Defines.VERSION,
				"Copyright (C) 2003, Cesar Garcia Tapia",
				Mono.Posix.Catalog.GetString ("Media cataloger"),
				authors, documenters, translaters, pixbuf);
		about.Show ();
	}

	private void Exit ()
	{
		SaveWindowState();
		database.Close();
		program.Quit();
	}
}
