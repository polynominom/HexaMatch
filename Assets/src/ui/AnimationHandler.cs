using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnimationHandler : MonoBehaviour
{
    public Animation TargetAnimation;
    public Animation LevelEndAnimation;
    public Animation LevelStartAnimation;
    
    private GameObject Level;
    private void Awake()
    {
        Level = GameObject.FindGameObjectWithTag("Level");
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    // Handles the Level End Animation
    public void OnLevelEnd()
    {
        if (LevelEndAnimation == null)
            return;
        //AnimationState state = LevelEndAnimation["LevelEndAnimation"];
        LevelEndAnimation.Play("LevelEndAnimation");
    }



    public void OnLevelStart()
    {
        if (LevelStartAnimation == null)
        {
            return;
        }
            
        LevelStartAnimation.Play();
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        OnLevelStart();
    }

    public static void ReverseAnimation(Animation anim, string name)
    {
        AnimationState state = anim[name];
        anim.Play(name);
        state.speed = -1.0f;
        state.time = state.length;
    }

    public static void PlayAnimation(Animation anim, string name)
    {
        AnimationState state = anim[name];
        anim.Play(name);
        state.speed = 1.0f;
    }
}
