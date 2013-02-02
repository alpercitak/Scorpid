namespace Scorpid.Forms
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.dgRcv = new DevExpress.XtraGrid.GridControl();
            this.objGVRcv = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.dgSnd = new DevExpress.XtraGrid.GridControl();
            this.objGVSnd = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgRcv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objGVRcv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objGVSnd)).BeginInit();
            this.SuspendLayout();
            // 
            // dgRcv
            // 
            this.dgRcv.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgRcv.Location = new System.Drawing.Point(0, 0);
            this.dgRcv.MainView = this.objGVRcv;
            this.dgRcv.Name = "dgRcv";
            this.dgRcv.Size = new System.Drawing.Size(984, 280);
            this.dgRcv.TabIndex = 0;
            this.dgRcv.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.objGVRcv});
            this.dgRcv.DoubleClick += new System.EventHandler(this.dgRcv_DoubleClick);
            // 
            // objGVRcv
            // 
            this.objGVRcv.GridControl = this.dgRcv;
            this.objGVRcv.Name = "objGVRcv";
            // 
            // dgSnd
            // 
            this.dgSnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgSnd.Location = new System.Drawing.Point(0, 280);
            this.dgSnd.MainView = this.objGVSnd;
            this.dgSnd.Name = "dgSnd";
            this.dgSnd.Size = new System.Drawing.Size(984, 282);
            this.dgSnd.TabIndex = 1;
            this.dgSnd.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.objGVSnd});
            // 
            // objGVSnd
            // 
            this.objGVSnd.GridControl = this.dgSnd;
            this.objGVSnd.Name = "objGVSnd";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 562);
            this.Controls.Add(this.dgSnd);
            this.Controls.Add(this.dgRcv);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "Scorpid";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgRcv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objGVRcv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objGVSnd)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl dgRcv;
        private DevExpress.XtraGrid.Views.Grid.GridView objGVRcv;
        private DevExpress.XtraGrid.GridControl dgSnd;
        private DevExpress.XtraGrid.Views.Grid.GridView objGVSnd;
    }
}

