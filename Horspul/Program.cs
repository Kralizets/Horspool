using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Horspul
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentString = GetDataFormFile(@"E:\Data\CurrentString.txt");
            string searchString = GetDataFormFile(@"E:\Data\SearchString.txt");

            if (searchString.Length > currentString.Length)
            {
                Console.WriteLine("Is not found current string, line length too large");
                Console.ReadLine();
                return;
            }

            Dictionary<char, int> offsetTable = GetOffsetTable(searchString);
            AlgorithmHorspul(currentString, searchString, offsetTable);

            Console.ReadLine();
        }

        public static string GetDataFormFile(string path)
        {
            Encoding enc = Encoding.GetEncoding(1251);
            using (StreamReader sr = new StreamReader(path, enc))
            {
                return sr.ReadToEnd();
            }
        }

        public static Dictionary<char, int> GetOffsetTable(string searchString)
        {
            Dictionary<char, int> offsetTable = new Dictionary<char, int>();
            int lengthString = searchString.Length - 2;

            for (int positionInSearchString = lengthString; positionInSearchString > -1; positionInSearchString--)
            {
                if (offsetTable.Any(x => x.Key == searchString[positionInSearchString]))
                {
                    continue;
                }

                offsetTable.Add(searchString[positionInSearchString], lengthString - positionInSearchString + 1);
            }

            offsetTable.Add('#', lengthString + 2);
            return offsetTable;
        }

        public static void AlgorithmHorspul(string currentString, string searchString, Dictionary<char, int> offsetTable)
        {
            int currentPosition = searchString.Length - 1;
            int maxSearchPosition = currentPosition;
            int maxPosistion = currentString.Length;
            bool isSuccessfully;

            while (currentPosition < maxPosistion)
            {
                isSuccessfully = true;

                for (int positionInSearchString = maxSearchPosition; positionInSearchString > -1; positionInSearchString--, currentPosition--)
                {
                    if (currentString[currentPosition] != searchString[positionInSearchString])
                    {
                        isSuccessfully = false;
                        currentPosition = offsetTable.Any(x => x.Key == currentString[currentPosition])
                            ? currentPosition + offsetTable.Single(x => x.Key == currentString[currentPosition]).Value : currentPosition + offsetTable['#'];

                        break;
                    }
                }

                if (isSuccessfully)
                {
                    Console.WriteLine("Index: [" + (currentPosition + 1) + "] in string: " + searchString);
                    return;
                }
            }
        }
    }
}