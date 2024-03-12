using GoblinGames.DesignPattern;
using UnityEngine;

namespace Deckfense
{
    public class AssetManager : MonoSingleton<AssetManager>
    {
        [SerializeField] private AssetSettings settings;

        public AssetSettings Settings { get { return settings; } }
    }
}
