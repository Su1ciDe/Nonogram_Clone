using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField] private Button btnPlayLevels;
		[SerializeField] private Button btnPlayRandom;

		private void Awake()
		{
			btnPlayLevels.onClick.AddListener(PlayLevels);
		}

		private void OnDestroy()
		{
			btnPlayLevels.onClick.RemoveListener(PlayLevels);
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
	}
}