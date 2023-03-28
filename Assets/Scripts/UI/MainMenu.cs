using System;
using System.Collections.Generic;
using LevelSystem;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField] private Button btnPlayLevels;
		[SerializeField] private Button btnPlayRandom;
		[Space]
		[SerializeField] private TMP_Dropdown drpDifficulty;
		[SerializeField] private TMP_Dropdown drpGridSize;

		private void Awake()
		{
			btnPlayLevels.onClick.AddListener(PlayLevels);
			btnPlayRandom.onClick.AddListener(PlayRandomLevel);

			PopulateDropdownWithEnum(drpDifficulty, typeof(Difficulty));
			drpDifficulty.value = 1;

			PopulateDropdownWithEnum(drpGridSize, typeof(GridSize));
		}

		private void OnDestroy()
		{
			btnPlayLevels.onClick.RemoveListener(PlayLevels);
			btnPlayRandom.onClick.RemoveListener(PlayRandomLevel);
		}

		public void Open()
		{
			gameObject.SetActive(true);
		}

		public void Close()
		{
			gameObject.SetActive(false);
		}

		private void PlayLevels()
		{
			UIManager.Instance.ShowPlayLevelUI();
			LevelManager.Instance.LoadCurrentLevel();
		}

		private void PlayRandomLevel()
		{
			UIManager.Instance.ShowRandomLevelUI();
			var level = ProceduralLevel.GenerateLevel((Difficulty)drpDifficulty.value, ((GridSize)drpGridSize.value).ToInt());
			LevelManager.Instance.LoadProceduralLevel(level, (Difficulty)drpDifficulty.value);
		}

		private void PopulateDropdownWithEnum(TMP_Dropdown dropdown, Type enumType)
		{
			var newOptions = new List<TMP_Dropdown.OptionData>();
			var enums = Enum.GetNames(enumType);
			for (int i = 0; i < enums.Length; i++)
				newOptions.Add(new TMP_Dropdown.OptionData(enums[i].Replace("_", "")));
			dropdown.ClearOptions();
			dropdown.AddOptions(newOptions);
		}
	}
}