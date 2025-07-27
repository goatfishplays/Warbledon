using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordboxController : MonoBehaviour
{

    // TODO: needs a queue system for sending it text to display
    public TMP_Text text;
    public GameObject wordbox;
    // public bool timeLimited = true;
    // public float disappearTime = -1f;
    public Coroutine co_timeLimited = null;


    /// <summary>
    /// <para>
    /// Sets text of the wordbox and makes it visible for `time` seconds
    /// </para>
    /// <para>
    /// Set time non-positive for infinite time 
    /// </para>
    /// </summary>
    /// <param name="message"></param>
    /// <param name="time"></param>
    public void SetText(string message, float time = -1)
    {
        SetVisible(true);
        text.text = message;
        Debug.Log($"Wordbox text set to '{message}'");
        if (time >= 0)
        {
            co_timeLimited = StartCoroutine(Timer(time));
        }
        // disappearTime = time;
    }

    public void Clear()
    {
        if (co_timeLimited != null)
        {
            StopCoroutine(co_timeLimited);
        }
        SetVisible(false);
    }

    public void SetVisible(bool visible)
    {
        wordbox.SetActive(visible);
    }

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        SetVisible(false);
    }

}
