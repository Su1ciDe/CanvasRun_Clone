using UnityEngine;

namespace Stack
{
	public class FollowPointGenerator : MonoBehaviour
	{
		public Vector3[] FollowPoints { get; private set; }

		private void FixedUpdate()
		{
			UpdatePoints();
		}

		public void Init(int size)
		{
			ClearPointList(size);
		}

		public void UpdatePoints()
		{
			if (FollowPoints is null) return;
			for (int i = 1; i < FollowPoints.Length; i++)
				FollowPoints[i - 1] = FollowPoints[i];

			FollowPoints[^1] = transform.position;
		}

		public void ClearPointList(int size)
		{
			FollowPoints = new Vector3[size];
			for (int i = 0; i < FollowPoints.Length; i++)
				FollowPoints[i] = transform.position;
		}
	}
}