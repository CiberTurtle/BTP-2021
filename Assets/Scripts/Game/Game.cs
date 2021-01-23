using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Game : MonoBehaviour
{
	public static Game current;

	public float timeToLive;
	[Space]
	public TMP_Text timeLeftText;
	public Image timeLeftBar;
	public Gradient timeColor;
	[Space]
	[SerializeField] Transform invContainer;
	[SerializeField] GameObject pfInvSlot;
	[Space]
	[NaughtyAttributes.ReadOnly] public float timeLeft;

	void Awake()
	{
		current = this;

		timeLeft = timeToLive;
	}

	public void UpdateHUD()
	{
		foreach (Transform child in invContainer)
			Destroy(child.gameObject);

		foreach (var item in Player.current.inv)
			// Very long
			Instantiate(pfInvSlot, invContainer).transform.GetChild(0).GetComponent<Image>().sprite = item.sprite;
	}

	void Update()
	{
		// if (timeLeft < 0)
		//	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

		timeLeft -= Time.deltaTime;

		var timeLeftFac = timeLeft / timeToLive;
		var color = timeColor.Evaluate(timeLeftFac);

		timeLeftText.text = Mathf.Round(timeLeft).ToString();
		timeLeftText.color = color;

		timeLeftBar.fillAmount = timeLeftFac;
		timeLeftBar.color = color;
	}
}