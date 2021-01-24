using UnityEngine;

public class Lava : MonoBehaviour
{
	public float dps;

	public void DealDamage()
	{
		Game.current.AddTime(-Time.deltaTime * dps);
	}
}