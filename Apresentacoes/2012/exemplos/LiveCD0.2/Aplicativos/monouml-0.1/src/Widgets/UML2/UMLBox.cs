/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2005  Rodolfo Campero <rodolfo.campero@gmail.com>

UMLBox.cs: the UML2 representation of Boxs

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
	public abstract class UMLBox : UMLNode
	{
		public UMLBox (UMLDiagram ownerDiagram, DI.GraphNode graphNode): base (ownerDiagram, graphNode)
		{
			_outerBox = new CanvasRect (ChildrensRoot);
			_outerBox.X1 = _outerBox.Y1 = 0; 
			_outerBox.X2 = Width;
			_outerBox.Y2 = Height;
			_outerBox.FillColor = DEFAULT_FILL_COLOR;
			_outerBox.OutlineColor = DEFAULT_OUTLINE_COLOR;
			Width = graphNode.Size.Width;
			Height = graphNode.Size.Height;
			// gets the corresponding model element
			Uml2SemanticModelBridge bridge = (Uml2SemanticModelBridge) graphNode.SemanticModel;
			_modelElement = (UML.NamedElement) bridge.Element;
			DI.GraphElement nestedDiagElem;
			DI.SimpleSemanticModelElement nestedBridge;
			// looks for the Name compartment
			foreach (DI.DiagramElement elem in GraphNode.Contained)
			{
				nestedDiagElem = elem as DI.GraphElement;
				if (nestedDiagElem != null)
				{
					nestedBridge = nestedDiagElem.SemanticModel as DI.SimpleSemanticModelElement;
					if (nestedBridge.TypeInfo == "NameCompartment")
					{
						// widget for the property Name 
						_name = new UMLEntry (this, (DI.GraphNode)nestedDiagElem, ChildrensRoot, false, _modelElement.Name);
						_name.Show();
						break;
					}
				} 
			}
			Redraw ();
		}

		protected static DI.GraphNode CreateNewGraphNode (
			UMLDiagram ownerDiagram,
			UML.Element modelElement)
		{
			// creates the graphical representation
			DI.GraphNode modelElementGN = new DI.GraphNode ();
			//    model bridge to the UML model element
			Uml2SemanticModelBridge bridge = new Uml2SemanticModelBridge (); 
			bridge.Element = modelElement;
			modelElementGN.SemanticModel = bridge;
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
			// adds the name GN to the model element GN
			modelElementGN.Contained.Add (nameGN);
			nameGN.Container = modelElementGN;
			// adds the modelElement to the diagram
			ownerDiagram.DIDiagram.Contained.Add (modelElementGN);
			return modelElementGN;
		}

		protected override void Redraw ()
		{
			base.Redraw ();
			// checks if the element is ready
			// -- if we don't do this, a NullReferenceException is produced
			if (_name != null)
			{
				double minH = _name.TextHeight + 16D;
				double minW = _name.TextWidth + 12D;
				if (minW > Width) { Width = minW; }
				_name.X = (Width - _name.TextWidth) / 2D;
				if (minH > Height) { Height = minH; }
				_name.RaiseToTop ();
				_outerBox.X2 = Width;
				_outerBox.Y2 = Height;
				base.SetDIProperties ();
			}
		}
		
		private CanvasRect _outerBox;
		protected UMLEntry _name;
		protected UML.NamedElement _modelElement;
	}	
}