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
///
///			29/11/2004 - Jacson - removido funcao VerFoto
///									 Levantado o Thread para navegacao
///
///			12/12/2004 Maverson/Marcelo: Added share files feature.
///			13/12/2004 Maverson: complete the first fileshare feature (console mode).
///
#endregion

using LifeLets.Lib;
using System.Collections;
using System.Threading;
using LifeLets.Core;

namespace LifeLets.Test 
{
	public class Console {
	
		private static Thread thrServidor;
		
		public static void rodar(Life life)
		{
		    System.Console.WriteLine("\nMeu nome é: " + life.Name);
			
			string opcao = "";
			
			if (life.Name == null)
				life.Name = "SEM_NOME";
				
			PeerNetwork com = new PeerNetwork(life);
			
			//cria Thread que fica verificando se os contatos estão online ou offline.
			com.VerifyContacts();
						
			thrServidor = new Thread(new ThreadStart(com.MakeAvailable));
			System.Console.WriteLine("Voce esta OnLine: " +life.Name);
			thrServidor.IsBackground = true;
			thrServidor.Start();			

			SovereChat chat = new SovereChat(life);

			Thread thrServidor2 = new Thread(new ThreadStart(chat.Listen));
			thrServidor2.IsBackground = true;
			thrServidor2.Start();									
			
			Thread thrServidor4 = new Thread(new ThreadStart(com.ContactListener));
			thrServidor4.IsBackground = true;
			thrServidor4.Start();
			
			//return share files list.
			Thread thrServidor5 = new Thread(new ThreadStart(com.FilesNameListener));
			thrServidor5.IsBackground = true;
			thrServidor5.Start();		

			//return file.
			Thread thrServidor6 = new Thread(new ThreadStart(com.FileListener));
			thrServidor6.IsBackground = true;
			thrServidor6.Start();		
			
			while(opcao.ToUpper() != "S")
			{				
			    // My Funcitions
			 	System.Console.WriteLine("\n\n " + life.Name + " What do you do ?");
				System.Console.WriteLine("N. Change Name");
				System.Console.WriteLine("L. Nickname List");
				System.Console.WriteLine("T. See  Photo");
				System.Console.WriteLine("F. Change Photo");
				System.Console.WriteLine("B. Change Default Browser's Path");
				System.Console.WriteLine("E. Sharing File List");
				System.Console.WriteLine("C. Connect");
				// On my contacts
				System.Console.WriteLine("A. Add contact/change nickname");
				System.Console.WriteLine("R. Remove Nickname");
				System.Console.WriteLine("D. Change trust degree"); 
				System.Console.WriteLine("J. Get Picture");
				System.Console.WriteLine("K. Get File");
				System.Console.WriteLine("P. FindIpAddress Contact");
				System.Console.WriteLine("Z . Navegation on Contacts");				
				// System.Console.WriteLine("M. Start Chat"); only for GUI
				
				System.Console.WriteLine("S. Sair");
				
				opcao = System.Console.ReadLine();
				
				switch (opcao.ToUpper())
				{
					case "A":
					{
						System.Console.Write("Digite o Apelido do seu novo Contato: ");
						string nickname = System.Console.ReadLine();
						
						System.Console.Write("Digite o IP de " + nickname + ": ");
						string ip = System.Console.ReadLine();
						
						life.AddContact(nickname,ip);
						break;
					}
					
					case "D":
					{
						System.Console.Write("Type the NickName of your contact: ");
						string nick = System.Console.ReadLine();
						
						Contact contact = life.SearchContact(nick);

						if(contact != null)
						{
							while(true)
							{
								try
								{
									System.Console.Write("\nType the new trust degree ( 0 to 100 ): ");
									int trust = int.Parse(System.Console.ReadLine());
									 
									if (trust >= 0 && trust <= 100)
										contact.Trust = trust;
									else
										throw(new System.Exception(""));
									break;
								}
								catch
								{
									System.Console.WriteLine("Invalid option, try again!");
								}
							}
						}
						else
						{
								System.Console.WriteLine("Invalid NickName!");
						}
						break;
					}					
							
					case "F":
					{ 
						System.Console.WriteLine("Enter path of pícture: ");
						life.PathToPhoto = System.Console.ReadLine();
						break;
					}
					
					case "T":
					{
						Photo.ShowPicture(life);
						break;
					}
					
					case "J":
					{
					 	System.Console.WriteLine("View Picture From: ");
				    	string nickname = System.Console.ReadLine();      
				      
				    	com.PictureRequest(nickname, life);
				                     
				    	break;
					}
					case "C":
					{
						System.Console.WriteLine("Enter the nickname: ");
						string nick = System.Console.ReadLine();
						Contact contact = life.SearchContact(nick);						
						System.Console.WriteLine("Your buddy at [" + contact.IP + "] is " + PeerNetwork.RetrieveStatus(contact));
						
					break;
					}
				
					case "N":
					{
					    System.Console.WriteLine("Enter your real name: ");
					    life.Name = System.Console.ReadLine();
						System.Console.Write("Your name is now : : ");
						System.Console.WriteLine(life.Name);
						break;
					}
					case "L":
					{
						System.Console.WriteLine("Yuors  Contacts so:");

						IDictionaryEnumerator enumerator = life.Contacts.GetEnumerator();

						while (enumerator.MoveNext())
						{
							System.Console.Write("    Apelido: ");
							System.Console.Write(enumerator.Key);
							System.Console.Write("    IP: ");
							System.Console.Write(((Contact)enumerator.Value).IP);
							System.Console.Write("	Trust degree:");
							System.Console.Write(((Contact)enumerator.Value).Trust.ToString() + "%");							
							System.Console.Write("    Status: ");
							System.Console.WriteLine(PeerNetwork.RetrieveStatus((Contact)enumerator.Value));
						}
						
						System.Console.WriteLine("--- FIM DA LISTA ---\n\n");
						break;
					}
// This is only available on GUI
					case "M":
					{
						//ChatWindow.Factory(life);
					
						System.Console.WriteLine("Enter the Nickname of your contact:");
						string nome = System.Console.ReadLine();
						string ip = life.FindIpAddress(nome);
						if (ip == null )
						{
							System.Console.WriteLine("Apelido não existe!!!");
						}
						else
						{

							SovereChat message = new SovereChat(ip,life.Name);


						}
						break;
					}

					case "P":
					{
						System.Console.WriteLine("Digite o Apelido:");
						string nome = System.Console.ReadLine();
						string ip = life.FindIpAddress(nome);
						System.Console.WriteLine("-------->>Apelido : {0}  IP: {1} ", nome,ip);
						break;
					}
					case "R":
					{
						System.Console.Write("Digite o Apelido que deseja remover: ");
						string nickname = System.Console.ReadLine();
						
						life.RemoveContact(nickname);
						System.Console.Write(nickname + " foi removido da sua lista.");
						break;
					}
					
					case "B":
					{
						System.Console.Write("Enter the new default browser's path: ");
						string browserPath = System.Console.ReadLine();
						
						life.BrowserPath = browserPath;
						System.Console.Write(browserPath + " is your new browser's path.");
						break;
					}
					case "Z":
					{

						// Passagem de 1 para o primeiro nível de trusting por ele ser vc mesmo o nível de confiança é total
						Navigator.ConsoleNavigator(">",life.Contacts,100);			
						
						break;
					}
					
					case "E":
					{
						life.RetrieveFiles();
						
						System.Console.WriteLine("Your sharing files are:");

						IDictionaryEnumerator enumerator = life.Files.GetEnumerator();

						while (enumerator.MoveNext())
						{
							System.Console.Write("    Name: ");
							System.Console.WriteLine(enumerator.Key);							
						}
						
						System.Console.WriteLine("--- END OF FILES LIST ---\n\n");
						
						break;			
					}
					
					case "K":
					{
						
						System.Console.WriteLine("Enter contact's nickname to get files:");
						string nickname = System.Console.ReadLine();
						Hashtable contactFiles = PeerNetwork.GetRemoteFilesNames(life.FindIpAddress(nickname));
						
						System.Console.WriteLine("{0}'s sharing files are:",nickname);

						IDictionaryEnumerator enumerator = contactFiles.GetEnumerator();

						while (enumerator.MoveNext())
							System.Console.WriteLine("    Name: " + enumerator.Key);
						
						System.Console.WriteLine("--- END OF FILES LIST ---\n\n");		
						
						System.Console.WriteLine("Which file would you like to get?	");
						string fileName = System.Console.ReadLine();
						
						System.Console.WriteLine("Which name would you like to save it?");
						string nameToSave = System.Console.ReadLine();
						
						PeerNetwork.GetRemoteFiles(life.FindIpAddress(nickname),fileName, nameToSave, life.FilesPath);
						
						System.Console.WriteLine("Sucessfull download of {0} executed!", fileName);
						
						break;			
					}				
															
					default:
					{
						if (thrServidor != null)
						{
					 		if (thrServidor.IsAlive)
					 			{
					 			//thrServidor.Interrupt();
					 			thrServidor.Abort();
					 			}					 			
					 	}	
					 	break;
					}
				}
			}
			
		}
	public void listRemoteContacts(string friendNickname)
	{
		
	}
	
	}
}
