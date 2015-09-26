/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2004  Manuel Cerón <ceronman@gmail.com>

UMLSelector.cs: a selector rectangle.

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
using System;
using System.Collections;

namespace MonoUML.Widgets.UML2
{
	internal class UMLSelector
	{
		public UMLSelector (CanvasGroup group)
		{
			_selected_elements =new ArrayList();
			_rectangle = new CanvasRect(group);
			
			_rectangle.FillColorRgba = DefaultFillColor;
			_rectangle.OutlineColor = DefaultOutlineColor;
		}
		
		public void StartSelection(double px, double py)
		{
			_selected_elements.Clear();
			_selecting = true;
			_rectangle.X1 = px;
			_rectangle.Y1 = py;
			_rectangle.X2 = px;
			_rectangle.Y2 = py;
		}
		
		public void StopSelection(ArrayList elements_to_select)
		{
			if (_selecting)
			{
				_rectangle.Hide();
				_selecting = false;
				
				if (elements_to_select != null)
				{
					SelectItemsOnRectangle(elements_to_select);
				}
			}
		}

		public void Clear ()
		{
			_selected_elements.Clear ();
			_selected_first = null;
		}

		public void DrawSelection(double x, double y)
		{
			if (_selecting) 
			{
				_rectangle.X2 = x;
				_rectangle.Y2 = y;
				
				_rectangle.RaiseToTop ();
				_rectangle.Show();
			}
		}
		
		public void MoveSelection(double x, double y)
		{
			_selected_first.CalculateLocalCoordinates(ref x, ref y);
			foreach (UMLElement element in _selected_elements)
			{
				element.Move(x, y);
				element.NotifyMove();				
				element.RequestRedraw();
			}
		}
		
		public void ResizeSelection(double width, double height)
		{
			foreach (UMLElement element in _selected_elements)
			{
				if (element != _selected_first) _selected_first.ProportionalResize(element);
				element.RequestRedraw();
				element.NotifyMove();
			}
		}
		
		public void SelectItemsOnRectangle (ArrayList element_list) 
		{
			double tmpx, tmpy;
			
			if (_rectangle.X1 > _rectangle.X2)
			{
				tmpx = _rectangle.X1;
				_rectangle.X1 = _rectangle.X2;
				_rectangle.X2 = tmpx;
			}
			
			if (_rectangle.Y1 > _rectangle.Y2)
			{
				tmpy = _rectangle.Y1;
				_rectangle.Y1 = _rectangle.Y2;
				_rectangle.Y2 = tmpy;
			}
			
			foreach (UMLElement element in element_list)
			{
				//if a element is inside the rectangle
				if (element != null 
					&& element.X > _rectangle.X1 
					&& element.Y > _rectangle.Y1
					&& element.X + element.Width < _rectangle.X2
					&& element.Y + element.Height < _rectangle.Y2)
				{
					_selected_elements.Add(element);
					element.Select();
					_selected_first = element;
				}
			}
		}
		
		public void Select(UMLElement element)
		{
			if (element != null) 
			{
				_selected_first = element;
				
				if (Empty)
				{
					_selected_elements.Add(element);
				}
				else if (!_selected_elements.Contains(element))
				{
					CleanSelection();
					FirstElement = element;
				}
				element.Select();
			}
		}
		
		public void CleanSelection()
		{
			foreach (UMLElement element in _selected_elements)
			//foreach (UMLNode element in _selected_elements)
			{
				if (element != null)
				{
					element.Deselect();
				}
			}
			_selected_elements.Clear();
		}
				
		public bool Selecting 
		{
			set { _selecting = value;}
			get { return _selecting;}
		}
		
		public bool Empty 
		{
			get { return _selected_elements.Count == 0; }
		}
		
		//primary element for work with
		public UMLElement FirstElement
		{
			//todo: throw an exception when FirstElement == null
			get { return _selected_first; }
			set 
			{
				if (Empty)
				{
					_selected_elements.Add(value);
				}
				else if (_selected_elements.Contains(value) )
				{
					_selected_elements.Add(value);
				}
				_selected_first = value;
			}
		}
		
		//array of nodes
		private ArrayList _selected_elements = null;
		//rectangle that will be drawn
		private CanvasRect _rectangle = null;
		
		private UMLElement _selected_first = null;
		
		private bool _selecting = false;
		
		private const uint DefaultFillColor = 0x55555515; //semi-transparent gray color
		private const string DefaultOutlineColor = "gray";
	}
}








