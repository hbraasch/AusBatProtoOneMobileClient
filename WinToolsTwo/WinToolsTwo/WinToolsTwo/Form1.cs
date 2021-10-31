using AusBatProtoOneMobileClient.Data;
using AusBatProtoOneMobileClient.Models;
using Mobile.Helpers;
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TreeApp.Helpers;
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

                var tableDefinition = new KeyTable();

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
                var keyTableDefinition = new KeyTable();
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
            keyTree.LoadTreeFromKeyTables();
        }

        private void buttonGetKeyNode_Click(object sender, EventArgs e)
        {
            var keyTree = new KeyTree();
            keyTree.LoadTreeFromKeyTables();
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

        List<RegionData> speciesDatas;
        private void buttonParseForRegions_Click(object sender, EventArgs e)
        {
            try
            {
                var table = Parser.LoadTable(textBoxRegionsSource);

                #region *// Get Species data
                speciesDatas = new List<RegionData>();
                for (int i = 1; i < table.RowAmount(); i++)
                {
                    var value = table.Get(i, 0);
                    var ids = value.Split(' ');
                    var genusId = ids[0];
                    var speciesId = "";
                    for (int j = 1; j < ids.Length; j++)
                    {
                        speciesId += ids[j] + " ";
                    }
                    speciesId = speciesId.Trim();
                    var regionsValue = table.Get(i, table.ColumnAmount() - 1);
                    List<int> regions = new List<int>();
                    if (regionsValue != "")
                    {
                        regions = regionsValue.Split(',').ToList().Select(o => int.Parse(o)).ToList();
                    }
                    speciesDatas.Add(new RegionData { GenusId = genusId, SpeciesId = speciesId, Regions = regions});
                }
                #endregion



                #region *// Fill species listbox
                var listText = "";
                foreach (var speciesData in speciesDatas)
                {
                    listText += $"{speciesData.GenusId} {speciesData.SpeciesId}" + Environment.NewLine;
                }
                textBoxSpecies.Text = listText;
                #endregion

                #region *// Fill species regions
                listText = "";
                foreach (var speciesData in speciesDatas)
                {
                    var regionString = "";
                    foreach (var region in speciesData.Regions)
                    {
                        regionString += region.ToString() + ",";
                    }
                    listText += regionString + Environment.NewLine;
                }
                textBoxRegions.Text = listText;
                #endregion


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        public class RegionData
        {
            public string GenusId;
            public string SpeciesId;
            public List<int> Regions;
        }

        private void buttonGenerateDatasetsWithRegions_Click(object sender, EventArgs e)
        {
            var sourceDatasetsPathFullname = @"C:\P2\AusBatProtoOneMobileClient\AusBatProtoOneMobileClient\Data\SpeciesDataSets";


            try
            {
                if (speciesDatas.IsEmpty()) return;

                foreach (var speciesData in speciesDatas)
                {
                    var speciesDataset = LoadSpecies(speciesData.GenusId, speciesData.SpeciesId);
                    speciesDataset.SpeciesId = speciesDataset.SpeciesId.ToLower();
                    speciesDataset.RegionIds = speciesData.Regions;
                    if (speciesDataset.DataTag != null) speciesDataset.CallImages = new List<string> { $"{speciesDataset.DataTag.ToLower()}_call_image.jpg" };
                    SaveSpeciesDataSetToFile(speciesDataset);
                    SaveSpeciesDetailsToFile(speciesDataset);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Species LoadSpecies(string genusId, string speciesId)
            {
                var name = $"{genusId.ToLower()}_{speciesId}_dataset.json".FormatToAndroidFilename();
                var fullFilename = Path.Combine(sourceDatasetsPathFullname, name);
                if (!File.Exists(fullFilename))
                {
                    throw new ApplicationException($"Species file {fullFilename} does not exist");

                }
                string speciesDatasetJson = File.ReadAllText(fullFilename);
                if (string.IsNullOrEmpty(speciesDatasetJson))
                {
                    throw new ApplicationException($"No data inside dataset file [{fullFilename}]");
                }
                try
                {
                    var species = JsonConvert.DeserializeObject<Species>(speciesDatasetJson);
                    Debug.WriteLine($"Filename: {fullFilename} Genus: {species.GenusId}, Species: {species.SpeciesId} Name: {species.Name}");
                    return species;
                }
                catch (Exception ex)
                {
                    throw new BusinessException($"JSON paring error in [{fullFilename}] file. {ex.Message}");
                }
            }
        }


        private void SaveSpeciesDataSetToFile(Species speciesDataset)
        {
            var destDatasetsPathFullname = @"C:\P\AustBatsOriginal\DataSets";
            if (!Directory.Exists(destDatasetsPathFullname))
            {
                Directory.CreateDirectory(destDatasetsPathFullname);
            }
            var fullFilename = Path.Combine(destDatasetsPathFullname, $"{speciesDataset.GenusId.ToLower()}_{speciesDataset.SpeciesId.ToLower()}_dataset.json");
            var json = JsonConvert.SerializeObject(speciesDataset, Formatting.Indented);
            File.WriteAllText(fullFilename, json);
        }



        private void SaveSpeciesDetailsToFile(Species speciesDataset)
        {
            var speciesName = $"{speciesDataset.GenusId.UppercaseFirstChar()}_{speciesDataset.SpeciesId}";
            var details = "<body style='background-color: transparent;'>" +
                "<p><strong>" + speciesName  + @"</strong><br />" + speciesDataset.Name + "<br />(Gray, 1838)</p>" +
                "<p><br /><strong>Other Names</strong>... etc</p>" +
                "</body>";
            var destDatasetsPathFullname = @"C:\P\AustBatsOriginal\Details";
            if (!Directory.Exists(destDatasetsPathFullname))
            {
                Directory.CreateDirectory(destDatasetsPathFullname);
            }
            var fullFilename = Path.Combine(destDatasetsPathFullname, $"{speciesDataset.GenusId.ToLower()}_{speciesDataset.SpeciesId.ToLower()}_details.html");
            File.WriteAllText(fullFilename, details);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var speciesDatasetsPathFullname = @"C:\P2\AusBatProtoOneMobileClient\AusBatProtoOneMobileClient\Data\SpeciesDataSets";
            var speciesImagesPathFullname = @"C:\P2\AusBatProtoOneMobileClient\AusBatProtoOneMobileClient\Data\SpeciesImages";
            var destSpeciesImagesPathFullname = @"C:\P\AustBatsOriginal\SpeciesImages";
            var destSpeciesDatasetsImagesPathFullname = @"C:\P\AustBatsOriginal\Datasets";

            if (!Directory.Exists(destSpeciesImagesPathFullname))
            {
                Directory.CreateDirectory(destSpeciesImagesPathFullname);
            }

            foreach (var fullFilename in Directory.GetFiles(speciesDatasetsPathFullname, "*dataset.json"))
            {
                var posts = new List<string> { "_head.jpg", "1.jpg", "2.jpg", "3.jpg" };
                var json = File.ReadAllText(fullFilename);
                var dataset = JsonConvert.DeserializeObject<Species>(json);
                var dataTag = dataset.DataTag;
                if (dataTag == null) continue;

                dataset.Images.Clear();
                foreach (var post in posts)
                {
                    var oldImageName = $"{dataTag}{post}".ToLower();
                    var newFilename = $"{dataset.GenusId.ToLower()}_{dataset.SpeciesId.ToLower()}{post}";

                    var oldImageFullFilename = Path.Combine(speciesImagesPathFullname, oldImageName);
                    if (File.Exists(oldImageFullFilename))
                    {
                        var newImageFullFilename = Path.Combine(destSpeciesImagesPathFullname, newFilename);
                        File.Copy(oldImageFullFilename, newImageFullFilename, true);
                        dataset.Images.Add(newFilename);
                    }

                }

                json = JsonConvert.SerializeObject(dataset, Formatting.Indented);
                var fileName = Path.GetFileName(fullFilename);
                var destDatasetFullFilename = Path.Combine(destSpeciesDatasetsImagesPathFullname, fileName);
                File.WriteAllText(destDatasetFullFilename, json);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var speciesDatasetsPathFullname = @"C:\P\AustBatsOriginal\Datasets";
            var speciesCallImagesPathFullname = @"C:\P2\AusBatProtoOneMobileClient\AusBatProtoOneMobileClient\Data\SpeciesCallImages";
            var destSpeciesCallImagesPathFullname = @"C:\P\AustBatsOriginal\CallImages";
            var destSpeciesDatasetsImagesPathFullname = @"C:\P\AustBatsOriginal\Datasets";

            if (!Directory.Exists(destSpeciesCallImagesPathFullname))
            {
                Directory.CreateDirectory(destSpeciesCallImagesPathFullname);
            }

            foreach (var fullFilename in Directory.GetFiles(speciesDatasetsPathFullname, "*dataset.json"))
            {
                var json = File.ReadAllText(fullFilename);
                var dataset = JsonConvert.DeserializeObject<Species>(json);
                var dataTag = dataset.DataTag;
                dataset.CallImages.Clear();
                if (dataTag == null) continue;

                var oldImageName = $"{dataTag}_call_image.jpg".ToLower();
                var newFilename = $"{dataset.GenusId.ToLower()}_{dataset.SpeciesId.ToLower()}_call_image.jpg";

                var oldImageFullFilename = Path.Combine(speciesCallImagesPathFullname, oldImageName);
                if (File.Exists(oldImageFullFilename))
                {
                    var newImageFullFilename = Path.Combine(destSpeciesCallImagesPathFullname, newFilename);
                    File.Copy(oldImageFullFilename, newImageFullFilename, true);
                    dataset.CallImages.Add(newFilename);
                }

                json = JsonConvert.SerializeObject(dataset, Formatting.Indented);
                var fileName = Path.GetFileName(fullFilename);
                var destDatasetFullFilename = Path.Combine(destSpeciesDatasetsImagesPathFullname, fileName);
                File.WriteAllText(destDatasetFullFilename, json);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var speciesDatasetsPathFullname = @"C:\P\AustBatsOriginal\Datasets";

            var destSpeciesDatasetsImagesPathFullname = @"C:\P\AustBatsOriginal\Datasets";

            foreach (var fullFilename in Directory.GetFiles(speciesDatasetsPathFullname, "*dataset.json"))
            {
                var json = File.ReadAllText(fullFilename);
                var dataset = JsonConvert.DeserializeObject<Species>(json);
                var dataTag = dataset.DataTag;

                if (dataTag == null) continue;

                var newFilename = $"{dataset.GenusId.ToLower()}_{dataset.SpeciesId.ToLower()}_details.html";
                dataset.DetailsHtml = newFilename;

                json = JsonConvert.SerializeObject(dataset, Formatting.Indented);
                var fileName = Path.GetFileName(fullFilename);
                var destDatasetFullFilename = Path.Combine(destSpeciesDatasetsImagesPathFullname, fileName);
                File.WriteAllText(destDatasetFullFilename, json);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var speciesDatasetsPathFullname = @"C:\P\AustBatsOriginal\Datasets";
            var destSpeciesDatasetsImagesPathFullname = @"C:\P\AustBatsOriginal\Datasets";

            foreach (var fullFilename in Directory.GetFiles(speciesDatasetsPathFullname, "*dataset.json"))
            {
                var json = File.ReadAllText(fullFilename);
                var dataset = JsonConvert.DeserializeObject<AusBatProtoOneMobileClient.DataNew.Species>(json);

                var newFilename = $"{dataset.GenusId.ToLower()}_{dataset.SpeciesId.ToLower()}_details.html";
                dataset.DetailsHtml = newFilename;

                json = JsonConvert.SerializeObject(dataset, Formatting.Indented);
                var fileName = Path.GetFileName(fullFilename);
                var destDatasetFullFilename = Path.Combine(destSpeciesDatasetsImagesPathFullname, fileName);
                File.WriteAllText(destDatasetFullFilename, json);
            }
        }

        private void buttonGenerateJsonFile_Click(object sender, EventArgs e)
        {
            var version = new DbaseVersion("0.0.0");
            var json = JsonConvert.SerializeObject(version);
            File.WriteAllText(@"c:\temp\version.json", json);
        }

        private void buttonLoadAudio_Click(object sender, EventArgs e)
        {
            var bytes = default(byte[]);
            var af = new AudioFileReader(@"c:\temp\bat.mp3");
            using (var stream = new StreamReader(af,true))
            {

                using (var memstream = new MemoryStream())
                {
                    var buff = new byte[512];
                    var bytesRead = default(int);
                    while ((bytesRead = stream.BaseStream.Read(buff, 0, buff.Length)) > 0)
                        memstream.Write(buff, 0, bytesRead);
                    bytes = memstream.ToArray();
                }
            }

            float max = 0;
            var buffer = new WaveBuffer(bytes);
            // interpret as 32 bit floating point audio
            for (int index = 0; index < bytes.Length / 4; index++)
            {
                var sample = buffer.FloatBuffer[index];

                // absolute value 
                if (sample < 0) sample = -sample;
                // is this the max value?
                if (sample > max) max = sample;
            }
        }
    }
}
