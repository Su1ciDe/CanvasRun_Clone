using Gameplay;
using TMPro;
using UnityEngine;

namespace UI
{
	public class FollowerCountUI : MonoBehaviour
	{
		[SerializeField] private TMP_Text txtFollowerCount;

		private void Start()
		{
			OnFollowerCountChanged(Player.Instance.MovingObject.TotalFollowerCount);
		}

		private void OnEnable()
		{
			Player.Instance.MovingObject.OnFollowerCountChanged += OnFollowerCountChanged;
		}

		private void OnDisable()
		{
			if (Player.Instance)
				Player.Instance.MovingObject.OnFollowerCountChanged += OnFollowerCountChanged;
		}

		private void OnFollowerCountChanged(int followerCount)
		{
			txtFollowerCount.SetText(followerCount.ToString());
		}
	}
}