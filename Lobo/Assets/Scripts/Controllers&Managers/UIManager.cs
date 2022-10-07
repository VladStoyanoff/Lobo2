using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_InputField levelIF;
    int levelSetting;
    [SerializeField] TMP_InputField densityIF;
    int densitySetting;

    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] TMP_Text bestDensityText;
    [SerializeField] TMP_Text bestLevelText;
    [SerializeField] TMP_Text lastDensityText;
    [SerializeField] TMP_Text lastLevelText;

    [SerializeField] Image coverImage;
    [SerializeField] GameObject menuPanel;

    ScoreManager scoreManager;
    GameManager gameManager;

    bool hasParsedLevelSetting;
    bool hasParsedDensitySetting;

    void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        GameManager.OnGameStarted += GameManager_OnGameStarted;
        GameManager.OnGameEnded += GameManager_OnGameEnded;
        UpdateHighScore();
    }

    void GameManager_OnGameStarted(object sender, EventArgs e)
    {
        if (hasParsedLevelSetting == false || hasParsedDensitySetting == false)
        {
            Debug.LogError("In order to start the game, the level and density settings must be set");
            return;
        }
        gameManager.SetIsGameActiveBool(true);
        menuPanel.SetActive(false);
        coverImage.gameObject.GetComponent<Image>().enabled = false;
        scoreManager.ClearScore();
    }

    void GameManager_OnGameEnded(object sender, EventArgs e)
    {
        coverImage.gameObject.GetComponent<Image>().enabled = true;
        menuPanel.SetActive(true);
        gameManager.SetIsGameActiveBool(false);

        lastDensityText.text = densitySetting.ToString();
        lastLevelText.text = levelSetting.ToString();

        if (scoreManager.GetNewHighScoreSetBool() == false) return;
        UpdateHighScore();
        scoreManager.SetHighScoreBool(false); 
    }

    void Update()
    {
        scoreText.text = "Score: " + scoreManager.GetScore().ToString();
    }

    void UpdateHighScore()
    {
        highScoreText.text = "High Score: " + scoreManager.GetBestScore().ToString();
        bestLevelText.text = scoreManager.GetBestLevel().ToString();
        bestDensityText.text = scoreManager.GetBestDensity().ToString();
    }

    public void ReadLevelIF()
    {
        hasParsedLevelSetting = int.TryParse(levelIF.GetComponent<TMP_InputField>().text, out var result);
        if (hasParsedLevelSetting && result > 0 && result < 10)
        {
            levelSetting = result;
        }
        else
        {
            Debug.LogError("The level setting will only accept integers from 1-9 as input");
            hasParsedLevelSetting = false;
        }
    }

    public void ReadDensityIF()
    {
        hasParsedDensitySetting = int.TryParse(densityIF.GetComponent<TMP_InputField>().text, out var result);
        if (hasParsedDensitySetting && result > 0 && result < 6)
        {
            densitySetting = result;
        }
        else
        {
            Debug.LogError("The density setting will only accept integers from 1-5 as input");
            hasParsedDensitySetting = false;
        }
    }

    public int GetLevelSetting() => levelSetting;
    public int GetDensitySetting() => densitySetting;
}
