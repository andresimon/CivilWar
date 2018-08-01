using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarScript : MonoBehaviour 
{
    private float fillAmount;

    [SerializeField] private float lerpSpeed;

    [SerializeField] private Image mask;
    [SerializeField] private Image content;

    [SerializeField] private Text valueText;

    public float MaxValue { get; set;}

    public float Value 
    {
        set
        {
            string[] tmp = valueText.text.Split(':');
            valueText.text = tmp[0] + ": " + value;
            fillAmount = Map(value, 0, MaxValue, 0, 1);
        }
    }

    [SerializeField] private bool lerpColors;
    [SerializeField] private Color lowColor;
    [SerializeField] private Color fullColor;

	void Start () 
    {
        if (lerpColors)
            content.color = fullColor;

        if (!lerpColors)
            mask.color = lowColor;
	}
	
	void Update () 
    {
        HandleBar();
	}

    private void HandleBar()
    {
        if ( fillAmount != content.fillAmount )
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);       

        if ( lerpColors )
            content.color = Color.Lerp(lowColor, fullColor, fillAmount);
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
