/*
MonoUML.IconLibrary - A library for representing the Widget elements
Copyright (C) 2004 -2005 Mario Carrión

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
using System;
using System.Collections;

namespace MonoUML.IconLibrary
{
	public static class PixbufLoader
	{
		static PixbufLoader ()
		{
			_icons = new Hashtable ();
			_icons.Add ("unknown_tree", Gdk.Pixbuf.LoadFromResource ("unknown_tree.png"));
			_icons.Add ("unknown_dnd", Gdk.Pixbuf.LoadFromResource ("unknown_dnd.png"));
			_icons.Add ("no_dnd", Gdk.Pixbuf.LoadFromResource ("no_dnd.png"));
		}
		
		public static Gdk.Pixbuf GetIconDnD (string fname)
		{
			return GetIcon (fname, "no_dnd");
		}

		public static Gdk.Pixbuf GetIcon (string fname)
		{
			return GetIcon (fname, "unknown_tree");
			/*Gdk.Pixbuf pixbuf = (Gdk.Pixbuf) _icons [fname];
			if (pixbuf == null)
			{
				try
				{
					pixbuf = Gdk.Pixbuf.LoadFromResource (fname);
					_icons.Add (fname, pixbuf);
				}
				catch (Exception ex)
				{
					System.Console.WriteLine ("Null pixbuf: "+fname+" using default");
					pixbuf = (Gdk.Pixbuf) _icons ["unknown_tree"]; 
				}
			}
			return pixbuf;*/
		}
		
		private static Gdk.Pixbuf GetIcon (string fname, string defPixbuf)
		{
			Gdk.Pixbuf pixbuf = (Gdk.Pixbuf) _icons [fname];
			if (pixbuf == null)
			{
				try
				{
					pixbuf = Gdk.Pixbuf.LoadFromResource (fname);
					_icons.Add (fname, pixbuf);
				}
				catch (Exception ex)
				{
					System.Console.WriteLine ("Null pixbuf: "+fname+" using default");
					pixbuf = (Gdk.Pixbuf) _icons [defPixbuf]; 
				}
			}
			return pixbuf;
		}

		private static Hashtable _icons; 
	}
}
