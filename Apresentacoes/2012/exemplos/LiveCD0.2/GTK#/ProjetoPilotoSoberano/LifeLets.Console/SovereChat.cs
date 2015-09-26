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
/// Class PeerNetwork.cs
///
/// Sumary:
///
///
/// Authors:
///
///
/// ChangeLog:
/// 27-11-2004 - change name class Chat to SovereChat, solve problem
///
///
///
#endregion



using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace LifeLets.Lib
{

 public class SovereChat
 {
 	#region varibles
 private string ip = null;
 private static int port = 2005; 
 private Life life;
 #endregion
  
 
 public SovereChat (Life vidaPar)
{
  this.life = vidaPar;
 }

 
 public SovereChat(string ip, string nomeVida )
 {
  this.ip = ip;
  System.Console.WriteLine("Enter the message");
  SendMessage(System.Console.ReadLine());
 }

	public SovereChat(string ip)
	{
		this.ip = ip;
	}

	
  public void SendMessage(string message)
 {
  
  
  TcpClient cliente;
     
  try
  {
   cliente = new TcpClient();
   cliente.Connect(ip,port);
   NetworkStream  output = cliente.GetStream();
                  
   BinaryWriter writer1 = new BinaryWriter(output);
   BinaryWriter writer2 = new BinaryWriter(output);
	      
   writer1.Write(message + "teste");
   writer2.Write("Teste srtring hard code");
   
   cliente.Close(); 
  }
  catch(Exception ex)
  {
   throw(ex);
  }
 }
 
 public void Listen()
 {
 
  TcpListener listener;

   try
   {
    listener = new TcpListener(port);
    listener.Start();
    
    while(true)
    {     
         
     
     Socket connection = listener.AcceptSocket();
     
     string ipSender = (((IPEndPoint)connection.RemoteEndPoint).Address.ToString ());
          
     string nickname = life.FindNickname(ipSender);
     
     if (nickname != @"/notInList")
     {
      System.Console.WriteLine(nickname + " Says: \n");
     }
     else
     {
      System.Console.WriteLine("Warning. You have received a message from a member that is not in your contact list.\n Member Name: " +life.Name + "\n Message:\n");
     }
     NetworkStream socketstream = new NetworkStream( connection );
          
     BinaryReader reader = new BinaryReader( socketstream );

     string message = reader.ReadString();
     System.Console.WriteLine(message); 
    }
   }
   catch(Exception ex)
   {
    throw(ex);
   } 
 }
  }

 	
 	


}
