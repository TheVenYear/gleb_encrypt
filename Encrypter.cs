using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace gleb_encrypt
{
    class Encrypter
    {
        T[,] Transpose<T>(T[,] matrix)
        {
            var transposeMatrix = new T[matrix.GetLength(1), matrix.GetLength(0)];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    transposeMatrix[j, i] = matrix[i, j];
                }
            }
            return transposeMatrix;
        }

        string GetString<T>(T[,] matrix)
        {
            var line = "";
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    line += matrix[i, j];
                }
            }
            return line;
        }

        char[,] GetMatrix(string line, int firstNum, int secondNum)
        {
            var lineCounter = 0;
            var matrix = new char[firstNum, secondNum];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (lineCounter + 1 <= line.Length)
                    {
                        matrix[i, j] = line[lineCounter];
                        lineCounter++;
                    }
                }
            }
            return matrix;
        }

        int[] GenerateKey(int length)
        {
            var key = new int[length];
            for (int i = 0; i < length; i++)
            {
                key[i] = i;
            }

            var random = new Random(DateTime.Now.Millisecond);
            key = key.OrderBy(x => random.Next()).ToArray();
            return key;
        }

        char[,] SetByKey(char[,] matrix, int[] key, bool isDecrypt)
        {
            var _matrix = new string[matrix.GetLength(0)];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var line = "";
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    line += matrix[i, j];
                }
                _matrix[i] = line;
            }

            var newMatrix = new char[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (isDecrypt)
                    {
                        newMatrix[i, j] = _matrix[key[i]][j];
                    }
                    else
                    {
                        newMatrix[key[i], j] = _matrix[i][j];
                    }

                }
            }
            return newMatrix;
        }

        public string Encrypt(string line, out string key)
        {
            key = "";
            var matrix = GetMatrix(line, 2, line.Length - line.Length / 2);
            var numericKey = GenerateKey(matrix.GetLength(0));
            matrix = SetByKey(matrix, numericKey, false);
            for (int i = 0; i < numericKey.Length; i++)
            {
                key += numericKey[i];
                if (i < numericKey.Length - 1)
                {
                    key += '.';
                }
            }
            key += ':';
            numericKey = GenerateKey(matrix.GetLength(1));

            matrix = SetByKey(Transpose(matrix), numericKey, false);
            for (int i = 0; i < numericKey.Length; i++)
            {
                key += numericKey[i];
                if (i < numericKey.Length - 1)
                {
                    key += '.';
                }
            }
            return GetString(matrix);
        }

        int[] GetKeys(string key, out int[] secondKey)
        {
            var isFirstHalf = true;
            string firstLine = "", secondLine = "";

            foreach (var item in key)
            {
                if (item == ':')
                {
                    isFirstHalf = false;
                }
                else if (isFirstHalf)
                {
                    firstLine += item;
                }
                else 
                {
                    secondLine += item;
                }
            }

            secondKey = GetKey(secondLine);
            return GetKey(firstLine);
        }

        int[] GetKey(string line)
        {
            var keyLine = "";
            var keyArr = new List<int>();
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '.')
                {
                    keyArr.Add(int.Parse(keyLine));
                    keyLine = "";
                }
                else if (i == line.Length - 1)
                {
                    keyLine += line[i];
                    keyArr.Add(int.Parse(keyLine));
                }
                else
                {
                    keyLine += line[i];
                }
            }
            return keyArr.ToArray();
        }

        public string Decrypt(string line, string lineKey)
        {
            var matrix = GetMatrix(line, line.Length - line.Length / 2,  2);
            int[] secondKey;
            var firstKey = GetKeys(lineKey, out secondKey);
            var ouline = "";
            matrix = SetByKey(matrix, secondKey, true);
            matrix = SetByKey(Transpose(matrix), firstKey, true);
            foreach (var item in secondKey)
            {
                ouline += item.ToString();
            }
            return GetString(matrix);
        }
    }
}
