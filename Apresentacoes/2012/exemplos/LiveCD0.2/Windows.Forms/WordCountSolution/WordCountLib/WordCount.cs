/*
 * Created by Alessandro de Oliveira Binhara 
 * * Date: 29/09/2005
 * Time: 16:13
 * 
 * Description: This a Class to filter, count and see a top ten Words in text
 * 
 * WordCount
 * Copyright (c) 2005, Alessandro de Oliveira Binhara
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are 
 * permitted provided that the following conditions are met:
 * 
 * - Redistributions of source code must retain the above copyright notice, this list 
 * of conditions and the following disclaimer.
 *
 * - Redistributions in binary form must reproduce the above copyright notice, this list
 * of conditions and the following disclaimer in the documentation and/or other materials 
 * provided with the distribution.
 *
 * - Neither the name of the <ORGANIZATION> nor the names of its contributors may be used to 
 * endorse or promote products derived from this software without specific prior written 
 * permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS &AS IS& AND ANY EXPRESS 
 * OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY 
 * AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER 
 * IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT 
 * OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */


using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace WordCountLib
{
	/// <summary>
	/// WordCount - is a word count and Score Word Generatros.
	/// </summary>
	public class WordCount 
	{
		private string text;
		private string find;
        private int percentual = 0; 

		public delegate void Atualizador();

		public event Atualizador EventoUpdateGUI;

		/// <summary>
		/// Creat a new word count constructod
		/// </summary>
		/// <param name="text"> String text to analise</param>
		/// <param name="find"> String tu find in text</param>
		public WordCount(string text, string find)
		{
			this.text = text.ToLower();
			this.find = find;
			this.filter();
		}

		public int Percentual
		{
			set
			{
				percentual = value;
			}

			get
			{
				return percentual;
			}
		}

		
		/// <summary>
		///  Count a words in text
		/// </summary>
		/// <returns> Nunbem of words in text </returns>
		public int count()
		{
			char[] sep = {' '};
			String[] split = this.text.Split(sep);
			int cont = 0;
			foreach (String e in split)
			{
				if(e.Equals("")) cont++;
			}
			return split.Length - cont;
		}

		/// <summary>
		/// C Count a words in text
		/// </summary>
		/// <param name="s"> string to count </param>
		/// <returns>Number of words in text</returns>
		public int count(string s)
		{
			char[] sep = {' '};
			String[] split = s.Split(sep);
			int cont = 0;
			//conta os espacos em branco
			foreach (String e in split)
			{
				if(e.Equals("")) cont++;
			}
			return split.Length - cont;
		}

		/// <summary>
		/// Count a number of string in text
		/// </summary>
		/// <returns>Number of strings</returns>
		public int howMany()
		{
			this.text.ToLower();
			String[] split = this.text.Split(' ');
			int count = 0;
			foreach (String s in split) 
			{
				if (s.Equals(this.find)) count++;
			}
			return count;
		}

		/// <summary>
		/// Generete a top score words
		/// </summary>
		/// <returns></returns>
		public ArrayList topWords()
		{
			ArrayList tmp = new ArrayList();
			String[] search = this.text.Split(' ');
			
			
			//eleminando duplicacos
			ArrayList lTmp = new ArrayList();
			for(int i = 0; i < search.Length; i++)
			{
				string t = search[i];
				if(!t.Equals(""))
					lTmp.Add(search[i]);

				for(int a = i; a < search.Length; a++)
				{
					if(search[a].Equals(t))
						search[a] = "";
				}

			}

			search = new  String[lTmp.Count];

			lTmp.CopyTo(search);
	
			int totalWords = this.count();
			int totalProgresso = search.Length;
			int count = 0;
			this.percentual = 100*count/totalProgresso;
			foreach (String word in search)
			{
				string temp =  Regex.Replace(this.text, word, "");

				int conte = this.count(temp);
				int score = totalWords - conte; 
				tmp.Add(new WordScore(word,score));

				totalWords = conte;
				this.text = temp;
				count++;

				this.percentual = 100*count/totalProgresso;
				this.EventoUpdateGUI();
			}
			tmp.Sort();

			return tmp;
		}

		


		#region Filter
		public void filter()
		{
			
			this.text =  Regex.Replace(this.text, "\\.", " ");
			this.text =  Regex.Replace(this.text, ";", " ");
			this.text =  Regex.Replace(this.text, ",", " ");
			this.text =  Regex.Replace(this.text, "!", " ");
			this.text =  Regex.Replace(this.text, "\\[", " ");
			this.text =  Regex.Replace(this.text, "\\]", " ");
			this.text =  Regex.Replace(this.text, "\\(", " ");
			this.text =  Regex.Replace(this.text, "\\)", " ");
			this.text =  Regex.Replace(this.text, "\\*", " ");
			this.text =  Regex.Replace(this.text, ">", " ");
			this.text =  Regex.Replace(this.text, "\\?", " ");
	
			
			
			//this.text =  Regex.Replace(this.text, "  ", " ");
			//this.text =  Regex.Replace(this.text, "  ", " ");
			//this.text =  Regex.Replace(this.text, "    ", " ");
			//this.text =  Regex.Replace(this.text, ".  .", " ");
			//this.text =  Regex.Replace(this.text, ".   .", " ");
		
			string[] preposicoes = new string[21]
			{	" de ", " a "," e ", " i ", " u ", " ante ", " até ", " após ", " com ", " contra ",
				 " desde ", " em ", " entre ", " para ",
				" per ", " perante ", " por ", " sem ", " sob ", " sobre ", " trás "
			
			}; 
			foreach (String p in preposicoes)
			{
				string text1=  Regex.Replace(this.text, p, " ");
				this.text = text1;
			}
				
			#region Contracoes
			string[] contracoes = new string[19]
			{	" à ",
				" às ",
				" as ",
				" àquele ", 
				" aquele ",
				" àquela ",
				" aquela ",
				" àquilo ",
				" aquilo ",
				" do ", 
				" dele " ,
				" deste ",
				" disto ", 
				" daqui ", 
				" nesse ",
				" no ",
				" num " , 
				" naquele " , 
				" pelo " 			
			};
			#endregion Contracoes

			foreach (String p in contracoes)
			{
				this.text =  Regex.Replace(this.text, p, " ");
			}

			#region Pronomes
			string[] pronomes = new string[29]
				{
				" eu "," tu ",
				" ele "," ela ", 
				" nós "," vós ",
				" eles "," elas ",
				" me "," te ",
				" se "," o ",
				" a " ," lhe ",
				" nos ",
				" vos ", " os ",
				" as ", " lhes " ,
				" mim ", " comigo ",
				" ti ", " contigo ",
				" si ", " consigo ",
				" conosco ", " convosco ",
				" você ", " vocês " 
				};
			#endregion Pronomes

			foreach (String p in pronomes)
			{
				this.text =  Regex.Replace(this.text, p, " ");
			}
				
			this.text =  Regex.Replace(this.text,  @"\s{2,99}" , " ");



		}
		#endregion Filter
	}

	public class WordScore : IComparable
	{
		private string word;
		private int score;

		public string Word
		{
			set
			{
				word = value;
			}

			get
			{
				return word;
			}
		}

		public int Score
		{
			set
			{
				score = value;
			}

			get
			{
				return score;
			}
		}

		public WordScore(string word, int score)
		{
			this.word = word;
			this.score = score;
		}

		int IComparable.CompareTo(object obj)
		{
			WordScore mc = (WordScore)obj;

			if (this.score > mc.score)
				return 1;
			if (this.score < mc.score)
				return -1;
			return 0;
		}

		

		


	}


	}


