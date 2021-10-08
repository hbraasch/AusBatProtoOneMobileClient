using AusBatProtoOneMobileClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        Table table = new Table();
        private void ParseTable_Click(object sender, EventArgs e)
        {

            if (tableBox.Text.Length == 0) MessageBox.Show("There is no table to parse");

            var lines = tableBox.Text.Split(Environment.NewLine);
            var tableRows = new List<Row>();
            foreach (var line in lines)
            {
                if (line == "") continue;
                var columns = line.Split("\t");
                var tableColumns = new List<Column>();
                foreach (var column in columns)
                {
                    tableColumns.Add(new Column { Value = column });
                }
                tableRows.Add(new Row { Columns = tableColumns });
            }
            table.Rows = tableRows;

        }

        class Table
        {
            public List<Row> Rows = new List<Row>();

            internal string Get(int row, int col)
            {
                return Rows[row].Columns[col].Value; ;
            }

            public void Set(int row, int col, string value)
            {
                Rows[row].Columns[col].Value = value;
            }
        }

        class Row
        {
            public List<Column> Columns = new List<Column>();
        }

        public class Column
        {
            public string Value;
        }

        private void GenarateNumerics_Click(object sender, EventArgs e)
        {
            var fileFullname = @"c:\temp\NumericCharacteristics.txt";
            string fileText = "";
            if (table == null) MessageBox.Show("Parse first");
            var classNames = new List<string>();
            var classValues = new List<(float, float)>();
            for (int i = 1; i < table.Rows[0].Columns.Count; i++)
            {
                var rawText = table.Get(0, i);
                var replacedText = String.Concat(rawText.Where(c => !Char.IsWhiteSpace(c)));
                replacedText = replacedText.Replace("3-MET", "ThreeMet");
                replacedText = replacedText.Replace("5-MET", "FiveMet");
                classNames.Add(replacedText);
            }

            for (int i = 1; i < table.Rows.Count; i++)
            {
                fileText += $"Calls for {table.Get(i,0)}" + Environment.NewLine;
                fileText += "Characters = new List<CharacterBase>" + Environment.NewLine + "{" + Environment.NewLine;
                foreach (var className in classNames)
                {
                    fileText += CreateNumericCall(className, table.Get(i, classNames.IndexOf(className)+1)) + Environment.NewLine;
                }
                fileText += "}" + Environment.NewLine + Environment.NewLine;
            }

            File.WriteAllText(fileFullname, fileText);
        }

        private string CreateNumericCall(string className, string value)
        {
            var values = value.Split("-");
            var val1 = float.Parse(values[0]).ToString("N1") + "f";
            var val2 = float.Parse(values[1]).ToString("N1") + "f";
            return $"new {className}Character({val1}, {val2}),";
        }

        private void GenerateOptions(object sender, EventArgs e)
        {
            var fileFullname = @"c:\temp\OptionCharacteristics.txt";
            string fileText = "";
            if (table == null) MessageBox.Show("Parse first");
            var classNames = new List<string>();
            var classValues = new List<(float, float)>();
            for (int i = 1; i < table.Rows[0].Columns.Count; i++)
            {
                var rawText = table.Get(0, i);
                var replacedText = GenerateTitleCaseClassName(rawText);
                classNames.Add(replacedText);
            }

            for (int i = 1; i < table.Rows.Count; i++)
            {
                fileText += $"Options for {table.Get(i, 0)}" + Environment.NewLine;
                foreach (var className in classNames)
                {
                    fileText += CreateOptionCall(className, table.Get(i, classNames.IndexOf(className) + 1)) + Environment.NewLine;
                }
                fileText += Environment.NewLine;
            }

            File.WriteAllText(fileFullname, fileText);
        }

        private string GenerateTitleCaseClassName(string rawText)
        {
            var result = "";
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            var rawTextTitleCased = textInfo.ToTitleCase(rawText.ToLower());
            var words = rawTextTitleCased.Split(" ");
            foreach (var word in words)
            {
                result += word;
            }
            return result;
        }

        private string CreateOptionCall(string className, string value)
        {
            return $"new {className}Character({className}Character.{className}Enum.{value}),";
        }
    }
}
