using System.Collections;
using System.IO;
using System.Collections.Generic;
using CTL.Core;

namespace CTL.Migration
{
    public class TreeSave
    {
        public int id;
        public int level;

        public TreeSave() { }
        public TreeSave(int id, int level)
        {
            this.level = level;
        }
    }

    public class SaveV3 : SaveV2
    {
        public new uint version = 3;

        public List<TreeSave> tree;

        /// <summary>
        /// defaults for a save of V3
        /// </summary>
        public SaveV3() : base()
        {
            this.version = 3;
            this.tree = new List<TreeSave>{
                new TreeSave(0, 0),
                new TreeSave(1, 0),
                new TreeSave(2, 0),
            };
        }

        /// <summary>
        /// creates a save of V3 from V2
        /// </summary>
        public SaveV3(SaveV2 prev) : this()
        {
            this.version = 3;
            // load data
            this.currency = prev.currency;
            this.maxCurrency = prev.maxCurrency;
            this.themeId = prev.themeId;
            this.pp = prev.pp;
            this.upgrades = prev.upgrades;
            this.settings = prev.settings;
            this.dp = prev.dp;
        }

        public override void Load()
        {
            base.Load();

            int id = 0;
            if (tree.Count == 0)
            {
                TreeSystem.instance.SetupTree();
            }
            foreach (TreeSave treeItem in tree)
            {
                TreeSystem.SetLevel(id, treeItem.level);
                id++;
            }
        }

        public override ISaveFileVersion Read(BinaryReader reader)
        {
            SaveV3 result = new SaveV3((SaveV2)base.Read(reader));
            try
            {
                int count = reader.ReadInt32();
                result.tree = new List<TreeSave>();

                for (int i = 0; i < count; i++)
                {
                    TreeSave item = new TreeSave();
                    item.level = reader.ReadInt32();
                    item.id = i;
                    result.tree.Add(item);
                }
            }
            catch (IOException) { };

            return result;
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);

            writer.Write(tree.Count);
            foreach (TreeSave treeItem in tree)
            {
                writer.Write(treeItem.level);
            }
        }

        public override void UpdateSave()
        {
            base.UpdateSave();
            tree = new List<TreeSave>();
            for (int i = 0; i < TreeSystem.GetCount(); i++)
            {
                tree.Add(new TreeSave(i, TreeSystem.GetLevel(i)));
            }
        }
    }
}
