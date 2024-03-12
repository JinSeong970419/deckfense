using System.Net.Sockets;
using GoblinGames;
using UnityEngine;

namespace Deckfense
{
    [CreateAssetMenu(fileName = "Game Event SocketError", menuName = "Goblin Games/Game Event/SocketError", order = 200)]
    public class GameEventSocketError : GameEvent<SocketError>
    {
    }
}
