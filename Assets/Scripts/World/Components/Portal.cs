using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
	[SerializeField] List<SOItem> gems;

	public void PlaceGem()
	{
		if (gems.Count < 1)
			Game.current.End();

		foreach (var gem in gems)
		{
			if (Player.current.UseItem(gem))
			{
				gems.Remove(gem);
				if (gems.Count < 1)
					GotAllGems();
			}
		}
	}

	void GotAllGems()
	{
		GetComponent<Interactable>().pickupDescription = "<color=purple>Enter</color>";
	}
}
