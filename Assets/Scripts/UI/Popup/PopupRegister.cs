using GoblinGames;
using Protocol.Network;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Deckfense
{
    public class PopupRegister : Popup
    {
        [SerializeField] private GameEvent<object> requestRegisterAccountEvent;
        [SerializeField] private GameEvent<object> responseRegisterAccountEvent;
        [SerializeField] private TMP_InputField inputFieldID;
        [SerializeField] private TMP_InputField inputFieldPW;
        [SerializeField] private TMP_InputField inputFieldConfirmPW;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private Button registerButton;

        [SerializeField] private Image successPanel;
        [SerializeField] private Button okButton;

        [SerializeField] private Image loadingPanel;
        [SerializeField] private Image loadingRing;

        private bool waitForResponse = false;

        protected override void OnEnable()
        {
            base.OnEnable();

            waitForResponse = false;
            loadingPanel.gameObject.SetActive(false);
            successPanel.gameObject.SetActive(false);

            registerButton.onClick.AddListener(OnRegisterButtonClick);
            responseRegisterAccountEvent.AddListener(OnResponseRegisterAccount);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            registerButton.onClick.RemoveListener(OnRegisterButtonClick);
            responseRegisterAccountEvent.RemoveListener(OnResponseRegisterAccount);
        }

        private void Update()
        {
            if (waitForResponse)
            {
                if (loadingPanel.gameObject.activeSelf == false)
                {
                    loadingPanel.gameObject.SetActive(true);
                }
                loadingRing.fillAmount = Time.unscaledTime % 1f;
            }
        }

        private void OnRegisterButtonClick()
        {
            string id = inputFieldID.text;
            string pw = inputFieldPW.text;
            Regex idRegex = new Regex(@"^[0-9a-zA-Z]{6,20}$");
            Regex pwRegex = new Regex(@"^[0-9a-zA-Z!@#$%^&*]{8,20}$");

            if (string.IsNullOrEmpty(id))
            {
                resultText.color = Color.red;
                resultText.text = "아이디를 입력해주세요.";
                return;
            }

            if (string.IsNullOrEmpty(pw))
            {
                resultText.color = Color.red;
                resultText.text = "비밀번호를 입력해주세요.";
                return;
            }

            if (idRegex.IsMatch(id) == false)
            {
                resultText.color = Color.red;
                resultText.text = "아이디 규칙: 숫자, 영어(대,소문자), 6~20자";
                return;
            }

            if (pwRegex.IsMatch(pw) == false)
            {
                resultText.color = Color.red;
                resultText.text = "비밀번호 규칙: 숫자, 영어(대,소문자), 특수문자(!@#$%^&*) 8~20자";
                return;
            }

            waitForResponse = true;
            requestRegisterAccountEvent.Invoke(new RequestRegisterAccount()
            {
                Type = MessageType.RequestRegisterAccount,
                Id = id,
                Password = pw,
            });
        }

        private void OnResponseRegisterAccount(object message)
        {
            waitForResponse = false;
            loadingPanel.gameObject.SetActive(false);

            ResponseRegisterAccount msg = message as ResponseRegisterAccount;
            if (msg.Result == RegisterAccountResult.Success)
            {
                OnRegisterAccountSuccess();
            }
            else
            {
                OnRegisterAccountFailed(msg.Result);
            }

        }

        private void OnRegisterAccountSuccess()
        {
            successPanel.gameObject.SetActive(true);
        }

        private void OnRegisterAccountFailed(RegisterAccountResult result)
        {
            switch (result)
            {
                case RegisterAccountResult.Unknown:
                    resultText.color = Color.red;
                    resultText.text = "알 수 없는 오류.";
                    break;
                case RegisterAccountResult.DuplicatedID:
                    resultText.color = Color.red;
                    resultText.text = "중복된 아이디.";
                    break;
                default:
                    break;
            }
        }
    }
}
