using System.Collections;
using System.Collections.Generic;
using GoblinGames;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Deckfense
{
    public class AssetReferenceTest : MonoBehaviour
    {
        [SerializeField] private AssetReference assetReference;


        [DebugButton]
        public void Test()
        {
#if UNITY_EDITOR
            Debug.Log(assetReference.editorAsset.name);
#endif
        }
    }
}
