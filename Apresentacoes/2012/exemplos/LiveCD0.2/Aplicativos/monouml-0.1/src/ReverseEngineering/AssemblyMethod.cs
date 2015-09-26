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
	internal class AssemblyMethod : AssemblyOperation 
	{
		public AssemblyMethod (AssemblyImporter importer, Uml2.Type umlType, MethodInfo method) 
			: base (importer, umlType)
		{
			_methodInfo = method;
			Begin ();
		}
		
		public override void Begin ()
		{ 
			Uml2.Operation operation = Uml2.Create.Operation ();
			Uml2.Class isClass = _umlType as Uml2.Class;
			if (isClass != null)
			{
				//operation.Class = (Uml2.Class) _umlType;
				operation.Class = (Uml2.Class) _umlType;
			}
			else
			{
				Uml2.Interface isInterface = _umlType as Uml2.Interface;
				if (isInterface != null)
				{
					operation.Owner = (Uml2.Interface) _umlType;
				}
				else
				{
					operation.Datatype = (Uml2.DataType) _umlType;
				}
			}
			operation.Name = _methodInfo.Name;

			CreateOperation (operation, _methodInfo.GetParameters ());

			if (_methodInfo.IsFamily)
			{
				operation.Visibility = ExpertCoder.Uml2.VisibilityKind.@protected;
			}
			else if (_methodInfo.IsPrivate)
			{
				operation.Visibility = ExpertCoder.Uml2.VisibilityKind.@private;
			}
			else if (_methodInfo.IsPublic)
			{
				operation.Visibility = ExpertCoder.Uml2.VisibilityKind.@public;
			}
			
			operation.IsStatic = _methodInfo.IsStatic;
			
			AssemblyHelper.SetParameter (Operation, _methodInfo.ReturnType, _importer);
		}

		private MethodInfo _methodInfo;
	}
}