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

namespace MonoUML.ReverseEngineering
{
	internal abstract class AssemblyType : AssemblyObject
	{
		public AssemblyType (AssemblyImporter importer, System.Type systemType)
			: base (importer, systemType)
		{
			_lastPkg = null;
			IsNested ();
			StartPackaging ();
			LookForMe ();
		}
		
		public bool AlreadyImported
		{
			get
			{
				return _alreadyImported;
			}
		}
		
		public Uml2.Package Package
		{
			get
			{
				return _lastPkg;
			}
		}

		private void IsNested ()
		{
			if (_systemType.IsNestedAssembly || _systemType.IsNestedFamANDAssem ||
					_systemType.IsNestedFamily ||  _systemType.IsNestedFamORAssem ||
					_systemType.IsNestedPrivate || _systemType.IsNestedPublic)
			{
				_nested = true;
			}
			else
			{
				_nested = false;
			}
		}
		
		private void LookForMe ()
		{
			AssemblyType found = (AssemblyType) _importer.Elements [_systemType.FullName];
			if (found != null)
			{
				_umlType = found.UmlType;
				_name = found.Name;
				_alreadyImported = true;
			}
		}
		
		private void StartPackaging ()
		{
			if (_systemType.FullName.IndexOf (".") != -1)
			{
				AssemblyPackage pkg = new AssemblyPackage (_importer, _systemType);
				pkg.Begin ();
				_lastPkg = pkg.LastPackage;
			}
		}
		
		protected bool _alreadyImported;
		protected bool _nested;
		protected Uml2.Package _lastPkg;
		protected AssemblyType _ownerType;
	}
}