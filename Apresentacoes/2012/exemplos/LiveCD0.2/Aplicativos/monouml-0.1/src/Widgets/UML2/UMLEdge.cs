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
using Gnome;
using Gdk;
using System;
using System.Collections;

namespace MonoUML.Widgets.UML2
{
	public class UMLEdge : UMLElement
	{
		public static readonly Gdk.Color HIGHLIGHTED_OUTLINE_COLOR;
		public static readonly Gdk.Color OUTLINE_COLOR;
		public static readonly Gdk.Color WHITE;

		static UMLEdge()
		{
			Gdk.Colormap colormap = Gdk.Colormap.System;
			Gdk.Color.Parse ("orange", ref HIGHLIGHTED_OUTLINE_COLOR);
			colormap.AllocColor (ref HIGHLIGHTED_OUTLINE_COLOR, true, true);
			Gdk.Color.Parse ("black", ref OUTLINE_COLOR);
			colormap.AllocColor (ref OUTLINE_COLOR, true, true);
			Gdk.Color.Parse ("white", ref WHITE);
			colormap.AllocColor (ref WHITE, true, true);
		}

		public UMLEdge (UMLDiagram ownerDiagram, DI.GraphEdge graphEdge)
			: this (ownerDiagram, graphEdge, true)
		{ }

		protected UMLEdge (UMLDiagram ownerDiagram, DI.GraphEdge graphEdge, bool forceRedraw)
			: base (ownerDiagram, graphEdge)
		{
			_graphEdge = graphEdge;
			DI.GraphConnector connector;
			_ends0 = new ArrayList (2);
			_ends1 = new ArrayList (2);
			connector = (DI.GraphConnector)graphEdge.Anchor [0];
			_from_element = ownerDiagram.GetUmlcanvasElement (connector.GraphElement);
			connector = (DI.GraphConnector)graphEdge.Anchor [1];
			_to_element = ownerDiagram.GetUmlcanvasElement (connector.GraphElement);
			_control_points = new ArrayList ();
			// control points and line segments
			//CreateControlPoints ();
			CreateLineSegments ();
			// From and To elements
			_from_element.Moved += FromNodeMoved;
			_from_element.Resized += FromNodeResized;
			_to_element.Moved += ToNodeMoved;
			_to_element.Resized += ToNodeResized;
			if (forceRedraw) { ForceRedraw (); }
		}
		
		// Adds an edge end to the list of edge ends
		public void AddFromEnd (UMLEdgeEnd end)
		{
			_ends0.Add (end);
		}
		
		// Adds an edge end to the list of edge ends
		public void AddToEnd (UMLEdgeEnd end)
		{
			_ends1.Add (end);
		}

		private void BreakSegment (CanvasLine segment, CanvasEventArgs args)
		{
			EventButton eb = new EventButton (args.Event.Handle);
			BreakSegment (segment, eb.X, eb.Y); 
		}
		
		private void BreakSegment (CanvasLine segment, double x, double y)
		{
			// if there is another control point near, don't break the line
			UMLControlPoint cp = null;
			DI.Point wp;
			for(int i = 0; i < _graphEdge.Waypoints.Count; i ++)
			{
				wp = (DI.Point) _graphEdge.Waypoints [i];
				if( Math.Abs (wp.X - x) < 3 && Math.Abs (wp.Y - y) < 3)
				{
					cp = (UMLControlPoint) _control_points [i];
					break;
				} 
			}
			if (cp == null)
			{
				int pos = _line_segments.IndexOf (segment) + 1;
				// creates a new line segment
				CanvasLine newSegment = CreateLine ();
				_line_segments.Insert (pos, newSegment);
				// creates a new way point
				_graphEdge.Waypoints.Insert (pos, new DI.Point (x, y));
				// creates a new control point
				cp = new UMLControlPoint (_ownerDiagram.CanvasRoot);
				cp.Moveable = true;
				cp.X = x;
				cp.Y = y;
				cp.ButtonReleased += new UMLElementButtonEventHandler (CPButtonReleased);
				cp.Moved += new MovedHandler (CPMoved);
				cp.Show ();
				cp.Raise (1);
				_control_points.Insert (pos, cp);
				base.UMLControlPoint = true;
			}
			// Redraw the edge
			cp.SetPressed ();
			ForceRedraw ();
			_ownerDiagram.EnableControlPointMotion (this, cp);
		}

