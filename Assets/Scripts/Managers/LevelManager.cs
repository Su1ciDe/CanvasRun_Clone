using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Managers
{
	[DefaultExecutionOrder(-2)]
	public class LevelManager : Singleton<LevelManager>
	{
		[Tooltip("If you have tutorial levels add them here to extract them from rotation")]
		public Level[] TutorialLevels;
		public Level[] Levels;
		public Level CurrentLevel { get; private set; }

		/// <summary>
		/// Number of the level currently played. This value is not modulated.
		/// </summary>
		public static int LevelNo
		{
			get => PlayerPrefs.GetInt("LevelNo", 1);
			set => PlayerPrefs.SetInt("LevelNo", value);
		}

		private bool isWon;
		private bool isLost;

		public static event UnityAction OnLevelLoad;
		public static event UnityAction OnLevelUnload;
		public static event UnityAction OnLevelStart;
		public static event UnityAction OnLevelSuccess;
		public static event UnityAction OnLevelFail;

		private void Awake()
		{
			if (Levels.Length.Equals(0))
			{
				Debug.LogError(name + ": There is no level added to the script!", this);
			}
		}

		private void Start()
		{
#if UNITY_EDITOR
			var levels = FindObjectsByType<Level>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (var level in levels)
				level.gameObject.SetActive(false);
#endif
			LoadCurrentLevel();
		}

		private void LoadCurrentLevel()
		{
			if (Levels.Length <= 0) return;

			int tutorialCount = TutorialLevels.Length;
			int levelCount;
			int levelIndex = LevelNo;

			if (LevelNo <= tutorialCount)
				levelCount = tutorialCount;
			else
			{
				levelCount = Levels.Length;
				levelIndex -= tutorialCount;
			}

			levelIndex %= levelCount;
			int selectedGameSceneIndex = levelIndex.Equals(0) ? levelCount : levelIndex;

			LoadLevel(selectedGameSceneIndex);
			CurrentLevel.Init();
		}

		public void StartLevel()
		{
			CurrentLevel.Play();
			
			OnLevelStart?.Invoke();
		}

		public void NextLevel()
		{
			UnloadLevel();

			LoadCurrentLevel();
		}

		public void RetryLevel()
		{
			UnloadLevel();

			LoadCurrentLevel();
		}

		public void UnloadLevel()
		{
			OnLevelUnload?.Invoke();
			DestroyImmediate(CurrentLevel.gameObject);
		}

		public void LoadLevel(int levelIndex)
		{
			isWon = false;
			isLost = false;
			CurrentLevel = Instantiate(LevelNo <= TutorialLevels.Length ? TutorialLevels[levelIndex - 1] : Levels[levelIndex - 1]);

			OnLevelLoad?.Invoke();
		}

		public void Win()
		{
			if (isWon) return;
			isWon = true;

			LevelNo++;
			
			OnLevelSuccess?.Invoke();
		}

		public void Lose()
		{
			if (isLost) return;
			isLost = true;
			
			OnLevelFail?.Invoke();
		}
	}
}