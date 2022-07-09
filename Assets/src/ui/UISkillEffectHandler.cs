using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillEffectHandler : MonoBehaviour
{
    public Color backgroundUsualColor;
    public Color backgroundInacvtiveColor;
    public Color backgroundActiveColor;

    private Transform Level;
    private Animation bgAnimation;
    private float changeColorsAfter = 0.20f; // seconds
    private float colorChangeDuration = 0.5f; // seconds

    void Start()
    {
        Level = GameObject.FindGameObjectWithTag("Level").transform;
        bgAnimation = transform.GetChild(0).GetChild(0).GetComponent<Animation>();
        SkillManager.Instance.RegisterUIEffectHandler(this);
    }

    public void OnSkillActivated()
    {
        if (bgAnimation == null || Level == null)
            return;

        AnimationHandler.PlayAnimation(bgAnimation, "SkillEffectUIBackgroundAnimation");
        StartCoroutine(UpdateHexagonsBgColors(backgroundUsualColor, backgroundInacvtiveColor, changeColorsAfter));

        // TODO: 
    }

    public void OnSkillDeactivated()
    {
        //reverse animation
        AnimationHandler.ReverseAnimation(bgAnimation, "SkillEffectUIBackgroundAnimation");
        //reverse colors
        StartCoroutine(UpdateHexagonsBgColors(backgroundInacvtiveColor, backgroundUsualColor, changeColorsAfter));

    }

    private void ChangeBackgroundColors(Color c)
    {
        for(int i = 0; i < Level.childCount; ++i)
        {
            //get the background which is the first child under the hexagon.
            Level.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().color = c;
        }
    }

    IEnumerator UpdateHexagonsBgColors(Color fromColor, Color toColor, float seconds)
    {
        // wait till the previous animation almost finished
        yield return new WaitForSeconds(seconds);
        // start interpolating colors of hexanode's background
        var t = 0.0f;
        while (t < colorChangeDuration)
        {
            t += Time.deltaTime / colorChangeDuration;
            Color lerpedColor = Color.Lerp(fromColor, toColor, t/ colorChangeDuration);
            ChangeBackgroundColors(lerpedColor);
            yield return new WaitForEndOfFrame();
        }
        ChangeBackgroundColors(toColor);
    }

    //TODO:
    //  - REPLACE ANIMATION
    //  - SELECTED INFRONT
    public void UpdateOneHexagonColor(HexaNode n, bool activating)
    {
        Color oldColor = n.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        Color targetColor = backgroundActiveColor;
        if (!activating)
            targetColor = backgroundInacvtiveColor;
        StartCoroutine(UpdateHexaColor(n, oldColor, targetColor));
    }

    IEnumerator UpdateHexaColor(HexaNode n, Color oldColor, Color targetColor)
    {
        var t = 0.0f;
        while (t < colorChangeDuration)
        {
            t += Time.deltaTime / colorChangeDuration;
            Color lerpedColor = Color.Lerp(oldColor, targetColor, t);
            n.transform.GetChild(0).GetComponent<SpriteRenderer>().color = lerpedColor;
            yield return new WaitForEndOfFrame();
        }
        n.transform.GetChild(0).GetComponent<SpriteRenderer>().color = targetColor;
        var pos = n.transform.GetChild(0).transform.localPosition;
        n.transform.GetChild(0).transform.localPosition = new Vector3(pos.x, pos.y, 0.1f);
    }
}
