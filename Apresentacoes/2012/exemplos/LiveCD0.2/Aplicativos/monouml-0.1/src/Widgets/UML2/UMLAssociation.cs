/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2005  Rodolfo Campero <rodolfo.campero@gmail.com>

UMLAssociation.cs: the UML2 Association

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
	public class UMLAssociation : UMLNamedEdge
	{
#region "UMLAssociationEnd nested class"
		// Represents and association end, and encapsulates the visualization
		// of the corresponding property's name, multiplicity and visibility. 
		class UMLAssociationEnd : UMLElement
		{
			public UMLAssociationEnd (UMLDiagram ownerDiagram, DI.GraphNode graphNode)
				: base (ownerDiagram, graphNode)
			{
				_circle = new CanvasEllipse (ownerDiagram.CanvasRoot);
				_circle.OutlineColorGdk = UMLEdge.HIGHLIGHTED_OUTLINE_COLOR;
				_graphNode = graphNode;
				_modelElement = (UML.Property) MonoUML.Widgets.Helper.GetSemanticElement (graphNode);
				DI.SimpleSemanticModelElement bridge;
				foreach (DI.GraphNode gn in graphNode.Contained)
				{
					bridge = gn.SemanticModel as DI.SimpleSemanticModelElement;
					switch (bridge.TypeInfo)
					{
						case "Name":
							_name = new UMLEntry (this, gn, ownerDiagram.CanvasRoot, true, _modelElement.Name);
							_name.TextChanged += new UMLElementNameChangedHandler (BroadcastNameChange);
							base.AddFreeEntry (_name);
							_name.Show ();
							break;
						case "Visibility":
							break;
						case "Multiplicity":
							_multiplicity = new UMLEntry (this, gn, ownerDiagram.CanvasRoot, true, GetMultiplicity ());
							_multiplicity.TextChanged += new UMLElementNameChangedHandler (ApplyMultiplicityChange);
							base.AddFreeEntry (_multiplicity);
							_multiplicity.Show ();
							break;
					}
				}
			}

			// Applies the changes made to the corresponding model element
			public override void ApplyModelChanges ()
			{
				_multiplicity.Text = GetMultiplicity ();
				_name.Text = _modelElement.Name;
			}
			
			private void ApplyMultiplicityChange (object obj, string newMultiplicity)
			{
				//TODO
				System.Console.WriteLine ("UMLAssociationEnd.ApplyMultiplicityChange> TODO: new multiplicity: " + newMultiplicity);
			}

			private void BroadcastNameChange (object obj, string newName)
			{
				if (_modelElement.Name != newName)
				{
					_modelElement.Name = newName;
					Hub.Instance.Broadcaster.BroadcastElementChange (obj);
				}
			}
			
			private static void AddNewCompartment (DI.GraphNode owner, string typeInfo)
			{
				DI.GraphNode compartment = new DI.GraphNode ();
				DI.SimpleSemanticModelElement bridge = new DI.SimpleSemanticModelElement ();
				bridge.TypeInfo = typeInfo;
				compartment.SemanticModel = bridge;
				owner.Contained.Add (compartment);
				compartment.Container = owner;
			}
			
			public static DI.GraphNode CreateNewGraphNode (UML.Property end)
			{
				DI.GraphNode endCompartment = new DI.GraphNode ();
				Uml2SemanticModelBridge modelBridge = new Uml2SemanticModelBridge ();
				modelBridge.Element = end;
				endCompartment.SemanticModel = modelBridge;
				// compartments
				AddNewCompartment (endCompartment, "Multiplicity");
				AddNewCompartment (endCompartment, "Name");
				AddNewCompartment (endCompartment, "Visibility");
				return endCompartment;
			}
			
			private string GetMultiplicity ()
			{
				string mult = "";
				if ( _modelElement.Lower != 1
					|| _modelElement.Upper == UML.UnlimitedNatural.Infinity
					|| _modelElement.Lower != (uint)_modelElement.Upper )
				{
					mult = _modelElement.Lower + ".." + _modelElement.Upper.ToString();
				}
				return mult;
			}
			
			public new void Move (double dx, double dy)
			{
				base.Move (dx, dy);
				_graphNode.Position.X += dx;
				_graphNode.Position.Y += dy;
			} 

			protected override void OnDeselected ()
			{
				_circle.Hide ();
			}
			
			protected override void OnSelected ()
			{
				const double r = 10D;
				_circle.X1 = _graphNode.Position.X - r;
				_circle.X2 = _graphNode.Position.X + r;
				_circle.Y1 = _graphNode.Position.Y - r;
				_circle.Y2 = _graphNode.Position.Y + r;
				_circle.Show ();
				Hub.Instance.Broadcaster.BroadcastElementSelection (GraphElement);
			}

			protected override void Redraw ()
			{ }
			
			// sets its position to the point passed as argument,
			// and the position of all the nested elements to (0,0)
			public void SetPosition (DI.Point pos)
			{
				pos.CopyTo (_graphNode.Position);
				_multiplicity.X = pos.X;
				_multiplicity.Y = pos.Y + 10;
				_name.X = pos.X;
				_name.Y = pos.Y - 20;
			}

			private CanvasEllipse _circle;
			private DI.GraphNode _graphNode;
			private UML.Property _modelElement;
			private UMLEntry _multiplicity;
			private UMLEntry _name;
		}
#endregion


		private UMLAssociation
		(
			UMLDiagram ownerDiagram,
			DI.GraphEdge associationGraphEdge,
			UML.Association associationModel
		)	: base (ownerDiagram, associationGraphEdge, associationModel, false)
		{
			_modelElement = associationModel;
			SetEdgeEnds ();
			SearchAndDrawEndTexts ();
		}

		public UMLAssociation (UMLDiagram ownerDiagram, DI.GraphEdge associationGraphEdge)
			: this (ownerDiagram, associationGraphEdge, (UML.Association) MonoUML.Widgets.Helper.GetSemanticElement (associationGraphEdge))
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

		// Changes the owner of the property.
		private void ChangeOwner (UML.Property attribute, UML.Classifier newOwner)
		{
			IBroadcaster b = Hub.Instance.Broadcaster;
			UML.Class cls;
			UML.DataType dt;
			UML.Interface iface;
			UML.Association assoc;
			// deletes the relationship with the old owner
			UML.Element oldOwner = attribute.Owner;
			if ((cls = oldOwner as UML.Class) != null)
			{
				cls.OwnedAttribute.Remove (attribute);
			}
			else if ((dt = oldOwner as UML.DataType) != null)
			{
				dt.OwnedAttribute.Remove (attribute);
			}
			else if ((iface = oldOwner as UML.Interface) != null)
			{
				iface.OwnedAttribute.Remove (attribute);
			}
			else if ((assoc = oldOwner as UML.Association) != null)
			{
				assoc.OwnedEnd.Remove (attribute);
			}
			b.BroadcastElementChange (oldOwner);
			// creates the relationship with the new owner
			if ((cls = newOwner as UML.Class) != null)
			{
				attribute.Class = cls;
				cls.OwnedAttribute.Add (attribute);
			}
			else if ((dt = newOwner as UML.DataType) != null)
			{
				attribute.Datatype = dt;
				dt.OwnedAttribute.Add (attribute);
			}
			else if ((iface = newOwner as UML.Interface) != null)
			{
				attribute.Interface = iface;
				iface.OwnedAttribute.Add (attribute);
			}
			else if ((assoc = newOwner as UML.Association) != null)
			{
				attribute.OwningAssociation = assoc;
				assoc.OwnedEnd.Add (attribute);
			}
			b.BroadcastElementChange (newOwner);
		}

		public static UMLAssociation CreateNew (
			CanvasGroup group,
			DI.Diagram ownerDiagram,
			UML.Association associationModel )
		{
			return null;
		}

		public static UMLAssociation CreateNew (
			UMLDiagram ownerDiagram,
			UMLElement fromElement,
			UMLElement toElement )
		{
			UMLAssociation association = null;
			DI.GraphElement fromGE = fromElement.GraphElement;
			DI.GraphElement toGE = toElement.GraphElement;
			UML.Classifier fromModelElement = MonoUML.Widgets.Helper.GetSemanticElement (fromGE) as UML.Classifier; 
			UML.Classifier toModelElement = MonoUML.Widgets.Helper.GetSemanticElement (toGE) as UML.Classifier; 
			if (fromModelElement != null && toModelElement != null)
			{
				// creates the new Association in the model
				UML.Association assocModel = UML.Create.Association ();
				UML.Property end0 = UML.Create.Property ();
				// the first end aims at the "TO" end
				end0.Type = toModelElement;
				assocModel.OwnedEnd.Add (end0);
				end0.OwningAssociation = assocModel;
				// the second (and last) end aims at the "FROM" end
				UML.Property end1 = UML.Create.Property ();
				end1.Type = fromModelElement;
				assocModel.OwnedEnd.Add (end1);
				end1.OwningAssociation = assocModel;
				// creates the graphical representation of the new Association
				DI.GraphEdge assocGE = new DI.GraphEdge ();
				//    graphical properties
				//    model bridge to the UML model element (Actor)
				Uml2SemanticModelBridge bridge = new Uml2SemanticModelBridge (); 
				bridge.Element = assocModel;
				assocGE.SemanticModel = bridge;
				// adds anchors and anchorages
				DI.GraphConnector cnn;
				cnn = new DI.GraphConnector ();
				cnn.GraphElement = fromGE;
				fromGE.Anchorage.Add (cnn);
				fromGE.Position.CopyTo (cnn.Position);
				assocGE.Anchor.Add (cnn);
				cnn.GraphEdge.Add (assocGE);
				cnn = new DI.GraphConnector ();
				cnn.GraphElement = toGE;
				toGE.Anchorage.Add (cnn);
				toGE.Position.CopyTo (cnn.Position);
				assocGE.Anchor.Add (cnn);
				cnn.GraphEdge.Add (assocGE);
				// adds waypoints
				DI.GraphNode gn = fromGE as DI.GraphNode;
				DI.Point f = (gn != null ? gn.Center : toGE.Position.Clone ());
				gn = toGE as DI.GraphNode;
				DI.Point t = (gn != null ? gn.Center : toGE.Position.Clone ());
				assocGE.Waypoints.Add (f);
				assocGE.Waypoints.Add (t);
				// adds the name compartment
				DI.SimpleSemanticModelElement simpleBridge;
				DI.GraphNode nameCompartmentGN = new DI.GraphNode ();
				simpleBridge = new DI.SimpleSemanticModelElement ();
				simpleBridge.TypeInfo = "DirectedName";
				nameCompartmentGN.SemanticModel = simpleBridge;
				assocGE.Contained.Add (nameCompartmentGN);
				nameCompartmentGN.Container = assocGE;
				DI.Point.GetHalfWayPoint (fromGE.Position, toGE.Position).CopyTo (nameCompartmentGN.Position);
				DI.GraphNode keywordMetaclassGN = new DI.GraphNode ();
				simpleBridge = new DI.SimpleSemanticModelElement ();
				simpleBridge.TypeInfo = "Name";
				keywordMetaclassGN.SemanticModel = simpleBridge;
				nameCompartmentGN.Contained.Add (keywordMetaclassGN);
				keywordMetaclassGN.Container = nameCompartmentGN;
				// adds the association ends compartments
				DI.GraphNode endCompartment0 = UMLAssociationEnd.CreateNewGraphNode (end0);
				assocGE.Contained.Add (endCompartment0);
				endCompartment0.Container = assocGE;
				DI.GraphNode endCompartment1 = UMLAssociationEnd.CreateNewGraphNode (end1);
				assocGE.Contained.Add (endCompartment1);
				endCompartment1.Container = assocGE;
				// adds the association to the diagram
				ownerDiagram.DIDiagram.Contained.Add (assocGE);
				assocGE.Container = ownerDiagram.DIDiagram;
				association = new UMLAssociation (ownerDiagram, assocGE, assocModel);
				association._assocEnd0.SetPosition ((DI.Point) association._graphEdge.Waypoints [0]);
				association._assocEnd1.SetPosition ((DI.Point) association._graphEdge.Waypoints [1]);
				ownerDiagram.AddNewClassifier (association, assocModel);
			}
			return association;
		}

		// gets the property that is linked at the indicated end.
		private UML.Property GetPropertyAtEnd (int index)
		{
			int currIndex = 0;
			UML.Property prop = null, semanticElement;
			foreach (DI.GraphElement contained in _graphEdge.Contained)
			{
				semanticElement = MonoUML.Widgets.Helper.GetSemanticElement (contained) as UML.Property;
				if (semanticElement != null)
				{
					if (currIndex++ == index)
					{
						prop = semanticElement;
						break;
					}
				}
			}
			return prop;
		}

		protected override void OnFromMoved (double dx, double dy)
		{
			_assocEnd0.Move (dx, dy);
		}

		protected override void OnToMoved (double dx, double dy)
		{
			_assocEnd1.Move (dx, dy);
		}

		protected override void Redraw ()
		{
			base.Redraw ();
		}

		private bool ReplaceModelElement (int index)
		{
			// properties are inverted: the property at the end 0
			// (if navigable) belongs to the element at the end 1.
			index = 1 - index;
			UML.Classifier hoverClassifier = Helper.GetHoverClassifier (_ownerDiagram, this);
			if (hoverClassifier != null)
			{
				// the UML property is the semantic model element corresponding
				// to the first contained graph node whose semantic element is
				// a property
				UML.Property attribute = GetPropertyAtEnd(index);
				// if the current property is owned by the association,
				// the only thing that changes is the Type.
				if (_modelElement.OwnedEnd.Contains (attribute))
				{
					attribute.Type = hoverClassifier;
					Hub.Instance.Broadcaster.BroadcastElementChange (attribute);
				}
				else
				{
					ChangeOwner (attribute, hoverClassifier);
				}
			}
			return hoverClassifier != null;
		}

		protected override bool ReplaceFromModelElement ()
		{
			return ReplaceModelElement (0);
		}

		protected override bool ReplaceToModelElement ()
		{
			return ReplaceModelElement (1);
		}

		// Looks for the contained graph nodes that represents the association ends,
		// then creates a UMLAssociationEnd for each one and shows it.
		private void SearchAndDrawEndTexts ()
		{
			// looks for the contained graph node that represents the edge name
			DI.GraphNode endGN;
			UMLAssociationEnd assocEnd;
			Uml2SemanticModelBridge bridge;
			bool found0 = false;
			for (int i = 0; i < _graphEdge.Contained.Count; i++)
			{
				endGN = _graphEdge.Contained [i] as DI.GraphNode;
				if (endGN != null)
				{
					bridge = endGN.SemanticModel as Uml2SemanticModelBridge;
					if (bridge != null && bridge.Element is UML.Property)
					{
						assocEnd = new UMLAssociationEnd (_ownerDiagram, endGN);
						assocEnd.Show ();
						if (!found0)
						{
							_assocEnd0 = assocEnd;
							found0 = true;
						}
						else
						{
							_assocEnd1 = assocEnd;
						}
					}
				}
			}
		}

		private void SetEdgeEnds ()
		{
			base.ClearEnds ();
			// the property at the end 0 aims at the FROM element
			UML.Property prop = GetPropertyAtEnd (0);
			if (prop.Aggregation == UML.AggregationKind.composite)
			{
				base.AddToEnd (new UMLDiamond (ChildrensRoot, FillKind.Filled));
			}
			else if (prop.Aggregation == UML.AggregationKind.shared)
			{
				base.AddToEnd (new UMLDiamond (ChildrensRoot, FillKind.Hollow));
			}
			if (prop.Owner != _modelElement)
			{
				base.AddFromEnd (new UMLOpenArrow (ChildrensRoot));
			}
			// the property at the end 1 aims at the TO element
			prop = GetPropertyAtEnd (1);
			if (prop.Aggregation == UML.AggregationKind.composite)
			{
				base.AddFromEnd (new UMLDiamond (ChildrensRoot, FillKind.Filled));
			}
			else if (prop.Aggregation == UML.AggregationKind.shared)
			{
				base.AddFromEnd (new UMLDiamond (ChildrensRoot, FillKind.Hollow));
			}
			if (prop.Owner != _modelElement)
			{
				base.AddToEnd (new UMLOpenArrow (ChildrensRoot));
			}
			base.ForceRedraw ();
		}

		public override void UpdateElement (object modified)
		{
			bool found = false;
			if (GetPropertyAtEnd (0) == modified)
			{
				_assocEnd0.ApplyModelChanges ();
				found = true;
			}
			if (GetPropertyAtEnd (1) == modified)
			{
				_assocEnd1.ApplyModelChanges ();
				found = true;
			}
			if (found) { SetEdgeEnds (); }
		}

		private UML.Association _modelElement;
		private UMLAssociationEnd _assocEnd0;
		private UMLAssociationEnd _assocEnd1;
	}
}
