/*
MonoUML.Widgets - A library for representing the Widget elements
Copyright (C) 2004  Rodolfo Campero

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
using UML = ExpertCoder.Uml2;
using MonoUML.I18n;

namespace MonoUML.Widgets
{
	public class NamedElementVisibilityViewer : Gtk.HBox
	{
		public NamedElementVisibilityViewer(IBroadcaster hub)
		{
			_hub = hub;
			base.PackStart(new Gtk.Label (GettextCatalog.GetString ("Visibility:")), false, false, 2);
			// public
			_public = new Gtk.RadioButton (GettextCatalog.GetString ("public"));
			_public.Toggled += new EventHandler(ToggledHandler);
			base.PackStart(_public, false, false, 5);
			// private
			_private = new Gtk.RadioButton(_public, GettextCatalog.GetString ("private"));
			_private.Toggled += new EventHandler(ToggledHandler);
			base.PackStart(_private, false, false, 5);
			// protected
			_protected = new Gtk.RadioButton(_public, GettextCatalog.GetString ("protected"));
			_protected.Toggled += new EventHandler(ToggledHandler);
			base.PackStart(_protected, false, false, 5);
			// package
			_package = new Gtk.RadioButton(_public, GettextCatalog.GetString  ("package"));
			_package.Toggled += new EventHandler(ToggledHandler);
			base.PackStart(_package, false, false, 5);
		}
		
		private UML.VisibilityKind Value
		{
			get
			{
				UML.VisibilityKind val = UML.VisibilityKind.@public;
				if(_public.Active) { val = UML.VisibilityKind.@public; }
				else if(_private.Active) { val = UML.VisibilityKind.@private; }
				else if(_protected.Active) { val = UML.VisibilityKind.@protected; }
				else if(_package.Active) { val = UML.VisibilityKind.package; }
				return val;
			}
			set
			{
				switch(value)
				{
					case UML.VisibilityKind.@public:
						_public.Active = true; 
						break;
					case UML.VisibilityKind.@private:
						_private.Active = true; 
						break;
					case UML.VisibilityKind.@protected:
						_protected.Active = true; 
						break;
					case UML.VisibilityKind.package:
						_package.Active = true; 
						break;
				}
			}
		}

		public new void Hide()
		{
			_namedElement = null;
			base.Hide ();
		}
		
		private void ToggledHandler(object sender, EventArgs args)
		{
			_namedElement.Visibility = this.Value;
			_hub.BroadcastElementChange(_namedElement);
		}

		public void ShowVisibilityFor(UML.NamedElement element)
		{
			_namedElement = element;
			this.Value = _namedElement.Visibility;
		}
		
		protected IBroadcaster _hub;
		private UML.NamedElement _namedElement;
		private Gtk.RadioButton _public;
		private Gtk.RadioButton _private;
		private Gtk.RadioButton _protected;
		private Gtk.RadioButton _package;
	}
}
