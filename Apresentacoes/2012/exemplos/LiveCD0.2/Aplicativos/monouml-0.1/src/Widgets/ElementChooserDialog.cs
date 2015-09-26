/*
MonoUML.Widgets - A library for representing the Widget elements
Copyright (C) 2004  Mario Carrión

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
using UML = ExpertCoder.Uml2;
using MonoUML.I18n;

namespace MonoUML.Widgets
{
	public class ElementChooserDialog : Gtk.Dialog, IBroadcaster
	{
		public ElementChooserDialog(Type requestedType)
			: this (requestedType, GettextCatalog.GetString ("Choose an element"))
		{ }

		public ElementChooserDialog(Type requestedType, string title)
		{
			base.Modal = true;
			base.HeightRequest = 400;
			base.WidthRequest = 250;
			//TODO: i18n
			base.Title = title;
			base.AddButton (GettextCatalog.GetString ("_Cancel"), Gtk.ResponseType.Cancel);
			base.AddButton (GettextCatalog.GetString ("_Accept"), Gtk.ResponseType.Accept);
			base.Response += new Gtk.ResponseHandler(OnResponse);
			base.SetResponseSensitive(Gtk.ResponseType.Accept, false);
			_requestedType = requestedType;
			Tree tree = new Tree();
			tree.IsReadOnly = true;
			tree.Draw(_elementList);
			tree.Show();
			_tree = tree;
			_tree.SetBroadcaster(this);
			Gtk.ScrolledWindow sw = new Gtk.ScrolledWindow();
			sw.Add(tree);
			base.VBox.Add(sw);
			sw.Show();
		}
		
		public object SelectedObject
		{
			get { return _selectedObject; }
			set
			{
				_tree.SelectElement(value);
				((IBroadcaster)this).BroadcastElementSelection(value);
			}
		}

		void IBroadcaster.BroadcastElementChange(object modifiedElement)
		{
		}

		void IBroadcaster.BroadcastElementSelection(object element)
		{
			bool isSensitive = (_requestedType == null) 
				|| _requestedType.IsInstanceOfType(element);
			base.SetResponseSensitive(Gtk.ResponseType.Accept, isSensitive);
			if(isSensitive)
			{
				_selectedObject = element;
			}
		}
		
		private void OnResponse(object o, Gtk.ResponseArgs args)
		{
			base.Hide();
		}
		
		public static void SetProjectElements(IList elements)
		{
			_elementList = elements;
		}
		
		private static IList _elementList;
		private Type _requestedType;
		private object _selectedObject;
		private IView _tree;
	}
}
