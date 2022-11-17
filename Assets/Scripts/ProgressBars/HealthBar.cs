using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider sliderHP;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    [SerializeField] Image transition;
    [SerializeField] float transTime = 0.5f;

    private void Awake()
    {
    }
    // Update is called once per frame
    public void SetHealthDown(float health)
    {
        sliderHP.value = health;
        fill.color = gradient.Evaluate(sliderHP.normalizedValue);
        StartCoroutine(HpTransitionDown());
    }
    public void SetHealthUp(float hpRatio)
    {
        transition.fillAmount = hpRatio;
        StartCoroutine(HpTransitionUp());
    }
    public void SetMaxHealth(float health)
    {
        sliderHP.maxValue = health;
        sliderHP.value = health;
        transition.fillAmount = 1f;
        fill.color = gradient.Evaluate(1f);
    }
    IEnumerator HpTransitionDown()
    {
        yield return new WaitForSeconds(0.4f);
        while (sliderHP.normalizedValue < transition.fillAmount)
        {
            transition.fillAmount -= (1 / transTime) * Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator HpTransitionUp()
    {
        yield return new WaitForSeconds(0.4f);
        while (sliderHP.normalizedValue < transition.fillAmount)
        {
            sliderHP.value += transTime * 5 * Time.deltaTime;
            fill.color = gradient.Evaluate(sliderHP.normalizedValue);
            yield return null;
        }
    }
    private void Update()
    {
        //if (sliderHP.normalizedValue < transition.fillAmount)
          //  transition.fillAmount -= 1 / transTime * Time.deltaTime;
    }
}
