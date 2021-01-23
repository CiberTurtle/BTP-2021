using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Mover2D target;
	public float smoothing;
	public Transform cam;

	void LateUpdate()
	{
		if (target.bIsGrounded)
			cam.position = Vector2.Lerp(cam.position, target.transform.position, smoothing * Time.deltaTime * 10);
		else
			cam.position = Vector2.Lerp(cam.position, new Vector2(target.transform.position.x, cam.position.y), smoothing * Time.deltaTime * 10);

		// cam.position = target.transform.position;
	}
}