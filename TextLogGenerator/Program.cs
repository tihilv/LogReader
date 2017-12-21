using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextLogGenerator
{
  class Program
  {
    static void Main(string[] args)
    {
        string fileName = "test.log";
        bool exit = false;
        do
        {
            var key = Console.ReadKey();
            exit = key.Key == ConsoleKey.Q;

            switch (key.Key)
            {
                case ConsoleKey.B:
                    PutBigBlock(fileName, 1000);
                    break;
                case ConsoleKey.M:
                    PutBigBlock(fileName, 1000000);
                    break;
            }

        } while (!exit);
    }

      private static void PutBigBlock(string fileName, int lineCount)
      {
          using (var writer = new StreamWriter(fileName, true))
          {
              for (int i = 0; i < lineCount; i++)
                  writer.WriteLine($"This is the line of the log number {i}");
              
              writer.Flush();
          }
      }
  }
}
