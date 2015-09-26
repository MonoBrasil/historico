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
	public class UMLGeneralization : UMLEdge
	{
		private UMLGeneralization
		(
			UMLDiagram ownerDiagram,
			DI.GraphEdge generalizationGraphEdge,
			UML.Generalization generalizationModel
		)	: base (ownerDiagram, generalizationGraphEdge, false)
		{
			_modelElement = generalizationModel;
			SetEdgeEnds ();
		}

		public UMLGeneralization (UMLDiagram ownerDiagram, DI.GraphEdge generalizationGraphEdge)
			: this (ownerDiagram, generalizationGraphEdge, (UML.Generalization) MonoUML.Widgets.Helper.GetSemanticElement (generalizationGraphEdge))
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

		public static UMLGeneralization CreateNew (
			UMLDiagram ownerDiagram,
			UMLElement fromElement,
			UMLElement toElement )
		{
			UMLGeneralization generalization = null;
			DI.GraphElement fromGE = fromElement.GraphElement;
			DI.GraphElement toGE = toElement.GraphElement;
			UML.Classifier fromModelElement = MonoUML.Widgets.Helper.GetSemanticElement (fromGE) as UML.Classifier; 
			UML.Classifier toModelElement = MonoUML.Widgets.Helper.GetSemanticElement (toGE) as UML.Classifier; 
			if (fromModelElement != null && toModelElement != null)
			{
				// creates the new Generalization in the model
				UML.Generalization generalizModel = UML.Create.Generalization ();
				generalizModel.General = toModelElement;
				generalizModel.Specific = fromModelElement;
				fromModelElement.Generalization.Add(generalizModel);
				// creates the graphical representation of the new Generalization
				DI.GraphEdge generalizGE = new DI.GraphEdge ();
				//    model bridge to the UML model element (Actor)
				Uml2SemanticModelBridge bridge = new Uml2SemanticModelBridge (); 
				bridge.Element = generalizModel;
				generalizGE.SemanticModel = bridge;
				// adds anchors and anchorages
				DI.GraphConnector cnn;
				cnn = new DI.GraphConnector ();
				cnn.GraphElement = fromGE;
				fromGE.Position.CopyTo (cnn.Position);
				fromGE.Anchorage.Add (cnn);
				generalizGE.Anchor.Add (cnn);
				cnn = new DI.GraphConnector ();
				cnn.GraphElement = toGE;
				toGE.Position.CopyTo (cnn.Position);
				toGE.Anchorage.Add (cnn);
				generalizGE.Anchor.Add (cnn);
				// adds waypoints
				DI.GraphNode gn = fromGE as DI.GraphNode;
				DI.Point f = (gn != null ? gn.Center : toGE.Position.Clone ());
				gn = toGE as DI.GraphNode;
				DI.Point t = (gn != null ? gn.Center : toGE.Position.Clone ());
				generalizGE.Waypoints.Add (f);
				generalizGE.Waypoints.Add (t);
				// adds the generalization to the diagram
				ownerDiagram.DIDiagram.Contained.Add (generalizGE);
				generalization = new UMLGeneralization (ownerDiagram, generalizGE, generalizModel);
				ownerDiagram.UMLCanvas.AddElement (generalization);
				Hub.Instance.Broadcaster.BroadcastElementChange(fromModelElement);
			}
			return generalization;
		}

		protected override bool ReplaceFromModelElement ()
		{
			UML.Classifier hoverClassifier = Helper.GetHoverClassifier (_ownerDiagram, this);
			if (hoverClassifier != null)
			{
				UML.Classifier oldOwner = _modelElement.Specific; 
				oldOwner.Generalization.Remove(_modelElement);
				_modelElement.Specific = hoverClassifier;
				hoverClassifier.Generalization.Add(_modelElement);
				IBroadcaster b = Hub.Instance.Broadcaster; 
				b.BroadcastElementChange (oldOwner);
				b.BroadcastElementChange (hoverClassifier);
			}
			return hoverClassifier != null;
		}

		protected override bool ReplaceToModelElement ()
		{
			UML.Classifier hoverClassifier = Helper.GetHoverClassifier (_ownerDiagram, this);
			if (hoverClassifier != null)
			{
				_modelElement.General = hoverClassifier;
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

		private UML.Generalization _modelElement;
	}
}
