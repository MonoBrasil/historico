/*
MonoUML.Widgets.UML2 - A library for representing the UML2 elements
Copyright (C) 2004  Mario Carrión <mario.carrion@gmail.com>
Copyright (C) 2004  Rodolfo Campero <rodolfo.campero@gmail.com>

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
	public abstract class UMLDiagram
	{
		// Constructor with default font
		private UMLDiagram (DI.Diagram diagram, string font, string size) : this (diagram)
		{
			DefaultFontName = font;
			DefaultFontSize = size;
		}
		
		protected UMLDiagram (DI.Diagram diagram, Widgets.NoteBook notebook) : this (diagram)
		{
			_canvas.NoteBook = notebook;
		}

		// Constructor
		private UMLDiagram (DI.Diagram diagram)
		{
			_diagram = diagram;
			_canvas = new UMLCanvas (this);
			_canvas.ElementNameChanged += ChangeElementNameHub;
			Refresh ();
		}
		
		//Returns the current diagram's UMLDiagramType
		public static UMLDiagram CreateFor (DI.Diagram diagram, Widgets.NoteBook notebook)
		{
			string diagramType = ((DI.SimpleSemanticModelElement)diagram.SemanticModel).TypeInfo.ToLower ();
			switch(diagramType)
			{
				case "classdiagram": return new UMLClassDiagram(diagram, notebook);
				case "objectdiagram": return new UMLObjectDiagram(diagram, notebook);
				case "usecasediagram": return new UMLUseCaseDiagram(diagram, notebook);
				default: throw new ApplicationException("Unexpected diagram type.");
			}
		}

		internal CanvasGroup CanvasRoot
		{
			get { return _canvas.CanvasRoot; }
		}

		public string DefaultFontName
		{
			get { return _default_font_name; }
			set { DefaultFontName = _default_font_name; }
		}

		public string DefaultFontSize
		{
			get { return _default_font_size; }
			set { DefaultFontSize = _default_font_size; }
		}

		public DI.Diagram DIDiagram
		{
			get { return _diagram; }
		}

		public UMLCanvas UMLCanvas
		{
			get { return _canvas; }
			set { _canvas = value; }
		}

		internal void AddMapping (DI.GraphElement diElement, UMLElement umlcanvasElement)
		{
			_di2umlcanvas.Add (diElement, umlcanvasElement);
		}

		internal void AddNewClassifier (UMLElement element, UML.Classifier classifier)
		{
			this.UMLCanvas.AddElement (element);
			UML.Element ns = ((MonoUML.DI.Uml2Bridge.Uml2SemanticModelBridge)this.DIDiagram.Namespace).Element;
			classifier.Owner = ns;
			UML.Package pkg = ns as UML.Package;
			if (pkg != null)
			{
				pkg.OwnedType.Add (classifier);
				Hub.Instance.Broadcaster.BroadcastElementChange(pkg);
			}
			else
			{
					Hub.Instance.AddNewUMLElement (classifier);
			}
		}

		internal void AddNewElement (UMLElement graphicElement, UML.Element modelElement)
		{
			this.UMLCanvas.AddElement (graphicElement);
			UML.Element ns = ((MonoUML.DI.Uml2Bridge.Uml2SemanticModelBridge)this.DIDiagram.Namespace).Element;
			modelElement.Owner = ns;
			UML.Package pkg = ns as UML.Package;
			if (pkg != null)
			{
				pkg.OwnedElement.Add (modelElement);
				Hub.Instance.Broadcaster.BroadcastElementChange(pkg);
			}
			else
			{
				Hub.Instance.AddNewUMLElement (modelElement);
			}
		}
		
		//Returns the current diagram's UMLDiagramType
		public static UMLDiagramType GetDiagramType (DI.Diagram diagram)
		{
			string diagramType = ((DI.SimpleSemanticModelElement)diagram.SemanticModel).TypeInfo;
			
			if (diagramType.ToLower ().Equals ("usecasediagram"))
			{
				return UML2.UMLDiagramType.UseCase;
			}
			//TODO. Add the other diagrams
			else 
			{
				return UML2.UMLDiagramType.Unknown;
			}
		}
		
		// Draws the given node.
		// Derived classes are supposed to try to draw the given node,
		// and if they fail, call this default implementation.
		protected virtual void DrawElement (DI.GraphNode node)
		{
			Uml2SemanticModelBridge bridge = node.SemanticModel as Uml2SemanticModelBridge;
			UML.Element element = bridge.Element;
			// if the element is any kind of classifier, represent it as an UMLClassifier.
			UML.Classifier classifier = element as UML.Classifier;
			if (classifier != null)
			{
				System.Console.WriteLine ("UMLDiagram.DrawElement> Adding a generic UMLClassifier to the canvas for element: " + element.ToString());
				UMLClassifier umlClassifier = new UMLClassifier (this, node);
				_canvas.AddElement (umlClassifier);
			}
		}

		// Draws the given edge.
		// Derived classes are supposed to try to draw the given edge,
		// and if they fail, call this default implementation.
		protected virtual void DrawElement (DI.GraphEdge edge)
		{
			Uml2SemanticModelBridge bridge = edge.SemanticModel as Uml2SemanticModelBridge;
			UML.Element umlElement = bridge.Element;
			// if the element is any kind of binary relationship, represent
			// it as an edge.
			UML.Relationship umlRelation = umlElement as UML.Relationship;
			if (umlRelation != null && umlRelation.RelatedElement.Count == 2)
			{
				System.Console.WriteLine ("UMLDiagram.DrawElement> Adding a generic UMLEdge to the canvas for element: " + umlElement.ToString());
				UMLStereotypedEdge umlEdge = new UMLStereotypedEdge (this, edge);
				_canvas.AddElement (umlEdge);
			}   
		}

		// Draws all the elements contained in the canvas
		private void DrawElements()
		{
			DI.GraphNode node;
			// we must draw first the nodes, and then the edges, because
			// when an edge is drawn, it asks for the umlcanvas# elements
			// corresponding to its related elements
			ArrayList edges = new ArrayList ();
			foreach(DI.DiagramElement elem in DIDiagram.Contained)
			{
				node = elem as DI.GraphNode;
				if(node != null)
				{
					if(node.SemanticModel is Uml2SemanticModelBridge)
					{
						DrawElement(node);
					} 
				} else if (elem is DI.GraphEdge)
				{
					edges.Add (elem);
				}
			}
			foreach(DI.GraphEdge edge in edges)
			{
				if(edge.SemanticModel is Uml2SemanticModelBridge)
				{
					DrawElement(edge);
				}
			}
		}
		
		//Notifies to the NoteBook that some element changed its name
		private void ChangeElementNameHub (object obj, string new_name)
		{
			_canvas.NoteBook.BroadcastElementChange (obj);
		}
		
		public void EnableControlPointMotion (UMLEdge edge, UMLControlPoint cp)
		{
			_canvas.EnableControlPointMotion (edge, cp);
		}
		
		public IList GetContextMenuOptions ()
		{
			ArrayList options = new ArrayList ();
			GetDerivedContextMenuOptions (options);
			return options; 
		}
		
		protected abstract void GetDerivedContextMenuOptions (ArrayList options);

		internal UMLElement GetUmlcanvasElement (DI.GraphElement diElement)
		{
			return (UMLElement)_di2umlcanvas [diElement];
		}
		
		public void Refresh ()
		{
			System.Console.WriteLine ("UMLDiagram.Refresh> refreshing diagram with hashcode: " + this.GetHashCode().ToString());
			_di2umlcanvas = new Hashtable ();
			_canvas.Clear ();
			_canvas.Zoom = DIDiagram.Zoom;
			_canvas.HeightRequest = (int) DIDiagram.Size.Height;
			_canvas.WidthRequest = (int) DIDiagram.Size.Width;
			DrawElements ();
		}
		
		// Updates the view of the modified model element.
		internal void UpdateElement (object modified)
		{
			foreach (DI.GraphElement diElem in _diagram.Contained)
			{
				UMLElement umlElem = GetUmlcanvasElement (diElem);
				if (diElem == modified || MonoUML.Widgets.Helper.GetSemanticElement(diElem) == modified)
				{
					umlElem.ApplyModelChanges ();
					// the loop must keep running because there may be
					// more than one representation of the same model element.
				}
				else
				{
					// check in the nested elements
					umlElem.UpdateElement (modified);
				}
			}
		}

		protected UMLCanvas _canvas;
		private string _default_font_size = "10";
		private string _default_font_name = "Courier";
		private DI.Diagram _diagram;
		// this hashtable maps DI elements to umlcanvas# elements.
		private Hashtable _di2umlcanvas;
	}
	
}
