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
    }
}