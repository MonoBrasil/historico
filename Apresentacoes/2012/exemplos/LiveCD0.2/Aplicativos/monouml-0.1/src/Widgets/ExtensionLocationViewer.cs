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
	public class ExtensionLocationViewer : MultipleObjectViewer
	{
		public ExtensionLocationViewer(IBroadcaster hub)
			: base(hub, GettextCatalog.GetString ("Extension locations:")) {}

		protected override bool IsAddSensitive
		{
			get { return _extend.ExtendedCase != null; }
		}

		protected override void Add()
		{
			using(MultiChooserDialog multi = new MultiChooserDialog(
				_extend.ExtendedCase.ExtensionPoint, _extend.ExtensionLocation))
			{
				if(multi.Run() == Gtk.ResponseType.Accept.value__)
				{
					_extend.ExtensionLocation.AddRange(multi.SelectedObjects);
					_hub.BroadcastElementChange(_extend);
				}
			}
		}

		protected override void Delete(int index)
		{
			_extend.ExtensionLocation.RemoveAt(index);
			_hub.BroadcastElementChange(_extend);
		}

		protected override void Edit(int index)
		{
			_hub.BroadcastElementSelection(_extend.ExtensionLocation[index]);
		}

		public new void Hide()
		{
			_extend = null;
			base.Hide();
		}

		public void ShowExtensionLocationFor(UML.Extend element)
		{
			_extend = element;
			string[] extensionLocationList = new string[element.ExtensionLocation.Count];
			for(int i = 0; i < element.ExtensionLocation.Count; i ++)
			{
				extensionLocationList[i] = ((UML.ExtensionPoint)element.ExtensionLocation[i]).QualifiedName;
			}
			base.ShowList(extensionLocationList);
		}
		
		private UML.Extend _extend;
	}
}
