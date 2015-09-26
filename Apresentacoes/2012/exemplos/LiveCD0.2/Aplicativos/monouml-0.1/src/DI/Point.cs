/*
MonoUML.DI - A library for handling Diagram Interchange elements
Copyright (C) 2004  Rodolfo Campero

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
using System;
using System.Collections;

namespace MonoUML.DI
{
	public class Point : ICloneable
	{
		public Point () {}

		public Point (double x, double y)
		{
			_x = x;
			_y = y;
		}

		public double X
		{
			get { return _x; }
			set { _x = value; }
		}

		public double Y
		{
			get { return _y; }
			set { _y = value; }
		}

		public static Point operator +(Point p1, Point p2) 
		{
			return new Point (p1._x + p2._x, p1._y + p2._y);
		}
		
		object ICloneable.Clone ()
		{
			return this.Clone ();
		}
		
		public Point Clone ()
		{
			return new Point (this.X, this.Y);
		}
		
		// copies the values of X and Y in the target point
		public void CopyTo (Point target)
		{
			target.X = _x;
			target.Y = _y;
		}
		
		// Returns a new point that is placed between the given points
		public static Point GetHalfWayPoint (Point p1, Point p2)
		{
			return new Point ((p1.X + p2.X) / 2D, (p1.Y + p2.Y) / 2D); 
		}

		public override string ToString()
		{
			System.Globalization.NumberFormatInfo nfi
				= System.Globalization.NumberFormatInfo.InvariantInfo; 
			return "(" + _x.ToString (nfi) + "," + _y.ToString (nfi) + ")";
		}

		private double _x;
		private double _y;
	}
}