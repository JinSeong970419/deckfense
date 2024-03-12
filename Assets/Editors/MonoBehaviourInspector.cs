using System;
using System.Collections.Generic;
using System.Reflection;
using GoblinGames;
using UnityEditor;
using UnityEngine;

namespace Deckfense.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MonoBehaviourInspector : UnityEditor.Editor
    {
        [SerializeField] private List<MethodInfo> methodInfos;

        private void OnEnable()
        {
            CreateDebugButton();
        }

        public override void OnInspectorGUI()
        {
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
            var methods = type.GetMethods();
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
