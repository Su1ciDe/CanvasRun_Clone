using Stack;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Finish
{
	public class Hole : MonoBehaviour
	{
		[SerializeField] private int multiplier;
		[Space]
		[SerializeField] private TextMeshPro txtMultiplier;

		public static event UnityAction<Follower, int> OnFollowerInHole; // multiplier

		private void Awake()
		{
			SetMultiplierText(multiplier);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Follower follower))
			{
				var multiplierUI = ObjectPooler.Instance.Spawn("MultiplierUI", follower.transform.position).GetComponent<MultiplierUI>();
				multiplierUI.SetMultiplierText(multiplier);
				multiplierUI.Show();

				OnFollowerInHole?.Invoke(follower, multiplier);
			}
		}

		private void SetMultiplierText(int _multiplier)
		{
			txtMultiplier.SetText("X" + _multiplier);
		}
	}
}