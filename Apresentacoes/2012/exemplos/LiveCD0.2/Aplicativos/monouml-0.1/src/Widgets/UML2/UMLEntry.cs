/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2004  Mario Carrión <mario.carrion@gmail.com>

UMLEntry.cs: The editable entry.

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
using Gtk;
using System.Text.RegularExpressions;
using DI = MonoUML.DI;

namespace MonoUML.Widgets.UML2
{

	// A text widget
	//TODO: Split this class into 4 classes:
	// UMLEntry (abstract) - base class for all other entries.
	// UMLContainedEntry   - an entry meant to be fixed within a container.
	// UMLFreeEntry        - an entry meant to be placed anywhere in the diagram.
	// UMLRelativeEntry    - like a UMLFreeEntry, but its position depends on
	//    another graph node; this is needed for associations, because an
	//    association label is placed relative to an intermediate node, and when
	//    the label is moved, the moved element is actually this intermediate node.
	//    For more information, see UMLNamedEdge.
	public sealed class UMLEntry : CanvasGroup
	{

		public UMLEntry
		(
			UMLElement owner,
			DI.GraphNode graphNode,
			CanvasGroup group,
			bool isMovable,
			string text
		)	: this (owner, graphNode, group, isMovable, text, null) 
		{ }

		public UMLEntry
		(
			UMLElement owner,
			DI.GraphNode graphNode,
			CanvasGroup group,
			bool isMovable,
			string text,
			string fontModifier
		)	: base (group)
		{
			_graphNode = graphNode;
			Movable = isMovable;
			_owner = owner;
			_text = new CanvasText (this); 
			_text.Text = (text == null ? System.String.Empty : text);
			_text.FillColor = "black";
			_text.CanvasEvent += EntryEvents;
			_text.Justification = Gtk.Justification.Left;
			_text.Anchor = Gtk.AnchorType.NorthWest;
			this.FontModifier = fontModifier;
			_root = group;
			X = _movable ? _graphNode.GlobalPosition.X : _graphNode.Position.X;
			Y = _movable ? _graphNode.GlobalPosition.Y : _graphNode.Position.Y;
			Show ();
		}
		
		public new double X
		{
			get  { return base.X; }
			set
			{
				base.X = value;
				_text.X = 0;
				// Positions in the coordinate system are always relative to
				// the surrounding container. See UML 2.0 Diagram Interchange
				// Specification (ptc/03-09-01), paragraph 8.7 (page 13).
				_graphNode.Position.X = value - (_movable ? _graphNode.Container.GlobalPosition.X : 0); 
			}
		}
		
		public new double Y
		{
			get { return base.Y; }
			set 
			{
				base.Y = value;
				_text.Y = 0;
				// See comment on property "X" 
				_graphNode.Position.Y = value - (_movable ? _graphNode.Container.GlobalPosition.Y : 0); 
			}
		}
		
		public CanvasGroup Root 
		{
			get { return _root; } 
		}
		
		//Is the editable text resizable?
		public bool Resizable
		{
			get { return _resizable; }
			set { _resizable = value; }
		}
		
		public bool Editable
		{
			get { return _editable; }
			set { _editable = value; }
		}
		
		public string FontModifier
		{
			get { return _fontModifier; }
			set
			{
				_fontModifier = value;
				string fontName = _graphNode.Property[DI.StandardProperty.FontFamily];
				string fontSize = _graphNode.Property[DI.StandardProperty.FontSize];
				string font = fontName + " " + fontSize 
					+ (_fontModifier != null ? " " + _fontModifier : "");
				_text.Font = font;
			}
		}
		
		public bool Movable
		{
			get { return _movable; }
			set
			{
				_movable = value;
//				_movable_container = !_movable;
			}
		}
		
		//Used when the entry is contained in some other UMLElement
		public bool MovableContainer
		{
			get { return _movable_container; }
			set 
			{
				_movable_container = value;
				_movable = !_movable_container;
			}
		}

		public string Text
		{
			get { return _text.Text; }
			set
			{
				_text.Text = value;
				_graphNode.Size.Height = TextHeight; 
				_graphNode.Size.Width = TextWidth; 
			}
		}
		
		public double TextX
		{
			get { return _text.X; }
		}
		
		public double TextY
		{
			get { return _text.Y; }
		}
		
		/*public CanvasText CanvasText 
		{
			get { return _text; } 
		}*/
		
		public double TextWidth
		{
			get { return _text.TextWidth; }
		}
		
