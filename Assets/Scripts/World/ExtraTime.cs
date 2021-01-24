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

	[SerializeField] float time;
	[SerializeField] Key[] sprites = new Key[0];

	void Awake()
	{
		OnValidate();
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
		Destroy(gameObject);
	}
}