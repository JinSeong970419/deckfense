using GoblinGames.Data;
using Protocol.Data;
using UnityEngine;

namespace Deckfense
{
    [CreateAssetMenu(fileName = "MapTable", menuName = "Goblin Games/Table/MapTable")]
    public class MapTable : DataTable<MapData>
    {
        public override Data OriginalInstance => new MapData();
    }
}
