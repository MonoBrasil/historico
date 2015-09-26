/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2005  Rodolfo Campero <rodolfo.campero@gmail.com>

UMLInstanceSpecification.cs: the UML2 representation of InstanceSpecifications

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
	public class UMLInstanceSpecification : UMLBox
	{
		public UMLInstanceSpecification (UMLDiagram ownerDiagram, DI.GraphNode graphNode): base (ownerDiagram, graphNode)
		{
		}

		protected override void AddContextMenuOptions (System.Collections.IList options)
		{
			options.Add (new CreateAssociationAction (_ownerDiagram));
			base.AddContextMenuOptions (options);
		}

		public static UMLInstanceSpecification CreateNew (
			UMLDiagram ownerDiagram,
			UML.InstanceSpecification iSpecModel)
		{
			DI.GraphNode isGN = UMLBox.CreateNewGraphNode(ownerDiagram, iSpecModel);
			return new UMLInstanceSpecification (ownerDiagram, isGN);
		}
	}	
}