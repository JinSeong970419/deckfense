using GoblinGames;
using Protocol.Network;
using TMPro;
using UnityEngine;

namespace Deckfense
{
    public class PopupChat : Popup
    {
        [SerializeField] private GameEvent<object> requestChatEvent;
        [SerializeField] private GameEvent<object> responseChatEvent;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private RectTransform content;
        [SerializeField] private GameObject chatMessagePrefab;

        private bool submitFlag = false;
        private bool deactivateFlag = false;

        private int test = 0;
        protected override void OnEnable()
        {
            base.OnEnable();

            responseChatEvent.AddListener(OnResponseChat);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            responseChatEvent.RemoveListener(OnResponseChat);
        }

        private void Start()
        {
            inputField.onSubmit.AddListener(OnSubmit);
        }

        private void Update()
        {
            //Debug.Log(inputField.isFocused);

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (submitFlag == false)
                {
                    inputField.ActivateInputField();
                }
                else
                {
                    submitFlag = false;
                    if (deactivateFlag)
                    {
                        deactivateFlag = false;
                    }
                    else
                    {
                        inputField.ActivateInputField();
                    }

                }
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                test++;
                OnResponseChat(new ResponseChat() { Message = $"TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTest{test}" });
            }

        }

        private void OnSubmit(string msg)
        {

            if (inputField.text == string.Empty)
            {
                deactivateFlag = true;
            }
            else
            {
                // Send
                requestChatEvent.Invoke(new RequestChat()
                {
                    Type = MessageType.RequestChat,
                    Message = msg
                });
            }

            inputField.text = string.Empty;
            submitFlag = true;

        }

        private void OnResponseChat(object args)
        {
            // Recv
            Debug.Log("PopupChat.OnResponseChat");
            ResponseChat res = args as ResponseChat;
            GameObject chatMessageObj = Instantiate(chatMessagePrefab, content);
            var text = chatMessageObj.GetComponent<UI_Chat>();
            text.Text = res.Message;
            Debug.Log(res.Message);
        }
    }
}
