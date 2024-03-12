using GoblinGames;
using UnityEngine;

namespace Deckfense
{
    public class Panel : MonoBehaviour
    {
        [SerializeField] private GameEvent<Vector2> resolutionChangedEvent;

        [SerializeField] private RectTransform rectTransform;

        private Vector2 oldResolution;

        private void Awake()
        {
            //처음에 1920f, 1080f 기준으로 잡아야 카드스케일 기준이 잘 잡힘
            //oldResolution = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
            oldResolution = new Vector2(1920f, 1080f);
            //Debug.Log($"old {rectTransform.rect}");
        }

        private void OnEnable()
        {
            resolutionChangedEvent.AddListener(OnResolutionChanged);
        }


        private void OnDisable()
        {
            resolutionChangedEvent.RemoveListener(OnResolutionChanged);
        }

        private void OnValidate()
        {
            if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        }


        private void OnResolutionChanged(Vector2 resolution)
        {
            float ratioX = resolution.x / oldResolution.x;
            float ratioY = resolution.y / oldResolution.y;

            rectTransform.localScale = new Vector2(ratioX, ratioY);
        }
    }
}
