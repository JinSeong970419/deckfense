using System.Collections.Generic;
using UnityEngine;

namespace Deckfense
{
	public abstract class Entity : MonoBehaviour
	{
		private static Dictionary<long, Entity> entities = new Dictionary<long, Entity>();
		public static Dictionary<long, Entity> Entities { get { return entities; } }
		public long ID { get; set; }

		protected virtual void OnEnable()
		{

		}

		protected virtual void OnDisable()
		{
			Entity.Remove(ID);
		}

		protected virtual void FixedUpdate()
		{

		}

		protected virtual void Update()
		{

		}

		protected virtual void LateUpdate()
		{

		}

		public static void Add(long id, Entity entity)
		{
			entities.Add(id, entity);
		}

		public static void Remove(long id)
		{
			entities.Remove(id);
		}
	}
}
