using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
	public class MultiplierUI : MonoBehaviour
	{
		[SerializeField] private TextMeshPro txtMultiplier;

		public void SetMultiplierText(int multiplier)
		{
			txtMultiplier.SetText("X" + multiplier);
		}

		private void OnDisable()
		{
			txtMultiplier.DOComplete();
			transform.DOComplete();
		}

		public void Show()
		{
			txtMultiplier.DOComplete();
			transform.DOComplete();
			transform.forward = GameManager.MainCamera.transform.forward;
			txtMultiplier.DOFade(0, 1);
			transform.DOMoveY(2, 1).SetRelative().OnComplete(() =>
			{
				gameObject.SetActive(false);
				txtMultiplier.alpha = 1;
			});
		}
	}
}