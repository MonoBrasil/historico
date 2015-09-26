/*
 * Copyright (C) 2004 Cesar Garcia Tapia <tapia@mcatalog.net>
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as
 * published by the Free Software Foundation; either version 2 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public
 * License along with this program; if not, write to the
 * Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 */

using System;

using Gdk;
using Gtk;

public class RatingWidget: HBox
{
	private Pixbuf bigStar;
	private Pixbuf littleStar;
		
	private Button[] buttons;
	private int MAX_RATING = 5;
	
	private int rating;
	
	public RatingWidget (): base ()
	{
		bigStar = Pixbuf.LoadFromResource ("bigstar.png");
		littleStar = Pixbuf.LoadFromResource ("littlestar.png");
		
		buttons = new Button[MAX_RATING];
		for (int i=0; i < MAX_RATING; i++) {
			Button button = new Button();
			button.Relief = ReliefStyle.None;
			button.CanFocus = false;
			button.Data["position"] = i;
			this.PackStart (button);
			button.Clicked += OnStarClicked;
			buttons[i] = button;
		}

		Value = 1;
	}
	
	public int Value {
		get {
			return rating;
		}
		set {
			rating = value;
			
			foreach (Button button in buttons) {
				if (button.Child != null) {
					button.Remove (button.Child);
				}
				
				int i = (int)button.Data["position"];
				if (i+1 <= rating) {	
					button.Add (new Gtk.Image(bigStar));
				}
				else {	
					button.Add (new Gtk.Image(littleStar));
				}
				button.Child.Visible = true;
			}
		}
	}
		
	private void OnStarClicked (object o, EventArgs args)
	{
		Gtk.Button button = (Gtk.Button)o;
		int position = (int)button.Data["position"];
		Value = position+1;
	}
}
