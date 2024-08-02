namespace ConnectFour
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ConnectFourController controller = new ConnectFourController();
            
            controller.RunGame();
        }
    }
}