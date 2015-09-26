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
using Gnome;
using Gdk;
using DI = MonoUML.DI;
using UML = ExpertCoder.Uml2;
using MonoUML.DI.Uml2Bridge;

namespace MonoUML.Widgets.UML2
{
	public class UMLInterfaceRealization : UMLEdge
	{
		private UMLInterfaceRealization
		(
			UMLDiagram ownerDiagram,
			DI.GraphEdge interfaceRealizationGraphEdge,
			UML.InterfaceRealization interfaceRealizationModel
		)	: base (ownerDiagram, interfaceRealizationGraphEdge, false)
		{
			_modelElement = interfaceRealizationModel;
			SetEdgeEnds ();
		}

		public UMLInterfaceRealization (UMLDiagram ownerDiagram, DI.GraphEdge interfaceRealizationGraphEdge)
			: this (ownerDiagram, interfaceRealizationGraphEdge, (UML.InterfaceRealization) MonoUML.Widgets.Helper.GetSemanticElement (interfaceRealizationGraphEdge))
		{
			// create the graphic elements that compose this element
			// create the UML canvas elements that compose this UML Element
			Redraw ();
			// create nested stuff
		}

		// Applies the changes made to the corresponding model element
		public override void ApplyModelChanges ()
		{
			base.ApplyModelChanges ();
		}

		public static UMLInterfaceRealization CreateNew (
			UMLDiagram ownerDiagram,
			UMLElement fromElement,
			UMLElement toElement )
		{
			UMLInterfaceRealization interfaceRealization = null;
			DI.GraphElement fromGE = fromElement.GraphElement;
			DI.GraphElement toGE = toElement.GraphElement;
			UML.BehavioredClassifier fromModelElement = MonoUML.Widgets.Helper.GetSemanticElement (fromGE) as UML.BehavioredClassifier; 
			UML.Interface toModelElement = MonoUML.Widgets.Helper.GetSemanticElement (toGE) as UML.Interface;
			if (fromModelElement != null && toModelElement != null)
			{
				// creates the new InterfaceRealization in the model
				UML.InterfaceRealization interfaceRealizationModel = UML.Create.InterfaceRealization ();
				interfaceRealizationModel.Contract = toModelElement;
				interfaceRealizationModel.ImplementingClassifier = fromModelElement;
				fromModelElement.InterfaceRealization.Add(interfaceRealizationModel);
				// creates the graphical representation of the new InterfaceRealization
				DI.GraphEdge interfaceRealizationGE = new DI.GraphEdge ();
				//    model bridge to the UML model element (Actor)
				Uml2SemanticModelBridge bridge = new Uml2SemanticModelBridge (); 
				bridge.Element = interfaceRealizationModel;
				interfaceRealizationGE.SemanticModel = bridge;
				// adds anchors and anchorages
				DI.GraphConnector cnn;
				cnn = new DI.GraphConnector ();
				cnn.GraphElement = fromGE;
				fromGE.Position.CopyTo (cnn.Position);
				fromGE.Anchorage.Add (cnn);
				interfaceRealizationGE.Anchor.Add (cnn);
				cnn = new DI.GraphConnector ();
				cnn.GraphElement = toGE;
				toGE.Position.CopyTo (cnn.Position);
				toGE.Anchorage.Add (cnn);
				interfaceRealizationGE.Anchor.Add (cnn);
				// adds waypoints
				DI.GraphNode gn = fromGE as DI.GraphNode;
				DI.Point f = (gn != null ? gn.Center : toGE.Position.Clone ());
				gn = toGE as DI.GraphNode;
				DI.Point t = (gn != null ? gn.Center : toGE.Position.Clone ());
				interfaceRealizationGE.Waypoints.Add (f);
				interfaceRealizationGE.Waypoints.Add (t);
				// adds the interfaceRealization to the diagram
				ownerDiagram.DIDiagram.Contained.Add (interfaceRealizationGE);
				interfaceRealization = new UMLInterfaceRealization (ownerDiagram, interfaceRealizationGE, interfaceRealizationModel);
				ownerDiagram.UMLCanvas.AddElement (interfaceRealization);
				Hub.Instance.Broadcaster.BroadcastElementChange(fromModelElement);
			}
			return interfaceRealization;
		}

		protected override bool ReplaceFromModelElement ()
		{
			UML.BehavioredClassifier hoverClassifier 
				= Helper.GetHoverClassifier (_ownerDiagram, this) as UML.BehavioredClassifier;
			if (hoverClassifier != null)
			{
				UML.BehavioredClassifier oldOwner = _modelElement.ImplementingClassifier; 
				oldOwner.InterfaceRealization.Remove(_modelElement);
				_modelElement.ImplementingClassifier = hoverClassifier;
				hoverClassifier.InterfaceRealization.Add(_modelElement);
				IBroadcaster b = Hub.Instance.Broadcaster; 
				b.BroadcastElementChange (oldOwner);
				b.BroadcastElementChange (hoverClassifier);
			}
			return hoverClassifier != null;
		}

		protected override bool ReplaceToModelElement ()
		{
			UML.Interface hoverClassifier
				= Helper.GetHoverClassifier (_ownerDiagram, this) as UML.Interface;
			if (hoverClassifier != null)
			{
				_modelElement.Contract = hoverClassifier;
				Hub.Instance.Broadcaster.BroadcastElementChange (_modelElement);
			}
			return hoverClassifier != null;
		}

		private void SetEdgeEnds ()
		{
			base.ClearEnds ();
			base.AddToEnd (new UMLTriangularEnd (ChildrensRoot, FillKind.Hollow));
			base.ForceRedraw ();
		}

		private UML.InterfaceRealization _modelElement;
	}
}
