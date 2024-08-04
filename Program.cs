using System;

namespace ConnectFour
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Connect Four: Game of the Year Edition");
            ConnectFourController controller = new ConnectFourController();
            
            controller.ShowMainMenu();
        }
    }
}