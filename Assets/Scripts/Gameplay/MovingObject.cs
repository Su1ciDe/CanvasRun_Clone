using System.Collections.Generic;
using DG.Tweening;
using Stack;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
	public class MovingObject : MonoBehaviour
	{
		public List<List<Follower>> FollowerStack = new List<List<Follower>>();
		private List<Transform> followerPoints = new List<Transform>();

		public int TotalFollowerCount { get; private set; }
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

		public event UnityAction OnFollowerColumnAdded;
		public event UnityAction OnFollowerColumnRemoved;
		public event UnityAction OnFollowerRowAdded;
		public event UnityAction OnFollowerRowRemoved;
		public event UnityAction<int> OnFollowerCountChanged; // totalFollowerCount

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
				AddColumn(i, startingBallColumnCount, startingBallRowCount);
			}

			CurrentStackWidth = startingBallColumnCount;
			CurrentStackLength = startingBallRowCount;
		}

		public void AddColumn(int columnIndex, int columnCount, int rowCount, bool isAnimated = false)
		{
			var followerPointT = Instantiate(followerPoint, transform);
			followerPoints.Add(followerPointT);
			if (!isAnimated)
			{
				followerPointT.localPosition = new Vector3((columnIndex - columnCount / 2f + ballSize) * ballSize, 0, 0);
			}
			else
			{
				int side = columnIndex < CurrentStackWidth / 2f ? -1 : 1;
				followerPointT.DOLocalMoveX(side * (columnIndex - columnCount / 2f + ballSize), 1).SetSpeedBased(true).SetEase(Ease.Linear);
			}

			var tempRowFollowers = new List<Follower>();
			for (int j = 0; j < rowCount; j++)
			{
				var followerPos = new Vector3(transform.position.x + (columnIndex - columnCount / 2f + ballSize) * ballSize, ballSize / 2f, transform.position.z);
				// TODO: change with pooling
				var follower = Instantiate(followerPrefab, followerPos, Quaternion.identity);
				tempRowFollowers.Add(follower);

				TotalFollowerCount++;
				OnFollowerCountChanged?.Invoke(TotalFollowerCount);
			}

			FollowerStack.Add(tempRowFollowers);

			OnFollowerColumnAdded?.Invoke();
		}

		public void AddColumByCount(int amount)
		{
		}

		public void RemoveColumn()
		{
			OnFollowerColumnRemoved?.Invoke();
		}

		public void RemoveColumnAt(int columnIndex)
		{
			OnFollowerColumnRemoved?.Invoke();
		}

		public void AddRowByCount(int amount)
		{
			int rowCount = amount / CurrentStackWidth;
			int remainder = amount % CurrentStackWidth;
			rowCount = remainder.Equals(0) ? rowCount : rowCount - 1;

			// row by row
			for (int i = 0; i < rowCount; i++)
			{
				for (int j = 0; j < CurrentStackWidth; j++)
				{
					var followerPos = new Vector3(transform.position.x + (j - CurrentStackWidth / 2f + ballSize) * ballSize, ballSize / 2f, 0);
					var follower = Instantiate(followerPrefab, followerPos, Quaternion.identity);

					TotalFollowerCount++;
					OnFollowerCountChanged?.Invoke(TotalFollowerCount);

					FollowerStack[j].Add(follower);
				}

				OnFollowerRowAdded?.Invoke();
			}

			// add the remainder
			for (int i = 0; i < remainder; i++)
			{
				var followerPos = new Vector3(transform.position.x + (i - CurrentStackWidth / 2f + ballSize) * ballSize, ballSize / 2f, 0);
				var follower = Instantiate(followerPrefab, followerPos, Quaternion.identity);

				TotalFollowerCount++;
				OnFollowerCountChanged?.Invoke(TotalFollowerCount);

				FollowerStack[i].Add(follower);
			}
		}

		public void RemoveRow()
		{
		}
	}
}