		private void CalculateDimensions ()
		{
			// xfb = x, "from" side, before
			// ytb = y, "to" side, before
			double xfb, xtb, xf0, xf1, xt0, xt1;
			double yfb, ytb, yf0, yf1, yt0, yt1;
			IList wps = _graphEdge.Waypoints;
			// fwp = "From" waypoint, twp = "To" waypoint
			DI.Point fwp = (DI.Point) wps[0];
			DI.Point twp = (DI.Point) wps[wps.Count - 1];
			xfb = fwp.X;
			yfb = fwp.Y;
			xtb = twp.X;
			ytb = twp.Y;
			xf0 = _from_element.X;
			xf1 = xf0 + _from_element.Width;
			yf0 = _from_element.Y;
			yf1 = yf0 + _from_element.Height;
			DI.Point f, t;
			DI.Point cp;
			int lineCount = wps.Count - 1;
			if (lineCount == 1)
			{
				xt0 = _to_element.X;
				xt1 = xt0 + _to_element.Width;
				yt0 = _to_element.Y;
				yt1 = yt0 + _to_element.Height;
				CalculateRoute(xf0, xf1, yf0, yf1, xt0, xt1, yt0, yt1, 
					fwp, twp);
			}
			else
			{
				// line from the "from" element to the first control point
				f = fwp.Clone ();
				cp = ((DI.Point)wps[1]).Clone ();
				xt0 = xt1 = cp.X;
				yt0 = yt1 = cp.Y;
				CalculateRoute(xf0, xf1, yf0, yf1, xt0, xt1, yt0, yt1, f, cp);
				f.CopyTo (fwp);
				// line from the last control point to the "to" element
				cp = ( (DI.Point)wps[wps.Count - 2]).Clone ();
				xf0 = xf1 = cp.X;
				yf0 = yf1 = cp.Y;
				xt0 = _to_element.X;
				xt1 = xt0 + _to_element.Width;
				yt0 = _to_element.Y;
				yt1 = yt0 + _to_element.Height;
				t = twp.Clone ();
				CalculateRoute(xf0, xf1, yf0, yf1, xt0, xt1, yt0, yt1, cp, t); 
				t.CopyTo (twp);
			}
//			System.Console.WriteLine ("UMLEdge.Calc...> {0}, {1}, {2}, {3}", xfb, yfb, xf0, yf0);
			if (xfb != fwp.X || yfb != fwp.Y)
			{
				OnFromMoved (fwp.X - xfb, fwp.Y - yfb);
			}
			if (xtb != twp.X || ytb != twp.Y)
			{
				OnToMoved (twp.X - xtb, twp.Y - ytb);
			}
        	Redraw ();
		}
		
