using Protocol.Network;
using UnityEngine;

namespace Deckfense
{
    public class PlayerController : Controller
    {
        private Direction currentDirection = Direction.Stop;
        
        // Start is called before the first frame update

        private void OnEnable()
        {
            GameManager.Instance.PlayerController = this;
        }

        private void OnDisable()
        {
            GameManager.Instance.PlayerController = null;
        }
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            KeyInputDetection();
        }

        private void KeyInputDetection()
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    ProcessMoveMessage(Direction.ForwardLeft);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    ProcessMoveMessage(Direction.ForwardRight);
                }
                else
                {
                    ProcessMoveMessage(Direction.Forward);
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    ProcessMoveMessage(Direction.BackLeft);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    ProcessMoveMessage(Direction.BackRight);
                }
                else
                {
                    ProcessMoveMessage(Direction.Back);
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                ProcessMoveMessage(Direction.Left);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                ProcessMoveMessage(Direction.Right);
            }
            else
            {
                ProcessMoveMessage(Direction.Stop);
            }
        }

        private void ProcessMoveMessage(Direction dir)
        {
            if (currentDirection == dir)
            {
                return;
            }
            
            if (currentDirection != Direction.Stop)
            {
                RequestPlayerMoveEnd endMsg = new RequestPlayerMoveEnd();
                endMsg.Type = MessageType.RequestPlayerMoveEnd;
                endMsg.ActorID = actor.ID;
                endMsg.PositionX = transform.position.x;
                endMsg.PositionY = transform.position.y;
                endMsg.PositionZ = transform.position.z;
                Vector3 direction = DirectionToVector3(currentDirection);
                endMsg.DirectionX = direction.x;
                endMsg.DirectionY = direction.y;
                endMsg.DirectionZ = direction.z;
                endMsg.RotationX = transform.rotation.eulerAngles.x;
                endMsg.RotationY = transform.rotation.eulerAngles.y;
                endMsg.RotationZ = transform.rotation.eulerAngles.z;
                GameController.Instance.RequestPlayerMoveEnd(endMsg);
            }

            currentDirection = dir;
            
            if (currentDirection != Direction.Stop)
            {
                RequestPlayerMoveStart startMsg = new RequestPlayerMoveStart();
                startMsg.Type = MessageType.RequestPlayerMoveStart;
                startMsg.ActorID = actor.ID;
                startMsg.PositionX = transform.position.x;
                startMsg.PositionY = transform.position.y;
                startMsg.PositionZ = transform.position.z;
                Vector3 direction = DirectionToVector3(currentDirection);
                startMsg.DirectionX = direction.x;
                startMsg.DirectionY = direction.y;
                startMsg.DirectionZ = direction.z;
                startMsg.RotationX = transform.rotation.eulerAngles.x;
                startMsg.RotationY = transform.rotation.eulerAngles.y;
                startMsg.RotationZ = transform.rotation.eulerAngles.z;
                GameController.Instance.RequestPlayerMoveStart(startMsg);
            }


        }

        private Vector3 DirectionToVector3(Direction dir)
        {
            Vector3 vec = Vector3.zero;

            switch (dir)
            {
                case Direction.Forward:
                    vec = Vector3.forward;
                    break;
                case Direction.ForwardLeft:
                    vec = Vector3.forward + Vector3.left;
                    break;
                case Direction.ForwardRight:
                    vec = Vector3.forward + Vector3.right;
                    break;
                case Direction.Back:
                    vec = Vector3.back;
                    break;
                case Direction.BackLeft:
                    vec = Vector3.back + Vector3.left;
                    break;
                case Direction.BackRight:
                    vec = Vector3.back + Vector3.right;
                    break;
                case Direction.Left:
                    vec = Vector3.left;
                    break;
                case Direction.Right:
                    vec = Vector3.right;
                    break;
                case Direction.Stop:
                    {
                        vec = Vector3.zero;
                        return vec;
                    }
                default:
                    break;
            }

            return vec.normalized;
        }
    }
}
