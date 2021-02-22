using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    [SerializeField] float value;
    [SerializeField] float maxValue = 100;

    void Start() {
        slider = GetComponent<Slider>();
        fill = GetComponent<Image>();

        setMaxValue(maxValue,value);
    }


    public void SetValue(float value) {
        if (slider) {
            //Debug.Log("SET VALUE " + value + ", MAX " + slider.maxValue);
            slider.value = value;
            this.value = slider.value;

            //Displays gradient depending on value
            //fill.color = gradient.Evaluate(slider.normalizedValue);

            float norm = slider.normalizedValue;
            //Debug.Log("FILL COLOR: " + fill.color);

            if (norm < 0.05) {
                fill.color = Color.black;
            } else if (norm <= 0.25) {
                fill.color = Color.red;
            } else if (norm <= 0.5) {
                fill.color = Color.yellow;
            } else if (norm <= 0.75) {
                fill.color = Color.magenta;
            } else {
                fill.color = Color.green;
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

            SetValue(value);
            //fill.color = gradient.Evaluate(1f); //Max value of the gradient
        }
    }
}
