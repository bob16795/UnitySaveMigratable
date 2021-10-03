using System.Collections;
using System.IO;
using System.Collections.Generic;
using CTL.Core;
using System;
using CTL.Input;

namespace CTL.Migration
{
    public class SaveV2 : SaveV1
    {
        public new uint version = 2;
        public List<bool> settings;
        public int dp;
        public SaveV2() : base()
        {
            version = 2;
            settings = new List<bool>();
            dp = 0;
        }

        public SaveV2(SaveV1 prev) : this()
        {
            this.version = 2;
            this.currency = prev.currency;
            this.maxCurrency = prev.maxCurrency;
            this.themeId = prev.themeId;
            this.pp = prev.pp;
            this.upgrades = prev.upgrades;
        }

        public override ISaveFileVersion Read(BinaryReader reader)
        {
            SaveV2 result = new SaveV2((SaveV1)base.Read(reader));

            int setitngsCount = reader.ReadInt16();

            result.settings = new List<bool>();

            for (int id = 0; id < setitngsCount; id++)
            {
                bool setting = reader.ReadBoolean();
                result.settings.Add(setting);
            }

            result.dp = reader.ReadInt32();

            return result;
        }

        public override void Load()
        {
            base.Load();

            int id = 0;
            foreach (bool b in settings)
            {
                Settings.instance.SetSetting(id, b);
                id++;
            }
            Descension.SetLevel(dp);
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write((System.UInt16)settings.Count);
            foreach (bool b in settings)
            {
                writer.Write(b);
            }
            writer.Write(dp);
        }

        public override void UpdateSave()
        {
            base.UpdateSave();
            settings = Settings.instance.GetSettings();
            dp = Descension.GetLevel();
        }
    }
}
