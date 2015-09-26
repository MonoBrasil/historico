using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using WordCountLib;

namespace TestGUIWordCount
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox textBox1;
		public System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.GroupBox groupBox2;
		public System.Windows.Forms.DataGrid dataGrid1;
		private System.Windows.Forms.TreeView treeView1;
		private WordCount c;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.button1 = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Controls.Add(this.progressBar1);
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Location = new System.Drawing.Point(16, 64);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(336, 416);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Texto Para Analisar";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(264, 368);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(64, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "Analisar";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(16, 368);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(240, 23);
			this.progressBar1.TabIndex = 1;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(8, 16);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(320, 328);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "textBox1";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.treeView1);
			this.groupBox2.Controls.Add(this.dataGrid1);
			this.groupBox2.Location = new System.Drawing.Point(360, 64);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(336, 416);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Score das Palavras";
			// 
			// treeView1
			// 
			this.treeView1.ImageIndex = -1;
			this.treeView1.Location = new System.Drawing.Point(8, 240);
			this.treeView1.Name = "treeView1";
			this.treeView1.SelectedImageIndex = -1;
			this.treeView1.Size = new System.Drawing.Size(224, 160);
			this.treeView1.TabIndex = 1;
			// 
			// dataGrid1
			// 
			this.dataGrid1.AllowSorting = false;
			this.dataGrid1.DataMember = "";
			this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid1.Location = new System.Drawing.Point(8, 16);
			this.dataGrid1.Name = "dataGrid1";
			this.dataGrid1.ReadOnly = true;
			this.dataGrid1.Size = new System.Drawing.Size(320, 216);
			this.dataGrid1.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(720, 501);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			c = new WordCount( textBox1.Text, "teste");
			c.EventoUpdateGUI += new WordCountLib.WordCount.Atualizador(c_EventoUpdateGUI);
			
			ArrayList l = c.topWords();
			WordScore[] scoreList = new WordScore[l.Count];
			l.CopyTo(scoreList);
			dataGrid1.DataSource = scoreList;

			
			//WordScore w = (WordScore) l[1];
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
		
		}

		private void c_EventoUpdateGUI()
		{
			this.progressBar1.Value = c.Percentual;
		}
	}
}
