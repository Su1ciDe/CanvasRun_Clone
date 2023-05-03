using UnityEngine;

namespace Utilities
{
	public class Level : MonoBehaviour
	{
		public virtual void Init()
		{
			gameObject.SetActive(true);
		}

		public virtual void Play()
		{
		}
	}
}