using System;
using System.Collections;
using System.Collections.Concurrent;
using GoblinGames;
using GoblinGames.DesignPattern;
using GoblinGames.Network;
using Protocol.Network;
using UnityEngine;

namespace Deckfense
{
    public class NetworkManager : MonoSingleton<NetworkManager>
    {
        [SerializeField] private API api;
        [SerializeField] private GameEvent<object> onReceiveEvent;

        private ConcurrentQueue<Message> messages = new ConcurrentQueue<Message>();
        private bool shutdownFlag = false;

        #region NetworkStatus
        [SerializeField] private Variable<long> roundTripTime;
        [SerializeField] private Variable<long> delayFromServerToClient;
        [SerializeField] private Variable<long> delayFromClientToServer;

        #endregion


        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            onReceiveEvent.AddListener(OnReceive);
			api.Events[(int)MessageType.RequestChangeClientState].AddListener(OnRequestChangeClientState);
			api.Events[(int)MessageType.RequestLogin].AddListener(OnRequestLogin);
            api.Events[(int)MessageType.RequestLogout].AddListener(OnRequestLogout);
            api.Events[(int)MessageType.RequestRegisterAccount].AddListener(OnRequestRegisterAccount);
            api.Events[(int)MessageType.RequestChat].AddListener(OnRequestChat);
            api.Events[(int)MessageType.RequestEnterGame].AddListener(OnRequestEnterGame);
            api.Events[(int)MessageType.RequestPlayerMoveStart].AddListener(OnRequestPlayerMoveStart);
            api.Events[(int)MessageType.RequestPlayerMoveEnd].AddListener(OnRequestPlayerMoveEnd);
            api.Events[(int)MessageType.RequestPlayerRotate].AddListener(OnRequestPlayerRotate);
        }

        private void OnDisable()
        {
            onReceiveEvent.RemoveListener(OnReceive);
			api.Events[(int)MessageType.RequestChangeClientState].RemoveListener(OnRequestChangeClientState);
			api.Events[(int)MessageType.RequestLogin].RemoveListener(OnRequestLogin);
            api.Events[(int)MessageType.RequestLogout].RemoveListener(OnRequestLogout);
            api.Events[(int)MessageType.RequestRegisterAccount].RemoveListener(OnRequestRegisterAccount);
            api.Events[(int)MessageType.RequestChat].RemoveListener(OnRequestChat);
			api.Events[(int)MessageType.RequestEnterGame].RemoveListener(OnRequestEnterGame);
			api.Events[(int)MessageType.RequestPlayerMoveStart].RemoveListener(OnRequestPlayerMoveStart);
            api.Events[(int)MessageType.RequestPlayerMoveEnd].RemoveListener(OnRequestPlayerMoveEnd);
            api.Events[(int)MessageType.RequestPlayerRotate].RemoveListener(OnRequestPlayerRotate);
        }

        private void Start()
        {
            StartCoroutine(ProcessReceive());
        }

        private IEnumerator ProcessReceive()
        {
            while (!shutdownFlag)
            {
                if (messages.TryDequeue(out var msg))
                {
                    MessageType type = (MessageType)msg.Type;
                    switch (type)
                    {
                        case MessageType.RequestValidateSessionSC:
                            OnRequestValidateSessionSC(msg as RequestValidateSessionSC);
                            break;
                        case MessageType.ResponseNetworkStatus:
                            OnResponseNetworkStatus(msg as ResponseNetworkStatus);
                            break;
                        case MessageType.ResponseLogin:
                            OnResponseLogin(msg as ResponseLogin);
                            break;
                        case MessageType.ResponseLogout:
                            OnResponseLogout(msg as ResponseLogout);
                            break;
                        case MessageType.ResponseRegisterAccount:
                            OnResponseRegisterAccount(msg as ResponseRegisterAccount);
                            break;
                        case MessageType.ResponseChat:
                            OnResponseChat(msg as ResponseChat);
                            break;
                        case MessageType.ResponseCreateActor:
                            OnResponseCreateActor(msg as ResponseCreateActor);
                            break;
                        case MessageType.ResponseDestroyActor:
                            OnResponseDestroyActor(msg as ResponseDestroyActor);
                            break;
                        case MessageType.ResponseActorMoveStart:
                            OnResponseActorMoveStart(msg as ResponseActorMoveStart);
                            break;
                        case MessageType.ResponseActorMoveEnd:
                            OnResponseActorMoveEnd(msg as ResponseActorMoveEnd);
                            break;
                        case MessageType.ResponseActorRotate:
                            OnResponseActorRotate(msg as ResponseActorRotate);
                            break;
                        default:
                            Debug.LogError("Invalid message type.");
                            break;
                    }
                }
                else
                {
                    yield return null;
                }
            }
        }

