
namespace Lexorama.NeuralDesktopMemoQ
{
    partial class NeuralDesktopOptionsForm
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBoxFramework = new System.Windows.Forms.GroupBox();
            this.rButtonLocal = new System.Windows.Forms.RadioButton();
            this.rButtonWizard = new System.Windows.Forms.RadioButton();
            this.rButtonLua = new System.Windows.Forms.RadioButton();
            this.groupBoxCon = new System.Windows.Forms.GroupBox();
            this.port_txtbox = new System.Windows.Forms.TextBox();
            this.address_txtbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.Cancel_btn = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Save_btn = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBoxFramework.SuspendLayout();
            this.groupBoxCon.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(11, 11);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(332, 266);
            this.tabControl1.TabIndex = 29;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.groupBoxFramework);
            this.tabPage1.Controls.Add(this.groupBoxCon);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(324, 240);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basic";
            // 
            // groupBoxFramework
            // 
            this.groupBoxFramework.Controls.Add(this.rButtonLocal);
            this.groupBoxFramework.Controls.Add(this.rButtonWizard);
            this.groupBoxFramework.Controls.Add(this.rButtonLua);
            this.groupBoxFramework.Location = new System.Drawing.Point(2, 8);
            this.groupBoxFramework.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxFramework.Name = "groupBoxFramework";
            this.groupBoxFramework.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxFramework.Size = new System.Drawing.Size(312, 99);
            this.groupBoxFramework.TabIndex = 25;
            this.groupBoxFramework.TabStop = false;
            this.groupBoxFramework.Text = "Framework";
            // 
            // rButtonLocal
            // 
            this.rButtonLocal.AutoSize = true;
            this.rButtonLocal.Location = new System.Drawing.Point(7, 77);
            this.rButtonLocal.Margin = new System.Windows.Forms.Padding(2);
            this.rButtonLocal.Name = "rButtonLocal";
            this.rButtonLocal.Size = new System.Drawing.Size(90, 17);
            this.rButtonLocal.TabIndex = 2;
            this.rButtonLocal.TabStop = true;
            this.rButtonLocal.Text = "Local Service";
            this.rButtonLocal.UseVisualStyleBackColor = true;
            // 
            // rButtonWizard
            // 
            this.rButtonWizard.AutoSize = true;
            this.rButtonWizard.Location = new System.Drawing.Point(7, 47);
            this.rButtonWizard.Margin = new System.Windows.Forms.Padding(2);
            this.rButtonWizard.Name = "rButtonWizard";
            this.rButtonWizard.Size = new System.Drawing.Size(85, 17);
            this.rButtonWizard.TabIndex = 1;
            this.rButtonWizard.TabStop = true;
            this.rButtonWizard.Text = "NMT Wizard";
            this.rButtonWizard.UseVisualStyleBackColor = true;
            // 
            // rButtonLua
            // 
            this.rButtonLua.AutoSize = true;
            this.rButtonLua.Location = new System.Drawing.Point(7, 17);
            this.rButtonLua.Margin = new System.Windows.Forms.Padding(2);
            this.rButtonLua.Name = "rButtonLua";
            this.rButtonLua.Size = new System.Drawing.Size(96, 17);
            this.rButtonLua.TabIndex = 0;
            this.rButtonLua.TabStop = true;
            this.rButtonLua.Text = "OpenNMT Lua";
            this.rButtonLua.UseVisualStyleBackColor = true;
            // 
            // groupBoxCon
            // 
            this.groupBoxCon.Controls.Add(this.port_txtbox);
            this.groupBoxCon.Controls.Add(this.address_txtbox);
            this.groupBoxCon.Controls.Add(this.label4);
            this.groupBoxCon.Controls.Add(this.label3);
            this.groupBoxCon.Location = new System.Drawing.Point(4, 121);
            this.groupBoxCon.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxCon.Name = "groupBoxCon";
            this.groupBoxCon.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxCon.Size = new System.Drawing.Size(310, 117);
            this.groupBoxCon.TabIndex = 24;
            this.groupBoxCon.TabStop = false;
            this.groupBoxCon.Text = "Connection";
            // 
            // port_txtbox
            // 
            this.port_txtbox.AccessibleName = "server_port";
            this.port_txtbox.Location = new System.Drawing.Point(88, 50);
            this.port_txtbox.Name = "port_txtbox";
            this.port_txtbox.Size = new System.Drawing.Size(86, 20);
            this.port_txtbox.TabIndex = 16;
            // 
            // address_txtbox
            // 
            this.address_txtbox.AccessibleName = "server_address";
            this.address_txtbox.Location = new System.Drawing.Point(88, 25);
            this.address_txtbox.Name = "address_txtbox";
            this.address_txtbox.Size = new System.Drawing.Size(192, 20);
            this.address_txtbox.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Server Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Server Address";
            // 
            // Cancel_btn
            // 
            this.Cancel_btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_btn.Location = new System.Drawing.Point(260, 283);
            this.Cancel_btn.Name = "Cancel_btn";
            this.Cancel_btn.Size = new System.Drawing.Size(80, 23);
            this.Cancel_btn.TabIndex = 28;
            this.Cancel_btn.Text = "Cancel";
            this.Cancel_btn.UseVisualStyleBackColor = true;
            // 
            // Save_btn
            // 
            this.Save_btn.Location = new System.Drawing.Point(160, 283);
            this.Save_btn.Name = "Save_btn";
            this.Save_btn.Size = new System.Drawing.Size(80, 23);
            this.Save_btn.TabIndex = 27;
            this.Save_btn.Text = "Save";
            this.Save_btn.UseVisualStyleBackColor = true;
            this.Save_btn.Click += new System.EventHandler(this.Save_btn_Click);
            // 
            // NeuralDesktopOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 329);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.Cancel_btn);
            this.Controls.Add(this.Save_btn);
            this.Name = "NeuralDesktopOptionsForm";
            this.Text = "NeuralDesktopOptionsForm";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBoxFramework.ResumeLayout(false);
            this.groupBoxFramework.PerformLayout();
            this.groupBoxCon.ResumeLayout(false);
            this.groupBoxCon.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBoxFramework;
        private System.Windows.Forms.RadioButton rButtonLocal;
        private System.Windows.Forms.RadioButton rButtonWizard;
        private System.Windows.Forms.RadioButton rButtonLua;
        private System.Windows.Forms.GroupBox groupBoxCon;
        private System.Windows.Forms.TextBox port_txtbox;
        private System.Windows.Forms.TextBox address_txtbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.Button Cancel_btn;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button Save_btn;
    }
}