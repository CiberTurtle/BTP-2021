using UnityEngine;
using DG.Tweening;

public class Slingshot : MonoBehaviour
{
	[SerializeField] Transform target;
	[SerializeField] Ease ease;
	[SerializeField] float timeMul;

	bool isRiding;

	public void Ride()
	{
		if (isRiding) return;
		isRiding = true;

		Player.current.transform.DOMove(target.position, Vector2.Distance(transform.position, target.position) * timeMul).SetEase(ease).OnComplete(() => isRiding = false);
	}
}