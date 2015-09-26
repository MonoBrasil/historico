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
using Gdk;
using Gtk;
using System;
using System.Collections;
using ExpertCoder.Xmi2;
using UML = ExpertCoder.Uml2;
using DI = MonoUML.DI;
using MonoUML.Widgets.UML2;
using MonoUML.DI.Uml2Bridge;

namespace MonoUML.Widgets
{
	public delegate void DiagramNameChanged (object obj, DI.Diagram diagram);
	
	public class Tree : Gtk.TreeView, IView
	{
		// Default constructor
		public Tree() : base()
		{ 
			_store = new TreeStore(typeof(Gdk.Pixbuf), typeof (string), typeof(bool), typeof (string));
			base.Model = _store; 
			CellRendererText ct = new CellRendererText ();
			ct.Edited += OnTreeViewCellEdited;
			CellRendererPixbuf cb = new CellRendererPixbuf ();
			TreeViewColumn column = new TreeViewColumn ();
			column.PackStart(cb, false);
			column.PackStart(ct, false);
			column.AddAttribute(cb, "pixbuf", 0);
			column.AddAttribute(ct, "text", 1);
			column.AddAttribute(ct, "editable", 2);
			AppendColumn(column);
			AppendColumn("ElementType", new CellRendererText(), "text", 3);
			GetColumn(1).Visible = false; //Hides the "what-am-I" column
			HeightRequest = 350;
			WidthRequest = 180;
			HeadersVisible = false;
			//
			_table = new Hashtable();
			_key = 0; // hastable key
			//
			//Drag&Drop
			base.Selection.Changed += new EventHandler(TreeSelectionChanged);
			Gtk.Drag.SourceSet (this, Gdk.ModifierType.Button1Mask, _fromTree, DragAction.Copy);
			//Gtk.Drag.SourceSet (this, Gdk.ModifierType.Button1Mask, FromImage, DragAction.Copy | DragAction.Move);
			DragDataGet += Data_Get;
			DragBegin += Drag_Begin;
		}
		
		public event DiagramNameChanged DiagramNameChanged;
		
		//PREVIEW!!
		private void Drag_Begin (object o, DragBeginArgs args)
		{
			Gdk.Pixbuf icon = MonoUML.IconLibrary.PixbufLoader.GetIconDnD ("unknown_dnd");
			object element = GetSelectedElement();
			object modifiedElement = element;
			DI.Diagram diagram;
			DI.GraphElement graphElement;
			UML.NamedElement umlNamedElement;
			if ((diagram = element as DI.Diagram) != null)
			{
				icon = MonoUML.IconLibrary.PixbufLoader.GetIconDnD ("no_dnd");
				System.Console.WriteLine ("diagram");
			}
			else if ((graphElement = element as DI.GraphElement) != null)
			{
				Uml2SemanticModelBridge bridge = graphElement.SemanticModel as Uml2SemanticModelBridge;
				if (bridge != null)
				{
					if((umlNamedElement = bridge.Element as UML.NamedElement) != null)
					{
						icon = MonoUML.IconLibrary.PixbufLoader.GetIconDnD ("no_dnd");
						System.Console.WriteLine ("DI.GraphElement");
						//umlNamedElement.Name = newName;
						//modifiedElement = umlNamedElement;
					}
				}
			}
			else if ((umlNamedElement = element as UML.NamedElement) != null)
			{
				string label, elementType, iconName;
				GetLabelAndType (umlNamedElement, out label, out elementType);
				iconName = elementType.ToLower() + "_dnd.png";
				icon = MonoUML.IconLibrary.PixbufLoader.GetIcon (iconName);
				System.Console.WriteLine ("umlNamedElement");
				Hub.Instance.DraggedElement = umlNamedElement;
				//umlNamedElement.Name = newName;
			}
			Gtk.Drag.SetIconPixbuf (args.Context, icon , 0, 0);
		}

		private void Data_Get (object o, DragDataGetArgs args)
		{
			Atom [] targets = args.Context.Targets;
			args.SelectionData.Set (targets [0], 8,  System.Text.Encoding.UTF8.GetBytes ("sending message from tree"));
		}

		private Gtk.TargetEntry[] _fromTree = new TargetEntry[]
			{
				new TargetEntry ("text/plain", 0, 0)
			};
		//PREVIEW!!
		#region IView implementation
		void IView.Clear()
		{
			_table.Clear ();
			_store.Clear ();
		}

