using Protocol.Data;
using UnityEngine;

namespace Deckfense
{
    [CreateAssetMenu(fileName = "SpawnerTable", menuName = "Goblin Games/Table/SpawnerTable")]
    public class SpawnerTable : DataTable<SpawnerData>
    {
        public override Data OriginalInstance => new SpawnerData();
    }
}
