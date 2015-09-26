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
using DI = MonoUML.DI;

namespace MonoUML.Widgets.UML2
{
	// Represents an open arrow drawn at the end of an edge. 
	public class UMLOpenArrow : UMLEdgeEnd
	{
		public UMLOpenArrow (CanvasGroup group) : base (group)
		{
			_arrowhead = new CanvasLine (group);
			_arrowhead.WidthUnits= 1.0;
			_arrowhead.FillColorGdk = UMLEdge.OUTLINE_COLOR;
			_arrowhead.Lower (1);
			Redraw ();
			_arrowhead.Show ();
		}
		
		public override void Clear ()
		{
			_arrowhead.Hide ();
			_arrowhead.Destroy ();
			_arrowhead = null;
		}

		public override void Redraw ()
		{
			if (base.Position != null)
			{
				// r = radius, a = angle
				double r, a0, a1;
				r = 9D;
				a0 = base.Orientation + 0.65D;
				a1 = base.Orientation - 0.65D;
				double x, y, x0, x1, y0, y1;
				// the diamond is drawn two pixels away from the end, so the
				// control point below it remains accessible.
				x = base.Position.X + 2D * Math.Cos (base.Orientation);
				y = base.Position.Y + 2D * Math.Sin (base.Orientation);
				x0 = x + r * Math.Cos (a0);
				y0 = y + r * Math.Sin (a0);
				x1 = x + r * Math.Cos (a1);
				y1 = y + r * Math.Sin (a1);
				_arrowhead.Points = new CanvasPoints (
					new double[] { x0, y0, x, y, x1, y1 }
				);
			}
		}

		public override void SetHighlighted (bool highlighted)
		{
			_arrowhead.FillColorGdk = (highlighted ? UMLEdge.HIGHLIGHTED_OUTLINE_COLOR : UMLEdge.OUTLINE_COLOR);
		}

		private CanvasLine _arrowhead;
	}
}