		void IView.SelectElement(object selectedElement)
		{
			_handlingEvent = true;
			Select(selectedElement);
			_handlingEvent = false;
		}

		void IView.UpdateElement(object modifiedElement)
		{
			RefreshElement(modifiedElement);
		}

		void IView.SetBroadcaster(IBroadcaster hub)
		{
			_hub = hub;
		}

		void IView.SetModel(System.Collections.IList modelElements)
		{
			Draw(modelElements);
		}
		#endregion

		private string CurrentKey
		{
			get { return _key.ToString(); }
		}
		
		// must be set before calling Draw
		public bool IsReadOnly
		{
			get { return _isReadOnly; }
			set { _isReadOnly = value; }
		}

		public void Draw (IList elementList)
		{
			string diagramType;
			Gdk.Pixbuf icon;
			TreeIter iter;
			UML.Element umlElement;

			_table.Clear ();
			_table = null;
			_table = new Hashtable ();
			_store.Clear();
			_namespace2iter = null;
			_namespace2iter = new Hashtable ();
			_diagrams = null;
			_diagrams = new ArrayList();

			foreach(object o in elementList)
			{
				if(o is DI.Diagram)
				{
					_diagrams.Add (o);
				}
				else if((umlElement = o as UML.Element) != null)
				{
					_table.Add(GetNewKey(), o);
					DrawUmlElement(umlElement, TreeIter.Zero, false);
				}
			}
			foreach(DI.Diagram d in _diagrams)
			{
				diagramType = ((DI.SimpleSemanticModelElement)d.SemanticModel).TypeInfo;
				icon = GetTreeIcon(diagramType);
				object ns = ( (Uml2SemanticModelBridge)d.Namespace).Element;
				_table.Add(GetNewKey(), d);
				iter = (TreeIter)_namespace2iter[ns];
				iter = _store.AppendValues(iter, icon, d.Name, !_isReadOnly, CurrentKey); //Editable
				DrawLeaves(d, iter);
				//iter.Free (); 
			}
			iter = Gtk.TreeIter.Zero;//#
			_namespace2iter = null;//#
		}

		// Draws all the leaves
		private void DrawLeaves(DI.Diagram diagram, TreeIter iter)
		{
			DI.GraphNode node;
			foreach(DI.DiagramElement elem in diagram.Contained)
			{
				node = elem as DI.GraphNode;
				if(node != null)
				{
					if(node.SemanticModel is Uml2SemanticModelBridge)
					{
						DrawLeaf(node, iter);
					} 
				}
			}
		}

		// Draws the element itself
		private void DrawLeaf(DI.GraphNode node, TreeIter iter)
		{
			string elementType, label;
			GetLabelAndType(Helper.GetSemanticElement(node), out label, out elementType);
			Gdk.Pixbuf icon = GetTreeIcon (elementType);
			_table.Add(GetNewKey(), node);
			_store.AppendValues(iter,icon,label,true,CurrentKey); ///Editable
		}

		private void DrawUmlElement(UML.Element element, TreeIter parentIter, bool drawNestedDiagrams)
		{
			TreeIter iter;
			_table.Add(GetNewKey(), element);
			string label, elementType;
			GetLabelAndType(element, out label, out elementType);
			Gdk.Pixbuf icon;
			UML.Property umlProp = element as UML.Property;
			if(umlProp!=null && umlProp.OwningAssociation!=null)
			{
				icon = GetTreeIcon ("associationend");
			}
			else
			{
				icon = GetTreeIcon(elementType);
			}
			if(parentIter.Equals(TreeIter.Zero))
			{
				iter = _store.AppendValues(icon, label, !_isReadOnly, CurrentKey);
			}
			else
			{
				iter = _store.AppendValues(parentIter, icon, label, !_isReadOnly, CurrentKey);
			}
			if (_namespace2iter != null && element is UML.Namespace)
			{
				_namespace2iter.Add (element, iter);
			}
			foreach(UML.Element owned in element.OwnedElement)
			{
				DrawUmlElement(owned, iter, drawNestedDiagrams);
			}
			/*if (parentIter.Equals (TreeIter.Zero)) //HANGS WITH THIS!
			{
				iter.Free ();
			}*/
			if (drawNestedDiagrams)
			{
				foreach(DI.Diagram d in _diagrams)
				{
					object ns = ( (Uml2SemanticModelBridge)d.Namespace).Element;
					if (ns == element)
					{
						string diagramType = ((DI.SimpleSemanticModelElement)d.SemanticModel).TypeInfo;
						icon = GetTreeIcon(diagramType);
						_table.Add(GetNewKey(), d);
						iter = _store.AppendValues(iter, icon, d.Name, !_isReadOnly, CurrentKey); //Editable
						DrawLeaves(d, iter);
					}
				}
			}
			iter = Gtk.TreeIter.Zero; //#
		}
		
