namespace BinanceChecker
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            nTotal = new Label();
            nValid = new Label();
            label4 = new Label();
            nInvalid = new Label();
            label6 = new Label();
            progressBar1 = new ProgressBar();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(100, 106);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "Start";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(206, 106);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 1;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(27, 26);
            label1.Name = "label1";
            label1.Size = new Size(45, 21);
            label1.TabIndex = 2;
            label1.Text = "Total:";
            // 
            // nTotal
            // 
            nTotal.AutoSize = true;
            nTotal.Font = new Font("Segoe UI", 12F);
            nTotal.Location = new Point(70, 26);
            nTotal.Name = "nTotal";
            nTotal.Size = new Size(19, 21);
            nTotal.TabIndex = 3;
            nTotal.Text = "...";
            // 
            // nValid
            // 
            nValid.AutoSize = true;
            nValid.Font = new Font("Segoe UI", 12F);
            nValid.Location = new Point(197, 26);
            nValid.Name = "nValid";
            nValid.Size = new Size(19, 21);
            nValid.TabIndex = 5;
            nValid.Text = "0";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(152, 26);
            label4.Name = "label4";
            label4.Size = new Size(47, 21);
            label4.TabIndex = 4;
            label4.Text = "Valid:";
            // 
            // nInvalid
            // 
            nInvalid.AutoSize = true;
            nInvalid.Font = new Font("Segoe UI", 12F);
            nInvalid.Location = new Point(327, 26);
            nInvalid.Name = "nInvalid";
            nInvalid.Size = new Size(19, 21);
            nInvalid.TabIndex = 7;
            nInvalid.Text = "0";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F);
            label6.Location = new Point(270, 26);
            label6.Name = "label6";
            label6.Size = new Size(59, 21);
            label6.TabIndex = 6;
            label6.Text = "Invalid:";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 65);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(352, 23);
            progressBar1.Step = 1;
            progressBar1.TabIndex = 8;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(376, 139);
            Controls.Add(progressBar1);
            Controls.Add(nInvalid);
            Controls.Add(label6);
            Controls.Add(nValid);
            Controls.Add(label4);
            Controls.Add(nTotal);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Binance Checker";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Label label1;
        private Label nTotal;
        private Label nValid;
        private Label label4;
        private Label nInvalid;
        private Label label6;
        private ProgressBar progressBar1;
    }
}
