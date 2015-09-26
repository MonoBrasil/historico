/*
MonoUML.ReverseEngineering - Reverse Engineering from .NET assemblies
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
using System;

namespace MonoUML.ReverseEngineering
{
	internal enum AssemblyDataStructureType
	{
		Class,
		Struct,
		DataType
	}

	internal abstract class AssemblyDataStructure : AssemblyType 
	{

		public AssemblyDataStructure (AssemblyDataStructureType dType, 
			AssemblyImporter importer, System.Type systemType) : base (importer, systemType)
		{
			_dType = dType;
		}
		
		public override void Begin ()
		{
			if (!_alreadyImported)
			{
				if (_nested)
				{
					BeginNested ();
				}
				else
				{
					BeginNotNested ();
				}
				//Enables full/assembly-only importing
				System.Type type = AssemblyHelper.SearchWithinAssemblies (_importer, _systemType.FullName);
				if (_importer.ImportInterfaces) { GetInterfaces (); }
				if ((type != null && _importer.ImportingLevel == AssemblyImporterLevel.AssemblyImport)
					|| (_importer.ImportingLevel == AssemblyImporterLevel.FullImport))
				{
					GetMethods ();
					GetFields ();
				}
				SetVisibility ();
			}
		}
		
		protected abstract void BeginNested ();
		protected abstract void BeginNotNested ();
		
		protected void GetFields ()
		{
			FieldInfo[] fields = _systemType.GetFields (AssemblyHelper.BINDING_FLAGS);
			bool import = true;
			
			foreach (FieldInfo fi in fields)
			{
				if (fi.IsFamily)
				{
					import = _importer.ImportProtectedFields;
				}
				else if (fi.IsPrivate)
				{
					import = _importer.ImportPrivateFields;
				}
				else if (fi.IsPublic)
				{
					import = _importer.ImportPublicFields;
				}
				
				if (import == true)
				{
					AssemblyField field = new AssemblyField (_importer, fi);
					
					if (_dType == AssemblyDataStructureType.Class
						|| _dType == AssemblyDataStructureType.Struct)
					{
						((Uml2.Class) _umlType).OwnedAttribute.Add (field.Property);
					}
				}
				import = true;
			}
		}

		protected void GetInterfaces ()
		{
			Type[] interfaces = _systemType.GetInterfaces ();
			Type[] baseTypeIfaces = _systemType.BaseType == null ? new Type[0] : _systemType.BaseType.GetInterfaces ();
			bool crossIfaceImpl;

			foreach (Type iface in interfaces)
			{
				// if the interface isn't inherited from the base class,
				// generate an InterfaceRealization representation.
				if (Array.IndexOf(baseTypeIfaces, iface) == -1)
				{
					// checks that the current interface is not implemented by another interface
					crossIfaceImpl = false;
					foreach (Type otherIface in interfaces)
					{
						if (Array.IndexOf(otherIface.GetInterfaces(), iface) != -1)
						{
							crossIfaceImpl = true;
							break;
						}
					}
					if (!crossIfaceImpl)
					{
						AssemblyInterface intrface = new AssemblyInterface (_importer, iface);
						intrface.Begin ();
						try
						{
							Uml2.InterfaceRealization interfaceRealization = Uml2.Create.InterfaceRealization ();
							interfaceRealization.Name = intrface.Name+"Contract";
							interfaceRealization.Contract = (Uml2.Interface) intrface.UmlType;
							if (_dType == AssemblyDataStructureType.Class
								|| _dType == AssemblyDataStructureType.Struct)
							{
								((Uml2.Class) _umlType).InterfaceRealization.Add (interfaceRealization);
							}
						}
						catch (Exception ex)
						{
							System.Console.WriteLine ("AssemblyDataStructure.GetInterfaces> ignored exception: " + ex.Message);
						}
					}
				}
			}
		}

		protected void GetMethods ()
		{
			MethodInfo []methods = _systemType.GetMethods (AssemblyHelper.BINDING_FLAGS);
			bool import = true;

			foreach (MethodInfo mf in methods )
			{
				if (mf.IsFamily)
				{
					import = _importer.ImportProtectedMethods;
				}
				else if (mf.IsPrivate)
				{
					import = _importer.ImportPrivateMethods;
				}
				else if (mf.IsPublic)
				{
					import = _importer.ImportPublicMethods;
				}
			
				if (mf.Name.IndexOf (".") > 0)
				{
					//Interface...
					//System.Console.WriteLine ("Is an interface! "+mf.Name); 
				}
				else
				{
					if (import)
					{
						AssemblyMethod method = new AssemblyMethod (_importer, _umlType, mf);
						
						if (_dType == AssemblyDataStructureType.Class 
							|| _dType == AssemblyDataStructureType.Struct)
						{
							((Uml2.Class) _umlType).OwnedOperation.Add (method.Operation);
						} 
						else if (_dType == AssemblyDataStructureType.DataType)
						{
							((Uml2.PrimitiveType) _umlType).OwnedOperation.Add (method.Operation);
						}
					}
				}
				import = true;
			}
		}

		protected AssemblyDataStructureType _dType;
	}

}
