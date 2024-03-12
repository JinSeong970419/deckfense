using UnityEngine;

namespace Deckfense
{
    [CreateAssetMenu(fileName = "LocalizationTable", menuName = "Goblin Games/Data/LocalizationTable", order = 100)]
    public class LocalizationTable : GoogleSheetTable<LocalizationData>
    {
    }
}
