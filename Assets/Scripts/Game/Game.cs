using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
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
	[SerializeField] GameObject minimap;
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
	[SerializeField] Color completedColor;
	[Space]
	[SerializeField] GameObject pauseMenu;
	[Space]
	[SerializeField] CanvasGroup beatGameMenu;
	[SerializeField] TMP_Text timeBeatText;
	[SerializeField] TMP_Text completionBeatText;
	[SerializeField] TMP_Text tagBeatText;
	[Space]
	[SerializeField] Image deathBG;
	[SerializeField] Transform deathCover;
	[Space]
	[SerializeField] SpriteRenderer bgBase;
	[SerializeField] SpriteRenderer bgOverlay;
	[Space]
	[SerializeField] AudioClip winSound;
	[SerializeField] AudioSource music;
	[Space]
	[SerializeField] UnityEngine.Experimental.Rendering.Universal.PixelPerfectCamera cam;
	[Space]
	[NaughtyAttributes.ReadOnly] public bool PAUSED = false;
	[NaughtyAttributes.ReadOnly] public float timeLeft;
	[NaughtyAttributes.ReadOnly] public float timeAlive;

	[HideInInspector] public bool hasWon;
	[HideInInspector] public int npcsToHelp;
	[HideInInspector] public int npcsHelped;
	bool unlimitedTime = false;
	bool isBGTransitioning;

	DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> textboxPopAnim;
	DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> textboxFadeAnim;

	DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> itemboxPopAnim;
	DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> itemboxFadeAnim;

	DG.Tweening.Core.TweenerCore<Color, Color, DG.Tweening.Plugins.Options.ColorOptions> bgAnimBase;
	DG.Tweening.Core.TweenerCore<Color, Color, DG.Tweening.Plugins.Options.ColorOptions> bgAnimOverlay;

	Inputs inputs;

	void Awake()
	{
		current = this;

		inputs = new Inputs();
		inputs.Game.Pause.performed += (x) => PAUSED = !PAUSED;

		inputs.Game.ScreenShot.performed += (x) =>
		{
			ScreenCapture.CaptureScreenshot(Application.dataPath + "/Screen Shots/" + DateTime.Now.ToString("yyyy-mm-dd hh.mm.ss") + ".screenshot.png");
		};

#if UNITY_EDITOR
		inputs.Game.UnlimitedTime.performed += (x) =>
		{
			unlimitedTime = !unlimitedTime;

			DisplayTextBox("[ Game ]", unlimitedTime ? "Toggled Unlimited Time <color=green>On</color>" : "Toggled Unlimited Time <color=red>Off</color>", 1.5f);
		};
#endif

		timeLeft = timeToLive;

		switch (PlayerPrefs.GetInt("mode", 0))
		{
			case 0:
				timeText.gameObject.SetActive(false);
				completionText.gameObject.SetActive(false);
				break;
			case 1:
				completionText.gameObject.SetActive(false);
				break;
			case 2: break;
			case 3: break;
		}

		music.enabled = PlayerPrefs.GetInt("music", 1) == 1;

		if (PlayerPrefs.GetInt("mode", 0) == 3)
		{
			timeLeftText.enabled = false;
			timeLeftBar.transform.parent.gameObject.SetActive(false);
			unlimitedTime = true;
		}
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
		if (hasWon) return;

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
			Restart(true);

		if (!unlimitedTime)
			timeLeft -= Time.deltaTime;

		var timeLeftFac = timeLeft / timeToLive;
		var color = timeColor.Evaluate(timeLeftFac);

		timeLeftText.text = Mathf.Round(timeLeft).ToString();
		timeLeftText.color = color;

		timeLeftBar.fillAmount = Mathf.Lerp(timeLeftBar.fillAmount, timeLeftFac, 1f * Time.deltaTime * 10);
		timeLeftBar.color = color;

		timeText.text =
		TimeSpan.FromSeconds(timeAlive).ToString(@"m\:ss");
		completionText.text = $"{npcsHelped} / {npcsToHelp}";
	}

	public void PlaySound(AudioClip clip)
	{
		var source = transform.GetChild(0).gameObject.AddComponent<AudioSource>();

		source.clip = clip;
		source.Play();

		Destroy(source, clip.length);
	}

	public void HelpedNPC()
	{
		npcsHelped++;

		if (npcsHelped >= npcsToHelp)
			completionText.color = completedColor;
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
			box.transform.localScale = Vector3.zero;
			box.transform.DOScale(Vector3.one, 0.5f).SetDelay((index) * 0.05f)
			.SetEase(index == Player.current.inv.Count - 1 && Player.current.inv.Count > Player.current.lastInvSize ? Ease.OutElastic : Ease.OutBack);

			index++;
		}
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
		textboxPopAnim = textbox.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
		textboxFadeAnim = textbox.DOFade(1f, 0.2f)
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
		itemboxPopAnim = itemBox.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
		itemboxFadeAnim = itemBox.DOFade(1f, 0.2f)
		.OnComplete(() => itemboxFadeAnim = itemBox.DOFade(0, 1f).SetDelay(Mathf.Clamp(item.name.Length * 0.15f + item.description.Length * 0.05f, 3, 10)));

		itemBoxTitle.text = $"You Picked Up: {item.name}!";
		itemBoxText.text = item.description;
		itemBoxIcon.sprite = item.sprite;
	}

	public void ChangeBG(Sprite bg)
	{
		if (bgBase.sprite == bg || isBGTransitioning && bgOverlay.sprite == bg) return;

		if (isBGTransitioning)
		{
			if (bgAnimBase != null) bgAnimBase.Kill();
			if (bgAnimOverlay != null) bgAnimOverlay.Kill();

			bgBase.sprite = bg;
		}

		isBGTransitioning = true;

		bgOverlay.sprite = bg;

		bgBase.color = Color.white;
		bgBase.DOColor(new Color(1, 1, 1, 0), 1f).SetEase(Ease.Linear);

		bgOverlay.color = new Color(1, 1, 1, 0);
		bgAnimOverlay = bgOverlay.DOColor(Color.white, 1f).SetEase(Ease.Linear).OnComplete(() =>
		{
			bgBase.sprite = bgOverlay.sprite;
			bgBase.color = Color.white;
			bgOverlay.color = new Color(1, 1, 1, 0);
			isBGTransitioning = false;
		});
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

	public void Restart(bool isNatral = false)
	{
		hasWon = true;

		HideAllHUD();

		Player.current.gameObject.SetActive(false);

		if (isNatral)
		{
			deathBG.DOColor(Color.black, 0.75f).SetEase(Ease.Linear).SetUpdate(true);
			deathCover.DOScale(Vector3.one * 2f, 2f).SetEase(Ease.OutSine).SetUpdate(true).OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
		}
		else
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	public void Exit()
	{
		SceneManager.LoadScene(0);
	}

	public void End()
	{
		if (hasWon) return;

		PlaySound(winSound);

		beatGameMenu.transform.DOScale(Vector3.one, 1.25f).SetDelay(0.9f).SetEase(Ease.OutBack).SetUpdate(true);
		beatGameMenu.DOFade(1f, 1f).SetDelay(1f).SetEase(Ease.Linear).SetUpdate(true)
		.OnComplete(() =>
		{
			beatGameMenu.interactable = true;
			beatGameMenu.blocksRaycasts = true;
		});

		timeBeatText.text =
		TimeSpan.FromSeconds(timeAlive).ToString(@"m\:ss");
		completionBeatText.text = $"{npcsHelped} / {npcsToHelp}";

		hasWon = true;
		Time.timeScale = 0f;

		HideAllHUD();

		switch (PlayerPrefs.GetInt("mode", 0))
		{
			case 0: break;
			case 1:
				Leaderboards.current.AddNewHighscore(PlayerPrefs.GetString("name", "anonymous"), (int)timeAlive * 100, false);
				break;
			case 2:
				if (npcsHelped >= npcsToHelp)
					Leaderboards.current.AddNewHighscore(PlayerPrefs.GetString("name", "anonymous"), (int)timeAlive * 100, true);
				else
					Leaderboards.current.AddNewHighscore(PlayerPrefs.GetString("name", "anonymous"), (int)timeAlive * 100, false);
				break;
			case 3: break;
		}
	}

	private void HideAllHUD()
	{
		timeLeftText.enabled = false;
		timeLeftBar.transform.parent.gameObject.SetActive(false);
		invContainer.gameObject.SetActive(false);
		minimap.SetActive(false);
		timeText.gameObject.SetActive(false);
		completionText.gameObject.SetActive(false);
		textbox.gameObject.SetActive(false);
		itemBox.gameObject.SetActive(false);
	}
}