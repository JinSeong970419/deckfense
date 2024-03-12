using System.Collections;
using System.Collections.Generic;
using GoblinGames;
using UnityEngine;
using UnityEngine.UI;

namespace Deckfense
{
    public class Progress : MonoBehaviour
    {
        [SerializeField] private Variable<float> progressLoad;
        
        private Slider progress;

        private float progressValue;
        public float ProgressValue 
        {
            get => progressValue;
            set => progressValue = progress.value = value;
        }

        private void OnEnable()
        {
            progressLoad.OnValueChanged.AddListener(ChangeProgressValue);
        }

        private void OnDisable()
        {
            progressLoad.OnValueChanged.RemoveListener(ChangeProgressValue);
        }

        private void Awake()
        {
            progress = GetComponent<Slider>();
        }

        private void ChangeProgressValue(float value)
        {
            ProgressValue = value;
        }
    }
}
