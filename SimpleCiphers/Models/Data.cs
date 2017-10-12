using System.Collections.Generic;

namespace SimpleCiphers.Models
{
    public static class Data
    {
        private const string Symbols = "0123456789";

        public static Dictionary<string, string> Words = new Dictionary<string, string>
        {
            ["ENG"] = "abcdefghijklmnopqrstuvwxyz•" + Symbols,
            ["РУС"] = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя•" + Symbols,
            ["УКР"] = "абвгґдеєжзиіїйклмнопрстуфхцчшщьюя•" + Symbols
        };

        public static Dictionary<string, ICipher> CipherMethods = new Dictionary<string, ICipher>
        {
            ["Цезарь"] = new CaesarCipher(),
            ["Лозунговый"] = new SloganCipher(),
            ["Полибианский"] = new PolybiusCipher(),
            ["Порта"] = new PortaCipher(),
            ["Плейфер"] = new PlayfairCipher(),
            ["Вертикальная перестановка"] = new ColumnarTransCipher(),
            ["Гаммирование"] = new XorCipher(),
        };
    }
}