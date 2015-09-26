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
using UML = ExpertCoder.Uml2;
using MonoUML.I18n;

namespace MonoUML.Widgets
{
	public class TypedElementTypeViewer : SingleObjectViewer
	{
		public TypedElementTypeViewer(IBroadcaster hub)
			: base(hub, GettextCatalog.GetString ("Type:")) {}
		
		protected override object ElementValue
		{
			get { return _typedElement.Type; }
		}

		protected override void Clear()
		{
			_typedElement.Type = null;
			_hub.BroadcastElementChange(_typedElement);
		}

		protected override void Edit()
		{
			ElementChooserDialog chooser = new ElementChooserDialog(typeof(UML.Type));
			chooser.SelectedObject = _typedElement.Type; 
			if(chooser.Run() == Gtk.ResponseType.Accept.value__)
			{
				_typedElement.Type = (UML.Type)chooser.SelectedObject;
				_hub.BroadcastElementChange(_typedElement);
			} 
		}

		public void ShowTypeFor(UML.TypedElement typedElement)
		{
			_typedElement = typedElement;
			base.SetValue(typedElement.Type == null
				? null : typedElement.Type.QualifiedName);
		}
		
		private UML.TypedElement _typedElement;
	}
}
