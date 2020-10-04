﻿using MMR.Randomizer.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace MMR.Randomizer.Asm
{
    /// <summary>
    /// MMR configuration structure.
    /// </summary>
    public struct MMRConfigStruct : IAsmConfigStruct
    {
        public uint Version;
        public ushort[] CycleRepeatableLocations;
        public ushort CycleRepeatableLocationsLength;

        public ushort LocationBottleRedPotion;
        public ushort LocationBottleGoldDust;
        public ushort LocationBottleMilk;
        public ushort LocationBottleChateau;

        public ushort LocationSwordKokiri;
        public ushort LocationSwordRazor;
        public ushort LocationSwordGilded;

        public ushort LocationMagicSmall;
        public ushort LocationMagicLarge;

        public ushort LocationWalletAdult;
        public ushort LocationWalletGiant;

        public ushort LocationBombBagSmall;
        public ushort LocationBombBagBig;
        public ushort LocationBombBagBiggest;

        public ushort LocationQuiverSmall;
        public ushort LocationQuiverLarge;
        public ushort LocationQuiverLargest;

        /// <summary>
        /// Convert to bytes.
        /// </summary>
        /// <returns>Bytes</returns>
        public byte[] ToBytes()
        {
            using (var memStream = new MemoryStream())
            using (var writer = new BinaryWriter(memStream))
            {
                ReadWriteUtils.WriteU32(writer, this.Version);

                foreach (var val in this.CycleRepeatableLocations)
                {
                    ReadWriteUtils.WriteU16(writer, val);
                }

                ReadWriteUtils.WriteU16(writer, CycleRepeatableLocationsLength);

                ReadWriteUtils.WriteU16(writer, this.LocationBottleRedPotion);
                ReadWriteUtils.WriteU16(writer, this.LocationBottleGoldDust);
                ReadWriteUtils.WriteU16(writer, this.LocationBottleMilk);
                ReadWriteUtils.WriteU16(writer, this.LocationBottleChateau);

                ReadWriteUtils.WriteU16(writer, this.LocationSwordKokiri);
                ReadWriteUtils.WriteU16(writer, this.LocationSwordRazor);
                ReadWriteUtils.WriteU16(writer, this.LocationSwordGilded);

                ReadWriteUtils.WriteU16(writer, this.LocationMagicSmall);
                ReadWriteUtils.WriteU16(writer, this.LocationMagicLarge);

                ReadWriteUtils.WriteU16(writer, this.LocationWalletAdult);
                ReadWriteUtils.WriteU16(writer, this.LocationWalletGiant);

                ReadWriteUtils.WriteU16(writer, this.LocationBombBagSmall);
                ReadWriteUtils.WriteU16(writer, this.LocationBombBagBig);
                ReadWriteUtils.WriteU16(writer, this.LocationBombBagBiggest);

                ReadWriteUtils.WriteU16(writer, this.LocationQuiverSmall);
                ReadWriteUtils.WriteU16(writer, this.LocationQuiverLarge);
                ReadWriteUtils.WriteU16(writer, this.LocationQuiverLargest);

                return memStream.ToArray();
            }
        }
    }

    /// <summary>
    /// MMR configuration.
    /// </summary>
    public class MMRConfig : AsmConfig
    {
        private ReadOnlyCollection<ushort> BaseCycleRepeatableLocations { get; }
        public List<ushort> CycleRepeatableLocations { get; }

        public ushort LocationBottleRedPotion { get; set; }
        public ushort LocationBottleGoldDust { get; set; }
        public ushort LocationBottleMilk { get; set; }
        public ushort LocationBottleChateau { get; set; }

        public ushort LocationSwordKokiri { get; set; }
        public ushort LocationSwordRazor { get; set; }
        public ushort LocationSwordGilded { get; set; }

        public ushort LocationMagicSmall { get; set; }
        public ushort LocationMagicLarge { get; set; }

        public ushort LocationWalletAdult { get; set; }
        public ushort LocationWalletGiant { get; set; }

        public ushort LocationBombBagSmall { get; set; }
        public ushort LocationBombBagBig { get; set; }
        public ushort LocationBombBagBiggest { get; set; }

        public ushort LocationQuiverSmall { get; set; }
        public ushort LocationQuiverLarge { get; set; }
        public ushort LocationQuiverLargest { get; set; }

        public MMRConfig()
        {
            CycleRepeatableLocations = new List<ushort>();
            BaseCycleRepeatableLocations = new ReadOnlyCollection<ushort>(new List<ushort>
            {
                0x0001,
                0x0002,
                0x0004,
                0x0005,
                0x0006,
                0x0007,
                0x000A,
                0x000B,
                0x000E,
                0x000F,
                0x0010,
                0x0011,
                0x0012,
                0x0013,
                0x0015,
                0x0016,
                0x0019,
                0x001A,
                0x001E,
                0x001F,
                0x0021,
                0x0028,
                0x002A,
                0x0032,
                0x0034,
                0x0035,
                0x0036,
                0x003A,
                0x0045,
                0x0051,
                0x0052,
                0x005B,
                0x005C,
                0x005D,
                0x005E,
                0x005F,
                0x0061,
                0x0062,
                0x0063,
                0x0065,
                0x0066,
                0x0067,
                0x0068,
                0x0069,
                0x006B,
                0x006D,
                0x006E,
                0x0091,
                0x0092,
                0x0093,
                0x009B,
                0x009C,
                0x009D,
                0x009E,
                0x00A4,
                0x00A9,
                0x00B3,
                0x00CF,
            });
        }

        /// <summary>
        /// Get a <see cref="MiscConfigStruct"/> representation.
        /// </summary>
        /// <param name="version">Structure version</param>
        /// <returns>Configuration structure</returns>
        public override IAsmConfigStruct ToStruct(uint version)
        {
            var endBuffer = 0x80 - this.CycleRepeatableLocations.Count - this.BaseCycleRepeatableLocations.Count;
            if (endBuffer < 0)
            {
                throw new ArgumentException("cycleRepeatableLocations is too large.");
            }
            var cycleRepeatableLocations = this.BaseCycleRepeatableLocations.Concat(this.CycleRepeatableLocations).Concat(Enumerable.Repeat((ushort)0, endBuffer)).ToArray();
            return new MMRConfigStruct
            {
                Version = version,
                CycleRepeatableLocations = cycleRepeatableLocations,
                CycleRepeatableLocationsLength = (ushort)cycleRepeatableLocations.Length,

                LocationBottleRedPotion = this.LocationBottleRedPotion,
                LocationBottleGoldDust = this.LocationBottleGoldDust,
                LocationBottleMilk = this.LocationBottleMilk,
                LocationBottleChateau = this.LocationBottleChateau,

                LocationSwordKokiri = this.LocationSwordKokiri,
                LocationSwordRazor = this.LocationSwordRazor,
                LocationSwordGilded = this.LocationSwordGilded,

                LocationMagicSmall = this.LocationMagicSmall,
                LocationMagicLarge = this.LocationMagicLarge,

                LocationWalletAdult = this.LocationWalletAdult,
                LocationWalletGiant = this.LocationWalletGiant,

                LocationBombBagSmall = this.LocationBombBagSmall,
                LocationBombBagBig = this.LocationBombBagBig,
                LocationBombBagBiggest = this.LocationBombBagBiggest,

                LocationQuiverSmall = this.LocationQuiverSmall,
                LocationQuiverLarge = this.LocationQuiverLarge,
                LocationQuiverLargest = this.LocationQuiverLargest,
    };
        }
    }
}
