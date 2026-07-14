using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

        public static Dictionary<int, string> itemNamesByIndex = new Dictionary<int, string>();
        public static Dictionary<string, string> itemNamesByTypeId = new Dictionary<string, string>();

        public class CsvItemInfo
        {
            public string TypeId { get; set; }
            public int Index { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public static Dictionary<int, CsvItemInfo> csvItemsByIndex = new Dictionary<int, CsvItemInfo>();
        public static Dictionary<string, CsvItemInfo> csvItemsByTypeId = new Dictionary<string, CsvItemInfo>();
        public static List<ListViewItem> allInventoryListViewItems = new List<ListViewItem>();

        public static void LoadItemNames()
        {
            csvItemsByIndex.Clear();
            csvItemsByTypeId.Clear();

            string csvPath = "Sheltered - Item #.csv";
            if (File.Exists(csvPath))
            {
                try
                {
                    var lines = File.ReadAllLines(csvPath);
                    foreach (var line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        var parts = line.Split(',');
                        if (parts.Length >= 3)
                        {
                            var typeStr = parts[0].Trim();
                            var indexStr = parts[1].Trim();
                            var nameStr = parts[2].Trim();

                            if (typeStr == "type" || indexStr == "i#") continue; // Skip header

                            List<string> notes = new List<string>();
                            for (int j = 3; j < parts.Length; j++)
                            {
                                var note = parts[j].Trim();
                                if (!string.IsNullOrEmpty(note)) notes.Add(note);
                            }
                            var descStr = string.Join(" | ", notes);

                            var info = new CsvItemInfo
                            {
                                TypeId = typeStr,
                                Name = nameStr,
                                Description = descStr
                            };

                            if (int.TryParse(indexStr, out int idx))
                            {
                                info.Index = idx;
                                csvItemsByIndex[idx] = info;
                            }
                            if (!string.IsNullOrEmpty(typeStr))
                            {
                                csvItemsByTypeId[typeStr] = info;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading CSV: " + ex.Message);
                }
            }
        }

        private Control CreateInputControl(string initialValue, string name, Point location)
        {
            if (initialValue.Equals("True", System.StringComparison.OrdinalIgnoreCase) || 
                initialValue.Equals("False", System.StringComparison.OrdinalIgnoreCase))
            {
                var comboBox = new ComboBox()
                {
                    Name = name,
                    Location = location,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Width = 100
                };
                comboBox.Items.Add("True");
                comboBox.Items.Add("False");
                comboBox.Text = initialValue.Equals("True", System.StringComparison.OrdinalIgnoreCase) ? "True" : "False";
                return comboBox;
            }
            else
            {
                var textBox = new TextBox()
                {
                    Text = initialValue,
                    Name = name,
                    Location = location,
                    AutoSize = true
                };

                double dummyDouble;
                int dummyInt;
                if (double.TryParse(initialValue, out dummyDouble) || int.TryParse(initialValue, out dummyInt))
                {
                    textBox.KeyPress += (sender, e) =>
                    {
                        char decimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
                        char minusSign = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NegativeSign[0];

                        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                            (e.KeyChar != decimalSeparator) && (e.KeyChar != minusSign))
                        {
                            e.Handled = true;
                        }

                        if ((e.KeyChar == decimalSeparator) && ((sender as TextBox).Text.IndexOf(decimalSeparator) > -1))
                        {
                            e.Handled = true;
                        }

                        if ((e.KeyChar == minusSign) && ((sender as TextBox).Text.IndexOf(minusSign) > -1 || (sender as TextBox).SelectionStart > 0))
                        {
                            e.Handled = true;
                        }
                    };
                }

                return textBox;
            }
        }

        public void StartProcess()
        {
            xmlData = form1.xmlDoc.FirstChild;
            xmlNames = form1.itemNames.FirstChild;
            if (xmlData.Name != "root") return;

            // Clear static map buffers to prevent duplicates when reloading saves
            inventoryMap.Clear();
            saveInfoMap.Clear();
            familyMap.Clear();
            treeMap.Clear();

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
            allInventoryListViewItems.Clear();
            form1.listView_inventory.Items.Clear();

            // Set Column 0 of tableLayout_inventory to 450 pixels absolute so that columns and search bar are fully visible!
            if (form1.tableLayout_inventory.ColumnStyles.Count > 0)
            {
                form1.tableLayout_inventory.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 450F);
            }

            // Set up multi-column view
            form1.listView_inventory.Columns.Clear();
            form1.listView_inventory.Columns.Add("Name", 170);
            form1.listView_inventory.Columns.Add("ID", 50);
            form1.listView_inventory.Columns.Add("Description", 200);
            form1.listView_inventory.HeaderStyle = ColumnHeaderStyle.Clickable;

            foreach (XmlNode node in invMan)
            {
                var realId = node.SelectSingleNode("type")?.InnerText ?? (node.ChildNodes.Count > 0 ? node.ChildNodes[0].InnerText : "");
                var count = node.SelectSingleNode("count")?.InnerText ?? (node.ChildNodes.Count > 1 ? node.ChildNodes[1].InnerText : "");

                string realName = null;
                string description = "";
                if (node.Name.StartsWith("i") && int.TryParse(node.Name.Substring(1), out int slotIdx))
                {
                    if (csvItemsByIndex.TryGetValue(slotIdx, out var info))
                    {
                        realName = info.Name;
                        description = info.Description;
                    }
                }
                if (string.IsNullOrEmpty(realName))
                {
                    if (csvItemsByTypeId.TryGetValue(realId, out var info))
                    {
                        realName = info.Name;
                        description = info.Description;
                    }
                }
                if (string.IsNullOrEmpty(realName))
                {
                    realName = "Undefined Item (" + realId + ")";
                }

                inventoryMap.Add(("InventoryManager_" + counter.ToString() + "_textbox", "InventoryManager/inventory/" + node.Name + "/count"));
                
                var lvi = new ListViewItem(realName) { Tag = counter };
                lvi.SubItems.Add(realId);
                lvi.SubItems.Add(description);

                allInventoryListViewItems.Add(lvi);
                form1.listView_inventory.Items.Add(lvi);
                counter += 1;
            }
            form1.listView_inventory.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            InitializeSearchBar();
        }

        private void InitializeSearchBar()
        {
            form1.BeginInvoke((MethodInvoker)delegate
            {
                var parent = form1.listView_inventory.Parent;
                if (parent != null && parent.Name != "searchContainerPanel")
                {
                    TableLayoutPanel searchContainer = new TableLayoutPanel()
                    {
                        Name = "searchContainerPanel",
                        Dock = DockStyle.Fill,
                        RowCount = 2,
                        ColumnCount = 1
                    };
                    searchContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
                    searchContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

                    Panel searchBarPanel = new Panel()
                    {
                        Dock = DockStyle.Fill,
                        Margin = new Padding(0)
                    };

                    Label searchLabel = new Label()
                    {
                        Text = "Search:",
                        Location = new Point(3, 8),
                        Size = new Size(55, 20),
                        TextAlign = ContentAlignment.MiddleLeft
                    };

                    TextBox searchBox = new TextBox()
                    {
                        Name = "textBox_searchInventory",
                        Location = new Point(60, 5),
                        Width = 380
                    };

                    searchBox.TextChanged += (sender, e) =>
                    {
                        FilterInventoryList(searchBox.Text);
                    };

                    searchBarPanel.Controls.Add(searchLabel);
                    searchBarPanel.Controls.Add(searchBox);

                    int col = 0, row = 0;
                    if (parent is TableLayoutPanel tlp)
                    {
                        col = tlp.GetColumn(form1.listView_inventory);
                        row = tlp.GetRow(form1.listView_inventory);
                        tlp.Controls.Remove(form1.listView_inventory);
                    }
                    else
                    {
                        parent.Controls.Remove(form1.listView_inventory);
                    }

                    searchContainer.Controls.Add(searchBarPanel, 0, 0);
                    searchContainer.Controls.Add(form1.listView_inventory, 0, 1);

                    if (parent is TableLayoutPanel tlp2)
                    {
                        tlp2.Controls.Add(searchContainer, col, row);
                    }
                    else
                    {
                        parent.Controls.Add(searchContainer);
                    }
                }
            });
        }

        private void FilterInventoryList(string query)
        {
            form1.listView_inventory.BeginUpdate();
            form1.listView_inventory.Items.Clear();

            var filtered = allInventoryListViewItems;
            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.Trim();
                filtered = allInventoryListViewItems.Where(item =>
                    item.Text.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 || 
                    item.SubItems[1].Text.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 || 
                    item.SubItems[2].Text.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();
            }

            foreach (var item in filtered)
            {
                form1.listView_inventory.Items.Add(item);
            }

            form1.listView_inventory.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            form1.listView_inventory.EndUpdate();
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

                var idLabel = form1.panel_inventory.Controls.Find("InventoryManager_" + index.ToString() + "_idLabel", false);
                if (idLabel.Length > 0) idLabel[0].Show();
                var descLabel = form1.panel_inventory.Controls.Find("InventoryManager_" + index.ToString() + "_descLabel", false);
                if (descLabel.Length > 0) descLabel[0].Show();
            }
            else
            {
                string displayName = "Unknown Item";
                string realId = "Unknown";
                string description = "";

                foreach (ListViewItem item in allInventoryListViewItems)
                {
                    if (item.Tag is int tagIdx && tagIdx == index)
                    {
                        displayName = item.Text;
                        realId = item.SubItems[1].Text;
                        description = item.SubItems[2].Text;
                        break;
                    }
                }

                form1.panel_inventory.Controls.Add(new Label() { Text = displayName, Name = "InventoryManager_" + index.ToString() + "_label", Location = new Point(10, 10), AutoSize = true, Font = new Font(form1.Font, FontStyle.Bold) });
                form1.panel_inventory.Controls.Add(new Label() { Text = "ID: " + realId, Name = "InventoryManager_" + index.ToString() + "_idLabel", Location = new Point(10, 35), AutoSize = true });
                form1.panel_inventory.Controls.Add(new Label() { Text = "Description: " + description, Name = "InventoryManager_" + index.ToString() + "_descLabel", Location = new Point(10, 60), AutoSize = true, MaximumSize = new Size(450, 0) });

                var initialValue = xmlData.SelectSingleNode(inventoryMap[index].Path).InnerText;
                var inputCtrl = CreateInputControl(initialValue, "InventoryManager_" + index.ToString() + "_textbox", new Point(200, 10));
                form1.panel_inventory.Controls.Add(inputCtrl);
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
            
            // stored water details
            form1.listView_saveInfo.Items.Add(new ListViewItem() { Text = "Stored Water" });
            saveInfoMap.Add(("SaveInfo_" + counter++.ToString() + "_textbox", "WaterManager/storedWater"));
            form1.listView_saveInfo.Items.Add(new ListViewItem() { Text = "Max Stored Water" });
            saveInfoMap.Add(("SaveInfo_" + counter++.ToString() + "_textbox", "WaterManager/maxStoredWater"));

            // stored food details
            form1.listView_saveInfo.Items.Add(new ListViewItem() { Text = "Pantry Rations" });
            saveInfoMap.Add(("SaveInfo_" + counter++.ToString() + "_textbox", "FoodManager/rations"));
            form1.listView_saveInfo.Items.Add(new ListViewItem() { Text = "Pantry Max Rations" });
            saveInfoMap.Add(("SaveInfo_" + counter++.ToString() + "_textbox", "FoodManager/maxRations"));
            form1.listView_saveInfo.Items.Add(new ListViewItem() { Text = "Pantry Raw Meat" });
            saveInfoMap.Add(("SaveInfo_" + counter++.ToString() + "_textbox", "FoodManager/meat"));
            form1.listView_saveInfo.Items.Add(new ListViewItem() { Text = "Pantry Max Meat" });
            saveInfoMap.Add(("SaveInfo_" + counter++.ToString() + "_textbox", "FoodManager/maxMeat"));

            // oxygen & radiation
            form1.listView_saveInfo.Items.Add(new ListViewItem() { Text = "Oxygen Level" });
            saveInfoMap.Add(("SaveInfo_" + counter++.ToString() + "_textbox", "EnvironmentManager/oxygenLevel"));
            form1.listView_saveInfo.Items.Add(new ListViewItem() { Text = "Radiation Level" });
            saveInfoMap.Add(("SaveInfo_" + counter++.ToString() + "_textbox", "EnvironmentManager/radiationLevel"));

            // Pet stats
            if (xmlData.SelectSingleNode("Pet") != null)
            {
                form1.listView_saveInfo.Items.Add(new ListViewItem() { Text = "Pet Dead" });
                saveInfoMap.Add(("SaveInfo_" + counter++.ToString() + "_textbox", "Pet/dead"));
                form1.listView_saveInfo.Items.Add(new ListViewItem() { Text = "Pet Health" });
                saveInfoMap.Add(("SaveInfo_" + counter++.ToString() + "_textbox", "Pet/health"));
                form1.listView_saveInfo.Items.Add(new ListViewItem() { Text = "Pet Hunger" });
                saveInfoMap.Add(("SaveInfo_" + counter++.ToString() + "_textbox", "Pet/hunger"));
            }

            // Game Time
            form1.listView_saveInfo.Items.Add(new ListViewItem() { Text = "Game Time Days" });
            saveInfoMap.Add(("SaveInfo_" + counter++.ToString() + "_textbox", "GameTime/days"));
            form1.listView_saveInfo.Items.Add(new ListViewItem() { Text = "Game Time Hour" });
            saveInfoMap.Add(("SaveInfo_" + counter++.ToString() + "_textbox", "GameTime/hour"));
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
                if (node == null)
                {
                    form1.panel_saveInfo.Controls.Add(new Label() { Text = form1.listView_saveInfo.Items[index].Text, Name = "SaveInfo_" + index.ToString() + "_label", Location = new Point(10, 10), AutoSize = true });
                    form1.panel_saveInfo.Controls.Add(new Label() { Text = "Not available in this save file", Name = "SaveInfo_" + index.ToString() + "_textbox", Location = new Point(200, 10), AutoSize = true, ForeColor = System.Drawing.Color.Red });
                    return;
                }

                if (node.Attributes != null && node.Attributes.Count > 0)
                {
                    string password = "";
                    foreach (XmlAttribute attr in node.Attributes)
                    {
                        password += attr.InnerText;
                    }
                    form1.panel_saveInfo.Controls.Add(new Label() { Text = form1.listView_saveInfo.Items[index].Text, Name = "SaveInfo_" + index.ToString() + "_label", Location = new Point(10, 10), AutoSize = true });
                    var inputCtrl = CreateInputControl(password, "SaveInfo_" + index.ToString() + "_textbox", new Point(200, 10));
                    form1.panel_saveInfo.Controls.Add(inputCtrl);
                }
                else
                {
                    form1.panel_saveInfo.Controls.Add(new Label() { Text = form1.listView_saveInfo.Items[index].Text, Name = "SaveInfo_" + index.ToString() + "_label", Location = new Point(10, 10), AutoSize = true });
                    var initialValue = node.InnerText;
                    var inputCtrl = CreateInputControl(initialValue, "SaveInfo_" + index.ToString() + "_textbox", new Point(200, 10));
                    form1.panel_saveInfo.Controls.Add(inputCtrl);
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
                        var initialValue = ((int)Math.Floor(double.Parse(value.InnerText) * 255)).ToString();
                        var inputCtrl = CreateInputControl(initialValue, chosen + "_textbox", new Point(200, 10 + i * 20));
                        form1.panel_character.Controls.Add(inputCtrl);
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
                    var initialValue = xmlData.SelectSingleNode(familyMap[index].Path).InnerText;
                    var inputCtrl = CreateInputControl(initialValue, chosen + "_textbox", new Point(200, 10));
                    form1.panel_character.Controls.Add(inputCtrl);
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
                            var inputCtrl = CreateInputControl(xmlNode.InnerText, node.ToolTipText + "_textbox", new Point(200, 10 + 30 * internalCounter));
                            form1.panel_tree.Controls.Add(inputCtrl);
                            internalCounter += 1;
                        }
                    }
                    else
                    {
                        form1.panel_tree.Controls.Add(new Label() { Text = node.Text, Name = node.ToolTipText + "_label", Location = new Point(10, 10), AutoSize = true });
                        var initialValue = xmlData.SelectSingleNode(node.FullPath).InnerText;
                        var inputCtrl = CreateInputControl(initialValue, node.ToolTipText + "_textbox", new Point(200, 10));
                        form1.panel_tree.Controls.Add(inputCtrl);
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