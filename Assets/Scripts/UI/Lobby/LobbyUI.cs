using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Deckfense
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private SelectGameTypeUI selectGameTypeUI;

        #region Property

        public SelectGameTypeUI SelectGameTypeUI { get { return selectGameTypeUI; } }

        public HorizontalScrollSnap scrollSnap { get; private set; }

        #endregion

        private void Awake()
        {
            scrollSnap = GetComponentInChildren<HorizontalScrollSnap>();
        }

        public void ChangeScreen(LobbyScreenType screenType)
        {
            scrollSnap.GoToScreen((int)screenType);
        }

    }
}
