using Collectibles;
using DG.Tweening;
using Gameplay;
using Managers;
using TMPro;
using UnityEngine;
using Utilities;

namespace UI
{
	public class GemUI : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI txtDiamondCount;
		[SerializeField] private Transform imageTarget;

		[SerializeField] private float diamondAnimDuration = .5f;

		private const string GEM_POOL = "Gem";

		private void Start()
		{
			SetGemText(GameManager.Instance.GemScore);
		}

		private void OnEnable()
		{
			Player.OnCollectGem += OnCollectGem;
		}

		private void OnDisable()
		{
			Player.OnCollectGem -= OnCollectGem;
		}

		private void SetGemText(int gemCount)
		{
			txtDiamondCount.SetText(gemCount.ToString());
		}

		private void OnCollectGem(Gem gem)
		{
			var imgCoin = ObjectPooler.Instance.Spawn(GEM_POOL, GameManager.MainCamera.WorldToScreenPoint(gem.transform.position), transform);
			imgCoin.transform.DOComplete();
			imgCoin.transform.DOMove(imageTarget.position, diamondAnimDuration).SetEase(Ease.InBack).OnComplete(() =>
			{
				imageTarget.DOPunchScale(Vector3.one * .9f, .2f, 2, .5f);

				imgCoin.SetActive(false);
				SetGemText(GameManager.Instance.GemScore);
			});
		}
	}
}