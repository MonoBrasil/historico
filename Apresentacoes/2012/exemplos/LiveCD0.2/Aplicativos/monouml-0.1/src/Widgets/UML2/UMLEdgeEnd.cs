/*
MonoUML.Widgets.UML2 - A library for representing the UML2 elements
Copyright (C) 2004, 2005  Mario Carrión <marioc@unixmexico.org>
Copyright (C) 2004, 2005  Manuel Cerón <ceronman@gmail.com>
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
using Gdk;
using Gnome;
using System;

namespace MonoUML.Widgets.UML2
{
	// Represents an icon drawn at the end of an edge. 
	public abstract class UMLEdgeEnd : CanvasGroup
	{
		protected UMLEdgeEnd (CanvasGroup group) : base (group)
		{ }
		
		public abstract void Clear ();

		// in radians
		public double Orientation
		{
			get { return _orientation; }
			set { _orientation = value; }
		}

		public DI.Point Position
		{
			get { return _position; }
			set { _position = value; }
		}

		public abstract void Redraw ();
		
		public abstract void SetHighlighted (bool highlighted);

		private double _orientation;
		private DI.Point _position;
	}
}
