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
using System.Text;
using System.Collections;

using Gtk;
using Gdk;

using Mono.Posix;

using Gecko;

public class ItemShelf: WebControl 
{
	public event EventHandler OnEditItemRequest;
	public event EventHandler OnLendItemRequest;
	public event EventHandler OnNewItemRequest;
	public event EventHandler OnReturnItemRequest;
	public event EventHandler OnItemSelected;
	public event EventHandler OnItemDeleted;
	
	private static int DEFAULT_ELEMENT_SIZE = 110;
	private int ELEMENT_SIZE = 110;

	private int verticalPos = 10;
	private int horizontalPos = 10;

	private ScrolledWindow swPresentation;
	private Presentation presentation;
	
	private Catalog catalog;
	private ArrayList elements;
	
	private string background;
	private string shelf;
	
	public ItemShelf (Presentation presentation): base (Conf.HomeDir, "gecko")
	{
		this.Visible = true;
		this.elements = new ArrayList();
		this.presentation = presentation;

		string themePath = Defines.IMAGE_DATADIR + "/" + Conf.Get ("ui/theme", "default");
		background = themePath + "/background.jpg";
		shelf = themePath + "/shelf.png";
	 
		OpenUri += OnOpenUri;
		DomMouseDblClick += OnDoubleClick;
	}

	public void ChangeTheme (string theme)
	{
		Conf.Set("ui/theme", theme);
		string themePath = Defines.IMAGE_DATADIR + "/" + Conf.Get ("ui/theme", "default");
		background = themePath + "/background.jpg";
		shelf = themePath + "/shelf.png";
		Render ();
	}
	
	public Catalog Catalog {
		set {
			this.catalog = value;
			catalog.OnItemCollectionChanged += Refresh;
			this.Zoom = this.Zoom;
		}
		get {
			return catalog;
		}
	}
		
	private void LoadAll ()
	{
		foreach (Item item in catalog.ItemCollection) {
			this.AddItem (item);
		}
	}

	public double Zoom {
		get {
			return catalog.Zoom;
		}
		set {
			catalog.Zoom = value;
			ELEMENT_SIZE = (int)(DEFAULT_ELEMENT_SIZE * value);
			Render ();
		}
	}
	
	private void AddItem (Item item)
	{
		ItemShelfElement element = new ItemShelfElement (item);
		if (catalog.IsBorrowed (item)) {
			element.IsBorrowed = true;
		}
						
		int pos = elements.Add (element);
		element.Position = pos;
	}

	private void Render ()
	{   
		horizontalPos = 10;
		verticalPos = 10;

		StringBuilder sb = new StringBuilder ("");

		sb.Append ("<html>");
		sb.Append ("<head>");
		sb.Append ("<style>");
		sb.Append ("body {");
		sb.Append ("	font-family: sans-serif;");
		sb.Append ("	margin: 0px;");
		sb.Append ("	height: 100%;");
		sb.Append ("	padding: 0px;");
		sb.Append ("	 background-image: url(\"");
		sb.Append (background);
		sb.Append ("\");");
		sb.Append ("	 background-repeat: repeat;");
		sb.Append ("}");
		sb.Append ("</style>");
		sb.Append ("<script language=\"JavaScript1.2\">");
		sb.Append ("var counter=1;");
		sb.Append ("var prefix=1;");
		sb.Append ("var h;");
		sb.Append ("var elementos = new Array();");
		foreach (ItemShelfElement element in elements) {
			sb.Append ("elementos[\"item");
			sb.Append (element.Item.Id.ToString());
			sb.Append ("\"] = false;");
		}
		sb.Append ("function zoomhelper(){");
		sb.Append ("	if (counter <=5) {");
		sb.Append ("		prefix=1;");
		sb.Append ("	}");
		sb.Append ("	else if (counter <= 10) {");
		sb.Append ("		prefix=-1;");
		sb.Append ("	}");
		sb.Append ("	else {");
		sb.Append ("		counter = 0;");
		sb.Append ("		clearzoom();");
		sb.Append ("		return;");
		sb.Append ("	}");
		sb.Append ("	image.style.width=parseInt(image.style.width)+1*prefix;");
		sb.Append ("	image.style.height=parseInt(image.style.height)+1*prefix;");
		sb.Append ("	counter = counter + 1;");
		sb.Append ("}");
		sb.Append ("function zoom(originalH, whatcache){");
		sb.Append ("	h = originalH;");
		sb.Append ("	if (!window.beginzoom) {");
		sb.Append ("		image = document.getElementById (whatcache);");
		sb.Append ("		if (image.style.width==\"\"){");
		sb.Append ("			image.style.height= h;");
		sb.Append ("		}");
		sb.Append ("		beginzoom=setInterval(\"zoomhelper()\",10);");
		sb.Append ("	}");
		sb.Append ("}");
		sb.Append ("function clearzoom(){");
		sb.Append ("	image.style.height= h;");
		sb.Append ("	clearInterval(beginzoom);");
		sb.Append ("	beginzoom=false;");
		sb.Append ("}");
		sb.Append ("function onclicked (event, whatcache) {");
		sb.Append ("	if (!event.ctrlKey) {");
		sb.Append ("		var i=0;");
		sb.Append ("		var k=0;");
		sb.Append ("		for (a in elementos) i++;");
		sb.Append ("		for (k=1;k<=i;k++) {");
		sb.Append ("			str = \"item\"+k;");
		sb.Append ("			if (str != whatcache && elementos[str]) {");
		sb.Append ("				elementos[str] = false;");
		sb.Append ("				image = document.getElementById (str);");
		sb.Append ("				image.style.border=\"0\";");
		sb.Append ("			}");
		sb.Append ("		}");
		sb.Append ("	}");
		sb.Append ("	image = document.getElementById (whatcache);");
		sb.Append ("	if (elementos[whatcache]) {");
		sb.Append ("		image.style.border=\"0\";");
		sb.Append ("	}");
		sb.Append ("	else {");
		sb.Append ("		image.style.border= \"medium solid blue\";");
		sb.Append ("	}");
		sb.Append ("	elementos[whatcache] = !elementos[whatcache];");
		sb.Append ("}");
		sb.Append ("</script>");
		sb.Append ("</head>");
		sb.Append ("<body>");
		sb.Append ("<table cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" border=\"0\">");
		sb.Append ("<tr align=\"center\">");

		foreach (ItemShelfElement element in elements) {
			sb.Append (DrawWidget (element));
		}
	
		sb.Append ("<tr><td height=\"");
		sb.Append ((ELEMENT_SIZE+10).ToString());
		sb.Append ("\"></tr>");
		sb.Append ("<tr><td colspan=\"100\" height=\"25\" background=\"");
		sb.Append (shelf);
		sb.Append ("\"></td></tr>");
		sb.Append ("</table>");
		sb.Append ("</body>");
		sb.Append ("</html>");

		string mime_type = "text/html";
		base.OpenStream ("file:///", mime_type);
		base.AppendData (sb.ToString());
		base.CloseStream ();
	}
	
