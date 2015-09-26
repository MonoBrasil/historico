/*
MonoUML.Widgets.UML2 - A library for representing the UML2 elements
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
using System.Collections;

namespace MonoUML.Widgets.UML2
{
	public class UMLObjectDiagram : UMLDiagram
	{
		public UMLObjectDiagram (DI.Diagram diagram, Widgets.NoteBook notebook)
			: base (diagram, notebook)
		{ }

		protected override void GetDerivedContextMenuOptions (ArrayList options)
		{
			options.Add (new CreateInstanceSpecificationAction (this));
		}
	}
}
