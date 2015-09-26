/*
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

	internal class AssemblyDataType : AssemblyDataStructure  
	{
	
		public AssemblyDataType (AssemblyImporter importer, System.Type sType)
			: base (AssemblyDataStructureType.DataType, importer, sType)
		{
		}

		protected override void BeginNotNested ()
		{
			BeginAnyway ();
		}
		
		protected override void BeginNested ()
		{
			BeginAnyway ();
		}
		
		private void BeginAnyway () 
		{
			_umlType = (Uml2.PrimitiveType) Uml2.Create.PrimitiveType (); 
			//_umlType = (Uml2.DataType) Uml2.Create.DataType ();
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
			}
			catch (System.Exception ex) { }

		}

	}
}