/*
MonoUML.Widgets.UML2 - A library for representing the UML2 elements
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
using Gtk;
using Gnome;
using System;
using System.Collections;
using DI = MonoUML.DI;
using MonoUML.DI.Uml2Bridge;
using Widgets = MonoUML.Widgets;
using UML = ExpertCoder.Uml2;

namespace MonoUML.Widgets.UML2
{
	public class UMLClassDiagram : UMLDiagram
	{
		public UMLClassDiagram (DI.Diagram diagram, Widgets.NoteBook notebook)
			: base (diagram, notebook)
		{ }

		protected override void DrawElement (DI.GraphEdge edge)
		{
			Uml2SemanticModelBridge bridge = edge.SemanticModel as Uml2SemanticModelBridge;
			UML.Element umlElement = bridge.Element;
			if (umlElement is UML.Association)
			{
				UMLAssociation association = new UMLAssociation (this, edge);
				_canvas.AddElement (association);
			}
			else if (umlElement is UML.Generalization)
			{
				UMLGeneralization generalization = new UMLGeneralization (this, edge);
				_canvas.AddElement (generalization);
			}
			else if (umlElement is UML.InterfaceRealization)
			{
				UMLInterfaceRealization interfaceRealization = new UMLInterfaceRealization (this, edge);
				_canvas.AddElement (interfaceRealization);
			}
			else
			{
				base.DrawElement(edge);
			}
		}

		protected override void GetDerivedContextMenuOptions (ArrayList options)
		{
			options.Add (new CreateClassAction (this));
		}
	}
	
}
