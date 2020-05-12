using ConsoleChat.Shared;
using Newtonsoft.Json;
using System;
using WebSocketSharp;

namespace ConsoleChat.Servicios
{
    interface ICliente
    {
        WebSocket conectarse(string direccion);
        bool enviarMensaje(string str);
        WebSocket recuperaSocket();
        void muestraUsuarioActual();
        void iniciaChat();
        bool establecerNombreUsuario();
        void cerrarSesion();

    }
    class Cliente : ICliente
    {
        readonly ILecturaEscritura _entradaSalida;
        readonly ITextosConsola _textosConsola;
        public Cliente(ILecturaEscritura io, ITextosConsola texto)
        {
            _entradaSalida = io;
            _textosConsola = texto;
        }
        private string _nombreUsuario { get; set; }

        private WebSocket _ws { get; set; }


        public void cerrarSesion()
        {
            _ws.Close();
        }


        public bool establecerNombreUsuario()
        {
            _entradaSalida.escribeInfo(_textosConsola.getTexto("introduceNombreUsuario"));
            _nombreUsuario = _entradaSalida.leeEntrada();
            _entradaSalida.escribeInfoOK(_textosConsola.getTexto("bienvenidaStr")+" " + _nombreUsuario + " "+ _textosConsola.getTexto("mensajeInicioChat"));

            return true;
        }

        public void iniciaChat()
        {
            var input = "";
            _entradaSalida.escribeInfo("");
            while (input != _textosConsola.getTexto("comandoSalir"))
            {
                muestraUsuarioActual();
                input = _entradaSalida.leeEntrada();
                if (input == _textosConsola.getTexto("comandoSalir")) break; else enviarMensaje(input);
            }
        }
    public WebSocket conectarse(string direccion)
        {
            _entradaSalida.escribeInfoText(_textosConsola.getTexto("strIntentoConexionCliente") + direccion);

            _ws = new WebSocketSharp.WebSocket(direccion);
            _ws.Log.Output = (_, __) => { };//desabilitamos el log para que no ensucie la pantalla

            _ws.OnMessage += (sender, e) =>
            {
                Mensaje men = JsonConvert.DeserializeObject<Mensaje>(e.Data);
                _entradaSalida.escribeInfo("");
                _entradaSalida.escribeInfo(men.usuario + ":" + men.str);
                muestraUsuarioActual();
            };
            _ws.OnError += (sender, e) =>
            {
                _entradaSalida.escribeInfoError(_textosConsola.getTexto("webSocketError") + " " + e.Message);
            };
            _ws.OnClose += (sender, e) =>
            {
                _entradaSalida.escribeInfoError(_textosConsola.getTexto("webSocketCerrado"));
            };

            try
            {
                _ws.Connect();
            }
            catch (Exception e)
            {
                _entradaSalida.escribeInfoError(_textosConsola.getTexto("webSocketError") + " :" + e.Message);
            }

            return _ws;
        }

        public WebSocket recuperaSocket()
        {
            return _ws;
        }

        public bool enviarMensaje(string str)
        {
            if (_ws.IsAlive)
            {
                Mensaje men = new Mensaje(_nombreUsuario, str);
                _ws.Send(JsonConvert.SerializeObject(men));
            }
            
            return true;
        }

        public void muestraUsuarioActual()
        {
            _entradaSalida.escribeUsuario(_nombreUsuario + "->");
        }
    }
}
