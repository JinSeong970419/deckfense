using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using GoblinGames;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Deckfense
{
    [CreateAssetMenu(fileName = "AssetSettings", menuName = "Goblin Games/Settings/AssetSettings")]
    public class AssetSettings : ScriptableObject
    {
        [SerializeField] private string uploaderPath;

		[SerializeField] private List<AssetReference> assetReferences = new List<AssetReference>();

		public List<AssetReference> AssetReferences { get { return assetReferences; } }

		private static string _uploaderPath;
        public static string UploaderPath { get { return _uploaderPath; } }

        private void OnValidate()
        {
            _uploaderPath = uploaderPath;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }


		[DebugButton]
		public void GenerateEnum()
		{
#if UNITY_EDITOR
			string enumName = "AssetKind";
			string fileName = enumName + ".cs";
			string path = "Assets/Scripts/Asset/";
			string fullPath = Path.Combine(path, fileName);
			var list = assetReferences.Select(x => x.editorAsset.name).ToList();
			Extension.GenerateEnumWithEnd(fullPath, enumName, "Deckfense", list);
#endif
		}
	}
}
