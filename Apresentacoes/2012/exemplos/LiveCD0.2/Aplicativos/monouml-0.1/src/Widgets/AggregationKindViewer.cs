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
	public class AggregationKindViewer : Gtk.HBox
	{
		public AggregationKindViewer(IBroadcaster hub)
		{
			_hub = hub;
			base.PackStart(new Gtk.Label (GettextCatalog.GetString ("Aggregation kind:")), false, false, 2);
			// none
			_none = new Gtk.RadioButton (GettextCatalog.GetString ("none"));
			_none.Toggled += new EventHandler(ToggledHandler);
			base.PackStart(_none, false, false, 5);
			// shared
			_shared = new Gtk.RadioButton(_none, GettextCatalog.GetString ("shared"));
			_shared.Toggled += new EventHandler(ToggledHandler);
			base.PackStart(_shared, false, false, 5);
			// composite
			_composite = new Gtk.RadioButton(_none, GettextCatalog.GetString ("composite"));
			_composite.Toggled += new EventHandler(ToggledHandler);
			base.PackStart(_composite, false, false, 5);
		}
		
		private UML.AggregationKind Value
		{
			get
			{
				UML.AggregationKind val = UML.AggregationKind.none;
				if(_none.Active) { val = UML.AggregationKind.none; }
				else if(_shared.Active) { val = UML.AggregationKind.shared; }
				else if(_composite.Active) { val = UML.AggregationKind.composite; }
				return val;
			}
			set
			{
				switch(value)
				{
					case UML.AggregationKind.none:
						_none.Active = true; 
						break;
					case UML.AggregationKind.shared:
						_shared.Active = true; 
						break;
					case UML.AggregationKind.composite:
						_composite.Active = true; 
						break;
				}
			}
		}
		
		public new void Hide()
		{
			_property = null;
			base.Hide();
		}
		
		private void ToggledHandler(object sender, EventArgs args)
		{
			_property.Aggregation = this.Value;
			_hub.BroadcastElementChange(_property);
		}

		public void ShowAggregationFor(UML.Property element)
		{
			_property = element;
			this.Value = _property.Aggregation;
		}
		
		protected IBroadcaster _hub;
		private UML.Property _property;
		private Gtk.RadioButton _none;
		private Gtk.RadioButton _shared;
		private Gtk.RadioButton _composite;
	}
}
