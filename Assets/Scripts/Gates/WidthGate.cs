using Gameplay;

namespace Gates
{
	public class WidthGate : BaseGate
	{
		private void OnEnable()
		{
			Player.Instance.MovingObject.OnFollowerRowAdded += OnFollowerRowAdded;
			Player.Instance.MovingObject.OnFollowerRowRemoved += OnFollowerRowRemoved;
		}

		private void OnDisable()
		{
			if (Player.Instance)
			{
				Player.Instance.MovingObject.OnFollowerRowAdded -= OnFollowerRowAdded;
				Player.Instance.MovingObject.OnFollowerRowRemoved -= OnFollowerRowRemoved;
			}
		}

		protected override void OnEnter(Player player)
		{
			player.MovingObject.AddColumByCount(amount);
			player.BoostSpeed();
			gameObject.SetActive(false);
		}

		protected override void SetAmountText()
		{
			followerCount = amount * Player.Instance.MovingObject.CurrentStackLength;
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