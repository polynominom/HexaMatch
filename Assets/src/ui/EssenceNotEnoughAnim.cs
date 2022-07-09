using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EssenceNotEnoughAnim : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform back;
    private Transform front;
    private Color backColor;
    private Color frontColor;
    private float colorChangeDuration = 0.4F;

    public void Awake()
    {
        SkillManager.Instance.EssenceNotEnough += OnEssenceNotEnough;
        back       = transform.GetChild(0);
        front      = transform.GetChild(1);
        backColor  = back.GetComponent<Image>().color;
        frontColor = front.GetComponent<Image>().color;
    }

    public void OnEssenceNotEnough()
    {
        // due to edge cases game object might be already removed but this still called making a paradox in multiverse!
        if(gameObject != null)
            StartCoroutine(ColorAnim());
    }

    IEnumerator ColorAnim()
    {
        var t = 0.0F;
        var t2 = colorChangeDuration;
        while(t < colorChangeDuration)
        {
            t += Time.deltaTime;
            t2 = Mathf.Max(0, t2 - Time.deltaTime);
            float lerpedAlpha = t / colorChangeDuration;
            float lerpedAlpha2 = t2 / colorChangeDuration;
            back.GetComponent<Image>().color = new Color(backColor.r, backColor.b, backColor.g, lerpedAlpha);
            front.GetComponent<Image>().color = new Color(frontColor.r, frontColor.b, frontColor.g, lerpedAlpha2);
            yield return new WaitForEndOfFrame();
        }

        t = 0.0F;
        t2 = colorChangeDuration;
        while (t < colorChangeDuration)
        {
            t += Time.deltaTime;
            t2 -= Time.deltaTime;
            float lerpedAlpha = t / colorChangeDuration;
            float lerpedAlpha2 = t2 / colorChangeDuration;
            back.GetComponent<Image>().color = new Color(backColor.r, backColor.b, backColor.g, lerpedAlpha2);
            front.GetComponent<Image>().color = new Color(frontColor.r, frontColor.b, frontColor.g, lerpedAlpha);
            yield return new WaitForEndOfFrame();
        }

        back.GetComponent<Image>().color = backColor;
        back.GetComponent<Image>().color = frontColor;
    }
}
