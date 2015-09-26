/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2005  Rodolfo Campero <rodolfo.campero@gmail.com>

UMLNamedEdge.cs: the UML2 NamedEdge

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
	public class UMLNamedEdge : UMLEdge
	{
		public UMLNamedEdge (
			UMLDiagram ownerDiagram,
			DI.GraphEdge graphEdge,
			UML.NamedElement namedElementModel,
			bool forceRedraw) : base (ownerDiagram, graphEdge, forceRedraw)
		{
			_namedElementModel = namedElementModel;
			SearchAndDrawName (graphEdge, ownerDiagram);
		}

		// Applies the changes made to the corresponding model element
		public override void ApplyModelChanges ()
		{
			_nameUMLEntry.Text = _namedElementModel.Name;
		}

		private void BroadcastNameChange (object obj, string newName)
		{
			if (_namedElementModel.Name != newName)
			{
				_namedElementModel.Name = newName;
				if (NameChanged != null)
				{
					NameChanged (GraphElement, newName);
				}
			}
		}

		// Looks for the contained graph node that represents the element name,
		// then creates a UMLEntry and shows it.
		// The name of the edge is represented as two graph nodes: one for the
		// point where the name is located (typeInfo DirectedName) and another one
		// (contained within the former) which represents the label itself.
		// As always, every position is relative to its owner's position.
		// Note that edges are always placed at (0, 0).
		private void SearchAndDrawName (DI.GraphEdge graphEdge, UMLDiagram ownerDiagram)
		{
			// looks for the contained graph node that represents the edge name
			DI.GraphNode directedNameGN = null;
			int i = 0;
			DI.SimpleSemanticModelElement bridge;
			while (_directedNameGN == null && i < graphEdge.Contained.Count)
			{
				directedNameGN = graphEdge.Contained [i++] as DI.GraphNode;
				if (directedNameGN != null)
				{
					bridge = directedNameGN.SemanticModel as DI.SimpleSemanticModelElement;
					if (bridge != null && bridge.TypeInfo == "DirectedName")
					{
						_directedNameGN = directedNameGN;
					}
				}
			}
			// if the corresponding graph node was found, draw it
			if (_directedNameGN != null)
			{
				// first, find the corresponding Graph Node
				i = 0;
				DI.GraphNode nameGN = null;
				bridge = null;
				do
				{
					nameGN = _directedNameGN.Contained [i++] as DI.GraphNode;
					if (nameGN != null)
					{
						bridge = nameGN.SemanticModel as DI.SimpleSemanticModelElement;
					}
				} while (bridge==null || bridge.TypeInfo != "Name");
				// now we're able to draw it
				_nameUMLEntry = new UMLEntry (this, nameGN, ownerDiagram.CanvasRoot, true, _namedElementModel.Name);
				base.AddFreeEntry (_nameUMLEntry);
				_nameUMLEntry.TextChanged += new UMLElementNameChangedHandler (BroadcastNameChange);
				_nameUMLEntry.Show ();
			}
		}

		public event UMLElementNameChangedHandler NameChanged = null;

		private DI.GraphNode _directedNameGN;
		private UML.NamedElement _namedElementModel;
		private UMLEntry _nameUMLEntry;
	}
}
