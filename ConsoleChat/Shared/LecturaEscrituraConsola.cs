using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChat.Shared

{
    interface ILecturaEscritura
    {
        void escribeInfo(string str, ConsoleColor color = ConsoleColor.White);
        void escribeInfoOK(string str);
        void escribeInfoText(string str);
        void escribeInfoWarm(string str);
        void escribeInfoError(string str);
        void escribeUsuario(string str);
        void escribeTituloApp();
        string leeEntrada();
    }
    class LecturaEscrituraConsola : ILecturaEscritura
    {
        public void escribeInfoOK(string str)
        {
            escribeInfo(str, ConsoleColor.Green);
        }
        public void escribeInfoText(string str)
        {
            escribeInfo(str, ConsoleColor.Cyan);
        }
        public void escribeInfoWarm(string str)
        {
            escribeInfo(str, ConsoleColor.Yellow);
        }
        public void escribeInfoError(string str)
        {
            escribeInfo(str, ConsoleColor.Red);
        }
        public void escribeInfo(string str, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(str);
        }
        public void escribeUsuario(string str)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(str);
        }

        public string leeEntrada()
        {
            return Console.ReadLine();
        }


        public void escribeTituloApp()
        {

            Console.WriteLine("  _____                      _         _____ _    _       _______ ");
            Console.WriteLine(" / ____|                    | |       / ____| |  | |   /\\|__   __|");
            Console.WriteLine("| |     ___  _ __  ___  ___ | | ___  | |    | |__| |  /  \\  | |   ");
            Console.WriteLine("| |    / _ \\| '_ \\/ __|/ _ \\| |/ _ \\ | |    |  __  | / /\\ \\ | |   ");
            Console.WriteLine("| |___| (_) | | | \\__ \\ (_) | |  __/ | |____| |  | |/ ____ \\| |   ");
            Console.WriteLine(" \\_____\\___/|_| |_|___/\\___/|_|\\___|  \\_____|_|  |_/_/    \\_\\_|   ");
            Console.WriteLine("========================================================================");
        }
    }
}
