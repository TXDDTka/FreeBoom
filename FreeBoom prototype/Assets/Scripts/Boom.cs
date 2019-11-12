using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boom : MonoBehaviour
{
    [SerializeField] private Button launchButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private AnimationCurve[] curves;
    [SerializeField] private Image image;
    [SerializeField, Range(0, 5)] private float duration = 1;
    [SerializeField] private float maxForce = 1000;
    //private float amount;
    //private float smoothAmount;
    private AnimationCurve finalCurve;
    private int currentIndex;
    private float smoothPercent;
    private IEnumerator boomRoutine;

    private void Start()
    {
        launchButton.onClick.AddListener(() => OnClickLaunchBoom());
        restartButton.onClick.AddListener(() => OnClickRestartBoom());

        OnClickRestartBoom();
    }

    private void Update()
    {
        //amount += duration * Time.deltaTime;
        //if (amount >= 1) amount -= 1;

        //smoothAmount = finalCurve.Evaluate(amount);
        //image.fillAmount = smoothAmount;
    }

    private void OnClickLaunchBoom()
    {
        Debug.Log($"запуск с силой {smoothPercent * maxForce}");
        if (boomRoutine != null) StopCoroutine(boomRoutine);
        ResetRoutine();
    }

    private void OnClickRestartBoom()
    {
        currentIndex = Random.Range(0, curves.Length);
        finalCurve = curves[currentIndex];

        boomRoutine = BoomRoutine();
        StartCoroutine(boomRoutine);
    }

    private void ResetRoutine()
    {
        image.gameObject.SetActive(false);
        launchButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
        smoothPercent = 0;
    }

    private IEnumerator BoomRoutine()
    {
        image.gameObject.SetActive(true);
        launchButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);

        float percent = 0;
        smoothPercent = 0;
        float speed = 1f / duration;

        float currentAmount = 0;
        float desiredAmount = 1;

        while (percent < 1)
        {
            percent += speed * Time.deltaTime;
            smoothPercent = finalCurve.Evaluate(percent);
            image.fillAmount = Mathf.MoveTowards(currentAmount, desiredAmount, smoothPercent);

            yield return null;
        }

        ResetRoutine();
    }
}
