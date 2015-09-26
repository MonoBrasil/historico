/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2004  Mario Fuentes <mario@gnome.cl>
Copyright (C) 2004  Mario Carrión <mario.carrion@gmail.com>
Copyright (C) 2004  Manuel Cerón <ceronman@gmail.com>
Copyright (C) 2005  Rodolfo Campero <rodolfo.campero@gmail.com>

UMLCanvas.cs: the UML canvas

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
using Gdk;
using Gtk;
using Gnome;
using System.Collections;
using DI = MonoUML.DI;
using Widgets = MonoUML.Widgets;

namespace MonoUML.Widgets.UML2
{
	public class UMLCanvas : ScrolledWindow 
	{
#region PUBLIC_API
	#region PUBLIC_CONSTRUCTORS
		public UMLCanvas (UMLDiagram diagram) : base ()
		{
			ShadowType = ShadowType.In;

			_diagram = diagram;

			//Canvas' details
			_canvas = Canvas.NewAa ();
			_canvas.Dither = Gdk.RgbDither.None;
			Canvas.NewAa();
			System.Console.WriteLine("Dither: {0}", _canvas.Dither);
			DI.Diagram diDiagram = diagram.DIDiagram;
			DI.Dimension canvasSize = diDiagram.Size;
			_canvas.WidthRequest = (int)canvasSize.Width;
			_canvas.HeightRequest = (int)canvasSize.Height;
			System.Console.WriteLine ("diDiagram.Zoom: " +diDiagram.Zoom);
			if (diDiagram.Zoom > 0)
			{
				_canvas.PixelsPerUnit = diDiagram.Zoom;
			}
			_canvas.SetScrollRegion (0.0, 0.0, 1200, 750);			
			_canvas.ButtonPressEvent += ButtonPressEventCb;
			_canvas.ButtonReleaseEvent += ButtonReleaseEventCb;
			_canvas.MotionNotifyEvent += MotionNotifyEventCb;
			//_canvas.SizeAllocated +=SizeAllocatedCb;
			
			InitBackGroup ();
			InitFrontGroup ();
			
			//Selector details
			_selector = new UMLSelector (_canvas.Root());
			_elements = new ArrayList();
			//_edges = new ArrayList();

			Add (_canvas);
			Grid = true;
			SizeAllocated += ChangeCanvasSize;
			
			//Drag&Drop
			Gtk.Drag.DestSet (_canvas, DestDefaults.All, _fromTree, Gdk.DragAction.Copy);
			_canvas.DragDataReceived += DataReceived;			
		}
	#endregion
	#region PUBLIC_PROPERTIES
		public UMLDiagram Diagram
		{
			get { return _diagram; }
		}
		
		public Widgets.NoteBook NoteBook
		{
			get { return _notebook; }
			set { _notebook = value; }
		}

		public CanvasGroup CanvasRoot
		{
			get { return _front_group; }
		}
		
		public double Zoom
		{
			get  { return _canvas.PixelsPerUnit; }
			set 
			{
				if (value <= 0) { _canvas.PixelsPerUnit = DEFAULT_ZOOM; }
				else { _canvas.PixelsPerUnit = value; }
			}
		}

		// Show GRID?
		public bool Grid
		{
			get { return _grid; }
			set
			{
				if (_grid == value)
					return;
				
				if (value)
					ShowGrid ();
				else
					HideGrid ();

				_grid = value;
			}
		}
		
		public ActionBase QueueAction
		{
			set
			{
				_queueAction = value;
			}
		}
	#endregion
	#region PUBLIC_EVENTS
		public event UMLElementNameChangedHandler ElementNameChanged = null;
	#endregion

	#region PUBLIC_METHODS
		public void AddElement (UMLElement element)
		{
			/*if (_elements.IndexOf (element) != -1)
			{
				MessageDialog md = new MessageDialog (
					null,
					DialogFlags.DestroyWithParent,
					Gtk.MessageType.Error,
					ButtonsType.Close,
					"Element is already drawn in the diagram");
				md.Run();
				md.Destroy();
			}
			else
			{*/
				element.ButtonPressed += NodeButtonPressedCb;
				element.Motioned += NodeMovedCb;
				/*
				//This will allow good UMLSelector-behavior (won't draw the rectangle)
				if (element is UMLNodeEntry)  
				{
					((UMLNodeEntry)element).EntryMoved += DisableSelectorEntryMoved;
					element.Resized += DisableSelectorResized;
				}
				*/
				element.Resized += NodeResizedCb;
				element.Selected += NodeSelected;
				//Particular Events
				if (element is UMLActor)
				{
					((UMLActor)element).NameChanged += NameChangedEvent;
				}
				_elements.Add (element);
			//}
		}

