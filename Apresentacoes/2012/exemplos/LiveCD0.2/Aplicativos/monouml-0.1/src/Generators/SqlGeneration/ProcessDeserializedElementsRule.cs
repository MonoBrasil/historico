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
using System.Collections;
using ES = ExpertCoder.ExpertSystem;
using UML = ExpertCoder.Uml2;

namespace MonoUML.Generators.SqlGeneration
{
	public class ProcessDeserializedElementsRule: ES.Rule
	{
		public ProcessDeserializedElementsRule(): base(new ES.InputElementInhibition()) {}

		public override bool IsActiveInState(ES.Environment env)
		{
			return env.CurrentInputElement==env.InputModel;
		}
		
		public override void Execute(ES.Environment env)
		{
			foreach(object o in (IEnumerable)env.InputModel)
			{
				env.PushState();
				env.CurrentInputElement = o;
				System.Console.WriteLine("objeto en push " +o);
				env.Expert.Process();
				env.PopState();
			}
		}
	}
}
