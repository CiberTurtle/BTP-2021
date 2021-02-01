using UnityEngine;
using DG.Tweening;

public class FloatingText : MonoBehaviour
{
	TMPro.TMP_Text text;

	void Awake()
	{
		text = transform.GetChild(0).GetComponent<TMPro.TMP_Text>();

		transform.DOScale(new Vector3(0.75f, 0.75f, 0.75f), 0.25f).SetEase(Ease.OutElastic)
		.OnComplete(() => transform.DOMoveY(transform.position.y + 1f, 1f).SetEase(Ease.Linear));

		text.DOFade(1, 0.15f).SetEase(Ease.Linear)
		.OnComplete(() => text.DOFade(0, 1f).SetEase(Ease.Linear));
	}
}