		public void Clear ()
		{
			foreach (UMLElement element in _elements)
			{
				element.Destroy ();
			}
			_elements.Clear ();
			_selector.Clear ();
		}

		// Gets the first UMLElement that is currently under the mouse pointer.
		// If the first element is the argument "ignoredElement", then looks for
		// the second one.
		internal UMLElement GetHoverElement (object ignoredElement)
		{
			UMLElement element = null;
			double x1, y1, x2, y2;
			System.Console.WriteLine (_pointer_x+"="+_pointer_y);
			foreach (UMLElement candidate in _elements)
			{
				if (object.ReferenceEquals (candidate, ignoredElement)) { continue; }
				candidate.GetBounds(out x1, out y1, out x2, out y2);
				//mario did :)
				//System.Console.WriteLine (x1+"-"+y1+"-"+x2+"-"+y2);
				if (_pointer_x > x1 && _pointer_x < x2 && _pointer_y > y1 && _pointer_y < y2)
				{
					if (x1 == 0 && y1 == 0) //Mario did. FIXME. Why can't you create the menu if you 
					{
						continue;
					}
					element = candidate;
					break;
				}
			}
			return element;
		}

		// Get a point snaped to the grid
		public void GetSnapedPoint (double x, double y, out double sx, out double sy)
		{
			if (!_grid)
			{
				sx = x;
				sy = y;
				return;
			}

			double dx = x % DEFAULT_GRID_SPACE;
			double dy = y % DEFAULT_GRID_SPACE;
			double medium = DEFAULT_GRID_SPACE / 2;
			
			if (dx < medium)
				sx = x - dx;
			else
				sx = x + (DEFAULT_GRID_SPACE - dx);

			if (dy < medium)
				sy = y - dy;
			else
				sy = y + (DEFAULT_GRID_SPACE - dy);
		}
		
		public void EnableControlPointMotion (UMLEdge e, UMLControlPoint cp)
		{
			_cp_motioned = cp;
			_umledge = e;
		}
	#endregion
#endregion
#region PRIVATE_API
		private Canvas _canvas;
		private UMLDiagram _diagram; //For knowing all the diagram attributes
		private Widgets.NoteBook _notebook; //For telling events
		private const double DEFAULT_ZOOM = 1.0; //100%
		private const string DEFAULT_FILL_COLOR = "#FFFFFF";
		private const string DEFAULT_GRID_COLOR = "#C0C0C0";//"#abc5ff";
		private const double DEFAULT_GRID_WIDTH = 0.5;//1.0
		private const double DEFAULT_GRID_SPACE = 20.0;//20.0
		
		private double _pointer_x, _pointer_y;
		
		private Gtk.TargetEntry[] _fromTree = new TargetEntry[]
			{
				new TargetEntry ("text/plain", 0, 0)
			};
		
		// Layers:
		// Group to draw background color and the grid
		private CanvasGroup _back_group;
		// Group to draw UML Objects
		private CanvasGroup _front_group;

		// Array to save components of the grid (n lines)
		private ArrayList _grid_components;
		
		//private UMLEditableField _editable_text;
		private UMLSelector _selector;
		private ArrayList _elements;
		private UMLEdge _current_edge = null;
		
		//TODO: read/write flags from GConf on starup and shutdown.
		//FLAG: Is any UMLEntry being moved?
		private bool _is_nodeentry_moved = false;
		private CanvasRect _bg;
		private bool _grid;
		private bool _snap_to_grid;
		
		private UMLElement _association_start = null;
		private UMLElement _association_end = null;
		                
		private bool _element_selected = false;
		private bool _dragging_association = false;
		// indicates whether we are creating an association
		private bool _association = false;
		private ActionBase _currentAction;
		private ActionBase _queueAction;
		
