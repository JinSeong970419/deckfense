using System.Collections;
using System.Collections.Generic;
using GoblinGames;
using UnityEngine;

namespace Deckfense
{
    public class DayNightSystem : MonoBehaviour
    {
        private static DayNightSystem instance;

        public static DayNightSystem Instance { get { return instance; } }

		[SerializeField] private Variable<float> inGameTime;
		[SerializeField] private Variable<float> dayDuration;
		[SerializeField] private Variable<float> solarAltitudeAngle;

		[SerializeField] private Camera mainCamera;
		[SerializeField] private GameObject sun;
		[SerializeField] private GameObject moon;
		[SerializeField] private float sunDistance = 990f;

		
		private void Awake()
		{
			instance = this;
			mainCamera = Camera.main;
		}

		private void OnDestroy()
		{
			instance = null;
		}

		private void OnValidate()
		{
			mainCamera = Camera.main;
		}

		private void Start()
		{
		}

		

		private void Update()
		{
			// 시간 경과
			inGameTime.Value += Time.unscaledDeltaTime;
			float currentTime = inGameTime.Value;

			// 태양 위치 계산
			Vector3 sunPosition = CalculateSunPosition(currentTime, sunDistance);

			// 태양 모델 위치 업데이트
			sun.transform.position = mainCamera.transform.position + sunPosition;

			// 달 위치 업데이트
			moon.transform.position = mainCamera.transform.position - sunPosition;
			Vector3 moonDirection = Vector3.Normalize(mainCamera.transform.position - moon.transform.position);
			moon.transform.forward = moonDirection;

			// Directional Light의 각도 업데이트 (태양과 일치)
			Vector3 sunDirection = Vector3.Normalize(mainCamera.transform.position - sunPosition);
			sun.transform.forward = sunDirection;
		}

		// 태양 위치 계산
		Vector3 CalculateSunPosition(float time, float distance)
		{
			float currentTime = time * (Mathf.PI * 2f) / dayDuration.Value;

			float angle = currentTime;

			float solarAngle = solarAltitudeAngle.Value;

			Vector3 sunPosition = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
			Vector3 direction = Quaternion.Euler(solarAngle, 0, 0) * sunPosition;
			direction = direction.normalized;

			sunPosition  = direction * distance;

			return sunPosition;
		}
	}
}
