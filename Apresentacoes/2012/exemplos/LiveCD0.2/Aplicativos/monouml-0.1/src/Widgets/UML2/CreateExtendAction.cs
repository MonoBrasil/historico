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
	public class CreateExtendAction : CreateEdgeAction
	{
		public CreateExtendAction (UMLDiagram owningDiagram) : base (owningDiagram)
		{
		}

		public override string Name
		{
			get { return "Create an Extend relationship"; }
		}

		public override void Execute ()
		{
			CreateExtendAction.Execute (_owningDiagram, base.FromElement, base.ToElement);
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
			UML.UseCase extension = MonoUML.Widgets.Helper.GetSemanticElement (fromGE) as UML.UseCase; 
			UML.UseCase extendedCase = MonoUML.Widgets.Helper.GetSemanticElement (toGE) as UML.UseCase; 
			if (extension != null && extendedCase != null)
			{
				// creates the new Extend in the model
				UML.Extend extendModel = UML.Create.Extend ();
				extension.Extend.Add (extendModel);
				extendModel.Extension = extension;
				extendModel.ExtendedCase = extendedCase;
				// creates the graphical representation of the new Extend
				UMLStereotypedEdge.CreateNewAndAdd (owningDiagram, extendModel, fromGE, toGE);
				Hub.Instance.Broadcaster.BroadcastElementChange (extension);
			}
		}
	}
}
