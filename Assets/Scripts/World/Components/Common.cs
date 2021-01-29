using UnityEngine;
using DG.Tweening;

[AddComponentMenu("Game/Components/Common")]
public class Common : MonoBehaviour
{
	[NaughtyAttributes.InfoBox(
	"LoopModes(Scale Z):\n00 Restart\n01 Yoyo\n02 Incremental"
	)]
	[NaughtyAttributes.InfoBox(
		"Easeing Types (Scale Y):\n00 Unset\n01 Linear\n02 InSine\n03 OutSine\n04 InOutSine\n05 InQuad\n06 OutQuad\n07 InOutQuad\n08 InCubic\n09 OutCubic\n10 InOutCubic\n11 InQuart\n12 OutQuart\n13 InOutQuart\n14 InQuint\n15 OutQuint\n16 InOutQuint\n17 InExpo\n18 OutExpo\n19 InOutExpo\n20 InCirc\n21 OutCirc\n22 InOutCirc\n23 InElastic\n24 OutElastic\n25 InOutElastic\n26 InBack\n27 OutBack\n28 InOutBack\n29 InBounce\n30 OutBounce\n31 InOutBounce\n32 Flash\n33 InFlash\n34 OutFlash\n35 InOutFlash"
		)]
	[NaughtyAttributes.InfoBox(
		"Scale X: Time to move\n" +
		"Scale Y: Easing Type\n" +
		"Scale Z: Loop Type\n" +
		"Rotation Z: Loops Amount (set to -1 for infinite)"
	)]
	[NaughtyAttributes.ReadOnly]
	public bool ignoreMePls;

	bool gaveItem;

	public void GiveItem(SOItem item) => Player.current.AddItem(item);
	public void GiveItemOnce(SOItem item)
	{
		if (!gaveItem)
		{
			gaveItem = true;
			Player.current.AddItem(item);
		}
	}
	public bool UseItem(SOItem item) => Player.current.UseItem(item);
	public void Say(string text) => Game.current.DisplayTextBox(name, text, 10f);
	public void AddTime(float amount) => Game.current.AddTime(amount);
	public void DestroyObj(Object obj) => Destroy(obj);
	public void MoveTo(Transform position) => transform.DOMove(position.position, position.localScale.x).SetEase((Ease)(int)position.localScale.y).SetLoops((int)position.eulerAngles.z, (LoopType)(int)position.localScale.z);
	public void PlaySound(AudioClip clip) => Game.current.PlaySound(clip);
	public void ChangeBG(Sprite bg) => Game.current.ChangeBG(bg);
}