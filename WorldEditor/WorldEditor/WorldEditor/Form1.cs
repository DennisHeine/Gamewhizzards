using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Data;
namespace WorldEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
        }

        public void selectItem()
        {
            String name = lbItems.SelectedItem.ToString();
            Data.Items.Item item = loadItemFromName(name);
            txtItemUIImage_UIImage.Text = item.UIImage;
            txtItemMaxLevel_MaxLevel.Text = item.MaxLevel.ToString();
            txtItemsMinLevel_MinLevel.Text = item.MinLevel.ToString();
            txtItemMesh_MeshNameAndPath.Text = item.MeshNameAndPath;
        }

        public void loadItems()
        {
            Width = grpSpells.Right + 25;
            Height = grpResources.Bottom + 75;
            foreach (Data.Items.Item item in WorldState.worldState.Items.Values)
            {
                lbItems.Items.Add(item.Name);
            }
        }

        public void saveItem()
        {
            String name = lbItems.SelectedItem.ToString();
            Data.Items.Item item = loadItemFromName(name);
            item.MaxLevel = int.Parse(txtItemMaxLevel_MaxLevel.Text);
            item.MinLevel = int.Parse(txtItemsMinLevel_MinLevel.Text);
            item.MeshNameAndPath = txtItemMesh_MeshNameAndPath.Text;
            item.UIImage = txtItemUIImage_UIImage.Text;
            ReplaceItem(item.Name, item);
        }

        private void ReplaceItem(object itemName, Data.Items.Item item)
        {
            bool found = false;
            foreach (Data.Items.Item itm in WorldState.worldState.Items.Values)
            {
                if (itm.Name == (String)itemName)
                {
                    found = true;
                    WorldState.worldState.Items[itm.Name] = item;
                }
            }
            if (!found)
                WorldState.worldState.Items.Add(item.Name, item);

        }

        private Data.Items.Item loadItemFromName(string name)
        {
            foreach(Data.Items.Item itm in WorldState.worldState.Items.Values)
            {
                if (itm.Name == name)
                    return itm;
            }
            return new Data.Items.Item();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            grpStats.Visible = true;
            grpStats.Left = ((Button)sender).Left;
            grpStats.Top = ((Button)sender).Bottom;

        }

        private void button9_Click(object sender, EventArgs e)
        {
            grpStats.Visible = true;
            grpStats.Left = ((Button)sender).Left;
            grpStats.Top = ((Button)sender).Bottom;

        }

        private void button10_Click(object sender, EventArgs e)
        {
            grpStats.Visible = true;
            grpStats.Left = ((Button)sender).Left;
            grpStats.Top = ((Button)sender).Bottom;

        }

        private void button32_Click(object sender, EventArgs e)
        {
            grpStats.Visible = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {

            grpSpellRotation.Visible = true;
            grpSpellRotation.Left = ((Button)sender).Left;
            grpSpellRotation.Top = ((Button)sender).Bottom;
        }

        private void button36_Click(object sender, EventArgs e)
        {
            grpDialogs.Visible = false;
        }

        private void button35_Click(object sender, EventArgs e)
        {
            grpLoot.Visible = false;
        }

        private void button34_Click(object sender, EventArgs e)
        {
            grpSpellRotation.Visible = false;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(Control c in groupBox1.Controls)
            {
                if(c.GetType()==typeof(TextBox))
                {
                    TextBox c1 = (TextBox)c;
                    c1.Text = "";
                }
            }
            lbItems.SelectedIndex = -1;
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //saveItem();
            FormControlItems c = new FormControlItems(this, "Data.Items.Item", new Dictionary<string, GroupBox>(), groupBox1, lbItems, "WorldClass");
            c.saveItem();
        }

        private void lbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectItem();
        }
    }
}
