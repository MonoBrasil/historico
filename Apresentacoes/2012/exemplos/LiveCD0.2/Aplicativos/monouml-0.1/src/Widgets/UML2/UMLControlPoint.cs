/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2004  Mario Fuentes <mario@gnome.cl>
Copyright (C) 2004  Mario Carrión <marioc@unixmexico.org>
Copyright (C) 2004  Manuel Cerón <ceronman@gmail.com>

ControlsPoints.cs: a sample element hier of UMLNode.

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
	public class UMLControlPoint : CanvasRect
	{
	
		public UMLControlPoint (UMLControlPointGroup group) : this ((CanvasGroup) group)
		{
			_container = group;
		}
		
		public UMLControlPoint (CanvasGroup group) : base (group)
		{
			if (_movable == true) { CPFillColor = DEFAULT_FILL_COLOR; }
			else { CPFillColor = DEFAULT_STATIC_FILL_COLOR; }

			OutlineColor = DEFAULT_OUTLINE_COLOR;
			WidthUnits = 1.0;
			CapStyle = Gdk.CapStyle.Round;
			CanvasEvent += CanvasEventCb;
			_container = null;
			Show ();
		}

		public double DX { get { return _dx; } }
		public double DY { get { return _dy; } }
		
		public double MX 
		{ 
			get { return _mx;  }
			set { _mx = value; } 
		}
		
		public double MY 
		{ 
			get { return _my;  }
			set { _my = value; } 
		}
		
		public string CPFillColor 
		{ 
			get { return _cp_fillcolor; }
			set 
			{ 
				_cp_fillcolor = value;
				FillColor = value; 
			} 
		}
		
		public event MovedHandler Moved;
		public event UMLElementButtonEventHandler ButtonPressed;
		public event UMLElementButtonEventHandler ButtonReleased;

		public double X
		{
			get { return (X2 + X1) / 2.0; }
			set
			{
				X1 = value - 4;
				X2 = value + 4;
			}
		}

		public double Y
		{
			get { return (Y2 + Y1) / 2.0; }
			set
			{
				Y1 = value - 4;
				Y2 = value + 4;
			}
		}
		
		//Is the element moveable?
		public bool Moveable 
		{
			get { return _movable; }
			set
			{
				if (value == true) { FillColor = _cp_fillcolor; }
				else { FillColor = DEFAULT_STATIC_FILL_COLOR; }
				_movable = value;
			}
		}
	
		private void CanvasEventCb (object o, CanvasEventArgs args)
		{
			Gdk.Event ev = args.Event;
			//System.Console.WriteLine ("UMLControlPoint> " + ev.Type);
			switch (ev.Type)
			{
				case EventType.EnterNotify:
				{
					if (!_hover) { SetHover (true); } else { SetPressed (); }
					break;
				}
				case EventType.LeaveNotify:
				{
					SetHover (false);
					break;
				}
				case EventType.ButtonPress:
				{
					EventButton eb = new EventButton (ev.Handle);
					if (eb.Button == 1)
					{
						if (_movable == false) { break; }

						if (ev.Type == EventType.ButtonPress)
						{
							SetPressed (eb.X, eb.Y);
						}
					}
					if (ButtonPressed != null)
					{
						ButtonPressed (this, eb);
						// stop the event from being further emitted
						args.RetVal = true;
					}
					break;
				}
				case EventType.ButtonRelease:
				{
					EventButton eb = new EventButton (ev.Handle);
					if (eb.Button == 1)
					{
						if (_movable == false) { break; }
						
						_pressed = false;
						
						if (_container != null)
						{ 
							_container.UMLElement.UMLControlPoint = false;
						}
					}
					if (ButtonReleased != null)
					{
						ButtonReleased (this, eb);
						// stop the event from being further emitted
						args.RetVal = true;
					}
					break;
				}
				case EventType.MotionNotify:
				{
					if (_pressed)
					{
						if (_movable == false) { break; }
						EventMotion em = new EventMotion (ev.Handle);
						ForceMove (em.X, em.Y);
					}
					break;
				}
			} // end switch
		}
		
		internal void ForceMove (double x, double y)
		{
			_dx = x - _mx;
			_dy = y - _my;
			_mx = x;
			_my = y;
			if (Moved != null) { Moved (this); }
		}
		
		internal void ForceRelease (Gtk.ButtonReleaseEventArgs args)
		{
			_pressed = false;
			if (_container != null)
			{ 
				_container.UMLElement.UMLControlPoint = false;
			}
			EventButton eb = new EventButton (args.Event.Handle);
			if (ButtonReleased != null)
			{
				ButtonReleased (this, eb);
			}
		}

		private void SetHover (bool onoff)
		{
			//System.Console.WriteLine ("UMLControlPoint.SetHover> {0}", onoff);
			_hover = onoff;
			if (_movable == true)
			{
				if (onoff) { FillColor = DEFAULT_HOVER_FILL_COLOR; }
				else if (!_pressed) { FillColor = _cp_fillcolor; }
			}
		}
		
		public void SetPressed ()
		{
			SetHover (true);
			SetPressed (X, Y);
		}
		
		private void SetPressed (double mx, double my)
		{
			//System.Console.WriteLine ("UMLControlPoint.SetPressed> {0}  {1}", mx, my);
			if (_container != null)
			{
				_container.UMLElement.UMLControlPoint = true;
			}
			_pressed = true;
			_mx = mx;
			_my = my;														
		}

		private const string DEFAULT_FILL_COLOR = "#FFFFAA";
		private const string DEFAULT_OUTLINE_COLOR = "dark blue";
		private const string DEFAULT_HOVER_FILL_COLOR = "red";
		private const string DEFAULT_STATIC_FILL_COLOR = "white";
		
		private string _cp_fillcolor;

		private bool _hover = false;
		private bool _pressed = false;
		private bool _movable = true;
		private double _mx, _my;
		private double _dx, _dy;
		private UMLControlPointGroup _container;
	}
}
