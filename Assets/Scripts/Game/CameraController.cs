using UnityEngine;

public class CameraController : MonoBehaviour
{
	// Perams
	[SerializeField] float smoothing;
	[SerializeField] Transform cam;

	// Cache
	Mover2D target;

	void Start()
	{
		target = Player.current.GetComponent<Mover2D>();
	}

	void LateUpdate()
	{
		if (target.bIsGrounded)
			cam.position = Vector2.Lerp(cam.position, target.transform.position, smoothing * Time.deltaTime * 10);
		else
			cam.position = Vector2.Lerp(cam.position, new Vector2(target.transform.position.x, cam.position.y), smoothing * Time.deltaTime * 10);

		// cam.position = target.transform.position;
	}
}