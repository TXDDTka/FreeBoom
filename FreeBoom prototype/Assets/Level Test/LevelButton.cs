using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum LevelState { Completed, Open, Locked }

public class LevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, 
                                          IPointerDownHandler, IPointerUpHandler
{
    private LevelState state = LevelState.Locked;
    private int levelIndex = -1;
    private Vector3 startSize = Vector3.one;

    private IEnumerator buttonRoutine = null;
    private LevelManager LM = null;
    private RectTransform rt = null;
    private Text levelIndexText = null;
    private Image img = null;

    private void Awake()
    {
        levelIndexText = GetComponentInChildren<Text>();
        img = GetComponent<Image>();
        rt = GetComponent<RectTransform>();

        startSize = rt.localScale;
    }

    private void Start() => LM = LevelManager.Instance;

    public void SetIndex(int index)
    {
        levelIndex = index;
        levelIndexText.text = levelIndex.ToString();
    }

    public void SetState(LevelState newState)
    {
        state = newState;
        img.color = GetColor();
    }

    #region ES

    public void OnPointerDown(PointerEventData eventData)
    {
        //if (state != LevelState.Locked)
        //{
        //    if (PointInside()) LM.InvokeClickRoutine(true, levelIndex);
        //}
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (state != LevelState.Locked)
        {
            if (PointInside()) LM.InvokeClickRoutine(true, levelIndex);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (state != LevelState.Locked)
        {
            InvokeButtonRoutine(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (state != LevelState.Locked)
        {
            InvokeButtonRoutine(false);
        }
    }

    #endregion

    private void InvokeButtonRoutine(bool enter)
    {
        if (buttonRoutine != null) StopCoroutine(buttonRoutine);

        buttonRoutine = ButtonRoutine(enter);
        StartCoroutine(buttonRoutine);
    }

    private IEnumerator ButtonRoutine(bool enter)
    {
        float percent = 0;
        float smoothPercent = 0;
        float speed = 1f / LM.Duration;

        Vector3 currentSize = rt.localScale;
        Vector3 desiredSize = enter ? LM.MaxSize : Vector3.one;

        while (percent < 1)
        {
            percent += speed * Time.deltaTime;
            smoothPercent = LM.SmoothCurve.Evaluate(percent);
            rt.localScale = Vector3.MoveTowards(currentSize, desiredSize, smoothPercent);

            yield return null;
        }
    }

    private bool PointInside(Vector3 position, Vector3[] worldCorners)
    {
        //bottom left -> top left -> top right -> bottom right
        //using bottom left and top right to detect

        Vector3 v0 = worldCorners[0];
        Vector3 v2 = worldCorners[2];

        bool inside = position.x > v0.x && position.x < v2.x &&
                      position.y > v0.y && position.y < v2.y;

        return inside;
    }

    private bool PointInside()
    {
        //bottom left -> top left -> top right -> bottom right
        //using bottom left and top right to detect

        Vector3 position = Input.mousePosition;
        Vector3[] worldCorners = new Vector3[4];
        rt.GetWorldCorners(worldCorners);

        Vector3 v0 = worldCorners[0];
        Vector3 v2 = worldCorners[2];

        bool inside = position.x > v0.x && position.x < v2.x &&
                      position.y > v0.y && position.y < v2.y;

        return inside;
    }

    private Color GetColor()
    {
        Color c = Color.white;

        switch (state)
        {
            case LevelState.Completed: c = Color.green; break;
            case LevelState.Open: c = Color.blue; break;
            case LevelState.Locked: c = Color.red; break;
        }
        return c;
    }
}
