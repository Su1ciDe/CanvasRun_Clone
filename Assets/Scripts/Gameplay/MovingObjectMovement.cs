using Controllers;
using UnityEngine;

namespace Gameplay
{
	public class MovingObjectMovement : MonoBehaviour
	{
		public bool CanMove { get; set; } = true;
		private float delta;

		private FollowerController followerController;

		private Vector3 objectPos;
		private Vector3 objectRot;
		private const float rotationMultiplier = 90;

		[SerializeField] private float leftLimit = -1;
		[SerializeField] private float rightLimit = 1;

		private void Awake()
		{
			followerController = GetComponent<FollowerController>();
		}

		private void Start()
		{
			Player.Instance.Input.OnDrag += OnInputDrag;
		}

		private void OnEnable()
		{
			followerController.OnFollowerColumnAdded += OnFollowerColumnAdded;
			followerController.OnFollowerColumnRemoved += OnFollowerColumnRemoved;
		}

		private void OnDisable()
		{
			followerController.OnFollowerColumnAdded -= OnFollowerColumnAdded;
			followerController.OnFollowerColumnRemoved -= OnFollowerColumnRemoved;
		}

		private void FixedUpdate()
		{
			Move();
			Rotate();
		}

		private void Move()
		{
			if (!CanMove) return;
			objectPos = transform.position;
			objectPos.x = Mathf.Clamp(objectPos.x + delta, leftLimit, rightLimit);
			transform.position = objectPos;
		}

		private void Rotate()
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(delta * rotationMultiplier, Vector3.up), Time.deltaTime * 10);
		}

		private void OnInputDrag(float deltaX)
		{
			delta = deltaX;
		}

		private void OnFollowerColumnAdded()
		{
			leftLimit += followerController.BallSize / 2f;
			rightLimit -= followerController.BallSize / 2f;
		}

		private void OnFollowerColumnRemoved()
		{
			leftLimit -= followerController.BallSize / 2f;
			rightLimit += followerController.BallSize / 2f;
		}
	}
}