		//If this is different than null means that a 
		//new ControlPoint is being moved.
		private UMLControlPoint _cp_motioned = null;
		private UMLEdge _umledge = null;
	#region PRIVATE_METHODS
	
		private void DataReceived (object o, DragDataReceivedArgs args)
		{
			//bool sucess = false;
			//Gtk.Widget source = Gtk.Drag.GetSourceWidget (args.Context);
			/*string data = System.Text.Encoding.UTF8.GetString (args.SelectionData.Data);
			/*switch (args.Info)
			{
				case 0:
					button.Label = (System.DateTime.Now.ToString());
					sucess = true;
					break;
				case 1:
					Gtk.Layout layout = (Gtk.Layout) o;
					layout = addImage(layout, null, args.X, args.Y);
					sucess = true;
					break;
			}*/
			if (MonoUML.Widgets.Hub.Instance.DraggedElement != null)
			{
				//FIXME
				ExpertCoder.Uml2.Element element = MonoUML.Widgets.Hub.Instance.DraggedElement;
				//UML Elements
				ExpertCoder.Uml2.Actor actor;
				ExpertCoder.Uml2.UseCase uCase;
				ExpertCoder.Uml2.Class clss;
				//
				if ((clss = element as ExpertCoder.Uml2.Class) != null)
				{ 
					UMLClass umlClass = UMLClass.CreateNew (_diagram, clss);
					umlClass.Move (_pointer_x, _pointer_y);
					this.AddElement (umlClass); 
				}
				else if ((actor = element as ExpertCoder.Uml2.Actor) != null)
				{ 
					UMLActor umlActor = UMLActor.CreateNew (_diagram, actor);
					umlActor.Move (_pointer_x, _pointer_y);
					this.AddElement (umlActor); 
				}
				else if ((uCase = element as ExpertCoder.Uml2.UseCase) != null)
				{ 
					UMLUseCase umlUseCase = UMLUseCase.CreateNew (_diagram, uCase);
					umlUseCase.Move (_pointer_x, _pointer_y); 
					this.AddElement (umlUseCase);
				}
			}
			//System.Console.WriteLine (MonoUML.Widgets.Hub.Instance.DraggedElement);
			MonoUML.Widgets.Hub.Instance.DraggedElement = null;
			Gtk.Drag.Finish (args.Context, true, false, args.Time);
		}
	
		private void InitBackGroup ()
		{
			_back_group = new CanvasGroup (_canvas.Root ());
			_back_group.X = 0;
			_back_group.Y = 0;
			
			//Nicer appearence with a drawn background
			_bg = new CanvasRect (_back_group);
			_bg.X1 = _bg.Y1 = 0;
			/*_bg.X2 = _canvas.Width;
			_bg.Y2 = _canvas.Height;*/
			_bg.FillColor = DEFAULT_FILL_COLOR;

			_grid = false;
			_snap_to_grid = false;

			_grid_components = new ArrayList ();
		}

		private void InitFrontGroup ()
		{
			_front_group = new CanvasGroup (_canvas.Root ());
			_front_group.X = 0;
			_front_group.Y = 0;
		}

		private void ShowGrid (bool force)
		{
			if (force && Grid)
				Grid = false;
			Grid = true;
		}
		
		private void ShowGrid ()
		{
			if (_grid_components.Count > 0)
				return;
			CanvasLine l;
			double x = 0.0, y = 0.0;
			double width = (double) _canvas.Width;
			double height = (double) _canvas.Height;
			
			// Columns
			while (x <= width)
			{
				l = new CanvasLine (_back_group);
				_grid_components.Add (l);

				l.Points = new CanvasPoints (new double [] {x, 0, x, height});
				l.FillColor = DEFAULT_GRID_COLOR;
				l.WidthUnits = DEFAULT_GRID_WIDTH;
				x += DEFAULT_GRID_SPACE;
			}

			// Rows
			while (y <= height)
			{
				l = new CanvasLine (_back_group);
				_grid_components.Add (l);

				l.Points = new CanvasPoints (new double [] {0, y, width, y});
				l.FillColor = DEFAULT_GRID_COLOR;
				l.WidthUnits = DEFAULT_GRID_WIDTH;
				y += DEFAULT_GRID_SPACE;
			}
		}

