using System.Collections.Generic;
using GoblinGames;
using UnityEngine;

namespace Deckfense
{
    [CreateAssetMenu(fileName = "GoogleSheet", menuName = "Goblin Games/Data/GoogleSheet", order = 100)]
    public class GoogleSheet : ScriptableObject
    {
        [SerializeField] private string sheetId;
        [SerializeField] private List<GoogleSheetTable> tables;

        public string SheetId { get { return sheetId; } }
        public List<GoogleSheetTable> Tables { get { return tables; } }

        private void OnValidate()
        {
            SetSheet();
        }

        private void SetSheet()
        {
            if (tables == null) return;

            int count = tables.Count;
            for (int i = 0; i < count; i++)
            {
                GoogleSheetTable table = tables[i];
                table.Sheet = this;
            }
        }

        [DebugButton]
        public void Load()
        {
            if (tables == null) return;

            int count = tables.Count;
            for (int i = 0; i < count; i++)
            {
                GoogleSheetTable table = tables[i];
                table.Load();
            }
        }

    }
}
