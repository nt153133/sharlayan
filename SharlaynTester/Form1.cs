using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharlaynTester {
    using Sharlayan;
    using Sharlayan.Core;
    using Sharlayan.Core.Enums;

    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            richTextBox1.Text = $"Attched: {Program.Attach()}\n";
        }


        private async void button2_Click_1(object sender, EventArgs e) {
            richTextBox1.Text += "Getting Inventory\n";
            listBox1.Items.Clear();

            var inventory = await Reader.GetInventory();
            
            richTextBox1.Text += $"Number of results {inventory.InventoryContainers.Count}\n";

            

            foreach (InventoryContainer bag in inventory.InventoryContainers)
            {
                listBox1.Items.Add($"Bag: {bag.BagId}  {bag.TypeID}  {bag.Amount} {bag.BagType}");
                foreach (InventoryItem item in bag.InventoryItems)
                {
                    listBox1.Items.Add($"{item.ID} : {item.Amount}");
                }
                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<Inventory.InventoryBagId> inventoryBagIds = (from Inventory.InventoryBagId bag in Enum.GetValues(typeof(Inventory.InventoryBagId))
                                                              select bag).ToList();

            comboBox1.DataSource = inventoryBagIds;
        }

        private async void Button3_ClickAsync(object sender, EventArgs e)
        {
            var inventory = await Reader.GetInventory();
            listBox1.Items.Clear();
            var bag = inventory.InventoryContainers.FirstOrDefault(i => i.BagId == (Inventory.InventoryBagId)comboBox1.SelectedItem);

            if (bag != null)
                foreach (InventoryItem item in bag.InventoryItems)
                {
                    listBox1.Items.Add(item.ToStringMine());
                }


        }

        private async void Button4_Click(object sender, EventArgs e)
        {
            
            if (listBox1.SelectedItem == null)
                return;

            var inventory = await Reader.GetInventory();
            var item = inventory.InventoryContainers.FirstOrDefault(i => i.BagId == (Inventory.InventoryBagId)comboBox1.SelectedItem).InventoryItems.ToArray()[listBox1.SelectedIndex];

            var test = typeof(InventoryItem).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var test2 = typeof(InventoryItem).GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            richTextBox1.Text = "Item ";
            richTextBox1.Text += $"Properties ({test.Length}) \n";
            foreach (var prop in test)
            {
                richTextBox1.Text += $"{prop.Name} {prop.GetValue(item)}\n";
            }
        }
    }


}