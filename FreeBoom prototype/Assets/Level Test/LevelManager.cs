using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LevelManager : MonoBehaviour
{
    #region Singleton

    public static LevelManager Instance { get; private set; }

    private void InitializeSingleton()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
    }

    #endregion

    #region Variables 

    [SerializeField] private Vector3 maxSize = new Vector3(1.3f, 1.3f, 1);
    [SerializeField, Range(0, 1)] private float duration = 0.5f;
    [SerializeField, Range(0, 3)] private float fadeDuration = 2f;
    [SerializeField] private AnimationCurve smoothCurve;
    [SerializeField] private AnimationCurve fadeCurve;
    [SerializeField] private CanvasGroup fadeCanvasGroup = null;
    [Space]
    [SerializeField] private GameObject selectButtons = null;
    [SerializeField] private GameObject levelInterface = null;
    [SerializeField] private Text currentLevelText = null;

    private int currentLevel = -1;
    private int lastOpened = 1;//def 1
    private int lastCompleted = 0;// def 0
    private LevelButton[] levelButtons = null;

    public Vector3 MaxSize => maxSize;
    public float Duration => duration;
    public AnimationCurve SmoothCurve => smoothCurve;

    #endregion

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        LoadData();
        PopulateButtons();
    }

    private void PopulateButtons()
    {
        levelButtons = selectButtons.GetComponentsInChildren<LevelButton>();

        for (int i = 0; i < levelButtons.Length; i++)
        {
            var btn = levelButtons[i];
            int lvlIndex = i + 1;
            LevelState state = LevelState.Locked;

            if (i < lastOpened) //OPENED == COMPLETED + 1
            {
                state = LevelState.Completed;

                if (i == lastCompleted) state = LevelState.Open;
                //if (i == lastOpened - 1) state = LevelState.Open; //WORKS FINE
            }

            btn.SetIndex(lvlIndex);
            btn.SetState(state);
        }
    }

    public void CompleteLevel(bool complete)
    {
        if (complete && currentLevel == lastOpened)
        {
            int btnCount = levelButtons.Length;

            if (lastCompleted < btnCount)
            {
                LevelButton completedBtn = levelButtons[lastCompleted];
                completedBtn.SetState(LevelState.Completed);

            }

            //if (lastOpened - 1 < btnCount) //WORKS FINE
            //{
            //    LevelButton completedBtn = levelButtons[lastOpened - 1];
            //    completedBtn.SetState(LevelState.Completed);

            //}

            if (lastOpened < btnCount)
            {
                LevelButton openedBtn = levelButtons[lastOpened];//is next - opened greater for 1
                openedBtn.SetState(LevelState.Open);
            }


            lastCompleted++;
            lastOpened++;
            SaveData();
        }

        InvokeClickRoutine(false);
    }

    public void InvokeClickRoutine(bool loadLevel, int levelIndex = 0)
    {
        currentLevel = levelIndex;
        StartCoroutine(ClickRoutine(loadLevel, levelIndex));
    }

    private IEnumerator ClickRoutine(bool loadLevel, int levelIndex)
    {
        float percent = 0;
        float smoothPercent = 0;
        float speed = 1f / fadeDuration;

        float currentAlpha = fadeCanvasGroup.alpha;

        while (percent < 1)
        {
            percent += speed * Time.deltaTime;

            if (percent >= 0.5f) TransitionToLevel(loadLevel, levelIndex);

            smoothPercent = fadeCurve.Evaluate(percent);
            fadeCanvasGroup.alpha = Mathf.MoveTowards(currentAlpha, 1, smoothPercent);

            yield return null;
        }

        fadeCanvasGroup.alpha = 0;
    }

    private void TransitionToLevel(bool loadLevel, int levelIndex)
    {
        levelInterface.SetActive(loadLevel);
        if (loadLevel) currentLevelText.text = levelIndex.ToString();

        selectButtons.SetActive(!loadLevel);
    }

    private void SaveData()
    {
        LevelsData lvlData = new LevelsData(lastOpened, lastCompleted);
        string data = JsonUtility.ToJson(lvlData, true);
        File.WriteAllText(Application.dataPath + "/save.json", data);

        //Debug.Log("saved");
    }

    private void LoadData()
    {
        string savePath = Application.dataPath + "/save.json";

        if (File.Exists(savePath))
        {
            string data = File.ReadAllText(savePath);
            LevelsData lvlData = JsonUtility.FromJson<LevelsData>(data);

            lastOpened = lvlData.LastOpened;
            lastCompleted = lvlData.LastCompleted;
        }
    }

    private class LevelsData
    {
        public int LastOpened;
        public int LastCompleted;

        public LevelsData(int lastOpened, int lastCompleted)
        {
            LastOpened = lastOpened;
            LastCompleted = lastCompleted;
        }
    }
}
