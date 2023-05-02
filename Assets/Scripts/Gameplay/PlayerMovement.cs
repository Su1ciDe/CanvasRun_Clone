using UnityEngine;

namespace Gameplay
{
	public class PlayerMovement : MonoBehaviour
	{
		public bool CanMoveForward { get; set; } = true;

		[SerializeField] private float moveForwardSpeed = 1;

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
			if (!CanMoveForward) return;
			var velocity = rb.velocity;
			velocity.z = moveForwardSpeed;
			rb.velocity = velocity;
			
			// transform.Translate(moveForwardSpeed * Time.deltaTime * Vector3.forward);
		}
	}
}