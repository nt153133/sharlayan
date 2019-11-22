// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Inventory.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Inventory.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sharlayan.Core.Enums {
    public class Inventory {
        public enum Container : byte {
            INVENTORY_1 = 0x0,

            INVENTORY_2 = 0x1,

            INVENTORY_3 = 0x2,

            INVENTORY_4 = 0x3,

            CURRENT_EQ = 0x4,

            EXTRA_EQ = 0x5,

            CRYSTALS = 0x6,

            QUESTS_KI = 0x9,

            HIRE_1 = 0x12,

            HIRE_2 = 0x13,

            HIRE_3 = 0x14,

            HIRE_4 = 0x15,

            HIRE_5 = 0x16,

            HIRE_6 = 0x17,

            HIRE_7 = 0x18,

            AC_MH = 0x1D,

            AC_OH = 0x1E,

            AC_HEAD = 0x1F,

            AC_BODY = 0x20,

            AC_HANDS = 0x21,

            AC_BELT = 0x22,

            AC_LEGS = 0x23,

            AC_FEET = 0x24,

            AC_EARRINGS = 0x25,

            AC_NECK = 0x26,

            AC_WRISTS = 0x27,

            AC_RINGS = 0x28,

            AC_SOULS = 0x29,

            COMPANY_1 = 0x2A,

            COMPANY_2 = 0x2B,

            COMPANY_3 = 0x2C,

            COMPANY_CRYSTALS = 0x2D
        }
        
        public enum InventoryBagId
        {
            Bag1 = 0,
            Bag2 = 1,
            Bag3 = 2,
            Bag4 = 3,
            Bag5 = 4,
            Bag6 = 5,
            EquippedItems = 1000,
            Currency = 2000,
            Crystals = 2001,
            KeyItems = 2004,
            Armory_MainHand = 3500,
            Armory_OffHand = 3200,
            Armory_Helmet = 3201,
            Armory_Chest = 3202,
            Armory_Glove = 3203,
            Armory_Belt = 3204,
            Armory_Pants = 3205,
            Armory_Boots = 3206,
            Armory_Earrings = 3207,
            Armory_Necklace = 3208,
            Armory_Writs = 3209,
            Armory_Rings = 3300,
            Armory_Souls = 3400,
            Retainer_Page1 = 10000,
            Retainer_Page2 = 10001,
            Retainer_Page3 = 10002,
            Retainer_Page4 = 10003,
            Retainer_Page5 = 10004,
            Retainer_Page6 = 10005,
            Retainer_Page7 = 10006,
            Retainer_EquippedItems = 11000,
            Retainer_Gil = 12000,
            Retainer_Crystals = 12001,
            Retainer_Market = 12002,
            GrandCompany_Page1 = 20000,
            GrandCompany_Page2 = 20001,
            GrandCompany_Page3 = 20002,
            GrandCompany_Gil = 22000,
            GrandCompany_Crystals = 22001
        }
    }
}