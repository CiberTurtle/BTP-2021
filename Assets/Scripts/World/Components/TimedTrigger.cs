using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TimedTrigger : MonoBehaviour
{
	public UnityEvent onTrimmerEnd = new UnityEvent();

	bool timmerStarted;

	public void StartTimmer(float time)
	{
		if (timmerStarted) return;

		timmerStarted = true;

		StartCoroutine(ICountDown(time));
	}

	IEnumerator ICountDown(float time)
	{
		yield return new WaitForSeconds(time);

		onTrimmerEnd.Invoke();
	}
}