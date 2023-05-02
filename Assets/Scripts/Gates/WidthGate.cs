using Gameplay;
using UnityEngine;

namespace Gates
{
	public class WidthGate : BaseGate
	{
		protected override void OnEnter(Player player)
		{
			player.MovingObject.AddColumByCount(amount);
			player.BoostSpeed();
			gameObject.SetActive(false);
		}
	}
}