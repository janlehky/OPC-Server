namespace OPC_Server
{
    partial class Form1
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnServerStart = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.stsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ipAddress_form = new System.Windows.Forms.TextBox();
            this.port_form = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.opcServerAddress = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 84);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(168, 46);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect to RSLinx";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnServerStart
            // 
            this.btnServerStart.Location = new System.Drawing.Point(12, 32);
            this.btnServerStart.Name = "btnServerStart";
            this.btnServerStart.Size = new System.Drawing.Size(168, 46);
            this.btnServerStart.TabIndex = 3;
            this.btnServerStart.Text = "Start Server";
            this.btnServerStart.UseVisualStyleBackColor = true;
            this.btnServerStart.Click += new System.EventHandler(this.btnServerStart_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stsLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 145);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(464, 25);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // stsLabel
            // 
            this.stsLabel.Name = "stsLabel";
            this.stsLabel.Size = new System.Drawing.Size(35, 20);
            this.stsLabel.Text = "Info";
            // 
            // ipAddress_form
            // 
            this.ipAddress_form.Location = new System.Drawing.Point(186, 44);
            this.ipAddress_form.Name = "ipAddress_form";
            this.ipAddress_form.Size = new System.Drawing.Size(150, 22);
            this.ipAddress_form.TabIndex = 8;
            // 
            // port_form
            // 
            this.port_form.Location = new System.Drawing.Point(342, 44);
            this.port_form.Name = "port_form";
            this.port_form.Size = new System.Drawing.Size(52, 22);
            this.port_form.TabIndex = 9;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // opcServerAddress
            // 
            this.opcServerAddress.Location = new System.Drawing.Point(186, 96);
            this.opcServerAddress.Name = "opcServerAddress";
            this.opcServerAddress.Size = new System.Drawing.Size(260, 22);
            this.opcServerAddress.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 170);
            this.Controls.Add(this.opcServerAddress);
            this.Controls.Add(this.port_form);
            this.Controls.Add(this.ipAddress_form);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnServerStart);
            this.Controls.Add(this.btnConnect);
            this.Name = "Form1";
            this.Text = "OPC Server";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnServerStart;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel stsLabel;
        private System.Windows.Forms.TextBox ipAddress_form;
        private System.Windows.Forms.TextBox port_form;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox opcServerAddress;
    }
}

