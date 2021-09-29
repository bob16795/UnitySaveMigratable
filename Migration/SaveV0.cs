using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using CTL.Core;
using CTL.Input;

namespace CTL.Migration
{
    public class SaveV0 : ISaveFileVersion
    {
        public new uint version = 0;
        public double currency;
        public int maxCurrency;
        public int themeId;

        public SaveV0()
        {
            currency = 500;
            maxCurrency = 500;
            themeId = 0;
        }
        public virtual ISaveFileVersion Read(BinaryReader reader)
        {
            return new SaveV0
            {
                currency = reader.ReadDouble(),
                maxCurrency = reader.ReadInt32(),
                themeId = reader.ReadInt32()
            };
        }
        public virtual void Write(BinaryWriter writer)
        {
            writer.Write(currency);
            writer.Write(maxCurrency);
            writer.Write(themeId);
        }

        public virtual void Load()
        {
            Currency.instance.SetCurrency(currency, maxCurrency);
            Settings.instance.SetTheme(themeId);
        }

        public virtual void UpdateSave()
        {
            currency = Currency.GetCurrency();
            maxCurrency = Currency.GetMaxCurrency();
            themeId = Settings.instance.GetTheme();
        }
    }
}
