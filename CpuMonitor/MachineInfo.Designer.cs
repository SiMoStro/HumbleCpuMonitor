
namespace HumbleCpuMonitor
{
    partial class MachineInfo
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.w_tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.w_lblPhy = new System.Windows.Forms.Label();
            this.w_lblCC = new System.Windows.Forms.Label();
            this.w_lblCpu = new System.Windows.Forms.Label();
            this.w_lblProc1 = new System.Windows.Forms.Label();
            this.w_lblProc2 = new System.Windows.Forms.Label();
            this.w_lblProc3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.w_prgProc1 = new HumbleCpuMonitor.CustomProgressBar();
            this.w_prgProc2 = new HumbleCpuMonitor.CustomProgressBar();
            this.w_prgProc3 = new HumbleCpuMonitor.CustomProgressBar();
            this.w_prgPhy = new HumbleCpuMonitor.CustomProgressBar();
            this.w_prgPageFile = new HumbleCpuMonitor.CustomProgressBar();
            this.w_prgCpu = new HumbleCpuMonitor.CustomProgressBar();
            this.w_tblMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // w_tblMain
            // 
            this.w_tblMain.ColumnCount = 2;
            this.w_tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.w_tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.w_tblMain.Controls.Add(this.w_lblPhy, 0, 0);
            this.w_tblMain.Controls.Add(this.w_prgPhy, 1, 0);
            this.w_tblMain.Controls.Add(this.w_lblCC, 0, 1);
            this.w_tblMain.Controls.Add(this.w_prgPageFile, 1, 1);
            this.w_tblMain.Controls.Add(this.w_lblCpu, 0, 2);
            this.w_tblMain.Controls.Add(this.w_prgCpu, 1, 2);
            this.w_tblMain.Location = new System.Drawing.Point(0, 0);
            this.w_tblMain.Name = "w_tblMain";
            this.w_tblMain.RowCount = 3;
            this.w_tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.w_tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.w_tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.w_tblMain.Size = new System.Drawing.Size(275, 72);
            this.w_tblMain.TabIndex = 0;
            // 
            // w_lblPhy
            // 
            this.w_lblPhy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.w_lblPhy.AutoSize = true;
            this.w_lblPhy.Location = new System.Drawing.Point(3, 0);
            this.w_lblPhy.Name = "w_lblPhy";
            this.w_lblPhy.Size = new System.Drawing.Size(29, 24);
            this.w_lblPhy.TabIndex = 0;
            this.w_lblPhy.Text = "PHY";
            this.w_lblPhy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // w_lblCC
            // 
            this.w_lblCC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.w_lblCC.AutoSize = true;
            this.w_lblCC.Location = new System.Drawing.Point(3, 24);
            this.w_lblCC.Name = "w_lblCC";
            this.w_lblCC.Size = new System.Drawing.Size(21, 24);
            this.w_lblCC.TabIndex = 0;
            this.w_lblCC.Text = "CC";
            this.w_lblCC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // w_lblCpu
            // 
            this.w_lblCpu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.w_lblCpu.AutoSize = true;
            this.w_lblCpu.Location = new System.Drawing.Point(3, 48);
            this.w_lblCpu.Name = "w_lblCpu";
            this.w_lblCpu.Size = new System.Drawing.Size(29, 24);
            this.w_lblCpu.TabIndex = 0;
            this.w_lblCpu.Text = "CPU";
            this.w_lblCpu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // w_lblProc1
            // 
            this.w_lblProc1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.w_lblProc1.AutoSize = true;
            this.w_lblProc1.Location = new System.Drawing.Point(3, 5);
            this.w_lblProc1.Name = "w_lblProc1";
            this.w_lblProc1.Size = new System.Drawing.Size(35, 13);
            this.w_lblProc1.TabIndex = 4;
            this.w_lblProc1.Text = "Proc1";
            this.w_lblProc1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // w_lblProc2
            // 
            this.w_lblProc2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.w_lblProc2.AutoSize = true;
            this.w_lblProc2.Location = new System.Drawing.Point(3, 29);
            this.w_lblProc2.Name = "w_lblProc2";
            this.w_lblProc2.Size = new System.Drawing.Size(35, 13);
            this.w_lblProc2.TabIndex = 4;
            this.w_lblProc2.Text = "Proc2";
            this.w_lblProc2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // w_lblProc3
            // 
            this.w_lblProc3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.w_lblProc3.AutoSize = true;
            this.w_lblProc3.Location = new System.Drawing.Point(3, 55);
            this.w_lblProc3.Name = "w_lblProc3";
            this.w_lblProc3.Size = new System.Drawing.Size(35, 13);
            this.w_lblProc3.TabIndex = 4;
            this.w_lblProc3.Text = "Proc3";
            this.w_lblProc3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.w_lblProc1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.w_lblProc2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.w_lblProc3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.w_prgProc2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.w_prgProc3, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.w_prgProc1, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 75);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(275, 75);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // w_prgProc1
            // 
            this.w_prgProc1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.w_prgProc1.Location = new System.Drawing.Point(53, 3);
            this.w_prgProc1.Maximum = 0;
            this.w_prgProc1.Minimum = 0;
            this.w_prgProc1.Name = "w_prgProc1";
            this.w_prgProc1.Size = new System.Drawing.Size(219, 18);
            this.w_prgProc1.TabIndex = 5;
            this.w_prgProc1.TabStop = false;
            this.w_prgProc1.Text = "<proc1_cpu%>";
            this.w_prgProc1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.w_prgProc1.Value = 0;
            // 
            // w_prgProc2
            // 
            this.w_prgProc2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.w_prgProc2.Location = new System.Drawing.Point(53, 27);
            this.w_prgProc2.Maximum = 0;
            this.w_prgProc2.Minimum = 0;
            this.w_prgProc2.Name = "w_prgProc2";
            this.w_prgProc2.Size = new System.Drawing.Size(219, 18);
            this.w_prgProc2.TabIndex = 6;
            this.w_prgProc2.TabStop = false;
            this.w_prgProc2.Text = "<proc2_cpu%>";
            this.w_prgProc2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.w_prgProc2.Value = 0;
            // 
            // w_prgProc3
            // 
            this.w_prgProc3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.w_prgProc3.Location = new System.Drawing.Point(53, 51);
            this.w_prgProc3.Maximum = 0;
            this.w_prgProc3.Minimum = 0;
            this.w_prgProc3.Name = "w_prgProc3";
            this.w_prgProc3.Size = new System.Drawing.Size(219, 21);
            this.w_prgProc3.TabIndex = 7;
            this.w_prgProc3.TabStop = false;
            this.w_prgProc3.Text = "<proc3_cpu%>";
            this.w_prgProc3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.w_prgProc3.Value = 0;
            // 
            // w_prgPhy
            // 
            this.w_prgPhy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.w_prgPhy.Location = new System.Drawing.Point(53, 3);
            this.w_prgPhy.Maximum = 0;
            this.w_prgPhy.Minimum = 0;
            this.w_prgPhy.Name = "w_prgPhy";
            this.w_prgPhy.Size = new System.Drawing.Size(219, 18);
            this.w_prgPhy.TabIndex = 1;
            this.w_prgPhy.TabStop = false;
            this.w_prgPhy.Text = "<phy_mem>%";
            this.w_prgPhy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.w_prgPhy.Value = 0;
            // 
            // w_prgPageFile
            // 
            this.w_prgPageFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.w_prgPageFile.Location = new System.Drawing.Point(53, 27);
            this.w_prgPageFile.Maximum = 0;
            this.w_prgPageFile.Minimum = 0;
            this.w_prgPageFile.Name = "w_prgPageFile";
            this.w_prgPageFile.Size = new System.Drawing.Size(219, 18);
            this.w_prgPageFile.TabIndex = 2;
            this.w_prgPageFile.TabStop = false;
            this.w_prgPageFile.Text = "<commit_charge>%";
            this.w_prgPageFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.w_prgPageFile.Value = 0;
            // 
            // w_prgCpu
            // 
            this.w_prgCpu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.w_prgCpu.Location = new System.Drawing.Point(53, 51);
            this.w_prgCpu.Maximum = 0;
            this.w_prgCpu.Minimum = 0;
            this.w_prgCpu.Name = "w_prgCpu";
            this.w_prgCpu.Size = new System.Drawing.Size(219, 18);
            this.w_prgCpu.TabIndex = 3;
            this.w_prgCpu.TabStop = false;
            this.w_prgCpu.Text = "<cpu>%";
            this.w_prgCpu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.w_prgCpu.Value = 0;
            // 
            // MachineInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 151);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.w_tblMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MachineInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "MachineInfo";
            this.TopMost = true;
            this.w_tblMain.ResumeLayout(false);
            this.w_tblMain.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel w_tblMain;
        private System.Windows.Forms.Label w_lblPhy;
        private CustomProgressBar w_prgPhy;
        private System.Windows.Forms.Label w_lblCC;
        private CustomProgressBar w_prgPageFile;
        private System.Windows.Forms.Label w_lblCpu;
        private CustomProgressBar w_prgCpu;
        private System.Windows.Forms.Label w_lblProc1;
        private System.Windows.Forms.Label w_lblProc2;
        private System.Windows.Forms.Label w_lblProc3;
        private CustomProgressBar w_prgProc1;
        private CustomProgressBar w_prgProc2;
        private CustomProgressBar w_prgProc3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}