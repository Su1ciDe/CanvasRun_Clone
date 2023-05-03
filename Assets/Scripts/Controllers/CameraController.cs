using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace Controllers
{
	[RequireComponent(typeof(CinemachineVirtualCamera))]
	public class CameraController : MonoBehaviour
	{
		private CinemachineVirtualCamera vcam;

		private void Awake()
		{
			vcam = GetComponent<CinemachineVirtualCamera>();
		}

		public void SetFollowTarget(Transform target)
		{
			vcam.m_Follow = target;
		}
		
		public void ShakeFov(float targetFov, float duration)
		{
			float tempFov = vcam.m_Lens.FieldOfView;
			vcam.DOComplete();
			DOTween.To(() => vcam.m_Lens.FieldOfView, x => vcam.m_Lens.FieldOfView = x, targetFov, duration).SetEase(Ease.InOutSine).OnComplete(() =>
			{
				DOTween.To(() => vcam.m_Lens.FieldOfView, x => vcam.m_Lens.FieldOfView = x, tempFov, duration * 2).SetEase(Ease.InOutSine);
			}).SetTarget(vcam);
		}
	}
}