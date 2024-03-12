using GoblinGames;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Deckfense
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("String Key")]
        [SerializeField] private string header_stringFormat;
        [SerializeField] private string content_stringFormat;

        [Header("Localization Tooltip")]
        [SerializeField] private LocalizationTable referenceTable;
        [SerializeField] private Variable<Language> referenceLanguage;

        [Header("Tooltip Setting")]
        [SerializeField] private TooltipPosition tooltipPosition;
        [SerializeField] private float padding;
        [SerializeField] private float openDelay = 1f;

        [Header("Tooltip Events")]
        [SerializeField] private GameEventTooltipShow gameEventTooltipShow;
        [SerializeField] private GameEventVoid gameEventTooltipHide;

        private RectTransform rectTransform;
        private Canvas canvas;
        private TooltipData data;
        private string header;
        private string content;
        private bool openTrigger;

        private void OnValidate()
        {
            rectTransform = transform.GetComponent<RectTransform>();
            canvas = transform.root.GetComponent<Canvas>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("OnPounterEnter Event");
            openTrigger = true;

            GetLocalizationString();
            SetTooltipData();

            StartCoroutine(Delay());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("OnPounterEnter Exit");
            openTrigger = false;

            gameEventTooltipHide.Invoke();
        }

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(openDelay);
            if (openTrigger)
            {
                Debug.Log("OnPounterEnter Action Start");
                gameEventTooltipShow.Invoke(data);
            }
        }

        private void GetLocalizationString()
        {
            if (referenceTable.Data.TryGetValue(header_stringFormat, out LocalizationData headerData) )
            {
                header = headerData.Localize(Localization.Instance.CurrentLanguage);
            }
            else
            {
                header = string.Empty;
                Debug.LogWarning("Invalid Localization header key.");
            }

            if (referenceTable.Data.TryGetValue(content_stringFormat, out LocalizationData contentData))
            {
                content = contentData.Localize(Localization.Instance.CurrentLanguage);
            }
            else
            {
                content = string.Empty;
                Debug.LogWarning("Invalid Localization content key.");
            }
        }

        private void SetTooltipData()
        {
            data = new TooltipData();
            //Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, gameObject.transform.position);
            Vector2 result = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, gameObject.transform.position);
            //Vector3 result = Vector3.zero;
            //RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPos, canvas.worldCamera, out result);

            switch (tooltipPosition)
            {
                case TooltipPosition.left:
                    result.x = result.x - padding - rectTransform.rect.width * 0.5f;
                    data.Pivot = new Vector2(1f, 0.5f);
                    break;
                case TooltipPosition.right:
                    result.x = result.x + padding + rectTransform.rect.width * 0.5f;
                    data.Pivot = new Vector2(0f, 0.5f);
                    break;
                case TooltipPosition.top:
                    result.y = result.y + padding + rectTransform.rect.height * 0.5f;
                    data.Pivot = new Vector2(0.5f, 0f);
                    break;
                case TooltipPosition.bottom:
                    result.y = result.y - padding - rectTransform.rect.height * 0.5f;
                    data.Pivot = new Vector2(0.5f, 1f);
                    break;
            }

            data.Position = result;
            data.TooltipPosition = tooltipPosition;
            data.Header = header;
            data.Content = content;
        }
    }
}