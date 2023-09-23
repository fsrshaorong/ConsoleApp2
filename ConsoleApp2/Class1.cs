using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{ 
    internal class Program
    {
        static void InsertDictionary(Dictionary<int,char>dictionary)
        {
            dictionary.Add(1,'一');
            dictionary.Add(2,'二');
            dictionary.Add(3,'三');
            dictionary.Add(4,'四');
            dictionary.Add(5,'五');
            dictionary.Add(6,'六');
            dictionary.Add(7,'七');
            dictionary.Add(8,'八');
            dictionary.Add(9,'九');
        }

        static void Find(string s,Dictionary<int,char>dictionary)
        {
            for (int i = 0; i < s.Length; i++)
            {
                int w = Convert.ToInt32(s[i]);
                Console.WriteLine(dictionary[w]);
            }
        }
        
        static void Main(string[] args)
        {
            Dictionary<int,char>dictionary= new Dictionary<int, char>();
            InsertDictionary(dictionary);
            Console.WriteLine("输入");
            string s=Console.ReadLine();
            Find(s,dictionary);
            Console.WriteLine("nihap");
        }
    }
}