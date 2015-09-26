/*
MonoUML.I18N - I18n
Copyright (C) 2004  Eduardo 'enzo' García

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
using System;
using Mono.Posix;

namespace MonoUML.I18n
{
	public class GettextCatalog
	{
		static GettextCatalog ()
		{
			Catalog.Init ("monouml", "/usr/share/locale/");
		}
	
		public static string GetString (string str)
		{
			return Catalog.GetString (str);
		}
	
		public static string GetPluralString (string singular, string plural, int n)
		{
			return Catalog.GetPluralString (singular, plural, n);
		}

	}
}
