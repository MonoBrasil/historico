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

namespace MonoUML.ReverseEngineering
{
	internal class AssemblyClass : AssemblyDataStructure  
	{
		public AssemblyClass (AssemblyImporter importer, System.Type sType)
			: base (AssemblyDataStructureType.Class, importer, sType)
		{
		}
		
		//This ONLY happens when new classes are added
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
			catch (System.Exception ex)
			{
				System.Console.WriteLine ("AssemblyClass.BeginNotNested> ignored exception: " + ex.Message);
			}
			Loop ();
		}
		
		//This ONLY happens when new classes are added
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
				catch (System.Exception ex) { }
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
			Loop ();
		}
		
		protected override void SetVisibility ()
		{
			Uml2.Class isClass = _umlType as Uml2.Class;
			if (isClass != null)
			{
				((Uml2.Class) _umlType).IsAbstract  = _systemType.IsAbstract;
			}
			base.SetVisibility ();
		}
		
		private void GetConstructors ()
		{
			ConstructorInfo []constructors = _systemType.GetConstructors (AssemblyHelper.BINDING_FLAGS);
			bool import = true;
			foreach (ConstructorInfo cf in constructors)
			{
				if (cf.IsFamily)
				{
					import = _importer.ImportProtectedMethods;
				}
				else if (cf.IsPrivate)
				{
					import = _importer.ImportPrivateMethods;
				}
				else if (cf.IsPublic)
				{
					import = _importer.ImportPublicMethods;
				}
				if (import)
				{
					AssemblyConstructor constructor = new AssemblyConstructor (_importer, (Uml2.Class) _umlType, cf);
					((Uml2.Class) _umlType).OwnedOperation.Add (constructor.Operation);
				}
				import = true;
			}
		}
		
		private void Loop ()
		{
			bool doFullImport = AssemblyHelper.SearchWithinAssemblies (_importer, _systemType.FullName) != null;
			if (doFullImport) { GetConstructors (); }
			// even if we aren't doing a full import, we must create the complete inheritance chain;
			// in this case, these classes will be empty, only their base classes
			// and implemented interfaces will be represented.
			System.Type baseType = _systemType.BaseType;
			if (baseType != null)
			{
				if (!baseType.FullName.Equals ("System.Object")) //FIXME. is this all right!?
				{
					AssemblyClass assemblyClass = new AssemblyClass (_importer, baseType);
					assemblyClass.Begin ();
					Uml2.Generalization generalization = Uml2.Create.Generalization ();
					generalization.General = (Uml2.Class) assemblyClass.UmlType;
					((Uml2.Class) _umlType).Generalization.Add (generalization);
				}
			}
		}

	}
}
