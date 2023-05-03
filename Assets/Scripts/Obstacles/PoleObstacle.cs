using Gameplay;
using Stack;

namespace Obstacles
{
	public class PoleObstacle : BaseObstacle
	{
		protected override void OnCollide(Follower follower)
		{
			Player.Instance.MovingObject.RemoveFollower(follower);
		}
	}
}