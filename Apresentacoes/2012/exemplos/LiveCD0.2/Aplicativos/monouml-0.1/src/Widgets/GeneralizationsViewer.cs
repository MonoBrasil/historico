/*
MonoUML.Widgets - A library for representing the Widget elements
Copyright (C) 2005  Rodolfo Campero

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
	public class GeneralizationsViewer : MultipleObjectViewer
	{
		public GeneralizationsViewer(IBroadcaster hub)
			: base(hub, GettextCatalog.GetString ("Generalizations:"))
		{
		}

		protected override void Add()
		{
			Console.WriteLine("GeneralizationsViewer.Add()");
			UML.Generalization newElement = (UML.Generalization)Helper.CreateUmlElement("Generalization");
			_owner.Generalization.Add(newElement);
			newElement.Specific = _owner;
			_hub.BroadcastElementChange(_owner);
		}

		protected override void Delete(int index)
		{
			_owner.Generalization.RemoveAt(index);
			_hub.BroadcastElementChange(_owner);
		}

		protected override void Edit(int index)
		{
			_hub.BroadcastElementSelection(_owner.Generalization[index]);
		}

		// When the widgets is hidden, it must release all the references to
		// the model objects.
		public new void Hide()
		{
			_owner = null;
			base.Hide();
		}

		public void ShowGeneralizationsFor(UML.Classifier element)
		{
			_owner = element;
			string[] propertyList = new string[element.Generalization.Count];
			UML.NamedElement ne;
			UML.Generalization current;
			string label;
			for(int i = 0; i < element.Generalization.Count; i ++)
			{
				current = (UML.Generalization)element.Generalization[i];
				ne = current.General;
				label = (ne!=null ? ne.Name : null);
				if(label == null || label == "")
				{
					label = GettextCatalog.GetString ("<<anonymous Generalization>>");
				}
				propertyList[i] = label;
			}
			base.ShowList(propertyList);
		}
		
		private UML.Classifier _owner;
	}
}
