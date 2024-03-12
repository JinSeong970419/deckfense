using UnityEngine;
using TMPro;

namespace Deckfense
{
    public class PopupNotification : Popup
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI contentText;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        public void ChangeNotificationText(string title, string content)
        {
            titleText.text = title;
            contentText.text = content;
        }
    }
}
