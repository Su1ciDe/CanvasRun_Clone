using Interfaces;
using Utilities;

namespace Gameplay
{
	public class Player : Singleton<Player>
	{
		public IInput Input { get; private set; }
		public MovingObject MovingObject { get; private set; }

		private void Awake()
		{
			Input = GetComponent<IInput>();
			MovingObject = GetComponentInChildren<MovingObject>();
		}

		public void BoostSpeed()
		{
		}
	}
}