/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2004  Mario Fuentes <mario@gnome.cl>
Copyright (C) 2004  Mario Carrión <marioc@unixmexico.org>

UMLNode.cs: base class for create any element.

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

namespace MonoUML.Widgets.UML2
{
	public abstract class UMLNode : UMLElement
	{
		protected UMLNode (UMLDiagram ownerDiagram, DI.GraphNode graphNode) : base (ownerDiagram, graphNode)
		{
			// hack para evitar un bug... 
			// si no hago esto al setear Width repinta todo, con lo cual borra el Height
			_layout_suspended = true;
			_graph_node = graphNode;
 			Width = graphNode.Size.Width;
			Height = graphNode.Size.Height;
			_layout_suspended = false;
			Move (graphNode.Position.X, graphNode.Position.Y);
		}

		// The DI graph node where this element keeps its graphical attributes
		public DI.GraphNode GraphNode
		{
			get { return _graph_node; }
		}
		
		// Delegate for UMLControlPointGroup Resized Event
		private void CPResized (object o, double w, double h, double dx, double dy)
		{
			if (Width > 0) _width_proportion = w / Width;
			if (Height > 0) _height_proportion = h / Height;
			Width = w;
			Height = h;
			
			_dx_proportion = dx;
			_dy_proportion = dy;
			Move (dx, dy);
			base.FireResizedEvent ();
		}

		// Creates the control points
		private void CreateUMLControlPointGroup ()
		{
			if (_control_points == null)
			{
				_control_points = new UMLControlPointGroup (this, _is_resizable);
				_control_points.Resized += CPResized;
			}
			else
			{
				_control_points.Show ();
			}
			RaiseToTop ();
			RequestRedraw ();	
		}

		protected override void EnterNotify (CanvasEventArgs args) 
		{
			_is_mouse_hover = true;
		}
		
		protected override void LeaveNotify (CanvasEventArgs args) 
		{
			_is_mouse_hover = false;
		}
		
		protected override void OnDeselected ()
		{
			RemoveUMLControlPointGroup ();
			_is_selected = false;
		}

		protected override void OnMoved ()
		{
			_graph_node.Position.X = X;
			_graph_node.Position.Y = Y;
		}
		
		protected override void OnSelected ()
		{
			CreateUMLControlPointGroup ();
		}
		
		protected override void Redraw ()
		{
			if (_control_points != null)
			{
				_control_points.Redraw ();
			}
		}

		// Method for removing current control points
		private void RemoveUMLControlPointGroup ()
		{
			if (_control_points != null)
			{
				_control_points.Hide ();
			}
			RequestRedraw ();
		}
		
		protected void SetDIProperties ()
		{
			_graph_node.Size.Width = Width;
			_graph_node.Size.Height = Height;
		}

		// Rectangles for the control points
		protected UMLControlPointGroup _control_points = null;
		private DI.GraphNode _graph_node;
		// FLAG: Is the mouse hover me?
		private bool _is_mouse_hover = false;
	}
}
