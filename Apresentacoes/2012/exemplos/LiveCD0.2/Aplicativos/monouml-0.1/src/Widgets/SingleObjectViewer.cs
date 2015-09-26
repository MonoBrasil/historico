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
using System;

namespace MonoUML.Widgets
{
	public abstract class SingleObjectViewer : Gtk.HBox
	{
		protected SingleObjectViewer(IntPtr raw) : base(raw) {}

		protected SingleObjectViewer(IBroadcaster hub, string caption)
			: base(false, 0)
		{
			Gtk.Image image;
			_hub = hub;
			// Caption label
			base.PackStart(new Gtk.Label(caption), false, false, 0);
			// Value entry
			_entry = new Gtk.Entry();
			_entry.IsEditable = false;
			base.PackStart(_entry, true, true, 0);
			// "Edit" button
			image = new Gtk.Image();
			image.Stock = Gtk.Stock.Find;
			_btnEdit = new Gtk.Button();
			_btnEdit.Add(image);
			_btnEdit.Relief = Gtk.ReliefStyle.None;
			_btnEdit.Clicked += new EventHandler(OnEditButtonClicked);
			_btnEdit.Sensitive = false;
			base.PackStart(_btnEdit, false, false, 0);
			// "Select" button
			image = new Gtk.Image();
			image.Stock = Gtk.Stock.JumpTo;
			_btnSelect = new Gtk.Button();
			_btnSelect.Add(image);
			_btnSelect.Relief = Gtk.ReliefStyle.None;
			_btnSelect.Clicked += new EventHandler(OnSelectButtonClicked);
			_btnSelect.Sensitive = false;
			base.PackStart(_btnSelect, false, false, 0);
			// "Clear" button
			image = new Gtk.Image();
			image.Stock = Gtk.Stock.Clear;
			_btnClear = new Gtk.Button();
			_btnClear.Add(image);
			_btnClear.Relief = Gtk.ReliefStyle.None;
			_btnClear.Clicked += new EventHandler(OnClearButtonClicked);
			_btnClear.Sensitive = false;
			base.PackStart(_btnClear, false, false, 0);
		}
		
		protected abstract object ElementValue { get; }
		protected abstract void Clear();
		protected abstract void Edit();
		
		private void OnClearButtonClicked(object sender, EventArgs args)
		{
			Clear();
		}
		
		private void OnEditButtonClicked(object sender, EventArgs args)
		{
			Edit();
		}
		
		private void OnSelectButtonClicked(object sender, EventArgs args)
		{
			_hub.BroadcastElementSelection(ElementValue);
		}
		
		protected void SetValue(string textValue)
		{
			bool hasValue = (textValue != null);
			_btnClear.Sensitive = hasValue;
			_btnEdit.Sensitive = true;
			_btnSelect.Sensitive = hasValue;
			_entry.Text = (hasValue ? textValue : String.Empty);
		}
		
		Gtk.Button _btnClear;
		Gtk.Button _btnEdit;
		Gtk.Button _btnSelect;
		protected IBroadcaster _hub;
		Gtk.Entry _entry;
	}
}
