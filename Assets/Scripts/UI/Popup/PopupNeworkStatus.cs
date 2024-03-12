using System.Text;
using GoblinGames;
using TMPro;
using UnityEngine;

namespace Deckfense
{
    public class PopupNeworkStatus : Popup
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Variable<long> roundTripTime;
        [SerializeField] private Variable<long> delayFromServerToClient;
        [SerializeField] private Variable<long> delayFromClientToServer;


        protected override void OnEnable()
        {
            base.OnEnable();

            roundTripTime.OnValueChanged.AddListener(OnRoundTripTimeChanged);
            delayFromServerToClient.OnValueChanged.AddListener(OnServerDelayChanged);
            delayFromClientToServer.OnValueChanged.AddListener(OnClientDelayChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            roundTripTime.OnValueChanged.RemoveListener(OnRoundTripTimeChanged);
            delayFromServerToClient.OnValueChanged.RemoveListener(OnServerDelayChanged);
            delayFromClientToServer.OnValueChanged.RemoveListener(OnClientDelayChanged);
        }

        private void Start()
        {
            UpdateText();
        }

        private void OnRoundTripTimeChanged(long roundTripTime)
        {
            UpdateText();
        }

        private void OnServerDelayChanged(long serverDelay)
        {
            UpdateText();
        }

        private void OnClientDelayChanged(long clientDelay)
        {
            UpdateText();
        }

        private void UpdateText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"RTT: {roundTripTime.Value}ms");
            sb.AppendLine($"ServerDelay: {delayFromServerToClient.Value}ms");
            sb.AppendLine($"ClientDelay: {delayFromClientToServer.Value}ms");

            text.text = sb.ToString();
        }
    }
}
