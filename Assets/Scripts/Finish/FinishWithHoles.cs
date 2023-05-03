using System.Collections;
using Cinemachine;
using Controllers;
using Gameplay;
using Managers;
using Stack;
using UnityEngine;

namespace Finish
{
	public class FinishWithHoles : MonoBehaviour
	{
		[Header("Camera")]
		[SerializeField] private CinemachineVirtualCamera vcamFinish;
		[SerializeField] private CinemachineTargetGroup targetGroup;

		private bool isFinished;

		private const float DELAY = .1f;
		private WaitForSeconds waitDelay;
		private const float FORCE_MULTIPLIER = 15f;

		private const float FINISH_TIMER = 5;
		private WaitForSeconds waitForFinish;
		private Coroutine waitForFinishCoroutine;

		private void Awake()
		{
			waitDelay = new WaitForSeconds(DELAY);
			waitForFinish = new WaitForSeconds(FINISH_TIMER);
		}

		private void OnEnable()
		{
			Hole.OnFollowerInHole += OnFollowerInHole;
		}

		private void OnDisable()
		{
			Hole.OnFollowerInHole -= OnFollowerInHole;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Player player))
			{
				player.FinishLine();
				vcamFinish.gameObject.SetActive(true);

				StartCoroutine(ShootFollowers(player.FollowerController));
			}
		}

		private IEnumerator ShootFollowers(FollowerController followerController)
		{
			int length = followerController.CurrentStackLength;
			int width = followerController.CurrentStackWidth;
			float force = followerController.TotalFollowerCount;
			for (int i = 0; i < length; i++)
			{
				yield return waitDelay;

				for (int j = 0; j < width; j++)
				{
					if (followerController.FollowerStack[j].Count <= 0) continue;

					var follower = followerController.FollowerStack[j][0];
					followerController.RemoveFollowerFromStack((j, 0));
					follower.AddForce(force * FORCE_MULTIPLIER * new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(0f, 1f), 10).normalized);

					targetGroup.AddMember(follower.transform, 1, 1);
				}
			}
		}

		private void OnFollowerInHole(Follower follower, int multiplier)
		{
			if (!targetGroup.FindMember(follower.transform).Equals(-1))
				targetGroup.RemoveMember(follower.transform);

			if (!isFinished)
			{
				if (waitForFinishCoroutine is not null)
				{
					StopCoroutine(waitForFinishCoroutine);
					waitForFinishCoroutine = null;
				}

				waitForFinishCoroutine = StartCoroutine(WaitForFinish());
			}
		}

		private IEnumerator WaitForFinish()
		{
			yield return waitForFinish;

			isFinished = true;
			Debug.Log("finish");
			LevelManager.Instance.Win();
		}
	}
}