		// modifies both points, "from" and "to".
		private void CalculateRoute (
			double xf0, double xf1,
			double yf0, double yf1,
			double xt0, double xt1,
			double yt0, double yt1,
			DI.Point f, DI.Point t)
		{
			double x, y;
			// X
			if (xf1 < xt0)
			{
				f.X = xf1;
				t.X = xt0;
			}
			else if (xt1 < xf0)
			{
				f.X = xf0;
				t.X = xt1;
			}
			else if (xt0 <= xf1)
			{
				x = Math.Max(xf0, xt0);
				f.X = x + (Math.Min(xf1, xt1) - x) / 2;
				t.X = f.X;
			}
			else if (xf0 <= xt1)
			{
				x = Math.Max(xf1, xt1);
				f.X = x + (Math.Min(xf0, xt0) - x) / 2;
				t.X = f.X;
			}
			// Y
			if (yf1 < yt0)
			{
				if (xf1 < xt0)
				{
					f.Y = yf1;
					t.Y = yt0;
				}
				else
				{
					f.Y = yf1;
					t.Y = yt0;
				}
			}
			else if (yt1 < yf0)
			{
				if (xf1 < xt0)
				{
					f.Y = yf0;
					t.Y = yt1;
				}
				else
				{
					f.Y = yf0 ;
					t.Y = yt1;
				}
			}
			else if (yt0 <= yf1)
			{
				y= Math.Max(yf0, yt0);
				f.Y = y + (Math.Min(yf1, yt1) - y) / 2;
				t.Y = f.Y;
			}
			else if (yf0 <= yt1)
			{
				f.Y = yf0 + (yt1 -yf0) / 2;
				t.Y = f.Y;
			}
		}
		
		protected void ClearEnds ()
		{
			foreach(UMLEdgeEnd end in _ends0)
			{
				end.Clear ();
			}
			foreach(UMLEdgeEnd end in _ends1)
			{
				end.Clear ();
			}
			_ends0.Clear ();
			_ends1.Clear ();
		}
        
        private void CPMoved(object obj)
        {
        	UMLControlPoint draggedCP = (UMLControlPoint) obj;
        	int index = _control_points.IndexOf (obj);
        	DI.Point p = (DI.Point) _graphEdge.Waypoints [index];
        	p.X += draggedCP.DX;
        	p.Y += draggedCP.DY;
        	draggedCP.X = draggedCP.MX;
        	draggedCP.Y = draggedCP.MY;
			int lastWaypoint = _graphEdge.Waypoints.Count - 1;
        	if (lastWaypoint != 1 && (index == 1 || index == lastWaypoint - 1))
        	{
        		// we have to recalculate the waypoints, because either
        		// the first or the last one may have to be moved.
        		CalculateDimensions ();
        		// moves the corresponding control points (the first and the last)
       			UMLControlPoint otherCP = (UMLControlPoint) _control_points [0];
       			p = (DI.Point) _graphEdge.Waypoints [0];
       			otherCP.X = p.X;
       			otherCP.Y = p.Y;
       			otherCP = (UMLControlPoint) _control_points [lastWaypoint];
       			p = (DI.Point) _graphEdge.Waypoints [lastWaypoint];
       			otherCP.X = p.X;
       			otherCP.Y = p.Y;
        	}
        	ForceRedraw ();
        }

        private void CPButtonReleased (object obj, Gdk.EventButton eb)
        {
//        	Console.WriteLine ("UMLEdge.CPButtonReleased");
        	int lastWaypoint = _graphEdge.Waypoints.Count - 1;
			// Checks if the user is dropping the first or last control point
			// over a different element
			int index = _control_points.IndexOf (obj);
			if (index == 0)
			{
				ReplaceFromElement ();
			}
			else if (index == lastWaypoint)
			{
				ReplaceToElement ();
			}
        	// check if the corresponding waypoint can be removed (thus
        	// joining two line segments)
        	int pos = 1;
        	while (pos < _graphEdge.Waypoints.Count - 1)
        	{
        		// compare the line segment slopes
        		DI.Point p0 = (DI.Point)_graphEdge.Waypoints[pos - 1];
        		DI.Point p = (DI.Point)_graphEdge.Waypoints[pos];
        		DI.Point p1 = (DI.Point)_graphEdge.Waypoints[pos +1];
        		double dy0 = p.Y - p0.Y;
        		double dx0 = p.X - p0.X;
        		double dy1 = p1.Y - p.Y;
        		double dx1 = p1.X - p.X;
        		double m0 = dx0 != 0 ? dy0 / dx0 : (Math.Sign (dy0) > 0 ? double.PositiveInfinity : double.NegativeInfinity);
        		double m1 = dx1 != 0 ? dy1 / dx1 : (Math.Sign (dy1) > 0 ? double.PositiveInfinity : double.NegativeInfinity);
        		double a0 = Math.Atan (m0);
        		double a1 = Math.Atan (m1);
        		if (a0 > 0 && a1 < 0) { a1 += Math.PI; } 
        		if (a1 > 0 && a0 < 0) { a0 += Math.PI; } 
       			//Console.WriteLine ("index {0}; dy0={1},dx0={2},dy1={3},dx1={4},m0={5},m1={6},atan(m0)={7},atan(m1)={8}", pos, dy0, dx0, dy1, dx1, m0, m1, a0, a1);
        		if ( (Math.Abs (dx0) < 4D && Math.Abs (dy0) < 4D) || Math.Abs (a0 - a1) < 0.08D)
        		{
		        	// hide the deleted control point and line segment
					CanvasLine line = (CanvasLine) _line_segments [pos];
					line.Hide ();
					line.Destroy ();
					line.Dispose (); 
        			// join both segments
					_line_segments.RemoveAt (pos);
					_graphEdge.Waypoints.RemoveAt (pos);
        		}
        		else
        		{
        			++ pos;
        		}
        	}
        	EnsureEdgeVisibility ();
        	RemoveControlPoints ();
        	base.Deselect ();
        	ForceRedraw ();
        }

