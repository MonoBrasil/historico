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
/// ChangeLog:
/// created on 26/11/2004 at 10:23
///
///
///
#endregion



using System;
using LifeLets.Lib;

namespace LifeLets
{

// ====================Opções do Console Novo====================
  public class ChangeYourName : MenuOption
  {
  	 public override string Name
  	 { 
  	 
  		 get{
  		 
  		 		return "Change your name";
  		 
  		 	}
  	 
  	 }
  	 
  	 public override void Run(Object obj)
  	 {
  	 	Life life = (Life) obj;
  	 	  	 
  	 	System.Console.WriteLine("Enter your real name: ");
		life.Name = System.Console.ReadLine();
		System.Console.WriteLine("Now your name is: {0}", life.Name);	
  	   	 
  	 }  	 
 }  
 
 
 public class ChangeYourPicture : MenuOption
 {
 	public override string Name
 	{
		get 
		{
			return "Change your picture";
		}
		
 	}
 	
 	public override void Run(Object obj)
 	{
 		Life life = (Life) obj;
 		System.Console.WriteLine("Enter picture file path: ");
 		string pathImage = System.Console.ReadLine();
		life.PathToPhoto = pathImage;
		System.Console.WriteLine("Now your picture file path is {0} " , pathImage);
		
 	}
 	
 
 }
 
 
  public class ChangeYourProfile : MenuOption
  {
 	public override string Name
 	{
		get 
		{
			return "Change your profile";
		}
		
 	}
 	
 	public override void Run(Object obj)
 	{
 		Life life = (Life) obj;
  	 	System.Console.WriteLine("Enter your new profile: ");
		life.Description = System.Console.ReadLine();
		System.Console.WriteLine("Now your new profile is: {0}", life.Description);	
		
 	}
 	
 
 }
 
  public class ContactsMenu : MenuOption
  {
 	public override string Name
 	{
		get 
		{
			return "Contacts";
		}
		
 	}
 	
 	public override void Run(Object obj)
 	{ 		
 			
 		new ContactsConsole((Life) obj); 		
		
 	}
 	
 
 }
 
 // =================Fim Opções do Console Novo====================
 
 // =================Opções do Contacts Console====================
 
  public class Chat : MenuOption
  {
 	public override string Name
 	{
		get 
		{
			return "Chat ";
		}
		
 	}
 	
 	public override void Run(Object obj)
 	{
 		Contact contact = (Contact) obj;
 		
 		System.Console.WriteLine("CHAT!!! {0}",contact.Name); 		
		
 	}	
 	
 } 
 	
 
 
 // ============== Fim Opções do Contacts Console==================
 
 /* Template
  public class ChangeYourPicture : MenuOption
  {
 	public override string Name
 	{
		get 
		{
			return "Change your ";
		}
		
 	}
 	
 	public override void Run(Object obj)
 	{
 		
		
 	}
 	
 
 }
 */
}