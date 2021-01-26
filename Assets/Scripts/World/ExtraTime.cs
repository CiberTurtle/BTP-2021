using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ExtraTime : MonoBehaviour
{
	[System.Serializable]
	public struct Key
	{
		public float time;
		public Sprite sprite;
	}

	[SerializeField] float time = 10;
	[SerializeField] float respawnTime = 60;
	[Space]
	[SerializeField] Key[] sprites = new Key[0];

	SpriteRenderer spr;
	Collider2D col;

	void Awake()
	{
		spr = GetComponent<SpriteRenderer>();
		col = GetComponent<Collider2D>();
	}

	void OnValidate()
	{
		foreach (var sprite in sprites)
		{
			if (sprite.time <= time)
				GetComponent<SpriteRenderer>().sprite = sprite.sprite;
		}
	}

	public void Pickup()
	{
		Game.current.AddTime(time);
		spr.enabled = false;
		col.enabled = false;

		if (respawnTime < 0) StartCoroutine(IRespawnTimmer());
	}

	IEnumerator IRespawnTimmer()
	{
		yield return new WaitForSeconds(respawnTime);
		spr.enabled = true;
		col.enabled = true;
	}
}