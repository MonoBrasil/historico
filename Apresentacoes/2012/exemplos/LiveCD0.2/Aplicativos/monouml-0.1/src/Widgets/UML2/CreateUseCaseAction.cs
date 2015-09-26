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
namespace MonoUML.Widgets.UML2
{
	public class CreateUseCaseAction : CreateNodeAction
	{
		public CreateUseCaseAction (UMLDiagram owningDiagram) : base (owningDiagram)
		{
		}

		// the label that the user will see
		public override string Name
		{
			get { return "Create a Use Case"; }
		}

		public override void Execute ()
		{
			CreateUseCaseAction.Execute (_owningDiagram, base.Position); 
		}

		public static void Execute (UMLDiagram owningDiagram)
		{
			Execute (owningDiagram, new MonoUML.DI.Point (0, 0));
		}

		public static void Execute (UMLDiagram owningDiagram, MonoUML.DI.Point position)
		{
			ExpertCoder.Uml2.UseCase useCaseModel = (ExpertCoder.Uml2.UseCase)MonoUML.Widgets.Helper.CreateUmlElement ("UseCase"); 
			UMLUseCase useCase = UMLUseCase.CreateNew (owningDiagram, useCaseModel);
			useCase.Move (position.X, position.Y);
			owningDiagram.AddNewClassifier(useCase, useCaseModel);
		}
	}
}
