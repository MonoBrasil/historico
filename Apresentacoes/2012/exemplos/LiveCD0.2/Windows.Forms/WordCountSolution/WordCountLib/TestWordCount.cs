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
using NUnit.Framework;
using System.Text;
using System.Collections;



namespace WordCountLib
{
	[TestFixture]
	public class TestWordCount
	{
				
		[Test]
		public void TestcCount1()
		{
			WordCount c = new WordCount("Teste de projetos. Este Projetos.", "projetos");
			int sum = c.count();
			Assert.AreEqual(sum, 5);
		}

		[Test]
		public void TestcCountEspaco()
		{
			WordCount c = new WordCount("Teste     de  projetos.    Este Projetos.", "projetos");
			int sum = c.count();
			Assert.AreEqual(sum, 5);
		}

		[Test]
		public void TestcCountPronto()
		{
			WordCount c = new WordCount("Teste de projetos Este projetos.", "projetos");
			int sum = c.howMany();
			Assert.AreEqual(sum, 2);
			
		}

		[Test]
		public void TestcCount()
		{
			WordCount c = new WordCount("Teste de projetos Este projetos ", "projetos");
			int sum = c.howMany();
			Assert.AreEqual(sum, 2);
			
		}

		[Test]
		public void TestcCountVirgula()
		{
			WordCount c = new WordCount("Teste de projetos, Este projetos", "projetos");
			int sum = c.howMany();
			Assert.AreEqual(sum, 2);
			
		}

		[Test]
		public void TestcCountCaixaAlta()
		{
			WordCount c = new WordCount("Teste de Projetos, Este projetos", "projetos");
			int sum = c.howMany();
			Assert.AreEqual(sum, 2);
			
		}

		[Test]
		public void TestEmail()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("From everaldo_canuto em yahoo.com.br  Tue Feb 22 00:55:15 2005");
			sb.Append("From: everaldo_canuto em yahoo.com.br (Everaldo Canuto)");
			sb.Append("Date: Tue Feb 22 00:53:59 2005");
			sb.Append("Subject: [Mono-Brasil] Re: Digest Mono-Brasil, volume 14, assunto 19");
			sb.Append("In-Reply-To: <ICANOW$5680B9A0B127D1D2018B661BF2F1D4F4@bol.com.br>");
			sb.Append("References: <ICANOW$5680B9A0B127D1D2018B661BF2F1D4F4@bol.com.br>");
			sb.Append("Message-ID: <1109044515.11191.10.camel@localhost.localdomain>");
			sb.Append("");
			sb.Append("Olá,");
			sb.Append("");
			sb.Append("Certo, você tem todo direito de continuar onde está, tem muita gente que");
			sb.Append("continua programando e Clipper. Agora uma coisa que não entendo é porque");
			sb.Append("você está postando isso numa lista de Mono, acho que seria mais");
			sb.Append("interessante você postar seus comentários em alguma lista ou fórum de");
			sb.Append("Delphi.");
			sb.Append("");
			sb.Append("Como já disse antes mesmo com o Delphi você terá de aprender tudo");
			sb.Append("novamente para a plataforma .Net, mas você pode escolher o caminho de");
			sb.Append("continuar como está afinal ainda teremos trabalho para programadores");
			sb.Append("Delphi por pelo menos mais uns 6 anos.");
			sb.Append("");
			sb.Append("Quanto a esquecer o GNOME, bem... o GNOME é um ótimo desktop assim como");
			sb.Append("o KDE, muita gente usa o GNOME e no mundo Linux a grande vantagem é ter");
			sb.Append("escolha, coisa que não ocorre quando se usa o Windows.");
			sb.Append("");
			sb.Append("Ah, só um toque, não leve a mal mas seu português precisa melhorar um");
			sb.Append("pouco porque algumas frases estão até incompreensíveis, imagine que você");
			sb.Append("precise fazer algum relatório de atividades na empresa e que trabalha ou");
			sb.Append("escrever algum documento para um cliente... isso também é importante.");
			sb.Append("");
			sb.Append("Abraços,");
			sb.Append("Everaldo.");

			WordCount c = new WordCount(sb.ToString(), "gnome");
			int sum = c.count();
			Assert.AreEqual(sum, 229);
		}


		[Test]
		public void TestEmail1()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("From everaldo_canuto em yahoo.com.br  Tue Feb 22 00:55:15 2005");
			sb.Append("From: everaldo_canuto em yahoo.com.br (Everaldo Canuto)");
			sb.Append("Date: Tue Feb 22 00:53:59 2005");
			sb.Append("Subject: [Mono-Brasil] Re: Digest Mono-Brasil, volume 14, assunto 19");
			sb.Append("In-Reply-To: <ICANOW$5680B9A0B127D1D2018B661BF2F1D4F4@bol.com.br>");
			sb.Append("References: <ICANOW$5680B9A0B127D1D2018B661BF2F1D4F4@bol.com.br>");
			sb.Append("Message-ID: <1109044515.11191.10.camel@localhost.localdomain>");
			sb.Append("");
			sb.Append("Olá,");
			sb.Append("");
			sb.Append("Certo, você tem todo direito de continuar onde está, tem muita gente que");
			sb.Append("continua programando e Clipper. Agora uma coisa que não entendo é porque");
			sb.Append("você está postando isso numa lista de Mono, acho que seria mais");
			sb.Append("interessante você postar seus comentários em alguma lista ou fórum de");
			sb.Append("Delphi.");
			sb.Append("");
			sb.Append("Como já disse antes mesmo com o Delphi você terá de aprender tudo");
			sb.Append("novamente para a plataforma .Net, mas você pode escolher o caminho de");
			sb.Append("continuar como está afinal ainda teremos trabalho para programadores");
			sb.Append("Delphi por pelo menos mais uns 6 anos.");
			sb.Append("");
			sb.Append("Quanto a esquecer o GNOME, bem... o GNOME é um ótimo desktop assim como");
			sb.Append("o KDE, muita gente usa o GNOME e no mundo Linux a grande vantagem é ter");
			sb.Append("escolha, coisa que não ocorre quando se usa o Windows.");
			sb.Append("");
			sb.Append("Ah, só um toque, não leve a mal mas seu português precisa melhorar um");
			sb.Append("pouco porque algumas frases estão até incompreensíveis, imagine que você");
			sb.Append("precise fazer algum relatório de atividades na empresa e que trabalha ou");
			sb.Append("escrever algum documento para um cliente... isso também é importante.");
			sb.Append("");
			sb.Append("Abraços,");
			sb.Append("Everaldo.");

			WordCount c = new WordCount(sb.ToString(), "gnome");
			int sum = c.howMany();
			Assert.AreEqual(sum, 3);
		}


		[Test]
		public void TestTopWord()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(" Teste de projeto a contar palavras");
			sb.Append(" Teste de aplicacao  se  investigar strings");
			sb.Append(" Teste de software  de procurar strings");

			WordCount c = new WordCount(sb.ToString(), "teste");
			ArrayList l = c.topWords();
			WordScore w = (WordScore) l[1];
			Assert.AreEqual(w.Word, "teste");
			Assert.AreEqual(w.Score, 3);

		}
		[TestFixtureSetUp]
		public void Init()
		{
			// TODO: Add Init code.
		}
		
		[TestFixtureTearDown]
		public void Dispose()
		{
			// TODO: Add tear down code.
		}
	}
}

