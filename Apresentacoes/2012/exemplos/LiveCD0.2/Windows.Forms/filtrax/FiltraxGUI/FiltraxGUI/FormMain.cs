using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiltraxGUI
{
	/// <summary>
	/// Summary description for Main.
	/// </summary>
	public class FormMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ToolBar toolBar2;
		private System.Windows.Forms.ToolBarButton toolBarButton2;
		private System.Windows.Forms.ToolBarButton toolBarButton3;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.DataGrid dataGrid1;
		private System.ComponentModel.IContainer components;

		public FormMain()
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
				if(components != null)
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
			this.components = new System.ComponentModel.Container();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.toolBar1 = new System.Windows.Forms.ToolBar();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.toolBar2 = new System.Windows.Forms.ToolBar();
			this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
			this.SuspendLayout();
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 487);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(704, 22);
			this.statusBar1.TabIndex = 0;
			this.statusBar1.Text = "statusBar1";
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "Filtrax";
			// 
			// toolBar1
			// 
			this.toolBar1.AllowDrop = true;
			this.toolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						this.toolBarButton1});
			this.toolBar1.DropDownArrows = true;
			this.toolBar1.Location = new System.Drawing.Point(0, 0);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(704, 42);
			this.toolBar1.TabIndex = 1;
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Text = "teste";
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(0, 42);
			this.panel1.Name = "panel1";
			this.panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.panel1.Size = new System.Drawing.Size(216, 445);
			this.panel1.TabIndex = 2;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.treeView1);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.groupBox1.Size = new System.Drawing.Size(216, 445);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Sistema";
			this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(216, 42);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 445);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.ImageIndex = -1;
			this.treeView1.Location = new System.Drawing.Point(3, 16);
			this.treeView1.Name = "treeView1";
			this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
																				  new System.Windows.Forms.TreeNode("Clientes", new System.Windows.Forms.TreeNode[] {
																																										new System.Windows.Forms.TreeNode("Cadastrar"),
																																										new System.Windows.Forms.TreeNode("Listar")}),
																				  new System.Windows.Forms.TreeNode("Financeiro"),
																				  new System.Windows.Forms.TreeNode("Produção", new System.Windows.Forms.TreeNode[] {
																																										new System.Windows.Forms.TreeNode("Em Espera"),
																																										new System.Windows.Forms.TreeNode("Finalizado")}),
																				  new System.Windows.Forms.TreeNode("Produtos", new System.Windows.Forms.TreeNode[] {
																																										new System.Windows.Forms.TreeNode("Cadastrar "),
																																										new System.Windows.Forms.TreeNode("Listar")})});
			this.treeView1.SelectedImageIndex = -1;
			this.treeView1.ShowLines = false;
			this.treeView1.Size = new System.Drawing.Size(210, 426);
			this.treeView1.TabIndex = 0;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.button1);
			this.panel2.Controls.Add(this.textBox1);
			this.panel2.Controls.Add(this.toolBar2);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(219, 42);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(485, 46);
			this.panel2.TabIndex = 4;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.groupBox2);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(219, 88);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(485, 399);
			this.panel3.TabIndex = 5;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.dataGrid1);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(0, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(485, 399);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "groupBox2";
			// 
			// toolBar2
			// 
			this.toolBar2.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						this.toolBarButton2,
																						this.toolBarButton3});
			this.toolBar2.DropDownArrows = true;
			this.toolBar2.Location = new System.Drawing.Point(0, 0);
			this.toolBar2.Name = "toolBar2";
			this.toolBar2.ShowToolTips = true;
			this.toolBar2.Size = new System.Drawing.Size(485, 42);
			this.toolBar2.TabIndex = 0;
			// 
			// toolBarButton2
			// 
			this.toolBarButton2.Text = "novo";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(296, 16);
			this.textBox1.Name = "textBox1";
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "textBox1";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(400, 16);
			this.button1.Name = "button1";
			this.button1.TabIndex = 2;
			this.button1.Text = "button1";
			// 
			// dataGrid1
			// 
			this.dataGrid1.DataMember = "";
			this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid1.Location = new System.Drawing.Point(3, 16);
			this.dataGrid1.Name = "dataGrid1";
			this.dataGrid1.Size = new System.Drawing.Size(479, 380);
			this.dataGrid1.TabIndex = 0;
			// 
			// FormMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(704, 509);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.toolBar1);
			this.Controls.Add(this.statusBar1);
			this.Menu = this.mainMenu1;
			this.Name = "FormMain";
			this.Text = "Main";
			this.panel1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void groupBox1_Enter(object sender, System.EventArgs e)
		{
		
		}

		private void treeView1_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
		
		}
	}
}
