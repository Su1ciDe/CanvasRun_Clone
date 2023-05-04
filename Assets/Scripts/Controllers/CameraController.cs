using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace Controllers
{
	[RequireComponent(typeof(CinemachineVirtualCamera))]
	public class CameraController : MonoBehaviour
	{
		private CinemachineVirtualCamera vcam;
		private float normalFov;

		private void Awake()
		{
			vcam = GetComponent<CinemachineVirtualCamera>();
			normalFov = vcam.m_Lens.FieldOfView;
		}

		public void SetFollowTarget(Transform target)
		{
			vcam.m_Follow = target;
		}

		public void BoostFov(float targetFov, float duration)
		{
			vcam.DOKill();
			var sequence = DOTween.Sequence();
			sequence.Append(DOTween.To(() => vcam.m_Lens.FieldOfView, x => vcam.m_Lens.FieldOfView = x, targetFov, duration).SetEase(Ease.OutExpo));
			sequence.Append(DOTween.To(() => vcam.m_Lens.FieldOfView, x => vcam.m_Lens.FieldOfView = x, normalFov, duration * 2).SetEase(Ease.InOutSine));
			sequence.SetTarget(vcam);
		}
	}
}