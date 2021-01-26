using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	[SerializeField] GameObject menu;
	[SerializeField] GameObject startMenu;
	[SerializeField] GameObject leaderboardsMenu;
	[SerializeField] GameObject settingsMenu;
	[Space]
	[SerializeField] Transform stages;

	private void Awake()
	{
		// Enable a random child
		foreach (Transform stage in stages)
			stages.gameObject.SetActive(false);
		stages.GetChild(Random.Range(0, stages.childCount - 1)).gameObject.SetActive(true);

		menu.SetActive(true);
	}

	public void StartGame()
	{
		SceneManager.LoadScene(1);
	}

	public void PrepNormal()
	{
		Debug.Log("Normal");
	}

	public void PrepAny()
	{
		Debug.Log("Any%");
	}

	public void PrepAll()
	{
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
