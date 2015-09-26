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
	public abstract class MultipleObjectViewer : Gtk.VBox
	{
		protected MultipleObjectViewer(IntPtr raw) : base(raw) {}

		protected MultipleObjectViewer(IBroadcaster hub, string caption)
			: base(false, 0)
		{
			_hub = hub;
			// --- first line: caption and buttons
			Gtk.HBox hbxFirstLine = new Gtk.HBox(false, 2);
			// Caption label
			Gtk.Alignment labelAlign = new Gtk.Alignment(0F, 0F, 1F, 1F);
			labelAlign.Add(new Gtk.Label(caption));
			hbxFirstLine.PackStart(labelAlign, false, false, 0);
			hbxFirstLine.PackStart(new Gtk.Label(String.Empty), true, true, 0);
			// "Add" button
			Gtk.Image image = new Gtk.Image();
			image.Stock = Gtk.Stock.Add;
			_btnAdd = new Gtk.Button();
			_btnAdd.Add(image);
			_btnAdd.Relief = Gtk.ReliefStyle.None;
			_btnAdd.Clicked += new EventHandler(OnAddButtonClicked);
			_btnAdd.Sensitive = false;
			hbxFirstLine.PackStart(_btnAdd, false, false, 0);
			// "Edit" button
			image = new Gtk.Image();
			image.Stock = Gtk.Stock.JumpTo;
			_btnEdit = new Gtk.Button();
			_btnEdit.Add(image);
			_btnEdit.Relief = Gtk.ReliefStyle.None;
			_btnEdit.Clicked += new EventHandler(OnEditButtonClicked);
			_btnEdit.Sensitive = false;
			hbxFirstLine.PackStart(_btnEdit, false, false, 0);
			// "Delete" button
			image = new Gtk.Image();
			image.Stock = Gtk.Stock.Remove;
			_btnDelete = new Gtk.Button();
			_btnDelete.Add(image);
			_btnDelete.Relief = Gtk.ReliefStyle.None;
			_btnDelete.Clicked += new EventHandler(OnDeleteButtonClicked);
			_btnDelete.Sensitive = false;
			hbxFirstLine.PackStart(_btnDelete, false, false, 0);
			base.PackStart(hbxFirstLine, false, false, 0);
			// --- second line: element list
			_store = new Gtk.TreeStore(typeof(string));
			_tvList = new Gtk.TreeView();
			_tvList.HeadersVisible = false;
			_tvList.AppendColumn("", new Gtk.CellRendererText(), "text", 0);
			_tvList.Model = _store;
			_tvList.FocusInEvent += new Gtk.FocusInEventHandler(EnableButtons);
			_tvList.ButtonPressEvent += new Gtk.ButtonPressEventHandler(ListClickedHandler); 
			Gtk.ScrolledWindow sw = new Gtk.ScrolledWindow();
			sw.ShadowType = Gtk.ShadowType.In;
			sw.Add(_tvList);
			base.PackStart(sw, true, true, 0);
			sw.Show();
		}

		protected virtual bool IsAddSensitive
		{
			get { return true; }
		}
		
		protected abstract void Add();
		protected abstract void Delete(int index);
		protected abstract void Edit(int index);
		
		private void EnableButtons(object o, Gtk.FocusInEventArgs args)
		{
			Gtk.TreeIter iter;
			// GetIterFirst returns true if there is at least one element in
			// the model.
			if(_tvList.Model.GetIterFirst(out iter))
			{
				_btnDelete.Sensitive = true;
				_btnEdit.Sensitive = true;
			}
		}

		public new void Hide()
		{
			_store.Clear();
			base.Hide ();
			base.Visible = false;
		}
		
		private void ListClickedHandler(object o, Gtk.ButtonPressEventArgs args)
		{
			if(args.Event.Button == 1
				&& args.Event.Type == Gdk.EventType.TwoButtonPress)
			{
				int index = SelectedIndex(); 
				if(index >= 0) Edit(index);
			} 
		}

		private void OnAddButtonClicked(object sender, EventArgs args)
		{
			Add();
		}
		
		private void OnDeleteButtonClicked(object sender, EventArgs args)
		{
			Delete(SelectedIndex());
		}
		
		private void OnEditButtonClicked(object sender, EventArgs args)
		{
			Edit(SelectedIndex());
		}
		
		private int SelectedIndex()
		{
			Gtk.TreeIter iter;
			Gtk.TreeModel model;
			int index = -1;
			if(_tvList.Selection.GetSelected(out model, out iter))
			{
				Gtk.TreePath path = model.GetPath(iter);
				index = path.Indices[0];
			}
			return index;
		}
		
		protected void ShowList(string[] elementList)
		{
			_btnDelete.Sensitive = false;
			_btnEdit.Sensitive = false;
			_store.Clear();
			foreach(string entry in elementList)
			{
				_store.AppendValues(entry);
			}
			_btnAdd.Sensitive = IsAddSensitive;
		}
		
		Gtk.Button _btnAdd;
		Gtk.Button _btnDelete;
		Gtk.Button _btnEdit;
		protected IBroadcaster _hub;
		Gtk.TreeStore _store;
		Gtk.TreeView _tvList; 
	}
}
