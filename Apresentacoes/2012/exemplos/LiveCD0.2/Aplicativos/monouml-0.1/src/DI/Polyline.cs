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
	public class Polyline : GraphicPrimitive
	{
		public Polyline()
		{
			_waypoints = new ArrayList();
		}
		
		public bool Closed
		{
			get { return _closed; }
			set { _closed = value; }
		}

		public ArrayList Waypoints
		{
			get { return _waypoints; }
		}

		private bool _closed;
		private ArrayList _waypoints;
	}
}