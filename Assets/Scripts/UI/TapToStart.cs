using Managers;
using UnityEngine.EventSystems;

namespace UI
{
	public class TapToStart : UIPanel
	{
		private EventTrigger tapToStartEventTrigger;

		public override void Init()
		{
			base.Init();
			tapToStartEventTrigger = GetComponent<EventTrigger>();
			var entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((eventData) => StartLevel());
			tapToStartEventTrigger.triggers.Add(entry);

			LevelManager.OnLevelLoad += OnLevelLoaded;
		}

		private void OnDestroy()
		{
			LevelManager.OnLevelLoad -= OnLevelLoaded;
		}

		private void OnLevelLoaded()
		{
			Show();
		}

		private void StartLevel()
		{
			LevelManager.Instance.StartLevel();
			Hide();
		}
	}
}