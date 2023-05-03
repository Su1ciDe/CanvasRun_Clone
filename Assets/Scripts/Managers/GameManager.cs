using Controllers;
using UnityEngine;
using Utilities;

namespace Managers
{
	public class GameManager : Singleton<GameManager>
	{
		public CameraController CameraController => cameraController;
		[SerializeField] private CameraController cameraController;

		private void Awake()
		{
			Application.targetFrameRate = 60;
		}
	}
}