using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NovelManager : MonoBehaviour
{
    [System.Serializable]
    public class NovelPlaylistItem
    {
        public NovelSequence sequence;
        public bool playAutomatically = true;
    }

    [SerializeField] private NovelPlaylistItem[] novelScenes;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI novelText;
    [SerializeField] private GameObject textPanel;
    [SerializeField] private float lettersPerSecond = 30f;

    private NovelSequence.SceneData currentScene;
    private int currentPlaylistIndex = 0;
    private int currentSceneIndex = 0;
    private int currentPageIndex = 0;
    private bool isTyping;
    private Coroutine typingCoroutine;

    public static NovelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            textPanel.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (novelScenes.Length > 0 && novelScenes[0].playAutomatically)
        {
            StartSequence(0);
        }
    }

    public void StartSequence(int playlistIndex)
    {
        if (playlistIndex >= novelScenes.Length) return;

        currentPlaylistIndex = playlistIndex;
        currentSceneIndex = 0;
        currentPageIndex = 0;

        var sequence = novelScenes[playlistIndex].sequence;
        novelText.font = sequence.font;
        novelText.color = sequence.textColor;

        StartCoroutine(PlayNextScene());
    }

    private IEnumerator PlayNextScene()
    {
        novelText.text = "";
        textPanel.SetActive(false);

        var currentSequence = novelScenes[currentPlaylistIndex].sequence;

        if (currentSceneIndex >= currentSequence.scenes.Length)
        {
            EndSequence();
            yield break;
        }

        currentScene = currentSequence.scenes[currentSceneIndex];
        currentPageIndex = 0;

        yield return StartCoroutine(ChangeBackground(currentScene.background));

        textPanel.SetActive(true);
        typingCoroutine = StartCoroutine(TypeText(currentScene.pages[currentPageIndex]));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        novelText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            novelText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }

    private void Update()
    {
        if (!textPanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                novelText.text = currentScene.pages[currentPageIndex];
                isTyping = false;
            }
            else
            {
                currentPageIndex++;
                var currentSequence = novelScenes[currentPlaylistIndex].sequence;

                if (currentPageIndex < currentScene.pages.Length)
                {
                    typingCoroutine = StartCoroutine(TypeText(currentScene.pages[currentPageIndex]));
                }
                else
                {
                    currentSceneIndex++;
                    StartCoroutine(PlayNextScene());
                }
            }
        }
    }

    private void EndSequence()
    {
        textPanel.SetActive(false);
        currentPlaylistIndex++;
        if (currentPlaylistIndex < novelScenes.Length)
        {
            StartSequence(currentPlaylistIndex);
        }
    }

    private IEnumerator ChangeBackground(Sprite newBackground)
    {
        if (textPanel.activeSelf)
        {
            yield return StartCoroutine(FadeText(1f, 0f, 0.2f));
        }
        float fadeTime = 0.5f;
        CanvasGroup canvasGroup = backgroundImage.GetComponent<CanvasGroup>();

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / fadeTime;
            yield return null;
        }
        backgroundImage.sprite = newBackground;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / fadeTime;
            yield return null;
        }
        yield return StartCoroutine(FadeText(0f, 1f, 0.3f));
    }

    private IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        CanvasGroup textCanvasGroup = textPanel.GetComponent<CanvasGroup>();
        if (!textCanvasGroup) textCanvasGroup = textPanel.AddComponent<CanvasGroup>();

        float elapsed = 0;
        while (elapsed < duration)
        {
            textCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        textCanvasGroup.alpha = endAlpha;
    }


}