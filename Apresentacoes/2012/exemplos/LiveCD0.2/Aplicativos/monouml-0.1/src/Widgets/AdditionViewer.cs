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

namespace MonoUML.Widgets
{
	public class AdditionViewer : SingleObjectViewer
	{
		public AdditionViewer(IBroadcaster hub)
			: base(hub, "Addition:") {}
		
		protected override object ElementValue
		{
			get { return _include.Addition; }
		}

		protected override void Clear()
		{
			_include.Addition = null;
			_hub.BroadcastElementChange(_include);
		}

		protected override void Edit()
		{
			ElementChooserDialog chooser = new ElementChooserDialog(typeof(UML.UseCase));
			chooser.SelectedObject = _include.Addition; 
			if(chooser.Run() == Gtk.ResponseType.Accept.value__)
			{
				_include.Addition = (UML.UseCase)chooser.SelectedObject;
				_hub.BroadcastElementChange(_include);
			} 
		}

		public void ShowAdditionFor(UML.Include include)
		{
			_include = include;
			base.SetValue(include.Addition == null
				? null : include.Addition.QualifiedName);
		}
		
		private UML.Include _include;
	}
}
