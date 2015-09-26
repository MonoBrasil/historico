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

	internal class AssemblyConstructor : AssemblyOperation 
	{

		public AssemblyConstructor (AssemblyImporter importer, Uml2.Class umlType,
			ConstructorInfo constructor) : base (importer, umlType)
		{
			_ctorInfo = constructor;
			Begin ();
		}
		
		public override void Begin ()
		{
			Uml2.Operation operation = Uml2.Create.Operation ();
			operation.Class = (Uml2.Class) _umlType;
			operation.Name = _umlType.Name;

			if (_ctorInfo.IsFamily)
			{
				operation.Visibility = ExpertCoder.Uml2.VisibilityKind.@protected;
			}
			else if (_ctorInfo.IsPrivate)
			{
				operation.Visibility = ExpertCoder.Uml2.VisibilityKind.@private;
			}
			else if (_ctorInfo.IsPublic)
			{
				operation.Visibility = ExpertCoder.Uml2.VisibilityKind.@public;
			}
			
			operation.IsStatic = _ctorInfo.IsStatic;
			
			CreateOperation (operation, _ctorInfo.GetParameters ());
		}
		
		private ConstructorInfo _ctorInfo;
	}
}