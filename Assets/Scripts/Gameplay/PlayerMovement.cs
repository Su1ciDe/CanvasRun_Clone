using UnityEngine;

namespace Gameplay
{
	public class PlayerMovement : MonoBehaviour
	{
		public bool CanMoveForward { get; set; } = true;

		[SerializeField] private float moveForwardSpeed = 1;
		
		private void Awake()
		{
		}

		private void Update()
		{
			MoveForward();
		}

		private void MoveForward()
		{
			if (!CanMoveForward) return;
			transform.Translate(moveForwardSpeed * Time.deltaTime * Vector3.forward);
		}
	}
}