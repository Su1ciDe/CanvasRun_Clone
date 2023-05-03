using Controllers;
using UnityEngine;
using Utilities;

namespace Managers
{
	[DefaultExecutionOrder(-1)]
	public class GameManager : Singleton<GameManager>
	{
		public static int GemScore
		{
			get => PlayerPrefs.GetInt("GemScore", 0);
			set => PlayerPrefs.SetInt("GemScore", value);
		}

		public static Camera MainCamera;
		public CameraController MainCameraController => mainCameraController;
		[SerializeField] private CameraController mainCameraController;

		private void Awake()
		{
			Input.multiTouchEnabled = false;
			Application.targetFrameRate = 60;
			Screen.sleepTimeout = SleepTimeout.NeverSleep;

			MainCamera = Camera.main;
		}
	}
}