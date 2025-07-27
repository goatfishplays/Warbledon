using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InputSliderSync : MonoBehaviour
{
    [SerializeField] protected float minVal = 0f;
    [SerializeField] protected float maxVal = 5f;
    [SerializeField] protected float _curVal = 1f;
    public float curVal => _curVal;
    [SerializeField] protected Slider slider;
    [SerializeField] protected TMP_InputField inputField;

    [SerializeField] protected UnityEvent<float> m_OnValueChanged = new UnityEvent<float>();
    public UnityEvent<float> onValueChanged { get { return m_OnValueChanged; } set { m_OnValueChanged = value; } }

    protected void Awake()
    {
        slider.minValue = minVal;
        slider.maxValue = maxVal;
        slider.value = _curVal;
        // inputField.text = _curVal.ToString(); 
    }


    /// <summary>
    /// Updates the text box upon slider changes
    /// </summary>
    /// <param name="val"></param>
    public void OnSliderChange(float val)
    {
        _curVal = val;
        // Debug.Log($"{name} had value set to {_curVal}");
        inputField.text = _curVal.ToString("F3");
        m_OnValueChanged.Invoke(_curVal);
    }

    /// <summary>
    /// Updates the slider upon text change
    /// </summary>
    /// <param name="val"></param>
    public void OnTextChange(string val)
    {
        if (!float.TryParse(val, out float newVal))
        {
            Debug.LogError($"{name} received a non-numerical value");
            inputField.text = _curVal.ToString();
            return;
        }
        SetValue(newVal);
    }


    public void SetValue(float val)
    {
        _curVal = Mathf.Clamp(val, minVal, maxVal);
        // Debug.Log($"{name} had value set to {_curVal}");
        slider.value = _curVal;
        inputField.text = _curVal.ToString();
        m_OnValueChanged.Invoke(_curVal);
    }
}
