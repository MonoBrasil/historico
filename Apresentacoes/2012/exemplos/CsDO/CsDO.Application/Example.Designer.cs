using CsDO.Lib;
using System.Collections;

namespace CsDO.Application
{
    public class TesteObj : DataObject
    {
        private int _id;
        [Column("Cod"), PrimaryKey()]
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        private string _nome;
        public string Nome
        {
            get
            {
                return _nome;
            }
            set
            {
                _nome = value;
            }
        }

        private int _idade;
        public int Idade
        {
            get
            {
                return _idade;
            }
            set
            {
                _idade = value;
            }
        }

        private double _peso;
        [Column("PesoKg")]
        public double Peso
        {
            get
            {
                return _peso;
            }
            set
            {
                _peso = value;
            }
        }

        private TesteObj3 _teste;
        [Column("Teste1")]
        public TesteObj3 TesteObj3
        {
            get
            {
                return _teste;
            }
            set
            {
                _teste = value;
            }
        }

        private bool _ativo;
        public bool Ativo
        {
            get { return _ativo; }
            set { _ativo = value; }
        }

        public void SetField(string col, object val)
        {
            setField(col, val);
        }

        public bool Retrieve(string col, object val)
        {
            return retrieve(col, val);
        }

        public string GetFields()
        {
            return Fields;
        }

        public string GetActiveFields()
        {
            return ActiveFields;
        }

        public string GetWhere()
        {
            return Where;
        }

        public void SetWhere(string value)
        {
            Where = value;
        }

        public string GetLimit()
        {
            return Limit;
        }

        public void SetLimit(string value)
        {
            Limit = value;
        }

        public string GetOrderBy()
        {
            return OrderBy;
        }

        public void SetOrderBy(string value)
        {
            OrderBy = value;
        }

        public string GetGroupBy()
        {
            return GroupBy;
        }

        public void SetGroupBy(string value)
        {
            GroupBy = value;
        }

        public IList GetPrimaryKeys()
        {
            return PrimaryKeys;
        }

        //private Sequence sequence = new Sequence();

        public TesteObj()
        {
            autoIncrement += new AutoIncrement(sequence.Increment);
        }
    }

    public class TesteObj3 : DataObject
    {
        private int _id;
        [Column("Cod"), PrimaryKey()]
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        private string _nome;
        public string Nome
        {
            get
            {
                return _nome;
            }
            set
            {
                _nome = value;
            }
        }

        private int _idade;
        public int Idade
        {
            get
            {
                return _idade;
            }
            set
            {
                _idade = value;
            }
        }

        private double _peso;
        [Column("PesoKg")]
        public double Peso
        {
            get
            {
                return _peso;
            }
            set
            {
                _peso = value;
            }
        }
    }

    [Table("TesteObject")]
    public class TesteObj2 : TesteObj { }


    partial class Example
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private TesteObj teste = new TesteObj();

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(47, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // Example
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "Example";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;

    }
}

