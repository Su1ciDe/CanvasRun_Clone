using Interfaces;
using UnityEngine;
using Utilities;

namespace Gameplay
{
	public class Player : Singleton<Player>
	{
		public IInput Input { get; private set; }

		public MovingObject MovingObject => movingObject;
		[SerializeField] private MovingObject movingObject;

		private void Awake()
		{
			Input = GetComponent<IInput>();
		}
	}
}