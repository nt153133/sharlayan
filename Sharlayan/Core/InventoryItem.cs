// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryItem.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   InventoryItem.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sharlayan.Core {
    using Sharlayan.Core.Interfaces;

    public class InventoryItem : IInventoryItem {
        public uint Amount { get; set; }

        public uint Durability { get; set; }

        public double DurabilityPercent => (double) decimal.Divide((100 *this.Durability), 30000);

        public uint GlamourID { get; set; }

        public uint ID { get; set; }

        public bool IsHQ => (HqFlag & 1) == 1;

        public uint SB { get; set; }

        public double SBPercent => (double) decimal.Divide(this.SB, 100);

        public int Slot { get; set; }

        public bool IsCollectable => (HqFlag & 8) > 0;

        public byte HqFlag { get; set; }

        public uint Collectability
        {
            get
            {
                if (!IsCollectable)
                {
                    return 0u;
                }
                return SB;
            }
        }
    }
}