using UnityEngine;

namespace Assets.Scripts.Savefile
{
    public static class Directories
    {
        public const string SAVEFILE_FOLDER_NAME = "Saved Circuits";
        public static string SAVEFILE_FOLDER_FULL_PATH = Application.persistentDataPath + "/" + SAVEFILE_FOLDER_NAME;
    }
}
