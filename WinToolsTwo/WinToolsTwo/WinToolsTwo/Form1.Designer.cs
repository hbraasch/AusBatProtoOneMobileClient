
namespace WinToolsTwo
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxNodeId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.textBoxPickers = new System.Windows.Forms.TextBox();
            this.textBoxNumerics = new System.Windows.Forms.TextBox();
            this.buttonParse = new System.Windows.Forms.Button();
            this.textBoxSource = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonGetKeyNode = new System.Windows.Forms.Button();
            this.buttonCreateTree = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.buttonRenameDistFiles = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1556, 641);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonSave);
            this.tabPage1.Controls.Add(this.buttonLoad);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.textBoxNodeId);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.buttonGenerate);
            this.tabPage1.Controls.Add(this.textBoxPickers);
            this.tabPage1.Controls.Add(this.textBoxNumerics);
            this.tabPage1.Controls.Add(this.buttonParse);
            this.tabPage1.Controls.Add(this.textBoxSource);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1548, 608);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Keys";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(818, 99);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(178, 35);
            this.buttonSave.TabIndex = 11;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(818, 45);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(178, 35);
            this.buttonLoad.TabIndex = 10;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 226);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "NodeId";
            // 
            // textBoxNodeId
            // 
            this.textBoxNodeId.Location = new System.Drawing.Point(95, 223);
            this.textBoxNodeId.Multiline = true;
            this.textBoxNodeId.Name = "textBoxNodeId";
            this.textBoxNodeId.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxNodeId.Size = new System.Drawing.Size(580, 26);
            this.textBoxNodeId.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(365, 252);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Options";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 249);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Numerics";
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(248, 548);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(178, 35);
            this.buttonGenerate.TabIndex = 4;
            this.buttonGenerate.Text = "Generate";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // textBoxPickers
            // 
            this.textBoxPickers.Location = new System.Drawing.Point(364, 275);
            this.textBoxPickers.Multiline = true;
            this.textBoxPickers.Name = "textBoxPickers";
            this.textBoxPickers.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxPickers.Size = new System.Drawing.Size(926, 248);
            this.textBoxPickers.TabIndex = 3;
            this.textBoxPickers.WordWrap = false;
            // 
            // textBoxNumerics
            // 
            this.textBoxNumerics.Location = new System.Drawing.Point(18, 275);
            this.textBoxNumerics.Multiline = true;
            this.textBoxNumerics.Name = "textBoxNumerics";
            this.textBoxNumerics.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxNumerics.Size = new System.Drawing.Size(311, 248);
            this.textBoxNumerics.TabIndex = 2;
            this.textBoxNumerics.WordWrap = false;
            // 
            // buttonParse
            // 
            this.buttonParse.Location = new System.Drawing.Point(248, 182);
            this.buttonParse.Name = "buttonParse";
            this.buttonParse.Size = new System.Drawing.Size(178, 35);
            this.buttonParse.TabIndex = 1;
            this.buttonParse.Text = "Parse";
            this.buttonParse.UseVisualStyleBackColor = true;
            this.buttonParse.Click += new System.EventHandler(this.buttonParse_Click);
            // 
            // textBoxSource
            // 
            this.textBoxSource.Location = new System.Drawing.Point(18, 19);
            this.textBoxSource.Multiline = true;
            this.textBoxSource.Name = "textBoxSource";
            this.textBoxSource.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSource.Size = new System.Drawing.Size(657, 157);
            this.textBoxSource.TabIndex = 0;
            this.textBoxSource.WordWrap = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonGetKeyNode);
            this.tabPage2.Controls.Add(this.buttonCreateTree);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1548, 608);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tree";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonGetKeyNode
            // 
            this.buttonGetKeyNode.Location = new System.Drawing.Point(84, 80);
            this.buttonGetKeyNode.Name = "buttonGetKeyNode";
            this.buttonGetKeyNode.Size = new System.Drawing.Size(234, 29);
            this.buttonGetKeyNode.TabIndex = 1;
            this.buttonGetKeyNode.Text = "GetKeyNode";
            this.buttonGetKeyNode.UseVisualStyleBackColor = true;
            this.buttonGetKeyNode.Click += new System.EventHandler(this.buttonGetKeyNode_Click);
            // 
            // buttonCreateTree
            // 
            this.buttonCreateTree.Location = new System.Drawing.Point(84, 32);
            this.buttonCreateTree.Name = "buttonCreateTree";
            this.buttonCreateTree.Size = new System.Drawing.Size(94, 29);
            this.buttonCreateTree.TabIndex = 0;
            this.buttonCreateTree.Text = "CreateTree";
            this.buttonCreateTree.UseVisualStyleBackColor = true;
            this.buttonCreateTree.Click += new System.EventHandler(this.buttonCreateTree_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.buttonRenameDistFiles);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1548, 608);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Rename";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // buttonRenameDistFiles
            // 
            this.buttonRenameDistFiles.Location = new System.Drawing.Point(113, 46);
            this.buttonRenameDistFiles.Name = "buttonRenameDistFiles";
            this.buttonRenameDistFiles.Size = new System.Drawing.Size(193, 29);
            this.buttonRenameDistFiles.TabIndex = 0;
            this.buttonRenameDistFiles.Text = "Rename dist files";
            this.buttonRenameDistFiles.UseVisualStyleBackColor = true;
            this.buttonRenameDistFiles.Click += new System.EventHandler(this.buttonRenameDistFiles_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1556, 641);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxNodeId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.TextBox textBoxPickers;
        private System.Windows.Forms.TextBox textBoxNumerics;
        private System.Windows.Forms.Button buttonParse;
        private System.Windows.Forms.TextBox textBoxSource;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonCreateTree;
        private System.Windows.Forms.Button buttonGetKeyNode;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button buttonRenameDistFiles;
    }
}

