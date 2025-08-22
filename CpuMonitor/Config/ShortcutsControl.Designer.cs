
namespace HumbleCpuMonitor.Config
{
    partial class ShortcutsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.w_pnlMain = new System.Windows.Forms.Panel();
            this.w_nudIdx = new System.Windows.Forms.NumericUpDown();
            this.w_btnClone = new System.Windows.Forms.Button();
            this.w_btnDir = new System.Windows.Forms.Button();
            this.w_tbWorkDir = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.w_cbAdmin = new System.Windows.Forms.CheckBox();
            this.w_btnExe = new System.Windows.Forms.Button();
            this.w_btnAddItem = new System.Windows.Forms.Button();
            this.w_btnRemoveItem = new System.Windows.Forms.Button();
            this.w_tbParameters = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.w_tbExecutable = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.w_tbFiling = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.w_tbDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.w_tbName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.w_btnApply = new System.Windows.Forms.Button();
            this.w_lvItems = new System.Windows.Forms.ListView();
            this.w_chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.w_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.w_errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.w_panelLeft = new System.Windows.Forms.Panel();
            this.w_pnlFilter = new System.Windows.Forms.Panel();
            this.w_tbFilter = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.w_pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.w_nudIdx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.w_errorProvider)).BeginInit();
            this.w_panelLeft.SuspendLayout();
            this.w_pnlFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // w_pnlMain
            // 
            this.w_pnlMain.Controls.Add(this.w_nudIdx);
            this.w_pnlMain.Controls.Add(this.w_btnClone);
            this.w_pnlMain.Controls.Add(this.w_btnDir);
            this.w_pnlMain.Controls.Add(this.w_tbWorkDir);
            this.w_pnlMain.Controls.Add(this.label6);
            this.w_pnlMain.Controls.Add(this.w_cbAdmin);
            this.w_pnlMain.Controls.Add(this.w_btnExe);
            this.w_pnlMain.Controls.Add(this.w_btnAddItem);
            this.w_pnlMain.Controls.Add(this.w_btnRemoveItem);
            this.w_pnlMain.Controls.Add(this.w_tbParameters);
            this.w_pnlMain.Controls.Add(this.label5);
            this.w_pnlMain.Controls.Add(this.w_tbExecutable);
            this.w_pnlMain.Controls.Add(this.label4);
            this.w_pnlMain.Controls.Add(this.w_tbFiling);
            this.w_pnlMain.Controls.Add(this.label7);
            this.w_pnlMain.Controls.Add(this.label3);
            this.w_pnlMain.Controls.Add(this.w_tbDescription);
            this.w_pnlMain.Controls.Add(this.label2);
            this.w_pnlMain.Controls.Add(this.w_tbName);
            this.w_pnlMain.Controls.Add(this.label1);
            this.w_pnlMain.Controls.Add(this.w_btnApply);
            this.w_pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.w_pnlMain.Location = new System.Drawing.Point(137, 0);
            this.w_pnlMain.Name = "w_pnlMain";
            this.w_pnlMain.Size = new System.Drawing.Size(360, 201);
            this.w_pnlMain.TabIndex = 3;
            // 
            // w_nudIdx
            // 
            this.w_nudIdx.Location = new System.Drawing.Point(305, 65);
            this.w_nudIdx.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.w_nudIdx.Name = "w_nudIdx";
            this.w_nudIdx.Size = new System.Drawing.Size(46, 20);
            this.w_nudIdx.TabIndex = 16;
            this.w_nudIdx.ValueChanged += new System.EventHandler(this.HandleNudValueChanged);
            // 
            // w_btnClone
            // 
            this.w_btnClone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.w_btnClone.Location = new System.Drawing.Point(96, 169);
            this.w_btnClone.Name = "w_btnClone";
            this.w_btnClone.Size = new System.Drawing.Size(78, 23);
            this.w_btnClone.TabIndex = 15;
            this.w_btnClone.Text = "Clone item";
            this.w_btnClone.UseVisualStyleBackColor = true;
            this.w_btnClone.Click += new System.EventHandler(this.HandleCloneButtonClick);
            // 
            // w_btnDir
            // 
            this.w_btnDir.Location = new System.Drawing.Point(307, 115);
            this.w_btnDir.Name = "w_btnDir";
            this.w_btnDir.Size = new System.Drawing.Size(43, 23);
            this.w_btnDir.TabIndex = 14;
            this.w_btnDir.Text = "DIR";
            this.w_btnDir.UseVisualStyleBackColor = true;
            this.w_btnDir.Click += new System.EventHandler(this.ChooseDirButtonClick);
            // 
            // w_tbWorkDir
            // 
            this.w_tbWorkDir.Location = new System.Drawing.Point(75, 117);
            this.w_tbWorkDir.Name = "w_tbWorkDir";
            this.w_tbWorkDir.Size = new System.Drawing.Size(216, 20);
            this.w_tbWorkDir.TabIndex = 7;
            this.w_toolTip.SetToolTip(this.w_tbWorkDir, "Executable to launch");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "WorkDir";
            // 
            // w_cbAdmin
            // 
            this.w_cbAdmin.AutoSize = true;
            this.w_cbAdmin.Location = new System.Drawing.Point(297, 16);
            this.w_cbAdmin.Name = "w_cbAdmin";
            this.w_cbAdmin.Size = new System.Drawing.Size(54, 17);
            this.w_cbAdmin.TabIndex = 2;
            this.w_cbAdmin.Text = "admin";
            this.w_cbAdmin.UseVisualStyleBackColor = true;
            this.w_cbAdmin.CheckedChanged += new System.EventHandler(this.CheckBoxAdminCheckedChanged);
            // 
            // w_btnExe
            // 
            this.w_btnExe.Location = new System.Drawing.Point(307, 89);
            this.w_btnExe.Name = "w_btnExe";
            this.w_btnExe.Size = new System.Drawing.Size(43, 23);
            this.w_btnExe.TabIndex = 6;
            this.w_btnExe.Text = "EXE";
            this.w_btnExe.UseVisualStyleBackColor = true;
            this.w_btnExe.Click += new System.EventHandler(this.ChoseExeButtonClick);
            // 
            // w_btnAddItem
            // 
            this.w_btnAddItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.w_btnAddItem.Location = new System.Drawing.Point(8, 169);
            this.w_btnAddItem.Name = "w_btnAddItem";
            this.w_btnAddItem.Size = new System.Drawing.Size(78, 23);
            this.w_btnAddItem.TabIndex = 9;
            this.w_btnAddItem.Text = "Add item";
            this.w_btnAddItem.UseVisualStyleBackColor = true;
            this.w_btnAddItem.Click += new System.EventHandler(this.HandleButtonAddItemClick);
            // 
            // w_btnRemoveItem
            // 
            this.w_btnRemoveItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.w_btnRemoveItem.Location = new System.Drawing.Point(184, 169);
            this.w_btnRemoveItem.Name = "w_btnRemoveItem";
            this.w_btnRemoveItem.Size = new System.Drawing.Size(78, 23);
            this.w_btnRemoveItem.TabIndex = 10;
            this.w_btnRemoveItem.Text = "Remove item";
            this.w_btnRemoveItem.UseVisualStyleBackColor = true;
            this.w_btnRemoveItem.Click += new System.EventHandler(this.HandleRemoveItemClick);
            // 
            // w_tbParameters
            // 
            this.w_tbParameters.Location = new System.Drawing.Point(75, 143);
            this.w_tbParameters.Name = "w_tbParameters";
            this.w_tbParameters.Size = new System.Drawing.Size(276, 20);
            this.w_tbParameters.TabIndex = 8;
            this.w_toolTip.SetToolTip(this.w_tbParameters, "Parameters to the application");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 146);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Parameters";
            // 
            // w_tbExecutable
            // 
            this.w_tbExecutable.Location = new System.Drawing.Point(75, 91);
            this.w_tbExecutable.Name = "w_tbExecutable";
            this.w_tbExecutable.Size = new System.Drawing.Size(216, 20);
            this.w_tbExecutable.TabIndex = 5;
            this.w_toolTip.SetToolTip(this.w_tbExecutable, "Executable to launch");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Executable";
            // 
            // w_tbFiling
            // 
            this.w_tbFiling.Location = new System.Drawing.Point(75, 65);
            this.w_tbFiling.Name = "w_tbFiling";
            this.w_tbFiling.Size = new System.Drawing.Size(197, 20);
            this.w_tbFiling.TabIndex = 4;
            this.w_toolTip.SetToolTip(this.w_tbFiling, "A list of tokens separated by the \";\" character");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(278, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(21, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Idx";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Filing";
            // 
            // w_tbDescription
            // 
            this.w_tbDescription.Location = new System.Drawing.Point(75, 39);
            this.w_tbDescription.Name = "w_tbDescription";
            this.w_tbDescription.Size = new System.Drawing.Size(276, 20);
            this.w_tbDescription.TabIndex = 3;
            this.w_toolTip.SetToolTip(this.w_tbDescription, "Description of the shortcut");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Description";
            // 
            // w_tbName
            // 
            this.w_tbName.Location = new System.Drawing.Point(75, 13);
            this.w_tbName.Name = "w_tbName";
            this.w_tbName.Size = new System.Drawing.Size(216, 20);
            this.w_tbName.TabIndex = 1;
            this.w_toolTip.SetToolTip(this.w_tbName, "Name of the shortcut");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name";
            // 
            // w_btnApply
            // 
            this.w_btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.w_btnApply.Location = new System.Drawing.Point(272, 169);
            this.w_btnApply.Name = "w_btnApply";
            this.w_btnApply.Size = new System.Drawing.Size(78, 23);
            this.w_btnApply.TabIndex = 11;
            this.w_btnApply.Text = "Apply";
            this.w_btnApply.UseVisualStyleBackColor = true;
            this.w_btnApply.Click += new System.EventHandler(this.HandleButtonApplyClick);
            // 
            // w_lvItems
            // 
            this.w_lvItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.w_chName});
            this.w_lvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.w_lvItems.FullRowSelect = true;
            this.w_lvItems.HideSelection = false;
            this.w_lvItems.Location = new System.Drawing.Point(0, 24);
            this.w_lvItems.Name = "w_lvItems";
            this.w_lvItems.Size = new System.Drawing.Size(137, 177);
            this.w_lvItems.TabIndex = 2;
            this.w_lvItems.UseCompatibleStateImageBehavior = false;
            this.w_lvItems.View = System.Windows.Forms.View.Details;
            this.w_lvItems.SelectedIndexChanged += new System.EventHandler(this.ListViewSelectedIndexChanged);
            // 
            // w_chName
            // 
            this.w_chName.Text = "Shortcuts";
            this.w_chName.Width = 119;
            // 
            // w_errorProvider
            // 
            this.w_errorProvider.ContainerControl = this;
            // 
            // w_panelLeft
            // 
            this.w_panelLeft.Controls.Add(this.w_lvItems);
            this.w_panelLeft.Controls.Add(this.w_pnlFilter);
            this.w_panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.w_panelLeft.Location = new System.Drawing.Point(0, 0);
            this.w_panelLeft.Name = "w_panelLeft";
            this.w_panelLeft.Size = new System.Drawing.Size(137, 201);
            this.w_panelLeft.TabIndex = 4;
            // 
            // w_pnlFilter
            // 
            this.w_pnlFilter.Controls.Add(this.w_tbFilter);
            this.w_pnlFilter.Controls.Add(this.label8);
            this.w_pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.w_pnlFilter.Location = new System.Drawing.Point(0, 0);
            this.w_pnlFilter.Name = "w_pnlFilter";
            this.w_pnlFilter.Size = new System.Drawing.Size(137, 24);
            this.w_pnlFilter.TabIndex = 0;
            // 
            // w_tbFilter
            // 
            this.w_tbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.w_tbFilter.Location = new System.Drawing.Point(38, 1);
            this.w_tbFilter.Name = "w_tbFilter";
            this.w_tbFilter.Size = new System.Drawing.Size(96, 20);
            this.w_tbFilter.TabIndex = 1;
            this.w_tbFilter.TextChanged += new System.EventHandler(this.HandleFilterTextChanged);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Filter";
            // 
            // ShortcutsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.w_pnlMain);
            this.Controls.Add(this.w_panelLeft);
            this.Name = "ShortcutsControl";
            this.Size = new System.Drawing.Size(497, 201);
            this.w_pnlMain.ResumeLayout(false);
            this.w_pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.w_nudIdx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.w_errorProvider)).EndInit();
            this.w_panelLeft.ResumeLayout(false);
            this.w_pnlFilter.ResumeLayout(false);
            this.w_pnlFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel w_pnlMain;
        private System.Windows.Forms.Button w_btnExe;
        private System.Windows.Forms.Button w_btnAddItem;
        private System.Windows.Forms.Button w_btnRemoveItem;
        private System.Windows.Forms.TextBox w_tbParameters;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox w_tbExecutable;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox w_tbFiling;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox w_tbDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox w_tbName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button w_btnApply;
        private System.Windows.Forms.ListView w_lvItems;
        private System.Windows.Forms.ToolTip w_toolTip;
        private System.Windows.Forms.CheckBox w_cbAdmin;
        private System.Windows.Forms.TextBox w_tbWorkDir;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button w_btnDir;
        private System.Windows.Forms.Button w_btnClone;
        private System.Windows.Forms.ErrorProvider w_errorProvider;
        private System.Windows.Forms.ColumnHeader w_chName;
        private System.Windows.Forms.NumericUpDown w_nudIdx;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel w_panelLeft;
        private System.Windows.Forms.Panel w_pnlFilter;
        private System.Windows.Forms.TextBox w_tbFilter;
        private System.Windows.Forms.Label label8;
    }
}
