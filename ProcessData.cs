using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
namespace ShelteredSE
{
    class ProcessData
    {
        Form1 form1;
        public ProcessData(Form1 form1)
        {
            this.form1 = form1;
        }
        public static XmlNode xmlNames;
        public static XmlNode xmlData;
        public static List<(string Name, string Path)> inventoryMap = new List<(string Name, string Path)> { };
        public static List<(string Name, string Path)> saveInfoMap = new List<(string Name, string Path)> { };

        public void StartProcess()
        {
            xmlData = form1.xmlDoc.FirstChild;
            xmlNames = form1.itemNames.FirstChild;
            if (xmlData.Name != "root") return;
            // SAVE INFO PROCESSING
            SProcess(xmlData);
            // INVENTORY PROCESSING
            IProcess(xmlData, xmlNames);
            // TREE EDITOR PROCESSING
            Process(form1.treeView1.Nodes, xmlData);
        }
        public void IProcess(XmlNode xml, XmlNode xmlNames)
        {
            var invMan = xml.SelectSingleNode("InventoryManager");
            int counter = 0;

            foreach (XmlNode node in invMan.SelectSingleNode("inventory"))
            {
                var realId = node.ChildNodes[0].InnerText;
                var realName = xmlNames.SelectSingleNode("item_" + realId).InnerText;
                var count = node.ChildNodes[1].InnerText;
                inventoryMap.Add(("InventoryManager_" + counter.ToString() + "_textbox", "InventoryManager/inventory/" + node.Name));
                string[] row = { realName };
                var listViewItem = new ListViewItem(row);
                form1.listView1.Items.Add(listViewItem);
                counter += 1;
            }
        }
        public void PaintInventoryManager(int index)
        {
            Console.WriteLine(index);
            foreach (Control ctrl in form1.panel3.Controls)
            {
                ctrl.Hide();
            }
            var selectedControls = form1.panel3.Controls.Find("InventoryManager_" + index.ToString() + "_textbox", false);
            if (selectedControls.Length > 0)
            {
                selectedControls[0].Show();
                form1.panel3.Controls.Find("InventoryManager_" + index.ToString() + "_label", false)[0].Show();
            }
            else
            {
                form1.panel3.Controls.Add(new Label() { Text = form1.listView1.Items[index].Text, Name = "InventoryManager_" + index.ToString() + "_label", Location = new Point(10, 10), AutoSize = true });
                form1.panel3.Controls.Add(new TextBox() { Text = xmlData.SelectSingleNode(inventoryMap[index].Path + "/count").InnerText, Name = "InventoryManager_" + index.ToString() + "_textbox", Location = new Point(200, 10), AutoSize = true });
            }
        }
        private void SProcess(XmlNode xml)
        {
            var saveInfo = xml.SelectSingleNode("SaveInfo");
            int counter = 0;
            foreach (XmlNode node in saveInfo)
            {
                var name = Regex.Replace(node.Name, "(\\B[A-Z])", " $1");
                name = char.ToUpper(name[0]) + name.Substring(1);
                string[] row = { name };
                var listViewItem = new ListViewItem(row);
                saveInfoMap.Add(("SaveInfo_" + counter.ToString() + "_textbox", "SaveInfo/" + node.Name));
                form1.listView2.Items.Add(listViewItem);
                counter += 1;
            }
        }
        public void PaintSaveInfo(int index)
        {
            Console.WriteLine(index);
            foreach (Control ctrl in form1.panel2.Controls)
            {
                ctrl.Hide();
            }
            var selectedControls = form1.panel2.Controls.Find("SaveInfo_" + index.ToString() + "_textbox", false);
            if (selectedControls.Length > 0)
            {
                selectedControls[0].Show();
                form1.panel2.Controls.Find("SaveInfo_" + index.ToString() + "_label", false)[0].Show();
            }
            else
            {
                form1.panel2.Controls.Add(new Label() { Text = form1.listView2.Items[index].Text, Name = "SaveInfo_" + index.ToString() + "_label", Location = new Point(10, 10), AutoSize = true });
                form1.panel2.Controls.Add(new TextBox() { Text = xmlData.SelectSingleNode(saveInfoMap[index].Path).InnerText, Name = "SaveInfo_" + index.ToString() + "_textbox", Location = new Point(200, 10), AutoSize = true });
            }
        }
        private void Process(TreeNodeCollection parent_nodes, XmlNode xml_node)
        {
            if (xml_node.ChildNodes.Count == 1 && xml_node.ChildNodes[0].Name == "#text") return;
            foreach (XmlNode child_node in xml_node.ChildNodes)
            {
                TreeNode new_node = parent_nodes.Add(child_node.Name);
                Process(new_node.Nodes, child_node);
            }
        }
    }
}