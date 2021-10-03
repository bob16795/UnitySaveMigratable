using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CTL.Migration
{
    public static class MigrationManager
    {
        /// <summary>
        /// migrates a save to the latest version
        /// </summary>
        /// <param name="save">a save file of any version</param>
        /// <returns>a save file of the latest version</returns>
        public static ISaveFileVersion Migrate(ISaveFileVersion save)
        {
            // the save is the latest version
            if (3 == save.version) return save;

            // convert the save to the next version
            ISaveFileVersion result = (ISaveFileVersion)new SaveV2();
            if (save.version == 0) result = (ISaveFileVersion)new SaveV1((SaveV0)save);
            else if (save.version == 1) result = (ISaveFileVersion)new SaveV2((SaveV1)save);
            else if (save.version == 2) result = (ISaveFileVersion)new SaveV3((SaveV2)save);
            result.version = save.version + 1;

            // migrate the save incase you were atleast 2 versions behind
            return Migrate(result);
        }
    }
}