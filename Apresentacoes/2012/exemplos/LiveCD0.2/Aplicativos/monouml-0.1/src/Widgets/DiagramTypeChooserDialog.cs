/*
MonoUML.Widgets - A library for representing the Widget elements
Copyright (C) 2005  Rodolfo Campero
Copyright (C) 2005  Mario Carrión

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
using Gtk;
using MonoUML.I18n;
using Uml2 = MonoUML.Widgets.UML2;

namespace MonoUML.Widgets
{
	public class DiagramTypeChooserDialog : ChooserDialog
	{
		public DiagramTypeChooserDialog() : base (GettextCatalog.GetString ("Choose a diagram type"))
		{
			AddButton (GettextCatalog.GetString  ("_Class"), SetClass);
			_selection = "Class";
			AddButton (GettextCatalog.GetString  ("_Use Case"), SetUseCase);
		}

		private void SetClass (object sender, EventArgs args)
		{
			_selection = "Class"; //No i18n
		}
		
		private void SetUseCase (object sender, EventArgs args)
		{
			_selection = "Use Case";//No i18n
		}

	}
}
