using UnityEngine;

[AddComponentMenu("Game/Elements/Item")]
[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{
	public SOItem item;
	[Space]
	public GameObject pfPickupEffect;

	SpriteRenderer spr;

	void Start()
	{
		spr = GetComponent<SpriteRenderer>();
		spr.sprite = item.sprite;
		name = item.name;
	}

	public void Pickup()
	{
		Player.current.AddItem(item);

		if (pfPickupEffect) Instantiate(pfPickupEffect);

		Destroy(gameObject);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(0, 0, 1, 0.25f);
		Gizmos.DrawCube(transform.position, Vector3.one);
	}
}