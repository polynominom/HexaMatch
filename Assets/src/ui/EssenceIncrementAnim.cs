using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EssenceIncrementAnim : MonoBehaviour
{
    public Text essenceTextObject;
    public ParticleSystem essenceParticleSystem;
    private int textEssence = 0;
    private float textAnimationTimeLimit = 2.0F;

    public void OnEnd()
    {
        var lst = LevelManager.Instance.GetLevelEssence();
        int targetEssence = (int)lst[0];
        textAnimationTimeLimit = lst[1];


        if (essenceParticleSystem != null)
            essenceParticleSystem.Play();

        if ( essenceTextObject != null )
            StartCoroutine(PointIncrement( targetEssence ));
    }

    private IEnumerator PointIncrement(int targetEssence)
    {
        int pts = textEssence;
        var t = 0.0f;
        while (t < textAnimationTimeLimit)
        {
            t += Time.deltaTime / textAnimationTimeLimit;
            textEssence = (int)Mathf.Lerp(pts, targetEssence, t);
            essenceTextObject.text = string.Format("x {0}", textEssence.ToString());
            yield return new WaitForEndOfFrame();
        }

        textEssence = targetEssence;
        essenceTextObject.text = string.Format("x {0}", textEssence.ToString());
    }
}