        private void OnReceive(object message)
        {
            Message msg = message as Message;
            if (msg == null)
            {
                Debug.LogError("Invalid message.");
                return;
            }

            messages.Enqueue(msg);
        }

        public void OnResponseValidateSession(object args)
        {
            Debug.Log($"OnResponseValidateSession()");
            Client.Instance.Send(args);
        }
        public void OnRequestChangeClientState(object args)
        {
			Debug.Log($"OnRequestChangeClientState()");
			Client.Instance.Send(args);
		}
        public void OnRequestLogin(object args)
        {
            Debug.Log($"OnRequestLogin()");
            Client.Instance.Send(args);
        }
        public void OnRequestLogout(object args)
        {
            Debug.Log($"OnRequestLogout()");
            Client.Instance.Send(args);
        }
        public void OnRequestRegisterAccount(object args)
        {
            Debug.Log($"OnRequestRegisterAccount()");
            Client.Instance.Send(args);
        }
        public void OnRequestChat(object args)
        {
            Debug.Log($"OnRequestChat()");
            Client.Instance.Send(args);
        }
		public void OnRequestEnterGame(object args)
		{
            Debug.Log($"OnRequestEnterGame()");
			Client.Instance.Send(args);
		}
		public void OnRequestPlayerMoveStart(object args)
        {
            Debug.Log($"OnRequestPlayerMoveStart()");
            Client.Instance.Send(args);
        }
        public void OnRequestPlayerMoveEnd(object args)
        {
            Debug.Log($"OnRequestPlayerMoveEnd()");
            Client.Instance.Send(args);
        }
        public void OnRequestPlayerRotate(object args)
        {
            //Debug.Log($"OnRequestPlayerRotate()");
            Client.Instance.Send(args);
        }

        // ===============================

        private void OnRequestValidateSessionSC(RequestValidateSessionSC args)
        {
            // 2. 도착시간을 서버로 보냄
            Debug.Log("OnRequestValidateSessionSC");
            DateTime koreaNow = DateTime.UtcNow.AddHours(9);
            long milliseconds = koreaNow.Ticks / TimeSpan.TicksPerMillisecond;

            OnResponseValidateSession(new ResponseValidateSessionCS()
            {
                Type = MessageType.ResponseValidateSessionCS,
                StartTick = args.StartTick,
                EndTick = milliseconds,
            });

            api.Events[(int)args.Type].Invoke(args);
        }

        private void OnResponseNetworkStatus(ResponseNetworkStatus args)
        {
            Debug.Log("OnResponseNetworkStatus");

            roundTripTime.Value = args.RoundTripTime;
            delayFromServerToClient.Value = args.ServerDelay;
            delayFromClientToServer.Value = args.ClientDelay;

            api.Events[(int)args.Type].Invoke(args);
        }

        private void OnResponseLogin(ResponseLogin args)
        {
            Debug.Log("OnResponseLogin");
            api.Events[(int)args.Type].Invoke(args);
        }
        private void OnResponseLogout(ResponseLogout args)
        {
            Debug.Log("OnResponseLogout");
            api.Events[(int)args.Type].Invoke(args);
        }
        private void OnResponseRegisterAccount(ResponseRegisterAccount args)
        {
            Debug.Log("OnResponseRegisterAccount");
            api.Events[(int)args.Type].Invoke(args);
        }
        private void OnResponseChat(ResponseChat args)
        {
            Debug.Log("OnResponseChat");
            api.Events[(int)args.Type].Invoke(args);
        }
        private void OnResponseCreateActor(ResponseCreateActor args)
        {
            Debug.Log("OnResponseCreateActor");
            api.Events[(int)args.Type].Invoke(args);
        }
        private void OnResponseDestroyActor(ResponseDestroyActor args)
        {
            Debug.Log("OnResponseDestroyActor");
            api.Events[(int)args.Type].Invoke(args);
        }
        private void OnResponseActorMoveStart(ResponseActorMoveStart args)
        {
            Debug.Log("OnResponseActorMoveStart");
            api.Events[(int)args.Type].Invoke(args);
        }
        private void OnResponseActorMoveEnd(ResponseActorMoveEnd args)
        {
            Debug.Log("OnResponseActorMoveEnd");
            api.Events[(int)args.Type].Invoke(args);
        }
        private void OnResponseActorRotate(ResponseActorRotate args)
        {
            Debug.Log("OnResponseActorRotate");
            api.Events[(int)args.Type].Invoke(args);
        }

    }
}
