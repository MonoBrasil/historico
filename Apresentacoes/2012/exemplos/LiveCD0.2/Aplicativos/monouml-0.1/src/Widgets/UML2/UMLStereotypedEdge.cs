/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2005  Rodolfo Campero <rodolfo.campero@gmail.com>

UMLStereotypedEdge.cs: the UML2 StereotypedEdge

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
using Gdk;
using DI = MonoUML.DI;
using UML = ExpertCoder.Uml2;
using MonoUML.DI.Uml2Bridge;

namespace MonoUML.Widgets.UML2
{
	public class UMLStereotypedEdge : UMLEdge
	{
		public UMLStereotypedEdge
		(
			UMLDiagram ownerDiagram,
			DI.GraphEdge graphEdge,
			UML.Relationship edgeModel
		)	: base (ownerDiagram, graphEdge, false)
		{
			_modelElement = edgeModel;
			base.AddToEnd (new UMLOpenArrow (ChildrensRoot));
			SearchAndDrawStereotype (graphEdge, ownerDiagram);
			base.ForceRedraw ();
		}

		public UMLStereotypedEdge (UMLDiagram ownerDiagram, DI.GraphEdge graphEdge)
			: this (ownerDiagram, graphEdge, (UML.Relationship) MonoUML.Widgets.Helper.GetSemanticElement (graphEdge))
		{
		}
		
		public static UMLStereotypedEdge CreateNewAndAdd
		(
			UMLDiagram owningDiagram,
			UML.Relationship stereotypedEdgeModel,
			DI.GraphElement fromGE,
			DI.GraphElement toGE
		)
		{
			// creates the graphical representation of the new Stereotyped Edge
			DI.GraphEdge stereotypedEdgeGE = new DI.GraphEdge ();
			// model semanticModelBridge to the UML model element
			Uml2SemanticModelBridge semanticModelBridge = new Uml2SemanticModelBridge (); 
			semanticModelBridge.Element = stereotypedEdgeModel;
			stereotypedEdgeGE.SemanticModel = semanticModelBridge;
			// adds anchors and anchorages
			DI.GraphConnector cnn;

			cnn = new DI.GraphConnector ();
			cnn.GraphElement = fromGE;
			fromGE.Position.CopyTo (cnn.Position);
			fromGE.Anchorage.Add (cnn);
			stereotypedEdgeGE.Anchor.Add (cnn);
			cnn.GraphEdge.Add(stereotypedEdgeGE);

			cnn = new DI.GraphConnector ();
			cnn.GraphElement = toGE;
			toGE.Position.CopyTo (cnn.Position);
			toGE.Anchorage.Add (cnn);
			stereotypedEdgeGE.Anchor.Add (cnn);
			cnn.GraphEdge.Add(stereotypedEdgeGE);
			// adds waypoints
			DI.GraphNode gn = fromGE as DI.GraphNode;
			DI.Point f = (gn != null ? gn.Center : fromGE.Position.Clone ());
			gn = toGE as DI.GraphNode;
			DI.Point t = (gn != null ? gn.Center : toGE.Position.Clone ());
			stereotypedEdgeGE.Waypoints.Add (f);
			stereotypedEdgeGE.Waypoints.Add (t);
			// adds the stereotype compartment
			DI.SimpleSemanticModelElement bridge;
			DI.GraphNode stCompartmentGN = new DI.GraphNode ();
			bridge = new DI.SimpleSemanticModelElement ();
			bridge.TypeInfo = "StereotypeCompartment";
			stCompartmentGN.SemanticModel = bridge;
			stereotypedEdgeGE.Contained.Add (stCompartmentGN);
			stCompartmentGN.Container = stereotypedEdgeGE;
			DI.Point.GetHalfWayPoint (fromGE.Position, toGE.Position).CopyTo (stCompartmentGN.Position);
			DI.GraphNode keywordMetaclassGN = new DI.GraphNode ();
			bridge = new DI.SimpleSemanticModelElement ();
			bridge.TypeInfo = "KeywordMetaclass";
			keywordMetaclassGN.SemanticModel = bridge;
			stCompartmentGN.Contained.Add (keywordMetaclassGN);
			keywordMetaclassGN.Container = stCompartmentGN;
			// adds the stereotypedEdge to the diagram
			owningDiagram.DIDiagram.Contained.Add (stereotypedEdgeGE);
			stereotypedEdgeGE.Container = owningDiagram.DIDiagram;
			UMLStereotypedEdge stereotypedEdge = new UMLStereotypedEdge (owningDiagram, stereotypedEdgeGE, stereotypedEdgeModel);
			owningDiagram.UMLCanvas.AddElement (stereotypedEdge);
			return stereotypedEdge;
		}

		// Looks for the contained graph node that represents the stereotype,
		// then creates a UMLEntry and shows it.
		// The name of the edge is represented as two graph nodes: one for the
		// point where the name is located (typeInfo DirectedName) and another one
		// (contained within the former) which represents the label itself.
		// As always, every position is relative to its owner's position.
		// Note that edges are always placed at (0, 0).
		private void SearchAndDrawStereotype (DI.GraphEdge graphEdge, UMLDiagram ownerDiagram)
		{
			// looks for the contained graph node that represents the edge name
			DI.GraphNode stereotypeCompartmentGN = null;
			int i = 0;
			DI.SimpleSemanticModelElement bridge;
			while (_stereotypeCompartmentGN == null && i < graphEdge.Contained.Count)
			{
				stereotypeCompartmentGN = graphEdge.Contained [i++] as DI.GraphNode;
				if (stereotypeCompartmentGN != null)
				{
					bridge = stereotypeCompartmentGN.SemanticModel as DI.SimpleSemanticModelElement;
					if (bridge != null && bridge.TypeInfo == "StereotypeCompartment")
					{
						_stereotypeCompartmentGN = stereotypeCompartmentGN;
					}
				}
			}
			// if the corresponding graph node was found, draw it
			if (_stereotypeCompartmentGN != null)
			{
				// first, find the corresponding Graph Node
				i = 0;
				DI.GraphNode keywordMetaclassGN = null;
				bridge = null;
				do
				{
					keywordMetaclassGN = _stereotypeCompartmentGN.Contained [i++] as DI.GraphNode;
					if (keywordMetaclassGN != null)
					{
						bridge = keywordMetaclassGN.SemanticModel as DI.SimpleSemanticModelElement;
					}
				} while (bridge==null || bridge.TypeInfo != "KeywordMetaclass");
				// now we're able to draw it
				string text = "<<" + _modelElement.GetType().Name.Substring(6).ToLower() + ">>";
				_keywordMetaclassUMLEntry = new UMLEntry (this, keywordMetaclassGN, ownerDiagram.CanvasRoot, true, text);
				_keywordMetaclassUMLEntry.Editable = false;
				base.AddFreeEntry (_keywordMetaclassUMLEntry);
				_keywordMetaclassUMLEntry.Show ();
			}
		}

		private DI.GraphNode _stereotypeCompartmentGN;
		private UML.Relationship _modelElement;
		private UMLEntry _keywordMetaclassUMLEntry;
	}
}
