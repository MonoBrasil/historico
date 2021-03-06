#region license
// LifeLets  - a first Mono Sovereign Computing Aplication
// Copyright (C) 2004 Mono Basic Brazilian Team
// see a team members on AUTHORS.TXT
//
// This software is licensed unde CC-GPL (Creative Commons GPL)
//
// GNU General Public License, Free Software Foundation
// The GNU General Public License is a Free Software license. 
// Like any Free Software license, it grants to you the four following freedoms:
//
//   0. The freedom to run the program for any purpose.
//   1. The freedom to study how the program works and adapt it to your needs.
//   2. The freedom to redistribute copies so you can help your neighbor.
//   3. The freedom to improve the program and release your improvements to the public,
//   so that the whole community benefits.
//
// You may exercise the freedoms specified here provided that you comply with the
// express conditions of this license. The principal conditions are:
//   1- You must conspicuously and appropriately publish on each copy distributed an 
//      appropriate copyright notice and disclaimer of warranty and keep intact all 
//      the notices that refer to this License and to the absence of any warranty; 
//      and give any other recipients of the Program a copy of the GNU General Public
//      License along with the Program. Any translation of the GNU General Public 
//      License must be accompanied by the GNU General Public License.
//   2- If you modify your copy or copies of the program or any portion of it,
//      or develop a program based upon it, you may distribute the resulting work 
//      provided you do so under the GNU General Public License. Any translation 
//      of the GNU General Public License must be accompanied by the GNU General 
//      Public License.
//   3- If you copy or distribute the program, you must accompany it with the complete 
//      corresponding machine-readable source code or with a written offer, valid for 
//      at least three years, to furnish the complete corresponding machine-readable source code.
//      Any of these conditions can be waived if you get permission from the copyright holder.
//      Your fair use and other rights are in no way affected by the above.
//      This is a human-readable summary of the Legal Code (the full GNU General Public License).
//      here  http://creativecommons.org/licenses/GPL/current/
//
// Contact Information
//
// http://monobasic.sl.org.br
// mailto:letslife@psl-pr.softwarelivre.org
//

#endregion

#region information
///
/// Class LLChangeName
///
/// Sumary:
///  Window to change name 
///
/// Authors:
///  Marco Antonio Konopaki
///  Alexandre Halicki
///  Alessandro Binhara 
///  Maverson Eduardo
///
/// ChangeLog: 
///  created on 9/12/2004 at 14:30
///	 window working						 		 
///
///
///
///
#endregion




using LifeLets.Lib;
using System;
using System.Collections;
using Gtk;
using Glade;

namespace LifeLets.GUI
{
	public class LLChangeName
	{
	
		private Life myLife;
		private LLMainWindow mainWindow;
		
		#region GladeSync
		[Glade.Widget] Window ChangeName;
		[Glade.Widget] Fixed fixed1;
		[Glade.Widget] Entry etrName;
		[Glade.Widget] Label lblName;
		[Glade.Widget] Statusbar stsBar;
		[Glade.Widget] Button btOk;
		#endregion

		public void on_btOk_clicked (object sender, EventArgs a)
		{
		
			myLife.Name = etrName.Text;
			this.mainWindow.RefreshFromLife();
			
			ChangeName.Destroy();
		}



		public LLChangeName (Life life, LLMainWindow mw)
		{
			this.myLife = life;
			this.mainWindow = mw;
			Glade.XML gxml = new Glade.XML (null, "LLChangeName.glade", "ChangeName", null);
			gxml.Autoconnect(this);
			ChangeName.WindowPosition = WindowPosition.Center;				    	
		}			
	}
}











