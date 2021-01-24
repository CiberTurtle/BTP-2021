using UnityEngine;

public class CameraController : MonoBehaviour
{
	// Perams
	[SerializeField] float smoothing;
	[SerializeField] Transform cam;
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
		if (target.bIsGrounded || Vector2.SqrMagnitude(transform.position - target.transform.position) > sqAlwaysFollowRange)
			gotoPos = target.transform.position;
		else
			gotoPos.x = target.transform.position.x;

		cam.position = Vector2.Lerp(cam.position, gotoPos, smoothing * Time.deltaTime * 10);

		// cam.position = target.transform.position;
	}
}