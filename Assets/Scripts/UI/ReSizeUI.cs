using GoblinGames;
using UnityEngine;

namespace Deckfense
{
    public class ReSizeUI : MonoBehaviour
    {
        private GameObject panel;
        [SerializeField] private GameEvent<Vector2> resolutionChangedEvent;

        void Awake()
        {
            panel = transform.parent.gameObject;
        }

        private void OnEnable()
        {
            resolutionChangedEvent.AddListener(OnResolutionChanged);
        }

        private void OnDisable()
        {
            resolutionChangedEvent.RemoveListener(OnResolutionChanged);
        }

        private void OnResolutionChanged(Vector2 resolution)
        {
            float resolutionRatioX = panel.transform.localScale.y / panel.transform.localScale.x;
            transform.localScale = new Vector3(resolutionRatioX, 1f, 1f);
        }
    }
}
