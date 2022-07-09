using System;

public class SkillManager: Singleton<SkillManager>
{
    public event Action EssenceNotEnough;

    private int obtainedSkills = -1;
    private bool isPerforming = false;
    private bool skillActive = false;
    private Skill performingSkill = null;
    private SkillPerformHandler performer = null;

    private UISkillEffectHandler uiSkillEffectHandler;
    private SkillSlideMenu uiSkillSlideMenu;

    public bool IsPerforming() => isPerforming;
    public bool IsSkillActive() => skillActive;
    public Skill GetSkill() => performingSkill;
    public SkillPerformHandler GetPerformer() => performer;
    public SkillSlideMenu GetSkillSlideMenu() => uiSkillSlideMenu;

    public event Action SkillCountChange;

    public SkillManager()
    {
        //acquire and check if the player's obtained skill counts is enough
        
    }

    public void BroadcastEssenceNotEnough()
    {
        if(EssenceNotEnough != null)
            EssenceNotEnough();
    }

    public void ResetSkillEventListeners()
    {
        SkillCountChange = () => { };
        EssenceNotEnough = () => { };
    }

    public void RegisterUISkillSlideMenu(SkillSlideMenu slideMenu)
    {
        uiSkillSlideMenu = slideMenu;
    }

    public bool CanAfford()
    {
        if(obtainedSkills == -1)
        {
            var st = DataManager.Instance.LoadStatistics();
            SetObtainedSkills(st.obtainedSkills);
        }

        return obtainedSkills >= 1;
    }

    public void RegisterUIEffectHandler(UISkillEffectHandler s)
    {
        uiSkillEffectHandler = s;
    }


    public void RegisterSkill(Skill skill)
    {
        if (performingSkill != null)
            return;

        skillActive = true;
        performingSkill = skill;
        if (performingSkill is SwapSkill)
        {
            performer = new SwapPerformHandler();
            performer.shouldPerform = false;
        }

        if (uiSkillEffectHandler != null)
            uiSkillEffectHandler.OnSkillActivated();
    }

    public void OnHexagon(HexaNode n)
    {
        isPerforming = true;
        if (performingSkill is SwapSkill)
        {
            int activationId = ((SwapPerformHandler)performer).OnHexaSelect(n);
            if (uiSkillEffectHandler && activationId != -1)
                uiSkillEffectHandler.UpdateOneHexagonColor(n, activationId != 3);

            if (performer.shouldPerform)
                PerformSkill();
        }
        else
        {
            //EXCEPTION
            CleanUp();
        }
    }
    
    //Idea:  might be more generic 
    private void PerformSkill()
    {
        if (performingSkill == null)
            return;

        // check again to avoid edge-case scenarios
        if (performer is SwapPerformHandler && performer.shouldPerform)
        {
            ((SwapPerformHandler)performer).SwapMaterials();
            ((SwapPerformHandler)performer).SwapTypes();
            // update obtainedSkill count variable
            SetObtainedSkills(Math.Max(obtainedSkills - 1, 0));
            DataManager.Instance.SaveObtainedSkills(obtainedSkills);
            CleanUp();
        }
    }

    public void SetObtainedSkills(int newSkillCount)
    {
        obtainedSkills = newSkillCount;
        SkillCountChange();
    }

    public int GetObtainedSkills()
    {
        if(obtainedSkills == -1)
        {
            var st = DataManager.Instance.LoadStatistics();
            SetObtainedSkills(st.obtainedSkills);
        }
        return obtainedSkills;
    }



    public void CleanUp()
    {
        performer = null;
        performingSkill = null;
        isPerforming = false;
        skillActive = false;
        if (uiSkillEffectHandler != null)
            uiSkillEffectHandler.OnSkillDeactivated();
    }

    // ---- Skill Buy Functions ---
    public bool IsSkillAffordable(Skill skill)
    {
        return GameManager.Instance.PlayerEssence >= skill.GetValue();
    }

    public void BuySkill(Skill skill)
    {
        // at this point the affordablity would be already checked.
        GameManager.UpdateEssence(-skill.GetValue());
        SetObtainedSkills(obtainedSkills + 1);
    }
}
