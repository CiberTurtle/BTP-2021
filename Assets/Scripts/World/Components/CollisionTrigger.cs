using UnityEngine;
using UnityEngine.Events;

public class CollisionTrigger : MonoBehaviour
{
	public UnityEvent onTriggerEnter = new UnityEvent();

	bool hasBeenTriggered;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (hasBeenTriggered) return;

		onTriggerEnter.Invoke();
		hasBeenTriggered = true;
	}
}