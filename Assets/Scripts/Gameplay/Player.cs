using LevelSystem;
using Managers;
using Utility;

namespace Gameplay
{
	public class Player : Singleton<Player>
	{
		public bool CanPlay { get; set; } = true;

		private void OnEnable()
		{
			LevelManager.OnLevelLoad += OnLevelLoaded;
			LevelManager.OnLevelStart += OnLevelStarted;
			LevelManager.OnLevelSuccess += OnLevelWon;
		}

		private void OnDisable()
		{
			LevelManager.OnLevelLoad -= OnLevelLoaded;
			LevelManager.OnLevelStart -= OnLevelStarted;
			LevelManager.OnLevelSuccess -= OnLevelWon;
		}

		private void OnLevelLoaded(Level level)
		{
		}

		private void OnLevelStarted()
		{
			CanPlay = true;
		}

		private void OnLevelWon()
		{
			CanPlay = false;
		}
	}
}