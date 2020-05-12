using ConsoleChat.Shared;
using Fleck;
using System;
using System.Collections.Generic;

namespace ConsoleChat.Servicios
{
    interface IServidor
    {
        void iniciarServidor(string direccion);
    }
    class Servidor :  IServidor
    {
        private List<IWebSocketConnection> _listaWebSockets { get; set; }
        readonly ILecturaEscritura _lecturaEscritura;
        readonly ITextosConsola _textosConsola;

        public Servidor(ILecturaEscritura io, ITextosConsola texto)
        {
            _lecturaEscritura = io;
            _textosConsola = texto;
        }


        public void iniciarServidor(string direccion)
        {
            _lecturaEscritura.escribeInfoWarm(_textosConsola.getTexto("modoServidor"));

            FleckLog.Level = Fleck.LogLevel.Error;
            _listaWebSockets = new List<IWebSocketConnection>();
            var server = new WebSocketServer(direccion);

            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    muestraNuevaConexion(socket);
                    _listaWebSockets.Add(socket);
                };

                socket.OnClose = () =>
                {
                    muestraFinConexion(socket);
                    _listaWebSockets.Remove(socket);
                };

                socket.OnMessage = (message) =>
                {
                    _lecturaEscritura.escribeInfoText(_textosConsola.getTexto("strMensajeChat") + " " + message);
                    _listaWebSockets.ForEach(s =>
                    {
                        if (socket.ConnectionInfo.Id != s.ConnectionInfo.Id) s.Send(message);}
                    ); 
                };

                socket.OnPing = (b) =>
                {
                    socket.SendPong(b);
                };
            });

            var input = _lecturaEscritura.leeEntrada();
            while (input != _textosConsola.getTexto("comandoSalir"))
            {
                foreach (var socket in _listaWebSockets)
                {
                    socket.Send(input);
                }

                input = _lecturaEscritura.leeEntrada();
            }
        }

        #region info para  mostrar por pantalla
        private void muestraNuevaConexion(IWebSocketConnection socket)
        {
            _lecturaEscritura.escribeInfo(String.Empty);
            _lecturaEscritura.escribeInfoOK(_textosConsola.getTexto("strNuevaConexionCliente"));
            muestraDatosConexion(socket);
            _lecturaEscritura.escribeInfoOK(_textosConsola.getTexto("strSeparador"));
            _lecturaEscritura.escribeInfo(String.Empty);
        }

        private void muestraFinConexion(IWebSocketConnection socket)
        {
            _lecturaEscritura.escribeInfo(String.Empty);
            _lecturaEscritura.escribeInfoWarm(_textosConsola.getTexto("strFinConexionCliente"));
            muestraDatosConexion(socket);
            _lecturaEscritura.escribeInfoWarm(_textosConsola.getTexto("strSeparador"));
            _lecturaEscritura.escribeInfo(String.Empty);
        }

        private void muestraDatosConexion(IWebSocketConnection socket)
        {
            _lecturaEscritura.escribeInfo(_textosConsola.getTexto("strGUID") + " " + socket.ConnectionInfo.Id);
            _lecturaEscritura.escribeInfo(_textosConsola.getTexto("strIP") + " " + socket.ConnectionInfo.ClientIpAddress);
            _lecturaEscritura.escribeInfo(_textosConsola.getTexto("strPuerto") + " " + socket.ConnectionInfo.ClientPort);
        }
        #endregion
    }
}
