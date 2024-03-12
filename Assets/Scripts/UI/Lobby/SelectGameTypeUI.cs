using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GoblinGames;
using Protocol.Network;

namespace Deckfense
{
    public enum MatchingType
    {
        VersusMatch,
        RankingMatch,
        SeasonMatch,
        MissionMode,
        PracticeMode,
    }

    public class SelectGameTypeUI : MonoBehaviour
    {
        //[SerializeField] private Button singleGameButton;
        [SerializeField] private GameEvent<object> requestEnterGameEvent;

        [Header("SelectModePanel")]
        [SerializeField] private GameObject selectModePanel;
        [SerializeField] private Button versusMatchButton;
        [SerializeField] private Button rankingMatchButton;
        [SerializeField] private Button seasonMatchButton;
        [SerializeField] private Button missionModeButton;
        [SerializeField] private Button practiceModeButton;
        private MatchingType matchingType;

        [Header("GameMatchingPanel")]
        [SerializeField] private GameObject gameMatchingPanel;
        [SerializeField] private GameObject matchingPanelTopBar;
        [SerializeField] private Button returnMatchingButton;
        [SerializeField] private Button gameMatchingButton;
        [SerializeField] private TMP_Text matchingStateText;
        [SerializeField] private TMP_Text gameMatchingButtonText;
        [SerializeField] private TMP_Text selectedDeckText;


        #region Property

        //public Button SingleGameButton { get { return singleGameButton; } }

        public GameObject SelectModePanel { get { return selectModePanel; } }
        public Button VersusMatchButton { get { return versusMatchButton; } }
        public Button RankingMatchButton { get { return rankingMatchButton; } }
        public Button SeasonMatchButton { get { return seasonMatchButton; } }
        public Button MissionModeButton { get { return missionModeButton; } }
        public Button PracticeModeButton { get { return practiceModeButton; } }
        public MatchingType MatchingType { get { return matchingType; } set { matchingType = value; } }

        public GameObject GameMatchingPanel { get { return gameMatchingPanel; } }
        public GameObject MatchingPanelTopBar { get { return matchingPanelTopBar; } }
        public Button ReturnMatchingButton { get { return returnMatchingButton; } }
        public Button GameMatchingButton { get { return gameMatchingButton; } }
        public TMP_Text MatchingStateText { get {  return matchingStateText; } }
        public TMP_Text GameMatchingButtonText { get {  return gameMatchingButtonText; } }
        public TMP_Text SelectedDeckText { get {  return selectedDeckText; } }

		#endregion

		private void OnEnable()
		{
            gameMatchingButton.onClick.AddListener(OnEnterGameButtonClick);
		}

		private void OnDisable()
		{
			gameMatchingButton.onClick.RemoveListener(OnEnterGameButtonClick);
		}

        private void OnEnterGameButtonClick()
        {
            requestEnterGameEvent.Invoke(new RequestEnterGame()
            {
                Type = MessageType.RequestEnterGame,
            });

            SceneController.LoadScene("InGame");
		}
	}
}
