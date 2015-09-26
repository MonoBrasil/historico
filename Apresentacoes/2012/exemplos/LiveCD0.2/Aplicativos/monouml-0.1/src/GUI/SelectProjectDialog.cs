/*
MonoUML.GUI - The core classes for running the MonoUML's GUI
Copyright (C) 2004  Mario Carrión

This file is part of MonoUML.

MonoUML is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

MonoUML is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MonoUML; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
using Gtk;
using Gnome;
using Glade;
using System;
using MonoUML.I18n;

namespace MonoUML.GUI
{
	public enum ProjectDialogAction
	{
		Cancel,
		ImportXmi,
		None,
		New,
		Open,
		ReverseEngineering
	}

	public class SelectProjectDialog : Gtk.Dialog, IDisposable
	{
		public SelectProjectDialog (Gtk.Window parent) :
			base (GettextCatalog.GetString ("Select project"), parent, Gtk.DialogFlags.DestroyWithParent)
		{
			Modal = true;
			_iconlistOptions = new IconList (78, new Adjustment (1, 1, 1, 1, 1, 1) , 0);
			ScrolledWindow scroll = new ScrolledWindow ();
			scroll.Add (_iconlistOptions);
			VBox.PackStart (scroll, true, true, 1);
			_buttonCancel = (Gtk.Button) AddButton (Gtk.Stock.Cancel , Gtk.ResponseType.Cancel);
			_buttonCancel.Clicked += OnButtonSelectProjectCancel;
			_buttonOK = (Gtk.Button) AddButton (Gtk.Stock.Ok , ResponseType.Ok);
			_buttonOK.Clicked += OnButtonSelectProjectOK;
			BuildIcons ();
			ShowAll ();
			WidthRequest = 350;
			HeightRequest = 160;
			WindowPosition = Gtk.WindowPosition.CenterAlways;
		}
		
		public ProjectDialogAction Action
		{
			get 
			{
				return _selection;
			}
		}
		
		public new void Dispose ()
		{
			_iconlistOptions.Dispose ();
			base.Dispose ();
		}

		// Inserts the icons
		private void BuildIcons()
		{
			_iconlistOptions.IconSelected += OnIconSelectedProject;
			//_iconlistOptions.IconUnselected += OnIconUnselectedProject; //Crashes with debian-based-systems and mono >= 1.1.5
			_iconlistOptions.KeyPressEvent += OnKeyPressEvent;
			
			Gtk.Button button = new Gtk.Button (); 
			Gdk.Pixbuf pbuf = button.RenderIcon (Gtk.Stock.New, Gtk.IconSize.LargeToolbar, Gtk.Stock.New); 
			_iconlistOptions.AppendPixbuf(pbuf , _icon_list[0,0], _icon_list[0,1]);
			pbuf = button.RenderIcon (Gtk.Stock.Open, Gtk.IconSize.LargeToolbar, Gtk.Stock.Open);
			_iconlistOptions.AppendPixbuf(pbuf , _icon_list[1,0], _icon_list[1,1]);
			pbuf = button.RenderIcon (Gtk.Stock.Convert, Gtk.IconSize.LargeToolbar, Gtk.Stock.Convert);
			_iconlistOptions.AppendPixbuf(pbuf , _icon_list[2,0], _icon_list[2,1]);
			_iconlistOptions.AppendPixbuf(pbuf , _icon_list[3,0], _icon_list[3,1]);
			//_iconlistOptions.SelectIcon (0); //Crashes with debian-based-systems and mono >= 1.1.5
			_selection = ProjectDialogAction.None;
		}

		// Enables the OK button
		private void OnIconSelectedProject (object o, IconSelectedArgs args)
		{
			try
			{			
				switch (args.Num)
				{
					case 0:
						_selection = ProjectDialogAction.New;
						break;
					case 1:
						_selection = ProjectDialogAction.Open;
						break;
					case 2:
						_selection = ProjectDialogAction.ImportXmi;
						break;
					case 3:
						_selection = ProjectDialogAction.ReverseEngineering;
						break;
					default:
						_selection = ProjectDialogAction.None;
						break;
				}
				_buttonOK.Sensitive = (args.Num >= 0);
			}
			catch (Exception ex) { } 
		}

		private void OnIconUnselectedProject (object o, IconUnselectedArgs args)
		{
			_selection = ProjectDialogAction.None;
			_buttonOK.Sensitive = false;
		}

		private void OnButtonSelectProjectOK (object obj, EventArgs args)
		{
			if (_selection == ProjectDialogAction.None)
			{
				MessageDialog md = new MessageDialog (
					 this,
					DialogFlags.DestroyWithParent,
					MessageType.Error,
					ButtonsType.Close,
					GettextCatalog.GetString ("Select some option from the icon list."));
				md.Run();
				md.Destroy();
			}
		}
		
		private void OnKeyPressEvent (object o, KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return)
			{
				try
				{
					System.Console.WriteLine ("Selected icon: "+_iconlistOptions.Selection[0]);
					_buttonOK.Click (); 
				}
				catch (Exception ex) { }
			}
		}
		
		private void OnButtonSelectProjectCancel (object obj, EventArgs args)
		{
			_selection = ProjectDialogAction.Cancel;
			this.Hide ();
		}
		
		private string [,]_icon_list = new string[,] 
			{
				{ "new.png", GettextCatalog.GetString ("New Project") }, 
				{ "open.png", GettextCatalog.GetString ("Open Project") },
				{ "wizard.png", GettextCatalog.GetString ("From other format") },
				{ "wizard.png", GettextCatalog.GetString ("From Reverse Engineering") }
			};

		private Button _buttonOK;
		private Button _buttonCancel;
		private ProjectDialogAction _selection;
		private IconList _iconlistOptions;
	}
}
