using UnityEngine;
using UnityEngine.Events;

namespace Interfaces
{
	public interface IInput
	{
		public bool CanInput { get; set; }
		public event UnityAction<float> OnDrag; // <dragDelta>
	}
}