using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        MainForm form1;
        public ProcessData(MainForm form1)
        {
            this.form1 = form1;
        }
        public static int universalCounter = 0;
        public static XmlNode xmlNames;
        public static XmlNode xmlData;
        public static string[] traitList = new string[16] { "Courageous", "Cowardice", "Proactive", "Lazy", "Optimistic", "Pesimistic", "Hands-on", "Hands-off", "Resourceful", "Wasteful", "Hygienic", "Unhygienic", "Deep Sleeper", "Light Sleeper", "Small Eater", "Big Eater" };
        public static List<(string Name, string Path)> inventoryMap = new List<(string Name, string Path)> { };
        public static List<(string Name, string Path)> saveInfoMap = new List<(string Name, string Path)> { };
        public static List<(string Name, string Path)> familyMap = new List<(string Name, string Path)> { };
        public static List<(string Name, string Path)> treeMap = new List<(string Name, string Path)> { };

        public void StartProcess()
        {
            xmlData = form1.xmlDoc.FirstChild;
            xmlNames = form1.itemNames.FirstChild;
            if (xmlData.Name != "root") return;
            // SAVE INFO PROCESSING
            SProcess(xmlData.SelectSingleNode("SaveInfo"));
            // INVENTORY PROCESSING
            IProcess(xmlData.SelectSingleNode("InventoryManager/inventory"));
            // CHARACTER EDITOR PROCESSING
            CProcess(form1.treeView_character.Nodes, xmlData.SelectSingleNode("FamilyMembers"));
            // TREE EDITOR PROCESSING
            TProcess(form1.treeView_tree.Nodes, xmlData);

        }
        // INVENTORY PROCESSING
        public void IProcess(XmlNode invMan)
        {
            int counter = 0;
            foreach (XmlNode node in invMan)
            {
                var realId = node.ChildNodes[0].InnerText;
                var realName = xmlNames.SelectSingleNode("item_" + realId).InnerText;
                var count = node.ChildNodes[1].InnerText;
                inventoryMap.Add(("InventoryManager_" + counter.ToString() + "_textbox", "InventoryManager/inventory/" + node.Name + "/count"));
                form1.listView_inventory.Items.Add(new ListViewItem() { Text = realName });
                counter += 1;
            }
            form1.listView_inventory.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        // INVENTORY PAINTER
        public void PaintInventoryManager(int index)
        {
            foreach (Control ctrl in form1.panel_inventory.Controls)
            {
                ctrl.Hide();
            }
            var selectedControls = form1.panel_inventory.Controls.Find("InventoryManager_" + index.ToString() + "_textbox", false);
            if (selectedControls.Length > 0)
            {
                selectedControls[0].Show();
                form1.panel_inventory.Controls.Find("InventoryManager_" + index.ToString() + "_label", false)[0].Show();
            }
            else
            {
                form1.panel_inventory.Controls.Add(new Label() { Text = form1.listView_inventory.Items[index].Text, Name = "InventoryManager_" + index.ToString() + "_label", Location = new Point(10, 10), AutoSize = true });
                form1.panel_inventory.Controls.Add(new TextBox() { Text = xmlData.SelectSingleNode(inventoryMap[index].Path).InnerText, Name = "InventoryManager_" + index.ToString() + "_textbox", Location = new Point(200, 10), AutoSize = true });
            }
        }
        // SAVE INFO PROCESSING
        private void SProcess(XmlNode saveInfo)
        {
            int counter = 0;
            foreach (XmlNode node in saveInfo)
            {
                var name = Regex.Replace(node.Name, "(\\B[A-Z])", " $1");
                name = char.ToUpper(name[0]) + name.Substring(1);
                saveInfoMap.Add(("SaveInfo_" + counter.ToString() + "_textbox", "SaveInfo/" + node.Name));
                form1.listView_saveInfo.Items.Add(new ListViewItem() { Text = name });
                counter += 1;
            }
            // hatch password and enabled status
            form1.listView_saveInfo.Items.Add(new ListViewItem() { Text = "Mystery Hatch Enabled" });
            saveInfoMap.Add(("SaveInfo_" + counter++.ToString() + "_textbox", "FamilyManager/particleTintActive"));
            form1.listView_saveInfo.Items.Add(new ListViewItem() { Text = "Mystery Hatch Password" });
            saveInfoMap.Add(("SaveInfo_" + counter++.ToString() + "_textbox", "FamilyManager/particleTint"));
        }
        // SAVE INFO PAINTER
        public void PaintSaveInfo(int index)
        {
            foreach (Control ctrl in form1.panel_saveInfo.Controls)
            {
                ctrl.Hide();
            }
            var selectedControls = form1.panel_saveInfo.Controls.Find("SaveInfo_" + index.ToString() + "_textbox", false);
            if (selectedControls.Length > 0)
            {
                selectedControls[0].Show();
                form1.panel_saveInfo.Controls.Find("SaveInfo_" + index.ToString() + "_label", false)[0].Show();
            }
            else
            {
                var node = xmlData.SelectSingleNode(saveInfoMap[index].Path);
                if (node.Attributes.Count > 0)
                {
                    string password = node.Attributes[0].InnerText + node.Attributes[1].InnerText + node.Attributes[2].InnerText + node.Attributes[3].InnerText;
                    form1.panel_saveInfo.Controls.Add(new Label() { Text = form1.listView_saveInfo.Items[index].Text, Name = "SaveInfo_" + index.ToString() + "_label", Location = new Point(10, 10), AutoSize = true });
                    form1.panel_saveInfo.Controls.Add(new TextBox() { Text = password, Name = "SaveInfo_" + index.ToString() + "_textbox", Location = new Point(200, 10), AutoSize = true });
                }
                else
                {
                    form1.panel_saveInfo.Controls.Add(new Label() { Text = form1.listView_saveInfo.Items[index].Text, Name = "SaveInfo_" + index.ToString() + "_label", Location = new Point(10, 10), AutoSize = true });
                    form1.panel_saveInfo.Controls.Add(new TextBox() { Text = xmlData.SelectSingleNode(saveInfoMap[index].Path).InnerText, Name = "SaveInfo_" + index.ToString() + "_textbox", Location = new Point(200, 10), AutoSize = true });
                }
            }
        }
        // CHARACTER EDITOR PROCESSING
        private void CProcess(TreeNodeCollection parent_nodes, XmlNode xml_node)
        {
            if (xml_node.ChildNodes.Count == 1 && xml_node.ChildNodes[0].Name == "#text") return;
            // First, fetch member name's and append it to list
            int counter = 0;
            foreach (XmlNode member in xml_node.ChildNodes)
            {
                TreeNode main_node = parent_nodes.Add(member.SelectSingleNode("firstName").InnerText + " " + member.SelectSingleNode("lastName").InnerText);
                TreeNode general_node = main_node.Nodes.Add("General");
                general_node.Nodes.Add("First Name").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/firstName"));
                general_node.Nodes.Add("Second Name").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/lastName"));
                general_node.Nodes.Add("Health").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/health"));
                general_node.Nodes.Add("Max Health").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/maxHealth"));
                general_node.Nodes.Add("Walk Speed").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/walkSpeed"));
                general_node.Nodes.Add("Climb Speed").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/ladderSpeed"));
                general_node.Nodes.Add("Hunger").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/BehaviourStats/hunger/value"));
                general_node.Nodes.Add("Thirst").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/BehaviourStats/thirst/value"));
                general_node.Nodes.Add("Fatigue").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/BehaviourStats/fatigue/value"));
                general_node.Nodes.Add("Dirtiness").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/BehaviourStats/dirtiness/value"));
                general_node.Nodes.Add("Toilet").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/BehaviourStats/toilet/value"));
                general_node.Nodes.Add("Stress").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/BehaviourStats/stress/value"));
                general_node.Nodes.Add("Trauma").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/BehaviourStats/trauma/value"));
                general_node.Nodes.Add("Loyalty").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/BehaviourStats/loyalty/value"));
                TreeNode mesh_node = main_node.Nodes.Add("Appearance");
                mesh_node.Nodes.Add("Head Texture").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/headTexture"));
                mesh_node.Nodes.Add("Torso Texture").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/torsoTexture"));
                mesh_node.Nodes.Add("Leg Texture").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/legTexture"));
                mesh_node.Nodes.Add("Hair Color").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/hairColor"));
                mesh_node.Nodes.Add("Skin Color").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/skinColor"));
                mesh_node.Nodes.Add("Shirt Color").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/shirtColor"));
                mesh_node.Nodes.Add("Pants Color").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/pantsColor"));
                TreeNode traits_node = main_node.Nodes.Add("Traits");
                traits_node.Nodes.Add("Member Trait").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/Traits"));
                TreeNode stats_node = main_node.Nodes.Add("Stats");
                stats_node.Nodes.Add("Strength").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/BaseStats/Strength/level"));
                stats_node.Nodes.Add("Dexterity").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/BaseStats/Dexterity/level"));
                stats_node.Nodes.Add("Intelligence").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/BaseStats/Intelligence/level"));
                stats_node.Nodes.Add("Charisma").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/BaseStats/Charisma/level"));
                stats_node.Nodes.Add("Perception").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/BaseStats/Perception/level"));
                TreeNode illnes_node = main_node.Nodes.Add("Illnesses");
                illnes_node.Nodes.Add("Radiation Poisoning").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/Illnesses/RadiationPoisoning/active"));
                illnes_node.Nodes.Add("Malnourishment").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/Illnesses/Malnourishment/active"));
                illnes_node.Nodes.Add("Infection").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/Illnesses/Infection/active"));
                illnes_node.Nodes.Add("Food Poisoning").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/Illnesses/FoodPoisoning/active"));
                illnes_node.Nodes.Add("Bleeding").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/Illnesses/Bleeding/active"));
                illnes_node.Nodes.Add("Suffocating").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/Illnesses/Suffocating/active"));
                illnes_node.Nodes.Add("Weak Heart").ToolTipText = "FamilyMembers_" + counter.ToString();
                familyMap.Add(("FamilyMembers_" + counter++.ToString(), "FamilyMembers/" + member.Name + "/Illnesses/WeakHeart/active"));
            }
        }
        public int[] CalculateIndexes(int index)
        {
            int[] indexes = new int[3];
            int grand = 0;
            int parent = 0;
            while (index >= 0)
            {
                index -= 14;
                if (index < 0) { index += 14; break; };
                parent++;
                index -= 7;
                if (index < 0) { index += 7; break; };
                parent++;
                index -= 1;
                if (index < 0) { index += 1; break; };
                parent++;
                index -= 5;
                if (index < 0) { index += 5; break; };
                parent++;
                index -= 7;
                if (index < 0) { index += 7; break; };
                parent = 0;
                grand += 1;
            }
            indexes[0] = grand;
            indexes[1] = parent;
            indexes[2] = index;
            return indexes;
        }
        public void PaintFamilyMembers(string chosen)
        {
            int index = int.Parse(chosen.Split('_')[1]);
            foreach (Control ctrl in form1.panel_character.Controls)
            {
                ctrl.Hide();
            }
            var selectedControls = form1.panel_character.Controls.Find(chosen + "_textbox", false);
            if (selectedControls.Length > 0)
            {
                Array.ForEach(form1.panel_character.Controls.Find(chosen + "_label", false), ctx => ctx.Show());
                Array.ForEach(form1.panel_character.Controls.Find(chosen + "_textbox", false), ctx => ctx.Show());
            }
            else
            {
                int[] indexes = CalculateIndexes(index);
                string text = form1.treeView_character.Nodes[indexes[0]].Nodes[indexes[1]].Nodes[indexes[2]].Text;
                if (text.Contains("Color"))
                {
                    for (int i = 0; i < xmlData.SelectSingleNode(familyMap[index].Path).Attributes.Count; i++)
                    {
                        var value = xmlData.SelectSingleNode(familyMap[index].Path).Attributes[i];
                        form1.panel_character.Controls.Add(new Label() { Text = value.Name, Name = chosen + "_label", Location = new Point(10, 10 + i * 20), AutoSize = true });
                        form1.panel_character.Controls.Add(new TextBox() { Text = ((int)Math.Floor(double.Parse(value.InnerText) * 255)).ToString(), Name = chosen + "_textbox", Location = new Point(200, 10 + i * 20), AutoSize = true });
                    }
                }
                else if (text == "Member Trait")
                {
                    form1.panel_character.Controls.Add(new Label() { Text = "Disabled for now", Name = chosen + "_label", Location = new Point(10, 10), AutoSize = true });
                    //var comboBox = new ComboBox() { Text = "Select a trait", Name = chosen + "_textbox", Location = new Point(200, 10), AutoSize = true };
                    //comboBox.DataSource = new BindingSource().DataSource = traitList;
                    //form1.panel4.Controls.Add(comboBox);
                }
                else
                {
                    form1.panel_character.Controls.Add(new Label() { Text = text, Name = chosen + "_label", Location = new Point(10, 10), AutoSize = true });
                    form1.panel_character.Controls.Add(new TextBox() { Text = xmlData.SelectSingleNode(familyMap[index].Path).InnerText, Name = chosen + "_textbox", Location = new Point(200, 10), AutoSize = true });
                }
            }
        }
        public void PaintTreeEditor(TreeNode node)
        {
            foreach (Control ctrl in form1.panel_tree.Controls)
            {
                ctrl.Hide();
            }
            var selectedControls = form1.panel_tree.Controls.Find(node.ToolTipText + "_textbox", false);
            if (selectedControls.Length > 0)
            {
                Array.ForEach(form1.panel_tree.Controls.Find(node.ToolTipText + "_label", false), ctx => ctx.Show());
                Array.ForEach(form1.panel_tree.Controls.Find(node.ToolTipText + "_textbox", false), ctx => ctx.Show());
            }
            else
            {
                if (node.Nodes.Count <= 1)
                {
                    if (xmlData.SelectSingleNode(node.FullPath).Attributes.Count > 0)
                    {
                        int internalCounter = 0;
                        foreach (XmlNode xmlNode in xmlData.SelectSingleNode(node.FullPath).Attributes)
                        {
                            form1.panel_tree.Controls.Add(new Label() { Text = xmlNode.Name, Name = node.ToolTipText + "_label", Location = new Point(10, 10 + 30 * internalCounter), AutoSize = true });
                            form1.panel_tree.Controls.Add(new TextBox() { Text = xmlNode.InnerText, Name = node.ToolTipText + "_textbox", Location = new Point(200, 10 + 30 * internalCounter), AutoSize = true });
                            internalCounter += 1;
                        }
                    }
                    else
                    {
                        form1.panel_tree.Controls.Add(new Label() { Text = node.Text, Name = node.ToolTipText + "_label", Location = new Point(10, 10), AutoSize = true });
                        form1.panel_tree.Controls.Add(new TextBox() { Text = xmlData.SelectSingleNode(node.FullPath).InnerText, Name = node.ToolTipText + "_textbox", Location = new Point(200, 10), AutoSize = true });
                    }
                }
            }
        }
        // TREE EDITOR PROCESSING
        private void TProcess(TreeNodeCollection parent_nodes, XmlNode xml_node)
        {
            if (xml_node.ChildNodes.Count == 1 && xml_node.ChildNodes[0].Name == "#text") return;
            foreach (XmlNode child_node in xml_node.ChildNodes)
            {
                TreeNode new_node = parent_nodes.Add(child_node.Name);
                new_node.ToolTipText = "TreeEditor_" + universalCounter.ToString();
                string extend = string.Empty;
                if (child_node.Attributes.Count > 0)
                {
                    for (var i = 0; i < child_node.Attributes.Count; i++)
                    {
                        treeMap.Add((new_node.ToolTipText + "@" + i.ToString(), new_node.FullPath));
                    }
                    universalCounter += child_node.Attributes.Count;
                }
                else
                {
                    treeMap.Add((new_node.ToolTipText, new_node.FullPath));
                    universalCounter += 1;
                }
                TProcess(new_node.Nodes, child_node);
            }
        }

        public void StartSave()
        {
            // Saving Tree
            List<string> processedTreeControls = new List<string>();
            Debug.WriteLine("Saving tree editor...");
            foreach (Control ctrl in form1.panel_tree.Controls)
            {
                if (ctrl.Name.Contains("_textbox") && processedTreeControls.Contains(ctrl.Name) == false)
                {
                    var foundControls = form1.panel_tree.Controls.Find(ctrl.Name, true);
                    int index = int.Parse(ctrl.Name.Split('_')[1]);
                    if (foundControls.Length > 1)
                    {
                        for (var i = 0; i < foundControls.Length; i++)
                        {
                            Debug.WriteLine("ATTR: " + treeMap[index + i].Name + " & " + foundControls[i].Name);
                            xmlData.SelectSingleNode(treeMap[index + i].Path).Attributes[i].InnerText = foundControls[i].Text;
                        }
                    }
                    else
                    {
                        Debug.WriteLine("NO ATTR: " + treeMap[index].Name + " & " + ctrl.Name);
                        xmlData.SelectSingleNode(treeMap[index].Path).InnerText = ctrl.Text;
                    }
                    processedTreeControls.Add(ctrl.Name);
                }
            }
            // Saving Save Info
            Debug.WriteLine("Saving save info editor...");
            foreach (Control ctrl in form1.panel_saveInfo.Controls)
            {
                if (ctrl.Name.Contains("_textbox"))
                {
                    int index = int.Parse(ctrl.Name.Split('_')[1]);
                    if (ctrl.Name == "SaveInfo_" + (form1.listView_saveInfo.Items.Count - 1).ToString() + "_textbox")
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            Debug.WriteLine("MYSTRY: " + saveInfoMap[index].Name + "@" + i.ToString() + " & " + ctrl.Name);
                            xmlData.SelectSingleNode(saveInfoMap[index].Path).Attributes[i].InnerText = ctrl.Text[i].ToString();
                        }
                    }
                    else
                    {
                        Debug.WriteLine("DEF: " + saveInfoMap[index].Name + " & " + ctrl.Name);
                        xmlData.SelectSingleNode(saveInfoMap[index].Path).InnerText = ctrl.Text;
                    }
                }
            }
            // Saving Inventory Editor
            Debug.WriteLine("Saving inventory editor...");
            foreach (Control ctrl in form1.panel_inventory.Controls)
            {
                if (ctrl.Name.Contains("_textbox"))
                {
                    int index = int.Parse(ctrl.Name.Split('_')[1]);
                    Debug.WriteLine("DEF: " + inventoryMap[index].Name + " & " + ctrl.Name);
                    xmlData.SelectSingleNode(inventoryMap[index].Path).InnerText = ctrl.Text;
                }
            }
            // Saving Character Editor
            List<string> processedCharacterControls = new List<string>();
            Debug.WriteLine("Saving character editor...");
            foreach (Control ctrl in form1.panel_character.Controls)
            {
                if (ctrl.Name.Contains("_textbox") && processedCharacterControls.Contains(ctrl.Name) == false)
                {
                    int index = int.Parse(ctrl.Name.Split('_')[1]);
                    var foundControls = form1.panel_tree.Controls.Find(ctrl.Name, true);
                    if (foundControls.Length > 1)
                    {
                        for (var i = 0; i < foundControls.Length; i++)
                        {
                            Debug.WriteLine("ATTR: " + familyMap[index].Name + " & " + ctrl.Name);
                            xmlData.SelectSingleNode(familyMap[index].Path).Attributes[i].InnerText = (double.Parse(ctrl.Text) / 255).ToString();
                        }
                    }
                    else
                    {
                        Debug.WriteLine("NO ATTR: " + familyMap[index].Name + " & " + ctrl.Name);
                        xmlData.SelectSingleNode(familyMap[index].Path).InnerText = ctrl.Text;
                    }
                    processedCharacterControls.Add(ctrl.Name);
                }
            }
            Debug.WriteLine("SAVE FINISHED");
            form1.xmlDoc.Save(ProcessFile.tempFilePath);
            form1.saveData();
        }
    }
}