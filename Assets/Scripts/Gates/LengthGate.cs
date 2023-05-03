using Gameplay;
using UnityEngine;

namespace Gates
{
	public class LengthGate : BaseGate
	{
		private void OnEnable()
		{
			Player.Instance.MovingObject.OnFollowerColumnAdded += OnFollowerColumnAdded;
			Player.Instance.MovingObject.OnFollowerColumnRemoved += OnFollowerColumnRemoved;
		}

		private void OnDisable()
		{
			if (Player.Instance)
			{
				Player.Instance.MovingObject.OnFollowerColumnAdded -= OnFollowerColumnAdded;
				Player.Instance.MovingObject.OnFollowerColumnRemoved -= OnFollowerColumnRemoved;
			}
		}

		protected override void OnEnter(Player player)
		{
			player.MovingObject.AddRowByCount(amount);
			player.BoostSpeed();
			gameObject.SetActive(false);
		}

		protected override void SetAmountText()
		{
			followerCount = amount * Player.Instance.MovingObject.CurrentStackWidth;
			base.SetAmountText();
		}

		private void OnFollowerColumnAdded()
		{
			SetAmountText();
		}

		private void OnFollowerColumnRemoved()
		{
			SetAmountText();
		}
	}
}