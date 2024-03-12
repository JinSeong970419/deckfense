using Protocol.Data;
using UnityEngine;

namespace Deckfense
{
    [CreateAssetMenu(fileName = "SpellTable", menuName = "Goblin Games/Table/SpellTable")]
    public class SpellTable : DataTable<SpellData>
    {
        public override Data OriginalInstance => new SpellData();
    }
}
