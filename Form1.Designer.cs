namespace ShelteredSE
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.openSaveFile = new System.Windows.Forms.OpenFileDialog();
            this.panel_welcome = new System.Windows.Forms.Panel();
            this.button_upload = new System.Windows.Forms.Button();
            this.label_welcome = new System.Windows.Forms.Label();
            this.editorSelector = new System.Windows.Forms.TabControl();
            this.generalTab = new System.Windows.Forms.TabPage();
            this.button_save = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveInfoTab = new System.Windows.Forms.TabPage();
            this.tableLayout_saveInfo = new System.Windows.Forms.TableLayoutPanel();
            this.listView_saveInfo = new System.Windows.Forms.ListView();
            this.panel_saveInfo = new System.Windows.Forms.Panel();
            this.inventoryTab = new System.Windows.Forms.TabPage();
            this.tableLayout_inventory = new System.Windows.Forms.TableLayoutPanel();
            this.listView_inventory = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel_inventory = new System.Windows.Forms.Panel();
            this.characterTab = new System.Windows.Forms.TabPage();
            this.tableLayout_character = new System.Windows.Forms.TableLayoutPanel();
            this.treeView_character = new System.Windows.Forms.TreeView();
            this.panel_character = new System.Windows.Forms.Panel();
            this.treeTab = new System.Windows.Forms.TabPage();
            this.tableLayout_tree = new System.Windows.Forms.TableLayoutPanel();
            this.treeView_tree = new System.Windows.Forms.TreeView();
            this.panel_tree = new System.Windows.Forms.Panel();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.panel_welcome.SuspendLayout();
            this.editorSelector.SuspendLayout();
            this.generalTab.SuspendLayout();
            this.saveInfoTab.SuspendLayout();
            this.tableLayout_saveInfo.SuspendLayout();
            this.inventoryTab.SuspendLayout();
            this.tableLayout_inventory.SuspendLayout();
            this.characterTab.SuspendLayout();
            this.tableLayout_character.SuspendLayout();
            this.treeTab.SuspendLayout();
            this.tableLayout_tree.SuspendLayout();
            this.SuspendLayout();
            // 
            // openSaveFile
            // 
            this.openSaveFile.DefaultExt = "dat";
            this.openSaveFile.FileName = "savegame_0x";
            this.openSaveFile.Filter = "DAT files|*.dat";
            // 
            // panel_welcome
            // 
            this.panel_welcome.AutoSize = true;
            this.panel_welcome.Controls.Add(this.button_upload);
            this.panel_welcome.Controls.Add(this.label_welcome);
            this.panel_welcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_welcome.Location = new System.Drawing.Point(0, 0);
            this.panel_welcome.Margin = new System.Windows.Forms.Padding(0);
            this.panel_welcome.Name = "panel_welcome";
            this.panel_welcome.Size = new System.Drawing.Size(730, 561);
            this.panel_welcome.TabIndex = 0;
            // 
            // button_upload
            // 
            this.button_upload.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_upload.AutoSize = true;
            this.button_upload.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_upload.Location = new System.Drawing.Point(341, 256);
            this.button_upload.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_upload.Name = "button_upload";
            this.button_upload.Size = new System.Drawing.Size(157, 37);
            this.button_upload.TabIndex = 1;
            this.button_upload.Text = "Open Save File";
            this.button_upload.UseVisualStyleBackColor = true;
            this.button_upload.Click += new System.EventHandler(this.button1_Click);
            // 
            // label_welcome
            // 
            this.label_welcome.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label_welcome.AutoSize = true;
            this.label_welcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_welcome.Location = new System.Drawing.Point(205, 43);
            this.label_welcome.Name = "label_welcome";
            this.label_welcome.Size = new System.Drawing.Size(352, 26);
            this.label_welcome.TabIndex = 0;
            this.label_welcome.Text = "Welcome to Sheltered Save Editor!";
            // 
            // editorSelector
            // 
            this.editorSelector.Controls.Add(this.generalTab);
            this.editorSelector.Controls.Add(this.saveInfoTab);
            this.editorSelector.Controls.Add(this.inventoryTab);
            this.editorSelector.Controls.Add(this.characterTab);
            this.editorSelector.Controls.Add(this.treeTab);
            this.editorSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorSelector.Font = new System.Drawing.Font("Arial", 9.75F);
            this.editorSelector.Location = new System.Drawing.Point(0, 0);
            this.editorSelector.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.editorSelector.Name = "editorSelector";
            this.editorSelector.SelectedIndex = 0;
            this.editorSelector.Size = new System.Drawing.Size(730, 561);
            this.editorSelector.TabIndex = 0;
            // 
            // generalTab
            // 
            this.generalTab.BackColor = System.Drawing.Color.White;
            this.generalTab.Controls.Add(this.button_save);
            this.generalTab.Controls.Add(this.label2);
            this.generalTab.Controls.Add(this.richTextBox1);
            this.generalTab.Controls.Add(this.label1);
            this.generalTab.Location = new System.Drawing.Point(4, 25);
            this.generalTab.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.generalTab.Name = "generalTab";
            this.generalTab.Size = new System.Drawing.Size(722, 532);
            this.generalTab.TabIndex = 4;
            this.generalTab.Text = "Info & Save";
            // 
            // button_save
            // 
            this.button_save.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_save.AutoSize = true;
            this.button_save.Location = new System.Drawing.Point(563, 52);
            this.button_save.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(87, 32);
            this.button_save.TabIndex = 4;
            this.button_save.Text = "Save";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 14F);
            this.label2.Location = new System.Drawing.Point(538, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 22);
            this.label2.TabIndex = 3;
            this.label2.Text = "Save Changes";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("Arial", 9.75F);
            this.richTextBox1.Location = new System.Drawing.Point(43, 52);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.Size = new System.Drawing.Size(470, 389);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 14F);
            this.label1.Location = new System.Drawing.Point(38, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Important Notices";
            // 
            // saveInfoTab
            // 
            this.saveInfoTab.Controls.Add(this.tableLayout_saveInfo);
            this.saveInfoTab.Location = new System.Drawing.Point(4, 25);
            this.saveInfoTab.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.saveInfoTab.Name = "saveInfoTab";
            this.saveInfoTab.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.saveInfoTab.Size = new System.Drawing.Size(722, 532);
            this.saveInfoTab.TabIndex = 0;
            this.saveInfoTab.Text = "Save Info";
            this.saveInfoTab.UseVisualStyleBackColor = true;
            // 
            // tableLayout_saveInfo
            // 
            this.tableLayout_saveInfo.AutoScroll = true;
            this.tableLayout_saveInfo.ColumnCount = 2;
            this.tableLayout_saveInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.59477F));
            this.tableLayout_saveInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.40523F));
            this.tableLayout_saveInfo.Controls.Add(this.listView_saveInfo, 0, 0);
            this.tableLayout_saveInfo.Controls.Add(this.panel_saveInfo, 1, 0);
            this.tableLayout_saveInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout_saveInfo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayout_saveInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tableLayout_saveInfo.Location = new System.Drawing.Point(3, 4);
            this.tableLayout_saveInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayout_saveInfo.Name = "tableLayout_saveInfo";
            this.tableLayout_saveInfo.RowCount = 1;
            this.tableLayout_saveInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout_saveInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 524F));
            this.tableLayout_saveInfo.Size = new System.Drawing.Size(716, 524);
            this.tableLayout_saveInfo.TabIndex = 0;
            // 
            // listView_saveInfo
            // 
            this.listView_saveInfo.AutoArrange = false;
            this.listView_saveInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_saveInfo.Location = new System.Drawing.Point(3, 4);
            this.listView_saveInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listView_saveInfo.MultiSelect = false;
            this.listView_saveInfo.Name = "listView_saveInfo";
            this.listView_saveInfo.Size = new System.Drawing.Size(198, 516);
            this.listView_saveInfo.TabIndex = 0;
            this.listView_saveInfo.UseCompatibleStateImageBehavior = false;
            this.listView_saveInfo.View = System.Windows.Forms.View.List;
            this.listView_saveInfo.SelectedIndexChanged += new System.EventHandler(this.listView2_SelectedIndexChanged);
            // 
            // panel_saveInfo
            // 
            this.panel_saveInfo.AutoSize = true;
            this.panel_saveInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_saveInfo.Location = new System.Drawing.Point(207, 4);
            this.panel_saveInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel_saveInfo.Name = "panel_saveInfo";
            this.panel_saveInfo.Size = new System.Drawing.Size(506, 516);
            this.panel_saveInfo.TabIndex = 1;
            // 
            // inventoryTab
            // 
            this.inventoryTab.Controls.Add(this.tableLayout_inventory);
            this.inventoryTab.Location = new System.Drawing.Point(4, 25);
            this.inventoryTab.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.inventoryTab.Name = "inventoryTab";
            this.inventoryTab.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.inventoryTab.Size = new System.Drawing.Size(722, 532);
            this.inventoryTab.TabIndex = 2;
            this.inventoryTab.Text = "Inventory Editor";
            this.inventoryTab.UseVisualStyleBackColor = true;
            // 
            // tableLayout_inventory
            // 
            this.tableLayout_inventory.ColumnCount = 2;
            this.tableLayout_inventory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.90196F));
            this.tableLayout_inventory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.09804F));
            this.tableLayout_inventory.Controls.Add(this.listView_inventory, 0, 0);
            this.tableLayout_inventory.Controls.Add(this.panel_inventory, 1, 0);
            this.tableLayout_inventory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout_inventory.Location = new System.Drawing.Point(3, 4);
            this.tableLayout_inventory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayout_inventory.Name = "tableLayout_inventory";
            this.tableLayout_inventory.RowCount = 1;
            this.tableLayout_inventory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayout_inventory.Size = new System.Drawing.Size(716, 524);
            this.tableLayout_inventory.TabIndex = 0;
            // 
            // listView_inventory
            // 
            this.listView_inventory.AutoArrange = false;
            this.listView_inventory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView_inventory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_inventory.Font = new System.Drawing.Font("Arial", 9.75F);
            this.listView_inventory.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView_inventory.Location = new System.Drawing.Point(3, 4);
            this.listView_inventory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listView_inventory.Name = "listView_inventory";
            this.listView_inventory.Size = new System.Drawing.Size(208, 516);
            this.listView_inventory.TabIndex = 0;
            this.listView_inventory.UseCompatibleStateImageBehavior = false;
            this.listView_inventory.View = System.Windows.Forms.View.Details;
            this.listView_inventory.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // panel_inventory
            // 
            this.panel_inventory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_inventory.Location = new System.Drawing.Point(217, 4);
            this.panel_inventory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel_inventory.Name = "panel_inventory";
            this.panel_inventory.Size = new System.Drawing.Size(496, 516);
            this.panel_inventory.TabIndex = 1;
            // 
            // characterTab
            // 
            this.characterTab.Controls.Add(this.tableLayout_character);
            this.characterTab.Location = new System.Drawing.Point(4, 25);
            this.characterTab.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.characterTab.Name = "characterTab";
            this.characterTab.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.characterTab.Size = new System.Drawing.Size(722, 532);
            this.characterTab.TabIndex = 3;
            this.characterTab.Text = "Character Editor";
            this.characterTab.UseVisualStyleBackColor = true;
            // 
            // tableLayout_character
            // 
            this.tableLayout_character.ColumnCount = 2;
            this.tableLayout_character.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.88235F));
            this.tableLayout_character.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.11765F));
            this.tableLayout_character.Controls.Add(this.treeView_character, 0, 0);
            this.tableLayout_character.Controls.Add(this.panel_character, 1, 0);
            this.tableLayout_character.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout_character.Location = new System.Drawing.Point(3, 4);
            this.tableLayout_character.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayout_character.Name = "tableLayout_character";
            this.tableLayout_character.RowCount = 1;
            this.tableLayout_character.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout_character.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 524F));
            this.tableLayout_character.Size = new System.Drawing.Size(716, 524);
            this.tableLayout_character.TabIndex = 0;
            // 
            // treeView_character
            // 
            this.treeView_character.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_character.Font = new System.Drawing.Font("Arial", 9.75F);
            this.treeView_character.Location = new System.Drawing.Point(3, 4);
            this.treeView_character.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeView_character.Name = "treeView_character";
            this.treeView_character.PathSeparator = "/";
            this.treeView_character.Size = new System.Drawing.Size(215, 516);
            this.treeView_character.TabIndex = 0;
            this.treeView_character.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView2_AfterSelect);
            // 
            // panel_character
            // 
            this.panel_character.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_character.Font = new System.Drawing.Font("Arial", 9.75F);
            this.panel_character.Location = new System.Drawing.Point(224, 4);
            this.panel_character.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel_character.Name = "panel_character";
            this.panel_character.Size = new System.Drawing.Size(489, 516);
            this.panel_character.TabIndex = 1;
            // 
            // treeTab
            // 
            this.treeTab.Controls.Add(this.tableLayout_tree);
            this.treeTab.Location = new System.Drawing.Point(4, 25);
            this.treeTab.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeTab.Name = "treeTab";
            this.treeTab.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeTab.Size = new System.Drawing.Size(722, 532);
            this.treeTab.TabIndex = 1;
            this.treeTab.Text = "Tree Editor";
            this.treeTab.UseVisualStyleBackColor = true;
            // 
            // tableLayout_tree
            // 
            this.tableLayout_tree.ColumnCount = 2;
            this.tableLayout_tree.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.16993F));
            this.tableLayout_tree.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.83006F));
            this.tableLayout_tree.Controls.Add(this.treeView_tree, 0, 0);
            this.tableLayout_tree.Controls.Add(this.panel_tree, 1, 0);
            this.tableLayout_tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout_tree.Location = new System.Drawing.Point(3, 4);
            this.tableLayout_tree.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayout_tree.Name = "tableLayout_tree";
            this.tableLayout_tree.RowCount = 1;
            this.tableLayout_tree.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayout_tree.Size = new System.Drawing.Size(716, 524);
            this.tableLayout_tree.TabIndex = 1;
            // 
            // treeView_tree
            // 
            this.treeView_tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_tree.Font = new System.Drawing.Font("Arial", 9.75F);
            this.treeView_tree.Location = new System.Drawing.Point(3, 4);
            this.treeView_tree.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeView_tree.Name = "treeView_tree";
            this.treeView_tree.PathSeparator = "/";
            this.treeView_tree.Size = new System.Drawing.Size(231, 516);
            this.treeView_tree.TabIndex = 0;
            this.treeView_tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // panel_tree
            // 
            this.panel_tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_tree.Font = new System.Drawing.Font("Arial", 9.75F);
            this.panel_tree.Location = new System.Drawing.Point(240, 4);
            this.panel_tree.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel_tree.Name = "panel_tree";
            this.panel_tree.Size = new System.Drawing.Size(473, 516);
            this.panel_tree.TabIndex = 1;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "dat";
            this.saveFileDialog.Filter = "DAT Files|*.dat";
            this.saveFileDialog.RestoreDirectory = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 561);
            this.Controls.Add(this.editorSelector);
            this.Controls.Add(this.panel_welcome);
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Text = "Sheltered Save Editor - emreyigitcbc";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel_welcome.ResumeLayout(false);
            this.panel_welcome.PerformLayout();
            this.editorSelector.ResumeLayout(false);
            this.generalTab.ResumeLayout(false);
            this.generalTab.PerformLayout();
            this.saveInfoTab.ResumeLayout(false);
            this.tableLayout_saveInfo.ResumeLayout(false);
            this.tableLayout_saveInfo.PerformLayout();
            this.inventoryTab.ResumeLayout(false);
            this.tableLayout_inventory.ResumeLayout(false);
            this.characterTab.ResumeLayout(false);
            this.tableLayout_character.ResumeLayout(false);
            this.treeTab.ResumeLayout(false);
            this.tableLayout_tree.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openSaveFile;
        private System.Windows.Forms.Panel panel_welcome;
        private System.Windows.Forms.Label label_welcome;
        private System.Windows.Forms.TabControl editorSelector;
        private System.Windows.Forms.TabPage saveInfoTab;
        private System.Windows.Forms.TabPage treeTab;
        private System.Windows.Forms.TabPage inventoryTab;
        private System.Windows.Forms.TabPage characterTab;
        public System.Windows.Forms.TreeView treeView_tree;
        public System.Windows.Forms.TableLayoutPanel tableLayout_saveInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayout_inventory;
        public System.Windows.Forms.ListView listView_inventory;
        public System.Windows.Forms.ListView listView_saveInfo;
        public System.Windows.Forms.Panel panel_saveInfo;
        public System.Windows.Forms.Panel panel_inventory;
        public System.Windows.Forms.TableLayoutPanel tableLayout_character;
        public System.Windows.Forms.TreeView treeView_character;
        public System.Windows.Forms.TableLayoutPanel tableLayout_tree;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        public System.Windows.Forms.Panel panel_character;
        public System.Windows.Forms.Panel panel_tree;
        public System.Windows.Forms.TabPage generalTab;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button_save;
        public System.Windows.Forms.Button button_upload;
        public System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

