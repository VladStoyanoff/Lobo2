using System;
using System.IO;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    static int currentScore = 0;
    bool newHighScoreSet;
    int bestScore;
    int bestLevel;
    int bestDensity;

    void Start()
    {
        GameManager.OnGameEnded += GameManager_OnGameEnded;
        LoadBestScore();
    }

    void GameManager_OnGameEnded(object sender, EventArgs e)
    {
        TrySaveBestScore();
        LoadBestScore();
    }

    public void ModifyScore(int score)
    {
        currentScore += score;
        currentScore = Mathf.Clamp(currentScore, 0, int.MaxValue);
    }

    public void ClearScore()
    {
        currentScore = 0;
    }

    public void SetHighScoreBool(bool boolean)
    {
        newHighScoreSet = boolean;
    }

    [Serializable]
    class SaveBestData
    {
        public int score;
        public int density;
        public int level;
    }

    [Serializable]
    class SaveLastRun
    {
        public int density;
        public int level;
    }
    
    void TrySaveBestScore()
    {
        var uiManager = FindObjectOfType<UIManager>();

        var data = new SaveBestData();
        if (currentScore < bestScore) return;
        newHighScoreSet = true;
        data.score = currentScore;
        data.density = uiManager.GetDensitySetting();
        data.level = uiManager.GetLevelSetting();
        var json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/saveBestScoreFile.json", json);
    }

    void LoadBestScore()
    {
        var path = Application.persistentDataPath + "/saveBestScoreFile.json";
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<SaveBestData>(json);

            bestScore = data.score;
            bestDensity = data.density;
            bestLevel = data.level;
        }
    }

    public int GetScore() => currentScore;
    public int GetBestScore() => bestScore;
    public int GetBestLevel() => bestLevel;
    public int GetBestDensity() => bestDensity;
    public bool GetNewHighScoreSetBool() => newHighScoreSet;
}
