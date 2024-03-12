using GoblinGames;
using UnityEngine;

namespace Deckfense
{
    public class InGameLoading : MonoBehaviour
    {
        [SerializeField] private Variable<float> progressLoad;

        private float loadingTimer;
        private bool isloadingCompleted = false;

        private void Awake()
        {

        }

        private void OnDestroy()
        {

        }

        private void Update()
        {
            loadingTimer += Time.deltaTime;
            progressLoad.Value = loadingTimer / 5f;
            if (loadingTimer > 5f)
            {
                SceneController.LoadScene("InGame");
            }
        }

    }
}
