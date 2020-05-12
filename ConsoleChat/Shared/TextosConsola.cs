using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;


namespace ConsoleChat.Shared
{
    interface ITextosConsola
    {
        string getTexto(string str);
    }

    class TextosConsola : ITextosConsola
    {
        private JObject _textos;
        public TextosConsola()
        {
            using (StreamReader file = File.OpenText("textos.json"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                _textos = (JObject)JToken.ReadFrom(reader);
            }
        }
        public string getTexto(string str)
        {
            try
            {
                return _textos.GetValue(str).ToObject<string>();
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
