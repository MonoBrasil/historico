/*
MonoUML.Widgets.UML - A library for representing the UML2 elements
Copyright (C) 2004  Mario Fuentes <mario@gnome.cl>
Copyright (C) 2004  Mario Carrión <marioc@unixmexico.org>

UMLControlsPointGroup.cs: the frame for resizing elements

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

namespace MonoUML.Widgets.UML2
{
	using Gdk;
	using Gnome;
	
	public class UMLControlPointGroup : CanvasGroup
	{	
		public event ResizedHandler Resized;
		
		public UMLControlPointGroup (UMLElement group, bool resizable) : this (group)
		//public UMLControlPointGroup (UMLNode group, bool resizable) : this (group)
		{
			for (int i = 0; i < ((int) Location.Total); i++)
			{
				_p[i].Moveable = resizable;
			} 
		}

		public UMLControlPointGroup (UMLElement group) : base ((CanvasGroup) group)
		//public UMLControlPointGroup (UMLNode group) : base ((CanvasGroup) group)
		{
			_element = group;
			_p = new UMLControlPoint [4];
			_frame = new CanvasRect (this);
			_frame.OutlineColor = DEFAULT_OUTLINE_COLOR;
			_frame.FillColorRgba = 0xfbfbfb15;
			_frame.WidthUnits = 1;
			_frame.Show ();
			
			for (int i = 0; i < ((int) Location.Total); i++)
			{
				_p[i] = new UMLControlPoint (this);
				_p[i].Moved += PointMoved;
			}
			Redraw ();
		}
		
		public UMLControlPoint []ControlPoint 
		{
			get { return _p; }
		}
		
		public UMLElement UMLElement
		//public UMLNode UMLNode  
		{
			get { return _element; }
		}

		public void Redraw ()
		{
			// Frame
			_frame.X1 = X;
			_frame.Y1 = Y;
			_frame.X2 = _element.Width;
			_frame.Y2 = _element.Height;
			// Top Left
			_p[(int) Location.TLeft].X = 0;
			_p[(int) Location.TLeft].Y = 0;
			// Top Right
			_p[(int) Location.TRight].X = _element.Width;
			_p[(int) Location.TRight].Y = 0;
			// Bottom Left
			_p[(int) Location.BLeft].X = 0;
			_p[(int) Location.BLeft].Y = _element.Height;
			// Bottom Right
			_p[(int) Location.BRight].X = _element.Width;
			_p[(int) Location.BRight].Y = _element.Height;
		}		

		private void PointMoved (object obj)
		{
			UMLControlPoint o = (UMLControlPoint) obj;
			double dx = 0.0, dy = 0.0, new_w = 0.0, new_h = 0.0;

			if (o == _p[(int) Location.TLeft])
			{
				dx = o.DX;
				dy = o.DY;				
				new_w = _element.Width + (o.DX*-1);
				new_h = _element.Height + (o.DY*-1);
			}
			else if (o == _p[(int) Location.TRight])
			{
				dy = o.DY;
				new_w = _element.Width + o.DX;
				new_h = _element.Height - o.DY;
			}
			else if (o == _p[(int) Location.BLeft])
			{
				dx = o.DX;
				new_w = _element.Width - o.DX;
				new_h = _element.Height + o.DY;
			}
			if (o == _p[(int) Location.BRight])
			{
				new_w = _element.Width + o.DX;
				new_h = _element.Height + o.DY;
			}
			
			// Only greater than five values
			if (new_w >= 5 && new_h >= 5)
			{
				if (Resized != null)
				{
					Resized (this, new_w, new_h, dx, dy);
				}
				Redraw ();
			}
		}

		protected override void Finalize ()
		{
			for (int i = 0; i < ((int) Location.Total); i++)
			{
				_p[i].Destroy ();
			}
		}

		private CanvasRect _frame;
		private UMLControlPoint []_p;
		private UMLElement _element;
		private const string DEFAULT_OUTLINE_COLOR = "gray";
		
		private enum Location
		{
			TLeft,   // TopLeft
			TRight,  // TopRight
			BLeft,   // BottomLeft
			BRight,  // BottomRight
			Total
		}

	}
}
