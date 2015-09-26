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
using UML = ExpertCoder.Uml2;

namespace MonoUML.Widgets
{
	public class ModelElementTypeViewer : Gtk.HBox
	{
		public ModelElementTypeViewer(IBroadcaster hub)
		{
			_hub = hub;
			base.PackStart(new Gtk.Label("Model Element type:"), false, false, 2);
			_lblType = new Gtk.Label();
			base.PackStart(_lblType, false, false, 2);
			base.PackStart(new Gtk.Label(String.Empty), true, true, 0);
			// "Up" button
			Gtk.Image image = new Gtk.Image();
			image.Stock = Gtk.Stock.GoUp;
			_btnGoUp = new Gtk.Button();
			_btnGoUp.Add(image);
			_btnGoUp.Relief = Gtk.ReliefStyle.None;
			_btnGoUp.Clicked += new EventHandler(OnGoUpButtonClicked);
			_btnGoUp.Sensitive = false;
			base.PackStart(_btnGoUp, false, false, 0);
			// "Delete" button
			image = new Gtk.Image();
			image.Stock = Gtk.Stock.Delete;
			Gtk.Button btnDelete = new Gtk.Button();
			btnDelete.Add(image);
			btnDelete.Relief = Gtk.ReliefStyle.None;
			btnDelete.Clicked += new EventHandler(OnDeleteButtonClicked);
			btnDelete.Sensitive = true;
			base.PackStart(btnDelete, false, false, 0);
		}

		private void OnDeleteButtonClicked(object o, EventArgs args)
		{
			Hub.Instance.DeleteElement(_element);
		}

		private void OnGoUpButtonClicked(object o, EventArgs args)
		{
			_hub.BroadcastElementSelection(_element.Owner);
		}
				
		public void ShowElementTypeFor(UML.Element element)
		{
			_element = element;
			_lblType.Text = (element == null ? 
				String.Empty : element.GetType().Name.Substring(6));
			_btnGoUp.Sensitive = (element.Owner != null);
		}
		
		Gtk.Button _btnGoUp;
		UML.Element _element;
		IBroadcaster _hub;
		Gtk.Label _lblType;
	}
}
