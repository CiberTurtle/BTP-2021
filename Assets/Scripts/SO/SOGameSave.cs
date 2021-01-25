using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Save", menuName = "Game/Game Save", order = 1)]
public class SOGameSave : ScriptableObject
{
	public List<SOItem> gemsLeft = new List<SOItem>();
	public float currentTime;
}