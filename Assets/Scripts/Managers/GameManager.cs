using UnityEngine;
using Utility;

namespace Managers
{
	[DefaultExecutionOrder(-2)]
	public class GameManager : Singleton<GameManager>
	{
		public Camera MainCamera { get; private set; }

		private void Awake()
		{
			MainCamera = Camera.main;
		}

		public void Win()
		{
			LevelManager.Instance.Win();
		}
	}
}