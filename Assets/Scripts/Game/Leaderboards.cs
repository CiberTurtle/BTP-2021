using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Leaderboards : MonoBehaviour
{
	public static Leaderboards current;

	public List<string> anyNames = new List<string>();
	public List<int> anyScores = new List<int>();

	public List<string> allNames = new List<string>();
	public List<int> allScores = new List<int>();

	public UnityEvent onGotLeaderboards = new UnityEvent();

	void Awake()
	{
		if (current != null)
		{
			Destroy(this.gameObject);
		}

		current = this;
		DontDestroyOnLoad(this);
	}

	public void AddNewHighscore(string username, int score, bool isAll)
	{
		StartCoroutine(UploadNewHighscore(username, score, isAll));
	}

	[NaughtyAttributes.Button]
	public void GetLeaderboards()
	{
		StartCoroutine(DownloadHighscores());
	}

	IEnumerator UploadNewHighscore(string username, int score, bool isAll)
	{
		if (isAll)
		{
			using (var www = UnityWebRequest.Get(Codes.webUrl + Codes.allPrivateCode + "/add/" + UnityWebRequest.EscapeURL(username) + "/" + score))
			{
				yield return www.SendWebRequest();

				if (!string.IsNullOrEmpty(www.downloadHandler.error))
					Debug.LogError("Web Request Failed");
			}
		}
		else
		{
			using (var www = UnityWebRequest.Get(Codes.webUrl + Codes.anyPrivateCode + "/add/" + UnityWebRequest.EscapeURL(username) + "/" + score))
			{
				yield return www.SendWebRequest();

				if (!string.IsNullOrEmpty(www.downloadHandler.error))
					Debug.LogError("Web Request Failed");
			}
		}
	}

	IEnumerator DownloadHighscores()
	{
		using (var any = UnityWebRequest.Get(Codes.webUrl + Codes.anyPublicCode + "/pipe-score-asc/"))
		{
			using (var all = UnityWebRequest.Get(Codes.webUrl + Codes.allPublicCode + "/pipe-score-asc/"))
			{

				yield return any.SendWebRequest();
				yield return all.SendWebRequest();

				if (!string.IsNullOrEmpty(any.downloadHandler.error) || !string.IsNullOrEmpty(all.downloadHandler.error))
					Debug.LogError("Web Request Failed");

				anyNames = new List<string>();
				anyScores = new List<int>();
				allNames = new List<string>();
				allScores = new List<int>();

				string[] anyLines = any.downloadHandler.text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
				foreach (var line in anyLines)
				{
					string[] col = line.Split(new char[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries);
					anyNames.Add(UnityWebRequest.UnEscapeURL(col[0]));
					anyScores.Add(int.Parse(col[1]));
				}

				string[] allLines = all.downloadHandler.text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
				foreach (var line in allLines)
				{
					string[] col = line.Split(new char[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries);
					allNames.Add(UnityWebRequest.UnEscapeURL(col[0]));
					allScores.Add(int.Parse(col[1]));
				}

				onGotLeaderboards.Invoke();
			}
		}
	}

	// IEnumerator GetRequest()
	// {
	// 	using (UnityWebRequest www = UnityWebRequest.Get(url))
	// 	{
	// 		yield return www.SendWebRequest();
	// 		highScores = www.downloadHandler.text;
	// 	}
	// }
}