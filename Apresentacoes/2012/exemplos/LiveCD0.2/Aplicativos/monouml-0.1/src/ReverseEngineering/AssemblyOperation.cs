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

	internal abstract class AssemblyOperation
	{
	
		public AssemblyOperation (AssemblyImporter importer, Uml2.Type umlType)
		{
			_importer = importer;
			_umlType = umlType;
		}

		public Uml2.Operation Operation
		{
			get 
			{
				return _operation;
			}
		}
		
		public abstract void Begin ();

		protected void CreateOperation (Uml2.Operation operation, ParameterInfo[] parms)
		{
			_operation = operation;

			foreach (ParameterInfo parm in parms)
			{
				Uml2.Parameter parameter = Uml2.Create.Parameter ();
				parameter.Name = parm.Name;
				AssemblyHelper.SetParameter (parameter, parm.ParameterType, _importer);
				_operation.OwnedParameter.Add (parameter);
			}
		}

		protected AssemblyImporter _importer;
		protected Uml2.Operation _operation;
		protected Uml2.Type _umlType;
	}
}