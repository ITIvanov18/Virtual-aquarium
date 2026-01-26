namespace AquariumProject
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.рибиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавиРибкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.запишиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.заредиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.рибиToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "Рибки";
            // 
            // рибиToolStripMenuItem
            // 
            this.рибиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавиРибкаToolStripMenuItem});
            this.рибиToolStripMenuItem.Name = "рибиToolStripMenuItem";
            this.рибиToolStripMenuItem.Size = new System.Drawing.Size(68, 29);
            this.рибиToolStripMenuItem.Text = "Риби";
            // 
            // добавиРибкаToolStripMenuItem
            // 
            this.добавиРибкаToolStripMenuItem.Name = "добавиРибкаToolStripMenuItem";
            this.добавиРибкаToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.добавиРибкаToolStripMenuItem.Text = "Добави рибка";
            this.добавиРибкаToolStripMenuItem.Click += new System.EventHandler(this.добавиРибкаToolStripMenuItem_Click);
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.запишиToolStripMenuItem,
            this.заредиToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(69, 29);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // запишиToolStripMenuItem
            // 
            this.запишиToolStripMenuItem.Name = "запишиToolStripMenuItem";
            this.запишиToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.запишиToolStripMenuItem.Text = "Запиши";
            this.запишиToolStripMenuItem.Click += new System.EventHandler(this.запишиToolStripMenuItem_Click);
            // 
            // заредиToolStripMenuItem
            // 
            this.заредиToolStripMenuItem.Name = "заредиToolStripMenuItem";
            this.заредиToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.заредиToolStripMenuItem.Text = "Зареди от файл";
            this.заредиToolStripMenuItem.Click += new System.EventHandler(this.заредиToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem рибиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавиРибкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem запишиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem заредиToolStripMenuItem;
    }
}

