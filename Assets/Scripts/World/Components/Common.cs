using UnityEngine;

[AddComponentMenu("Game/Components/Common")]
public class Common : MonoBehaviour
{
	public void GiveItem(SOItem item) => Player.current.AddItem(item);
	public bool UseItem(SOItem item) => Player.current.UseItem(item);
	public void Say(string text) => Game.current.DisplayTextBox(name, text, 10f);
	public void AddTime(float amount) => Game.current.AddTime(amount);
	public void DestroyObj(Object obj) => Destroy(obj);
}