/*
MonoUML.Widgets - A library for representing the Widget elements
Copyright (C) 2004  Mario Carrión
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
using Gtk;
using ExpertCoder.Xmi2;
using DI = MonoUML.DI;
using UML = ExpertCoder.Uml2;
using MonoUML.I18n;
using MonoUML.DI.Uml2Bridge;
using System;
using System.Collections;

namespace MonoUML.Widgets
{
	public class Hub
	{
		private class CentralBroadcaster : IBroadcaster
		{
			public CentralBroadcaster()
			{
				_views = new ArrayList();
			}

			public void AddView(IView view)
			{
				_views.Add(view);
			}

			public void BroadcastElementChange(object modifiedElement)
			{
				foreach(IView view in _views)
				{
					view.UpdateElement(modifiedElement);
				}
			}

			public void BroadcastElementSelection(object element)
			{
				foreach(IView view in _views)
				{
					view.SelectElement(element);
				}
			}
			
			public void BroadcastNewModel(IList newModel)
			{
				foreach(IView view in _views)
				{
					view.SetModel(newModel);
				}
			}

			public void ClearAllViews()
			{
				foreach(IView view in _views)
				{
					view.Clear();
				}
			}

			private ArrayList _views;
		}

		// The hub is a singleton; there's only one instance which is visible
		// form everywhere in the project.
		public static readonly Hub Instance = new Hub ();

		private Hub ()
		{
			_currentNotebook = null;
			_draggedElement = null;
			_filename = String.Empty;
			ClearElementsList();
			_broadcaster = new CentralBroadcaster();
			_eventQueue = new EventQueue(_broadcaster);
		}
		
		// The central broadcaster, accessible from everywhere. 
		public IBroadcaster Broadcaster
		{
			get { return _eventQueue; }
		}
		
		public UML.NamedElement DraggedElement
		{
			set
			{
				_draggedElement = value;
			}
			get
			{
				return _draggedElement;
			}
		}
		
		public void AddNewUMLElement(UML.Element element)
		{
			_elementsList.Add(element);
			_broadcaster.BroadcastNewModel(_elementsList);
			ElementChooserDialog.SetProjectElements(_elementsList);
		}

		// Adds a view to the hub; the view will be notified when events arise.
		public void AddView (IView view)
		{
			_broadcaster.AddView (view);
			Tree tree = view as Tree;
			if (tree != null)
			{
				tree.DiagramNameChanged += DiagramNameChanged;
			}
			NoteBook nbook = view as NoteBook;
			if (nbook != null)
			{
				_currentNotebook = nbook;
			}
			view.SetBroadcaster(_eventQueue);
		}
		
		private void DiagramNameChanged (object obj, DI.Diagram diagram)
		{
			try
			{
				_currentNotebook.UpdateDiagramName (diagram);
			}
			catch (Exception ex) { System.Console.WriteLine ("Exception when updating Diagram Name!"+ex.Message); }
		}

		public void CreateNewProject()
		{
			ClearElementsList();
			_broadcaster.ClearAllViews();
			_elementsList = new ArrayList();
			_filename = String.Empty;
			_broadcaster.BroadcastNewModel(_elementsList);
		}
		
		private void ClearElementsList()
		{
			_elementsList = null;
			// intentionally lose the reference to the old serializer, in order
			// to rescue some memory.
			_ser = new SerializationDriver();
			_ser.AddSerializer(new MonoUML.DI.Serialization.Serializer());
			_ser.AddSerializer(new MonoUML.DI.Uml2Bridge.Serialization.Serializer());
			_ser.AddSerializer(new ExpertCoder.Uml2.Serialization.Serializer());
		}
		
		public void CloseProject()
		{
			//TODO: Did the user do some changes?
			CreateNewProject();
		}
		
		public void DeleteElement(UML.Element element)
		{
			DeleteElement(element, true);
		}
		
		private void DeleteElement(UML.Element element, bool doBroadcast)
		{
			//System.Console.WriteLine("DeleteElement> element: {0}; broadcast: {1}", element.GetType().FullName, doBroadcast);
			bool newModel = false;
			if(_elementsList.Contains(element))
			{
				_elementsList.Remove(element);
				newModel = true;
			}
			UML.Element owner = element.Owner;
			UML.Element rootLevelElement;
			MonoUML.DI.Uml2Bridge.Uml2SemanticModelBridge bridge;
			DI.Diagram diagram;
			object e;
			int lastDeletionIndex;
			_currentDeletionIndex = -1;
			IList deleted;
			ArrayList allDeleted = new ArrayList();
			for (int i = 0; i < _elementsList.Count; i ++)
			{
				if (_currentDeletionIndex != i)
				{
					e = _elementsList [i];
					if ( (rootLevelElement = e as UML.Element) != null)
					{
						lastDeletionIndex = _currentDeletionIndex;
						_currentDeletionIndex = i;
						deleted = rootLevelElement.DeleteAllReferencesTo(element);
						allDeleted.AddRange (deleted);
						foreach (UML.Element deletedElement in deleted)
						{
							if(!object.ReferenceEquals(deletedElement, element))
							{
								DeleteElement(deletedElement, false);
							}
						}
						_currentDeletionIndex = lastDeletionIndex;
					}
				}
			}
			for (int i = 0; i < _elementsList.Count; i ++)
			{
				e = _elementsList [i];
				if ( (diagram = e as DI.Diagram) != null)
				{
					bridge = diagram.Namespace as MonoUML.DI.Uml2Bridge.Uml2SemanticModelBridge;
					foreach(object deletedElement in allDeleted)
					{
						if (bridge != null && object.ReferenceEquals(bridge.Element, deletedElement))
						{
							_elementsList.Remove(diagram);
							newModel = true;
						}
						else
						{
							if (diagram.DeleteRepresentations(deletedElement) && doBroadcast)
							{
								_eventQueue.BroadcastElementChange(diagram);
							}
						}
					}
				}
			}
			allDeleted = null;
			if(doBroadcast)
			{
				if (owner != null)
				{
					_eventQueue.BroadcastElementChange(owner);
				}
				if (newModel) { _broadcaster.BroadcastNewModel(_elementsList); }
			}
		}

		// this method can be used to delete graph elements and diagrams
		public void DeleteElement(DI.GraphElement representation)
		{
			bool newModel = false;
			if(_elementsList.Contains(representation))
			{
				_elementsList.Remove(representation);
				newModel = true;
			}
			foreach(object container in representation.Delete())
			{
				_broadcaster.BroadcastElementChange (container);
			}
			if (newModel) { _broadcaster.BroadcastNewModel(_elementsList); }
		}

		public IList ElementsList
		//internal IList ElementsList
		{
			get { return _elementsList; }
		}
		
		public void NewUMLDiagram()
		{
			if(_elementsList == null || _elementsList.Count == 0)
			{
				Gtk.MessageDialog md = new Gtk.MessageDialog (
					null, 
					Gtk.DialogFlags.DestroyWithParent,
					Gtk.MessageType.Info,
					Gtk.ButtonsType.Close,
					GettextCatalog.GetString ("There are no elements in the model; you should create some elements first."));
				md.Run ();
				md.Destroy();
				return;
			}
			// pick a diagram type
			DiagramTypeChooserDialog dialog = new DiagramTypeChooserDialog();
			int rtn = dialog.Run(); 
			if (rtn != Gtk.ResponseType.Accept.value__ && rtn != Gtk.ResponseType.Ok.value__) 
			{
				return;
			}
			string diagramType = dialog.Selection;
			// pick a namespace
			ElementChooserDialog chooser = new ElementChooserDialog(
				typeof(UML.Namespace), GettextCatalog.GetString ("Choose a namespace"));
			if (_eventQueue.LastSelectedElement is UML.Namespace)
			{
				chooser.SelectedObject = _eventQueue.LastSelectedElement;
			}
			if(chooser.Run() != Gtk.ResponseType.Accept.value__) return;
			UML.Namespace ns = (UML.Namespace)chooser.SelectedObject;
			// create a new diagram
			DI.Diagram newDiagram = new DI.Diagram();
			DI.SimpleSemanticModelElement bridge = new DI.SimpleSemanticModelElement();
			bridge.TypeInfo = diagramType.Replace(" ", "") + "Diagram";
			newDiagram.SemanticModel = bridge;
			Uml2SemanticModelBridge nsbridge = new Uml2SemanticModelBridge();
			nsbridge.Element = ns;
			newDiagram.Namespace =  nsbridge;
			_elementsList.Add (newDiagram);
			// broadcast the change
			_broadcaster.BroadcastNewModel(_elementsList);
			ElementChooserDialog.SetProjectElements(_elementsList);
		}
		
		public void NewUMLElement()
		{
			RootLevelElementChooserDialog dialog = new RootLevelElementChooserDialog();
			int rtn = dialog.Run();
			if (rtn == Gtk.ResponseType.Accept.value__ || rtn == Gtk.ResponseType.Ok.value__)
			{
				string elementType = dialog.Selection;
				if (elementType!=null)
				{
					AddNewUMLElement (Helper.CreateUmlElement(elementType));
				}
			}
		}
		
		public void OpenProject (ArrayList arrayList)
		{
			ClearElementsList();
			_elementsList = arrayList;
			_filename = "some-xmi.xmi";
			_broadcaster.ClearAllViews (); 
			_broadcaster.BroadcastNewModel(_elementsList);
			ElementChooserDialog.SetProjectElements (_elementsList);
		}

		public void OpenProject (string filename)
		{
			ClearElementsList();
			_elementsList = _ser.Deserialize(filename);
			_filename = filename;
			_broadcaster.ClearAllViews (); 
			_broadcaster.BroadcastNewModel(_elementsList);
			ElementChooserDialog.SetProjectElements(_elementsList);
		}
		
		public void OpenProject (System.IO.Stream xmiStream)
		{
			ClearElementsList();
			try
			{
			_elementsList = _ser.Deserialize (xmiStream);
			_filename = String.Empty;
			_broadcaster.ClearAllViews();
			_broadcaster.BroadcastNewModel(_elementsList);
			ElementChooserDialog.SetProjectElements(_elementsList);
			}
			catch (Exception ex) { System.Console.WriteLine ("no se pudo cargar"); }
		}
		
		// Returns Gtk.ResponseType.Ok if a project was opened.
		public Gtk.ResponseType OpenProject()
		{
			//TODO: i18n
			FileChooserDialog fc = new FileChooserDialog (
				GettextCatalog.GetString ("Select your project"), 
				null,
				FileChooserAction.Open);
			FileFilter filter;
			// xmi files filter
			filter = new FileFilter ();
			filter.Name = GettextCatalog.GetString ("XMI files");
			filter.AddPattern ("*.xmi");
			fc.AddFilter (filter);
			// argo files filter
			filter = new FileFilter ();
			filter.Name = GettextCatalog.GetString ("All files");
			filter.AddPattern ("*.*");
			fc.AddFilter (filter);
			// configures the buttons
			fc.AddButton (Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);
			fc.AddButton (Gtk.Stock.Open, Gtk.ResponseType.Ok);
			// runs the file chooser
			fc.SelectMultiple = false;
			Gtk.ResponseType response = (Gtk.ResponseType)fc.Run();
			string filename = fc.Filename;
			fc.Hide();
			if (response == Gtk.ResponseType.Ok)
			{
				Console.WriteLine ("Selected filename: {0}", filename);
				OpenProject (filename);
			}
			return response;
		}
		
		public void Save()
		{
			if(_filename == String.Empty)
			{
				SaveAs ();
			}
			else
			{
				Console.WriteLine ("saving " + _filename);
				_ser.Serialize(_elementsList, _filename);
			}
		}
		
		public void SaveAs ()
		{
			FileChooserDialog fc = new FileChooserDialog (
					GettextCatalog.GetString ("Save your project as"),
					null,
					FileChooserAction.Save);
			FileFilter filter;
			// xmi files filter
			filter = new FileFilter ();
			filter.Name = GettextCatalog.GetString ("XMI files");
			filter.AddPattern ("*.xmi");
			fc.AddFilter (filter);
			// configures the buttons
			fc.AddButton (Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);
			fc.AddButton (Gtk.Stock.SaveAs, Gtk.ResponseType.Ok);
			// runs the file chooser
			fc.SelectMultiple = false;
			Gtk.ResponseType response = (Gtk.ResponseType) fc.Run();
			fc.Hide ();
			if (response == Gtk.ResponseType.Ok)
			{
				string filename = fc.Filename;
				// filename can't be empty, I promise :)
				if (!filename.EndsWith(".xmi")) { filename += ".xmi"; }
				bool doSave = true;
				if (System.IO.File.Exists (filename))
				{
					MessageDialog md = new MessageDialog (null, 
						DialogFlags.DestroyWithParent,
						MessageType.Question, 
						ButtonsType.YesNo,
						GettextCatalog.GetString ("A file named {0} already exists.\nDo you want to replace it with the one you are saving?"), filename);
					ResponseType result = (ResponseType)md.Run ();
					md.Hide ();
					doSave = (result == ResponseType.Yes);
					md.Destroy ();
				}
				if (doSave)
				{
					System.Console.WriteLine ("saving as " + filename);
					_ser.Serialize (_elementsList, filename);
					_filename = filename;
				}
			}
			fc.Destroy ();
		}

		private CentralBroadcaster _broadcaster;
		//this index is used during the deletion of an element in order to avoid
		//processing twice the same model tree (every UML element in _elementsList
		//is the root of a model tree). 
		private int _currentDeletionIndex;
		private string _filename;
		private ArrayList _elementsList;
		private EventQueue _eventQueue;
		private SerializationDriver _ser;
		private NoteBook _currentNotebook;
		//
		private UML.NamedElement _draggedElement; //Used when draggind & dropping
	}
}
