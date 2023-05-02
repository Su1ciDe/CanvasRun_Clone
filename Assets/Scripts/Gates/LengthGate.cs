using Gameplay;
using UnityEngine;

namespace Gates
{
	public class LengthGate : BaseGate
	{
		protected override void OnEnter(Player player)
		{
			player.MovingObject.AddRowByCount(amount);
			player.BoostSpeed();
			gameObject.SetActive(false);
		}
	}
}