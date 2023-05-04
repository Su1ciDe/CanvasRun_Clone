using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public class FailScreen : UIPanel
	{
		private void Awake()
		{
			var btnTapToRetry = GetComponent<Button>();
			btnTapToRetry.onClick.AddListener(TapToRetry);
		}

		public override void Init()
		{
			base.Init();
			LevelManager.OnLevelFail += OnLevelFailed;
		}

		private void OnDestroy()
		{
			LevelManager.OnLevelFail -= OnLevelFailed;
		}

		private void OnLevelFailed()
		{
			Show();
		}

		private void TapToRetry()
		{
			LevelManager.Instance.RetryLevel();
			Hide();
		}
	}
}