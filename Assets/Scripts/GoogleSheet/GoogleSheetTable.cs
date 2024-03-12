using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using GoblinGames;
using GoblinGames.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Deckfense
{
    public abstract class GoogleSheetTable : ScriptableObject
    {
        [SerializeField] private GoogleSheet sheet;

        public GoogleSheet Sheet { get { return sheet; } set { sheet = value; } }

        public abstract void Load();
    }
    public abstract class GoogleSheetTable<T> : GoogleSheetTable where T : GoogleSheetRow
    {
        private const string spreadSheetsURL = "https://docs.google.com/spreadsheets/d/";

        [SerializeField] private string gid;
        [SerializeField] private SerializableDictionary<string, T> data;

        public string Gid { get { return gid; } }
        public string URL
        {
            get
            {
                return $"{spreadSheetsURL}{Sheet.SheetId}/export?format=tsv&gid={Gid}";
            }
        }

        public SerializableDictionary<string, T> Data { get { return data; } }

        [DebugButton]
        public override void Load()
        {
            Debug.Log(URL);
            UnityWebRequest request = UnityWebRequest.Get(URL);
            request.SendWebRequest().completed += OnLoadCompleted;
        }

        private void OnLoadCompleted(AsyncOperation operation)
        {
            UnityWebRequestAsyncOperation webRequestOperation = operation as UnityWebRequestAsyncOperation;
            if (webRequestOperation.isDone)
            {
                Parse(webRequestOperation.webRequest.downloadHandler.text);
            }
        }

        private void Parse(string text)
        {
            if (data == null)
            {
                data = new SerializableDictionary<string, T>();
            }

            data.Clear();

            Type type = typeof(T);

            List<FieldInfo> fieldInfos = new List<FieldInfo>();

            string[] rows = text.Split("\r\n");
            string[] columns = rows[0].Split('\t');
            int rowCount = rows.Length;
            int colCount = columns.Length;

            for (int col = 0; col < colCount; col++)
            {
                string column = columns[col];
                //Debug.Log(column);
                // Set Column
                FieldInfo fieldInfo = type.GetField(column, BindingFlags.Public | BindingFlags.Instance);
                fieldInfos.Add(fieldInfo);
            }

            for (int row = 1; row < rowCount; row++)
            {
                string line = rows[row];
                string[] cols = line.Split('\t');
                object instance = Activator.CreateInstance(type);

                for (int col = 0; col < colCount; col++)
                {
                    string element = cols[col];

                    Type fieldType = fieldInfos[col].ReflectedType;
                    
                    if(fieldType.IsEnum)
                    {
                        object enumValue = Enum.Parse(fieldType, element);
                        fieldInfos[col].SetValue(instance, enumValue);
                        continue;
                    }

                    fieldInfos[col].SetValue(instance, element);
                }

                data.Add(cols[0], instance);
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