		private void HideGrid ()
		{
			if (_grid_components.Count == 0)
				return;
			
			foreach (CanvasLine l in _grid_components)
			{
				l.Destroy ();
			}

			_grid_components.Clear ();
		}

		private void MakeMenu()
		{
			Menu popupMenu = new Menu();
			UMLElement hoverElem = GetHoverElement (null);
			IList options = null;
			// if the user right-clicked on the diagram, ask the diagram for options
			if (hoverElem == null)
			{
				options = Diagram.GetContextMenuOptions ();
			}
			else
			{
				options = hoverElem.GetContextMenuOptions ();
			}
			if (options != null)
			{
				MenuItem item;
				foreach (ActionBase action in options)
				{
					action.SetCanvas (this);
					item = new MenuItem (action.Name);
					item.Activated += new EventHandler (action.SelectedHandler);
					popupMenu.Append (item);
					item.Show ();
				}
				popupMenu.Popup ();
			}
		}
		
		internal void StartActionExecution (ActionBase action)
		{
			CreateNodeAction createNode;
			CreateEdgeAction createEdge;
			if ((createNode = action as CreateNodeAction) != null)
			{
				createNode.Position.X = _pointer_x;
				createNode.Position.Y = _pointer_y;
				createNode.Execute ();
			}
			else if ((createEdge = action as CreateEdgeAction) != null)
			{
				_currentAction = createEdge;
				_association = true;
				_association_start = GetHoverElement (null);
			}
		}

		//temporal - for testing
		private void AssociationActivatedCb (object o, EventArgs args)
		{
			_association = true;
		} 
		
		private void ButtonPressEventCb (object obj, ButtonPressEventArgs args)
		{
			_canvas.WindowToWorld(args.Event.X, args.Event.Y, out _pointer_x, out _pointer_y);
			//this prevents bad behavior caused by double clicking and right or middle click
			if (args.Event.Type == EventType.ButtonPress && args.Event.Button == 1) 
			{
				//System.Console.WriteLine ("UMLCanvas> ButtonPressEventCb");
				if (!_association && !_element_selected && !_selector.Empty)
				{
					//System.Console.WriteLine ("Current {0} Selected: {1}", _current_element, _element_selected);
					if (_selector.FirstElement != null && !_selector.FirstElement.UMLControlPoint)
					{
						_selector.CleanSelection();
					}
				}
				else if (_queueAction != null)
				{
					CreateNodeAction action = _queueAction as CreateNodeAction;
					if (action != null)
					{ 
						action.Position.X = _pointer_x;
						action.Position.Y = _pointer_y;
						action.Execute ();
						_element_selected = true;
					}
					else
					{
						System.Console.WriteLine ("Queued action is not a node action");
					}
					_queueAction = null;
				} 
									
				if (!_element_selected)
				{
					System.Console.WriteLine ("Should start creating the rectangle!");
					//this prevents bad behavior when window is resized
					_selector.StartSelection (_pointer_x, _pointer_y);
				}
				_element_selected = false;
				_association = false;
			}
			else if (args.Event.Button == 3) 
			{
				MakeMenu ();
			}
		}
		
		//Called from UMLNodeEntry.EntryMoved, event added when AddElement
		private void DisableSelectorEntryMoved (object obj, UMLEntry entry, double dx, double dy)
		{
			_is_nodeentry_moved = true;
		}
		
		//Called from UMLElement.Resized, event added when AddElement
		private void DisableSelectorResized (object obj, double w, double h)
		{
			//System.Console.WriteLine ("[UMLCanvas] DisableSelectorResized()");
			_is_nodeentry_moved = true;
		}
				
		private void ButtonReleaseEventCb (object obj, ButtonReleaseEventArgs args)
		{		
			_selector.StopSelection (_elements);
			_is_nodeentry_moved = false;
			
			if (_dragging_association)
			{
				_dragging_association = false;
				//TODO: Fix this
//				_current_edge.CalculateToElement ();			
			}
			if (_cp_motioned != null)
			{
				_cp_motioned.ForceRelease (args);
				_cp_motioned = null;
				_umledge = null;
			}
		}
      
