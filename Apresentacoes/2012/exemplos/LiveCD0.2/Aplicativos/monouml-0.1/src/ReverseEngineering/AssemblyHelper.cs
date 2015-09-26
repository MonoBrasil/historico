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
	internal class AssemblyHelper
	{
	
		public static string GetNestedClass (string name)
		{
			return name.Substring (0, name.LastIndexOf ("+"));
		}

		public static System.Type SearchWithinAssemblies (AssemblyImporter importer, string fName)
		{
			System.Type newType = null;
			foreach (System.Reflection.Assembly assmb in importer.Assemblies) //Looping in all the imported assemblies
			{
				newType = assmb.GetType (fName);
				if (newType != null)
				{
					break;
				}
			}
			if (newType == null) //Searching in mscorlib.dll
			{
				if (importer.ImportingLevel == AssemblyImporterLevel.FullImport)
				{
					newType = System.Type.GetType (fName); //MUST BE FOUND HERE!
				}
			}
			return newType;
		}
		
		public static void SetParameter (object umlType, System.Type systemType, AssemblyImporter importer)
		{		
			while (systemType.HasElementType)//Removing &, *, and []
			{ 
				if (systemType.IsByRef || systemType.IsPointer)
				{
					Uml2.Parameter parameter = umlType as Uml2.Parameter;
					if  (parameter != null)
					{
						parameter.Direction = ExpertCoder.Uml2.ParameterDirectionKind.inout;
					}
				}
				if (systemType.IsArray)
				{
					Uml2.MultiplicityElement array = umlType as Uml2.MultiplicityElement;
					if (array != null)
					{ 
						array.Lower = 0;
						array.Upper = Uml2.UnlimitedNatural.Infinity;
					}
				}
				systemType = systemType.GetElementType ();
			}
			
			AssemblyType assemblyType = AssemblyHelper.CreateAssemblyType (importer, systemType);

			Uml2.TypedElement umlTyped = umlType as Uml2.TypedElement;
			if (umlTyped != null && assemblyType != null)
			{
				umlTyped.Type = assemblyType.UmlType;
			}
		}
		
		public static AssemblyType CreateAssemblyType (AssemblyImporter importer, System.Type systemType)
		{
			AssemblyType assemblyType = null;
			if (systemType.IsEnum)
			{
				assemblyType = new AssemblyEnumeration (importer, systemType);
				assemblyType.Begin ();
			}
			else if (systemType.IsInterface)
			{
				assemblyType = new AssemblyInterface (importer, systemType);
				assemblyType.Begin ();
			}
			else if (systemType.IsClass)
			{
				assemblyType = new AssemblyClass (importer, systemType);
				assemblyType.Begin ();
			}
			else if (systemType.IsPrimitive)
			{
				assemblyType = new AssemblyDataType (importer, systemType);
				assemblyType.Begin ();
			}
			else if (systemType.IsValueType)
			{
				if (systemType.GetMembers (AssemblyHelper.BINDING_FLAGS).Length == 1)
				{
					assemblyType = new AssemblyDataType (importer, systemType);
					assemblyType.Begin ();
				}
				else
				{
					assemblyType = new AssemblyStruct (importer, systemType);
					assemblyType.Begin ();
				}
			}
			else
			{
				System.Console.WriteLine ("System.FullName: "+systemType.FullName+" sealed: "+systemType.IsSealed );
			}
			return assemblyType;
		}
		
		public static BindingFlags BINDING_FLAGS = 
					BindingFlags.Instance |
					BindingFlags.Public |
					BindingFlags.NonPublic |
					BindingFlags.Static |
					BindingFlags.DeclaredOnly;
	}									
}