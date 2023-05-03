using Controllers;
using UnityEngine;
using Utilities;

namespace Managers
{
	public class GameManager : Singleton<GameManager>
	{
		public int GemScore
		{
			get => PlayerPrefs.GetInt("GemScore", 0);
			set => PlayerPrefs.SetInt("GemScore", value);
		}

		public static Camera MainCamera;
		public CameraController MainCameraController => mainCameraController;
		[SerializeField] private CameraController mainCameraController;

		private void Awake()
		{
			Application.targetFrameRate = 60;
			
			MainCamera = Camera.main;
		}
	}
}