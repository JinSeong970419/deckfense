using System;
using System.Collections.Generic;
using GoblinGames;
using GoblinGames.Network;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Deckfense
{
	public enum LobbyScreenType
	{
		Shop,
		Lobby,
		Deck
	}
	public class Lobby : MonoBehaviour
	{
		[SerializeField] private LobbyUI lobbyUI;
		[SerializeField] private LobbyBottomBarUI lobbyBottomBarUI;

		private void OnEnable()
		{
			lobbyUI.SelectGameTypeUI.VersusMatchButton.onClick.AddListener(OnClickVersusMatchButton);
			lobbyUI.SelectGameTypeUI.RankingMatchButton.onClick.AddListener(OnClickRankingMatchButton);
			lobbyUI.SelectGameTypeUI.SeasonMatchButton.onClick.AddListener(OnClickSeasonMatchButton);
			lobbyUI.SelectGameTypeUI.MissionModeButton.onClick.AddListener(OnClickMissionModeButton);
			lobbyUI.SelectGameTypeUI.PracticeModeButton.onClick.AddListener(OnClickPracticeModeButton);
			lobbyUI.SelectGameTypeUI.ReturnMatchingButton.onClick.AddListener(OnClickReturnMatchingButton);


			lobbyBottomBarUI.shopButton.onClick.AddListener(() => { lobbyUI.ChangeScreen(LobbyScreenType.Shop); });
			lobbyBottomBarUI.lobbyButton.onClick.AddListener(() => { lobbyUI.ChangeScreen(LobbyScreenType.Lobby); });
			lobbyBottomBarUI.deckButton.onClick.AddListener(() => { lobbyUI.ChangeScreen(LobbyScreenType.Deck); });

		}

		private void OnDisable()
		{
			lobbyUI.SelectGameTypeUI.VersusMatchButton.onClick.RemoveListener(OnClickVersusMatchButton);
			lobbyUI.SelectGameTypeUI.RankingMatchButton.onClick.RemoveListener(OnClickRankingMatchButton);
			lobbyUI.SelectGameTypeUI.SeasonMatchButton.onClick.RemoveListener(OnClickSeasonMatchButton);
			lobbyUI.SelectGameTypeUI.MissionModeButton.onClick.RemoveListener(OnClickMissionModeButton);
			lobbyUI.SelectGameTypeUI.PracticeModeButton.onClick.RemoveListener(OnClickPracticeModeButton);
			lobbyUI.SelectGameTypeUI.ReturnMatchingButton.onClick.RemoveListener(OnClickReturnMatchingButton);


			lobbyBottomBarUI.shopButton.onClick.RemoveAllListeners();
			lobbyBottomBarUI.lobbyButton.onClick.RemoveAllListeners();
			lobbyBottomBarUI.deckButton.onClick.RemoveAllListeners();
		}

		private void Update()
		{
			
		}

		private void OpenGameMatchingPanel(MatchingType type)
		{
			lobbyUI.SelectGameTypeUI.SelectModePanel.SetActive(false);
			lobbyUI.SelectGameTypeUI.GameMatchingPanel.SetActive(true);
			lobbyUI.SelectGameTypeUI.MatchingType = type;
			lobbyUI.SelectGameTypeUI.MatchingPanelTopBar.transform.GetChild((int)type).gameObject.SetActive(true);
		}

		
		private void OnClickVersusMatchButton()
		{
			OpenGameMatchingPanel(MatchingType.VersusMatch);
		}

		private void OnClickRankingMatchButton()
		{
			OpenGameMatchingPanel(MatchingType.RankingMatch);
		}

		private void OnClickSeasonMatchButton()
		{
			OpenGameMatchingPanel(MatchingType.SeasonMatch);
		}

		private void OnClickMissionModeButton()
		{
			OpenGameMatchingPanel(MatchingType.MissionMode);
		}

		private void OnClickPracticeModeButton()
		{
			OpenGameMatchingPanel(MatchingType.PracticeMode);
		}

		private void OnClickReturnMatchingButton()
		{
			lobbyUI.SelectGameTypeUI.SelectModePanel.SetActive(true);
			lobbyUI.SelectGameTypeUI.GameMatchingPanel.SetActive(false);
			lobbyUI.SelectGameTypeUI.MatchingPanelTopBar.transform.GetChild((int)lobbyUI.SelectGameTypeUI.MatchingType).gameObject.SetActive(false);
		}

	}
}
