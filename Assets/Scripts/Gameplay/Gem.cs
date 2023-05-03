using Stack;
using UnityEngine;

namespace Gameplay
{
	public class Gem : MonoBehaviour
	{
		[SerializeField] private int score = 1;

		private void OnTriggerEnter(Collider other)
		{
			if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Follower _))
			{
				gameObject.SetActive(false);
				Player.Instance.CollectGem(score, transform.position);
			}
		}
	}
}