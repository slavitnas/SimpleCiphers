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
        //   a  b     text = 1
        // a 1  2     return x = 0, y = 0
        // b 3  4     abc[0,0] = aa - расшифрованный
        public static bool ContainsIn(string text, string[,] encAbc, out int x, out int y)
        {
            for (int i = 0; i < encAbc.GetLength(0); i++)
            {
                for (int j = 0; j < encAbc.GetLength(1); j++)
                {
                    if (text == encAbc[i, j])
                    {
                        x = i;
                        y = j;
                        return true;
                    }
                }
            }
            x = -1;
            y = -1;
            return false;
        }

        // Содержится ли text на измерениях массива encAbc
        //   1 2     text = 11
        // 1 a b     return x = 0, y = 0
        // 2 c d     encAbc[0,0] = a - зашифрованный
        public static bool ContainsOut(string text, string[,] encAbc, out int x, out int y)
        {
            for (int i = 0; i < encAbc.GetLength(0); i++)
            {
                for (int j = 0; j < encAbc.GetLength(1); j++)
                {
                    if (text == $"{i + 1}{j + 1}" && encAbc[i, j] != string.Empty)
                    {
                        x = i;
                        y = j;
                        return true;
                    }
                }
            }
            x = -1;
            y = -1;
            return false;
        }
    }
}