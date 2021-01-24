using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Game/Components/FetchQuest")]
public class FetchQuest : MonoBehaviour
{
	public enum NPCState
	{
		HasNotStarted,
		Started,
		Done
	}

	public SOItem requestedItem;
	[Space]
	[NaughtyAttributes.ResizableTextArea] public string startText;
	[NaughtyAttributes.ResizableTextArea] public string duringText;
	[NaughtyAttributes.ResizableTextArea] public string givenText;
	[NaughtyAttributes.ResizableTextArea] public string endText;
	[Space]
	public UnityEvent onComplete;

	NPCState state = NPCState.HasNotStarted;

	public void Interact()
	{
		TalkTo();
	}

	public void TalkTo()
	{
		switch (state)
		{
			case NPCState.HasNotStarted:
				Game.current.DisplayTextBox(name, startText);
				state = NPCState.Started;
				break;
			case NPCState.Started:
				if (Player.current.UseItem(requestedItem))
				{
					Game.current.DisplayTextBox(name, givenText);
					onComplete.Invoke();
					state = NPCState.Done;
				}
				else
					Game.current.DisplayTextBox(name, duringText);
				break;
			case NPCState.Done:
				Game.current.DisplayTextBox(name, endText);
				break;
		}
	}
}