using System.Collections;
using System.Collections.Concurrent;
using System.Net.Sockets;
using GoblinGames;
using GoblinGames.Network;
using UnityEngine;

namespace Deckfense
{
	public class Title : MonoBehaviour
	{
		[SerializeField] private GameEvent<object> onConnectEvent;

		private ConcurrentQueue<SocketError> callbacks = new ConcurrentQueue<SocketError>();

		private void Awake()
		{
			StartCoroutine(ProcessCallback());
			onConnectEvent.AddListener(OnConnected);
		}
		private void Start()
		{
			PopupManager.Instance.OpenPopup(PopupKind.PopupLoading);
			Client.Instance.ConnectAsync();
		}

		private void OnConnected(object error)
		{
			Debug.Log("OnConnected");
			callbacks.Enqueue((SocketError)error);
		}

		private void OnConnectedCallback(SocketError error)
		{
			PopupManager.Instance.ClosePopup(PopupKind.PopupLoading);

			if (error == SocketError.Success)
			{
				PopupManager.Instance.OpenPopup(PopupKind.PopupLogin);
			}
			else
			{
				PopupManager.Instance.OpenPopup(PopupKind.PopupNotification);
				PopupNotification popup = PopupManager.Instance.GetPopup(PopupKind.PopupNotification) as PopupNotification;
				popup.ChangeNotificationText("네트워크 연결 실패", error.ToString());
			}
		}

		private IEnumerator ProcessCallback()
		{
			while (true)
			{
				if (callbacks.Count > 0)
				{
					if (callbacks.TryDequeue(out SocketError error))
					{
						OnConnectedCallback(error);
					}
				}

				yield return null;
			}
		}

	}
}
