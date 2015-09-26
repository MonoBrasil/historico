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
using System.Reflection;
using UML = ExpertCoder.Uml2;
using MonoUML.I18n;

namespace MonoUML.Widgets
{
	// This widgets allows to add and remove elements not owned by the collection,
	// but instead references to those elements.
	// This means that the elements are chosen from a list instead of created
	// when the user clicks "+".
	public class MixedElementsViewer : MultipleObjectViewer
	{
		public MixedElementsViewer(IBroadcaster hub, string label, 
			string mixedPropertyName, string ownerPropertyName,
			string mixedElementsTypeName)
			: base(hub, label)
		{
			_mixedPropertyName = mixedPropertyName;
			_ownerPropertyName = ownerPropertyName;
			_mixedElementsTypeName = mixedElementsTypeName;
		}

		protected override void Add()
		{
			string typename = "ExpertCoder.Uml2." + _mixedElementsTypeName;
			try
			{
				Type type = typeof(UML.Element).Assembly.GetType(typename, true);
				ElementChooserDialog chooser = new ElementChooserDialog(type);
				if(chooser.Run() == Gtk.ResponseType.Accept.value__)
				{
					object newElement = chooser.SelectedObject;
					_mixedElements.Add(newElement);
					_hub.BroadcastElementChange(_owner);
					if(_ownerPropertyName != null)
					{
						newElement.GetType().InvokeMember(
							_ownerPropertyName,
							BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance,
							null,
							newElement,
							new object[] { _owner });
					}
				}
				_hub.BroadcastElementChange(_owner);
			}
			catch (Exception e)
			{
				Gtk.MessageDialog md = new Gtk.MessageDialog (null, 
					Gtk.DialogFlags.DestroyWithParent,
					Gtk.MessageType.Error, 
					Gtk.ButtonsType.Close, e.Message);
				md.Run ();
				md.Destroy();
			}
		}

		protected override void Delete(int index)
		{
			_mixedElements.RemoveAt(index);
			_hub.BroadcastElementChange(_owner);
		}

		protected override void Edit(int index)
		{
			_hub.BroadcastElementSelection(_mixedElements[index]);
		}

		// When the widgets is hidden, it must release all the references to
		// the model objects.
		public new void Hide()
		{
			_owner = null;
			_mixedElements = null;
			base.Hide();
		}

		public void ShowMixedElementsFor(UML.Element element)
		{
			_owner = element;
			_mixedElements = (IList)_owner.GetType().InvokeMember(
				_mixedPropertyName,
				BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance,
				null,
				_owner,
				null);
			string[] propertyList = new string[_mixedElements.Count];
			UML.NamedElement ne;
			object current;
			Type currentType;
			string label;
			for(int i = 0; i < _mixedElements.Count; i ++)
			{
				current = _mixedElements[i];
				ne = current as UML.NamedElement;
				label = (ne!=null ? ne.Name : null);
				if(label == null || label == "")
				{
					label = current.ToString();
					currentType = current.GetType();
					if(label == null || label == "" || label == currentType.FullName)
					{
						// removes the "__Impl" part of the class name
						//FIXME. i18n
						label = "<<"+GettextCatalog.GetString ("anonymous") +" "+ currentType.Name.Substring(6) + ">>";
					}
				}
				propertyList[i] = label;
			}
			base.ShowList(propertyList);
		}
		
		private object _owner;
		private IList _mixedElements;
		private string _mixedElementsTypeName;
		private string _mixedPropertyName;
		private string _ownerPropertyName;
	}
}
