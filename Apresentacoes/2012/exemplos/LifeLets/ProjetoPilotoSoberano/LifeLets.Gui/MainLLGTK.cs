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
/// Class Console.cs
///
/// Sumary:
///
///
/// Authors:
///
///
/// ChangeLog: 29/11/2004 - Jacson - removido funcao VerFoto
///									 Levantado o Thread para navegacao
///
///
/// ChangeLog: 06/12/2004 - Maverson Eduardo - Passing the ChatWindow Container to the sovereChat and LLMainWindow
///
///
#endregion

using LifeLets.Lib;
using System.Collections;
using System.Threading;
using LifeLets.GUI; 

namespace LifeLets.GUI 
{
	public class MainGTK {
	
		private static Thread thrServidor;
		
		public static void Run(Life life)
		{
			Hashtable chats = new Hashtable();
			
			if (life.Name == null)
				life.Name = "SEM_NOME";
				
			PeerNetwork com = new PeerNetwork(life);
			
			//cria Thread que fica verificando se os contatos estão online ou offline.
			com.VerifyContacts();			
	
			thrServidor = new Thread(new ThreadStart(com.MakeAvailable));
			System.Console.WriteLine("You are now onLine: " +life.Name);
			thrServidor.IsBackground = true;
			thrServidor.Start();

			LifeLets.GUI.SovereChat chat = new LifeLets.GUI.SovereChat(life, chats);

			Thread thrServidor2 = new Thread(new ThreadStart(chat.Listen));
			thrServidor2.IsBackground = true;
			thrServidor2.Start();	
			
			
			Thread thrServidor4 = new Thread(new ThreadStart(com.ContactListener));
			thrServidor4.IsBackground = true;
			thrServidor4.Start();	

			new LLMainWindow(life, chats);
		}
	}
}
