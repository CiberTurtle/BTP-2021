using UnityEngine;
using UnityEngine.Events;

public class StartTrigger : MonoBehaviour
{
	public UnityEvent onStart = new UnityEvent();

	void Start()
	{
		onStart.Invoke();
	}
}