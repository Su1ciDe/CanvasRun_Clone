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

		protected const float boostAmount = 1.5f;

		private void OnTriggerEnter(Collider other)
		{
			if (other.isTrigger && other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Player player))
			{
				OnEnter(player);
			}
		}

		protected virtual void OnEnter(Player player)
		{
			player.BoostSpeed(boostAmount);
			gameObject.SetActive(false);
		}

		protected virtual void SetAmountText()
		{
			string sign = amount >= 0 ? "+" : "-";
			txtAmount.SetText(sign + followerCount.ToString());
		}
	}
}