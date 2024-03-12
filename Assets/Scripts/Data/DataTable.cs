using System;
using System.IO;
using System.Text;
using GoblinGames;
using GoblinGames.Collections;
using Protocol.Data;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Deckfense
{
    public abstract class DataTable : ScriptableObject
    {
        [HideInInspector][SerializeField] protected int profile;
        public int SelectedProfileIndex { get { return profile; } set { profile = value; } }

        protected virtual void OnValidate()
        {
            //UnityEditor.EditorUtility.SetDirty(this);
        }

        public abstract void Build();
        public abstract void Load();

    }
    public abstract class DataTable<T> : DataTable where T : Data
    {
        [SerializeField] private string fileName;
        [SerializeField] private SerializableDictionary<string, T> data = new SerializableDictionary<string, T>();
        public new int SelectedProfileIndex { get { return profile; } set { profile = value; } }

        public SerializableDictionary<string, T> Data { get { return data; } }
        public abstract Data OriginalInstance { get; }


        private DataProfile CurrentProfile { get { return DataSettings.DataProfiles[SelectedProfileIndex]; } }
        private string ProjectPath { get { return Path.GetFullPath(Path.Combine(Application.dataPath, @"../")); } }
        private string FullBuildPath { get { return ProjectPath + CurrentProfile.BuildPath + fileName; } }
        private string FullLoadPath { get { return ProjectPath + CurrentProfile.LoadPath + fileName; } }
        private string BuildPath { get { return CurrentProfile.BuildPath + fileName; } }
        private string LoadPath { get { return CurrentProfile.LoadPath + fileName; } }


        protected override void OnValidate()
        {
            base.OnValidate();
        }

        [DebugButton]
        public override void Build()
        {
            string fullBuildPath = FullBuildPath;
            string directoryName = Path.GetDirectoryName(fullBuildPath);
            Type type = OriginalInstance.GetType();

            if (Directory.Exists(directoryName) == false)
            {
                Directory.CreateDirectory(directoryName);
            }

            //var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All  };

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var data in Data)
            {
                string json = JsonConvert.SerializeObject(data.Value);
                stringBuilder.AppendLine(json);
            }

            using (StreamWriter streamWriter = File.CreateText(fullBuildPath))
            {
                streamWriter.WriteLine(stringBuilder.ToString());
            }

            Debug.Log($"Build Success. BuildPath: {fullBuildPath}");
        }

        [DebugButton]
        public override void Load()
        {
            data.Clear();

            Type type = OriginalInstance.GetType();
            bool isUrl = CurrentProfile.LoadPath.Contains(":");

            if (isUrl == false)
            {
                // Local
                string fullLoadPath = FullLoadPath;

                using (StreamReader streamReader = new StreamReader(fullLoadPath))
                {
                    string json;
                    while ((json = streamReader.ReadLine()) != null)
                    {
                        Debug.Log(json);
                        if (string.IsNullOrEmpty(json))
                        {
                            continue;
                        }

                        object obj = JsonConvert.DeserializeObject(json, type);
                        if (obj == null)
                        {
                            Debug.LogError($"DataTable.Load(): Invalid data. [{json}]");
                            continue;
                        }

                        T data = obj as T;
                        if (data == null)
                        {
                            Debug.LogError($"DataTable.Load(): Invalid data. [{json}]");
                            continue;
                        }

                        this.data.Add(data.Id, data);
                    }

                    Debug.Log($"Load Success. LoadPath: {LoadPath}");
                }

            }
            else
            {
                // Remote
                UnityWebRequest request = UnityWebRequest.Get(LoadPath);
                request.SendWebRequest().completed += OnLoadCompleted;
            }


        }

        private void OnLoadCompleted(AsyncOperation operation)
        {
            UnityWebRequestAsyncOperation webRequestOperation = operation as UnityWebRequestAsyncOperation;
            if (webRequestOperation.isDone)
            {
                data.Clear();
                Type type = OriginalInstance.GetType();

                string result = webRequestOperation.webRequest.downloadHandler.text;
                string[] list = result.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < list.Length; i++)
                {
                    string json = list[i];
                    if (string.IsNullOrEmpty(json)) continue;
                    Debug.Log(json);

                    object obj = JsonConvert.DeserializeObject(json, type);
                    if (obj == null)
                    {
                        Debug.LogError($"DataTable.Load(): Invalid data. [{json}]");
                        continue;
                    }

                    T data = obj as T;
                    if (data == null)
                    {
                        Debug.LogError($"DataTable.Load(): Invalid data. [{json}]");
                        continue;
                    }

                    this.data.Add(data.Id, data);
                }

                Debug.Log($"Load Success. LoadPath: {LoadPath}");
            }
        }
    }
}
