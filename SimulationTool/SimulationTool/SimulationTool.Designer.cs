namespace SimulationTool
{
    partial class SimulationTool
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
            this.createWorkstationButton = new System.Windows.Forms.Button();
            this.empTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.startLampsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // createWorkstationButton
            // 
            this.createWorkstationButton.Location = new System.Drawing.Point(37, 97);
            this.createWorkstationButton.Name = "createWorkstationButton";
            this.createWorkstationButton.Size = new System.Drawing.Size(396, 34);
            this.createWorkstationButton.TabIndex = 0;
            this.createWorkstationButton.Text = "Create workstation with employee";
            this.createWorkstationButton.UseVisualStyleBackColor = true;
            this.createWorkstationButton.Click += new System.EventHandler(this.createWorkstationButton_Click);
            // 
            // empTypeComboBox
            // 
            this.empTypeComboBox.FormattingEnabled = true;
            this.empTypeComboBox.Location = new System.Drawing.Point(37, 51);
            this.empTypeComboBox.Name = "empTypeComboBox";
            this.empTypeComboBox.Size = new System.Drawing.Size(396, 21);
            this.empTypeComboBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Choose employee type";
            // 
            // startLampsButton
            // 
            this.startLampsButton.Enabled = false;
            this.startLampsButton.Location = new System.Drawing.Point(37, 161);
            this.startLampsButton.Name = "startLampsButton";
            this.startLampsButton.Size = new System.Drawing.Size(396, 34);
            this.startLampsButton.TabIndex = 3;
            this.startLampsButton.Text = "Make some lamps";
            this.startLampsButton.UseVisualStyleBackColor = true;
            this.startLampsButton.Click += new System.EventHandler(this.startLampsButton_Click);
            // 
            // SimulationTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 459);
            this.Controls.Add(this.startLampsButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.empTypeComboBox);
            this.Controls.Add(this.createWorkstationButton);
            this.Name = "SimulationTool";
            this.Text = "Simulation Tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button createWorkstationButton;
        private System.Windows.Forms.ComboBox empTypeComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button startLampsButton;
    }
}

