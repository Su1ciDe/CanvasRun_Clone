using Gameplay;

namespace Gates
{
	public class WidthGate : BaseGate
	{
		private void OnEnable()
		{
			Player.Instance.FollowerController.OnFollowerRowAdded += OnFollowerRowAdded;
			Player.Instance.FollowerController.OnFollowerRowRemoved += OnFollowerRowRemoved;
		}

		private void OnDisable()
		{
			if (Player.Instance)
			{
				Player.Instance.FollowerController.OnFollowerRowAdded -= OnFollowerRowAdded;
				Player.Instance.FollowerController.OnFollowerRowRemoved -= OnFollowerRowRemoved;
			}
		}

		protected override void OnEnter(Player player)
		{
			base.OnEnter(player);

			if (amount > 0)
				player.FollowerController.AddColumByCount(amount);
			else if (amount < 0)
				player.FollowerController.RemoveColumByCount(amount);
		}

		protected override void SetAmountText()
		{
			followerCount = amount * Player.Instance.FollowerController.CurrentStackLength;
			base.SetAmountText();
		}

		private void OnFollowerRowAdded()
		{
			SetAmountText();
		}

		private void OnFollowerRowRemoved()
		{
			SetAmountText();
		}
	}
}