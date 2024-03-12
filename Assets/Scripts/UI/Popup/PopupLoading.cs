using UnityEngine;
using UnityEngine.UI;

namespace Deckfense
{
	public class PopupLoading : Popup
	{
		[SerializeField] private Image image;
		[SerializeField] private float speed;

		private void FixedUpdate()
		{
			float angle = Time.fixedTime * speed;
			image.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
		}
	}
}
