/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2004  Mario Carrión <mario.carrion@gmail.com>

UMLEditableField.cs: The really editable widget

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
using Gtk;
using Gdk;

namespace MonoUML.Widgets.UML2
{
	
	public class UMLEditableField : UMLWidget
	{
	
		public UMLEditableField (UMLEntry entry, bool resizable) : base ((CanvasGroup) entry) 
		{
			_textwidget = new CanvasWidget ((CanvasGroup) entry);
			_entry = entry;
			_is_resizable = resizable; 
			_textview = new TextView ();
			TextBuffer tb = _textview.Buffer;
			tb.Text = _entry.Text;
						
			double x = entry.TextX, y = entry.TextY;
			_textwidget.W2i (ref x, ref y);
			X = x + entry.X;
			Y = y + entry.Y;
			
			_textwidget.Widget = _textview;
			_textwidget.Width = entry.TextWidth + 10;//FIXME?
			_textwidget.Height = entry.TextHeight + 10; //FIXME?
			_textview.KeyReleaseEvent += LookingEsc;
			_textview.Show ();
			Width = _textwidget.Width;
			Height = _textwidget.Height;
			//TODO: Dejar esto como estaba
			//CreateUMLControlPointGroup ();
			
			Resized += ElementResized;
			_entry.Root.CanvasEvent += ClickCanvasEvent;
		}
		
		public string Text
		{
			get  { return _textview.Buffer.Text;  }
		}

		protected override void Redraw ()
		{
			if (_textwidget != null)
			{
				if (_first_time_off == true)
				{
					_textwidget.X = X;
					_textwidget.Y = Y;
					_textwidget.Width = (int) Width;
					_textwidget.Height = (int) Height;
				}
				else if (_first_time_off == false) { _first_time_off = true; }
			}
		}

		protected new void Hide ()
		{
			_textview.HideAll ();
			base.Hide ();
		}
		
		protected override void Finalize ()
		{
			_textview.Destroy ();
			_textwidget.Destroy ();
		}

		//Did the user press ESC? ESC = Cancel
		private void LookingEsc (object o, KeyReleaseEventArgs args)
		{
			EventKey evnt = args.Event;
			if (evnt.Key.Equals (Gdk.Key.Escape))
			{
				_entry.Root.CanvasEvent -= ClickCanvasEvent;
				Hide ();
				_entry.Show ();
			}
		}
		
		//Used for saving the changes when losing focus. Called from event
		private void ClickCanvasEvent (object obj, CanvasEventArgs args)
		{
			EventButton eb = new EventButton (args.Event.Handle);
			if (args.Event.Type == EventType.ButtonPress)
			{
				if (_is_control_point != true)
				{
					HideMe (eb);
				}
			}
			else if (args.Event.Type == EventType.ButtonRelease)
			{
				HideMe (eb);
				_was_resized = false;
			}
		}
		
		private void HideMe (EventButton eb)
		{
			double x = 0, y = 0;
			I2w(ref x, ref y);
			if (eb.Button == 1)
			{
				if (eb.X > x || eb.Y > y || x > eb.X || y > eb.Y)
				{
					if (_first_canvas_double_click == false) { _first_canvas_double_click = true; }
					else if (_was_resized == false) 
					{
						_entry.SetChanges (this);
						_entry.Root.CanvasEvent -= ClickCanvasEvent;
						Hide ();
						_textwidget.Destroy ();
					}
				}
			}
		}
		
		//The element was resized. Called from event
		private void ElementResized (object obj, double w, double h)
		{
			_was_resized = true;
		}
		
		private CanvasWidget _textwidget;
		private UMLEntry _entry;
		private TextView _textview;
		//FLAGS
		private bool _first_time_off = false; //For stoping bad first-time-showed behavior
		private bool _first_canvas_double_click = false;
		private bool _was_resized = false;
	}
}
