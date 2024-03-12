using GoblinGames;
using UnityEngine;

namespace Deckfense
{
    public class UIScaler : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private GameEvent<Vector2> resolutionChangedEvent;
        private Canvas canvas;

        private Vector2 oldScreenSize;
        public Vector2 OldScreenSize { get { return oldScreenSize; } }

        private void Start()
        {
            Debug.Log($"{rectTransform.rect}");
            oldScreenSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
            resolutionChangedEvent.Invoke(oldScreenSize);
        }

        private void Update()
        {
            if (CheckEpsilon(oldScreenSize.x, rectTransform.rect.width) || CheckEpsilon(oldScreenSize.y, rectTransform.rect.height))
            {
                oldScreenSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
                resolutionChangedEvent.Invoke(oldScreenSize);
            }
        }

        private void OnValidate()
        {
            if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        }

        private bool CheckEpsilon(float a, float b)
        {
            float value = Mathf.Abs(a - b);
            if (value > float.Epsilon)
            {
                return true;
            }

            return false;
        }
    }
}