		// creates one control point for each internal line break
		// that is, it will create 0..N control points.
		private void CreateControlPoints ()
		{
			UMLControlPoint cp;
			DI.Point waypoint;
			_control_points.Clear ();
			IList wps = _graphEdge.Waypoints;
			for (int i = 0; i < wps.Count; i ++)
			{
				waypoint = (DI.Point)wps[i];
				cp = new UMLControlPoint (_ownerDiagram.CanvasRoot);
				cp.Moveable = true;
				cp.X = waypoint.X;
				cp.Y = waypoint.Y;
				cp.ButtonReleased += new UMLElementButtonEventHandler (CPButtonReleased);
				cp.Moved += new MovedHandler (CPMoved);
				_control_points.Add (cp);
			}
		}

		// there's no need to specify the location, because every segment will 
		// be placed in the canvas in the method Redraw.
		private CanvasLine CreateLine ()
		{
	        CanvasLine line = new CanvasLine (this);
	        line.CanvasEvent += new CanvasEventHandler (LineEventHandler);
			line.FillColorGdk = OUTLINE_COLOR;
	        line.WidthUnits = WIDTH_UNITS;
	        line.Lower (1);
			line.Show ();
			return line;
		}
		
		private void CreateLineSegments ()
		{
			IList wps = _graphEdge.Waypoints;
			_line_segments = new ArrayList(wps.Count - 1);
			for (int i = 0; i < wps.Count - 1; i ++)
			{
				_line_segments.Add (CreateLine ());
			}
		}

		protected override void EnterNotify (CanvasEventArgs args) 
		{
			SetLineHighlighted (true);
		}

		// When the "from" and "to" elements are the same, the edge
		// is no longer visible because the first and last
		// waypoints are the same.
		// In this case, we must ensure that there are some other
		// waypoints so the line is visible.  
		private void EnsureEdgeVisibility ()
		{
			if (_from_element == _to_element)
			{
				// create a loop
				IList wps = _graphEdge.Waypoints;
				if (wps.Count <= 2)
				{
					DI.Point elemPoint = _from_element.GraphElement.Position;
					DI.Point p = (DI.Point) wps[0];
					double dx = (p.X <= elemPoint.X ? -25D : 25D);
					double dy = (p.Y <= elemPoint.Y ? -25D : 25D);
					BreakSegment ((CanvasLine)_line_segments[0], p.X + dx, p.Y + dy);
					BreakSegment ((CanvasLine)_line_segments[0], p.X + dx, p.Y);
				}
			}
		}

		protected override void Finalize ()
		{
			foreach (CanvasLine line in _line_segments)
			{
				line.Destroy ();
			}
		}
		
