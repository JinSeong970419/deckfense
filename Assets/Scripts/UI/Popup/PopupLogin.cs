using System.Text.RegularExpressions;
using GoblinGames;
using GoblinGames.Data;
using GoblinGames.Network;
using Protocol.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Deckfense
{
    public class PopupLogin : Popup
    {
        [SerializeField] private GameEvent<object> requestLoginEvent;
        [SerializeField] private GameEvent<object> responseLoginEvent;
        [SerializeField] private TMP_InputField inputFieldID;
        [SerializeField] private TMP_InputField inputFieldPW;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private Button loginButton;
        [SerializeField] private Button registerButton;

        [SerializeField] private Image loadingPanel;
        [SerializeField] private Image loadingRing;

        private bool waitForLoginResponse = false;
        

        protected override void OnEnable()
        {
            base.OnEnable();

            loginButton.onClick.AddListener(OnLoginButtonClick);
            registerButton.onClick.AddListener(OnRegisterButtonClick);
            responseLoginEvent.AddListener(OnResponseLogin);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            loginButton.onClick.RemoveListener(OnLoginButtonClick);
            registerButton.onClick.RemoveListener(OnRegisterButtonClick);
            responseLoginEvent.RemoveListener(OnResponseLogin);
        }

        private void Update()
        {
            if (waitForLoginResponse)
            {
                if (loadingPanel.gameObject.activeSelf == false)
                {
                    loadingPanel.gameObject.SetActive(true);
                }
                loadingRing.fillAmount = Time.unscaledTime % 1f;
            }
        }

        private void OnLoginButtonClick()
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

            waitForLoginResponse = true;
            requestLoginEvent.Invoke(new RequestLogin()
            {
                Type = MessageType.RequestLogin,
                Id = id,
                Password = pw,
            });
        }

        private void OnRegisterButtonClick()
        {
            PopupManager.Instance.OpenPopup(PopupKind.PopupRegister);
        }

        private void OnResponseLogin(object msg)
        {
            ResponseLogin response = msg as ResponseLogin;
            if (response.Result == LoginResult.Success)
            {
                OnLoginSuccess();
            }
            else
            {
                OnLoginFailed(response.Result);
            }


        }

        private void OnLoginSuccess()
        {
            waitForLoginResponse = false;
            loadingPanel.gameObject.SetActive(false);
            SceneController.LoadScene("Lobby");
        }

        private void OnLoginFailed(LoginResult result)
        {
            waitForLoginResponse = false;
            loadingPanel.gameObject.SetActive(false);

            resultText.color = Color.red;
            switch (result)
            {
                case LoginResult.Unknown:
                    resultText.text = "알 수 없는 오류.";
                    break;
                case LoginResult.IDDeosNotExist:
                    resultText.text = "존재하지 않는 아이디 입니다.";
                    break;
                case LoginResult.MismatchedPassword:
                    resultText.text = "비밀번호가 일치하지 않습니다.";
                    break;
                case LoginResult.AlreadyLoggedIn:
                    resultText.text = "이미 로그인 접속 중입니다.";
                    break;
                default:
                    resultText.text = "알 수 없는 오류.";
                    break;
            }
        }

    }
}
