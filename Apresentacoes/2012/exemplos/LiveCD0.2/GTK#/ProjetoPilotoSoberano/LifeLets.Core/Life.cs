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
/// Class Life.cs
///
/// Sumary: 
/// 
///
/// Authors: 
///
///
/// ChangeLog: 27/11 - Marcelo e Jacson - Implementacao da configuracao do caminho do browser
///
///
///
///
#endregion

using System;
using System.Collections;
using System.Drawing;
using System.IO;

namespace LifeLets.Lib 
{
	[Serializable]
	public class Life :  System.MarshalByRefObject
	{

		private string name = "NO_NAME";
		private Hashtable contacts;
		private Hashtable files;
		private string pathToPhoto = null;
		private string description = "";
		private string browserPath = "/usr/bin/konqueror";
		private string filesPath = Path.Combine(Environment.CurrentDirectory, "files");

		public Life()
		{
			contacts = new Hashtable();
			files = new Hashtable();
		}
		
		public Hashtable Contacts 
		{
			get
			{	Hashtable temp = new Hashtable();
			
				if (contacts == null) return temp;
				
				IDictionaryEnumerator enumerator = contacts.GetEnumerator();
				
				while (enumerator.MoveNext())
				{
					temp.Add(enumerator.Key, enumerator.Value);
				}
				
				return temp;
			}
		}
		
		public string Name 
		{
			get
			{
				return name;
			}
			set 
			{
				name = value;
			}
		} 
		
		public string Description 
		{
			get
			{
				return description;
			}
			set 
			{
				description = value;
			}
		} 
		
		public string PathToPhoto 
		{
			get
			{
				return pathToPhoto;
			}
			set 
			{
				pathToPhoto = value;
			}
		}
		
		public string BrowserPath
		{
			get
			{
				return browserPath;
			}
			set
			{
				browserPath = value;
			}
		}

		
		public string FilesPath
		{
			get
			{
				return filesPath;
			}
			set
			{
				filesPath = value;
			}
		}
		
		public Hashtable Files
		{
			get
			{
				Hashtable temp = new Hashtable();
				
				if (files == null) return temp;
				
				IDictionaryEnumerator enumerator = files.GetEnumerator();
				
				while (enumerator.MoveNext())
				{
					temp.Add(enumerator.Key, enumerator.Value);
				}
				
				return temp;
			}			
		}
			
		public void AddContact(string nickname, string ipAddress)
		{
			Contact contact = new Contact(ipAddress);
			contact.Nick = nickname;
			
			if (this.FindNickname(nickname) != null)
				contacts.Add(nickname, contact);
			else
			{
				contacts.Remove(nickname);
				contacts.Add(nickname, contact);
			}
		}		

		public void RetrieveFiles()
		{
			File file;
			string[] sAux;
			string fileName;
			
			if(!Directory.Exists(filesPath))
			{
				Directory.CreateDirectory(filesPath);
			}		
			
			foreach (string s in Directory.GetFiles(filesPath))
			{
				sAux = s.Split(Path.DirectorySeparatorChar);
				fileName = sAux[sAux.Length-1];
				
				if(!FindFile(fileName))
				{
					file = new File(fileName);
					files.Add(fileName,file);
				}		
			}			
		}
		
		public void RemoveContact(string nickname)
		{
			contacts.Remove(nickname);
		}
		
		public string FindIpAddress(string nickname)
		{
			if (contacts.ContainsKey(nickname))
				return ((Contact)contacts[nickname]).IP;
			return null;
		}
		
		public Contact SearchContact(string nickname)
		{
			if (contacts.ContainsKey(nickname))
				return contacts[nickname] as Contact;
			return null;
		}
		
		public string FindNickname(string ipAddress)
		{
			foreach (DictionaryEntry d in contacts)
				if (((Contact)d.Value).IP == ipAddress)
					return d.Key as string;
			return @"/notInList";
		}
		
		public bool FindFile(string fileName)
		{
			foreach (DictionaryEntry d in files)
				if (((File)d.Value).Name == fileName)
					return true;
					return false;
		}
	}
}
