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
	internal class AssemblyStruct : AssemblyDataStructure
	{	
		public AssemblyStruct (AssemblyImporter importer, System.Type type) :
			base (AssemblyDataStructureType.Struct, importer, type)
		{
		}
		
		protected override void BeginNotNested ()
		{
			_umlType = (Uml2.Class) Uml2.Create.Class ();
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
		
		//This ONLY happens when new structs are added
		protected override void BeginNested ()
		{
			string ownerClass = AssemblyHelper.GetNestedClass (_systemType.FullName);
			_ownerType = (AssemblyType) _importer.Elements [ownerClass];
			if (_ownerType == null)
			{
				AssemblyHelper.CreateAssemblyType (_importer , _systemType.DeclaringType);
				_ownerType = (AssemblyType) _importer.Elements [ownerClass];
			}			
			AssemblyClass assemblyClass = _ownerType as AssemblyClass;
			AssemblyStruct assemblyStruct = _ownerType as AssemblyStruct;
			
			if (assemblyClass != null)
			{
				try 
				{
					_umlType = (Uml2.Class) Uml2.Create.Class ();
					_umlType.Name = Name;
					((Uml2.Class) assemblyClass.UmlType).NestedClassifier.Add (_umlType);
					_importer.Elements.Add (_systemType.FullName, this);
				}
				catch (System.Exception ex) {}
			}
			else if (assemblyStruct != null)
			{
				try
				{
					_umlType = (Uml2.Class) Uml2.Create.Class ();
					_umlType.Name = Name;
					((Uml2.Class) assemblyStruct.UmlType).NestedClassifier.Add (_umlType);
					_importer.Elements.Add (_systemType.FullName, this);
				}
				catch (System.Exception ex) {}
			}

		}
	}
}