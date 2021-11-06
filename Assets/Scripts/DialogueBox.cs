using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    private class TextData
    {
        public string text;
        public float timeToShow;
        public float lockTime;
        public float BeginTime { get; set; }
        public bool Skippable { get { return DeltaTime >  lockTime; } }
        public float Diff { get { return 1 - (timeToShow - DeltaTime) / timeToShow; } }

        public float DeltaTime { get { return Time.realtimeSinceStartup - BeginTime; } }


        public TextData(string text, float timeToShow, float lockTime) 
        {
            this.text = text;
            this.timeToShow = timeToShow;
            this.lockTime = lockTime;
            BeginTime = 0;
        }
    }
    [SerializeField] TMP_Text mainTextField;
    [SerializeField] TMP_Text continueTextField;
    private readonly Queue<TextData> _textQueue = new Queue<TextData>();
    private TextData? currentPair = null;
    private void Start()
    {

    }
    private void Update()
    {
        if (_textQueue.Count > 0 && currentPair is null)
        {
            Skip();
        }
        if (!(currentPair is null))
        {
            int index = Mathf.CeilToInt(currentPair.text.Length * Mathf.Min(1, currentPair.Diff));
            mainTextField.SetText(currentPair.text.Substring(0, index));
            continueTextField.alpha = 4 * Mathf.Clamp01((currentPair.DeltaTime - currentPair.lockTime - currentPair.timeToShow) / (currentPair.timeToShow + currentPair.lockTime));
        }
    }
    private void OnMouseDown()
    {
        if (!(currentPair is null))
        {
            if (currentPair.Diff < 1)
            {
                currentPair.timeToShow = Time.realtimeSinceStartup - currentPair.BeginTime;
            } else if(currentPair.Skippable)
            {
                Skip();
            }
        }
    }


    public void Skip()
    {
        if (_textQueue.Count > 0)
        {
            currentPair = _textQueue.Dequeue();
            if (!(currentPair is null))
            {
                currentPair.BeginTime = Time.realtimeSinceStartup;
            }
        }
        else
        {
            currentPair = null;
            gameObject.SetActive(false);
        }
        mainTextField.SetText("");
    }

    public bool IsEmpty()
    {
        return _textQueue.Count == 0;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="text">text to show</param>
    /// <param name="timing"></param>
    public void AddTextToQueue(string text, float timing, float lockTime = 0)
    {
        _textQueue.Enqueue(new TextData(text, timing, lockTime));
        gameObject.SetActive(true);
    }

    public void ResetQueue()
    {
        _textQueue.Clear();
    }
}
