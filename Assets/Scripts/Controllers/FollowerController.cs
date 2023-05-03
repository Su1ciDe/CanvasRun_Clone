using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Stack;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Controllers
{
	public class FollowerController : MonoBehaviour
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

		[SerializeField] private Follower followerPrefab;
		[SerializeField] private Transform followerPointPrefab;
		[Space]
		[SerializeField] private int startingBallColumnCount = 4;
		[SerializeField] private int startingBallRowCount = 10;

		public float BallSize => ballSize;
		[Space]
		[SerializeField] private float ballSize = .1f;

		public event UnityAction OnFollowerColumnAdded;
		public event UnityAction OnFollowerColumnRemoved;
		public event UnityAction OnFollowerRowAdded;
		public event UnityAction OnFollowerRowRemoved;
		public event UnityAction<int> OnFollowerCountChanged; // totalFollowerCount

		private void Awake()
		{
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
			for (int i = 0; i < CurrentStackWidth; i++)
			{
				var rowCount = FollowerStack[i].Count;
				if (rowCount <= 0) continue;
				FollowerStack[i][0].transform.position = followerPoints[i].transform.position;
				for (int j = 1; j < rowCount; j++)
				{
					var previousFollower = FollowerStack[i][j - 1];
					var newPos = previousFollower.PreviousPosition;
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

			ReadjustFollowingPoints();
		}

		private Follower SpawnFollower(Vector3 pos)
		{
			var follower = Instantiate(followerPrefab, pos, Quaternion.identity);
			TotalFollowerCount++;
			return follower;
		}

		public void RemoveFollower(Follower follower)
		{
			follower.DestroySelf();
			var coordinates = FollowerStack.GetCoordinates(follower);
			FollowerStack[coordinates.x].RemoveAt(coordinates.y);
			TotalFollowerCount--;

			if (FollowerStack[coordinates.x].Count > 0)
			{
				followerPoints[coordinates.x].DOKill();
				followerPoints[coordinates.x].position = FollowerStack[coordinates.x][0].transform.position;
				followerPoints[coordinates.x].DOLocalMove(new Vector3(FindFollowerPosition(coordinates.x, CurrentStackWidth), 0, 0), .15f).SetEase(Ease.Linear)
					.SetUpdate(UpdateType.Fixed);
			}
			else
			{
				RemoveColumn(coordinates.x);
			}
		}

		private void AddColumn(int columnIndex, int columnCount, int rowCount, bool isAnimated = false)
		{
			// to choose a side for followerPoint (left/right)
			int side = columnIndex % 2 == 0 ? -1 : 1;
			var followerPointT = Instantiate(followerPointPrefab, transform);
			if (side.Equals(1))
				followerPoints.Add(followerPointT);
			else
				followerPoints.Insert(0, followerPointT);
			var posX = side * (Mathf.FloorToInt(columnCount / 2f) + 1 / 2f) * ballSize;
			if (!isAnimated)
				followerPointT.localPosition = posX * Vector3.right;
			else
				followerPointT.DOLocalMoveX(posX, 4f).SetSpeedBased(true).SetEase(Ease.Linear);

			var tempRowFollowers = new List<Follower>();
			for (int j = 0; j < rowCount; j++)
			{
				var followerPos = new Vector3(followerPointT.transform.position.x, ballSize / 2f, transform.position.z);
				// TODO: change with pooling
				var follower = SpawnFollower(followerPos);

				tempRowFollowers.Add(follower);
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

		public void RemoveColumn(int index)
		{
			FollowerStack.RemoveAt(index);
			var removedFollowerPoint = followerPoints[index];
			followerPoints.RemoveAt(index);
			removedFollowerPoint.DOKill();
			Destroy(removedFollowerPoint.gameObject);

			CurrentStackWidth--;
			OnFollowerColumnRemoved?.Invoke();

			ReadjustFollowingPoints(true);
		}

		public void AddRowByCount(int rowCount)
		{
			// row by row
			for (int i = 0; i < rowCount; i++)
			{
				for (int j = 0; j < CurrentStackWidth; j++)
				{
					var followerPos = new Vector3(transform.position.x + FindFollowerPosition(j, CurrentStackWidth) * ballSize, ballSize / 2f, 0);
					var follower = SpawnFollower(followerPos);

					FollowerStack[j].Add(follower);
				}

				CurrentStackLength++;
				OnFollowerRowAdded?.Invoke();
			}
		}

		public void RemoveRow(int index)
		{
			OnFollowerRowRemoved?.Invoke();
		}

		private void ReadjustFollowingPoints(bool isAnimated = false)
		{
			for (int i = 0; i < followerPoints.Count; i++)
			{
				float posX = FindFollowerPosition(i, CurrentStackWidth);
				if (isAnimated)
				{
					followerPoints[i].DOLocalMoveX(posX, .5f).SetEase(Ease.Linear);
				}
				else
				{
					followerPoints[i].transform.localPosition = posX * Vector3.right;
				}
			}
		}

		private float FindFollowerPosition(int index, int width)
		{
			return (index - (width / 2f - .5f)) * ballSize;
		}
	}
}