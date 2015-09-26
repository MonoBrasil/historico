using System;
using System.Collections;

namespace FiltraxCore
{
	/// <summary>
	/// Summary description for Prioridade.
	/// </summary>
	public class Prioridade
	{
		private string prioridade;
		private int id;

		public Prioridade(int id, string prioridade)
		{
			this.prioridade = prioridade;
			this.id = id;
		}

		public Prioridade()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public string Descricao
		{
			set
			{
				prioridade = value;
			}

			get
			{
				return prioridade;
			}
		}

		public int Id
		{
			set
			{
				id = value;
			}

			get
			{
				return id;
			}
		}
		public static Prioridade[] Listar()
		{
			ArrayList l = new ArrayList();
			l.Add(new Prioridade(1,"Urgente"));
			l.Add(new Prioridade(2,"Alta"));
			l.Add(new Prioridade(3,"Média"));
			l.Add(new Prioridade(4,"Normal"));
			l.Add(new Prioridade(5,"Baixa"));
			Prioridade[] p = new Prioridade[l.Count];
			l.CopyTo(p);
			return p;
		}
	}
}
