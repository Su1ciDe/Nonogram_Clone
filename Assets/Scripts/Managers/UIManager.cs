﻿using UI;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Managers
{
	public class UIManager : Singleton<UIManager>
	{
		public MainMenu MainMenu => mainMenu;
		public Grid.Grid Grid => grid;
		public LevelBar LevelBar => levelBar;
		public WinScreen WinScreen => winScreen;

		[SerializeField] private Grid.Grid grid;
		[SerializeField] private MainMenu mainMenu;
		[SerializeField] private LevelBar levelBar;
		[SerializeField] private Button backToMainMenu;
		[SerializeField] private Button hint;
		[SerializeField] private WinScreen winScreen;

		private void Awake()
		{
			backToMainMenu.onClick.AddListener(GoBackToMainMenu);
			hint.onClick.AddListener(grid.ShowHint);
			WinScreen.Init();
		}

		private void OnDestroy()
		{
			backToMainMenu.onClick.RemoveListener(GoBackToMainMenu);
			hint.onClick.RemoveListener(grid.ShowHint);
		}

		public void GoBackToMainMenu()
		{
			LevelManager.Instance.UnloadLevel();
			OpenMainMenu();
		}

		public void OpenMainMenu()
		{
			MainMenu.Open();
			LevelBar.gameObject.SetActive(false);
			Grid.gameObject.SetActive(false);
			backToMainMenu.gameObject.SetActive(false);
			hint.gameObject.SetActive(false);
		}

		public void ShowPlayLevelUI()
		{
			MainMenu.Close();
			Grid.gameObject.SetActive(true);
			LevelBar.gameObject.SetActive(true);
			backToMainMenu.gameObject.SetActive(true);
			hint.gameObject.SetActive(true);
		}

		public void ShowRandomLevelUI()
		{
			MainMenu.Close();
			Grid.gameObject.SetActive(true);
			backToMainMenu.gameObject.SetActive(true);
			hint.gameObject.SetActive(true);
		}
	}
}