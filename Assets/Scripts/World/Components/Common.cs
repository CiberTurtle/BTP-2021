using UnityEngine;

[AddComponentMenu("Game/Components/Common")]
public class Common : MonoBehaviour
{
	public void GiveItem(SOItem item) => Player.current.AddItem(item);
	public bool UseItem(SOItem item) => Player.current.UseItem(item);
	public void AddTime(float amount) => Game.current.timeLeft += amount;
	public void DestroyObj(Object obj) => Destroy(obj);
}