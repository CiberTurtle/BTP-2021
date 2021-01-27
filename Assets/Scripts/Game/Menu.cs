using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
	[SerializeField] GameObject menu;
	[SerializeField] GameObject startMenu;
	[SerializeField] GameObject leaderboardsMenu;
	[SerializeField] GameObject settingsMenu;
	[Space]
	[SerializeField] GameObject pfEntry;
	[SerializeField] Transform anyLeaderboard;
	[SerializeField] Transform allLeaderboard;
	[Space]
	[SerializeField] TMP_InputField nameField;
	[Space]
	[SerializeField] Transform stages;

	private void Awake()
	{
		Leaderboards.current.onGotLeaderboards.AddListener(() =>
		{
			for (int i = 0; i < Leaderboards.current.anyNames.Count; i++)
			{
				var entry = Instantiate(pfEntry, anyLeaderboard).transform;

				entry.GetChild(0).GetComponent<TMP_Text>().text = Leaderboards.current.anyNames[i];
				entry.GetChild(1).GetComponent<TMP_Text>().text = TimeSpan.FromSeconds(((float)Leaderboards.current.anyScores[i] / 100)).ToString(@"m\:ss\.ff");
			}

			anyLeaderboard.GetComponent<RectTransform>().sizeDelta = new Vector2(anyLeaderboard.GetComponent<RectTransform>().sizeDelta.x, 64 * Leaderboards.current.anyNames.Count);

			for (int i = 0; i < Leaderboards.current.allNames.Count; i++)
			{
				var entry = Instantiate(pfEntry, allLeaderboard).transform;

				entry.GetChild(0).GetComponent<TMP_Text>().text = Leaderboards.current.allNames[i];
				entry.GetChild(1).GetComponent<TMP_Text>().text = ((float)Leaderboards.current.allScores[i] / 100).ToString();
			}

			allLeaderboard.GetComponent<RectTransform>().sizeDelta = new Vector2(allLeaderboard.GetComponent<RectTransform>().sizeDelta.x, 64 * Leaderboards.current.allNames.Count);
		});

		nameField.text = PlayerPrefs.GetString("name", "anonymous");

		// var rng = new Random(DateTime.Today.Second);
		stages.GetChild(new System.Random(DateTime.Today.Second).Next(0, stages.childCount)).gameObject.SetActive(true);

		menu.SetActive(true);
	}

	public void StartGame()
	{
		SceneManager.LoadScene(1);
	}

	public void PrepNormal()
	{
		PlayerPrefs.SetInt("mode", 0);
		Debug.Log("Normal");
	}

	public void PrepAny()
	{
		PlayerPrefs.SetInt("mode", 1);
		Debug.Log("Any%");
	}

	public void PrepAll()
	{
		PlayerPrefs.SetInt("mode", 2);
		Debug.Log("100%");
	}

	public void OpenStart(bool open)
	{
		menu.SetActive(!open);
		startMenu.SetActive(open);
	}

	public void OpenSettings(bool open)
	{
		menu.SetActive(!open);
		settingsMenu.SetActive(open);
	}

	public void OpenLeaderboards(bool open)
	{
		menu.SetActive(!open);
		leaderboardsMenu.SetActive(open);

		RefreshLeaderboards();
	}

	public void SetName(string name)
	{
		if (string.IsNullOrEmpty(name)) name = "anonymous";

		PlayerPrefs.SetString("name", name);
	}

	public void RefreshLeaderboards()
	{
		foreach (Transform child in anyLeaderboard)
			Destroy(child.gameObject);
		foreach (Transform child in allLeaderboard)
			Destroy(child.gameObject);

		Leaderboards.current.GetLeaderboards();
	}

	public void OpenURL(string url)
	{
		Application.OpenURL(url);
	}

	public void QuitOutOfThisHorribleGame()
	{
		Application.Quit();
	}
}
