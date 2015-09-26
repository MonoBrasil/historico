using System;
using System.Collections;

namespace FiltraxCore
{
	/// <summary>
	/// Summary description for Usuario.
	/// </summary>
	public class Usuario
	{
		private int id;
		private string nome;
		private string departamento;
		private string login;
		private string senha;

		public Usuario(int id, string nome, string departamento)
		{
			this.id = id;
			this.nome = nome;
			this.departamento = departamento;
		}

		public Usuario()
		{
			//
			// TODO: Add constructor logic here
			//
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

		public string Departamento
		{
			set
			{
				departamento = value;
			}

			get
			{
				return departamento;
			}
		}

		public string Login
		{
			set
			{
				login = value;
			}

			get
			{
				return login;
			}
		}

		public string Senha
		{
			set
			{
				senha = value;
			}

			get
			{
				return senha;
			}
		}

		public static ArrayList Lista()
		{
			ArrayList l = new ArrayList();
			l.Add(new Usuario(1,"Alexandre","Diretoria"));
			l.Add(new Usuario(2,"Binhara","Informatica"));
			l.Add(new Usuario(3,"José","Vendas"));
			
			return l;
		}
	}
}
