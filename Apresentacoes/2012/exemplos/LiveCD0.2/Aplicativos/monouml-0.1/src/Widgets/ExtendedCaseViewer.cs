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
	public class ExtendedCaseViewer : SingleObjectViewer
	{
		public ExtendedCaseViewer(IBroadcaster hub)
			: base(hub, GettextCatalog.GetString ("Extended case:")) {}
		
		protected override object ElementValue
		{
			get { return _extend.ExtendedCase; }
		}

		protected override void Clear()
		{
			_extend.ExtendedCase = null;
			_extend.ExtensionLocation.Clear();
			_hub.BroadcastElementChange(_extend);
		}

		protected override void Edit()
		{
			ElementChooserDialog chooser = new ElementChooserDialog(typeof(UML.UseCase));
			chooser.SelectedObject = _extend.ExtendedCase; 
			if(chooser.Run() == Gtk.ResponseType.Accept.value__)
			{
				_extend.ExtendedCase = (UML.UseCase)chooser.SelectedObject;
				_extend.ExtensionLocation.Clear();
				_hub.BroadcastElementChange(_extend);
			} 
		}

		public void ShowExtendedCaseFor(UML.Extend extend)
		{
			_extend = extend;
			base.SetValue(extend.ExtendedCase == null
				? null : extend.ExtendedCase.QualifiedName);
		}
		
		private UML.Extend _extend;
	}
}
