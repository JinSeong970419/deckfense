using System;
using System.Collections;
using System.Collections.Generic;
using GoblinGames;
using GoblinGames.Network;
using UnityEngine;

using UnityEngine.UI;


namespace Deckfense
{
    public class Loading : MonoBehaviour
    {
        public enum State
        {
            None = 0,

            CalculatingSize,
            NothingToDownload,

            AskingDownload,
            Downloading,
            DownloadFinished
        }

        [Serializable]
        public class Root
        {
            public State state;
            public Transform root;
        }

        [SerializeField] private Variable<float> progressLoad;

        private float loadingTimer;


        private void Update()
        {
            loadingTimer += Time.deltaTime;
            progressLoad.Value = loadingTimer / 5f;
            if(loadingTimer > 5f)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            }
        }

        public void OnClickCancelBtn()
        {
#if UNITY_EDITOR
            if (Application.isEditor)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
#else
            Application.Quit();
#endif
        }

        public void OnClickEnterGame()
        {
            Debug.Log("Start Game!");
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }


    }
}
