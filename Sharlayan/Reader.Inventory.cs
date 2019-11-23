// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Reader.Inventory.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Reader.Inventory.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sharlayan {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Sharlayan.Core;
    using Sharlayan.Core.Enums;
    using Sharlayan.Models.ReadResults;

    public static partial class Reader {
        public static bool CanGetInventory() {
            var canRead = Scanner.Instance.Locations.ContainsKey(Signatures.InventoryKey);
            if (canRead) {
                // OTHER STUFF?
            }

            return canRead;
        }

        private static Dictionary<int, int> bags; 

        //public static 
        public static async Task<InventoryResult> GetInventory() {
            var result = new InventoryResult();

            if (!CanGetInventory() || !MemoryHandler.Instance.IsAttached) {
                return result;
            }
            
            //int bagCount = MemoryHandler.Instance.GetInt32(Scanner.Instance.Locations["INVENTORYSTART"].GetAddress(), 0x4a);
            //Debug.WriteLine($"bagcount {bagCount}");
            var BagList = MemoryHandler.Instance.GetArray<int>(Scanner.Instance.Locations["INVENTORYBAGS"].GetAddress(), 0x4a);

             bags = new Dictionary<int, int>();

            
         
            for (var index = 0; index < BagList.Length; index++) {
                
                var i = BagList[index];
                bags.Add(i, index);
                Debug.WriteLine($"bag {index} is type {i}");
            }
            


            try {
                foreach (Inventory.InventoryBagId bag in Enum.GetValues(typeof(Inventory.InventoryBagId)))
                {
                    if (bags.ContainsKey((int)bag))
                        result.InventoryContainers.Add(GetInventoryItemsBags(bag));
                }
                
                
                /*
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.INVENTORY_1));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.INVENTORY_2));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.INVENTORY_3));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.INVENTORY_4));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.CURRENT_EQ));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.EXTRA_EQ));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.CRYSTALS));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.QUESTS_KI));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.HIRE_1));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.HIRE_2));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.HIRE_3));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.HIRE_4));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.HIRE_5));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.HIRE_6));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.HIRE_7));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.COMPANY_1));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.COMPANY_2));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.COMPANY_3));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.COMPANY_CRYSTALS));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.AC_MH));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.AC_OH));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.AC_HEAD));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.AC_BODY));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.AC_HANDS));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.AC_BELT));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.AC_LEGS));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.AC_FEET));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.AC_EARRINGS));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.AC_NECK));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.AC_WRISTS));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.AC_RINGS));
                result.InventoryContainers.Add(GetInventoryItems(Inventory.Container.AC_SOULS));
                */
            }
            catch (Exception ex) {
                MemoryHandler.Instance.RaiseException(Logger, ex, true);
            }

            return result;
        }

        private static InventoryContainer GetInventoryItems(Inventory.Container type) {
            var InventoryPointerMap = new IntPtr(MemoryHandler.Instance.GetPlatformUInt(Scanner.Instance.Locations[Signatures.InventoryKey]));
            //Debug.WriteLine($"InventoryPointerMap : {InventoryPointerMap.ToInt64():X}");

            var offset = (uint) ((int) type * 24);
            var containerAddress = MemoryHandler.Instance.GetPlatformUInt(InventoryPointerMap, offset);

            var container = new InventoryContainer {
                Amount = MemoryHandler.Instance.GetByte(InventoryPointerMap, offset + MemoryHandler.Instance.Structures.InventoryContainer.Amount),
                TypeID = (byte) type,
                ContainerType = type,
                BagType = MemoryHandler.Instance.GetInt32(Scanner.Instance.Locations["INVENTORYBAGS"].GetAddress(), sizeof(int) * (int)type)
            };
            Debug.WriteLine($"bag {type} is bagid {MemoryHandler.Instance.GetInt32(InventoryPointerMap,  offset + 4)} ");
            // The number of item is 50 in COMPANY's locker
            int limit = MemoryHandler.Instance.GetByte(InventoryPointerMap, offset + MemoryHandler.Instance.Structures.InventoryContainer.Amount) * 56;
            /*switch (type) {
                case Inventory.Container.COMPANY_1:
                case Inventory.Container.COMPANY_2:
                case Inventory.Container.COMPANY_3:
                    limit = MemoryHandler.Instance.GetByte(InventoryPointerMap, offset + MemoryHandler.Instance.Structures.InventoryContainer.Amount) * 56;
                    break;
                default:
                    limit = MemoryHandler.Instance.GetByte(InventoryPointerMap, offset + MemoryHandler.Instance.Structures.InventoryContainer.Amount) * 56;
                    break;
            }*/

            for (var i = 0; i < limit; i += 56) {
                var itemOffset = new IntPtr(containerAddress + i);
                var id = MemoryHandler.Instance.GetPlatformUInt(itemOffset, MemoryHandler.Instance.Structures.InventoryItem.ID);
                if (id > 0) {
                    container.InventoryItems.Add(
                        new InventoryItem {
                            ID = (uint) id,
                            Slot = MemoryHandler.Instance.GetByte(itemOffset, MemoryHandler.Instance.Structures.InventoryItem.Slot),
                            Amount = (uint)MemoryHandler.Instance.GetPlatformUInt(itemOffset, MemoryHandler.Instance.Structures.InventoryItem.Amount),
                            SB = MemoryHandler.Instance.GetUInt16(itemOffset, MemoryHandler.Instance.Structures.InventoryItem.SB),
                            Durability = MemoryHandler.Instance.GetUInt16(itemOffset, MemoryHandler.Instance.Structures.InventoryItem.ID),
                            GlamourID = (uint) MemoryHandler.Instance.GetPlatformUInt(itemOffset, MemoryHandler.Instance.Structures.InventoryItem.GlamourID),

                            // get the flag that show if the item is hq or not
                            HqFlag = MemoryHandler.Instance.GetByte(itemOffset, MemoryHandler.Instance.Structures.InventoryItem.IsHQ) 
                        });
                }
            }

            return container;
        }
        
        private static InventoryContainer GetInventoryItemsBags(Inventory.InventoryBagId bagId) {
            var InventoryPointerMap = new IntPtr(MemoryHandler.Instance.GetPlatformUInt(Scanner.Instance.Locations[Signatures.InventoryKey]));
            //Debug.WriteLine($"InventoryPointerMap : {InventoryPointerMap.ToInt64():X}");
            int type = bags[(int) bagId];
            
            //Debug.WriteLine($"{bagId} {type}");
            var offset = (uint) ((int) type * 24);
            var containerAddress = MemoryHandler.Instance.GetPlatformUInt(InventoryPointerMap, offset);

            var container = new InventoryContainer {
                Amount = MemoryHandler.Instance.GetByte(InventoryPointerMap, offset + MemoryHandler.Instance.Structures.InventoryContainer.Amount),
                TypeID = (byte) type,
                BagId = bagId,
                BagType = MemoryHandler.Instance.GetInt32(Scanner.Instance.Locations["INVENTORYBAGS"].GetAddress(), sizeof(int) * (int)type)
            };

            int limit = MemoryHandler.Instance.GetByte(InventoryPointerMap, offset + MemoryHandler.Instance.Structures.InventoryContainer.Amount) * 56;

            for (var i = 0; i < limit; i += 56) {
                var itemOffset = new IntPtr(containerAddress + i);
                var id = MemoryHandler.Instance.GetPlatformUInt(itemOffset, MemoryHandler.Instance.Structures.InventoryItem.ID);
                if (id > 0) {
                    container.InventoryItems.Add(
                        new InventoryItem {
                            ID = (uint) id,
                            Slot = MemoryHandler.Instance.GetByte(itemOffset, MemoryHandler.Instance.Structures.InventoryItem.Slot),
                            Amount = (uint)MemoryHandler.Instance.GetPlatformUInt(itemOffset, MemoryHandler.Instance.Structures.InventoryItem.Amount),
                            SB = MemoryHandler.Instance.GetUInt16(itemOffset, MemoryHandler.Instance.Structures.InventoryItem.SB),
                            Durability = MemoryHandler.Instance.GetUInt16(itemOffset, MemoryHandler.Instance.Structures.InventoryItem.ID),
                            GlamourID = (uint) MemoryHandler.Instance.GetPlatformUInt(itemOffset, MemoryHandler.Instance.Structures.InventoryItem.GlamourID),

                            // get the flag that show if the item is hq or not
                            HqFlag = MemoryHandler.Instance.GetByte(itemOffset, MemoryHandler.Instance.Structures.InventoryItem.IsHQ)
                        });
                }
            }

            return container;
        }
    }
}