		public double TextHeight
		{
			get { return _text.TextHeight; }
		}
		
		public string Font
		{
			get { return _text.Font; }
			set
			{
				if (value != null)
				{
					_text.Font = value;
				}
			}
		}
		
		public event UMLEntryMovedHandler Moved = null;
		public event UMLElementResizedHandler Resized = null;
		public event UMLElementNameChangedHandler TextChanged = null; 

		public void EntryEvents (object obj, CanvasEventArgs args)
		{
			if (!_movable) return;
			switch (args.Event.Type)
			{
				case (EventType.TwoButtonPress) :
				{
					EventButton eb = new EventButton (args.Event.Handle);

					switch (eb.Button)
					{
						//Creating the editable text!!
						case 1:
						{
							if (_editable == true)
							{
								UMLEditableField edit = new UMLEditableField (this, _resizable);
								edit.Resized += EditableFieldResizedTrigger;
								_text.Hide ();
								_owner.Select ();
							} 
						}
						break;
					}
					_mouse_left_pressed = false;
					break;
				}

				case (EventType.ButtonPress) :
				{
					EventButton eb = new EventButton (args.Event.Handle);
					switch (eb.Button)
					{
						case 1:
						{
						 	_mouse_left_pressed = true; 
						 	_dx = eb.X - X;
						 	_dy = eb.Y - Y;
						 	// if we don't deselect the owner and it was previously
						 	// selected, the UML Selector gets activated.
							_owner.Deselect ();
						 	_owner.Select ();
						 	break; 
						 }
					}
					break;
				}

				case (EventType.ButtonRelease) :
				{
					EventButton eb = new EventButton (args.Event.Handle);
					switch (eb.Button)
					{
						case 1: 
						{ 
							_mouse_left_pressed = false;
							_owner.Deselect ();
							break;
						}
					}
					break;
				}
				
				case (EventType.MotionNotify):
				{
					if (_mouse_left_pressed)
					{
						if (_movable == true || _movable_container == true)
						//if (_movable == true)
						{
							EventMotion em = new EventMotion (args.Event.Handle);
							if (_dx > 0 && _dy > 0)
							{
								X = em.X - _dx;
								Y = em.Y - _dy;
							}
							_text.X = _text.Y = 0;
							if (_movable == true) //The UMLEntry doesn't care if is inside or outside the element 
							{
								if (Moved != null) { Moved (this, em.X, em.Y); }
							}
							else if (_movable_container == true) //The entry MUST BE inside the element
							{ 
								if (Moved != null) { Moved (this, X-UMLEntry.DEFAULT_SPACE, Y-UMLEntry.DEFAULT_SPACE); }
							}
							_owner.Select ();
						}
					}
					break;
				}
			}
		}
		
		public new void Show ()
		{
			_text.Show ();
			base.Show ();
		} 
		
		// Set the changes
		public void SetChanges (UMLEditableField editable)
		{
			double x = editable.X, y = editable.Y;
			I2w (ref x, ref y);
			X = x;
			Y = y;
			System.Console.WriteLine ("setChanges");
			_text.Text = (string) editable.Text.Clone ();
			_text.Show ();
			_text.RaiseToTop ();
			editable.Destroy ();
			_owner.Deselect ();
			if (TextChanged != null)
			{
				TextChanged (this, _text.Text);
			}
			//_edit = null;
		}
		
		private void EditableFieldResizedTrigger (object obj, double w, double h)
		{
			//System.Console.WriteLine ("[UMLEntry] UMLEditableField.Resized");
			if (Resized != null)
			{
				Resized (this, w, h);
			}
		}
		
		public static int DEFAULT_SPACE = 7; //Used for Use Cases
		protected const string DEFAULT_FONT = "Verdana";
		protected const string DEFAULT_SIZE = "10";
		private DI.GraphNode _graphNode;
		//		
		private CanvasText _text;
		//private UMLEditableField _edit;
		private CanvasGroup _root;
		// FLAG: Is the left-mouse-button pressed?
		private bool _mouse_left_pressed;
		// FLAG: Allow resizing while editing?
		private bool _resizable = true;
		// FLAG: Allow moving?
		private bool _movable = true;
		// FLAG: Allow moving (used when it's contained in other UMLElement)
		private bool _movable_container = false;
		// FLAG: Allow editing
		private bool _editable = true;
		private string _fontModifier;
		private double _dx, _dy; //Relative distance
		private UMLElement _owner;
	}

}
