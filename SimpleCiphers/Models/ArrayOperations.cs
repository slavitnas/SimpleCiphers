using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCiphers.Models
{
    public static class ArrayOperations
    {
        // Содержится ли text в encAbc. Если да, то возвращаются индексы в encAbc
        //   a b     text = 1
        // a 1 2     return x = 0, y = 0
        // b 3 4     abc[0,0] = aa - расшифрованный
        //           encAbc[0,0] = 1 - зашифрованный 
        public static bool ContainsIn(string text, string[,] encAbc, out int x, out int y)
        {
            for (var i = 0; i < encAbc.GetLength(0); i++)
            {
                for (var j = 0; j < encAbc.GetLength(1); j++)
                {
                    if (text != encAbc[i, j]) continue;
                    x = i;
                    y = j;
                    return true;
                }
            }
            x = -1;
            y = -1;
            return false;
        }

        public static string[,] Turn1DTo2D(string[] encAbc)
        {
            var arr = new string[1, encAbc.Length];
            for (var i = 0; i < encAbc.Length; i++)
            {
                arr[0, i] = $"{encAbc[i]}";
            }
            return arr;
        }
    }
}