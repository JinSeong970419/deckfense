using GoblinGames;
using GoblinGames.DesignPattern;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Deckfense
{
    public class Localization : MonoSingleton<Localization>
    {
        [SerializeField] private GoogleSheet sheet;

        [SerializeField] private Variable<Language> currentLanguage;

        public GoogleSheet Sheet { get { return sheet; } }
        public Language CurrentLanguage { get { return currentLanguage.Value; } }

        protected override void Awake()
        {
            base.Awake();

            sheet.Load();

            OnSelectedLocaleChanged(LocalizationSettings.SelectedLocale);
        }

        private void OnEnable()
        {
            LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;
        }


        private void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;
        }

        private void OnSelectedLocaleChanged(UnityEngine.Localization.Locale obj)
        {
            int index = LocalizationSettings.AvailableLocales.Locales.IndexOf(obj);
            currentLanguage.Value = (Language)index;
            Debug.Log($"Current Language: {currentLanguage.Value}");
        }
    }
}