	private string DrawWidget (ItemShelfElement element)
	{
		StringBuilder render = new StringBuilder ("");

		if (horizontalPos + ELEMENT_SIZE*2 + 10 >= this.Allocation.Width) {
			render.Append (element.Render (horizontalPos, verticalPos, ELEMENT_SIZE));

			render.Append ("<tr><td height=\"");
			render.Append ((ELEMENT_SIZE+10).ToString());
			render.Append ("\"></tr>");
			render.Append ("<tr><td colspan=\"100\" height=\"25\" background=\"");
			render.Append (shelf);
			render.Append ("\"></td></tr>");
			
			horizontalPos = 10;
			verticalPos += ELEMENT_SIZE + 35;
		}
		else {	
			render.Append (element.Render (horizontalPos, verticalPos, ELEMENT_SIZE));
			horizontalPos += ELEMENT_SIZE + 20;
		}

		return render.ToString ();
	}

	private void OnOpenUri (object o, OpenUriArgs args)
	{
		string uri = args.AURI;

		args.RetVal = true;

		if (uri.StartsWith ("item://")) {
			int itemPos = Int32.Parse (uri.Substring (7));

			ItemShelfElement element = (ItemShelfElement) elements[itemPos];
			presentation.Load (catalog.Table, element.Item);
			catalog.ItemCollection.UnselectAll ();
			element.Item.IsSelected = true;
			OnItemSelected (null, null);
		}
	}

	private void OnDoubleClick (object o, DomMouseDblClickArgs args)
	{
		if (LinkMessage != null && LinkMessage.StartsWith ("item://")) {
			OnEditItemRequest (null, null);
		}	
		else {
			OnNewItemRequest (null, null);
		}
		args.RetVal = 1;
	}
	
	private void Refresh () {
		if (this.IsRealized) {
			Clear();
			LoadAll ();
			Render ();
		}
	}

	public void SizeAllocate (object o, SizeAllocatedArgs args)
	{
		if (base.IsRealized) {
			Render ();
		}
	}

	
	private void Clear ()
	{
		elements.Clear();
	}

