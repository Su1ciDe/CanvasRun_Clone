using System.Collections;
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

		private int totalFollowerCount;
		public int TotalFollowerCount
		{
			get => totalFollowerCount;
			private set
			{
				totalFollowerCount = value;
				OnFollowerCountChanged?.Invoke(TotalFollowerCount);
			}
		}
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
			StartCoroutine(InitFollowerStack());
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

		private IEnumerator InitFollowerStack()
		{
			yield return null;

			for (int i = 0; i < startingBallColumnCount; i++)
			{
				AddColumn(i, CurrentStackWidth, startingBallRowCount);
			}

			CurrentStackLength = startingBallRowCount;
			OnFollowerRowAdded?.Invoke();
		}

		private void AddColumn(int columnIndex, int columnCount, int rowCount, bool isAnimated = false)
		{
			// to choose a side for followerPoint (left/right)
			int side = columnIndex % 2 == 0 ? -1 : 1;
			var followerPointT = Instantiate(followerPoint, transform);
			followerPoints.Add(followerPointT);
			var posX = side * Mathf.FloorToInt(columnCount / 2f) * ballSize;
			if (!isAnimated)
				followerPointT.localPosition = posX * Vector3.right;
			else
				followerPointT.DOLocalMoveX(posX, 3f).SetSpeedBased(true).SetEase(Ease.Linear);

			var tempRowFollowers = new List<Follower>();
			for (int j = 0; j < rowCount; j++)
			{
				var followerPos = new Vector3(followerPointT.transform.position.x, ballSize / 2f, transform.position.z);
				// TODO: change with pooling
				var follower = Instantiate(followerPrefab, followerPos, Quaternion.identity);

				tempRowFollowers.Add(follower);
				TotalFollowerCount++;
			}

			if (side.Equals(1))
				FollowerStack.Add(tempRowFollowers);
			else
				FollowerStack.Insert(0, tempRowFollowers);

			CurrentStackWidth++;
			OnFollowerColumnAdded?.Invoke();
		}

		public void AddColumByCount(int amount)
		{
			int columnCount = CurrentStackWidth + amount;
			for (int i = CurrentStackWidth; i < columnCount; i++)
			{
				AddColumn(i, CurrentStackWidth, CurrentStackLength, true);
			}
		}

		public void RemoveColumn()
		{
			OnFollowerColumnRemoved?.Invoke();
		}

		public void RemoveColumnAt(int columnIndex)
		{
			OnFollowerColumnRemoved?.Invoke();
		}

		public void AddRowByCount(int rowCount)
		{
			// row by row
			for (int i = 0; i < rowCount; i++)
			{
				for (int j = 0; j < CurrentStackWidth; j++)
				{
					var followerPos = new Vector3(transform.position.x + (j - CurrentStackWidth / 2f + ballSize) * ballSize, ballSize / 2f, 0);
					var follower = Instantiate(followerPrefab, followerPos, Quaternion.identity);

					TotalFollowerCount++;

					FollowerStack[j].Add(follower);
				}

				CurrentStackLength++;
				OnFollowerRowAdded?.Invoke();
			}
		}

		public void RemoveRow()
		{
			OnFollowerRowRemoved?.Invoke();
		}
	}
}