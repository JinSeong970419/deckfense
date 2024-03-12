using GoblinGames;
using TMPro;
using UnityEngine;

namespace Deckfense
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private LocalizationTable referenceTable;
        [SerializeField] private Variable<Language> referenceLanguage;
        [SerializeField] private string stringFormat;

        private void OnValidate()
        {
            if (text == null) text = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            referenceLanguage.OnValueChanged.AddListener(OnLanguageChanged);
            Refresh();
        }

        private void OnDisable()
        {
            referenceLanguage.OnValueChanged.RemoveListener(OnLanguageChanged);
        }

        public void Refresh()
        {
            if (referenceTable.Data.TryGetValue(stringFormat, out LocalizationData data))
            {
                text.text = data.Localize(Localization.Instance.CurrentLanguage);
            }
            else
            {
                Debug.LogWarning("Invalid Localization key.");
            }
        }

        private void OnLanguageChanged(Language language)
        {
            Debug.Log("OnLanguageChanged");
            Refresh();
        }
    }
}
