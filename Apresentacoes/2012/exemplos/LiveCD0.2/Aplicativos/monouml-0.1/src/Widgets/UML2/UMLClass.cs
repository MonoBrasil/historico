/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2005  Rodolfo Campero <rodolfo.campero@gmail.com>

UMLClass.cs: the UML2 representation of Classs

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
using Gnome;
using Gtk;
using DI = MonoUML.DI;
using UML = ExpertCoder.Uml2;
using MonoUML.DI.Uml2Bridge;

namespace MonoUML.Widgets.UML2
{
	public class UMLClass : UMLClassifier
	{
		public UMLClass (UMLDiagram ownerDiagram, DI.GraphNode graphNode): base (ownerDiagram, graphNode)
		{
		}

		protected override void AddContextMenuOptions (System.Collections.IList options)
		{
			options.Add (new CreateAssociationAction (_ownerDiagram));
			base.AddContextMenuOptions (options);
		}

		public static UMLClass CreateNew (
			UMLDiagram ownerDiagram,
			UML.Class clsModel )
		{
			// creates the graphical representation of the new Class
			DI.GraphNode classifierGN = new DI.GraphNode ();
			//    model bridge to the UML model element (Class)
			Uml2SemanticModelBridge bridge = new Uml2SemanticModelBridge (); 
			bridge.Element = clsModel;
			classifierGN.SemanticModel = bridge;
			// graphical representation of the property "Name"
			DI.GraphNode nameGN = new DI.GraphNode ();
			//    graphical properties
			nameGN.Position.X = 4D;
			nameGN.Position.Y = 6D;
			nameGN.Property[DI.StandardProperty.FontFamily] = "Verdana";
			nameGN.Property[DI.StandardProperty.FontSize] = "10";
			//    model bridge to the property
			DI.SimpleSemanticModelElement nameBridge = new DI.SimpleSemanticModelElement ();
			nameBridge.TypeInfo = "NameCompartment";
			nameGN.SemanticModel = nameBridge;
			// adds the name GN to the Use Case GN
			classifierGN.Contained.Add (nameGN);
			nameGN.Container = classifierGN;
			// adds the classifier to the diagram
			ownerDiagram.DIDiagram.Contained.Add (classifierGN);
			classifierGN.Container = ownerDiagram.DIDiagram;
			return new UMLClass (ownerDiagram, classifierGN);
		}
	}	
}