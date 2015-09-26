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
	public class GraphElement : DiagramElement
	{
		public GraphElement()
		{
			_anchorage = new ArrayList();
			_contained = new ArrayList();
			_link = new ArrayList();
			_position = new Point();
		}

		public ArrayList Anchorage
		{
			get { return _anchorage; }
		}

		public ArrayList Contained
		{
			get { return _contained; }
		}

		public bool DeleteRepresentations(object semanticElement)
		{
			return DeleteRepresentations(semanticElement, new ArrayList());
		}
		
		private bool DeleteRepresentations(object semanticElement, IList affectedContainers)
		{
			bool deleted = false;
			int i = 0;
			GraphElement contained;
			ISemanticBridge bridge;
			while(i < Contained.Count)
			{
				contained = (GraphElement)Contained[i];
				bridge = contained.SemanticModel as ISemanticBridge;
				if(bridge != null && object.ReferenceEquals(bridge.Element, semanticElement))
				{
					contained.Delete(affectedContainers);
					deleted = true;
				}
				else
				{
					deleted |= contained.DeleteRepresentations(semanticElement, affectedContainers);
					i ++;
				}
			}
			return deleted;
		}

		// returns the list of the containers that were affected by the operation
		public IList Delete()
		{
			ArrayList containers = new ArrayList ();
			Delete (containers);
			return containers;
		}
		
		protected virtual void Delete(IList affectedContainers)
		{
			System.Console.WriteLine("GraphElement.Delete> deleting: " + this.ToString());
			GraphConnector anchorage;
			while (this.Anchorage.Count > 0)
			{
				anchorage = (GraphConnector) this.Anchorage[0];
				// we must delete all the edges that end in this element
				while (anchorage.GraphEdge.Count > 0)
				{
					((GraphEdge)anchorage.GraphEdge[0]).Delete(affectedContainers);
				}
				this.Anchorage.Remove (anchorage);
			}
			GraphElement container = this.Container;
			if(container != null)
			{
				container.Contained.Remove(this);
				if(!affectedContainers.Contains(container)) affectedContainers.Add(container);
			}
		}

		public Point GlobalPosition
		{
			get
			{
				if (base.Container != null)
				{
					return base.Container.GlobalPosition + _position;
				}
				else
				{
					return _position;
				}
			}
		}

		public ArrayList Link
		{
			get { return _link; }
		}

		public Point Position
		{
			get { return _position; }
		}
		
		public SemanticModelBridge SemanticModel
		{
			get { return _semanticModel; }
			set { _semanticModel = value; }
		}

		internal void SetPosition(Point position)
		{
			_position = position;
		}

		private ArrayList _anchorage;
		private ArrayList _contained;
		private ArrayList _link;
		private Point _position;
		private SemanticModelBridge _semanticModel;
	}
}
