// project created on 29/11/2004 at 01:48
using System;
using Gtk;
using Glade;
using System.IO;
using System.Collections;
using System.Reflection;


public class GladeApp
{
        public static void Main (string[] args)
        {
                new GladeApp (args);
        }

        public GladeApp (string[] args) 
        {
                Application.Init();

                //Glade.XML gxml = new Glade.XML (null, "gui.glade", "window1", null);
                //gxml.Autoconnect (this);
                SplashScreenForm.SplashScreen.ShowAll();
                Application.Run();
               
        }

        /* Connect the Signals defined in Glade */
        public void OnWindowDeleteEvent (object o, DeleteEventArgs args) 
        {
                Application.Quit ();
                args.RetVal = true;
        }
}

public class SplashScreenForm : Gtk.Window
	{
		static SplashScreenForm splashScreen = new SplashScreenForm();
		
		public static SplashScreenForm SplashScreen {
			get {
				return splashScreen;
			}
		}		
		
		public SplashScreenForm() : base (Gtk.WindowType.Popup)
		{
			this.Decorated = false;
			this.WindowPosition = WindowPosition.Center;
			this.TypeHint = Gdk.WindowTypeHint.Splashscreen;
			Gdk.Pixbuf bitmap = new Gdk.Pixbuf(Assembly.GetEntryAssembly(), "lifelets.jpg");
			DefaultWidth = bitmap.Width;
			DefaultHeight = bitmap.Height;
			Gtk.Image image = new Gtk.Image (bitmap);
			image.Show ();
			this.Add (image);
		}

		}


