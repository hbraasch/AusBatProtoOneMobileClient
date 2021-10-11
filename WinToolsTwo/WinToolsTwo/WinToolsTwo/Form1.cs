using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WinToolsTwo.Parser;


namespace WinToolsTwo
{
    public partial class Form1 : Form
    {

        [DllImport("msvcrt", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int rename(
                [MarshalAs(UnmanagedType.LPStr)]
            string oldpath,
                [MarshalAs(UnmanagedType.LPStr)]
            string newpath);


        string pathFullname = @"C:\P\AustBatsOriginal\KeyTables";
        public Form1()
        {
            InitializeComponent();

            textBoxNodeId.Text = Properties.Settings.Default.StartupNodeId;

        }


        private void buttonParse_Click(object sender, EventArgs e)
        {
            try
            {
                var table = Parser.LoadTable(textBoxSource);

                #region *// Get indexes for numeric
                List<int> numericColumnIndexes = new List<int>();
                for (int i = 0; i < table.ColumnAmount(); i++)
                {
                    if (IsNumericCell(table.Get(1, i)))
                    {
                        numericColumnIndexes.Add(i);
                    }
                }
                #endregion

                #region *// Build numeric names
                var numericIds = new List<(int columnIndex, string id)>();
                foreach (var numericColumnIndex in numericColumnIndexes)
                {
                    numericIds.Add((numericColumnIndex, GenerateCharacterId(table.Get(0, numericColumnIndex))));
                }
                #endregion


                #region *// Fill numeric listbox
                var listText = "";
                foreach (var numericId in numericIds)
                {
                    listText += $"{numericId.columnIndex}: {numericId.id}" + Environment.NewLine;
                }
                textBoxNumerics.Text = listText;
                #endregion

                #region *// Build pickers names
                var pickerColumnIndexes = new List<int>();

                for (int i = 1; i < table.ColumnAmount() - 1; i++)
                {
                    if (!numericColumnIndexes.Contains(i))
                    {
                        pickerColumnIndexes.Add(i);
                    }
                }
                #endregion



                #region *// Fill picker listbox
                listText = "";
                foreach (var pickerColumnIndex in pickerColumnIndexes)
                {
                    var pickerId = GenerateCharacterId(table.Get(0, pickerColumnIndex));
                    listText += $"{pickerColumnIndex}: {pickerId}" + Environment.NewLine;
                    foreach (var pickerOption in GetPickerOptions(table, pickerColumnIndex))
                    {
                        listText += "\t" + pickerOption + Environment.NewLine;
                    }
                }
                textBoxPickers.Text = listText;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private List<string> GetPickerOptions(Table table, int pickerIndex)
        {
            List<string> result = new List<string>();
            var rawOptions = GetPickerRawOptions(table, pickerIndex);
            int optionIndex = 0;
            foreach (var rawOption in rawOptions)
            {
                result.Add($"Option{optionIndex++}: {UppercaseFirstChar(rawOption)}");
            }
            return result;
        }

        private List<string> GetPickerRawOptions(Table table, int pickerIndex)
        {
            List<string> result = new List<string>();
            for (int i = 1; i < table.RowAmount(); i++)
            {
                var optionRaw = table.Get(i, pickerIndex).Trim();
                if (!result.Exists(o=>o.ToLower().Trim() == optionRaw.ToLower()) )
                {
                    result.Add(UppercaseFirstChar(optionRaw));
                }
            }
            return result;
        }

        private string UppercaseFirstChar(string rawText)
        {
            var firstChar = rawText[0].ToString().ToUpper();
            return $"{firstChar}{rawText.Substring(1)}";
        }

        private string GenerateCharacterId(string value)
        {
            var id = "";
            value = value.Replace("3-Met", "ThreeMet");
            value = value.Replace("5-Met", "FiveMet");
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            var words = value.Split(" ");
            foreach (var word in words)
            {
                var rawTextTitleCased = textInfo.ToTitleCase(word.ToLower());
                id += rawTextTitleCased;
            }

            return id;
        }

        private bool IsNumericCell(string v)
        {
            var numbers = v.Split("-");
            float result;
            if (numbers.Length == 2)
            {
                if (float.TryParse(numbers[0] , out result))
                {
                    return true;
                }
            }
            return false;
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxNodeId.Text == "")
                {
                    throw new ApplicationException("NodeId is empty");
                }
                var fileFullname = Path.Combine(pathFullname, $"{textBoxNodeId.Text}_RawTable.json");
                var rawTable = JsonConvert.DeserializeObject<RawTable>(File.ReadAllText(fileFullname));
                textBoxSource.Text = rawTable.Source;
                textBoxNumerics.Text = rawTable.Numerics;
                textBoxPickers.Text = rawTable.Pickers;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxNodeId.Text == "")
                {
                    throw new ApplicationException("NodeId is empty");
                }
                var fileFullname = Path.Combine(pathFullname, $"{textBoxNodeId.Text}_RawTable.json");
                var rawTable = new RawTable { Source = textBoxSource.Text, Numerics = textBoxNumerics.Text, Pickers = textBoxPickers.Text };
                var rawTableJson = JsonConvert.SerializeObject(rawTable);
                File.WriteAllText(fileFullname, rawTableJson);

                Properties.Settings.Default.StartupNodeId = textBoxNodeId.Text;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public class RawTable
        {
            public string Source { get; set; }

            public string Numerics { get; set; }

            public string Pickers { get; set; }
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                var androidFilename = FormatToAndroidFilename(textBoxNodeId.Text);
                var fileFullname = Path.Combine(pathFullname, $"{androidFilename}_keytable.json");

                var sourceTable = Parser.LoadTable(textBoxSource);

                var tableDefinition = new KeyTableDefinition();

                #region *// Extract data from textboxes
                var numericLines = textBoxNumerics.Text.Split("\r\n");
                var numericDatas = new List<NumericData>();
                foreach (string line in numericLines)
                {
                    if (line == "") continue;
                    var lineData = line.Split(": ");
                    numericDatas.Add(new NumericData { columnIndex = int.Parse(lineData[0]), id = lineData[1] });
                }

                var pickersLines = textBoxPickers.Text.Split("\r\n");
                var pickerDatas = new List<PickerData>();
                PickerData pickerData = null;
                for (int rowIndex = 0; rowIndex < pickersLines.Length; rowIndex++)
                {
                    if (pickersLines[rowIndex] == "") continue;
                    if (IsIdLine(pickersLines, rowIndex))
                    {
                        // Id line
                        if (pickerData != null)
                        {
                            pickerDatas.Add(pickerData);
                            pickerData = null;
                        }
                        var lineData = pickersLines[rowIndex].Split(": ");
                        pickerData = new PickerData { columnIndex = int.Parse(lineData[0]), id = lineData[1] };
                        var pickerOptions = new List<(string id, string prompt)>();
                        for (int rowIndex2 = rowIndex + 1; rowIndex2 < pickersLines.Length; rowIndex2++)
                        {
                            if (pickersLines[rowIndex2] == "") continue;
                            if (IsIdLine(pickersLines, rowIndex2))
                            {
                                break;
                            }
                            else
                            {
                                var optionLine = pickersLines[rowIndex2];
                                var optionsValues = optionLine.Split(": ");
                                pickerOptions.Add((optionsValues[0].Substring(1), optionsValues[1]));
                            }
                        }
                        pickerData.options = pickerOptions;
                    }
                }
                if (pickerData != null) pickerDatas.Add(pickerData);
                #endregion


                #region *// Fill keytable with data
                var keyTableDefinition = new KeyTableDefinition();
                keyTableDefinition.KeyIds = new string[numericDatas.Count + pickerDatas.Count].ToList();
                foreach (var numericDataItem in numericDatas)
                {
                    keyTableDefinition.KeyIds[numericDataItem.columnIndex - 1] = numericDataItem.id;
                }
                foreach (var pickerDataItem in pickerDatas)
                {
                    keyTableDefinition.KeyIds[pickerDataItem.columnIndex - 1] = pickerDataItem.id;
                }

                foreach (var pickerDataItem in pickerDatas)
                {
                    var picker = new Picker { Id = pickerDataItem.id };
                    picker.OptionIds = pickerDataItem.options.Select(o => o.id).ToList();
                    picker.OptionPrompts = pickerDataItem.options.Select(o => o.prompt).ToList();
                    keyTableDefinition.Pickers.Add(picker);
                }

                for (int rowIndex = 1; rowIndex < sourceTable.RowAmount(); rowIndex++)
                {
                    var rowData = new string[numericDatas.Count + pickerDatas.Count].ToList();
                    foreach (var keyId in keyTableDefinition.KeyIds)
                    {
                        var colIndex = keyTableDefinition.KeyIds.IndexOf(keyId);
                        var value = sourceTable.Get(rowIndex, colIndex + 1);
                        if (keyTableDefinition.Pickers.Exists(o => o.Id == keyId))
                        {
                            // Item is a picker, so substitute value with id's
                            var pickerColumnIndex = pickerDatas.FirstOrDefault(o => o.id == keyId).columnIndex;
                            value = SubstitutePickerValue(sourceTable, pickerColumnIndex, keyTableDefinition.Pickers.FirstOrDefault(o => o.Id == keyId), value);
                        }
                        rowData[colIndex] = value;
                    }

                    keyTableDefinition.NodeRows.Add(new NodeRow { NodeId = sourceTable.Get(rowIndex, 0), Values = rowData });
                }
                #endregion

                #region *// Save json file
                keyTableDefinition.NodeId = textBoxNodeId.Text;
                var tableJson = JsonConvert.SerializeObject(keyTableDefinition, Formatting.Indented);
                File.WriteAllText(fileFullname, tableJson);
                MessageBox.Show("Table succesfully created");

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string SubstitutePickerValue(Table sourceTable, int pickerColumnIndex, Picker picker, string prompt)
        {


            var sourceOptions = GetPickerRawOptions(sourceTable, pickerColumnIndex);
            var index = sourceOptions.Select(o=>o.ToLower()).ToList().IndexOf(prompt.ToLower().Trim());
            if (index == -1)
            {
                throw new ApplicationException($"Cannot find prompt [{prompt}] inside picker [{picker.Id}]");
            }
            return picker.OptionIds[index];
        }


        private static bool IsIdLine(string[] pickersLines, int rowIndex)
        {
            return pickersLines[rowIndex][0].ToString() != "\t";
        }

        public class NumericData
        {
            public int columnIndex;
            public string id;
        }

        public class PickerData
        {
            public int columnIndex;
            public string id;
            public List<(string id, string prompt)> options = new List<(string, string)>();
        }

        public string FormatToAndroidFilename(string text)
        {
            var result = text.Replace("-", "_");
            result = result.Replace(" ", "_");
            return result.ToLower();

        }

        private void buttonCreateTree_Click(object sender, EventArgs e)
        {
            var keyTree = new KeyTree();
            keyTree.LoadTree();
        }

        private void buttonGetKeyNode_Click(object sender, EventArgs e)
        {
            var keyTree = new KeyTree();
            keyTree.LoadTree();
            var node = keyTree.GetKeyNode("");
        }

        private void buttonRenameDistFiles_Click(object sender, EventArgs e)
        {
            var pathName = @"C:\P2\AusBatProtoOneMobileClient\AusBatProtoOneMobileClient\Data\SpeciesDistributionMaps";
            foreach (var fullFilename in Directory.GetFiles(pathName,"*.jpg"))
            {
                var name = Path.GetFileNameWithoutExtension(fullFilename);
                var newFileName = $"{name}_dist.jpg";
                rename(fullFilename, Path.Combine(pathName, newFileName));
            }
        }


    }
}
