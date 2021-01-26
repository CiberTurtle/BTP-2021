using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Game/Components/FetchQuest")]
public class FetchQuest : MonoBehaviour
{
	[System.Serializable]
	public struct Key
	{
		public NPCState state;
		public Sprite sprite;
	}

	public enum NPCState
	{
		HasNotStarted,
		Started,
		Done
	}

	public bool countTowardsCompletion = true;
	public SOItem requestedItem;
	[Space]
	[TextArea] public string startText;
	[TextArea] public string duringText;
	[TextArea] public string givenText;
	[TextArea] public string endText;
	[Space]
	public UnityEvent onComplete;
	[Space]
	[SerializeField] SpriteRenderer status;
	[SerializeField] List<Key> sprites = new List<Key>();

	NPCState state = NPCState.HasNotStarted;

	void OnEnable()
	{
		if (countTowardsCompletion)
		{
			status.sprite = sprites.Find((x) => x.state == state).sprite;
			Game.current.npcsToHelp++;
		}
		else
		{
			status.sprite = null;
		}
	}

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
					if (countTowardsCompletion) Game.current.HelpedNPC();
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

		if (countTowardsCompletion) status.sprite = sprites.Find((x) => x.state == state).sprite;
	}
}