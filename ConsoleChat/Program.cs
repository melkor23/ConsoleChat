using ConsoleChat.Servicios;
using ConsoleChat.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;

namespace ConsoleChat
{
    class Program
    {
        static void Main(string[] args)
        {
            ITextosConsola textosConsola = new TextosConsola();
            ILecturaEscritura entradaSalida = new LecturaEscrituraConsola();


            entradaSalida.escribeTituloApp();

            int? puerto = null; ;
            if (args.Length > 0)
            {
                puerto = validarPuerto(args[0]);
            }

            if (puerto == null)
            {
                entradaSalida.escribeInfoError(textosConsola.getTexto("ErrorPuerto"));
                Environment.Exit(-1);
            }
            else
            {
                var serviceProvider = new ServiceCollection()
                    .AddSingleton<ICliente, Cliente>(_ =>new Cliente(entradaSalida, textosConsola))
                    .AddSingleton<IServidor, Servidor>(_ => new Servidor(entradaSalida, textosConsola))
                    .BuildServiceProvider();

                Chat nuevoChat= new Chat(serviceProvider, entradaSalida, textosConsola, (int)puerto);
                nuevoChat.inicia();
            }
        }

       
        private static int? validarPuerto(string str)
        {
            if (int.TryParse(str, out int n))
            {
                return n < 65535 ? (int?)n : null;
            }
            else
            { return null; }

        }
    }
}



