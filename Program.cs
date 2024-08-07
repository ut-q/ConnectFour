using System;

namespace ConnectFour
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("WELCOME TO CONNECT FOUR: GAME OF THE YEAR EDITION");
            ConnectFourController controller = new ConnectFourController();
            MainMenu mainMenu = new MainMenu(controller);
            mainMenu.ShowMainMenu();
        }
    }
}