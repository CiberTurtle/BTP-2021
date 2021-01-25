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
	[SerializeField] GameObject itemBox;
	[SerializeField] TMP_Text itemBoxTitle;
	[SerializeField] TMP_Text itemBoxText;
	[SerializeField] Image itemBoxIcon;
	[Space]
	[SerializeField] UnityEngine.Experimental.Rendering.Universal.PixelPerfectCamera cam;
	[Space]
	[NaughtyAttributes.ReadOnly] public bool PAUSED = false;
	[NaughtyAttributes.ReadOnly] public float timeLeft;

	float textBoxDisplayTime = 0;
	float itemBoxDisplayTime = 0;
	bool unlimitedTime = false;

	Inputs inputs;

	void Awake()
	{
		current = this;

		inputs = new Inputs();
		inputs.Game.Pause.performed += (x) => PAUSED = !PAUSED;
#if UNITY_EDITOR
		inputs.Game.UnlimitedTime.performed += (x) =>
		{
			unlimitedTime = !unlimitedTime;

			DisplayTextBox("[ Game ]", unlimitedTime ? "Toggled Unlimited Time <color=green>On</color>" : "Toggled Unlimited Time <color=red>Off</color>", 1.5f);
		};
#endif

		timeLeft = timeToLive;
	}

	void OnEnable() => inputs.Enable();
	void OnDisable() => inputs.Disable();

	void Start()
	{
		cam.enabled = Screen.width % 2 == 0 && Screen.height % 2 == 0;

		UpdateHUD();
	}

	void Update()
	{
		if (PAUSED) Time.timeScale = 0;
		else Time.timeScale = 1;

		if (timeLeft < 0)
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

		if (!unlimitedTime)
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

		if (itemBoxDisplayTime < 0)
			itemBox.SetActive(false);
		itemBoxDisplayTime -= Time.deltaTime;
	}

	public void UpdateHUD()
	{
		foreach (Transform child in invContainer)
			Destroy(child.gameObject);

		foreach (var item in Player.current.inv)
			Instantiate(pfInvSlot, invContainer).transform.GetChild(0).GetComponent<Image>().sprite = item.sprite;
	}

	public void DisplayTextBox(string title, string text)
	{
		DisplayTextBox(title, text, Mathf.Clamp(text.Length * 0.075f, 3, 8));
	}

	public void DisplayTextBox(string title, string text, float time = 0f)
	{
		textbox.SetActive(true);
		textBoxDisplayTime = time;

		textBoxTitle.text = title;
		textBoxText.text = text;
	}

	public void DisplayItemBox(SOItem item)
	{
		itemBox.SetActive(true);
		itemBoxDisplayTime = Mathf.Clamp(item.name.Length * 0.15f + item.description.Length * 0.05f, 3, 10);

		itemBoxTitle.text = $"You Picked Up: {item.name}!";
		itemBoxText.text = item.description;
		itemBoxIcon.sprite = item.sprite;
	}

	public void AddTime(float time)
	{
		timeLeft += time;

		timeLeft = Mathf.Clamp(timeLeft, 0, timeToLive);
	}
}