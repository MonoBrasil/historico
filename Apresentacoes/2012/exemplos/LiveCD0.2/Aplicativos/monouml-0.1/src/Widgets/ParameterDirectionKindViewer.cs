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
	public class ParameterDirectionKindViewer : Gtk.HBox
	{
		public ParameterDirectionKindViewer(IBroadcaster hub)
		{
			_hub = hub;
			base.PackStart(new Gtk.Label(GettextCatalog.GetString ("Direction:")), false, false, 2);
			// public
			_in = new Gtk.RadioButton("in");
			_in.Toggled += new EventHandler(ToggledHandler);
			base.PackStart(_in, false, false, 5);
			// private
			_inout = new Gtk.RadioButton(_in, "inout");
			_inout.Toggled += new EventHandler(ToggledHandler);
			base.PackStart(_inout, false, false, 5);
			// protected
			_out = new Gtk.RadioButton(_in, "out");
			_out.Toggled += new EventHandler(ToggledHandler);
			base.PackStart(_out, false, false, 5);
			// package
			_return = new Gtk.RadioButton(_in, GettextCatalog.GetString ("return"));
			_return.Toggled += new EventHandler(ToggledHandler);
			base.PackStart(_return, false, false, 5);
		}
		
		private UML.ParameterDirectionKind Value
		{
			get
			{
				UML.ParameterDirectionKind val = UML.ParameterDirectionKind.@in;
				if(_in.Active) { val = UML.ParameterDirectionKind.@in; }
				else if(_inout.Active) { val = UML.ParameterDirectionKind.inout; }
				else if(_out.Active) { val = UML.ParameterDirectionKind.@out; }
				else if(_return.Active) { val = UML.ParameterDirectionKind.@return; }
				return val;
			}
			set
			{
				switch(value)
				{
					case UML.ParameterDirectionKind.@in:
						_in.Active = true; 
						break;
					case UML.ParameterDirectionKind.inout:
						_inout.Active = true; 
						break;
					case UML.ParameterDirectionKind.@out:
						_out.Active = true; 
						break;
					case UML.ParameterDirectionKind.@return:
						_return.Active = true; 
						break;
				}
			}
		}
		
		private void ToggledHandler(object sender, EventArgs args)
		{
			_parameter.Direction = this.Value;
			_hub.BroadcastElementChange(_parameter);
		}

		public void ShowDirectionFor(UML.Parameter element)
		{
			_parameter = element;
			this.Value = _parameter.Direction;
		}
		
		protected IBroadcaster _hub;
		private UML.Parameter _parameter;
		private Gtk.RadioButton _in;
		private Gtk.RadioButton _inout;
		private Gtk.RadioButton _out;
		private Gtk.RadioButton _return;
	}
}