		protected void ForceRedraw ()
		{
			_element_motioned = true;
			Redraw ();
		}

		private void FromNodeMoved (object obj, double dx, double dy)
		{
			RemoveControlPoints ();
			_element_motioned = _from_element.IsMotioned;
			CalculateDimensions ();
		}
		
		private void FromNodeResized (object obj, double w, double h)
		{
			RemoveControlPoints ();
			_element_motioned = _from_element.IsMotioned;
			CalculateDimensions ();
		}

		private static double GetAngle (DI.Point p0, DI.Point p1)
		{
			double dy = p1.Y - p0.Y;
			double dx = p1.X - p0.X;
			double a;
			if (dy == 0)
			{
				a = (dx >= 0 ? Math.PI : 0D);
			}
			else if (dx == 0)
			{
				a = (dy >= 0 ? Math.PI + Math.PI / 2D : Math.PI / 2D);
			}
			else
			{
				a = Math.Atan (dy / dx);
				if (dx > 0)
				{
					a += Math.PI;
				}
			}
			//System.Console.WriteLine ("UMLEdge.GetAngle> dx: {0}, dy: {1}, a: {2}", dx, dy, a);
			return a;
		}

		protected override void LeaveNotify (CanvasEventArgs args) 
		{
			SetLineHighlighted (false);
		}

		// handler for click events on line segments
		private void LineEventHandler (object source, CanvasEventArgs args)
		{
			//if (!_is_selected) return;
			EventButton eb;
			switch (args.Event.Type)
			{
				case EventType.ButtonPress:
					eb = new EventButton (args.Event.Handle);
					if (eb.Button == 1 && !_mouse_left_pressed)
					{
						if (_control_points.Count == 0) { CreateControlPoints (); }
						BreakSegment ((CanvasLine) source, args);
						_mouse_left_pressed = true;
					}
					break;
				case EventType.ButtonRelease:
					eb = new EventButton (args.Event.Handle);
					if (eb.Button == 1) { _mouse_left_pressed = false; }
					break;
			}
		}

		protected override void OnDeselected ()
		{
			SetLineHighlighted (false);
			RemoveControlPoints ();
		}

		protected virtual void OnFromMoved (double dx, double dy)
		{ }
		
		protected override void OnSelected ()
		{
			SetLineHighlighted (true);
//			CreateControlPoints ();
		}

		protected virtual void OnToMoved (double dx, double dy)
		{ }

		protected override void Redraw ()
		{
			if (_element_motioned)
			{
				IList wps = _graphEdge.Waypoints;
				DI.Point f, t;
				CanvasLine line;
				for(int i = 0; i < _line_segments.Count; i ++)
				{
					line = (CanvasLine) _line_segments [i];
					f = (DI.Point) wps [i];
					t = (DI.Point) wps [i + 1];
//					System.Console.WriteLine ("UMLEdge.Redraw> drawing line from {0} to {1}.", f, t);
		            line.Points = new CanvasPoints (new double [] {f.X, f.Y, t.X, t.Y});
		            line.Show ();
				}
				f = (DI.Point) wps [0];
				DI.Point f1 = (DI.Point) wps [1];
				double orient = GetAngle (f1, f);
				foreach (UMLEdgeEnd end0 in _ends0)
				{
					end0.Position = f;
					end0.Orientation = orient;
					end0.Redraw ();
				}
				t = (DI.Point) wps [_line_segments.Count];
				DI.Point t1 = (DI.Point) wps [_line_segments.Count - 1];
				orient = GetAngle (t1, t);
				foreach (UMLEdgeEnd end1 in _ends1)
				{
					end1.Position = t;
					end1.Orientation = orient;
					end1.Redraw ();
				}
				_element_motioned = false;
            }
		}

		private void RemoveControlPoints ()
		{
			foreach (UMLControlPoint cp in _control_points)
			{
				cp.Hide ();
				cp.Destroy ();
				cp.Dispose ();
			}
			_control_points.Clear ();
		}

