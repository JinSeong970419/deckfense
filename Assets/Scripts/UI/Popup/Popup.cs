using GoblinGames;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Deckfense
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] protected Button _closeButton;
        [SerializeField] protected GameEvent<int> openPopupEvent;
        [SerializeField] protected GameEvent<int> closePopupEvent;
        [SerializeField] protected UnityEvent onOpen;
        [SerializeField] protected UnityEvent onClosed;

        protected virtual void OnEnable()
        {
            _closeButton?.onClick.AddListener(Close);
            onOpen.Invoke();
        }

        protected virtual void OnDisable()
        {
            onClosed.Invoke();
            _closeButton?.onClick.RemoveListener(Close);
        }

        public int Index { get; set; }

        public void Open()
        {
            //openPopupEvent?.Invoke(Index);
            PopupManager.Instance.OpenPopup(Index);
        }
        public void Close()
        {
            //closePopupEvent?.Invoke(Index);
            PopupManager.Instance.ClosePopup(Index);
        }

        public virtual void OnOpen()
        {

        }

        public virtual void OnClosed()
        {

        }
    }
}
