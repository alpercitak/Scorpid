namespace Scorpid.Forms
{
    partial class frmSendFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSendFile));
            this.tx_Filename = new DevExpress.XtraEditors.TextEdit();
            this.tx_Recepient = new DevExpress.XtraEditors.TextEdit();
            this.bn_OK = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.lb_File = new DevExpress.XtraEditors.LabelControl();
            this.lb_Recepient = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.tx_Filename.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tx_Recepient.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tx_Filename
            // 
            this.tx_Filename.Location = new System.Drawing.Point(62, 8);
            this.tx_Filename.Name = "tx_Filename";
            this.tx_Filename.Properties.ReadOnly = true;
            this.tx_Filename.Size = new System.Drawing.Size(216, 20);
            this.tx_Filename.TabIndex = 5;
            this.tx_Filename.TextChanged += new System.EventHandler(this.tx_Common_TextChanged);
            this.tx_Filename.DoubleClick += new System.EventHandler(this.tx_Filename_DoubleClick);
            // 
            // tx_Recepient
            // 
            this.tx_Recepient.Location = new System.Drawing.Point(62, 34);
            this.tx_Recepient.Name = "tx_Recepient";
            this.tx_Recepient.Size = new System.Drawing.Size(216, 20);
            this.tx_Recepient.TabIndex = 10;
            this.tx_Recepient.TextChanged += new System.EventHandler(this.tx_Common_TextChanged);
            // 
            // bn_OK
            // 
            this.bn_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bn_OK.Location = new System.Drawing.Point(8, 60);
            this.bn_OK.Name = "bn_OK";
            this.bn_OK.Size = new System.Drawing.Size(75, 23);
            this.bn_OK.TabIndex = 15;
            this.bn_OK.Text = "OK";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Location = new System.Drawing.Point(203, 60);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 20;
            this.simpleButton2.Text = "Cancel";
            // 
            // lb_File
            // 
            this.lb_File.Location = new System.Drawing.Point(8, 10);
            this.lb_File.Name = "lb_File";
            this.lb_File.Size = new System.Drawing.Size(16, 13);
            this.lb_File.TabIndex = 21;
            this.lb_File.Text = "File";
            // 
            // lb_Recepient
            // 
            this.lb_Recepient.Location = new System.Drawing.Point(8, 36);
            this.lb_Recepient.Name = "lb_Recepient";
            this.lb_Recepient.Size = new System.Drawing.Size(48, 13);
            this.lb_Recepient.TabIndex = 22;
            this.lb_Recepient.Text = "Recepient";
            // 
            // frmSendFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 92);
            this.Controls.Add(this.lb_Recepient);
            this.Controls.Add(this.lb_File);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.bn_OK);
            this.Controls.Add(this.tx_Recepient);
            this.Controls.Add(this.tx_Filename);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 130);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 130);
            this.Name = "frmSendFile";
            this.Text = "Scorpid";
            this.Load += new System.EventHandler(this.frmSendFile_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tx_Filename.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tx_Recepient.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit tx_Filename;
        private DevExpress.XtraEditors.TextEdit tx_Recepient;
        private DevExpress.XtraEditors.SimpleButton bn_OK;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.LabelControl lb_File;
        private DevExpress.XtraEditors.LabelControl lb_Recepient;
    }
}