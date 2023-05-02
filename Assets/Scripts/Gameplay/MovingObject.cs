using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
	public class MovingObject : MonoBehaviour
	{
		public event UnityAction OnBallAdded;
		public event UnityAction OnBallRemoved;

		public float BallSize => ballSize;
		[SerializeField] private float ballSize = .1f;

		public void AddBall()
		{
			OnBallAdded?.Invoke();
		}
	}
}