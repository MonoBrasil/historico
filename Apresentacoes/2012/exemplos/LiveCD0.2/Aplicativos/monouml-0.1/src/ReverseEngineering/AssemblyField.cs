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
	internal class AssemblyField 
	{

		public AssemblyField (AssemblyImporter importer, FieldInfo field)
		{
			_importer = importer;
			_fieldInfo = field;
			Begin ();
		}
		
		public Uml2.Property Property
		{
			get
			{
				return (Uml2.Property) _property;
			}
		}
		
		public void Begin ()
		{
			_property = Uml2.Create.Property ();
			_property.Name = _fieldInfo.Name;
			
			AssemblyHelper.SetParameter (_property, _fieldInfo.FieldType, _importer);

			if (_fieldInfo.IsFamily)
			{
				_property.Visibility = ExpertCoder.Uml2.VisibilityKind.@protected;
			}
			else if (_fieldInfo.IsPrivate)
			{
				_property.Visibility = ExpertCoder.Uml2.VisibilityKind.@private;
			}
			else if (_fieldInfo.IsPublic)
			{
				_property.Visibility = ExpertCoder.Uml2.VisibilityKind.@public;
			}
			else if (_fieldInfo.IsAssembly)
			{
				_property.Visibility = ExpertCoder.Uml2.VisibilityKind.@package;
			}

			_property.IsStatic = _fieldInfo.IsStatic;
		}
		
		private FieldInfo _fieldInfo;
		private Uml2.Property _property;
		private AssemblyImporter _importer;
	}
}