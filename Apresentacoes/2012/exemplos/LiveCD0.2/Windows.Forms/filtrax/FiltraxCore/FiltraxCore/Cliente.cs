using System;
using System.Collections;

namespace FiltraxCore
{
	/// <summary>
	/// Summary description for Cliente.
	/// </summary>
	public class Cliente
	{
		private int id;
		private string nome;
		private string nomeFantasia;

		//lista 
		private static int cont;
		private static ArrayList clientes = new ArrayList(); 

		public Cliente(string nome)
		{
			this.id = cont++;
			this.nome = nome;
		}

		public Cliente()
		{
			this.id = cont++;
		}

		public string NomeFantasia
		{
			set
			{
				nomeFantasia = value;
			}

			get
			{
				return nomeFantasia;
			}
		}

		public string Nome
		{
			set
			{
				nome = value;
			}

			get
			{
				return nome;
			}
		}

		protected static ArrayList Lista()
		{
			ArrayList l = clientes;
			l.Add(new Cliente("Ferragens Negro"));
			l.Add(new Cliente("Osten Ferragens"));
			l.Add(new Cliente("Coopervale Ltda"));
			return l;
		}

		public static Cliente[]  Listar()
		{
			ArrayList clientes = Cliente.Lista();
			Cliente[]  c = new Cliente[clientes.Count]; 
			clientes.CopyTo(c);
			return c;
		}

		public bool inserir()
		{
			clientes.Add(this);
			return true;
		}
	}
}
