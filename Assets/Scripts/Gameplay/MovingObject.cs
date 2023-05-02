using System.Collections.Generic;
using Stack;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
	public class MovingObject : MonoBehaviour
	{
		public List<List<Follower>> FollowerStack = new List<List<Follower>>();
		private List<Transform> followerPoints = new List<Transform>();

		public int CurrentStackWidth { get; private set; }
		public int CurrentStackLength { get; private set; }

		[SerializeField] private Transform followerPoint;
		[SerializeField] private Follower followerPrefab;
		[Space]
		[SerializeField] private int startingBallColumnCount = 4;
		[SerializeField] private int startingBallRowCount = 10;

		public float BallSize => ballSize;
		[Space]
		[SerializeField] private float ballSize = .1f;

		private FollowPointGenerator followPointGenerator;

		public event UnityAction OnBallAdded;
		public event UnityAction OnBallRemoved;

		private void Awake()
		{
			followPointGenerator = GetComponent<FollowPointGenerator>();

			transform.localPosition = new Vector3(0, ballSize / 2f, 0);
		}

		private void Start()
		{
			InitFollowerStack();
		}

		private void FixedUpdate()
		{
			Follow();
		}

		private void Follow()
		{
			int columCount = FollowerStack.Count;
			for (int i = 0; i < columCount; i++)
			{
				// FollowerStack[i][0].transform.position = new Vector3(transform.position.x + (i - columCount / 2f + ballSize) * ballSize, transform.position.y, transform.position.z);
				FollowerStack[i][0].transform.position = followerPoints[i].transform.position;
				for (int j = 1; j < FollowerStack[i].Count; j++)
				{
					var previousFollower = FollowerStack[i][j - 1]; 
					var newPos = previousFollower.PreviousPosition;
					// newPos.z = previousFollower.transform.position.z - ballSize;
					FollowerStack[i][j].transform.position = newPos;
				}
			}
		}

		private void InitFollowerStack()
		{
			for (int i = 0; i < startingBallColumnCount; i++)
			{
				AddColumnFollower(i, startingBallRowCount);
			}

			CurrentStackWidth = startingBallColumnCount;
			CurrentStackLength = startingBallRowCount;
		}

		public void AddColumnFollower(int columnIndex, int rowCount, bool isAnimated = false)
		{
			var followerPointT = Instantiate(followerPoint,
				new Vector3(transform.position.x + (columnIndex - startingBallColumnCount / 2f + ballSize) * ballSize, ballSize / 2f, transform.position.z),
				Quaternion.identity, transform);
			followerPoints.Add(followerPointT);

			var tempRowFollowers = new List<Follower>();
			for (int j = 0; j < rowCount; j++)
			{
				var followerPos = new Vector3(transform.position.x + (columnIndex - startingBallColumnCount / 2f + ballSize) * ballSize, ballSize / 2f, transform.position.z);
				// TODO: change with pooling
				var follower = Instantiate(followerPrefab, followerPos, Quaternion.identity);
				tempRowFollowers.Add(follower);
			}

			FollowerStack.Add(tempRowFollowers);

			OnBallAdded?.Invoke();
		}

		public void RemoveBall()
		{
			OnBallRemoved?.Invoke();
		}
	}
}