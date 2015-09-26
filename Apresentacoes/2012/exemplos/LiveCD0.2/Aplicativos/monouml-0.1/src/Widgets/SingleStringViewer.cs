/*
MonoUML.Widgets - A library for representing the Widget elements
Copyright (C) 2004  Mario Carrión
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
using System.Reflection;
using UML = ExpertCoder.Uml2;

namespace MonoUML.Widgets
{
	public class SingleStringViewer : Gtk.HBox
	{
		public SingleStringViewer(IBroadcaster hub, string caption,
			string propertyName)
		{
			_hub = hub;
			_propertyName = propertyName;
			base.PackStart(new Gtk.Label(caption), false, false, 2);
			_entry = new Gtk.Entry();
			_entry.Activated += new EventHandler(CheckIfChanged);
			_entry.FocusOutEvent += new Gtk.FocusOutEventHandler(CheckIfChanged);
			base.PackStart(_entry);
		}

		private void CheckIfChanged()
		{
			string newValue = _entry.Text;
			if(newValue != _lastValue)
			{
				_lastValue = newValue;
				_owner.GetType().InvokeMember(
					_propertyName,
					BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance,
					null,
					_owner,
					new object[] { newValue });
				_hub.BroadcastElementChange(_owner);
			}
		}
		
		private void CheckIfChanged(object o, EventArgs args)
		{
			CheckIfChanged();
		}
		
		private void CheckIfChanged(object o, Gtk.FocusOutEventArgs args)
		{
			CheckIfChanged();
		}

		public void ShowPropertyValueFor(UML.Element element)
		{
			if(_owner!=null && !object.ReferenceEquals(_owner, element))
			{
				CheckIfChanged();
			}
			_owner = element;
			_lastValue = (string)_owner.GetType().InvokeMember(
				_propertyName,
				BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance,
				null,
				_owner,
				null);
			_entry.Text = _lastValue==null ? String.Empty : _lastValue;
		}

		private Gtk.Entry _entry;
		private IBroadcaster _hub;
		private object _owner;
		private string _propertyName;
		private string _lastValue;
	}
}
