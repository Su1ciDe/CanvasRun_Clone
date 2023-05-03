using Stack;
using UnityEngine;

namespace Collectibles
{
	public abstract class Collectible : MonoBehaviour
	{
		public int Score => score;
		[SerializeField] private int score = 1;

		protected virtual void OnTriggerEnter(Collider other)
		{
			if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Follower follower))
			{
				OnCollect(follower);
			}
		}

		protected virtual void OnCollect(Follower follower)
		{
			gameObject.SetActive(false);
		}
	}
}