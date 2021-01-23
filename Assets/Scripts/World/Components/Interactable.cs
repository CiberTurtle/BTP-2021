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
}