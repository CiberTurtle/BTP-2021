using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Portal : MonoBehaviour
{
	[SerializeField] List<SOItem> gems;
	[Space]
	[SerializeField] GameObject ui;
	[SerializeField] Transform items;
	[SerializeField] GameObject slot;
	[Space]
	[SerializeField] Transform circle;

	void Awake()
	{
		PlaceGem();
	}

	public void PlaceGem()
	{
		if (!Game.current.hasWon)
		{
			if (gems.Count < 1)
			{
				circle.DOScale(new Vector3(100, 100, 1), 2f).SetEase(Ease.InOutExpo).SetUpdate(true).OnComplete(() => circle.localScale = new Vector3(1000, 1000, 1));

				Game.current.End();
			}
			else
			{
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
		}
		else
		{
			ui.SetActive(false);
		}

		foreach (Transform item in items)
			Destroy(item.gameObject);

		foreach (var gem in gems)
			Instantiate(slot, items).transform.GetChild(0).GetComponent<Image>().sprite = gem.sprite;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
			ui.SetActive(true);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
			ui.SetActive(false);
	}

	void GotAllGems()
	{
		GetComponent<Interactable>().pickupDescription = "<color=purple>Enter</color>";
	}
}
