using GoblinGames;
using GoblinGames.Network;
using UnityEngine;

namespace Deckfense
{
    public class InGameLoading2 : MonoBehaviour
    {
        [SerializeField] private Variable<float> progressLoad;

        private float tick = 0f;
        private bool isloadingCompleted = false;

        private void Awake()
        {
            
        }

        private void OnDestroy()
        {
            
        }

        private void Update()
        {
            tick += Time.deltaTime;
            progressLoad.Value = tick / 5f;
            if (tick > 5f)
            {
                if(!isloadingCompleted)
                {
                    isloadingCompleted = true;
					SceneController.LoadScene("InGame");
				}
            }
        }

    }
}
