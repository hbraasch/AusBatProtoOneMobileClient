using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    }
}
