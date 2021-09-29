using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CTL.Migration
{
    public static class MigrationManager
    {
        public static ISaveFileVersion Migrate(ISaveFileVersion save)
        {
            if (2 == save.version) return save;
            else
            {
                ISaveFileVersion result = (ISaveFileVersion)new SaveV2();
                if (save.version == 0) result = (ISaveFileVersion)new SaveV1((SaveV0)save);
                else if (save.version == 1) result = (ISaveFileVersion)new SaveV2((SaveV1)save);
                result.version = save.version + 1;
                return Migrate(result);
            }
        }
    }
}