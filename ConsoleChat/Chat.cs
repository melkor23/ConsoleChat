using ConsoleChat.Servicios;
using ConsoleChat.Shared;
using System;
using WebSocketSharp;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleChat
{
    class Chat
    {
        readonly IServiceProvider _serviceProvider;
        readonly ILecturaEscritura _entradaSalida;
        readonly ITextosConsola _textosConsola;
        private int _puerto;

        public Chat(IServiceProvider serviceProvider, ILecturaEscritura entradaSalida, ITextosConsola texto, int puerto)
        {
            _serviceProvider = serviceProvider;
            _entradaSalida = entradaSalida;
            _textosConsola = texto;
            _puerto = puerto;
        }

        public void inicia()
        {
            _entradaSalida.escribeInfoText(_textosConsola.getTexto("Bienvenida"));

            ICliente clienteWS = _serviceProvider.GetService<ICliente>();
            using (WebSocket ws = clienteWS.conectarse("ws://127.0.0.1:" + _puerto.ToString()))
            {
                if (ws.ReadyState != WebSocketState.Closed)
                {
                    //modo Cliente
                    _entradaSalida.escribeInfoWarm(_textosConsola.getTexto("modoCliente"));
                    clienteWS.establecerNombreUsuario();

                    clienteWS.iniciaChat();
                }
                else
                {
                    //modo Servidor
                    IServidor servidor = _serviceProvider.GetService<IServidor>();
                    servidor.iniciarServidor("ws://127.0.0.1:" + _puerto);
                }
            }
        }
    }
}
