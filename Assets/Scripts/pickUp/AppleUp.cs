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
