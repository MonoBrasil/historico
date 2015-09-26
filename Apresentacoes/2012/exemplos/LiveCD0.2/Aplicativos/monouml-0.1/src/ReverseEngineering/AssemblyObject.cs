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
using System;

namespace MonoUML.ReverseEngineering
{
	internal abstract class AssemblyObject
	{

		public AssemblyObject (AssemblyImporter importer, Type systemType)
		{
			_systemType = systemType;
			_importer = importer;
			_name = _systemType.Name;
		}
		
		public Uml2.Type UmlType
		{
			get
			{
				return _umlType;
			}
		}
		
		public string Name
		{
			get
			{
				return _name;
			}
		}
		
		public abstract void Begin ();

		protected virtual void SetVisibility ()
		{
			Uml2.VisibilityKind visibility =  ExpertCoder.Uml2.VisibilityKind.@public;
			
			if (_systemType.IsNestedPublic || _systemType.IsPublic)
			{
				visibility = ExpertCoder.Uml2.VisibilityKind.@public;
			}
			else if (_systemType.IsNestedFamily)
			{
				visibility = ExpertCoder.Uml2.VisibilityKind.@protected;
			} 
			else if (_systemType.IsNestedPrivate)
			{
				visibility = ExpertCoder.Uml2.VisibilityKind.@private;
			}
			else if (!_systemType.IsNestedPrivate && !_systemType.IsNestedPublic)
			{
				visibility = ExpertCoder.Uml2.VisibilityKind.@package;
			}
			
			((Uml2.Type) _umlType).Visibility = visibility;
		}
		
		protected AssemblyImporter _importer;
		protected string _name = "";
		protected System.Type _systemType;
		protected Uml2.Type _umlType = null;
	}
}
