/*
MonoUML.Widgets - A library for representing the Widget elements
Copyright (C) 2004  Rodolfo Campero
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
using IconLibrary = MonoUML.IconLibrary;

namespace MonoUML.Widgets
{
	public class RootLevelElementChooserDialog : ChooserDialog
	{
		public RootLevelElementChooserDialog () : base (GettextCatalog.GetString ("Choose a root-level element"))
		{
			AddButton (GettextCatalog.GetString ("Model"), OnModel);
			_selection = "Model";
			AddButton (GettextCatalog.GetString ("Package"), OnPackage);
			AddButton (GettextCatalog.GetString ("Profile"), OnProfile);
		}

		private void OnModel (object sender, EventArgs args)
		{
			_selection = "Model"; //No i18n
		}
		
		private void OnPackage (object sender, EventArgs args)
		{
			_selection = "Package"; //No i18n
		}

		private void OnProfile (object sender, EventArgs args)
		{
			_selection = "Profile"; //No i18n
		}

	}
}
