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
	public class GraphNode : GraphElement
	{
		public GraphNode()
		{
			_size = new Dimension();
		}

		public Point Center
		{
			get
			{
				return new Point (Position.X + Size.Width / 2D, Position.Y + Size.Height / 2D);
			}
		}

		public Dimension Size
		{
			get { return _size; }
		}
		
		internal void SetSize(Dimension size)
		{
			_size = size;
		}
		
		private Dimension _size;
	}
}