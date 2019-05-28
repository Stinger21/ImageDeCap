namespace imageDeCap
{
    partial class AboutWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutWindow));
            this.imageContainer = new System.Windows.Forms.PictureBox();
            this.Website = new System.Windows.Forms.Label();
            this.Contributors = new System.Windows.Forms.Label();
            this.Andrew = new System.Windows.Forms.Label();
            this.Alastair = new System.Windows.Forms.Label();
            this.Peter = new System.Windows.Forms.Label();
            this.ImageDecapLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imageContainer)).BeginInit();
            this.SuspendLayout();
            // 
            // imageContainer
            // 
            this.imageContainer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.imageContainer.Image = ((System.Drawing.Image)(resources.GetObject("imageContainer.Image")));
            this.imageContainer.Location = new System.Drawing.Point(10, 9);
            this.imageContainer.Name = "imageContainer";
            this.imageContainer.Size = new System.Drawing.Size(64, 64);
            this.imageContainer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageContainer.TabIndex = 2;
            this.imageContainer.TabStop = false;
            // 
            // Website
            // 
            this.Website.AutoSize = true;
            this.Website.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Website.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Website.ForeColor = System.Drawing.Color.RoyalBlue;
            this.Website.Location = new System.Drawing.Point(80, 26);
            this.Website.Name = "Website";
            this.Website.Size = new System.Drawing.Size(89, 13);
            this.Website.TabIndex = 7;
            this.Website.Text = "Mattias Westphal";
            // 
            // Contributors
            // 
            this.Contributors.AutoSize = true;
            this.Contributors.Location = new System.Drawing.Point(80, 60);
            this.Contributors.Name = "Contributors";
            this.Contributors.Size = new System.Drawing.Size(63, 13);
            this.Contributors.TabIndex = 11;
            this.Contributors.Text = "Contributors";
            // 
            // Andrew
            // 
            this.Andrew.AutoSize = true;
            this.Andrew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Andrew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Andrew.ForeColor = System.Drawing.Color.RoyalBlue;
            this.Andrew.Location = new System.Drawing.Point(80, 73);
            this.Andrew.Name = "Andrew";
            this.Andrew.Size = new System.Drawing.Size(125, 13);
            this.Andrew.TabIndex = 12;
            this.Andrew.Text = "Andrew Newton - Design";
            this.Andrew.Click += new System.EventHandler(this.Andrew_Click);
            // 
            // Alastair
            // 
            this.Alastair.AutoSize = true;
            this.Alastair.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Alastair.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Alastair.ForeColor = System.Drawing.Color.RoyalBlue;
            this.Alastair.Location = new System.Drawing.Point(80, 88);
            this.Alastair.Name = "Alastair";
            this.Alastair.Size = new System.Drawing.Size(105, 13);
            this.Alastair.TabIndex = 13;
            this.Alastair.Text = "Alastair Stuart - Fixes";
            this.Alastair.Click += new System.EventHandler(this.Alastair_Click);
            // 
            // Peter
            // 
            this.Peter.AutoSize = true;
            this.Peter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Peter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Peter.ForeColor = System.Drawing.Color.RoyalBlue;
            this.Peter.Location = new System.Drawing.Point(80, 103);
            this.Peter.Name = "Peter";
            this.Peter.Size = new System.Drawing.Size(109, 13);
            this.Peter.TabIndex = 14;
            this.Peter.Text = "Peter Lindgren - Fixes";
            this.Peter.Click += new System.EventHandler(this.Peter_Click);
            // 
            // ImageDecapLabel
            // 
            this.ImageDecapLabel.AutoSize = true;
            this.ImageDecapLabel.Cursor = System.Windows.Forms.Cursors.No;
            this.ImageDecapLabel.Location = new System.Drawing.Point(80, 9);
            this.ImageDecapLabel.Name = "ImageDecapLabel";
            this.ImageDecapLabel.Size = new System.Drawing.Size(100, 13);
            this.ImageDecapLabel.TabIndex = 15;
            this.ImageDecapLabel.Text = "Image Decap Label";
            // 
            // AboutWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 133);
            this.Controls.Add(this.ImageDecapLabel);
            this.Controls.Add(this.Peter);
            this.Controls.Add(this.Alastair);
            this.Controls.Add(this.Andrew);
            this.Controls.Add(this.Contributors);
            this.Controls.Add(this.Website);
            this.Controls.Add(this.imageContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutWindow";
            this.Text = "About ImageDeCap";
            ((System.ComponentModel.ISupportInitialize)(this.imageContainer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox imageContainer;
        private System.Windows.Forms.Label Website;
        private System.Windows.Forms.Label Contributors;
        private System.Windows.Forms.Label Andrew;
        private System.Windows.Forms.Label Alastair;
        private System.Windows.Forms.Label Peter;
        private System.Windows.Forms.Label ImageDecapLabel;
    }
}