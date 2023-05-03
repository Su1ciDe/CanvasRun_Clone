using System.Collections;
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
		public IInput Input { get; private set; }
		public PlayerMovement PlayerMovement { get; private set; }
		public FollowerController FollowerController => followerController;
		[SerializeField] private FollowerController followerController;

		[Space]
		[SerializeField] private Collider trigger;

		public static event UnityAction<Vector3> OnCollectGem;

		private void Awake()
		{
			Input = GetComponent<IInput>();
			PlayerMovement = GetComponent<PlayerMovement>();
		}

		public void CollectGem(int gemAmount, Vector3 gemPosition)
		{
			GameManager.Instance.GemScore += gemAmount;
			OnCollectGem?.Invoke(gemPosition);
		}

		public void BoostSpeed(float boostAmount)
		{
			StartCoroutine(BoostCoroutine(boostAmount));
		}

		private IEnumerator BoostCoroutine(float boostAmount)
		{
			PlayerMovement.BoostSpeed(boostAmount);
			GameManager.Instance.MainCameraController.ShakeFov(85, 2);

			trigger.enabled = false;

			yield return new WaitForSeconds(.5f);

			trigger.enabled = true;
		}
	}
}