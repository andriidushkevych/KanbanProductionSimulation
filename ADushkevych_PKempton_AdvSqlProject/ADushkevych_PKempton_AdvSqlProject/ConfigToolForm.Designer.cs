namespace ADushkevych_PKempton_AdvSqlProject
{
    partial class ConfigToolForm
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
            this.ConfigInfoDG = new System.Windows.Forms.DataGridView();
            this.updateConfigButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ConfigInfoDG)).BeginInit();
            this.SuspendLayout();
            // 
            // ConfigInfoDG
            // 
            this.ConfigInfoDG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ConfigInfoDG.Location = new System.Drawing.Point(12, 12);
            this.ConfigInfoDG.Name = "ConfigInfoDG";
            this.ConfigInfoDG.Size = new System.Drawing.Size(307, 619);
            this.ConfigInfoDG.TabIndex = 0;
            // 
            // updateConfigButton
            // 
            this.updateConfigButton.Location = new System.Drawing.Point(395, 29);
            this.updateConfigButton.Name = "updateConfigButton";
            this.updateConfigButton.Size = new System.Drawing.Size(191, 50);
            this.updateConfigButton.TabIndex = 1;
            this.updateConfigButton.Text = "Update config";
            this.updateConfigButton.UseVisualStyleBackColor = true;
            this.updateConfigButton.Click += new System.EventHandler(this.updateConfigButton_Click);
            // 
            // ConfigToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 643);
            this.Controls.Add(this.updateConfigButton);
            this.Controls.Add(this.ConfigInfoDG);
            this.Name = "ConfigToolForm";
            this.Text = "ADushkevych_PKempton_AdvSqlProject";
            ((System.ComponentModel.ISupportInitialize)(this.ConfigInfoDG)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ConfigInfoDG;
        private System.Windows.Forms.Button updateConfigButton;
    }
}

