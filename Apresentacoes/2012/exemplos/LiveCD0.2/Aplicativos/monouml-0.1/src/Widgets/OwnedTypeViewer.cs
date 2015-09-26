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
using UML = ExpertCoder.Uml2;
using MonoUML.I18n;

namespace MonoUML.Widgets
{
	public class OwnedTypeViewer : MultipleObjectViewer
	{
		public OwnedTypeViewer(IBroadcaster hub)
			: base(hub, GettextCatalog.GetString ("Owned types:")) {}

		protected override void Add()
		{
			TypeKindChooserDialog dialog = new TypeKindChooserDialog();
//			UML.PackageableElement pe;
			UML.Type t;
			if(dialog.Run()==Gtk.ResponseType.Cancel.value__) return;
			string elementType = dialog.Selection;
			if(elementType!=null)
			{
				object newElem = Helper.CreateUmlElement(elementType);
				_package.OwnedType.Add(newElem);
				if((t = newElem as UML.Type) != null)
				{
					t.Package = _package;
				}
//				if((pe = newElem as UML.PackageableElement) != null)
//				{
//TODO: EC doesn't not expose this property, according to the standard.
//We must think about what to do in this case.
//					pe.OwningPackage = _package;
//				}
				_hub.BroadcastElementChange(_package);
			}
		}

		protected override void Delete(int index)
		{
			_package.OwnedType.RemoveAt(index);
			_hub.BroadcastElementChange(_package);
		}

		protected override void Edit(int index)
		{
			_hub.BroadcastElementSelection(_package.OwnedType[index]);
		}
		
		public new void Hide()
		{
			_package = null;
			base.Hide();
		}

		public void ShowOwnedTypeFor(UML.Package package)
		{
			_package = package;
			string name;
			string[] typeList = new string[package.OwnedType.Count];
			for(int i = 0; i < package.OwnedType.Count; i ++)
			{
				name = ((UML.Type)package.OwnedType[i]).Name;
				if (name==null || name==String.Empty)
				{
					name = "<<" + package.OwnedType[i].GetType().Name.Substring(6) + ">>";
				}
				typeList[i] = name;
			}
			base.ShowList(typeList);
		}
		
		private UML.Package _package;
	}
}
