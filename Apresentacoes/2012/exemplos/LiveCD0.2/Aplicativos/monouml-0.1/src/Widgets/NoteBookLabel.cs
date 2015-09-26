/*
MonoUML.Widgets - A library for representing the Widget elements
Copyright (C) 2004  Mario Carrión <mario.carrion@gmail.com>

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
using Gtk;
using System;
using DI = MonoUML.DI;
using MonoUML.I18n;

namespace MonoUML.Widgets
{
	public class NoteBookLabel : Gtk.Table
	{	
		public NoteBookLabel(NoteBook parent, DI.Diagram diagram) : this(diagram)
		{
			_parent = parent;
		}
		
		public string Label
		{
			set
			{
				_label.Text = value;
			}
		}
	
		// Constructor
		private NoteBookLabel(DI.Diagram diagram) : base(1, 3, false)
		{
			_parent = null;
			_diagram = diagram;
			//this must change depending of the diagram's type
			string diagramType = ((DI.SimpleSemanticModelElement)diagram.SemanticModel).TypeInfo;
			_icon = GetIcon(diagramType);
			//_icon = new Gdk.Pixbuf (new Gdk.Colorspace(), false, 8, 15, 15);
			//_icon.Fill (0xffff0000);
			//
			Attach(new Gtk.Image(_icon), 0, 1, 0, 1);
			//
			_label = new Label(_diagram.Name);
			Attach(_label, 1, 2, 0, 1);
			//
			Image image = new Image();
			image.Stock = Gtk.Stock.Close;
			_close_button = new Button();
			_close_button.Add(image);
			_close_button.HeightRequest = 20;
			_close_button.WidthRequest = 20;
			_close_button.Relief = Gtk.ReliefStyle.None;
			_close_button.Clicked += OnCloseButtonClicked;
			Tooltips ttips = new Tooltips ();
			ttips.SetTip (_close_button, GettextCatalog.GetString ("Close diagram"), GettextCatalog.GetString ("Close diagram"));
			//_close_button. 
			Attach(_close_button, 2, 3, 0, 1);
			ShowAll();
		}

		private Gdk.Pixbuf GetIcon(string baseName)
		{
			Gdk.Pixbuf icon = MonoUML.IconLibrary.PixbufLoader.GetIcon (baseName.ToLower() + "_tree.png");
			return icon;
		}
		
		// Closes current page
		private void OnCloseButtonClicked(object o, EventArgs args)
		{
			if (_parent != null)
			{
				_parent.RemoveDiagram(_diagram);
			}
		}

		private Gdk.Pixbuf _icon;
		private Label _label;
		private Button _close_button;
		private NoteBook _parent;
		private DI.Diagram _diagram;

	}
}
