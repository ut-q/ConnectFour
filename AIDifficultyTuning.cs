namespace ConnectFour
{
    public class AIDifficultyTuning
    {
        public int MaxDepth { get; private set; }
        public string Name { get; private set; }

        public static AIDifficultyTuning EasyTuning = new AIDifficultyTuning()
        {
            MaxDepth = 2,
            Name = "Easy"
        };
        
        public static AIDifficultyTuning MediumTuning = new AIDifficultyTuning()
        {
            MaxDepth = 5,
            Name = "Medium"
        };
        
        public static AIDifficultyTuning HardTuning = new AIDifficultyTuning()
        {
            MaxDepth = 8,
            Name = "Hard"
        };
        
        public static AIDifficultyTuning ProTuning = new AIDifficultyTuning()
        {
            MaxDepth = 11,
            Name = "Pro"
        };
    }
}