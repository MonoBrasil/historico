/*
MonoUML.Widgets.Toolbar - Canvas' Toolbars
Copyright (C) 2005  Mario Carrión

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
using Gdk;
using MonoUML.Widgets;
using MonoUML.Widgets.UML2;
using MonoUML.I18n;
using System;

namespace MonoUML.Widgets.Toolbar
{

	public abstract class ToolbarBase : Gtk.Toolbar
	{
	
		public ToolbarBase (UMLDiagram diagram) : base ()
		{
			_diagram = diagram;
			ToolbarStyle = ToolbarStyle.Icons;
			Tooltips = true;
        	_tooltips  = new Tooltips ();

        	_tbuttonGrid = new ToggleToolButton ();
			_tbuttonGrid.IconWidget = new Gtk.Image (MonoUML.IconLibrary.PixbufLoader.GetIcon ("grid_tbar.png"));
			_tbuttonGrid.SetTooltip (_tooltips, GettextCatalog.GetString ("Show grid"), GettextCatalog.GetString ("Show grid"));
			
        	_tbuttonSnap2Grid = new ToggleToolButton ();
			_tbuttonSnap2Grid.IconWidget = new Gtk.Image (MonoUML.IconLibrary.PixbufLoader.GetIcon ("snap2grid_tbar.png"));
			_tbuttonSnap2Grid.SetTooltip (_tooltips, GettextCatalog.GetString ("Snap to grid"), GettextCatalog.GetString ("Snap to grid"));

			Insert (_tbuttonGrid, -1);
			Insert (_tbuttonSnap2Grid, -1);
			InsertSeparator ();
			
			DrawIcons ();
		}

		public bool Grid
		{
			get
			{
				return _tbuttonGrid.Active;
			}
			set
			{
				_tbuttonGrid.Active = value;
			}
		}
		
		public bool SnapToGrid
		{
			get
			{
				return _tbuttonSnap2Grid.Active;
			}
			set
			{
				_tbuttonSnap2Grid.Active = value;
			}
		}
		
		public ToggleToolButton ButtonGrid
		{
			get
			{
				return _tbuttonGrid;
			}
		}
		
		protected void InsertSeparator ()
		{
			Gtk.ToolItem item = new ToolItem ();
			item.Child = new VSeparator ();
			Insert (item, -1);
		}
		
		protected Gtk.RadioToolButton CreateRadioToolButton (string pixbuf, string tooltip)
		{
			Gtk.RadioToolButton rtn = new Gtk.RadioToolButton (new GLib.SList (typeof (RadioToolButton)));
			rtn.IconWidget = new Gtk.Image (MonoUML.IconLibrary.PixbufLoader.GetIcon (pixbuf));
			rtn.SetTooltip (_tooltips, tooltip, tooltip);
			return rtn;
		}
		
		protected Gtk.ToolButton CreateToolButton (string pixbuf, string tooltip)
		{
			ToolButton rtn = new Gtk.ToolButton ("");
			rtn.IconWidget = new Gtk.Image (MonoUML.IconLibrary.PixbufLoader.GetIcon (pixbuf));
			rtn.SetTooltip (_tooltips, tooltip, tooltip);
			return rtn;
		}
		
		protected Gtk.ToggleToolButton CreateToggleToolButton (string pixbuf, string tooltip)
		{
			ToggleToolButton rtn = new Gtk.ToggleToolButton ("");
			rtn.IconWidget = new Gtk.Image (MonoUML.IconLibrary.PixbufLoader.GetIcon (pixbuf));
			rtn.SetTooltip (_tooltips, tooltip, tooltip);
			return rtn;
		}

		protected abstract void DrawIcons ();
		
		protected UMLDiagram _diagram;
		protected Tooltips _tooltips;
		protected ToggleToolButton _tbuttonGrid;
		protected ToggleToolButton _tbuttonSnap2Grid;
	}
}