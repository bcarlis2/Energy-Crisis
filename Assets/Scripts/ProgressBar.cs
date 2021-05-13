/*
	Project:    Energy Crisis
	
	Script:     ProgressBar
	Desc:       Visualizes the charge amount for battery icons
	
	Credits:	Brandon Carlisle
	
*/

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
    public Color[] batteryColors = new Color[7];
    [SerializeField] float value;
    [SerializeField] float maxValue = 100;

    void Start() {
        slider = GetComponent<Slider>();
        fill = GetComponent<Image>();

        batteryColors[0] = Color.black;
        batteryColors[1] = Color.red;
        batteryColors[2] = new Color(1,0.6f,0.2f); //Orange
        batteryColors[3] = Color.yellow;
        batteryColors[4] = new Color(0,0.6f,0); //Dark Green
        batteryColors[5] = Color.green;
        batteryColors[6] = Color.cyan;

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
            fill.fillAmount = norm;

            Color newColor;
            //Debug.Log("NORM: " + norm);

            if (charging) {
                newColor = batteryColors[6]; //Cyan (Charging)
            } else if (norm < 0.05) { //TODO: Make this correspond to gun's needed charge
                newColor = batteryColors[0]; //Black (Empty)
            } else if (norm <= 0.25) {
                newColor = batteryColors[1]; //Red (Very Low)
            } else if (norm <= 0.5) {
                newColor = batteryColors[2]; //Orange (Under Half)
            } else if (norm <= 0.75) {
                newColor = batteryColors[3]; //Yellow (Getting Low)
            } else if (norm < 1) {
                newColor = batteryColors[4]; //Dark green (Almost Full)
            } else {
                newColor = batteryColors[5]; //Bright green (Full)
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
            //Debug.Log("SET MAX " + max);
            slider.maxValue = max;
            slider.value = value;

            SetValue(value,false);
            //fill.color = gradient.Evaluate(1f); //Max value of the gradient
        }
    }
}
