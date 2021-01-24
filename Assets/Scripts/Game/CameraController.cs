using UnityEngine;

public class CameraController : MonoBehaviour
{
	// Perams
	[SerializeField] bool snapCam;
	[SerializeField] float smoothing;
	[SerializeField] float alwaysFollowRange;

	// Data
	Vector2 gotoPos;

	// Cache
	Mover2D target;
	float sqAlwaysFollowRange;

	void Start()
	{
		target = Player.current.GetComponent<Mover2D>();

		sqAlwaysFollowRange = alwaysFollowRange * alwaysFollowRange;
	}

	void LateUpdate()
	{
		if (snapCam)
		{
			transform.position = target.transform.position;
			return;
		}

		if (target.bIsGrounded || Vector2.SqrMagnitude(transform.position - target.transform.position) > sqAlwaysFollowRange)
			gotoPos = target.transform.position;
		else
			gotoPos.x = target.transform.position.x;

		transform.position = Vector2.Lerp(transform.position, gotoPos, smoothing * Time.deltaTime * 10);
	}
}