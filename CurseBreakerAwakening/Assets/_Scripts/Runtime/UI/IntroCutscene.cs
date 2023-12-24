using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IntroCutscene : MonoBehaviour
{
    public CutseneSequence[] sequences;

    private Sequence _wholeSequence;

    private void Start()
    {
        _wholeSequence = DOTween.Sequence();

        bool waitForPrevious = false;

        for (int i = 0; i < sequences.Length; i++) 
        {
            sequences[i].SetAnimation();

            if (sequences[i].MainTweenType == ETweenType.Wait)
            {
                if (waitForPrevious)
                    _wholeSequence.Append(DOVirtual.DelayedCall(sequences[i].Duration, null));
                else
                    _wholeSequence.Join(DOVirtual.DelayedCall(sequences[i].Duration, null));

                waitForPrevious = true;

                continue;
            }

            if (waitForPrevious)
            {
                _wholeSequence.Append(sequences[i].Tween);
            }
            else
            {
                _wholeSequence.Join(sequences[i].Tween);
            }

            waitForPrevious = sequences[i].WaitForCompletion;

        }

        _wholeSequence.Play().OnPlay(() =>Debug.Log(_wholeSequence.stringId));
    }

    private void Update()
    {
        Debug.Log(Time.time);
    }

    private void OnValidate()
    {
        if (sequences == null)
            return;

        float cumulativeTime = 0f;

        bool waitForPrevious = false;

        for(int i = 0; i < sequences.Length;i++) 
        {
            if (sequences[i].MainTweenType == ETweenType.Wait)
            {
                sequences[i].StartTime = cumulativeTime;

                cumulativeTime += sequences[i].Duration;
                sequences[i].EndTime = cumulativeTime;

                sequences[i].UpdateSequenceName(i);

                waitForPrevious = true;

                continue;
            }

            if (waitForPrevious)
            {
                sequences[i].StartTime = cumulativeTime;

                cumulativeTime += sequences[i].Duration;
                sequences[i].EndTime = cumulativeTime;

                sequences[i].UpdateSequenceName(i);
            }
            else
            {
                sequences[i].StartTime = sequences[i - 1].StartTime;

                sequences[i].EndTime = sequences[i - 1].StartTime + sequences[i].Duration;
            }

            sequences[i].UpdateSequenceName(i);

            waitForPrevious = sequences[i].WaitForCompletion;
        }
    }
}

[Serializable]
public class CutseneSequence
{
    [HideInInspector] public string SequenceName;
    [HideInInspector] public float StartTime;
    [HideInInspector] public float EndTime;

    public Graphic UIElement;
    public ETweenType MainTweenType = ETweenType.MoveX;
    public float From;
    public float To;
    public float Duration = 1;
    public bool WaitForCompletion = true;
    public Ease EaseType = Ease.Linear;

    public Tween Tween;

    public void UpdateSequenceName(int i)
    {
        TimeSpan start = TimeSpan.FromSeconds(StartTime);
        TimeSpan end = TimeSpan.FromSeconds(EndTime);

        string startTimeString = String.Format("{0:00}:{1:00}:{2:00}", (int)start.TotalMinutes, start.Seconds, start.Milliseconds / 10);
        string endTimeString = String.Format("{0:00}:{1:00}:{2:00}", (int)end.TotalMinutes, end.Seconds, end.Milliseconds / 10);

        if (MainTweenType == ETweenType.Wait)
        {
            SequenceName = $"{i + 1}: Wait for {Duration} seconds \t ({startTimeString} - {endTimeString})";
        }
        else
        {
            SequenceName = $"{i + 1}: {MainTweenType.ToString()} {(UIElement != null ?  UIElement.name : "")} \t ({startTimeString} - {endTimeString})";
        }
    }

    public void SetAnimation()
    {
        switch (MainTweenType)
        {
            case ETweenType.MoveX:
                SetMoveX();
                break;
            case ETweenType.MoveY:
                SetMoveY();
                break;
            case ETweenType.Scale:
                SetScale();
                break;
            case ETweenType.Fade:
                SetFade();
                break;
            case ETweenType.Wait:
                break;
            default:
                break;
        }
    }

    public void SetMoveX()
    {
        this.Tween = UIElement.rectTransform.DOAnchorPosX(To, Duration).SetEase(EaseType);

        this.Tween.OnPlay(() => UIElement.rectTransform.anchoredPosition = Vector2.right * From + Vector2.up * UIElement.rectTransform.anchoredPosition.y);
    }

    public void SetMoveY()
    {
        this.Tween = UIElement.rectTransform.DOAnchorPosY(To, Duration).SetEase(EaseType);

        this.Tween.OnPlay(() => UIElement.rectTransform.anchoredPosition = Vector2.right * UIElement.rectTransform.anchoredPosition.x + Vector2.up * From);
    }

    public void SetScale()
    {
        this.Tween = UIElement.rectTransform.DOScale(To, Duration).SetEase(EaseType);

        this.Tween.OnPlay(() => UIElement.rectTransform.localScale = Vector3.one * From);
    }

    public void SetFade()
    {
        Color color = UIElement.color;
        color.a = From;
        
        this.Tween = UIElement.DOFade(To, Duration).SetEase(EaseType);
        this.Tween.OnPlay(() => UIElement.color = color);
    }

    [ContextMenu("Reset")]
    public void Reset()
    {
        UIElement = null;
    }
}

public enum ETweenType
{
    MoveX,
    MoveY,
    Scale,
    Fade,
    Wait
}
