using DG.Tweening;
using Gameplay;
using Stack;
using UnityEngine;

namespace Collectibles
{
	public class Gem : Collectible
	{
		[Space]
		[SerializeField] private float rotationSpeed = 100;

		private void Start()
		{
			transform.GetChild(0).DOLocalRotate(360 * Vector3.up, rotationSpeed, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart)
				.SetSpeedBased(true);
		}

		protected override void OnCollect(Follower follower)
		{
			base.OnCollect(follower);
			Player.Instance.CollectGem(this);
		}
	}
}