using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public class WinScreen : UIPanel
	{
		private void Awake()
		{
			var btnTapToContinue = GetComponent<Button>();
			btnTapToContinue.onClick.AddListener(TapToContinue);
		}

		public override void Init()
		{
			base.Init();
			LevelManager.OnLevelSuccess += OnLevelWon;
		}

		private void OnDestroy()
		{
			LevelManager.OnLevelSuccess -= OnLevelWon;
		}

		private void OnLevelWon()
		{
			Show();
		}

		private void TapToContinue()
		{
			LevelManager.Instance.NextLevel();
			Hide();
		}
	}
}