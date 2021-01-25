using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Game/Item", order = 0)]
public class SOItem : ScriptableObject
{
	[NaughtyAttributes.ShowAssetPreview(64, 64)] public Sprite sprite;
	[NaughtyAttributes.ResizableTextArea] public string description;
}