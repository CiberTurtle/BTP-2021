using UnityEngine;

[AddComponentMenu("Game/Elements/Item")]
[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{
	public SOItem item;
	[Space]
	public GameObject pfPickupEffect;

	void Start()
	{
		name = item.name;

		OnValidate();
	}

	public void Pickup()
	{
		Player.current.AddItem(item);

		if (pfPickupEffect) Instantiate(pfPickupEffect);

		Destroy(gameObject);
	}

	void OnValidate()
	{
		GetComponent<SpriteRenderer>().sprite = item.sprite;
	}

	// void OnDrawGizmos()
	// {
	// 	Gizmos.color = new Color(0, 0, 1, 0.25f);
	// 	Gizmos.DrawCube(transform.position, Vector3.one);
	// }
}