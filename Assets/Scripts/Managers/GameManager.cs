using UnityEngine;
using Utility;

namespace Managers
{
	public class GameManager : Singleton<GameManager>
	{
		public Camera MainCamera { get; private set; }

		private void Awake()
		{
			MainCamera = Camera.main;
		}
	}
}