/*
MonoUML.Widgets - A library for representing the Widget elements
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
using System;
using Gtk;
using MonoUML.I18n;
using Uml2 = MonoUML.Widgets.UML2;

namespace MonoUML.Widgets
{
	public abstract class ChooserDialog : Gtk.Dialog
	{
		public ChooserDialog (string title)
		{
			base.Title = title;
			base.Modal = true;
			base.Icon = IconLibrary.PixbufLoader.GetIcon ("main_icon.png");
			base.AddButton (GettextCatalog.GetString ("_Cancel"), Gtk.ResponseType.Cancel);
			base.AddButton (GettextCatalog.GetString ("_Accept"), Gtk.ResponseType.Accept);
			base.Response += new Gtk.ResponseHandler(OnResponse);
			_groupButton = null;
		}

		public string Selection
		{
			get
			{
				return _selection;
			}
		}
		
		protected Gtk.RadioButton AddButton (string label, EventHandler handler)
		{
			return  AddButton (base.VBox, label, handler);
		}
		
		protected Gtk.RadioButton AddButton (Gtk.VBox vbox, string label, EventHandler handler)
		{
			Gtk.RadioButton btn;
			if (_groupButton != null)
			{
				btn = new Gtk.RadioButton (_groupButton, label);
			}
			else
			{
				btn = new Gtk.RadioButton (label);
				_groupButton = btn;
				btn.Toggle ();
				btn.Active = true;
			}
			btn.Toggled += handler;
			btn.KeyPressEvent += OnKeyPressEvent;
			vbox.Add (btn);
			btn.Show();
			return btn;
		}
		
	 	private void OnResponse(object o, Gtk.ResponseArgs args)
		{
			base.Hide();
		}
		
		private void OnKeyPressEvent (object o, KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return)
			{
				try
				{
					Respond (Gtk.ResponseType.Ok);
				}
				catch (Exception ex) { }
			}
		}

		private Gtk.RadioButton _groupButton;		
		protected string _selection;
	}

}