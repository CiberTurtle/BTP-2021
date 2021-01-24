using UnityEngine;
using UnityEngine.Events;

public class CollisionTrigger : MonoBehaviour
{
	public UnityEvent onTriggerEnter = new UnityEvent();
	public UnityEvent onTriggerStay = new UnityEvent();

	bool hasBeenTriggered;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Player")) return;

		if (hasBeenTriggered) return;

		onTriggerEnter.Invoke();
		hasBeenTriggered = true;
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (!other.CompareTag("Player")) return;

		onTriggerStay.Invoke();
	}
}