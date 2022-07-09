using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlideMenu : MonoBehaviour
{
    public Material backgroundLineMaterial;
    bool normalAnimationStarts = false;
    private GameObject buttonGO;
    private Color leftColorAvailable = Color.white;
    private Color rightColorAvailable = Color.green;
    private Color leftColorUnavailable = Color.red;
    private Color rightColorUnavailable = Color.white;

    // Start is called before the first frame update
    void Awake()
    {
        SkillManager.Instance.RegisterUISkillSlideMenu(this);
    }

    public void Activate(GameObject buttonObject)
    {
        
        buttonObject.SetActive(false);
        buttonGO = buttonObject;

        transform.GetChild(0).gameObject.SetActive(true);
        normalAnimationStarts = true;
        AnimationHandler.PlayAnimation(GetComponent<Animation>(), "SkillBuyWindowAnimation");
    }

    private void ReverseAnimation()
    {
        //prevent called multiple times!
        if (!normalAnimationStarts)
            return;
        //reverse the animation
        normalAnimationStarts = false;
        AnimationHandler.ReverseAnimation(GetComponent<Animation>(), "SkillBuyWindowAnimation");
    }

    public void OnOk()
    {
        Skill swapSkill = new SwapSkill();
        if(SkillManager.Instance.IsSkillAffordable(swapSkill))
        {
            // Perform "Buy" on skill manager
            SkillManager.Instance.BuySkill(swapSkill);
            ReverseAnimation();
        }
        else if (backgroundLineMaterial)
        {
            // NOT AFFORDABLE! DISPLAY SOME ANIMATION
            //backgroundLineMaterial.SetColor("_Color2", rightColorUnavailable);
            //backgroundLineMaterial.SetColor("_Color", leftColorUnavailable);
            SkillManager.Instance.BroadcastEssenceNotEnough();
            StartCoroutine(UpdateBgColor("_Color", leftColorAvailable, leftColorUnavailable));
            StartCoroutine(UpdateBgColor("_Color2", rightColorAvailable, rightColorUnavailable));
        }
    }

    // ping pong transition fromColor -> toColor -> fromColor
    IEnumerator UpdateBgColor(string name, Color fromColor, Color toColor, float seconds=0.1F, float colorChangeDuration = 0.4F)
    {
        // wait till the previous animation almost finished
        yield return new WaitForSeconds(seconds);
        // start interpolating colors of hexanode's background
        var t = 0.0f;
        while (t < colorChangeDuration)
        {
            t += Time.deltaTime / colorChangeDuration;
            Color lerpedColor = Color.Lerp(fromColor, toColor, t / colorChangeDuration);
            backgroundLineMaterial.SetColor(name, lerpedColor);
            yield return new WaitForEndOfFrame();
        }

        t = 0.0f;
        while (t < colorChangeDuration)
        {
            t += Time.deltaTime / colorChangeDuration;
            Color lerpedColor = Color.Lerp(toColor, fromColor, t / colorChangeDuration);
            backgroundLineMaterial.SetColor(name, lerpedColor);
            yield return new WaitForEndOfFrame();
        }

        backgroundLineMaterial.SetColor(name, fromColor);
    }

    public void OnAnimationReverseEnds()
    {
        //if this function is called FOR FIRST ANIMATION PLAY. It will be ignored!
        if (normalAnimationStarts)
            return;

        if(buttonGO)
            buttonGO.SetActive(true);
        backgroundLineMaterial.SetColor("_Color2", rightColorAvailable);
        backgroundLineMaterial.SetColor("_Color", leftColorAvailable);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnCancel()
    {
        ReverseAnimation();
    }
}
