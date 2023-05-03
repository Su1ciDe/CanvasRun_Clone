using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
	public class PlayerMovement : MonoBehaviour
	{
		public bool CanMoveForward { get; set; } = true;

		[SerializeField] private float moveForwardSpeed = 1;
		[Header("Boost")]
		[SerializeField] private float boostDuration = 1;

		private float multiplier = 1;

		private Rigidbody rb;

		private void Awake()
		{
			rb = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			MoveForward();
		}

		private void MoveForward()
		{
			if (!CanMoveForward)
			{
				rb.velocity = Vector3.zero;
				return;
			}

			var velocity = rb.velocity;
			velocity.z = moveForwardSpeed * multiplier;
			rb.velocity = velocity;

			// transform.Translate(moveForwardSpeed * Time.deltaTime * Vector3.forward);
		}

		public void BoostSpeed(float boostAmount)
		{
			transform.DOKill();

			multiplier = boostAmount;

			DOTween.To(() => multiplier, x => multiplier = x, 1, boostDuration * 2).SetDelay(boostDuration).SetEase(Ease.InOutSine).SetTarget(transform);
		}
	}
}