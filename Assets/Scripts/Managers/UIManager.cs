using UI;
using UnityEngine;

namespace Managers
{
	public class UIManager : MonoBehaviour
	{
		private void Awake()
		{
			var panels = GetComponentsInChildren<UIPanel>(true);
			foreach (var panel in panels)
				panel.Init();
		}
	}
}