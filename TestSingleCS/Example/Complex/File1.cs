using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public 
class MyClass{
    public string Name { get; set; }
    
    public int Number { get; set; }
}

namespace Miscellaneous
{
    public class ReturnToGarden
    {
        public static void Main()
        {
            var a = Scanner.ReadAllArrays<int>(3);

            int x, y, x1, y1;
            Scanner.Read(out x, out y);
            Scanner.Read(out x1, out y1);

            Console.WriteLine(x1 - x > 0 ? "Right" : "Left");
        }
    }
}

namespace System
{
    public static class Scanner
    {
        /// <summary>
        /// Default transform function for converting string to another type
        /// </summary>
        /// <param name="value">Input text</param>
        private static TResult Selector<TResult>(string value) => (TResult)Convert.ChangeType(value, typeof(TResult));




        /// <summary>
        /// Reads the next line
        /// </summary>
        /// <returns></returns>
        public static string Read() => Console.ReadLine();

        /// <summary>
        /// Reads next line and converts it into another type using Convert class
        /// </summary>
        public static TResult Read<TResult>() => Selector<TResult>(Read());

        /// <summary>
        /// Reads next line and converts it into new type using custom selector
        /// </summary>
        /// <param name="selector">Transform function</param>
        public static TResult Read<TResult>(Func<string, TResult> selector) => selector(Read());





        /// <summary>
        /// Reads elements of next line into an array using custom selector
        /// </summary>
        /// <param name="selector">Transform function</param>
        public static TResult[] ReadArray<TResult>(Func<string, TResult> selector, char separator = ' ')
            => Read().Split(separator).Select(selector).ToArray();

        /// <summary>
        /// Reads elements of next line into an array using Convert class
        /// </summary>
        /// <typeparam name="TResult">Return type</typeparam>
        public static TResult[] ReadArray<TResult>(char separator = ' ') => ReadArray(Selector<TResult>, separator);

        /// <summary>
        /// Reads elements of next line into an array
        /// </summary>
        public static string[] ReadArray(char separator = ' ') => ReadArray(x => x, separator);


        /// <summary>
        /// Read lines into an array using custom selector
        /// </summary>
        /// <param name="lines">Number of lines</param>
        /// <param name="selector">Transform function</param>
        public static TResult[] ReadArray<TResult>(int lines, Func<string, TResult> selector)
        {
            var result = new TResult[lines];
            for (int i = 0; i < lines; i++)
                result[i] = Read(selector);
            return result;
        }

        /// <summary>
        /// Reads lines into an array using Convert class
        /// </summary>
        /// <param name="lines">Number of lines</param>
        public static TResult[] ReadArray<TResult>(int lines) => ReadArray(lines, Selector<TResult>);

        /// <summary>
        /// Reads lines into an array
        /// </summary>
        /// <param name="lines">Number of lines</param>
        public static string[] ReadArray(int lines) => ReadArray(lines, l => l);


        /// <summary>
        /// Reads lines into an array while lines match the predicate using custom selector
        /// </summary>
        /// <param name="predicate">Match lines</param>
        /// <param name="selector">Transform function</param>
        public static TResult[] ReadArray<TResult>(Predicate<string> predicate, Func<string, TResult> selector)
        {
            var result = new List<TResult>();
            string line;
            while (predicate(line = Read()))
                result.Add(selector(line));
            return result.ToArray();
        }

        /// <summary>
        /// Reads lines into an array while lines match the predicate using Convert class
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static TResult[] ReadArray<TResult>(Predicate<string> predicate) => ReadArray(predicate, Selector<TResult>);

        /// <summary>
        /// Reads lines into an array while lines match the predicate
        /// </summary>
        /// <param name="predicate">Match lines</param>
        public static string[] ReadArray(Predicate<string> predicate) => ReadArray(predicate, l => l);




        /// <summary>
        /// Reads elements in all lines into a jagged array using custom selector
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="lines"></param>
        /// <param name="selector"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static TResult[][] ReadAllArrays<TResult>(int lines, Func<string, TResult> selector, char separator = ' ')
        {
            var result = new TResult[lines][];
            for (int i = 0; i < lines; i++)
                result[i] = Read().Split(separator).Select(selector).ToArray();
            return result;
        }

        /// <summary>
        /// Reads elements in all lines into a jagged array using Convert class
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="lines"></param>
        /// <param name="selector"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static TResult[][] ReadAllArrays<TResult>(int lines, char separator = ' ')
            => ReadAllArrays(lines, Selector<TResult>, separator);

        /// <summary>
        /// Reads elements in all lines into a jagged array
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="lines"></param>
        /// <param name="selector"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[][] ReadAllArrays(int lines, char separator = ' ')
            => ReadAllArrays(lines, l => l, separator);


        /// <summary>
        /// Reads elements in all lines into a jagged array using custom selector
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="lines"></param>
        /// <param name="selector"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static TResult[][] ReadAllArrays<TResult>(Predicate<string> predicate, Func<string, TResult> selector, char separator = ' ')
        {

            var result = new List<TResult[]>();
            string line;
            while (predicate(line = Read()))
                result.Add(Read().Split(separator).Select(selector).ToArray());
            return result.ToArray();
        }

        /// <summary>
        /// Reads elements in all lines into a jagged array using Convert class
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="lines"></param>
        /// <param name="selector"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static TResult[][] ReadAllArrays<TResult>(Predicate<string> predicate, char separator = ' ')
            => ReadAllArrays(predicate, Selector<TResult>, separator);

        /// <summary>
        /// Reads elements in all lines into a jagged array
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="lines"></param>
        /// <param name="selector"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[][] ReadAllArrays(Predicate<string> predicate, char separator = ' ')
            => ReadAllArrays(predicate, l => l, separator);



        private static Queue<string> elements;
        private static T Next<T>()
        {
            if (elements?.Count == 0)
                return default(T);
            return Selector<T>(elements.Dequeue());
        }

        public static TResult ReadFirst<TResult>()
        {
            elements = new Queue<string>(ReadArray());
            return Next<TResult>();
        }

        public static TResult ReadNext<TResult>()
        {
            return Next<TResult>();
        }


        public static void Read<T>(out T v1)
        {
            elements = new Queue<string>(ReadArray());
            v1 = Next<T>();
        }
        public static void Read<T>(out T v1, out T v2)
        {
            Read(out v1);
            v2 = Next<T>();
        }
        public static void Read<T>(out T v1, out T v2, out T v3)
        {
            Read(out v1, out v2);
            v3 = Next<T>();
        }
        public static void Read<T>(out T v1, out T v2, out T v3, out T v4)
        {
            Read(out v1, out v2, out v3);
            v4 = Next<T>();
        }
        public static void Read<T>(out T v1, out T v2, out T v3, out T v4, out T v5)
        {
            Read(out v1, out v2, out v3, out v4);
            v5 = Next<T>();
        }
        public static void Read<T>(out T v1, out T v2, out T v3, out T v4, out T v5, out T v6)
        {
            Read(out v1, out v2, out v3, out v4, out v5);
            v6 = Next<T>();
        }
        public static void Read<T>(out T v1, out T v2, out T v3, out T v4, out T v5, out T v6, out T v7)
        {
            Read(out v1, out v2, out v3, out v4, out v5, out v6);
            v7 = Next<T>();
        }
        public static void Read<T>(out T v1, out T v2, out T v3, out T v4, out T v5, out T v6, out T v7, out T v8)
        {
            Read(out v1, out v2, out v3, out v4, out v5, out v6, out v7);
            v8 = Next<T>();
        }
    }
}

}