/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2005  Rodolfo Campero <rodolfo.campero@gmail.com>

UMLClassifier.cs: the UML2 representation of Classifiers

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
	public class UMLClassifier : UMLBox
	{
		public UMLClassifier (UMLDiagram ownerDiagram, DI.GraphNode graphNode): base (ownerDiagram, graphNode)
		{
			_modelElement = (UML.Classifier)base._modelElement;
			_name.FontModifier = (_modelElement.IsAbstract ? "italic" : "");
		}

		protected override void AddContextMenuOptions (System.Collections.IList options)
		{
			options.Add (new CreateAssociationAction (_ownerDiagram));
			base.AddContextMenuOptions (options);
		}

		// Applies the changes made to the corresponding model element
		public override void ApplyModelChanges ()
		{
			_name.FontModifier = (_modelElement.IsAbstract ? "italic" : ""); 
			if (_name.Text != _modelElement.Name)
			{
				_name.Text = _modelElement.Name;
				Redraw ();
			} 
		}

		public static UMLClassifier CreateNew (
			UMLDiagram ownerDiagram,
			UML.Classifier classifierModel )
		{
			DI.GraphNode classifierGN = UMLBox.CreateNewGraphNode(ownerDiagram, classifierModel);
			return new UMLClassifier (ownerDiagram, classifierGN);
		}
		
		protected new UML.Classifier _modelElement;
	}	
}