using UnityEngine;
using UnityEngine.Tilemaps;

public class StageShadow : MonoBehaviour
{
	[SerializeField] Color shadowColor = Color.white;
	[SerializeField] Vector3 shadowOffset;
	[Space]
	[SerializeField] GameObject baseTilemap;

	void Awake()
	{
		var tilemap = Instantiate(baseTilemap, baseTilemap.transform.parent).transform;

		Destroy(tilemap.GetComponent<TilemapCollider2D>());
		tilemap.GetComponent<Tilemap>().color = shadowColor;
		tilemap.GetComponent<TilemapRenderer>().sortingOrder = -1;
		tilemap.position += shadowOffset;
	}
}