		private string GetLabel(object o)
		{
			DI.Diagram diagram;
			UML.Element umlElement;
			DI.GraphElement graphElement;
			string label;
			if((diagram = o as DI.Diagram) != null)
			{
				label = diagram.Name;
			}
			else if((umlElement = o as UML.Element) != null)
			{
				string elementType;
				GetLabelAndType(umlElement, out label, out elementType);
			}
			else if((graphElement = o as DI.GraphElement) != null)
			{
				label = GetLabel(graphElement);
			}
			else
			{
				label = o.GetType().Name;
			}
			return label;
		}
		
		private string GetLabel(DI.GraphElement element)
		{
			string label, elementType;
			GetLabelAndType(Helper.GetSemanticElement(element), out label, out elementType);
			return label;
		}

		private void GetLabelAndType(UML.Element element, out string label, out string elementType)
		{
			elementType = element.GetType().Name.Substring(6);
			UML.NamedElement namedElem = element as UML.NamedElement;
			UML.Generalization gen = element as UML.Generalization;
			UML.InterfaceRealization imp = element as UML.InterfaceRealization;
			if(gen != null)
			{
				label = gen.General!=null ? gen.General.QualifiedName : "(Generalization)"; 
			}
			else if(imp != null)
			{
				label = imp.Name;
				if((label==null || label==""))
				{
					label = imp.Contract!=null ? imp.Contract.QualifiedName : "(InterfaceRealization)";
				}
 			}
			else
			{
				label = (namedElem==null ? elementType : namedElem.Name);
			}
		}

		private string GetNewKey()
		{
			return (++_key).ToString();
		}
		
		// Gets the element corresponding to 
		// the current tree cell.
		private object GetSelectedElement()
		{
			TreeIter iter;
			TreeModel model;
			object selected = null;
			if (Selection.GetSelected(out model, out iter))
			{
				selected = _table[model.GetValue(iter, 3)];
				//iter.Free (); //#
				iter = Gtk.TreeIter.Zero;//#
    		}
    		return selected;
		}

		private Gdk.Pixbuf GetTreeIcon(string baseName)
		{
			Gdk.Pixbuf icon = MonoUML.IconLibrary.PixbufLoader.GetIcon (baseName.ToLower()+"_tree.png");
			if (icon == null)
			{
				icon = GetTreeIcon ("unknown_tree");
			}
			return icon;
		}
		
		private bool LookForMatchingRows(TreeModel model, TreePath path, TreeIter iter)
		{
			DI.GraphElement graphElement;
			object treeObject = _table[model.GetValue(iter, 3)];
			if (object.ReferenceEquals(_modifiedElement, treeObject))
			{
				_matchingRows.Add(new TreeRowReference(model, path));
			}
			else if((graphElement = treeObject as DI.GraphElement) != null)
			{
				Uml2SemanticModelBridge bridge = graphElement.SemanticModel as Uml2SemanticModelBridge;
				if(bridge != null && object.ReferenceEquals(_modifiedElement, bridge.Element))
				{
					_matchingRows.Add(new TreeRowReference(model, path));
				}
			}
			return false;
		}

		// The editable text had changed
		private void OnTreeViewCellEdited(object o, EditedArgs args)
		{
			object element = GetSelectedElement();
			object modifiedElement = element;
			string newName = args.NewText;
			DI.Diagram diagram;
			DI.GraphElement graphElement;
			UML.NamedElement umlNamedElement;
			if ((diagram = element as DI.Diagram) != null)
			{
				diagram.Name = newName;
				if (DiagramNameChanged != null)
				{
					DiagramNameChanged (this, diagram);
				}
				//System.Console.WriteLine ("Diagram Name Changed!");
			}
			else if ((graphElement = element as DI.GraphElement) != null)
			{
				Uml2SemanticModelBridge bridge = graphElement.SemanticModel as Uml2SemanticModelBridge;
				if (bridge != null)
				{
					if((umlNamedElement = bridge.Element as UML.NamedElement) != null)
					{
						umlNamedElement.Name = newName;
						modifiedElement = umlNamedElement;
					}
				}
			}
			else if ((umlNamedElement = element as UML.NamedElement) != null)
			{
				umlNamedElement.Name = newName;
			}
			_hub.BroadcastElementChange(modifiedElement);
		}
		
