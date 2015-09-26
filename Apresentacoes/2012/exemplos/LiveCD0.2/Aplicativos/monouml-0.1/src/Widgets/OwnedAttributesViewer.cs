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
	public class OwnedAttributesViewer : MultipleObjectViewer
	{
		public OwnedAttributesViewer(IBroadcaster hub)
			: this(hub, GettextCatalog.GetString ("Owned attributes:"))
		{
		}

		public OwnedAttributesViewer(IBroadcaster hub, string label)
			: base(hub, label)
		{
		}

		protected override void Add()
		{
			UML.Property newProperty = UML.Create.Property();
			newProperty.Name = GettextCatalog.GetString ("<<New Property>>");
			UML.Association ownerAssociation;
			UML.Class ownerClass;
			UML.DataType ownerDatatype;
			if((ownerAssociation = _owner as UML.Association) != null)
			{
				newProperty.OwningAssociation = ownerAssociation;
				newProperty.Name = GettextCatalog.GetString ("<<New End>>");
			}
			else if((ownerClass = _owner as UML.Class) != null)
			{
				newProperty.Class = ownerClass;
			}
			else if((ownerDatatype = _owner as UML.DataType) != null)
			{
				newProperty.Datatype = ownerDatatype;
			}
			_ownedAttributes.Add(newProperty);
			_hub.BroadcastElementChange(_owner);
		}

		protected override void Delete(int index)
		{
			UML.Property property = (UML.Property)_ownedAttributes[index];
			_ownedAttributes.RemoveAt(index);
			property.Class = null;
			property.Datatype = null;
			_hub.BroadcastElementChange(_owner);
		}

		protected override void Edit(int index)
		{
			_hub.BroadcastElementSelection(_ownedAttributes[index]);
		}
		
		// When the widgets is hidden, it must release all the references to
		// the model objects.
		public new void Hide()
		{
			_ownedAttributes = null;
			_owner = null;
			base.Hide();
		}
		
		public void ShowOwnedAttributesFor(UML.Classifier element)
		{
			_owner = element;
			// sets a global reference to the collection
			UML.Association ownerAssociation;
			UML.Class ownerClass;
			UML.DataType ownerDatatype;
			UML.Interface ownerInterface;
			if((ownerAssociation = _owner as UML.Association) != null)
			{
				_ownedAttributes = ownerAssociation.OwnedEnd;
			}
			else if((ownerClass = _owner as UML.Class) != null)
			{
				_ownedAttributes = ownerClass.OwnedAttribute;
			}
			else if((ownerDatatype = _owner as UML.DataType) != null)
			{
				_ownedAttributes = ownerDatatype.OwnedAttribute;
			}
			else if((ownerInterface = _owner as UML.Interface) != null)
			{
				_ownedAttributes = ownerInterface.OwnedAttribute;
			}
			// shows the collection items
			string[] propertyList = new string[_ownedAttributes.Count];
			UML.Property prop;
			string propName;
			for(int i = 0; i < _ownedAttributes.Count; i ++)
			{
				prop = (UML.Property)_ownedAttributes[i];
				propName = prop.Name;
				if (propName == null || propName == "")
				{
					propName = "<<"+GettextCatalog.GetString ("anonymous")+ prop.GetType().Name.Substring(6) + ">>";
				}
				propertyList[i] = propName;
			}
			base.ShowList(propertyList);
		}
		
		private IList _ownedAttributes;
		private UML.Classifier _owner;
	}
}
