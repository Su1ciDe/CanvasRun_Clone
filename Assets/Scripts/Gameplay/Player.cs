using System.Collections;
using Collectibles;
using Controllers;
using Interfaces;
using Managers;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Gameplay
{
	public class Player : Singleton<Player>
	{
		public bool IsLevelFinished { get; set; }

		public IInput Input { get; private set; }
		public PlayerMovement PlayerMovement { get; private set; }
		public FollowerController FollowerController => followerController;
		[SerializeField] private FollowerController followerController;

		[Space]
		[SerializeField] private Collider trigger;

		public static event UnityAction<Gem> OnCollectGem;

		private void Awake()
		{
			Input = GetComponent<IInput>();
			PlayerMovement = GetComponent<PlayerMovement>();
		}

		#region Level Actions

		private void OnEnable()
		{
			LevelManager.OnLevelLoad += OnLevelLoaded;
			LevelManager.OnLevelStart += OnLevelStarted;
			LevelManager.OnLevelSuccess += OnLevelWon;
			LevelManager.OnLevelFail += OnLevelFailed;
		}

		private void OnDisable()
		{
			LevelManager.OnLevelLoad -= OnLevelLoaded;
			LevelManager.OnLevelStart -= OnLevelStarted;
			LevelManager.OnLevelSuccess -= OnLevelWon;
			LevelManager.OnLevelFail -= OnLevelFailed;
		}

		private void OnLevelLoaded()
		{
			GameManager.Instance.MainCameraController.SetFollowTarget(transform);
		}

		private void OnLevelStarted()
		{
			Input.CanInput = true;
			FollowerController.CanFollow = true;
			FollowerController.FollowerControllerMovement.CanMove = true;
			PlayerMovement.CanMoveForward = true;
		}

		private void OnLevelWon()
		{
			Input.CanInput = false;
			FollowerController.CanFollow = false;
			FollowerController.FollowerControllerMovement.CanMove = false;
			PlayerMovement.CanMoveForward = false;
		}

		private void OnLevelFailed()
		{
			Input.CanInput = false;
			FollowerController.CanFollow = false;
			FollowerController.FollowerControllerMovement.CanMove = false;
			PlayerMovement.CanMoveForward = false;
		}

		#endregion

		public void FinishLine()
		{
			IsLevelFinished = true;
			Input.CanInput = false;
			FollowerController.CanFollow = false;
			FollowerController.FollowerControllerMovement.CanMove = false;
			PlayerMovement.CanMoveForward = false;
		}

		public void CollectGem(Gem gem)
		{
			GameManager.GemScore += gem.Score;
			OnCollectGem?.Invoke(gem);
		}

		public void BoostSpeed(float boostAmount)
		{
			StartCoroutine(BoostCoroutine(boostAmount));
		}

		private IEnumerator BoostCoroutine(float boostAmount)
		{
			PlayerMovement.BoostSpeed(boostAmount);
			GameManager.Instance.MainCameraController.BoostFov(85, 2);

			trigger.enabled = false;

			yield return new WaitForSeconds(.5f);

			trigger.enabled = true;
		}
	}
}