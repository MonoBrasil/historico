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

namespace MonoUML.Widgets
{
	public class OwnedElementsViewer : MultipleObjectViewer
	{
		public OwnedElementsViewer(IBroadcaster hub, string label, 
			string ownedPropertyName, string ownerPropertyName,
			string ownedElementsTypeName)
			: base(hub, label)
		{
			_ownedPropertyName = ownedPropertyName;
			_ownerPropertyName = ownerPropertyName;
			_ownedElementsTypeName = ownedElementsTypeName;
		}

		protected override void Add()
		{
			Console.WriteLine("OwnedElementsViewer.Add()");
			object newElement = Helper.CreateUmlElement(_ownedElementsTypeName);
			_ownedElements.Add(newElement);
			if(_ownerPropertyName != null)
			{
				Console.WriteLine("_ownerPropertyName = " + _ownerPropertyName);
				newElement.GetType().InvokeMember(
					_ownerPropertyName,
					BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance,
					null,
					newElement,
					new object[] { _owner });
			}
			_hub.BroadcastElementChange(_owner);
		}

		protected override void Delete(int index)
		{
			_ownedElements.RemoveAt(index);
			_hub.BroadcastElementChange(_owner);
		}

		protected override void Edit(int index)
		{
			_hub.BroadcastElementSelection(_ownedElements[index]);
		}

		// When the widgets is hidden, it must release all the references to
		// the model objects.
		public new void Hide()
		{
			_owner = null;
			_ownedElements = null;
			base.Hide();
		}

		public void ShowOwnedElementsFor(UML.Element element)
		{
			_owner = element;
			_ownedElements = (IList)_owner.GetType().InvokeMember(
				_ownedPropertyName,
				BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance,
				null,
				_owner,
				null);
			string[] propertyList = new string[_ownedElements.Count];
			UML.NamedElement ne;
			object current;
			Type currentType;
			string label;
			for(int i = 0; i < _ownedElements.Count; i ++)
			{
				current = _ownedElements[i];
				ne = current as UML.NamedElement;
				label = (ne!=null ? ne.Name : null);
				if(label == null || label == "")
				{
					label = current.ToString();
					currentType = current.GetType();
					if(label == "" || label == currentType.FullName)
					{
						// removes the "__Impl" part of the class name
						label = currentType.Name.Substring(6);
					}
				}
				propertyList[i] = label;
			}
			base.ShowList(propertyList);
		}
		
		private object _owner;
		private IList _ownedElements;
		private string _ownedElementsTypeName;
		private string _ownedPropertyName;
		private string _ownerPropertyName;
	}
}
