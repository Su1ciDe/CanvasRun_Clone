using Gameplay;
using UnityEngine;

namespace Gates
{
	public class LengthGate : BaseGate
	{
		private void OnEnable()
		{
			Player.Instance.FollowerController.OnFollowerColumnAdded += OnFollowerColumnAdded;
			Player.Instance.FollowerController.OnFollowerColumnRemoved += OnFollowerColumnRemoved;
		}

		private void OnDisable()
		{
			if (Player.Instance)
			{
				Player.Instance.FollowerController.OnFollowerColumnAdded -= OnFollowerColumnAdded;
				Player.Instance.FollowerController.OnFollowerColumnRemoved -= OnFollowerColumnRemoved;
			}
		}

		protected override void OnEnter(Player player)
		{
			base.OnEnter(player);
			
			player.FollowerController.AddRowByCount(amount);
		}

		protected override void SetAmountText()
		{
			followerCount = amount * Player.Instance.FollowerController.CurrentStackWidth;
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