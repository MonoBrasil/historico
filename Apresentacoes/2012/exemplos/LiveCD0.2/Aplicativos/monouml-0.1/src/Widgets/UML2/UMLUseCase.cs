/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2004  Mario Carrión <mario.carrion@gmail.com>

UMLUseCase.cs: the UML2 Use case

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

	public class UMLUseCase : UMLNode
	{
	
		public UMLUseCase (UMLDiagram ownerDiagram, DI.GraphNode graphNode): base (ownerDiagram, graphNode)
		{
			_circle = new CanvasEllipse (ChildrensRoot);
			_circle.X1 = _circle.Y1 = 0; 
			_circle.X2 = Width;
			_circle.Y2 = Height;
			_circle.FillColor = DEFAULT_FILL_COLOR;
			_circle.OutlineColor = DEFAULT_OUTLINE_COLOR;
			//
			Width = GraphNode.Size.Width;
			Height = GraphNode.Size.Height;
			//
			Uml2SemanticModelBridge bridge = (Uml2SemanticModelBridge) GraphNode.SemanticModel;
			_modelElement = (UML.UseCase) bridge.Element;
			DI.GraphElement nestedDiagElem;
			DI.SimpleSemanticModelElement nestedBridge;
			//
			foreach (DI.DiagramElement elem in GraphNode.Contained)
			{
				nestedDiagElem = elem as DI.GraphElement;
				if (nestedDiagElem != null)
				{
					nestedBridge = nestedDiagElem.SemanticModel as DI.SimpleSemanticModelElement;
					System.Console.WriteLine ("UMLUseCase nestedBridge.TypeInfo "+nestedBridge.TypeInfo);
					if (nestedBridge.TypeInfo == "NameCompartment")
					{
						// widget for the property Name 
						string fontModifier = (_modelElement.IsAbstract ? "italic" : "");
						_name = new UMLEntry (this, (DI.GraphNode)nestedDiagElem, ChildrensRoot, false, _modelElement.Name, fontModifier);
						_name.Show();
						break;
					}
				} 
			}
			_circle.X1 = _circle.Y1 = 0;
			_circle.X2 = Width;
			_circle.Y2 = Height;
			Redraw ();
		}

		protected override void AddContextMenuOptions (System.Collections.IList options)
		{
			options.Add (new CreateAssociationAction (_ownerDiagram));
			options.Add (new CreateIncludeAction (_ownerDiagram));
			options.Add (new CreateExtendAction (_ownerDiagram));
			base.AddContextMenuOptions (options);
		}

		// Applies the changes made to the corresponding model element
		public override void ApplyModelChanges ()
		{
			_name.FontModifier = (_modelElement.IsAbstract ? "italic" : ""); 
			_name.Text = _modelElement.Name; 
		}

		public static UMLUseCase CreateNew (
			UMLDiagram ownerDiagram,
			UML.UseCase useCaseModel )
		{
			// creates the graphical representation of the new UseCase
			DI.GraphNode useCaseGN = new DI.GraphNode ();
			//    model bridge to the UML model element (UseCase)
			Uml2SemanticModelBridge bridge = new Uml2SemanticModelBridge (); 
			bridge.Element = useCaseModel;
			useCaseGN.SemanticModel = bridge;
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
			useCaseGN.Contained.Add (nameGN);
			nameGN.Container = useCaseGN;
			// adds the useCase to the diagram
			ownerDiagram.DIDiagram.Contained.Add (useCaseGN);
			useCaseGN.Container = ownerDiagram.DIDiagram; 
			return new UMLUseCase (ownerDiagram, useCaseGN);
		}

		protected override void Redraw ()
		{
			base.Redraw ();
			// checks if the element is ready
			// -- if we don't do this, a NullReferenceException is produced
			if (_name != null)
			{
				double minH = _name.TextHeight + 16D;
				double minW = _name.TextWidth + 6D;
				if (minW < Width)
				{
					_name.X = (Width - _name.TextWidth) / 2D;
				}
				else
				{
					Width = minW; 
				}
				if (minH < Height)
				{
					_name.Y = (Height - _name.TextHeight) / 2D;
				}
				else
				{
					Height = minH; 
				}
				_name.RaiseToTop ();
				_circle.X2 = Width;
				_circle.Y2 = Height;
				base.SetDIProperties ();
			}
		}
		
		private CanvasEllipse _circle;
		private UMLEntry _name;
		private UML.UseCase _modelElement;
	}	
}