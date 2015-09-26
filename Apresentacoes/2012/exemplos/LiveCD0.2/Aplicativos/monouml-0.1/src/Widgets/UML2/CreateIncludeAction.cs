/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2005  Rodolfo Campero <rodolfo.campero@gmail.com>

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
using UML = ExpertCoder.Uml2;

namespace MonoUML.Widgets.UML2
{
	public class CreateIncludeAction : CreateEdgeAction
	{
		public CreateIncludeAction (UMLDiagram owningDiagram) : base (owningDiagram)
		{
		}

		public override string Name
		{
			get { return "Create an Include relationship"; }
		}

		public override void Execute ()
		{
			CreateIncludeAction.Execute (_owningDiagram, base.FromElement, base.ToElement);
		}

		public static void Execute
		(
			UMLDiagram owningDiagram, 
			UMLElement fromElement, 
			UMLElement toElement
		)
		{ 
			DI.GraphElement fromGE = fromElement.GraphElement;
			DI.GraphElement toGE = toElement.GraphElement;
			UML.UseCase includingCase = MonoUML.Widgets.Helper.GetSemanticElement (fromGE) as UML.UseCase; 
			UML.UseCase addition = MonoUML.Widgets.Helper.GetSemanticElement (toGE) as UML.UseCase; 
			if (includingCase != null && addition != null)
			{
				// creates the new Include in the model
				UML.Include includeModel = UML.Create.Include ();
				includingCase.Include.Add (includeModel);
				includeModel.IncludingCase = includingCase;
				includeModel.Addition = addition;
				// creates the graphical representation of the new Include
				UMLStereotypedEdge.CreateNewAndAdd (owningDiagram, includeModel, fromGE, toGE);
				Hub.Instance.Broadcaster.BroadcastElementChange (includingCase);
			}
		}
	}
}