		private void MotionNotifyEventCb (object obj, MotionNotifyEventArgs args)
		{
			_canvas.WorldToWindow (args.Event.X, args.Event.Y, out _pointer_x, out _pointer_y); 
			if (_cp_motioned != null && _umledge != null)
			{
				_cp_motioned.ForceMove (_pointer_x, _pointer_y);
			}
			if (_dragging_association)
			{
				//this prevents bad behavior when window is resized
				//TODO: create an association 
//				_current_edge.DisplayEdge(_pointer_x, _pointer_y);
			}
			else if (_is_nodeentry_moved == false)
			{
				//this prevents bad behavior when window is resized
				_selector.DrawSelection (_pointer_x, _pointer_y);
			}
		}
		
		// Verify if the current element was the last selected element
		private void NodeButtonPressedCb (object obj, Gdk.EventButton eb)
		{
			//System.Console.WriteLine ("NodeButtonPressedCb");
			//FIXME. When moving any UMLNodeEntryContained's UMLEntry
			if (_association)
			{
				if (_association_start == null)
				{
					_association_start = (UMLElement) obj;
					System.Console.WriteLine ("NodeButtonPressedCb> set association start.");
				}
				else if (_association_end == null)
				{
					//System.Console.WriteLine ("NodeButtonPressedCb> set association end.");
					_association_end = (UMLElement) obj;
					CreateEdgeAction createEdge = (CreateEdgeAction)_currentAction;
					createEdge.FromElement = _association_start;
					createEdge.ToElement = _association_end;
					createEdge.Execute ();
					_association = false;
					_association_start = _association_end = null;
				}
			}
			else 
			{
				_selector.Select ((UMLElement) obj);
				_element_selected = true;
			}
		}

		private void NodeMovedCb (object obj, double px, double py)
		{
			UMLNode shape = obj as UMLNode;
			if (shape == null) return;
	
			if ( !_dragging_association && shape.IsMouseLeftPressed && !shape.UMLControlPoint)
			{
				_selector.MoveSelection(px, py);
				if (px + shape.Width > _canvas.Width)
				{
					_canvas.Width = (uint)(px + shape.Width);
					_bg.X2 = _canvas.Width;
				}
				if (py + shape.Height >_canvas.Height)
				{
					_canvas.Height = (uint)(py + shape.Height);
					_bg.Y2 = _canvas.Height;
				}
				/*
				//if (_grid)  
				//	ShowGrid (true);
				*/
        		//System.Console.WriteLine("algo se esta moviendo mas alla del canvas");
        		//System.Console.WriteLine("X{0}, Y{0}",_bg.X2,_bg.Y2);    
			}			
		}
		
		private void NodeResizedCb (object obj, double width, double height)
		{
			//UMLNode shape = (UMLNode) obj;
			_selector.ResizeSelection(width, height);
		}
		
		private void EdgeControlMovedCb(object obj)
		{
			_selector.StopSelection(null);
		}
		//Called when the user resizes its container used for good behavior
		private void ChangeCanvasSize (object sender, Gtk.SizeAllocatedArgs e)
		{
			//FIXME. this must be changed, in order to allow user-resizing
			_canvas.Width = (uint) e.Allocation.Width;
			_canvas.Height = (uint) e.Allocation.Height;
			_bg.X2 = _canvas.Width;
			_bg.Y2 = _canvas.Height;
			_canvas.SetScrollRegion (0.0, 0.0,
					(double) e.Allocation.Width, (double) e.Allocation.Height);
			if (_grid)
				ShowGrid (true);
		}
		
		//Activated when the element is selected
		private void NodeSelected (object obj)
		{
			_element_selected = true;
			//System.Console.WriteLine ("Object selected: "+obj);
			if (_notebook != null)
			{	
				UMLElement gnode = (UMLElement) obj;
				_notebook.BroadcastElementSelection (gnode.GraphElement);
			}
		}
		
		private void NameChangedEvent (object obj, string new_name)
		{
			System.Console.WriteLine ("UMLCANVAS. Name changed: "+new_name);
			if (ElementNameChanged != null)
			{
				ElementNameChanged (obj, new_name);
			}	
		}

	#endregion
	}
}
#endregion