	public ScrolledWindow PresentationWidget
	{
		get {
			return this.swPresentation;
		}
		set {
			this.swPresentation = value;
		}
	}
/*
	private void ShowPopup (ItemShelfElement element)
	{
		Gtk.Menu menu = new Gtk.Menu ();
		menu.AccelGroup = new AccelGroup ();
		Gtk.MenuItem editMenuItem = new Gtk.MenuItem (Mono.Posix.Catalog.GetString ("Edit"));
		
		Gtk.MenuItem lendMenuItem;
		if (catalog.IsBorrowed (element.Item)) {
			lendMenuItem = new Gtk.MenuItem (Mono.Posix.Catalog.GetString ("Return"));
			lendMenuItem.Activated  += OnReturnMenuItemClicked;
		}
		else {
			 lendMenuItem = new Gtk.MenuItem (Mono.Posix.Catalog.GetString ("Lend"));
			 lendMenuItem.Activated  += OnLendMenuItemClicked;
		}
		
		Gtk.ImageMenuItem removeMenuItem = new Gtk.ImageMenuItem (Gtk.Stock.Remove, menu.AccelGroup);
		Gtk.SeparatorMenuItem separator = new Gtk.SeparatorMenuItem();

		editMenuItem.Activated += OnEditMenuItemClicked;
		removeMenuItem.Activated += OnRemoveMenuItemClicked;

		menu.Append (editMenuItem);
		menu.Append (lendMenuItem);
		menu.Append (separator);
		menu.Append (removeMenuItem);

		menu.Popup (null, null, null, IntPtr.Zero, 3, Gtk.Global.CurrentEventTime);	
		
		menu.ShowAll ();
	}
	
	private void OnEditMenuItemClicked (object o, EventArgs args)
	{
		OnEditItemRequest (null, null);
	}
	
	private void OnLendMenuItemClicked (object o, EventArgs args)
	{
		OnLendItemRequest(null, null);
	}
	
	private void OnReturnMenuItemClicked (object o, EventArgs args)
	{
		OnReturnItemRequest (null, null);
	}
	
	private void OnRemoveMenuItemClicked (object o, EventArgs args)
	{
		OnItemDeleted (null, null);
	}
*/
}

public class ItemShelfElement
{
	private string cover;
	private string background;
	private string borrowed;
	
	public Item Item;
	public int Position;

	public ItemShelfElement (Item item)
	{
		this.Item = item;

		if (item.LargeCover != null) {
			cover = item.LargeCover;
		}
		else {
			cover = Defines.IMAGE_DATADIR + "/shelfbgnp.png";
		}

		background = Defines.IMAGE_DATADIR + "/shelfbg.png";

		borrowed = null;
	}

	public bool IsBorrowed {
		get {
			return borrowed != null;
		}
		set {
			if (value) {
				borrowed = "borrowed.png";
			}
			else {
				borrowed = null;
			}
		}
	}

	public bool Selected {
		get {
			return this.Item.IsSelected;
		}
	}

	public string Render (int left, int top, int ELEMENT_SIZE) {
		StringBuilder render = new StringBuilder ();
                render.Append ("<div style=\"position:absolute;top:");
		render.Append (top.ToString ());
		render.Append ("px;left:");
		render.Append (left.ToString());
		render.Append ("px; width: ");
		render.Append (ELEMENT_SIZE.ToString());
		render.Append ("px; height: ");
		render.Append (ELEMENT_SIZE.ToString());
		render.Append ("px;\">");
		render.Append ("<div style=\"background-image:");
		render.Append (background);
		render.Append (";position:relative;\"><a href=\"item://");
		render.Append (Position.ToString());
		render.Append ("\">");
		render.Append ("<img style=\"z-index:-1\" id=\"item");
		render.Append (Item.Id.ToString());
		render.Append ("\" src=\"");
		render.Append (cover);
		render.Append ("\" border=\"0\"");
		render.Append ("\" height=\"");
		render.Append (ELEMENT_SIZE.ToString());
		render.Append ("\" onMouseOver=\"zoom(");
		render.Append (ELEMENT_SIZE.ToString());
		render.Append (", 'item");
		render.Append (Item.Id.ToString());
		render.Append ("')\"");
		render.Append (" onclick=\"onclicked(event, 'item");
		render.Append (Item.Id.ToString());
		render.Append ("')\">");
		render.Append ("</a></div>");
		
		if (this.IsBorrowed) {
			render.Append ("<div style=\"position:relative;\" width=\"");
			render.Append (ELEMENT_SIZE.ToString());
			render.Append ("\" height=\"");
			render.Append (ELEMENT_SIZE.ToString());
			render.Append ("\"><center><img style=\"position:relative;top:-5");
//			render.Append ((ELEMENT_SIZE-5).ToString());
			render.Append ("px; font-size:10; font-weight:bold;\" src=\"");
			render.Append (Defines.IMAGE_DATADIR+"/borrowed.png");
			render.Append ("\"></center></div>");
		}
		
		if (Item.CoverPixbuf == null) {
			render.Append ("<div style=\"position:relative;\"><label style=\"position:relative;top:-");
			render.Append ((ELEMENT_SIZE-5).ToString());
			render.Append ("px; left:5px; font-size:10; font-weight:bold;\">");
			render.Append ("<a style=\"text-decoration:none;color:#000000;\" href=\"item://");
			render.Append (Position.ToString());
			render.Append ("\">");
			render.Append (Item.Columns["author"]);
			render.Append ("<br>");
			render.Append (Item.Title+"</a></label></div>");
		}
		
		render.Append ("</div>");
		
		return render.ToString();
	}
}
