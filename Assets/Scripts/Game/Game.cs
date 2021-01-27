using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

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
	[SerializeField] CanvasGroup textbox;
	[SerializeField] TMP_Text textBoxTitle;
	[SerializeField] TMP_Text textBoxText;
	[Space]
	[SerializeField] CanvasGroup itemBox;
	[SerializeField] TMP_Text itemBoxTitle;
	[SerializeField] TMP_Text itemBoxText;
	[SerializeField] Image itemBoxIcon;
	[Space]
	[SerializeField] TMP_Text timeText;
	[SerializeField] TMP_Text completionText;
	[Space]
	[SerializeField] GameObject pauseMenu;
	[Space]
	public SOGameSave resetGameSave;
	[SerializeField] UnityEngine.Experimental.Rendering.Universal.PixelPerfectCamera cam;
	[Space]
	[NaughtyAttributes.ReadOnly] public bool PAUSED = false;
	[NaughtyAttributes.ReadOnly] public float timeLeft;
	[NaughtyAttributes.ReadOnly] public float timeAlive;

	[HideInInspector] public static SOGameSave gameSave = null;
	[HideInInspector] public int npcsToHelp;
	bool unlimitedTime = false;

	DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> textboxPopAnim;
	DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> textboxFadeAnim;

	DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> itemboxPopAnim;
	DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> itemboxFadeAnim;

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

		if (gameSave == null) gameSave = Instantiate(resetGameSave);
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
		if (PAUSED)
		{
			Time.timeScale = 0;
			pauseMenu.SetActive(true);
		}
		else
		{
			Time.timeScale = 1;
			pauseMenu.SetActive(false);
		}

		timeAlive += Time.deltaTime;

		if (timeLeft < 0)
			Respawn();

		if (!unlimitedTime)
			timeLeft -= Time.deltaTime;

		var timeLeftFac = timeLeft / timeToLive;
		var color = timeColor.Evaluate(timeLeftFac);

		timeLeftText.text = Mathf.Round(timeLeft).ToString();
		timeLeftText.color = color;

		timeLeftBar.fillAmount = timeLeftFac;
		timeLeftBar.color = color;

		timeText.text =
		TimeSpan.FromSeconds(timeAlive).ToString(@"m\:ss") + " / " + TimeSpan.FromSeconds(gameSave.currentTime).ToString(@"m\:ss");
		completionText.text = $"{gameSave.npcsHelped} / {npcsToHelp}";
	}

	public void UpdateHUD()
	{
		foreach (Transform child in invContainer)
			Destroy(child.gameObject);

		int index = 0;
		foreach (var item in Player.current.inv)
		{
			var box = Instantiate(pfInvSlot, invContainer).transform.GetChild(0).GetComponent<Image>();
			box.sprite = item.sprite;
			box.DOColor(Color.white, (index + 1) * 0.25f);

			index++;
		}
	}

	public void HelpedNPC()
	{
		gameSave.npcsHelped++;

		if (gameSave.npcsHelped >= npcsToHelp)
			Debug.Log("YOU HELPED ALL THE NPCs!!!");
	}

	public void DisplayTextBox(string title, string text)
	{
		DisplayTextBox(title, text, Mathf.Clamp(text.Length * 0.075f, 3, 8));
	}

	public void DisplayTextBox(string title, string text, float time = 0f)
	{
		if (textboxPopAnim != null)
		{
			textboxPopAnim.Kill();
			textboxFadeAnim.Kill();
		}
		textbox.alpha = 0;
		textbox.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
		textboxPopAnim = textbox.transform.DOScale(Vector3.one, 0.1f);
		textboxFadeAnim = textbox.DOFade(1f, 0.1f)
		.OnComplete(() => textboxFadeAnim = textbox.DOFade(0, 1f).SetDelay(time));

		textBoxTitle.text = title;
		textBoxText.text = text;
	}

	public void DisplayItemBox(SOItem item)
	{
		if (itemboxPopAnim != null)
		{
			itemboxPopAnim.Kill();
			itemboxFadeAnim.Kill();
		}
		itemBox.alpha = 0;
		itemBox.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
		itemboxPopAnim = itemBox.transform.DOScale(Vector3.one, 0.1f);
		itemboxFadeAnim = itemBox.DOFade(1f, 0.1f)
		.OnComplete(() => itemboxFadeAnim = itemBox.DOFade(0, 1f).SetDelay(Mathf.Clamp(item.name.Length * 0.15f + item.description.Length * 0.05f, 3, 10)));

		itemBoxTitle.text = $"You Picked Up: {item.name}!";
		itemBoxText.text = item.description;
		itemBoxIcon.sprite = item.sprite;
	}

	public void AddTime(float time)
	{
		timeLeft += time;

		timeLeft = Mathf.Clamp(timeLeft, 0, timeToLive);
	}

	public void Resume()
	{
		PAUSED = false;
	}

	public void Respawn()
	{
		gameSave.currentTime += timeAlive;

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void Restart()
	{
		gameSave = Instantiate(resetGameSave);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void Exit()
	{
		Application.Quit();
	}

	public void End()
	{
		Debug.Log("YOU BEAT THE GAME!!! pog");
	}
}