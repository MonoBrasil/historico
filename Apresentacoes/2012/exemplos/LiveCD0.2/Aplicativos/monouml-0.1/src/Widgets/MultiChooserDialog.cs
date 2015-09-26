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
using System;
using System.Collections;
using Gtk;
using MonoUML.I18n;

namespace MonoUML.Widgets
{
	public class MultiChooserDialog : Gtk.Dialog, IDisposable
	{
		public MultiChooserDialog(IList options, IList banned)
		{
			base.Modal = true;
			base.HeightRequest = 400;
			base.WidthRequest = 250;
			//TODO: i18n
			base.Title = GettextCatalog.GetString ("Choose elements");
			base.AddButton(GettextCatalog.GetString ("_Cancel"), Gtk.ResponseType.Cancel);
			base.AddButton(GettextCatalog.GetString ("_Accept"), Gtk.ResponseType.Accept);
			base.Response += new Gtk.ResponseHandler(OnResponse);

			TreeView treeView = new TreeView();
			treeView.HeadersVisible = false;
			_store = new ListStore(typeof(bool), typeof(string));
			treeView.Model = _store; 
			CellRendererToggle crtgl = new CellRendererToggle();
			crtgl.Activatable = true;
			crtgl.Toggled += new ToggledHandler(CheckboxToggledHandler);
			TreeViewColumn column = new TreeViewColumn ();
			column.PackStart(crtgl, false);
			column.AddAttribute(crtgl, "active", 0);
			treeView.AppendColumn(column);
			CellRendererText crtxt = new CellRendererText ();
			column = new TreeViewColumn ();
			column.PackStart(crtxt, false);
			column.AddAttribute(crtxt, "text", 1);
			treeView.AppendColumn(column);
			Gtk.ScrolledWindow sw = new Gtk.ScrolledWindow();
			sw.ShadowType = Gtk.ShadowType.In;
			sw.Add(treeView);
			treeView.Show();
			base.VBox.Add(sw);
			ShowList(options, banned);
			sw.Show();
		}
		
		private void CheckboxToggledHandler(object o, ToggledArgs args)
		{
			TreeIter iter;
			_store.GetIterFromString(out iter, args.Path);
			bool val = (bool)_store.GetValue(iter, 0);
			_store.SetValue(iter, 0, !val);
		}
		
		private void OnResponse(object o, Gtk.ResponseArgs args)
		{
			base.Hide();
		}
		
		public IList SelectedObjects
		{
			get
			{
				ArrayList selected = new ArrayList();
				TreeIter iter;
				int i = 0;
				foreach(object o in _options)
				{
					if(!_banned.Contains(o))
					{
						_store.GetIterFromString(out iter, (i++).ToString());
						if((bool)_store.GetValue(iter, 0))
						{
							selected.Add(o);
						}
					}
				}
				return selected;
			}
		}
		
		private void ShowList(IList options, IList banned)
		{
			_banned = banned;
			_options = options;
			foreach(object o in options)
			{
				if(!banned.Contains(o))
				{
					_store.AppendValues(false, o.ToString());
				}
			}
		}
		
		void IDisposable.Dispose()
		{
			_banned = null;
			_options = null;
			_store = null;
		}
		
		private IList _banned;
		private IList _options;
		private ListStore _store;
	}
}
