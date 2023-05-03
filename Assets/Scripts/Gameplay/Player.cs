using System.Collections;
using Controllers;
using Interfaces;
using Managers;
using UnityEngine;
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

		private void Awake()
		{
			Input = GetComponent<IInput>();
			PlayerMovement = GetComponent<PlayerMovement>();
		}

		public void BoostSpeed(float boostAmount)
		{
			StartCoroutine(BoostCoroutine(boostAmount));
		}

		private IEnumerator BoostCoroutine(float boostAmount)
		{
			PlayerMovement.BoostSpeed(boostAmount);
			GameManager.Instance.CameraController.ShakeFov(85, 2);

			trigger.enabled = false;

			yield return new WaitForSeconds(.5f);

			trigger.enabled = true;
		}
	}
}