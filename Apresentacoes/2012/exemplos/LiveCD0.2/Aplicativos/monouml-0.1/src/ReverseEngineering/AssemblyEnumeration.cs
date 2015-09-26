/*
MonoUML.ReverseEngineering - Does the RE from an .NET assembly
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
using Uml2 = ExpertCoder.Uml2;
using System.Reflection;

namespace MonoUML.ReverseEngineering
{
	//TODO: Add BaseType for enumerations
	internal class AssemblyEnumeration : AssemblyType
	{	
		public AssemblyEnumeration (AssemblyImporter importer, System.Type systemType)
			: base (importer, systemType)
		{
		}

		public override void Begin ()
		{
			if (!_alreadyImported)
			{
				_umlType = (Uml2.Enumeration ) Uml2.Create.Enumeration ();
				_umlType.Name = Name;
				try
				{
					if (_lastPkg != null)
					{
						_umlType.Namespace = _lastPkg;
						_lastPkg.OwnedType.Add (_umlType);
					}
					_importer.Elements.Add (_systemType.FullName, this);
					_importer.XmiElements.Add (_umlType);
					GetFields ();
				}
				catch (System.Exception ex) { }
			}
		}
		
		private void GetFields ()
		{
			FieldInfo[] fields = _systemType.GetFields (AssemblyHelper.BINDING_FLAGS);

			foreach (FieldInfo fi in fields)
			{
				if (!fi.IsSpecialName)
				{
					Uml2.EnumerationLiteral literal = Uml2.Create.EnumerationLiteral ();
					literal.Name = fi.Name;
					((Uml2.Enumeration) _umlType).OwnedLiteral.Add (literal);
				}
			}
		}

	}
}