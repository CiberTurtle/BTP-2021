using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Game/Item", order = 0)]
public class SOItem : ScriptableObject
{
	[NaughtyAttributes.ShowAssetPreview] public Sprite sprite;
	[NaughtyAttributes.ResizableTextArea] public string description;
}