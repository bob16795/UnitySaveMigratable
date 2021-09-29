using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using CTL.Core;
using System;

namespace CTL.Migration
{
    public class UpgradeSave
    {
        public int id;
        public double cost;
        public double adds;
        public int level;

        public UpgradeSave() { }
        public UpgradeSave(int id, double cost, uint adds, int level)
        {
            this.id = id;
            this.cost = cost;
            this.adds = adds;
            this.level = level;
        }
    }

    public class SaveV1 : SaveV0
    {
        public new uint version = 1;

        public List<UpgradeSave> upgrades;

        public int pp;

        public SaveV1(SaveV0 prev) : this()
        {
            this.version = 2;
            this.currency = prev.currency;
            this.maxCurrency = prev.maxCurrency;
        }

        public SaveV1() : base()
        {
            version = 1;
            upgrades = new List<UpgradeSave>
            {
                new UpgradeSave(0, 495, 5, 0),
                new UpgradeSave(1, 450, 10, 0),
                new UpgradeSave(2, 450, 30, 0),
                new UpgradeSave(3, 0, 0, 0),
                new UpgradeSave(4, 5, 0, 0),
                new UpgradeSave(5, 20, 0, 0),
                new UpgradeSave(6, 50, 0, 0),
                new UpgradeSave(7, 100, 0, 0),
                new UpgradeSave(8, 1000, 0, 0)
            };
            pp = 0;
        }
        public override ISaveFileVersion Read(BinaryReader reader)
        {
            SaveV1 result = new SaveV1((SaveV0)base.Read(reader));
            result.upgrades = new List<UpgradeSave>();
            int ugCount = reader.ReadInt16();
            for (int i = 0; i < ugCount; i++)
            {
                UpgradeSave upgradeSave = new UpgradeSave();
                upgradeSave.id = reader.ReadInt32();
                upgradeSave.cost = reader.ReadDouble();
                upgradeSave.adds = reader.ReadDouble();
                upgradeSave.level = reader.ReadInt32();
                result.upgrades.Add(upgradeSave);
            }
            result.pp = reader.ReadInt32();
            return result;
        }

        public override void Load()
        {
            base.Load();
            foreach (var upgrade in upgrades)
            {
                Upgrades.instance.Setup(upgrade.id, upgrade.adds, upgrade.cost, upgrade.level);
            }
            Prestige.SetValue(pp);
        }
        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write((System.Int16)upgrades.Count);
            foreach (var upgrade in upgrades)
            {
                writer.Write(upgrade.id);
                writer.Write(upgrade.cost);
                writer.Write(upgrade.adds);
                writer.Write(upgrade.level);
            }
            writer.Write(pp);
        }

        public override void UpdateSave()
        {
            base.UpdateSave();

            upgrades = new List<UpgradeSave>();
            for (int i = 0; i < Upgrades.GetCount(); i++)
            {
                UpgradeSave upgrade = new UpgradeSave();
                upgrade.id = i;
                upgrade.level = Upgrades.GetLevel(i);
                upgrade.cost = Upgrades.GetCost(i);
                upgrade.adds = Upgrades.GetAdds(i);
                upgrades.Add(upgrade);
            }
            pp = Prestige.GetLevel();
        }
    }
}
