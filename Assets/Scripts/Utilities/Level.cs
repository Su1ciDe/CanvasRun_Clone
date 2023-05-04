using UnityEngine;

namespace Utilities
{
	public class Level : MonoBehaviour
	{
		public Finish.Finish Finish { get; private set; }
		private void Awake()
		{
			Finish = GetComponentInChildren<Finish.Finish>();
		}

		public virtual void Init()
		{
			gameObject.SetActive(true);
		}

		public virtual void Play()
		{
		}
	}
}