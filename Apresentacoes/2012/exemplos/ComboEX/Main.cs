
 using System;
 using Gtk;
 
 class ComboSample
 {
      Combo combo;
 
      static void Main ()
      {
           new ComboSample ();
      }
 
      ComboSample ()
      {
           Application.Init ();
 
           Window win = new Window ("ComboSample");
           win.DeleteEvent += new DeleteEventHandler (OnWinDelete);
 
           string[] list = new string[] {"one", "two", "three"};
 
           combo = new Combo ();
           combo.PopdownStrings = list;
           combo.DisableActivate ();
           combo.Entry.Activated += new EventHandler (OnEntryActivated);
 
           win.Add (combo);
 
           win.ShowAll ();
           Application.Run ();
      }
 
      void OnEntryActivated (object o, EventArgs args)
      {
           Console.WriteLine (combo.Entry.Text);
      }
 
      void OnWinDelete (object obj, DeleteEventArgs args)
      {
           Application.Quit ();
      }
 }