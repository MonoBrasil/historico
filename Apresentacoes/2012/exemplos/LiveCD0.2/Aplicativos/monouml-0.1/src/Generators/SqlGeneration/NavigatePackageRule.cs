/*
MonoUML.Widgets - A library for representing the Widget elements
Copyright (C) 2005  Miguel Huerta

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
using System;
using ES = ExpertCoder.ExpertSystem;
using UML = ExpertCoder.Uml2;

namespace MonoUML.Generators.SqlGeneration
{
	public class NavigatePackageRule: ES.Rule
	{
		public NavigatePackageRule()
			:base(new ES.InputElementInhibition())
		{
		}
	
		public override bool IsActiveInState(ES.Environment env)
		{
			UML.Package pkg = env.CurrentInputElement as UML.Package;
			return pkg!=null && ((UML.NamedElement)pkg).Name!="System";
		}

		public override void Execute(ES.Environment env)
		{
			UML.Package pkg = env.CurrentInputElement as UML.Package;
			foreach(UML.Type type in pkg.OwnedType)
			{
				type.Package = pkg;
				if(env.VerboseLevel>=2) 
					Console.WriteLine("    - NavigatePackageRule: cie=" + ((UML.NamedElement)type).Name);
				env.PushState();
				env.CurrentInputElement = type;
				env.Expert.Process();
				env.PopState();
			}
			foreach(UML.Package nestedPkg in pkg.NestedPackage)
			{
				nestedPkg.NestingPackage = pkg;
				env.PushState();
				env.CurrentInputElement = nestedPkg;
				env.Expert.Process();
				env.PopState();
			}
		}
	}
}
