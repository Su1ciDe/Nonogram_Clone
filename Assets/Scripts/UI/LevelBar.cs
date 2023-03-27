using LevelSystem;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
	public class LevelBar : MonoBehaviour
	{
		[SerializeField] private TMP_Text txtLevelNo;

		private void OnEnable()
		{
			LevelManager.OnLevelLoad += OnLevelLoaded;
		}

		private void OnDisable()
		{
			LevelManager.OnLevelLoad -= OnLevelLoaded;
		}

		private void OnLevelLoaded(Level level)
		{
			txtLevelNo.SetText("LEVEL " + (level.LevelNo + 1).ToString());
		}
	}
}