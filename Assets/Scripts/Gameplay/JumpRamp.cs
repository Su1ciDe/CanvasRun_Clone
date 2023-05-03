using UnityEngine;

namespace Gameplay
{
	public class JumpRamp : MonoBehaviour
	{
		[SerializeField] private float boostAmount = 1.5f;

		private void OnTriggerEnter(Collider other)
		{
			if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Player player))
			{
				player.BoostSpeed(boostAmount);
			}
		}
	}
}