using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GoblinGames;
using GoblinGames.DesignPattern;
using UnityEngine;

namespace Deckfense
{
	public class ObjectPool : MonoSingleton<ObjectPool>
	{
		[SerializeField] private GameObject[] prefabs;
		private Dictionary<string, List<GameObject>> _pools = new Dictionary<string, List<GameObject>>();

		[DebugButton]
		public void GenerateEnum()
		{
#if UNITY_EDITOR
			string enumName = "ObjectKind";
			string fileName = enumName + ".cs";
			string path = "Assets/Scripts/ObjectPool/";
			string fullPath = Path.Combine(path, fileName);
			var list = prefabs.Select(x => x.name).ToList();
			Extension.GenerateEnumWithEnd(fullPath, enumName, GetType().Namespace, list);
#endif
		}

		public void Initialize()
		{
			for (int i = 0; i < prefabs.Length; i++)
			{
				GameObject prefab = prefabs[i];
				string key = prefab.name.Split('(')[0];
				prefab.name = key;
				if (!_pools.ContainsKey(key))
				{
					_pools.Add(key, new List<GameObject>());
				}
				UpSizing(key);
			}
		}
		public GameObject Allocate(string key)
		{
			string _key = key.Split('(')[0];
			if (!_pools.ContainsKey(_key))
			{
				_pools.Add(_key, new List<GameObject>());
			}

			if (_pools[_key].Count == 0)
			{
				UpSizing(_key);
			}
			GameObject gameObject = _pools[_key].PopFront();
			gameObject.name = _key;
			gameObject.SetActive(true);

			return gameObject;
		}
		public GameObject Allocate(string key, Vector3 position)
		{
			var gameObject = Allocate(key);
			gameObject.transform.position = position;

			return gameObject;
		}
		public GameObject Allocate(string key, Vector3 position, Quaternion rotation)
		{
			var gameObject = Allocate(key);
			gameObject.transform.SetPositionAndRotation(position, rotation);

			return gameObject;
		}

		public GameObject Allocate(ObjectKind key)
		{
			string _key = key.ToString();
			if (!_pools.ContainsKey(_key))
			{
				_pools.Add(_key, new List<GameObject>());
			}

			if (_pools[_key].Count == 0)
			{
				UpSizing(_key);
			}
			GameObject gameObject = _pools[_key].PopFront();
			gameObject.name = _key;
			gameObject.SetActive(true);

			return gameObject;
		}
		public GameObject Allocate(ObjectKind key, Vector3 position)
		{
			var gameObject = Allocate(key);
			gameObject.transform.position = position;

			return gameObject;
		}
		public GameObject Allocate(ObjectKind key, Vector3 position, Quaternion rotation)
		{
			var gameObject = Allocate(key);
			gameObject.transform.SetPositionAndRotation(position, rotation);

			return gameObject;
		}


		public void Free(GameObject _gameObject)
		{
			string key = _gameObject.name.Split('(')[0];
			if (!_pools.ContainsKey(key))
			{
				_pools.Add(key, new List<GameObject>());
			}
			_gameObject.SetActive(false);
			_pools[key].PushFront(_gameObject);
		}
		private void UpSizing(string key)
		{
			if (!_pools.ContainsKey(key))
			{
				_pools.Add(key, new List<GameObject>());
			}

			GameObject prefab = GetPrefab(key);

			for (int i = 0; i < 10; i++)
			{
				GameObject go = Instantiate(prefab, transform);
				go.SetActive(false);
				_pools[key].Add(go);
			}
		}
		private GameObject GetPrefab(string key)
		{
			GameObject _prefab = null;
			for (int i = 0; i < prefabs.Length; i++)
			{
				GameObject prefab = prefabs[i];
				if (prefab.name == key)
				{
					_prefab = prefab;
					break;
				}
			}
			return _prefab;
		}

	}
}
