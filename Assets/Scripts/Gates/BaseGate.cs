using Gameplay;
using TMPro;
using UnityEngine;

namespace Gates
{
	public abstract class BaseGate : MonoBehaviour
	{
		[SerializeField] protected int amount;
		[Space]
		[SerializeField] private TextMeshPro txtAmount;

		private void Awake()
		{
			SetAmountText();
		}

		private void OnValidate()
		{
			SetAmountText();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.isTrigger && other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Player player))
			{
				OnEnter(player);
			}
		}

		protected abstract void OnEnter(Player player);

		private void SetAmountText()
		{
			string sign = amount >= 0 ? "+" : "-";
			txtAmount.SetText(sign + amount.ToString());
		}
	}
}