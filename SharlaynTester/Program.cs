using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharlaynTester {
    using System.Diagnostics;
    using System.Threading;

    using Sharlayan;
    using Sharlayan.Core;
    using Sharlayan.Events;
    using Sharlayan.Models;

    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        
        internal static bool Attach()
        {
            Process process = Process.GetProcessesByName("ffxiv_dx11").FirstOrDefault();

            if (process != null) {

                MemoryHandler.Instance.SignaturesFoundEvent += delegate (object sender, SignaturesFoundEvent e) {
                    foreach (KeyValuePair<string, Signature> kvp in e.Signatures)
                    {
                        Debug.WriteLine($"{kvp.Key} => {kvp.Value.GetAddress().ToInt64():X}");
                    }
                };

                MemoryHandler.Instance.SetProcess(
                    new ProcessModel {
                        IsWin64 = true,
                        Process = process
                    }).GetAwaiter().GetResult();

                while (Scanner.Instance.IsScanning) {
                    Thread.Sleep(1000);
                    Console.WriteLine("Scanning...");
                }

                /*var signatures = new List<Signature>();
                signatures.Add(new Signature
                {
                    Key = "INVENTORYBAGS",
                    PointerPath = new List<long>
                    {
                        0x15786f0
                    }
                });
                
                Scanner.Instance.LoadOffsets(Signatures.Resolve(ProcessModel.IsWin64, GameLanguage));*/
                
                return true;
            }

            return false;
        }


    }
    public static class InventoryItemExt
    {
        public static string ToStringMine(this InventoryItem item)
        {
            return $"{item.ID} : {item.Amount}";
        }
    }
}