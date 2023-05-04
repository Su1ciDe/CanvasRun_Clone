using System.Collections;
using Gameplay;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class LevelBarUI : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI txtLevel;
		[SerializeField] private TextMeshProUGUI txtNextLevel;
		[Space]
		[SerializeField] private Image fill;

		private const float fillUpdateInterval = .1f;
		private WaitForSeconds waitFillUpdate;
		private Coroutine fillBarCoroutine;

		private void OnEnable()
		{
			LevelManager.OnLevelLoad += OnLevelLoaded;
			LevelManager.OnLevelStart += OnLevelStarted;
		}

		private void OnDisable()
		{
			LevelManager.OnLevelLoad -= OnLevelLoaded;
			LevelManager.OnLevelStart -= OnLevelStarted;
		}

		private void OnLevelLoaded()
		{
			txtLevel.SetText(LevelManager.LevelNo.ToString());
			txtNextLevel.SetText((LevelManager.LevelNo + 1).ToString());

			fill.fillAmount = 0;

			if (fillBarCoroutine is not null)
			{
				StopCoroutine(fillBarCoroutine);
				fillBarCoroutine = null;
			}
		}

		private void OnLevelStarted()
		{
			fillBarCoroutine = StartCoroutine(FillTheBar());
		}

		private IEnumerator FillTheBar()
		{
			while (true)
			{
				yield return waitFillUpdate;
				fill.fillAmount = Player.Instance.transform.position.z / LevelManager.Instance.CurrentLevel.Finish.transform.position.z;
			}
		}
	}
}