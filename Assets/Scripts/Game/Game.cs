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
	[SerializeField] GameObject textbox;
	[SerializeField] TMP_Text textBoxTitle;
	[SerializeField] TMP_Text textBoxText;
	[Space]
	[NaughtyAttributes.ReadOnly] public float timeLeft;

	float textBoxDisplayTime;

	void Awake()
	{
		current = this;

		timeLeft = timeToLive;
	}

	void Start()
	{
		UpdateHUD();
	}

	void Update()
	{
		if (timeLeft < 0)
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

		timeLeft -= Time.deltaTime;

		var timeLeftFac = timeLeft / timeToLive;
		var color = timeColor.Evaluate(timeLeftFac);

		timeLeftText.text = Mathf.Round(timeLeft).ToString();
		timeLeftText.color = color;

		timeLeftBar.fillAmount = timeLeftFac;
		timeLeftBar.color = color;

		if (textBoxDisplayTime < 0)
			textbox.SetActive(false);
		textBoxDisplayTime -= Time.deltaTime;
	}

	public void UpdateHUD()
	{
		foreach (Transform child in invContainer)
			Destroy(child.gameObject);

		foreach (var item in Player.current.inv)
			// Very long
			Instantiate(pfInvSlot, invContainer).transform.GetChild(0).GetComponent<Image>().sprite = item.sprite;
	}

	public void DisplayTextBox(string title, string text)
	{
		DisplayTextBox(title, text, Mathf.Clamp(text.Length * 0.1f, 3, 10));
	}

	public void DisplayTextBox(string title, string text, float time = 0f)
	{
		textbox.SetActive(true);
		textBoxDisplayTime = time;

		textBoxTitle.text = title;
		textBoxText.text = text;
	}

	public void AddTime(float time)
	{
		timeLeft += time;

		timeLeft = Mathf.Clamp(timeLeft, 0, timeToLive);
	}
}