		// The TreeView had recieved a "click"
		protected override bool OnButtonPressEvent(Gdk.EventButton eventButton)
		{
			//Console.WriteLine("Evento {0}{1}",eventButton,eventButton.Type);
			switch (eventButton.Button)
			{
				case 1:
					break;
				case 2:
					Console.WriteLine("Middle Click");
					break;
				case 3:
					Console.WriteLine("Right  Click");
					//Here we'll build the "Popup Menu"
					/*TreeIter iter;
					TreeModel model;
					//this.Selection.GetSelected
					if (this.Selection.GetSelected(out model, out iter))
					{
						string val = (string) model.GetValue(iter, 1);
						string Type = (string) model.GetValue(iter, 3);
						Console.WriteLine ("RIGHT CLICK: {0} Type: {1}", val, Type);
//						buildMenu(Type);
					}*/
					break;
				default:
					Console.WriteLine("Other Click");
					break;
			}
			return base.OnButtonPressEvent(eventButton);						
		}

		private void RefreshElement(object element)
		{
			string newLabel;
			_modifiedElement = element;
			_matchingRows = new ArrayList();
			newLabel = GetLabel(element);
			_store.Foreach(new TreeModelForeachFunc(LookForMatchingRows));
			TreeIter iter, child;
			DI.Diagram diagram;
			UML.Element umlElement;
			foreach(TreeRowReference trowref in _matchingRows)
			{
				_store.GetIter(out iter, trowref.Path);
				_store.SetValue(iter, 1, newLabel);
				//limpia el nodo del arbol
				while(_store.IterChildren(out child, iter))
				{
					_store.Remove(ref child);
					//child.Free ();
				}
				//regenera los hijos del arbol
				if((diagram = element as DI.Diagram) != null)
				{
					DrawLeaves(diagram, iter);
				}
				else if((umlElement = element as UML.Element) != null)
				{
					foreach(UML.Element owned in umlElement.OwnedElement)
					{
						DrawUmlElement(owned, iter, true);
					}
					if (umlElement is UML.Namespace)
					{
						foreach(DI.Diagram d in _diagrams)
						{
							object ns = ( (Uml2SemanticModelBridge)d.Namespace).Element;
							if (ns == umlElement)
							{
								string diagramType = ((DI.SimpleSemanticModelElement)d.SemanticModel).TypeInfo;
								System.Console.WriteLine ("diagramType=> "+diagramType);
								Gdk.Pixbuf icon = GetTreeIcon(diagramType);
								_table.Add(GetNewKey(), d);
								iter = _store.AppendValues(iter, icon, d.Name, !_isReadOnly, CurrentKey); //Editable
								DrawLeaves(d, iter);
							}
						}
					}
				}
				//iter.Free ();//////
				base.ExpandRow(trowref.Path, false);
				//trowref.Path.Free ();//# 
				iter = child = Gtk.TreeIter.Zero;//#
			}
			newLabel = null;
			_modifiedElement = null;
			_matchingRows = null;
		}
		
		private void Select(object element)
		{
			if(_isChanging || element == null) return;
			_modifiedElement = element;
			_matchingRows = new ArrayList();
			_store.Foreach(new TreeModelForeachFunc(LookForMatchingRows));
			base.Selection.UnselectAll();
			foreach(TreeRowReference trowref in _matchingRows)
			{
				base.ExpandToPath(trowref.Path);
				base.Selection.SelectPath(trowref.Path);
				//trowref.Path.Free ();//# 
			}
			_modifiedElement = null;
			_matchingRows = null;
		}
		
		private void TreeSelectionChanged(object o, EventArgs args)
		{
			if(!_handlingEvent)
			{
				_isChanging = true;
				_hub.BroadcastElementSelection(GetSelectedElement());
				_isChanging = false;
			}
		}

		private TreeStore _store;
		private IBroadcaster _hub;
		// this flag signals that the tree is being modified due to an external (broadcasted) event.
		private bool _handlingEvent;
		private bool _isChanging;
		private bool _isReadOnly;
		private int _key;
		private object _modifiedElement;
		private ArrayList _matchingRows;
		private ArrayList _diagrams;
		private Hashtable _namespace2iter;
		private Hashtable _table;
	}
}
//EOF
