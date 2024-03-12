using GoblinGames;
using Protocol.Data;
using Protocol.Network;
using UnityEngine;

namespace Deckfense
{
    public class SingleGameLogic : MonoBehaviour
    {
        [SerializeField] private GameEvent<string> sceneChangedEvent;

        private long actorIdIssued = 0;

        private void OnEnable()
        {
            sceneChangedEvent.AddListener(OnSceneChanged);
        }

        private void OnDisable()
        {
            sceneChangedEvent.RemoveListener(OnSceneChanged);
        }

        private void Start()
        {
            GameController.Instance.SingleGameLogic = this;
            if (!GameController.Instance.IsMulti)
            {
                InitInGame();
            }
        }

        private void OnSceneChanged(string sceneName)
        {

        }

        private void Update()
        {

        }

        private void InitInGame()
        {
            GameController.Instance.ResponseCreateActorEvent.Invoke(new ResponseCreateActor()
            {
                Type = MessageType.ResponseCreateActor,
                ActorID = IssueActorID(),
                AssetKind = "Jester",
                ActorType = ActorType.Player,
                PositionX = 0,
                PositionY = 1,
                PositionZ = 0,
                RotationX = 0,
                RotationY = 0,
                RotationZ = 0,
                
            });

            //GameController.Instance.ResponseCreateActorEvent.Invoke(new ResponseCreateActor()
            //{
            //    Type = MessageType.ResponseCreateActor,
            //    ActorID = IssueActorID(),
            //    AssetKind = "Mage",
            //    Position = new Vector3(220f, 0f, 320f),
            //    Rotation = Quaternion.identity
            //});
        }


        public void RequestValidateSessionSC(RequestValidateSessionSC args)
        {

        }

        public void RequestLogin(RequestLogin args)
        {

        }

        public void RequestLogout(RequestLogout args)
        {

        }

        public void RequestRegisterAccount(RequestRegisterAccount args)
        {

        }

        public void RequestPlayerMoveStart(RequestPlayerMoveStart args)
        {
            GameController.Instance.ResponseActorMoveStartEvent.Invoke(new ResponseActorMoveStart()
            {
                Type = MessageType.ResponseActorMoveStart,
                ActorID = args.ActorID,
                PositionX = args.PositionX,
                PositionY = args.PositionY,
                PositionZ = args.PositionZ,
                DirectionX = args.DirectionX,
                DirectionY = args.DirectionY,
                DirectionZ = args.DirectionZ,
            });
        }

        public void RequestPlayerMoveEnd(RequestPlayerMoveEnd args)
        {
            GameController.Instance.ResponseActorMoveEndEvent.Invoke(new ResponseActorMoveEnd()
            {
                Type = MessageType.ResponseActorMoveEnd,
                ActorID = args.ActorID,
                PositionX = args.PositionX,
                PositionY = args.PositionY,
                PositionZ = args.PositionZ,
                DirectionX = args.DirectionX,
                DirectionY = args.DirectionY,
                DirectionZ = args.DirectionZ,
            });
        }

        public long IssueActorID()
        {
            actorIdIssued++;
            return actorIdIssued;
        }
    }
}
