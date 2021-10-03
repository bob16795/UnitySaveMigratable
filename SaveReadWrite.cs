using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using CTL.Migration;

namespace CTL.Saves
{
    public class SaveReadWrite : MonoBehaviour
    {
        bool first_load = false;
        static string SAVE_PATH;

        private SaveV3 save;

        /// <summary>
        /// read the save
        /// </summary>
        public void Read()
        {
            if (File.Exists(SAVE_PATH))
            {
                using FileStream fs = File.OpenRead(SAVE_PATH);

                BinaryReader reader = new System.IO.BinaryReader(fs);

                ISaveFileVersion tempSave;

                uint version = reader.ReadUInt32();

                switch (version)
                {
                    case 0:
                        tempSave = new SaveV0().Read(reader);
                        break;
                    case 1:
                        tempSave = new SaveV1().Read(reader);
                        break;
                    case 2:
                        tempSave = new SaveV2().Read(reader);
                        break;
                    case 3:
                        tempSave = new SaveV3().Read(reader);
                        break;
                    default:
                        tempSave = new SaveV3();
                        break;
                }
                tempSave.version = version;
                save = (SaveV3)MigrationManager.Migrate(tempSave);
            }
            else
            {
                save = new SaveV3();
                Write();
            }
        }

        /// <summary>
        /// write the save
        /// </summary>
        /// <param name="end">end data was for verification never finished</param>
        /// <returns>the file stream</returns>
        public BinaryWriter Write(byte[] end = null)
        {
            using (FileStream fw = File.OpenWrite(SAVE_PATH))
            {
                BinaryWriter writer = new BinaryWriter(fw);
                writer.Write(save.version);
                save.Write(writer);
                return writer;
            }
        }

        /// <summary>
        /// load the save data to the game
        /// </summary>
        public void Load()
        {
            save.Load();
        }

        /// <summary>
        /// updates the save according to game data
        /// </summary>
        public void UpdateSave()
        {
            save.UpdateSave();
        }

        /// <summary>
        /// load and read the save
        /// </summary>
        void Start()
        {
            SAVE_PATH = Application.persistentDataPath + "/save.bin";
            Read();
            Load();
        }

        /// <summary>
        /// save the game data in case the game is killed
        /// </summary>
        void OnApplicationPause()
        {
            if (first_load == false)
            {
                first_load = true;
                return;
            }
            UpdateSave();
            BinaryWriter wr = Write();
        }


        /// <summary>
        /// save the game data bc the game was killed
        /// </summary>
        void OnApplicationQuit()
        {
            UpdateSave();
            BinaryWriter wr = Write();
        }
    }
}
