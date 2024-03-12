using GoblinGames;
using GoblinGames.DesignPattern;
using Protocol.Network;
using UnityEngine;

namespace Deckfense
{
    public class GameController : MonoSingleton<GameController>
    {
        [SerializeField] private API api;

        [SerializeField] private bool isMulti;
        private SingleGameLogic singleGameLogic;
        public bool IsMulti { get { return isMulti; } set { isMulti = value; } }
        public SingleGameLogic SingleGameLogic { set { singleGameLogic = value; } }


        public GameEvent<object> ResponseCreateActorEvent { get { return api.Events[(int)MessageType.ResponseCreateActor]; } }
        public GameEvent<object> ResponseDestroyActorEvent { get { return api.Events[(int)MessageType.ResponseDestroyActor]; } }
        public GameEvent<object> ResponseActorMoveStartEvent { get { return api.Events[(int)MessageType.ResponseActorMoveStart]; } }
        public GameEvent<object> ResponseActorMoveEndEvent { get { return api.Events[(int)MessageType.ResponseActorMoveEnd]; } }
        public GameEvent<object> ResponseActorRotateEvent { get { return api.Events[(int)MessageType.ResponseActorRotate]; } }

        public void RequestValidateSessionSC(RequestValidateSessionSC args)
        {
            api.Events[(int)MessageType.RequestValidateSessionSC].Invoke(args);
        }

        public void ResponseValidateSessionCS(ResponseValidateSessionCS args)
        {
            api.Events[(int)MessageType.ResponseValidateSessionCS].Invoke(args);
        }

        public void ResponseNetworkStatus(ResponseNetworkStatus args)
        {
            api.Events[(int)MessageType.ResponseNetworkStatus].Invoke(args);
        }

        public void RequestChangeClientState(RequestChangeClientState args)
        {
            api.Events[(int)MessageType.RequestChangeClientState].Invoke(args);
        }

        public void RequestLogin(RequestLogin args)
        {
            api.Events[(int)MessageType.RequestLogin].Invoke(args);
        }

        public void ResponseLogin(ResponseLogin args)
        {
            api.Events[(int)MessageType.ResponseLogin].Invoke(args);
        }

        public void RequestLogout(RequestLogout args)
        {
            api.Events[(int)MessageType.RequestLogout].Invoke(args);
        }

        public void ResponseLogout(ResponseLogout args)
        {
            api.Events[(int)MessageType.ResponseLogout].Invoke(args);
        }

        public void RequestRegisterAccount(RequestRegisterAccount args)
        {
            api.Events[(int)MessageType.RequestRegisterAccount].Invoke(args);
        }

        public void ResponseRegisterAccount(ResponseRegisterAccount args)
        {
            api.Events[(int)MessageType.ResponseRegisterAccount].Invoke(args);
        }

        public void RequestChat(RequestChat args)
        {
            api.Events[(int)MessageType.RequestChat].Invoke(args);
        }

        public void ResponseChat(ResponseChat args)
        {
            api.Events[(int)MessageType.ResponseChat].Invoke(args);
        }

        public void RequestEnterGame(RequestEnterGame args)
        {   
            api.Events[(int)MessageType.RequestEnterGame].Invoke(args);
        }

        public void RequestPlayerMoveStart(RequestPlayerMoveStart args)
        {
            if (isMulti)
            {
                api.Events[(int)MessageType.RequestPlayerMoveStart].Invoke(args);
            }
            else
            {
                singleGameLogic.RequestPlayerMoveStart(args);
            }
        }

        public void RequestPlayerMoveEnd(RequestPlayerMoveEnd args)
        {
            if (isMulti)
            {
                api.Events[(int)MessageType.RequestPlayerMoveEnd].Invoke(args);
            }
            else
            {
                singleGameLogic.RequestPlayerMoveEnd(args);
            }
        }

        public void RequestPlayerRotate(RequestPlayerRotate args)
        {
            if (isMulti)
            {
                api.Events[(int)MessageType.RequestPlayerRotate].Invoke(args);
            }
            else
            {
                
            }
        }

    }
}
