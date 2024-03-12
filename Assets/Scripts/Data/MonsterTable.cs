using Protocol.Data;
using UnityEngine;

namespace Deckfense
{
    [CreateAssetMenu(fileName = "MonsterTable", menuName = "Goblin Games/Table/MonsterTable")]
    public class MonsterTable : DataTable<MonsterDataEx>
    {
        public override Data OriginalInstance => new MonsterDataEx();
    }
}
