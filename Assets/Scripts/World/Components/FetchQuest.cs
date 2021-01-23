using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Game/Components/FetchQuest")]
public class FetchQuest : MonoBehaviour
{
	public SOItem requestedItem;
	[Space]
	[NaughtyAttributes.ResizableTextArea] public string startText;
	[NaughtyAttributes.ResizableTextArea] public string duringText;
	[NaughtyAttributes.ResizableTextArea] public string givenText;
	[NaughtyAttributes.ResizableTextArea] public string endText;
	[Space]
	public UnityEvent onComplete;

	public void Interact()
	{
		if (Player.current.UseItem(requestedItem))
		{
			onComplete.Invoke();
		}
	}
}