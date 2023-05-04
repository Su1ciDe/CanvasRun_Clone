using Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
	public class TestInput : MonoBehaviour, IInput
	{
		public bool CanInput { get; set; } = true;
		public event UnityAction<float> OnDrag;

		[SerializeField] private float dragMultiplier = 1f;
		[SerializeField] private float maxDragSpeed = 1f;

		private float previousPositionX;
		private float deltaX;

		private void Update()
		{
			Inputs();
		}

		private void Inputs()
		{
			if (!CanInput) return;

			if (Input.GetMouseButtonDown(0))
			{
				previousPositionX = Input.mousePosition.x;
			}

			if (Input.GetMouseButton(0))
			{
				deltaX = Input.mousePosition.x - previousPositionX;
				OnDrag?.Invoke(Mathf.Clamp(deltaX * dragMultiplier * Time.deltaTime, -maxDragSpeed, maxDragSpeed));

				previousPositionX = Input.mousePosition.x;
			}

			if (Input.GetMouseButtonUp(0))
			{
				deltaX = 0;
				OnDrag?.Invoke(0);
			}
		}
	}
}