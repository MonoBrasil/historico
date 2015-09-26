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

	//TODO: To fix the Generalization.
	internal class AssemblyInterface : AssemblyType
	{

		public AssemblyInterface (AssemblyImporter importer, System.Type systemType)
			: base (importer, systemType)
		{
		}
		
		public override void Begin ()
		{
			if (!_alreadyImported)
			{
				_umlType = (Uml2.Interface) Uml2.Create.Interface ();
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
				GetInterfaces ();
				//
				System.Type type = AssemblyHelper.SearchWithinAssemblies (_importer, _systemType.FullName);
				if ((type != null && _importer.ImportingLevel == AssemblyImporterLevel.AssemblyImport)
					|| (_importer.ImportingLevel == AssemblyImporterLevel.FullImport))
				{
					GetMethods ();
				}					
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
							Uml2.Generalization generalization = Uml2.Create.Generalization ();
							generalization.General = (Uml2.Interface) intrface.UmlType;
							((Uml2.Interface) _umlType).Generalization.Add (generalization);
						}
						catch (Exception ex) { System.Console.WriteLine ("ex"+ex.Message); }
					}
				}
			}
		}
		
		private void GetMethods ()
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
				if (import)
				{
					AssemblyMethod method = new AssemblyMethod (_importer, _umlType, mf);
					((Uml2.Interface) _umlType).OwnedOperation.Add (method.Operation);
				}
				import = true;
			}
		}
	

	}

}
