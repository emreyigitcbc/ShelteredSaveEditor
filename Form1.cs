using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ShelteredSE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ProcessFile processFile = new ProcessFile();
        public string decodedData = string.Empty;
        public XmlDocument xmlDoc = new XmlDocument();
        public XmlDocument itemNames = new XmlDocument();

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Left = (this.ClientSize.Width - label1.Size.Width) / 2;
            button1.Left = (this.ClientSize.Width - button1.Size.Width) / 2;

            panel1.Show();
            tabControl1.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openSaveFile.ShowDialog();
            if (result == DialogResult.OK)
            {
                decodedData = processFile.LoadFile(openSaveFile.FileName);
                if (decodedData != string.Empty)
                {
                    // Open editor
                    panel1.Hide();
                    tabControl1.Show();
                    xmlDoc.LoadXml(decodedData);
                    itemNames.Load("ItemNames.xml");
                    ProcessData processData = new ProcessData(this);
                    processData.StartProcess();
                }
                else
                {   // Retry
                    MessageBox.Show("This file is not a valid Sheltered save file.", "Error");
                }
            }
        }

        private void saveData()
        {
            DialogResult result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    processFile.SaveFile(saveFileDialog.FileName);
                    MessageBox.Show("File saved succesfully!", "File Saved");
                }
                catch
                {

                }
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedIndices.Count <= 0)
            {
                return;
            }
            int selectedIndex = listView2.SelectedIndices[0];
            if (selectedIndex >= 0)
            {
                ProcessData processData = new ProcessData(this);
                processData.PaintSaveInfo(selectedIndex);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count <= 0)
            {
                return;
            }
            int selectedIndex = listView1.SelectedIndices[0];
            if (selectedIndex >= 0)
            {
                ProcessData processData = new ProcessData(this);
                processData.PaintInventoryManager(selectedIndex);
            }
        }
    }
}
