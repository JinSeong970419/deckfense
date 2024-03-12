using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using GoblinGames;
using GoblinGames.Data;
using UnityEditor;
using UnityEngine;

namespace Deckfense.Editor
{
    [CustomEditor(typeof(DataTable), true)]
    public class DataTableInspector : UnityEditor.Editor
    {
        [SerializeField] private List<MethodInfo> methodInfos;
        private DataTable selected;

        private void OnValidate()
        {
            //Debug.Log("OnValidate");
            CreateDebugButton();
        }

        private void OnEnable()
        {
            //Debug.Log("OnEnable");
            CreateDebugButton(); 
            selected = (DataTable)target;
        }

        public override void OnInspectorGUI()
        {
            selected = (DataTable)target;
            var array = DataSettings.DataProfiles.Select(profile => profile.Name).ToArray();
            int old = selected.SelectedProfileIndex;
            int newIndex = EditorGUILayout.Popup("Profile", selected.SelectedProfileIndex, array);
            selected.SelectedProfileIndex = newIndex;
            if (old != newIndex)
            {
                Debug.Log($"{old} {newIndex}");
                UnityEditor.EditorUtility.SetDirty(selected);
            }

            base.OnInspectorGUI();

            if (Application.isBatchMode) return;

            int methodCount = methodInfos.Count;
            for (int i = 0; i < methodCount; i++)
            {
                var method = methodInfos[i];
                GUILayout.Space(10);
                if (GUILayout.Button(method.Name.Spacing()))
                {
                    method.Invoke(target, null);
                }
            }
        }

        private void CreateDebugButton()
        {
            methodInfos = new List<MethodInfo>();

            Type type = target.GetType();
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            int methodCount = methods.Length;

            for (int i = 0; i < methodCount; i++)
            {
                var method = methods[i];
                var attributes = method.GetCustomAttributes(typeof(DebugButton), false);
                int attributeCount = attributes.Length;
                for (int j = 0; j < attributeCount; j++)
                {
                    var attribute = attributes[j] as DebugButton;
                    if (attribute == null) continue;

                    methodInfos.Add(method);
                }
            }
        }
    }
}
