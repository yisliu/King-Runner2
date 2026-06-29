/*
using UnityEngine;

public class AppleUp : pickUp
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float adjustChangeMoveSpeedAmount = 3f;
    LevelCooker levelGenerator;
    
    public void init(LevelCooker levelGenerator)
    {
        this.levelGenerator = levelGenerator;
    }
    protected override void pickUpEffect()
    {
        levelGenerator.changeChunkSpeed(adjustChangeMoveSpeedAmount);
    }
}
*/
using UnityEngine;

public class AppleUp : pickUp
{
    [SerializeField] float adjustChangeMoveSpeedAmount = 3f;

    private ILevelCooker levelCooker;

    public void init(ILevelCooker cooker)
    {
        levelCooker = cooker;
    }

    protected override void pickUpEffect()
    {
        if (levelCooker != null)
            levelCooker.ChangeSpeed(adjustChangeMoveSpeedAmount);
        else
            Debug.LogWarning("AppleUp: No level cooker reference assigned.");
    }
}