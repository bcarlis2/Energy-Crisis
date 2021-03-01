using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Color currentColor;
    [SerializeField] float value;
    [SerializeField] float maxValue = 100;

    void Start() {
        slider = GetComponent<Slider>();
        fill = GetComponent<Image>();

        setMaxValue(maxValue,value);
    }

    public float getValue() {
        return value;
    }


    public void SetValue(float inValue, bool charging) {
        if (slider) {
            //Debug.Log("SET VALUE " + inValue + ", MAX " + slider.maxValue);
            slider.value = inValue;
            this.value = slider.value;

            //Displays gradient depending on value
            //fill.color = gradient.Evaluate(slider.normalizedValue);

            float norm = slider.normalizedValue;
            Color newColor;
            //Debug.Log("NORM: " + norm);

            if (charging) {
                newColor = Color.cyan;
            } else if (norm < 0.05) {
                newColor = Color.black;
            } else if (norm <= 0.25) {
                newColor = Color.red;
            } else if (norm <= 0.5) {
                newColor = Color.yellow;
            } else if (norm <= 0.75) {
                newColor = Color.magenta;
            } else {
                newColor = Color.green;
            }

            if (currentColor == null) {
                fill.color = newColor;
                currentColor = newColor;
            } else if (newColor != currentColor) {
                fill.color = newColor;
                currentColor = newColor;
            }
        }
    }

    public void setMaxValue(float max, float value) {
        
        maxValue = max;
        this.value = value;

        if (slider) {
            Debug.Log("SET MAX " + max);
            slider.maxValue = max;
            slider.value = value;

            SetValue(value,false);
            //fill.color = gradient.Evaluate(1f); //Max value of the gradient
        }
    }
}
