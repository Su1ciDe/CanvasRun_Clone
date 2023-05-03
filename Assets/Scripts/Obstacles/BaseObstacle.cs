using Stack;
using UnityEngine;

namespace Obstacles
{
	[RequireComponent(typeof(Rigidbody))]
	public abstract class BaseObstacle : MonoBehaviour
	{
		protected virtual void OnCollisionEnter(Collision other)
		{
			if (other.rigidbody && other.rigidbody.TryGetComponent(out Follower follower))
			{
				// OnCollide(follower);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Follower follower))
			{
				OnCollide(follower);

			}
		}

		protected abstract void OnCollide(Follower follower);
	}
}