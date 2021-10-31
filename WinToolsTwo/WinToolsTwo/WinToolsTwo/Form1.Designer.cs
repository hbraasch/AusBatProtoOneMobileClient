
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
            this.buttonGenerateJsonFile = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonRenameDistFiles = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.buttonSave2 = new System.Windows.Forms.Button();
            this.buttonLoad2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxNodeId2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonGenerateDatasetsWithRegions = new System.Windows.Forms.Button();
            this.textBoxRegions = new System.Windows.Forms.TextBox();
            this.textBoxSpecies = new System.Windows.Forms.TextBox();
            this.buttonParseForRegions = new System.Windows.Forms.Button();
            this.textBoxRegionsSource = new System.Windows.Forms.TextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.buttonLoadAudio = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1362, 545);
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
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1354, 515);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Keys";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(716, 84);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(156, 30);
            this.buttonSave.TabIndex = 11;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(716, 38);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(156, 30);
            this.buttonLoad.TabIndex = 10;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 192);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "NodeId";
            // 
            // textBoxNodeId
            // 
            this.textBoxNodeId.Location = new System.Drawing.Point(83, 190);
            this.textBoxNodeId.Multiline = true;
            this.textBoxNodeId.Name = "textBoxNodeId";
            this.textBoxNodeId.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxNodeId.Size = new System.Drawing.Size(508, 23);
            this.textBoxNodeId.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(319, 214);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Options";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 212);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Numerics";
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(217, 466);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(156, 30);
            this.buttonGenerate.TabIndex = 4;
            this.buttonGenerate.Text = "Generate";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // textBoxPickers
            // 
            this.textBoxPickers.Location = new System.Drawing.Point(318, 234);
            this.textBoxPickers.Multiline = true;
            this.textBoxPickers.Name = "textBoxPickers";
            this.textBoxPickers.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxPickers.Size = new System.Drawing.Size(811, 211);
            this.textBoxPickers.TabIndex = 3;
            this.textBoxPickers.WordWrap = false;
            // 
            // textBoxNumerics
            // 
            this.textBoxNumerics.Location = new System.Drawing.Point(16, 234);
            this.textBoxNumerics.Multiline = true;
            this.textBoxNumerics.Name = "textBoxNumerics";
            this.textBoxNumerics.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxNumerics.Size = new System.Drawing.Size(273, 211);
            this.textBoxNumerics.TabIndex = 2;
            this.textBoxNumerics.WordWrap = false;
            // 
            // buttonParse
            // 
            this.buttonParse.Location = new System.Drawing.Point(217, 155);
            this.buttonParse.Name = "buttonParse";
            this.buttonParse.Size = new System.Drawing.Size(156, 30);
            this.buttonParse.TabIndex = 1;
            this.buttonParse.Text = "Parse";
            this.buttonParse.UseVisualStyleBackColor = true;
            this.buttonParse.Click += new System.EventHandler(this.buttonParse_Click);
            // 
            // textBoxSource
            // 
            this.textBoxSource.Location = new System.Drawing.Point(16, 16);
            this.textBoxSource.Multiline = true;
            this.textBoxSource.Name = "textBoxSource";
            this.textBoxSource.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSource.Size = new System.Drawing.Size(575, 134);
            this.textBoxSource.TabIndex = 0;
            this.textBoxSource.WordWrap = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonGetKeyNode);
            this.tabPage2.Controls.Add(this.buttonCreateTree);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1354, 515);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tree";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonGetKeyNode
            // 
            this.buttonGetKeyNode.Location = new System.Drawing.Point(74, 68);
            this.buttonGetKeyNode.Name = "buttonGetKeyNode";
            this.buttonGetKeyNode.Size = new System.Drawing.Size(205, 25);
            this.buttonGetKeyNode.TabIndex = 1;
            this.buttonGetKeyNode.Text = "GetKeyNode";
            this.buttonGetKeyNode.UseVisualStyleBackColor = true;
            this.buttonGetKeyNode.Click += new System.EventHandler(this.buttonGetKeyNode_Click);
            // 
            // buttonCreateTree
            // 
            this.buttonCreateTree.Location = new System.Drawing.Point(74, 27);
            this.buttonCreateTree.Name = "buttonCreateTree";
            this.buttonCreateTree.Size = new System.Drawing.Size(82, 25);
            this.buttonCreateTree.TabIndex = 0;
            this.buttonCreateTree.Text = "CreateTree";
            this.buttonCreateTree.UseVisualStyleBackColor = true;
            this.buttonCreateTree.Click += new System.EventHandler(this.buttonCreateTree_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.buttonGenerateJsonFile);
            this.tabPage3.Controls.Add(this.button4);
            this.tabPage3.Controls.Add(this.button3);
            this.tabPage3.Controls.Add(this.button2);
            this.tabPage3.Controls.Add(this.button1);
            this.tabPage3.Controls.Add(this.buttonRenameDistFiles);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1354, 515);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Rename";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // buttonGenerateJsonFile
            // 
            this.buttonGenerateJsonFile.Location = new System.Drawing.Point(99, 378);
            this.buttonGenerateJsonFile.Name = "buttonGenerateJsonFile";
            this.buttonGenerateJsonFile.Size = new System.Drawing.Size(169, 25);
            this.buttonGenerateJsonFile.TabIndex = 5;
            this.buttonGenerateJsonFile.Text = "Generate json file";
            this.buttonGenerateJsonFile.UseVisualStyleBackColor = true;
            this.buttonGenerateJsonFile.Click += new System.EventHandler(this.buttonGenerateJsonFile_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(99, 204);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(169, 25);
            this.button4.TabIndex = 4;
            this.button4.Text = "Renove DataTag value";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(99, 174);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(169, 25);
            this.button3.TabIndex = 3;
            this.button3.Text = "Rename details";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(99, 144);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(169, 25);
            this.button2.TabIndex = 2;
            this.button2.Text = "Rename call image files";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(99, 115);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(169, 25);
            this.button1.TabIndex = 1;
            this.button1.Text = "Rename species image files";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonRenameDistFiles
            // 
            this.buttonRenameDistFiles.Location = new System.Drawing.Point(99, 39);
            this.buttonRenameDistFiles.Name = "buttonRenameDistFiles";
            this.buttonRenameDistFiles.Size = new System.Drawing.Size(169, 25);
            this.buttonRenameDistFiles.TabIndex = 0;
            this.buttonRenameDistFiles.Text = "Rename dist files";
            this.buttonRenameDistFiles.UseVisualStyleBackColor = true;
            this.buttonRenameDistFiles.Click += new System.EventHandler(this.buttonRenameDistFiles_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.buttonSave2);
            this.tabPage4.Controls.Add(this.buttonLoad2);
            this.tabPage4.Controls.Add(this.label4);
            this.tabPage4.Controls.Add(this.textBoxNodeId2);
            this.tabPage4.Controls.Add(this.label5);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.buttonGenerateDatasetsWithRegions);
            this.tabPage4.Controls.Add(this.textBoxRegions);
            this.tabPage4.Controls.Add(this.textBoxSpecies);
            this.tabPage4.Controls.Add(this.buttonParseForRegions);
            this.tabPage4.Controls.Add(this.textBoxRegionsSource);
            this.tabPage4.Location = new System.Drawing.Point(4, 26);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1354, 515);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "AddRegions";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // buttonSave2
            // 
            this.buttonSave2.Location = new System.Drawing.Point(724, 87);
            this.buttonSave2.Name = "buttonSave2";
            this.buttonSave2.Size = new System.Drawing.Size(156, 30);
            this.buttonSave2.TabIndex = 22;
            this.buttonSave2.Text = "Save";
            this.buttonSave2.UseVisualStyleBackColor = true;
            // 
            // buttonLoad2
            // 
            this.buttonLoad2.Location = new System.Drawing.Point(724, 41);
            this.buttonLoad2.Name = "buttonLoad2";
            this.buttonLoad2.Size = new System.Drawing.Size(156, 30);
            this.buttonLoad2.TabIndex = 21;
            this.buttonLoad2.Text = "Load";
            this.buttonLoad2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 17);
            this.label4.TabIndex = 20;
            this.label4.Text = "NodeId";
            // 
            // textBoxNodeId2
            // 
            this.textBoxNodeId2.Location = new System.Drawing.Point(92, 192);
            this.textBoxNodeId2.Multiline = true;
            this.textBoxNodeId2.Name = "textBoxNodeId2";
            this.textBoxNodeId2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxNodeId2.Size = new System.Drawing.Size(508, 23);
            this.textBoxNodeId2.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(328, 217);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 17);
            this.label5.TabIndex = 18;
            this.label5.Text = "Regions";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 214);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 17);
            this.label6.TabIndex = 17;
            this.label6.Text = "Species";
            // 
            // buttonGenerateDatasetsWithRegions
            // 
            this.buttonGenerateDatasetsWithRegions.Location = new System.Drawing.Point(215, 480);
            this.buttonGenerateDatasetsWithRegions.Name = "buttonGenerateDatasetsWithRegions";
            this.buttonGenerateDatasetsWithRegions.Size = new System.Drawing.Size(156, 30);
            this.buttonGenerateDatasetsWithRegions.TabIndex = 16;
            this.buttonGenerateDatasetsWithRegions.Text = "Generate";
            this.buttonGenerateDatasetsWithRegions.UseVisualStyleBackColor = true;
            this.buttonGenerateDatasetsWithRegions.Click += new System.EventHandler(this.buttonGenerateDatasetsWithRegions_Click);
            // 
            // textBoxRegions
            // 
            this.textBoxRegions.Location = new System.Drawing.Point(327, 236);
            this.textBoxRegions.Multiline = true;
            this.textBoxRegions.Name = "textBoxRegions";
            this.textBoxRegions.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxRegions.Size = new System.Drawing.Size(811, 211);
            this.textBoxRegions.TabIndex = 15;
            this.textBoxRegions.WordWrap = false;
            // 
            // textBoxSpecies
            // 
            this.textBoxSpecies.Location = new System.Drawing.Point(24, 236);
            this.textBoxSpecies.Multiline = true;
            this.textBoxSpecies.Name = "textBoxSpecies";
            this.textBoxSpecies.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSpecies.Size = new System.Drawing.Size(273, 211);
            this.textBoxSpecies.TabIndex = 14;
            this.textBoxSpecies.WordWrap = false;
            // 
            // buttonParseForRegions
            // 
            this.buttonParseForRegions.Location = new System.Drawing.Point(226, 157);
            this.buttonParseForRegions.Name = "buttonParseForRegions";
            this.buttonParseForRegions.Size = new System.Drawing.Size(156, 30);
            this.buttonParseForRegions.TabIndex = 13;
            this.buttonParseForRegions.Text = "Parse";
            this.buttonParseForRegions.UseVisualStyleBackColor = true;
            this.buttonParseForRegions.Click += new System.EventHandler(this.buttonParseForRegions_Click);
            // 
            // textBoxRegionsSource
            // 
            this.textBoxRegionsSource.Location = new System.Drawing.Point(24, 19);
            this.textBoxRegionsSource.Multiline = true;
            this.textBoxRegionsSource.Name = "textBoxRegionsSource";
            this.textBoxRegionsSource.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxRegionsSource.Size = new System.Drawing.Size(575, 134);
            this.textBoxRegionsSource.TabIndex = 12;
            this.textBoxRegionsSource.WordWrap = false;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.buttonLoadAudio);
            this.tabPage5.Location = new System.Drawing.Point(4, 26);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1354, 515);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Audio";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // buttonLoadAudio
            // 
            this.buttonLoadAudio.Location = new System.Drawing.Point(99, 53);
            this.buttonLoadAudio.Name = "buttonLoadAudio";
            this.buttonLoadAudio.Size = new System.Drawing.Size(83, 25);
            this.buttonLoadAudio.TabIndex = 0;
            this.buttonLoadAudio.Text = "LoadAudio";
            this.buttonLoadAudio.UseVisualStyleBackColor = true;
            this.buttonLoadAudio.Click += new System.EventHandler(this.buttonLoadAudio_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1362, 545);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button buttonSave2;
        private System.Windows.Forms.Button buttonLoad2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxNodeId2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonGenerateDatasetsWithRegions;
        private System.Windows.Forms.TextBox textBoxRegions;
        private System.Windows.Forms.TextBox textBoxSpecies;
        private System.Windows.Forms.Button buttonParseForRegions;
        private System.Windows.Forms.TextBox textBoxRegionsSource;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button buttonGenerateJsonFile;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button buttonLoadAudio;
    }
}

