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
using System.Reflection;

namespace MonoUML.Widgets
{
	public class SingleBooleanViewer : Gtk.HBox
	{
		public SingleBooleanViewer(IBroadcaster hub, string caption,
			string propertyName)
		{
			_hub = hub;
			_propertyName = propertyName;
			base.PackStart(new Gtk.Label(caption), false, false, 2);
			_checkButton = new Gtk.CheckButton();
			_checkButton.Toggled += new EventHandler(ToggledHandler);
			base.PackStart(_checkButton);
		}

		public new void Hide()
		{
			_owner = null;
			base.Visible = false;
			base.Hide();
		}

		public void ShowPropertyValueFor(UML.Element element)
		{
			_owner = element;
			_checkButton.Active = (bool)_owner.GetType().InvokeMember(
				_propertyName,
				BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance,
				null,
				_owner,
				null);
		}
		
		private void ToggledHandler(object sender, EventArgs args)
		{
			_owner.GetType().InvokeMember(
				_propertyName,
				BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance,
				null,
				_owner,
				new object[] { _checkButton.Active });
			_hub.BroadcastElementChange(_owner);
		}

		private Gtk.CheckButton _checkButton;
		protected IBroadcaster _hub;
		private string _propertyName;
		private object _owner;
	}
}
