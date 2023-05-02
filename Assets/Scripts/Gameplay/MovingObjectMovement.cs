using UnityEngine;

namespace Gameplay
{
	public class MovingObjectMovement : MonoBehaviour
	{
		public bool CanMove { get; set; } = true;
		private float delta;

		private MovingObject movingObject;

		private Vector3 playerPos;

		[SerializeField] private float leftLimit = -1;
		[SerializeField] private float rightLimit = 1;

		private void Awake()
		{
			movingObject = GetComponent<MovingObject>();
		}

		private void Start()
		{
			Player.Instance.Input.OnDrag += OnInputDrag;
		}

		private void OnEnable()
		{
			movingObject.OnBallAdded += OnBallAdded;
			movingObject.OnBallRemoved += OnBallRemoved;
		}

		private void OnDisable()
		{
			movingObject.OnBallAdded -= OnBallAdded;
			movingObject.OnBallRemoved -= OnBallRemoved;
		}

		private void Update()
		{
			Move();
		}

		private void Move()
		{
			if (!CanMove) return;
			playerPos = transform.position;
			playerPos.x = Mathf.Clamp(playerPos.x + delta, leftLimit, rightLimit);
			transform.position = playerPos;
		}

		private void OnInputDrag(float deltaX)
		{
			delta = deltaX;
		}

		private void OnBallAdded()
		{
			leftLimit += movingObject.BallSize;
			rightLimit -= movingObject.BallSize;
		}

		private void OnBallRemoved()
		{
			leftLimit -= movingObject.BallSize;
			rightLimit += movingObject.BallSize;
		}
	}
}