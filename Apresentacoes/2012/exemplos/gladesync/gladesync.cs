//
// GladeSync
//
// Keeps your C# code file in sync with your glade file 
// by adding/removing widgets and event handlers to your C#
// code.
//
// For this to work you have to add the following lines at a place
// where GladeSync may insert its code.
//
// #region GladeSync
// #endregion
//
// Author:
//   Ray Molenkamp (ray@lcdstudio.com)
//
// (C) 2004 Ray Molenkamp
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

using System;
using System.Collections;
using System.Xml;
using System.Text;

public class GladeSync
{
	class EventHandlerItem
	{
		private string _EventName;
		private string _Handler;
		private bool _Found;
		
		public string EventName
		{
			get { return _EventName;}
		}
		
		public string Handler
		{
			get { return _Handler;}
		}
		
		public bool Found
		{
			get {return _Found;}
			set {_Found = value;}
		}
		
		public EventHandlerItem(string EventName, string Handler)
		{
			_EventName = EventName;
			_Handler = Handler;
			_Found = false;
		}
	}
	
	public static void ScanFileForEventHandlers(string FileName,Hashtable EventList)
	{
		 System.IO.StreamReader sr = new System.IO.StreamReader(FileName);
	     string line;

		 while ((line = sr.ReadLine()) != null) 
         {
     			IDictionaryEnumerator myEnumerator = EventList.GetEnumerator();
      			while ( myEnumerator.MoveNext() )
				{
					EventHandlerItem i = (EventHandlerItem) myEnumerator.Value;
					//Todo: A nice Regex to detect EventHandlers would be a good idea.
					if ( (line.IndexOf(i.Handler) != -1) && (line.IndexOf("void") != -1) )
					{
						i.Found=true;
					}
				}
		 }
		 sr.Close();
		 
	}
	
	public static string GetMissingHandlers(Hashtable EventList)
	{
			StringBuilder Handlers = new StringBuilder();
			
   			IDictionaryEnumerator myEnumerator = EventList.GetEnumerator();
  			while ( myEnumerator.MoveNext() )
			{
				EventHandlerItem i = (EventHandlerItem) myEnumerator.Value;
				if (!i.Found)
				{
					Handlers.AppendFormat("\tpublic void {0} (object sender, EventArgs a)\n\t{1}\n\t//TODO: Add your code here.\n\t{2}\n\n",i.Handler,'{','}');					
				}
			}
			return Handlers.ToString();
	}

	public static void ScanGladeFile(string FileName,out String ControlListString, ref Hashtable EventsTable)
	{
	    StringBuilder ControlList = new StringBuilder();
	    XmlDocument doc = new XmlDocument();

	    doc.Load(FileName);

	    XmlNodeList elemList = doc.GetElementsByTagName("widget");	//Scan al widgets
		
	    for (int i=0; i < elemList.Count; i++)  
	    { 

		  if (elemList[i].Attributes["class"].Value.Substring(0,3)=="Gtk")
		  {

			ControlList.AppendFormat("\t[Glade.Widget] {0} {1};\n",elemList[i].Attributes["class"].Value.Substring(3),elemList[i].Attributes["id"].Value);

		    XmlNodeList EventList = doc.GetElementsByTagName("signal"); //Scan all signals for this widget
			for (int j=0; j < EventList.Count; j++)
			{ 
				string Name = EventList[j].Attributes["name"].Value;
				string Handler = EventList[j].Attributes["handler"].Value;
				EventsTable[Handler] = new EventHandlerItem(Name,Handler);
			}
			
		  }
		  
		  else
		  	Console.WriteLine("Unsupported control : {0}, ignoring",elemList[i].Attributes["class"].Value);
	    
		}
		ControlListString = ControlList.ToString(); 
	}
	
	public static void Main(string [] Args)
	{
		if (Args.Length!=2)
		{
			Console.WriteLine("Usage GladeSync [Glade-File] [CSharp-File]");
			return;
		}
		Hashtable Events = new Hashtable();
		string ControlList;
		
		ScanGladeFile(Args[0],out ControlList,ref Events); //Scan for widgets & signals

		ScanFileForEventHandlers(Args[1],Events);               //Scan for already existing Event handlers
		
		string MissingHandlers = GetMissingHandlers(Events); 	      //Generate a dummy stub for all missing handlers 
			
		//Write New file with missing widgets and handlers
		
		StringBuilder ResultFile = new StringBuilder();
		 
		System.IO.StreamReader sr = new System.IO.StreamReader(Args[1]);
	    string line;
		bool InSync=false;
		while ((line = sr.ReadLine()) != null) 
        {
			 if ((line.IndexOf("#endregion") != -1) && (InSync))
			 {
				 InSync = false;
	  			 ResultFile.AppendFormat("{0}\n\n",line);
				 ResultFile.AppendFormat("{0}",MissingHandlers); 		//insert missing handlers
			 }
			 else														//Ignore all Widgets aready in the region passthough all other text
			 	if (!InSync)
				 	 ResultFile.AppendFormat("{0}\n",line);
			 
			 if (line.IndexOf("#region GladeSync") != -1)               //Add all widgets found in the glade file
			 {
				ResultFile.AppendFormat("{0}\n",	ControlList.ToString());			 
				InSync = true;
			 }
			 
		 }
		 sr.Close();
		 
		 //Write Results 
		 System.IO.StreamWriter sw = new System.IO.StreamWriter(Args[1]);
		 sw.WriteLine(ResultFile.ToString());
		 sw.Close();
		
	}
}
