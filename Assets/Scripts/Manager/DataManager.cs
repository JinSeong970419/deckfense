using GoblinGames;
using GoblinGames.DesignPattern;
using Protocol.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Deckfense
{
    public class DataManager : MonoSingleton<DataManager>
    {

        [SerializeField] private DataSettings settings;
        [SerializeField] private List<DataTable> tables = new List<DataTable>();
        [SerializeField] private SpawnerData spawnerData;

        private UserInfo userInfo = new UserInfo();

        public List<DataTable> Tables { get { return tables; } }
        public MonsterTable MonsterTable { get { return Tables[(int)DataTableKind.MonsterTable] as MonsterTable; } }
        public SpellTable SpellTable { get { return Tables[(int)DataTableKind.SpellTable] as SpellTable; } }
        public MapTable MapTable { get { return Tables[(int)DataTableKind.MapTable] as MapTable; } }
        public SpawnerTable SpawnerTable { get { return Tables[(int)DataTableKind.SpawnerTable] as SpawnerTable; } }

        public SpawnerData SpawnerData { get { return spawnerData; } }
        public UserInfo UserInfo { get { return userInfo; } }


        [DebugButton]
        public void GenerateDataTableKind()
        {
#if UNITY_EDITOR
            var array = Tables.Select(o => o.name);
            Extension.GenerateEnum("Assets/Scripts/Data/DataTableKind.cs", "DataTableKind", "Deckfense", array);
#endif
        }

        [DebugButton]
        public void Build()
        {
            int count = tables.Count;
            for (int i = 0; i < count; i++)
            {
                DataTable table = tables[i];
                table.Build();
            }
        }

        [DebugButton]
        public void Load()
        {
            int count = tables.Count;
            for (int i = 0; i < count; i++)
            {
                DataTable table = tables[i];
                table.Load();
            }
        }
    }
}
