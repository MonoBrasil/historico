/*
MonoUML.Preferences - Handles the settings from GConf
Copyright (C) 2005  Mario Carrión  <mario.carrion@gmail.com>

This file is part of MonoUML.

MonoUML is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

MonoUML is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MonoUML; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
using Gtk;
using Glade;

namespace MonoUML.Preferences
{
	public class Core
	{
		public static readonly Core Instance = new Core ();

		private Core ()
		{
			_glade = new XML ("gui.glade", "_preferencesDlg");
			_glade.Autoconnect (this);
		}

		public void Run ()
		{
			_preferencesDlg.Run ();
		}
		
		private XML _glade;
		[Widget] private Dialog _preferencesDlg;  
	}
}
