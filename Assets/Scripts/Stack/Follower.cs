using UnityEngine;

namespace Stack
{
	public class Follower : MonoBehaviour
	{
		public Vector3 PreviousPosition { get; private set; }

		public Rigidbody Rb => rb;
		[SerializeField] private Rigidbody rb;

		protected virtual void Awake()
		{
			rb = GetComponent<Rigidbody>();
		}

		protected virtual void FixedUpdate()
		{
			PreviousPosition = rb.position;
		}

		public void DestroySelf()
		{
			gameObject.SetActive(false);
		}
	}
}