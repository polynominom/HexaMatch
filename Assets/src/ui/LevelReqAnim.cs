using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelReqAnim : MonoBehaviour
{
    
    public HomeUIEventHandler Handler;
    private Vector3 upTarget = new Vector3(61F, 105F, 0F);
    private Vector3 downTarget = new Vector3(17.187F, 29.584F, 0F);
    private Vector3 currentTarget;
    private bool isUp = true;
    private bool animRunning = false;
    private float trackedTime = 0.0F;
    private Image panelImage;
    private Image cornerImage;

    private void Awake()
    {
        panelImage = transform.GetChild(1).GetComponent<Image>();
        cornerImage = transform.GetChild(0).GetComponent<Image>();
        currentTarget = downTarget;
        OnAnimEnd();
    }

    private void OnAnimEnd()
    {
        transform.GetChild(2).gameObject.SetActive(!isUp);
    }

    public void OnAnim()
    {
        isUp = !isUp;
        if (!isUp)
            currentTarget = downTarget;
        else
            currentTarget = upTarget;
        animRunning = true;
        trackedTime = 0.0F;

        if(!isUp)
            transform.GetChild(2).gameObject.SetActive(true);
    }

    private void Update()
    {
        if(animRunning)
        {
            trackedTime += Time.deltaTime;
            if (Vector3.Distance(transform.localPosition, currentTarget) > 0.01F)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, currentTarget, trackedTime / 0.4F);
                panelImage.color = Color.Lerp(panelImage.color, new Color(1, 1, 1, isUp?0F:1F), trackedTime / 0.4F);
                cornerImage.color = Color.Lerp(cornerImage.color,
                    new Color(cornerImage.color.r, cornerImage.color.g, cornerImage.color.b, isUp?0F:1F), trackedTime / 0.4F);

            }
            else
            {
                animRunning = false;
                trackedTime = 0.0F;
                OnAnimEnd();
            }
        }
        
    }
}
