using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Game/Components/Interactable")]
public class Interactable : MonoBehaviour
{
	[SerializeField] string pickupDescription;
	[SerializeField] bool includeNameOfGO = true;
	[Space]
	[SerializeField] UnityEvent onInteract;

	public string pickupName
	{
		get
		{
			if (includeNameOfGO)
				return pickupDescription + " " + name;
			return pickupDescription;
		}
	}

	public void Interact() => onInteract.Invoke();

	void OnDrawGizmos()
	{
		var color = new Color(0, 0, 1, 0.75f);

		for (int i = 0; i < onInteract.GetPersistentEventCount(); i++)
		{
			if (onInteract.GetPersistentTarget(i) is Transform trans)
			{
				Debug.DrawLine(transform.position, trans.position, color, 0);
			}
			else if (onInteract.GetPersistentTarget(i) is GameObject go)
			{
				Debug.DrawLine(transform.position, go.transform.position, color, 0);
			}
			else if (onInteract.GetPersistentTarget(i) is MonoBehaviour mono)
			{
				Debug.DrawLine(transform.position, mono.transform.position, color, 0);
			}
		}
	}
}