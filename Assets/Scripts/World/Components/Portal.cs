using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
	public void PlaceGem()
	{
		if (Game.gameSave.gemsLeft.Count < 1)
			Game.current.End();

		foreach (var gem in Game.gameSave.gemsLeft)
		{
			if (Player.current.UseItem(gem))
			{
				Game.gameSave.gemsLeft.Remove(gem);
				if (Game.gameSave.gemsLeft.Count < 1)
					GotAllGems();
			}
		}
	}

	void GotAllGems()
	{
		GetComponent<Interactable>().pickupDescription = "<color=purple>Enter</color>";
	}
}
