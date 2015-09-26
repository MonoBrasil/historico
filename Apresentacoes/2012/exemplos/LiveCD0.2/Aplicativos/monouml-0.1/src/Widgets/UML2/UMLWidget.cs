/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2004, 2005  Mario Carrión <mario.carrion@gmail.com>
Copyright (C) 2004, 2005  Manuel Cerón <ceronman@gmail.com>
Copyright (C) 2005  Rodolfo Campero <rodolfo.campero@gmail.com>

UMLWidget.cs: base class for all user-interactive-elements 

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

namespace MonoUML.Widgets.UML2
{
	public abstract class UMLWidget : CanvasGroup
	{
		public UMLWidget (CanvasGroup group) : base (group)
		{
			InitializeRoot ();
			CanvasEvent += TriggerEvents;
		}

#region Properties
		// Where all the childrens should live
		protected CanvasGroup ChildrensRoot
		{
			get { return _childrensRoot; }
		}
		
		public double Height
		{
			get { return _height; }
			set
			{
				if ((value < 0) ||  (value == _height)) return;
				_height = value;
				if (!_layout_suspended) Redraw ();
			}
		}
		
		//Is the element being motioned?
		public bool IsMotioned
		{
			get { return _motioned; }
		}
		
		public bool IsMouseLeftPressed
		{
			get { return _mouse_left_pressed; }
		}

		public bool IsSelected
		{
			get { return _is_selected; }
		}

		public bool Resizable
		{
			get  { return _is_resizable; }
		}

		public double Width
		{
			get { return _width; }
			set
			{
				if ((value < 0) || (value == _width)) return;
				_width = value;
				if (!_layout_suspended) Redraw ();
			}
		}
		
		// Is the user clicking any control point?
		public bool UMLControlPoint
		{
			set { _is_control_point = value; }
			get { return _is_control_point; }
		}
#endregion

#region Events
		public event UMLElementButtonEventHandler ButtonPressed = null;
		public event UMLElementButtonEventHandler ButtonReleased = null;
		public event UMLElementEnterNotifyHandler EnterNotified = null;
		public event UMLElementLeaveNotifyHandler LeaveNotified = null;
		public event UMLElementMotionedHandler Motioned = null;
		public event UMLElementMovedHandler Moved = null;
		public event UMLElementResizedHandler Resized = null;
		public event UMLElementSelectedHandler Selected = null;
#endregion

#region Methods
		// Clicked
		protected virtual void ButtonPress (CanvasEventArgs args) 
		{
			if (args != null) 
			{
				EventButton eb = new EventButton (args.Event.Handle);
				if (eb.Button == 1)
				{
					_mouse_left_pressed = true;
					// Used for keeping the relative distance between the 
					// upper corner point and the current mouse point
					// in the canvas
					_distance_x_to_corner = eb.X - X;
					_distance_y_to_corner = eb.Y - Y;
				}
			}
		}

		// Released
		protected virtual void ButtonRelease (CanvasEventArgs args) 
		{
			if (args != null) 
			{
				EventButton eb = new EventButton (args.Event.Handle);
					
				if (eb.Button == 1)
				{
					_mouse_left_pressed = false;
					_motioned = false;
				}
			}
		}

		// converts the world coordinates to local, taking in count _distance_x_to_corner and _distance_y_to_corner variables
		public void CalculateLocalCoordinates(ref double x, ref double y)
		{
			W2i(ref x, ref y);
			x = x - _distance_x_to_corner;
			y = y - _distance_y_to_corner;
		}

		public void Deselect ()
		{
			if (_is_selected == true)
			{
				OnDeselected ();
				_is_selected = false;
			}		
		}

		// The mouse is in 
		protected virtual void EnterNotify (CanvasEventArgs args) {}

		protected void FireResizedEvent ()
		{
			if (Resized != null) { Resized (this, Width, Height); }
		}

		private void InitializeRoot ()
		{
			_childrensRoot = new CanvasGroup (this);
			_childrensRoot.X = 0;
			_childrensRoot.Y = 0;
			_childrensRoot.Show ();
		}

