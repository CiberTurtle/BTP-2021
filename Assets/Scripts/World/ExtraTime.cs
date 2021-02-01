using System.Collections;
using UnityEngine;
using DG.Tweening;

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
	[SerializeField] GameObject floatingText;
	[SerializeField] Key[] sprites = new Key[0];

	SpriteRenderer spr;
	Collider2D col;
	SpriteRenderer dot;

	void Awake()
	{
		spr = GetComponent<SpriteRenderer>();
		col = GetComponent<Collider2D>();
		dot = transform.GetChild(0).GetComponent<SpriteRenderer>();

		dot.color = new Color(1, 1, 1, 0);
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

		Instantiate(floatingText, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity).transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = $"+ {time}s";

		if (respawnTime > 0) StartCoroutine(IRespawnTimmer());
	}

	IEnumerator IRespawnTimmer()
	{
		dot.transform.localScale = new Vector3(0.1f, 0.1f, 1);
		dot.color = new Color(1, 1, 1, 0.1f);

		DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> pop = null;

		dot.DOColor(new Color(1, 1, 1, 0.5f), respawnTime).SetEase(Ease.Linear);
		dot.transform.DOScale(new Vector3(0.4f, 0.4f, 1), respawnTime - 0.15f).SetEase(Ease.Linear)
		.OnComplete(() => pop = dot.transform.DOScale(new Vector3(0.5f, 0.5f, 1), 0.75f).SetEase(Ease.OutElastic));

		yield return new WaitForSeconds(respawnTime);
		spr.enabled = true;
		col.enabled = true;

		if (pop != null) pop.Kill();

		dot.color = new Color(1, 1, 1, 0);
	}
}