using GoblinGames;
using GoblinGames.DesignPattern;
using Protocol.Data;
using Protocol.Network;
using System;
using UnityEngine;

namespace Deckfense
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private GameEvent<string> sceneChangedEvent;

        [SerializeField] private Variable<float> inGameTime;
        [Header("Input")]
        [SerializeField] private InputReader inputReader;
        [SerializeField] private ControlsRemapping controlsRemapping;

        private bool isGameScene = false;
        private GameObject dynamicUiPanel;
        [SerializeField] private Terrain currentTerrain;
        private PlayerController playerController;

        public bool IsGameScene { get { return isGameScene; } }
        public float InGameTime { get { return inGameTime.Value; } }
        public PlayerController PlayerController { get { return playerController; } set { playerController = value; } }


        public GameObject UIPanel
        {
            get
            {
                if (dynamicUiPanel == null)
                {
                    dynamicUiPanel = GameObject.Find("DynamicCanvas").transform.GetChild(0).GetChild(0).gameObject;
                }
                return dynamicUiPanel;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            controlsRemapping?.Init(inputReader.Controller);
        }

        private void OnEnable()
        {
            sceneChangedEvent.AddListener(OnSceneChanged);
            inputReader.backQute += OnKeyAction;
            GameController controller = gameObject.GetComponent<GameController>();
            controller.ResponseCreateActorEvent.AddListener(OnResponseCreateActor);
            controller.ResponseActorMoveStartEvent.AddListener(OnResponseActorMoveStartEvent);
            controller.ResponseActorMoveEndEvent.AddListener(OnResponseActorMoveEndEvent);
            controller.ResponseActorRotateEvent.AddListener(OnResponseActorRotateEvent);
        }

        private void OnDisable()
        {
            sceneChangedEvent.RemoveListener(OnSceneChanged);
            inputReader.backQute -= OnKeyAction;
            GameController controller = gameObject.GetComponent<GameController>();
            controller.ResponseCreateActorEvent.RemoveListener(OnResponseCreateActor);
            controller.ResponseActorMoveStartEvent.RemoveListener(OnResponseActorMoveStartEvent);
            controller.ResponseActorMoveEndEvent.RemoveListener(OnResponseActorMoveEndEvent);
            controller.ResponseActorRotateEvent.RemoveListener(OnResponseActorRotateEvent);
        }

        private void Update()
        {
            if (isGameScene == false) return;

        }

        public GameObject CreateActor(AssetKind kind)
        {
            GameObject actor = ObjectPool.Instance.Allocate(ObjectKind.Actor);
            Actor actorComponent = actor.GetComponent<Actor>();
            actorComponent.SetAsset(kind);

            return actor;
        }



        private void OnSceneChanged(string sceneName)
        {
            if (sceneName == "InGame")
            {
                isGameScene = true;
                inGameTime.Value = 0;
                currentTerrain = GameObject.Find("First Island").GetComponent<Terrain>();
            }
            else
            {
                isGameScene = false;
            }
        }

        private void OnKeyAction(Action action)
        {
            action.Invoke();
        }

        private void OnResponseCreateActor(object msg)
        {
            if(!isGameScene)
            {
                return;
            }

            ResponseCreateActor response = msg as ResponseCreateActor;

            if (Enum.TryParse(response.AssetKind, out AssetKind kind))
            {
                GameObject actor = CreateActor(kind);
                Actor actorComponent = actor.GetComponent<Actor>();

                Controller controller = null;
                switch (response.ActorType)
                {
                    case ActorType.NPC:
                        break;
                    case ActorType.Player:
                        controller = actor.AddComponent<PlayerController>();
                        break;
                    case ActorType.Enemy:
                        controller = actor.AddComponent<MonsterAi>();
                        break;
                    case ActorType.OtherPlayer:
                        controller = actor.AddComponent<OtherPlayerController>();
                        break;
                    default:
                        break;
                }

                //Vector3 posInTerrain = new Vector3(response.PositionX, response.PositionY, response.PositionZ);
                //posInTerrain.y = currentTerrain.SampleHeight(posInTerrain) + currentTerrain.transform.position.y + 1f;
                //actor.transform.position = posInTerrain;
                actor.transform.position = new Vector3(response.PositionX, response.PositionY, response.PositionZ);
                actor.transform.rotation = Quaternion.Euler(new Vector3(response.RotationX, response.RotationY, response.RotationZ));
                actorComponent.ID = response.ActorID;


                Entity.Add(response.ActorID, actorComponent);
            }
        }

        private void OnResponseActorMoveStartEvent(object msg)
        {
            if (!isGameScene)
            {
                return;
            }
            ResponseActorMoveStart response = msg as ResponseActorMoveStart;

            if (!Actor.Entities.ContainsKey(response.ActorID))
            {
                return;
            }
            Actor actor = Actor.Entities[response.ActorID] as Actor;
            if (actor == null)
            {
                return;
            }

            actor.MoveStart(
                new Vector3(response.PositionX, response.PositionY, response.PositionZ),
                new Vector3(response.DirectionX, response.DirectionY, response.DirectionZ),
                new Vector3(response.RotationX, response.RotationY, response.RotationZ)
                );
        }

        private void OnResponseActorMoveEndEvent(object msg)
        {
            if (!isGameScene)
            {
                return;
            }
            ResponseActorMoveEnd response = msg as ResponseActorMoveEnd;

            if (!Actor.Entities.ContainsKey(response.ActorID))
            {
                return;
            }
            Actor actor = Actor.Entities[response.ActorID] as Actor;
            if( actor == null)
            {
                return;
            }

            actor.MoveEnd(
                new Vector3(response.PositionX, response.PositionY, response.PositionZ),
                new Vector3(response.DirectionX, response.DirectionY, response.DirectionZ),
                new Vector3(response.RotationX, response.RotationY, response.RotationZ)
                );
        }

        private void OnResponseActorRotateEvent(object msg)
        {
            if (!isGameScene)
            {
                return;
            }
            ResponseActorRotate response = msg as ResponseActorRotate;

            if(playerController == null)
            {
                return;
            }
            if (playerController.Actor == null)
            {
                return;
            }
            if (response.ActorID == playerController.Actor.ID)
            {
                return;
            }
            if(!Actor.Entities.ContainsKey(response.ActorID))
            {
                return;
            }

            Actor actor = Actor.Entities[response.ActorID] as Actor;
            if (actor == null)
            {
                return;
            }
            actor.transform.rotation = Quaternion.Euler(new Vector3(response.RotationX, response.RotationY, response.RotationZ));
        }

        [DebugButton]
        public void Test()
        {
            //GameObject obj = CreateActor(AssetKind.Mage);
            //obj.AddComponent<TestController>();

            //RequestPlayerMoveStart startMsg = new RequestPlayerMoveStart();
            //startMsg.Type = MessageType.RequestPlayerMoveStart;
            //startMsg.ActorID = 0;
            //startMsg.Position = Vector3.up;
            //startMsg.Direction = Vector3.forward;
            //GameController.Instance.RequestPlayerMoveStart(startMsg);

            //GameController.Instance.RequestEnterGame(new RequestEnterGame()
            //{
            //    Type = MessageType.RequestEnterGame,
            //});
        }
    }
}