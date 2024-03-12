using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Deckfense
{
    public class Splash : MonoBehaviour
    {
        [Header("Fade In")]
        [SerializeField] private Image logo;
        [SerializeField] private float duration;
        [SerializeField] private float beginFadeinTime;
        [SerializeField] private float endFadeinTime;

        [SerializeField] private TMP_Text text1;
        [SerializeField] private TMP_Text text2;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float beginAudioPlayTime;

        private bool start = false;
        private float startTime;

        private bool isAudioPlay = false;

        private void Update()
        {
            if(start == false)
            {
                start = true;
                startTime = Time.time;
                return;
            }

            float elapsed = Time.time - startTime;

            float fadeinDuration = endFadeinTime - beginFadeinTime;

            if(elapsed > beginFadeinTime)
            {
                float fadeinElapsed = elapsed - beginFadeinTime;
                float alpha = fadeinElapsed / fadeinDuration;

                Color color = logo.color;
                Color color1 = text1.color;
                Color color2 = text2.color;

                color.a = alpha;
                color1.a = alpha;
                color2.a = alpha;

                logo.color = color;
                text1.color = color1;
                text2.color = color2;
            }

            if(isAudioPlay == false)
            {
                if (elapsed > beginAudioPlayTime)
                {
                    audioSource.Play();
                    isAudioPlay = true;
                }
            }

            if(Time.time >= startTime + duration)
            {
                SceneController.LoadScene("Loading");
            }

            //tick += Time.deltaTime;
            //if (tick > delay)
            //{
            //    tick = 0f;
            //    SceneController.LoadScene("Loading");

            //}
        }
    }
}
