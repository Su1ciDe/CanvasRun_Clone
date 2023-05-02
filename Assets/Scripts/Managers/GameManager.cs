using UnityEngine;
using Utilities;

namespace Managers
{
	public class GameManager : Singleton<GameManager>
	{
		private void Awake()
		{
			Application.targetFrameRate = 60;
		}
	}
}