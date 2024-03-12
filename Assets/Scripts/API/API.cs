using GoblinGames;
using Protocol.Network;
using UnityEngine;

namespace Deckfense
{
    [CreateAssetMenu(fileName = "API", menuName = "Goblin Games/API")]
    public class API : ScriptableObject
    {
        [SerializeField] private GameEvent<object>[] apiEvents = new GameEvent<object>[(int)MessageType.End];

        public GameEvent<object>[] Events { get { return apiEvents; } }

        private void OnValidate()
        {
            int count = (int)MessageType.End;
            if (apiEvents == null)
            {
                apiEvents = new GameEvent<object>[count];
            }
            else
            {
                int len = apiEvents.Length;
                if (len < count)
                {
                    var array = new GameEvent<object>[count];
                    for (int i = 0; i < len; i++)
                    {
                        array[i] = apiEvents[i];
                    }
                }
                else if (len > count)
                {
                    var array = new GameEvent<object>[count];
                    for (int i = 0; i < count; i++)
                    {
                        array[i] = apiEvents[i];
                    }
                }
            }
        }
    }
}
