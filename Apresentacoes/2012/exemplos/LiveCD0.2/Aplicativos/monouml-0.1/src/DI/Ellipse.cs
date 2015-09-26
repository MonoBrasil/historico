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
	public class Ellipse : GraphicPrimitive
	{
		public Point Center
		{
			get { return _center; }
			set { _center = value; }
		}

		public double EndAngle
		{
			get { return _endAngle; }
			set { _endAngle = value; }
		}

		public double RadiusX
		{
			get { return _radiusX; }
			set { _radiusX = value; }
		}

		public double RadiusY
		{
			get { return _radiusY; }
			set { _radiusY = value; }
		}

		public double Rotation
		{
			get { return _rotation; }
			set { _rotation = value; }
		}

		public double StartAngle
		{
			get { return _startAngle; }
			set { _startAngle = value; }
		}

		private Point _center;
		private double _endAngle;
		private double _radiusX;
		private double _radiusY;
		private double _rotation;
		private double _startAngle;
	}
}