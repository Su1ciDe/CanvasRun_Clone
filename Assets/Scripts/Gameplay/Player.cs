using System.Collections;
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

		[Space]
		[SerializeField] private Collider trigger;

		private void Awake()
		{
			Input = GetComponent<IInput>();
		}

		public void BoostSpeed()
		{
			StartCoroutine(BoostCoroutine());
		}

		private IEnumerator BoostCoroutine()
		{
			trigger.enabled = false;

			yield return new WaitForSeconds(.5f);

			trigger.enabled = true;
		}
	}
}