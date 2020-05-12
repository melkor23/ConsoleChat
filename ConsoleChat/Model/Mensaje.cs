using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChat
{
    class Mensaje
    {
        public string usuario { get; set; }
        public string str { get; set; }

        public Mensaje(string usuario, string str)
        {
            this.usuario = usuario;
            this.str = str;
        }

    }
}