		private void ReplaceFromElement ()
		{
			if (ReplaceFromModelElement ())
			{
	            _from_element.Moved -= new UMLElementMovedHandler (FromNodeMoved);
	            _from_element.Resized -= new UMLElementResizedHandler (FromNodeResized);
	            DI.GraphConnector anchorage = (DI.GraphConnector)_graphEdge.Anchor [0];
	            _from_element.GraphElement.Anchorage.Remove (anchorage);
				_from_element = _ownerDiagram.UMLCanvas.GetHoverElement (this);
	            _from_element.Moved += new UMLElementMovedHandler (FromNodeMoved);
	            _from_element.Resized += new UMLElementResizedHandler (FromNodeResized);
	            _from_element.GraphElement.Anchorage.Add (anchorage);
				anchorage.GraphElement = _from_element.GraphElement;
				((DI.Point) _graphEdge.Waypoints [0]).CopyTo (anchorage.Position); 
			}
			CalculateDimensions ();
			EnsureEdgeVisibility ();
		}

		// In order to allow replacing a source or target element,
		// a specialized edge must redefine this member, and return "true"
		// if the replacement completed sucessfully.
		protected virtual bool ReplaceFromModelElement ()
		{
			return false;
		}

		private void ReplaceToElement ()
		{
			if (ReplaceToModelElement ())
			{
				_to_element.Moved -= new UMLElementMovedHandler (ToNodeMoved);
				_to_element.Resized -= new UMLElementResizedHandler (ToNodeResized);
				DI.GraphConnector anchorage = (DI.GraphConnector)_graphEdge.Anchor [1];
				_to_element.GraphElement.Anchorage.Remove (anchorage);
				_to_element = _ownerDiagram.UMLCanvas.GetHoverElement (this);
				_to_element.Moved += new UMLElementMovedHandler (ToNodeMoved);
				_to_element.Resized += new UMLElementResizedHandler (ToNodeResized);
				_to_element.GraphElement.Anchorage.Add (anchorage);
				anchorage.GraphElement = _to_element.GraphElement;
			}
			CalculateDimensions ();
			EnsureEdgeVisibility ();
		}

		// See comment on ReplaceFromModelElement.
		protected virtual bool ReplaceToModelElement ()
		{
			return false;
		}

		private void SetLineHighlighted (bool highlighted)
		{
			Gdk.Color gdkcolor = highlighted ? HIGHLIGHTED_OUTLINE_COLOR : OUTLINE_COLOR; 
			foreach (CanvasLine line in _line_segments)
			{
				line.FillColorGdk = gdkcolor;
			}
			foreach (UMLEdgeEnd end in _ends0)
			{
				end.SetHighlighted (highlighted);
			}
			foreach (UMLEdgeEnd end in _ends1)
			{
				end.SetHighlighted (highlighted);
			}
		}
		
        private void ToNodeMoved (object obj, double dx, double dy)
        {
			RemoveControlPoints ();
			_element_motioned = _to_element.IsMotioned;
			CalculateDimensions ();
        }
        
        private void ToNodeResized (object obj, double w, double h)
        {
			RemoveControlPoints ();
			_element_motioned = _to_element.IsMotioned;
			CalculateDimensions ();
        }
		
		
		private const int CONTROL_POINTS_WIDTH = 5;
		private const double WIDTH_UNITS = 2.0;
		// I join the following elements 
		private UMLElement _from_element;
		private UMLElement _to_element;
		protected DI.GraphEdge _graphEdge;
		// FLAG: FromNode/ToNode is being motioned 
		private bool _element_motioned = false;
		// typeof UMLControlPoint
		private ArrayList _control_points;
		// From and To control points
		private UMLControlPoint _from_cp;
		private UMLControlPoint _to_cp;
		// The line - typeof CanvasLine
		private ArrayList _line_segments;
		// an arraylist of UMLEdgeEnds
		private ArrayList _ends0;
		private ArrayList _ends1;
	}
}
