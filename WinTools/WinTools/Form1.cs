﻿using AusBatProtoOneMobileClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WinTools.Keys;

namespace WinTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var file in Directory.EnumerateFiles(@"C:\P2\AusBatProtoOneMobileClient\AusBatProtoOneMobileClient\Data\DistributionMaps"))
            {
                var path = Path.GetDirectoryName(file);
                var newFilename = "X" + Path.GetFileName(file).ToLower();
                var newFile = Path.Combine(path, newFilename);
                if (File.Exists(newFile))
                {
                    File.Copy(file, newFile, true);
                    File.Delete(file);
                }
                else
                {
                    File.Copy(file, newFile, true);
                    File.Delete(file);
                }
                var newFilename2 = Path.GetFileName(file).ToLower();
                var newFile2 = Path.Combine(path, newFilename2);
                if (File.Exists(newFile2))
                {
                    File.Copy(newFile, newFile2, true);
                    File.Delete(newFile);
                }
                else
                {
                    File.Copy(newFile, newFile2, true);
                }
            }
        
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var dbase = Dbase.Load();
            await dbase.Init();
            var destPath = @"c:\temp\spesies";
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
            foreach (var bat in dbase.Bats)
            {
                var filename = $"{bat.GenusId.ToLower()}_{bat.SpeciesId.ToLower()}_dataset.json";
                var fileFullname = Path.Combine(destPath, filename);
                var speciesJson = JsonConvert.SerializeObject(bat,  Formatting.Indented);
                File.WriteAllText(fileFullname, speciesJson);
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var dbase = Dbase.Load();
            await dbase.Init();
            var sourcePath = @"c:\temp\calls\orig";
            var sourceFiles = Directory.EnumerateFiles(sourcePath);
            var destPath = @"c:\temp\calls\dest";
            int index = 0;
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
            foreach (var bat in dbase.Bats)
            {
                var destFullFilename = Path.Combine(destPath, $"{bat.DataTag.ToLower()}_call_image.jpg");
                File.Copy(sourceFiles.ToList()[index++], destFullFilename, true);
            }
            Debug.WriteLine("Done");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var tree = new DecisionTree();
            tree.Init();
        }
    }
}
