/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2004  Mario Carrión <mario.carrion@gmail.com>
Copyright (C) 2004  Manuel Cerón <ceronman@gmail.com>

UMLElement.cs: base class for all user-interactive-elements 

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
using System.Collections;

namespace MonoUML.Widgets.UML2
{
	public abstract class UMLElement : UMLWidget
	{
		public UMLElement (UMLDiagram ownerDiagram, DI.GraphElement graphElement) : base (ownerDiagram.CanvasRoot)
		{
			_ownerDiagram = ownerDiagram;
			_graphElement = graphElement; 
			ownerDiagram.AddMapping (graphElement, this);
			_freeEntries = new ArrayList ();
		}
		
		public UMLDiagram OwnerDiagram
		{
			get { return _ownerDiagram; }
		}

		// every UML Element must have a corresponding DI graph element
		public DI.GraphElement GraphElement
		{
			get { return _graphElement; }
		}
		
		// When overriding this method, first add the more specific options
		// and then call base.AddContextMenuOptions (options).
		protected virtual void AddContextMenuOptions (IList options)
		{
			options.Add (new DeleteRepresentationAction (_ownerDiagram, this));
		}

		// Adds an entry, which can be placed anywhere in the canvas; that is,
		// the entry is not placed inside the owner. However, when the owner
		// is moved, the entry will change its position accordingly.
		protected void AddFreeEntry (UMLEntry entry)
		{
			_freeEntries.Add (entry);
		}

		// Applies the changes made to the corresponding model element
		public virtual void ApplyModelChanges ()
		{ }

		// Hides and destroys this graphical element and the graphical elements
		// that were created by this one.
		public new void Destroy ()
		{
			foreach (UMLEntry entry in  _freeEntries)
			{
				entry.Hide ();
				entry.Destroy ();
			}
			Hide ();
			base.Destroy ();
		}

		public IList GetContextMenuOptions ()
		{
			System.Collections.ArrayList options = new System.Collections.ArrayList ();
			AddContextMenuOptions (options);
			return options; 
		}

		//Iterates through all the widgets in the element to move them.
		private void IterateChildrenMoved (double dx, double dy)
		{
			foreach (UMLEntry entry in  _freeEntries)
			{
				entry.X += dx;
				entry.Y += dy;
			}
		}

		// params: distance moved over each axis
		public new void Move (double dx, double dy)
		{
			IterateChildrenMoved (dx, dy);
			base.Move (dx, dy);
			OnMoved ();
		}

		public virtual void UpdateElement (object modified)
		{ }

		private ArrayList _freeEntries;
		private DI.GraphElement _graphElement;
		protected UMLDiagram _ownerDiagram;
	}
}
