/*
MonoUML.Widgets - A library for representing the Widget elements
Copyright (C) 2004  Mario Carrión

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
using Gtk;
using Gnome;
using System;
using System.Collections;
using DI = MonoUML.DI;
using Uml2 = MonoUML.Widgets.UML2;
using TBar = MonoUML.Widgets.Toolbar;

namespace MonoUML.Widgets
{
	public class NoteBook : Gtk.Notebook, IBroadcaster, IView
	{
	
		public NoteBook (CanvasArea canvasArea) : this ()
		{
			_canvasArea = canvasArea;
		}

		private NoteBook () : base()
		{
			Scrollable = true;
			_diagramsKey = new ArrayList ();
			_toolbarsKey = new Hashtable ();
			SwitchPage += OnDriagramNotebookSwitchPage;
		}
		
		public MonoUML.Widgets.UML2.UMLDiagram CurrentDiagram
		{
			get
			{
				return _current == null ? null : _current.Diagram;
			}
		}

		// Shows a diagram at the NoteBook
		public void ShowDiagram (DI.Diagram diagram)
		{
			bool created = false;
			if (!_diagramsKey.Contains (diagram))
			{
				_diagramsKey.Add (diagram);
				Uml2.UMLDiagram uDiagram = Uml2.UMLDiagram.CreateFor (diagram, this);
				AppendPage (uDiagram.UMLCanvas, new NoteBookLabel (this, diagram));
				CreateToolbar (diagram, uDiagram);
				created = true;
			}
			for (int nbpages = 0; nbpages < NPages; nbpages++)
			{
				Uml2.UMLCanvas canvas = (Uml2.UMLCanvas) GetNthPage (nbpages);
				if (canvas.Diagram.DIDiagram == diagram)
				{
					Page = nbpages;
					if (created == false)
					{
						SetCurrentCanvas (canvas.Diagram.DIDiagram, canvas);
					}
					break;
				}
			}
		}
		
		public void UpdateDiagramName (DI.Diagram diagram)
		{
			System.Console.WriteLine ("New name: "+diagram.Name);
			if (_diagramsKey.Contains (diagram))
			{
				for (int nbpages = 0; nbpages < NPages; nbpages++)
				{
					Uml2.UMLCanvas canvas = (Uml2.UMLCanvas) GetNthPage (nbpages);
					if (canvas.Diagram.DIDiagram == diagram)
					{
						NoteBookLabel label = (NoteBookLabel) GetTabLabel (canvas);
						if (label != null)
						{
							label.Label = diagram.Name;
						}
						else
						{
							System.Console.WriteLine ("NoteBookLabel: Not found!!");
						}
						break;
					}
				}
			}
			else
			{
				System.Console.WriteLine ("UpdateDiagramName ERROR: diagram not found");
			}
			//_diagramsKey.Add (diagram);
		}
		
		#region Event handlers
		private void OnDriagramNotebookSwitchPage (object o, SwitchPageArgs args)
		{
			Uml2.UMLCanvas canvas = (Uml2.UMLCanvas) CurrentPageWidget;
			SetCurrentCanvas (canvas.Diagram.DIDiagram, canvas);
			_current = (Uml2.UMLCanvas) CurrentPageWidget;
			canvas.QueueAction = null;
		}
		
		private void OnGridToggled (object o, EventArgs args)
		{
			ToggleToolButton tb = (ToggleToolButton) o;
		
			if (tb.Active)
			{
				ShowDiagramGrid ();
			}
			else
			{ 
				HideDiagramGrid ();
			}
		}
		#endregion
		
		public void ShowDiagramGrid ()
		{
			if (_current == null)
			{
				return;
			}
			_current.Grid = true;
		}
		
		public void HideDiagramGrid ()
		{
			Console.WriteLine ("Current Page: {0}", Page);
			if (_current == null)
			{
				return;
			}
			Console.WriteLine ("Set Grid to False");
			_current.Grid = false;
		}
		
		private void CreateToolbar (DI.Diagram diagram, Uml2.UMLDiagram uDiagram)
		{
			TBar.ToolbarBase tbar = null;
			string diagramType = ((DI.SimpleSemanticModelElement) diagram.SemanticModel).TypeInfo.ToLower ();
			switch (diagramType)
			{
				case "classdiagram":
					tbar = new TBar.ToolbarClass (uDiagram);
					break;
				case "usecasediagram":
					tbar = new TBar.ToolbarUseCase (uDiagram);
					break;
				default:
					System.Console.WriteLine ("Unexpected diagram type.");
					break;
			}
			tbar.ButtonGrid.Toggled += OnGridToggled;
			_toolbarsKey.Add (diagram, tbar);
			_canvasArea.PackStart (tbar, false, false, 0);
		}

		// Removes all the pages
		private void RemoveAll()
		{
			//int currentPages = NPages;
			//for (int i = 0; i < currentPages; i++)
			//{ 
			//	RemovePage (0);
			//}
			while (_diagramsKey.Count > 0)
			{
				DI.Diagram diagram = (DI.Diagram) _diagramsKey[0];
				RemoveDiagram (diagram);
			}
			_diagramsKey.Clear();
			_toolbarsKey.Clear ();
		}
		
		public void RemoveDiagram (DI.Diagram diagram)
		{
			if (_diagramsKey.Contains(diagram))
			{
				for (int nbpages = 0; nbpages < NPages; nbpages++)
				{
					Uml2.UMLCanvas canvas = (Uml2.UMLCanvas) GetNthPage (nbpages);
					if (canvas.Diagram.DIDiagram == diagram)
					{
						RemovePage(nbpages);
						canvas.Destroy ();
						break;
					}
				}
				if (NPages == 0)
				{
					_canvasArea.Remove ((TBar.ToolbarBase) _toolbarsKey [diagram]);
				}
				_diagramsKey.Remove (diagram);
				_toolbarsKey.Remove (diagram);
			}
		}
		
		private void SetCurrentCanvas (DI.Diagram diagram, Uml2.UMLCanvas canvas)
		{
			TBar.ToolbarBase tbar = (TBar.ToolbarBase) _toolbarsKey [diagram];
			if (tbar != null)
			{
				_canvasArea.PackStart (tbar, false, false, 0);
				((TBar.ToolbarBase)tbar).Grid = canvas.Grid;
			}
		}

		public void SetBroadcaster(IBroadcaster hub)
		{
			_hub = hub;
		}

		#region IView implementation
		void IView.Clear ()
		{
			RemoveAll ();
		}

		void IView.SelectElement (object selectedElement)
		{
			DI.Diagram diagram;
			if ((diagram = selectedElement as DI.Diagram) != null)
			{
				ShowDiagram (diagram);
				ShowAll ();
			}
		}

		void IView.UpdateElement (object modifiedElement)
		{
			if (modifiedElement is DI.Diagram)
			{
				Uml2.UMLCanvas page;
				for(int i = 0; i < base.NPages; i ++)
				{
					page = (Uml2.UMLCanvas) base.GetNthPage (i);
					if (object.ReferenceEquals(page.Diagram.DIDiagram, modifiedElement))
					{
						page.Diagram.Refresh ();
					}
				}
			}
			Uml2.UMLDiagram currentDiagram = CurrentDiagram;
			if (currentDiagram != null)
			{
				currentDiagram.UpdateElement (modifiedElement);
			}
		}

		void IView.SetBroadcaster (IBroadcaster hub)
		{
			_hub = hub;
		}

		void IView.SetModel(System.Collections.IList modelElements)
		{
			RemoveAll ();
		}
		#endregion
		
		#region INTERFACE_IBroadcasterInterface
		public void BroadcastElementChange (object modifiedElement)
		{
			_hub.BroadcastElementChange(modifiedElement);
		}
		
		public void BroadcastElementSelection (object element)
		{
			_hub.BroadcastElementSelection (element);
		}
		#endregion

		private CanvasArea _canvasArea;
		private Uml2.UMLCanvas _current = null;
		private ArrayList _diagramsKey;
		private Hashtable _toolbarsKey;
		private IBroadcaster _hub;
	}
}
