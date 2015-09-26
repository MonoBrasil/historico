/*
MonoUML.Widgets - A library for representing the Widget elements
Copyright (C) 2004  Rodolfo Campero

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
using MonoUML.DI.Uml2Bridge;

namespace MonoUML.Widgets.UML2
{
	public sealed class Helper
	{
		private Helper() {}

		// returns the classifier that is under the mouse pointer
		public static UML.Classifier GetHoverClassifier (UMLDiagram ownerDiagram, object ignoredElement)
		{
			UML.Classifier hoverClassifier = null;
			UMLElement hoverElement = ownerDiagram.UMLCanvas.GetHoverElement (ignoredElement);
			// checks that the new model element is a Classifier 
			if (hoverElement != null)
			{
				UML.Element hoverModel = MonoUML.Widgets.Helper.GetSemanticElement (hoverElement.GraphElement);
				if (hoverModel != null)
				{
					hoverClassifier = hoverModel as UML.Classifier;
				}
			}
			return hoverClassifier;
		}
	}
}
