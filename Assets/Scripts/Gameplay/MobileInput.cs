using Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
	public class MobileInput : MonoBehaviour, IInput
	{
		public bool CanInput { get; set; }

		[SerializeField] private float dragMultiplier = 1f;
		[SerializeField] private float maxDragSpeed = 1f;

		private float previousPositionX;
		private float deltaX;

		public event UnityAction<float> OnDrag;

		private void Update()
		{
			Inputs();
		}

		private void Inputs()
		{
			if (!CanInput) return;

			if (Input.touchCount > 0)
			{
				var touch = Input.GetTouch(0);
				switch (touch.phase)
				{
					case TouchPhase.Began:
						previousPositionX = touch.position.x;

						break;
					case TouchPhase.Moved:
						deltaX = touch.position.x - previousPositionX;
						OnDrag?.Invoke(Mathf.Clamp(deltaX * dragMultiplier * Time.deltaTime, -maxDragSpeed, maxDragSpeed));
						previousPositionX = touch.position.x;

						break;
					case TouchPhase.Ended:
						deltaX = 0;
						OnDrag?.Invoke(0);

						break;
				}
			}
		}
	}
}