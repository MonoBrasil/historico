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
///		Marcelo D'avila
///		Maverson Eduardo Schulze Rosa
///
/// ChangeLog:
///				12/12/2004 Maverson/Marcelo: Added share files feature.
///				13/12/2004 Maverson: complete the first fileshare feature (console mode).
///
#endregion

using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using LifeLets.Core;

namespace LifeLets.Lib
{
	public class PeerNetwork : NetworkServer
	{
		private Life life;
		private string status = "Offline";
		private string ip;
		private Contact contact;
		private Network network;
		
		public PeerNetwork(Life life)
		{
			this.life = life;
			network = new Network(2004);
		}		

		public string RespondTo(string serviceName, string data)
		{
			if (serviceName == "name") return life.Name;
			if (serviceName == "picture") return LifeLets.Lib.Photo.ConvertPicture(life);
			return "Unknown service name: " + serviceName;
		}

		public void VerifyContacts()
		{
			Thread thrSearchContactsStatus = new Thread(new ThreadStart(SearchContactsStatus));
			thrSearchContactsStatus.IsBackground = true;
			thrSearchContactsStatus.Start();
		}

		public void SearchContactsStatus()
		{
			while(true)
			{	
				IDictionaryEnumerator enumerator = life.Contacts.GetEnumerator();

				while (enumerator.MoveNext())
				{
					
					contact = (Contact) enumerator.Value;
					string status = contact.Status;
					
					TcpClient cliente;
				
					Thread thrCliente = new Thread(new ThreadStart(Connect));
					thrCliente.Start();
					Thread.Sleep(1500);
					thrCliente.Abort();
					
					if(status != contact.Status)
					{
						Console.WriteLine("\nContact " + enumerator.Key.ToString() + " is now " + contact.Status); 
					}
				}
				Thread.Sleep(3000);
			}
		}			
					
		
		public void MakeAvailable()
		{
			
			NetworkBridge.MainListener(life,2004);
		}
		
	
		public void Connect()
		{
						
			try
			{	
				String name ; 

				NameService ns = new NameService(life);
				name = ns.CallService("oi",contact.IP);
								
				contact.Status = "Online";
				contact.Name = name;

			}
			catch(Exception ex)
			{
				contact.Status = "Invalid Address" + ex.Message;
				return;
			}
		}
		
		public static String RetrieveStatus(Contact contact)
		{
			TcpClient cliente;
		
			PeerNetwork com = new PeerNetwork(null);
			com.contact = contact;
			Thread thrCliente = new Thread(new ThreadStart(com.Connect));
			thrCliente.Start();
			Thread.Sleep(1500);
			thrCliente.Abort();
			return (com.contact.Name + " " + com.contact.Status);			
		}

	/*	public static void Send43Picture(NetworkStream output, string pic) 
		{
			try
			{
				BinaryWriter writer = new BinaryWriter( output );																			
				
				writer.Write(pic);
			}
			catch(Exception ex)
			{
				throw(ex);
			}
		}
		
*/
		
		public void PictureRequest(string nickname, Life life)
		{
				string pictureString = network.CallService("picture", "", life.FindIpAddress(nickname)); 						
				LifeLets.Lib.Photo.GetPicture(pictureString,life);
		}
		
		public static Hashtable GetRemoteContacts(string ip)
		{
			TcpClient cliente;
			int port = 2006;
			Hashtable contact = new Hashtable();
													
				cliente = new TcpClient();
				cliente.SendTimeout = 3000;
				cliente.ReceiveTimeout = 3000;
				cliente.Connect(ip, port);
				NetworkStream  output = cliente.GetStream();
																			
				BinaryReader reader = new BinaryReader( output );
								
				string contactsReturn = reader.ReadString();
				
				//System.Console.WriteLine(contactsReturn);
				
				string[] parts = contactsReturn.Split(new Char[]  {';'});
				
				for(int i = 0;i < (parts.Length-1); i += 4)
				{
					Contact newContact = new Contact();
					newContact.Nick = parts[i];
					newContact.Name = parts[i+1];
					newContact.IP = parts[i+2];
					newContact.Trust = Convert.ToInt32(parts[i+3]);
					contact.Add(parts[i],newContact); 
				}

				cliente.Close();				

			
			return contact;
			
		}
		
