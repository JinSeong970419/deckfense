using UnityEngine;

namespace Deckfense
{
    public enum Direction
    {
        Forward,
        ForwardLeft,
        ForwardRight,
        Back,
        BackLeft,
        BackRight,
        Left,
        Right,
        Stop,
    }

    public enum ControllerKind
    {
        PlayerController,
        MonsterAi,
        TestController,
    }
    public abstract class Controller : MonoBehaviour
    {
        protected Actor actor;
        
        public Actor Actor { get { return actor; } set { actor = value; } }

        private void Awake()
        {
            actor = GetComponent<Actor>();
        }


    }
}
