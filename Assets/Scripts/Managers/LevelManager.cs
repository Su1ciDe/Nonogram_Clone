using LevelSystem;
using UnityEngine;
using UnityEngine.Events;
using Utility;

namespace Managers
{
	[DefaultExecutionOrder(-2)]
	public class LevelManager : Singleton<LevelManager>
	{
		public Level CurrentLevel { get; private set; }

		/// <summary>
		/// Number of the level currently played. This value is not modulated.
		/// </summary>
		public static int LevelNo
		{
			get => PlayerPrefs.GetInt("LevelNo", 0);
			set => PlayerPrefs.SetInt("LevelNo", value);
		}

		private int levelCount;

		public static event UnityAction<Level> OnLevelLoad;
		public static event UnityAction OnLevelUnload;
		public static event UnityAction OnLevelStart;
		public static event UnityAction OnLevelSuccess;

		private void Awake()
		{
			var levels = Resources.LoadAll("Levels");
			levelCount = levels.Length;
		}

		public void LoadCurrentLevel()
		{
			int levelIndex = LevelNo % levelCount;
			CurrentLevel = new Level(LevelSystem.LevelSystem.Load(levelIndex), LevelNo);
			LoadLevel();
		}

		private void LoadLevel()
		{
			OnLevelLoad?.Invoke(CurrentLevel);
			StartLevel();
		}

		public void StartLevel()
		{
			OnLevelStart?.Invoke();
		}

		public void NextLevel()
		{
			UnloadLevel();

			LoadCurrentLevel();
		}

		public void UnloadLevel()
		{
			OnLevelUnload?.Invoke();
			CurrentLevel = null;
		}

		public void Win()
		{
			LevelNo++;

			OnLevelSuccess?.Invoke();

			Debug.Log("win");
		}
	}
}