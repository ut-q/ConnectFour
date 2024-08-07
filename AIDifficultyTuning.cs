namespace ConnectFour
{
    public class AIDifficultyTuning
    {
        public int MaxDepth { get; private set; }
        public string Name { get; private set; }

        public static readonly AIDifficultyTuning EasyTuning = new AIDifficultyTuning()
        {
            MaxDepth = 1,
            Name = "Easy"
        };
        
        public static readonly AIDifficultyTuning MediumTuning = new AIDifficultyTuning()
        {
            MaxDepth = 3,
            Name = "Medium"
        };
        
        public static readonly AIDifficultyTuning HardTuning = new AIDifficultyTuning()
        {
            MaxDepth = 5,
            Name = "Hard"
        };
        
        public static readonly AIDifficultyTuning ProTuning = new AIDifficultyTuning()
        {
            MaxDepth = 7,
            Name = "Pro"
        };
    }
}