		// The mouse is out
		protected virtual void LeaveNotify (CanvasEventArgs args) {}
		
		public new void Move (double dx, double dy)
		{
			_dx = dx;
			_dy = dy;
			base.Move (dx, dy);
		}

		// Motion
		protected virtual void MotionNotify (CanvasEventArgs args) 
		{
			if (args != null)
			{
//				EventMotion em = new EventMotion (args.Event.Handle);
				if (_mouse_left_pressed)
				{
					_motioned = true;
				}
			}
		}

		public void NotifyMove ()
		{
			OnMoved();
			if (Moved != null)
			{
				Moved(this, _dx, _dy);
			}
			_motioned = true;
		}

		protected virtual void OnDeselected () {}

		protected virtual void OnMoved () {}
		
		protected virtual void OnSelected () {}
		
		public void ProportionalResize(UMLElement shape)
		{
			shape.Width *= _width_proportion;
			shape.Height *= _height_proportion;
			shape.Move(_dx_proportion, _dy_proportion);
		}

		// Method to redraw childrens
		protected abstract void Redraw ();

		// Redraws the sizeable-frame 
		public void RequestRedraw ()
		{
			Redraw ();
		}

		public void Select ()
		{
//			System.Console.WriteLine ("UMLWidget.Select> _is_selected = " + _is_selected); 
			if (_is_selected == false)
			{
				_is_selected = true;
				OnSelected ();
				if (Selected != null) { Selected (this); }
			}		
		}
		
		private void TriggerEvents (object o, CanvasEventArgs args)
		{
			switch (args.Event.Type)
			{
				case (EventType.EnterNotify) :
				{
					EnterNotify (args);
					
					if (EnterNotified != null) 
					{
						EnterNotified(this);
					}	
					
					break;
				}
				case (EventType.LeaveNotify) :
				{
					LeaveNotify (args);
					if (LeaveNotified != null) 
					{
						LeaveNotified(this);
					}	
					break;
				}
				case (EventType.ButtonPress) :
				{
					ButtonPress (args);
					
					EventButton eb = new EventButton (args.Event.Handle);
					if (ButtonPressed != null) 
					{
						ButtonPressed(this, eb);
					}					
					break;
				}
				case (EventType.ButtonRelease):
				{
					ButtonRelease (args);
					
					EventButton eb = new EventButton (args.Event.Handle);
					if (ButtonReleased != null) 
					{
						ButtonReleased(this, eb);
					}
					break;
				}
				case (EventType.MotionNotify) :
				{
					MotionNotify (args);
					
					EventMotion em = new EventMotion (args.Event.Handle);
					if (Motioned != null)
					{
						Motioned (this, em.X, em.Y);
					}
					break;	
				}

			}
		}
#endregion

		// Class Constants
		protected const string DEFAULT_FILL_COLOR = "white";
		protected const string DEFAULT_OUTLINE_COLOR = "black";
		
		// Internal group for drawing children items
		protected CanvasGroup _childrensRoot;

		// mouse position relative to the upper left X,Y coordinates
		private double _distance_x_to_corner, _distance_y_to_corner;
		private double _dx, _dy;

		// FLAG: (any) Control point selected?
		protected bool _is_control_point = false;
		// FLAG: Am I resizable?
		protected bool _is_resizable = true; 
		// FLAG: Am I selected?
		protected bool _is_selected = false;
		// Controls whether the layout should be redrawn when Width and Heigth are modified
		protected bool _layout_suspended = false;
		// FLAG: Is the mouse left button pressing over me?
		protected bool _mouse_left_pressed = false;
		// FLAG: Am I motioned?
		protected bool _motioned = false;
		
		// width & height save size of the rectangle where the children lives
		protected double _height;
		protected double _width;
		
		//needed for porportional resize;
		protected double _width_proportion;
		protected double _height_proportion;
		protected double _dx_proportion;
		protected double _dy_proportion;
	}
}
