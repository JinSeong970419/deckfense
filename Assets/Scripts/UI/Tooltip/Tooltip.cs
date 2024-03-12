using GoblinGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Deckfense
{
    [ExecuteInEditMode]
    public class Tooltip : MonoBehaviour
    {
        [Header("Default Tooltip")]
        [SerializeField] private GameObject defaultTooltip;
        [SerializeField] private TextMeshProUGUI headerField;
        [SerializeField] private TextMeshProUGUI contentField;
        [SerializeField] private LayoutElement layoutElement;
        [SerializeField] private int characterWrapLimit;
        [SerializeField] private RectTransform rectTransform;

        [Header("Tooltip Events")]
        [SerializeField] private GameEventTooltipShow gameEventTooltipShow;
        [SerializeField] private GameEventVoid gameEventTooltipHide;

        private void OnEnable()
        {
            gameEventTooltipShow.AddListener(Show);
            gameEventTooltipHide.AddListener(Hide);
        }

        private void OnDisable()
        {
            gameEventTooltipShow.RemoveListener(Show);
            gameEventTooltipHide.RemoveListener(Hide);
        }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Show(TooltipData data)
        {
            defaultTooltip.SetActive(true);
            SetText(data);
        }

        private void Hide()
        {
            defaultTooltip.SetActive(false);
        }

        public void SetText(TooltipData data)
        {
            headerField.text = data.Header;
            headerField.gameObject.SetActive(!string.IsNullOrEmpty(data.Header));
            contentField.text = data.Content;

            int headerLength = headerField.text.Length;
            int contentLength = contentField.text.Length;
            layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;

            defaultTooltip.transform.position = data.Position;
            defaultTooltip.GetComponent<RectTransform>().pivot = data.Pivot;
            //defaultTooltip.transform.position = new Vector3(data.Position.x - (defaultTooltip.GetComponent<RectTransform>().rect.width * 0.5f), data.Position.y, data.Position.z);
        }
    }
}
