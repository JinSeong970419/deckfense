using Protocol.Network;
using UnityEngine;

namespace Deckfense
{
	public enum CameraMode
	{
		FirstPerson,
		ThirdPerson,
		Test,
	}
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private CameraMode mode;
		[SerializeField] private float speed = 5f;
		[SerializeField] private Vector3 offset;
		[SerializeField] private float zoom;
		private Vector3 rotation;
		private Vector3 oldRotation;
        [SerializeField] private float angleX;
        [SerializeField] private float limitAngleX;
        [SerializeField] private float rotateXSpeed;
		[SerializeField] private float rotateYSpeed = 1f;
		[SerializeField] private Transform lookTarget;

		private void Awake()
		{
			Cursor.lockState = CursorLockMode.Locked;
        }

		private void FixedUpdate()
		{
			SyncPlayerRotate();
        }

		private void LateUpdate()
		{
			ProcessMove();
            ProcessRotate();
            if (Input.GetKeyDown(KeyCode.Escape))
			{
				Cursor.lockState = CursorLockMode.None;
			}
		}

		private void ProcessMove()
		{
			switch (mode)
			{
				case CameraMode.FirstPerson:
					break;
				case CameraMode.ThirdPerson:
					ThirdPersonPointView();
					break;
				case CameraMode.Test:
					ProcessTest();
					break;
				default:
					break;
			}
		}

		private void ProcessTest()
		{
			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");
			Vector3 direction = (transform.forward * vertical + transform.right * horizontal).normalized;
			// 001 * 5 = 005
			// 100 * 3 = 300
			transform.position = transform.position + direction * speed * Time.deltaTime;

			//Debug.Log($"{horizontal}/{vertical}");

			float mouseX = Input.GetAxis("Mouse X");
			float mouseY = Input.GetAxis("Mouse Y");

			//Debug.Log($"{mouseX}/{mouseY}");


			rotation = transform.rotation.eulerAngles;

			rotation.x += -mouseY;
			rotation.y += mouseX;

			transform.rotation = Quaternion.Euler(rotation);


		}

		private void ThirdPersonPointView()
		{
			if (GameManager.Instance.PlayerController == null)
			{
				return;
			}
			if (GameManager.Instance.PlayerController.Actor == null)
			{
				return;
			}

			Actor playerActor = GameManager.Instance.PlayerController.Actor;
			Vector3 target = playerActor.transform.position + offset;

			float mouseY = Input.GetAxis("Mouse Y");
			angleX += mouseY * Time.deltaTime * rotateXSpeed;
			if(angleX > limitAngleX)
			{
				angleX = limitAngleX;
			}
			else if(angleX < -limitAngleX)
			{
				angleX = -limitAngleX;
			}

			//transform.localPosition = -playerActor.transform.forward * zoom;
			Vector3 angle = lookTarget.transform.rotation.eulerAngles;
			angle.x = -angleX;
			angle.y = playerActor.transform.rotation.eulerAngles.y;
			lookTarget.transform.rotation = Quaternion.Euler(angle);
			lookTarget.transform.position = target;
        }

        private Vector3 RotateY(Vector3 pos, float angle)
        {
            Vector3 rotate;
            rotate.x = Mathf.Cos(angle) * pos.x + Mathf.Sin(angle) * pos.z;
            rotate.y = pos.y;
            rotate.z = -Mathf.Sin(angle) * pos.x + Mathf.Cos(angle) * pos.z;

            return rotate;
        }

        private Vector3 RotateZ(Vector3 pos, float angle)
		{
			Vector3 rotate;
            rotate.x = Mathf.Cos(angle) * pos.x - Mathf.Sin(angle) * pos.y;
            rotate.y = Mathf.Sin(angle) * pos.x + Mathf.Cos(angle) * pos.y;
            rotate.z = pos.z;

			return rotate;
        }

		private void ProcessRotate()
		{
			switch (mode)
			{
				case CameraMode.FirstPerson:
					break;
				case CameraMode.ThirdPerson:
                    ThirdPersonRotate();
					break;
				case CameraMode.Test:
					break;
				default:
					break;
			}
		}

		private void ThirdPersonRotate()
		{
            if (GameManager.Instance.PlayerController == null)
            {
                return;
            }
            if (GameManager.Instance.PlayerController.Actor == null)
            {
                return;
            }

			Actor playerActor = GameManager.Instance.PlayerController.Actor;
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            //Debug.Log($"{mouseX}/{mouseY}");

            rotation = playerActor.transform.rotation.eulerAngles;

			//rotation.x += -mouseY;
			rotation.y += mouseX * rotateYSpeed;

			playerActor.Rotate(rotation);
			//transform.LookAt(playerActor.transform.position + offset);
        }

		private void SyncPlayerRotate()
		{
			Vector3 difference = oldRotation - rotation;
			if(difference.magnitude > float.Epsilon)
			{
                GameController.Instance.RequestPlayerRotate(new RequestPlayerRotate()
                {
                    Type = MessageType.RequestPlayerRotate,
                    ActorID = GameManager.Instance.PlayerController.Actor.ID,
                    RotationX = rotation.x,
                    RotationY = rotation.y,
                    RotationZ = rotation.z,
                });
            }
            oldRotation = rotation;
        }

    }
}
