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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        ProcessFile processFile = new ProcessFile();
        public string decodedData = string.Empty;
        public XmlDocument xmlDoc = new XmlDocument();
        public XmlDocument itemNames = new XmlDocument();

        private void Form1_Load(object sender, EventArgs e)
        {
            label_welcome.Left = (this.ClientSize.Width - label_welcome.Size.Width) / 2;
            button_upload.Left = (this.ClientSize.Width - button_upload.Size.Width) / 2;

            panel_welcome.Show();
            editorSelector.Hide();
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
                    panel_welcome.Hide();
                    editorSelector.Show();
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

        public void saveData()
        {
            saveFileDialog.FileName = ProcessFile.fileName;
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
            if (listView_saveInfo.SelectedIndices.Count <= 0)
            {
                return;
            }
            int selectedIndex = listView_saveInfo.SelectedIndices[0];
            if (selectedIndex >= 0)
            {
                ProcessData processData = new ProcessData(this);
                processData.PaintSaveInfo(selectedIndex);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_inventory.SelectedIndices.Count <= 0)
            {
                return;
            }
            int selectedIndex = listView_inventory.SelectedIndices[0];
            if (selectedIndex >= 0)
            {
                ProcessData processData = new ProcessData(this);
                processData.PaintInventoryManager(selectedIndex);
            }
        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView_character.SelectedNode == null)
            {
                return;
            }
            string selectedNode = treeView_character.SelectedNode.ToolTipText;
            Console.WriteLine(selectedNode);
            if (selectedNode != string.Empty)
            {
                ProcessData processData = new ProcessData(this);
                processData.PaintFamilyMembers(selectedNode);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView_tree.SelectedNode == null)
            {
                return;
            }
            TreeNode selectedNode = treeView_tree.SelectedNode;
            if (selectedNode != null)
            {
                ProcessData processData = new ProcessData(this);
                processData.PaintTreeEditor(selectedNode);
            }
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            ProcessData processData = new ProcessData(this);
            processData.StartSave();
        }
    }
}
