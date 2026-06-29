public static class RunData
{
    public static int FinalScore { get; set; } = 0;
    public static int CoinsCollected { get; set; } = 0;
    public static float TimeSurvived { get; set; } = 0f;
    public static bool IsNewHighScore { get; set; } = false;
    public static string LevelSceneName { get; set; } = "";
    public static bool IsWinState { get; set; } = false;
    public static int CoinGoal { get; set; } = 50;
}
