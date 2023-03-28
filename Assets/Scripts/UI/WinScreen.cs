using Managers;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
	public class WinScreen : MonoBehaviour
	{
		[SerializeField] private Button btnNextLevel;
		[SerializeField] private Button btnBackToMainMenu;

		public void Init()
		{
			LevelManager.OnLevelSuccess += OnLevelSuccess;
		}

		private void OnEnable()
		{
			btnNextLevel.onClick.AddListener(NextLevel);
			btnBackToMainMenu.onClick.AddListener(BackToMainMenu);

			ParticlePooler.Instance.Spawn("Confetti", Vector3.zero);
		}

		private void OnDisable()
		{
			btnNextLevel.onClick.RemoveListener(NextLevel);
			btnBackToMainMenu.onClick.RemoveListener(BackToMainMenu);
		}

		private void OnDestroy()
		{
			LevelManager.OnLevelSuccess -= OnLevelSuccess;
		}

		private void OnLevelSuccess()
		{
			gameObject.SetActive(true);
		}

		private void NextLevel()
		{
			gameObject.SetActive(false);
			LevelManager.Instance.NextLevel();
		}

		private void BackToMainMenu()
		{
			gameObject.SetActive(false);
			UIManager.Instance.GoBackToMainMenu();
		}
	}
}