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

namespace MonoUML.DI
{
	public class DiagramLink
	{
		public Diagram Diagram
		{
			get { return _diagram; }
			set { _diagram = value; }
		}

		public GraphElement GraphElement
		{
			get { return _graphElement; }
			set { _graphElement = value; }
		}

		public Point Viewport
		{
			get { return _viewport; }
			set { _viewport = value; }
		}

		public double Zoom
		{
			get { return _zoom; }
			set { _zoom = value; }
		}

		private Diagram _diagram;
		private GraphElement _graphElement;
		private Point _viewport;
		private double _zoom;
	}
}