using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
	[SerializeField] List<SOItem> requiredGems;

	List<SOItem> gemsLeft;

	private void Awake()
	{
		gemsLeft = requiredGems;
	}

	public void PlaceGem()
	{
		if (gemsLeft.Count < 1)
			Game.current.End();

		foreach (var gem in gemsLeft)
		{
			if (Player.current.UseItem(gem))
			{
				gemsLeft.Remove(gem);
				if (gemsLeft.Count < 1)
					GotAllGems();
			}
		}
	}

	void GotAllGems()
	{
		GetComponent<Interactable>().pickupDescription = "<color=purple>Enter</color>";
	}
}
