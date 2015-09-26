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

	public class ToolbarClass : ToolbarBase
	{
	
		public ToolbarClass (UMLDiagram diagram) : base (diagram)
		{
			_actionClass = new CreateClassAction (diagram);
			_tbuttonClass.Clicked += QueueClass;
		}
		
		protected override void DrawIcons ()
		{
        	_tbuttonClass = new Gtk.ToolButton ("");
			_tbuttonClass.IconWidget = new Gtk.Image (MonoUML.IconLibrary.PixbufLoader.GetIcon ("class_tree.png"));
			
			_tbuttonClass.SetTooltip (_tooltips, GettextCatalog.GetString ("Add a class"), GettextCatalog.GetString ("Add a class"));
			Insert (_tbuttonClass, -1);
		}
		
		private void QueueClass (object sender, EventArgs e)
		{
			_diagram.UMLCanvas.QueueAction = _actionClass;
		}
		
		private Gtk.ToolButton _tbuttonClass;
		private CreateClassAction _actionClass;
	}
}