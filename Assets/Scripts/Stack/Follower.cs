using UnityEngine;

namespace Stack
{
	public abstract class Follower : MonoBehaviour
	{
		public Vector3 PreviousPosition { get; private set; }

		public Rigidbody Rb => rb;
		[SerializeField] private Rigidbody rb;

		[SerializeField] private MeshRenderer mRenderer;
		[SerializeField] private float huePower = 50f;

		protected virtual void Awake()
		{
			rb = GetComponent<Rigidbody>();
		}

		protected virtual void FixedUpdate()
		{
			PreviousPosition = rb.position;
		}

		public virtual void Spawn()
		{
			gameObject.SetActive(true);
		}
		
		public virtual void Despawn()
		{
			gameObject.SetActive(false);
		}

		public void ChangeColor(int rowIndex)
		{
			mRenderer.material.color = Color.HSVToRGB((rowIndex / huePower) % 1f, .6f, 1);
		}

		public void AddForce(Vector3 force)
		{
			Rb.isKinematic = false;
			Rb.constraints = RigidbodyConstraints.None;

			Rb.AddForce(force);
		}
	}
}