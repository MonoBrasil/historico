/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2004  Mario Carrión <mario.carrion@gmail.com>
Copyright (C) 2004  Rodolfo Campero <rodolfo.campero@gmail.com>

UMLActor.cs: the UML2 Actor

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
	public class UMLActor : UMLNode
	{
		public UMLActor (UMLDiagram ownerDiagram, DI.GraphNode actorGraphNode): base (ownerDiagram, actorGraphNode)
		{
			// old ctor
			_head = new CanvasEllipse (ChildrensRoot);
			_head.OutlineColor = DEFAULT_OUTLINE_COLOR;
			_head.FillColor = DEFAULT_FILL_COLOR;
			//
			_arms = new CanvasLine (ChildrensRoot);
			_arms.WidthUnits = 1.0;
			_arms.FillColor = DEFAULT_OUTLINE_COLOR;
			//
			_body = new CanvasLine (ChildrensRoot);
			_body.WidthUnits = 1.0;
			_body.FillColor = DEFAULT_OUTLINE_COLOR;
			//
			_legs = new CanvasLine (ChildrensRoot);
			_legs.WidthUnits= 1.0;
			_legs.FillColor = DEFAULT_OUTLINE_COLOR;
			// /old ctor
			_modelElement = (UML.Actor) MonoUML.Widgets.Helper.GetSemanticElement (actorGraphNode);
			DI.GraphElement nestedDiagElem;
			DI.SimpleSemanticModelElement nestedBridge;
			//
			DI.DiagramElement elem = (DI.DiagramElement) actorGraphNode.Contained[0];
			nestedDiagElem = elem as DI.GraphElement;
			if (nestedDiagElem != null)
			{
				nestedBridge = nestedDiagElem.SemanticModel as DI.SimpleSemanticModelElement;
				if (nestedBridge.TypeInfo == "Name")
				{
					string fontModifier = (_modelElement.IsAbstract ? "italic" : "");
					_nameUMLEntry = new UMLEntry (this, (DI.GraphNode)nestedDiagElem, ownerDiagram.CanvasRoot, true, _modelElement.Name, fontModifier);
					base.AddFreeEntry (_nameUMLEntry);
					_nameUMLEntry.Show ();
					_nameDIGraphNode = nestedDiagElem;
				} 
			}
			else
			{
				//TODO: Create both, entry and DI nested element
			} 
			_nameUMLEntry.TextChanged += CallHub;
			Redraw ();
		}
		
		public event UMLElementNameChangedHandler NameChanged = null;

		protected override void AddContextMenuOptions (System.Collections.IList options)
		{
			options.Add (new CreateAssociationAction (_ownerDiagram));
			base.AddContextMenuOptions (options);
		}

		// Applies the changes made to the corresponding model element
		public override void ApplyModelChanges ()
		{
			_nameUMLEntry.FontModifier = (_modelElement.IsAbstract ? "italic" : ""); 
			_nameUMLEntry.Text = _modelElement.Name; 
		}

		//Broadcast the element.
		private void CallHub (object obj, string new_nameUMLEntry)
		{
			if (NameChanged != null)
			{
				System.Console.WriteLine ("UMLACTOR. Name changed: "+new_nameUMLEntry);
				_modelElement.Name = new_nameUMLEntry;
				NameChanged (GraphNode, new_nameUMLEntry);
			}
		}

		public static UMLActor CreateNew (
			UMLDiagram ownerDiagram,
			UML.Actor actorModel )
		{
			// creates the graphical representation of the new Actor
			DI.GraphNode actorGN = new DI.GraphNode ();
			//    graphical properties
			actorGN.Size.Height = DEFAULT_HEIGHT;
			actorGN.Size.Width = DEFAULT_WIDTH;
			//    model bridge to the UML model element (Actor)
			Uml2SemanticModelBridge bridge = new Uml2SemanticModelBridge (); 
			bridge.Element = actorModel;
			actorGN.SemanticModel = bridge;
			// graphical representation of the property "Name"
			DI.GraphNode nameGN = new DI.GraphNode ();
			//    graphical properties
			nameGN.Position.Y = actorGN.Size.Height + 10D;
			nameGN.Position.X = 10D;
			nameGN.Property[DI.StandardProperty.FontFamily] = "Verdana";
			nameGN.Property[DI.StandardProperty.FontSize] = "10";
			//    model bridge to the property
			DI.SimpleSemanticModelElement nameBridge = new DI.SimpleSemanticModelElement ();
			nameBridge.TypeInfo = "Name";
			nameGN.SemanticModel = nameBridge;
			// adds the name GN to the Actor GN
			actorGN.Contained.Add (nameGN);
			nameGN.Container = actorGN;
			// adds the actor to the diagram
			ownerDiagram.DIDiagram.Contained.Add (actorGN);
			actorGN.Container = ownerDiagram.DIDiagram;
			return new UMLActor (ownerDiagram, actorGN);
		}

		protected override void Redraw ()
		{
			base.Redraw ();
			if (_head != null)
			{
				double head_width = Width * 0.34D; //34% of the Width
				double head_height = Height * 0.25D; //25% of the Height
				_head.X1 = Width * 0.67D;
				_head.Y1 = 0;
				_head.X2 = head_width;
				_head.Y2 = head_height;
			}
			double arms_y = Height * 0.30D; // Height - ((70*Height)/100)
			if (_arms != null)
			{
				_arms.Points = new CanvasPoints (new double[]{ 5,arms_y,  Width-5,arms_y});
			}
			double body_x = Width / 2;
			double body_height = Height * 0.65D; // Height - ((35*Height)/100)
			if (_body != null)
			{
				_body.Points = new CanvasPoints (new double[]{ body_x, arms_y,  body_x, body_height});
			}
			if (_legs != null)
			{
				double legs_y = arms_y + Height * 0.70D; // Height - ((30*Height)/100)
				double legs_x = Width * 0.80D; // Width - ((20*Width)/100)
				_legs.Points = new CanvasPoints (
					new double[] { legs_x, legs_y,  body_x, body_height, Width-legs_x, legs_y}
				);
			}
			if (_nameUMLEntry != null)
			{
				_nameUMLEntry.RaiseToTop ();
			}
			//Updating DI Values
			//TODO: This shouldn't be here... every property should do its thing.
			base.SetDIProperties ();
		}
		
		private CanvasEllipse _head;
		private CanvasLine _arms, _body, _legs;
		private UMLEntry _nameUMLEntry;
		private UML.Actor _modelElement;
		private DI.GraphElement _nameDIGraphNode;

		private const double DEFAULT_HEIGHT = 85D;
		private const double DEFAULT_WIDTH = 60D;
	}
}
