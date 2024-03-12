using System.Collections.Generic;
using System.Linq;
using GoblinGames;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Compilation;
#endif

namespace Deckfense
{
    public class PopupContainer : MonoBehaviour
    {
        [SerializeField] private List<Popup> popupList;

        public List<Popup> PopupList { get { return popupList; } }

        [DebugButton]
        public void Generate()
        {
            var items = popupList.Select(o => o.name).ToList();
            Extension.GenerateEnum("Assets/Scripts/UI/Popup/PopupKind.cs", "PopupKind", "Deckfense", items);

#if UNITY_EDITOR
			CompilationPipeline.RequestScriptCompilation();
#endif
        }
    }
}
