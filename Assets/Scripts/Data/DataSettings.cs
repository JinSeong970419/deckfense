using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using GoblinGames;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Deckfense
{
    [CreateAssetMenu(fileName = "DataSettings", menuName = "Goblin Games/Settings/DataSettings")]
    public class DataSettings : ScriptableObject
    {
        [Header("[Remote Uploader]")]
        [SerializeField] private string uploaderPath;
        [Header("[Profile Settings]")]
        [ArrayElementTitle("name")]
        [SerializeField] private List<DataProfile> dataProfiles= new List<DataProfile>();

        private static string _uploaderPath;
        public static string UploaderPath { get { return _uploaderPath; } }

        private static List<DataProfile> _dataProfiles;
        public static IReadOnlyList<DataProfile> DataProfiles { get { return _dataProfiles; } }

        private void OnValidate()
        {
            Debug.Log("DataSettings.OnValidate");
            _uploaderPath = uploaderPath;
            _dataProfiles = dataProfiles;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        [DebugButton]
        public void SetProfile()
        {
#if UNITY_EDITOR
            var array = dataProfiles.Select(o => o.Name);
            Extension.GenerateEnum("Assets/Scripts/Data/DataProfileKind.cs", "DataProfileKind", "Deckfense", array);
#endif
        }
    }

    [System.Serializable]
    public class DataProfile
    {
        [SerializeField] private string name;
        [SerializeField] private string buildPath;
        [SerializeField] private string loadPath;

        public string Name { get { return name; } }
        public string BuildPath { get { return buildPath; } }
        public string LoadPath { get { return loadPath; } }
    }
}
