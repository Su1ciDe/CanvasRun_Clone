using Gameplay;
using TMPro;
using UnityEngine;

namespace Gates
{
	public abstract class BaseGate : MonoBehaviour
	{
		[SerializeField] protected int amount;
		protected int followerCount;
		[Space]
		[SerializeField] private TextMeshPro txtAmount;

		

		private void OnTriggerEnter(Collider other)
		{
			if (other.isTrigger && other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Player player))
			{
				OnEnter(player);
			}
		}

		protected abstract void OnEnter(Player player);

		protected virtual void SetAmountText()
		{
			string sign = amount >= 0 ? "+" : "-";
			txtAmount.SetText(sign + followerCount.ToString());
		}
	}
}