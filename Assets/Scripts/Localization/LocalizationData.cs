using UnityEngine;

namespace Deckfense
{
    public enum Language
    {
        English,
        Korean,
        Japanese,
        ChineseSimplified,
        ChineseTraditional,
    }

    [System.Serializable]
    public class LocalizationData : GoogleSheetRow
    {
        public string English;
        public string Korean;
        public string Japanese;
        public string ChineseSimplified;
        public string ChineseTraditional;

        public string Localize(Language language)
        {
            switch (language)
            {
                case Language.English: return English;
                case Language.Korean: return Korean;
                case Language.Japanese: return Japanese;
                case Language.ChineseSimplified: return ChineseSimplified;
                case Language.ChineseTraditional: return ChineseTraditional;
                default:
                    Debug.LogError("Language not supported.");
                    break;
            }

            return string.Empty;
        }
    }
}
