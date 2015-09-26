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
	public class OwnedOperationsViewer : MultipleObjectViewer
	{
		public OwnedOperationsViewer(IBroadcaster hub)
			: base(hub, GettextCatalog.GetString ("Owned operations:"))
		{
		}

		protected override void Add()
		{
			UML.Operation newOperation = UML.Create.Operation();
			newOperation.Name = GettextCatalog.GetString ("<<New Operation>>");
			UML.Class ownerClass;
			UML.DataType ownerDatatype;
			if((ownerClass = _owner as UML.Class) != null)
			{
				newOperation.Class = ownerClass;
			}
			else if((ownerDatatype = _owner as UML.DataType) != null)
			{
				newOperation.Datatype = ownerDatatype;
			}
			_ownedOperations.Add(newOperation);
			_hub.BroadcastElementChange(_owner);
		}

		protected override void Delete(int index)
		{
			UML.Operation operation = (UML.Operation)_ownedOperations[index];
			_ownedOperations.RemoveAt(index);
			operation.Class = null;
			operation.Datatype = null;
			_hub.BroadcastElementChange(_owner);
		}

		protected override void Edit(int index)
		{
			_hub.BroadcastElementSelection(_ownedOperations[index]);
		}
		
		public void ShowOwnedOperationsFor(UML.Classifier element)
		{
			_owner = element;
			// sets a global reference to the collection
			UML.Class ownerClass;
			UML.DataType ownerDatatype;
			UML.Interface ownerInterface;
			if((ownerClass = _owner as UML.Class) != null)
			{
				_ownedOperations = ownerClass.OwnedOperation;
			}
			else if((ownerDatatype = _owner as UML.DataType) != null)
			{
				_ownedOperations = ownerDatatype.OwnedOperation;
			}
			else if((ownerInterface = _owner as UML.Interface) != null)
			{
				_ownedOperations = ownerInterface.OwnedOperation;
			}
			// shows the collection items
			string[] operationList = new string[_ownedOperations.Count];
			UML.Operation prop;
			for(int i = 0; i < _ownedOperations.Count; i ++)
			{
				prop = (UML.Operation)_ownedOperations[i];
				operationList[i] = prop.Name;
			}
			base.ShowList(operationList);
		}
		
		private IList _ownedOperations;
		private UML.Classifier _owner;
	}
}
