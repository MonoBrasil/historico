/*
MonoUML.Widgets - MU's widgets :)
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
using System;
using Uml2 = MonoUML.Widgets.UML2;

namespace MonoUML.Widgets
{
	public class CanvasArea: Gtk.VBox
	{
		public CanvasArea () : base (false, 0)
		{	
			_notebook = new NoteBook (this); 
			PackEnd (_notebook);
			_toolbarWidget = null;
			ShowAll ();
		}
		
		public new void PackStart (Widget widget, bool b0, bool b1, uint i)
		{
			if (_toolbarWidget != null)
			{
				Remove (_toolbarWidget);
			}
			_toolbarWidget = widget;
			base.PackStart (widget, b0, b1, i);
			ShowAll ();
		}
		
		public new void Remove (Widget widget)
		{
			foreach (Widget wd in Children)
			{
				if (wd.Equals (widget))
				{
					base.Remove (widget);
				}
			}
		}
		
		public NoteBook NoteBook
		{
			get
			{
				return _notebook;
			}
		}
		
		#region Private variables
		private Widget _toolbarWidget;
		private NoteBook _notebook;
		#endregion
	}
}