		public void ContactListener()
		{
			TcpListener listener;
			listener = new TcpListener(2006);
			
			
			try
			{				
				listener.Start();				
				
				while(true)
				{
					Socket connection = listener.AcceptSocket();
					
					NetworkStream  output = new NetworkStream( connection );
																				
					BinaryWriter writer = new BinaryWriter( output );
					
					string contactstring = "";
					
					foreach(Contact contact in life.Contacts.Values)
					{
						 contactstring += contact.Nick + ";" + contact.Name + ";" + contact.IP + ";" + contact.Trust + ";";
					}				
					writer.Write(contactstring);
				}
			}
			catch(Exception ex)
			{
				throw(ex);
			}
			finally
			{
				listener.Stop();
			}		
			
		}
		
		public void FilesNameListener()
		{
			TcpListener listener;
			listener = new TcpListener(2007);
						
			try
			{				
				listener.Start();				
				
				while(true)
				{
					Socket connection = listener.AcceptSocket();
					
					NetworkStream  output = new NetworkStream( connection );
																				
					BinaryWriter writer = new BinaryWriter( output );					
					
					string filestring = "";
					
					life.RetrieveFiles();
					
					foreach(File file in life.Files.Values)
					{
						 filestring += file.Name + ";";
					}				
					writer.Write(filestring);
				}
			}
			catch(Exception ex)
			{
				throw(ex);
			}
			finally
			{
				listener.Stop();
			}		
			
		}
		
		public void FileListener()
		{
			TcpListener listener;
			listener = new TcpListener(2008);
						
			try
			{				
				listener.Start();				
				
				while(true)
				{
					Socket connection = listener.AcceptSocket();
					
					NetworkStream  output = new NetworkStream( connection );
																				
					BinaryWriter writer = new BinaryWriter( output );
					
					BinaryReader reader = new BinaryReader( output );
					
					string fileName = reader.ReadString();
					
					FileStream fileStream = new FileStream(string.Format("{0}/{1}",life.FilesPath,fileName),FileMode.Open,FileAccess.Read);
					
					BinaryReader breader = new BinaryReader(fileStream);
					
					Byte[] fileBytes = new Byte[Convert.ToInt32(fileStream.Length)];
					breader.Read(fileBytes, 0, Convert.ToInt32(fileStream.Length));
					
					writer.Write(fileBytes.Length);
					
					writer.Write(fileBytes);							

					breader.Close();
					fileStream.Close();	
				}
			}
			catch(Exception ex)
			{
				throw(ex);
			}
			finally
			{
				listener.Stop();
			}		
			
		}
		
		public static Hashtable GetRemoteFilesNames(string ip)
		{
			TcpClient cliente;
			int port = 2007;
			Hashtable files = new Hashtable();
													
			cliente = new TcpClient();
			cliente.SendTimeout = 3000;
			cliente.ReceiveTimeout = 3000;
			cliente.Connect(ip, port);
			NetworkStream  output = cliente.GetStream();
																		
			BinaryReader reader = new BinaryReader( output );
							
			string filesReturn = reader.ReadString();

			//System.Console.WriteLine(contactsReturn);

			cliente.Close();

			string[] filesNames = filesReturn.Split(new Char[]  {';'});
			
			if(filesNames != null)
			{								
				for(int i = 0;i < (filesNames.Length-1); i++)
				{
					files.Add(filesNames[i],new File(filesNames[i])); 
				}			
			}
							
			return files;			
		}
		
		public static void GetRemoteFiles(string ip, string fileName, string nameToSave, string filesPath)
		{
			TcpClient cliente;
			int port = 2008;
			Hashtable files = new Hashtable();
													
			cliente = new TcpClient();
			cliente.SendTimeout = 3000;
			cliente.ReceiveTimeout = 3000;
			cliente.Connect(ip, port);
			NetworkStream  output = cliente.GetStream();
			
			BinaryWriter writer = new BinaryWriter( output );
			
			BinaryReader reader = new BinaryReader( output );
			
			writer.Write(fileName);			
			
			int bytesSize = reader.ReadInt32();
			
			Byte[] fileBytes = reader.ReadBytes(bytesSize);

			FileStream fs = System.IO.File.Create(filesPath + @"/" + nameToSave);
			fs.Write(fileBytes, 0, bytesSize);

			cliente.Close();				
		}

	}
}