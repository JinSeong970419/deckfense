#if UNITY_EDITOR
using GoblinGames;
using UnityEditor;
using UnityEngine;

namespace Deckfense.Editor
{
    public class DataBuildLauncher
    {

        [MenuItem("Goblin Games/Data/Upload to remote storage")]
        public static void UploadDataToRemote()
        {
            Debug.Log($"{DataSettings.UploaderPath}");
            BatchFileExecutor.Execute(DataSettings.UploaderPath);
        }
    }
}
#endif