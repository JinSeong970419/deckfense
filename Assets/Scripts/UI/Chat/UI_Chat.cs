using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Deckfense
{
    public class UI_Chat : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TMP_Text text;


        public string Text 
        {
            get 
            {
                return text.text; 
            }

            set
            {
                text.text = value;
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, text.preferredHeight);
                
            }
        }

        private void Start()
        {
        }

        private void OnValidate()
        {
            if(rectTransform == null) rectTransform = GetComponent<RectTransform>();
        }

    }
}
