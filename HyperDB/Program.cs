using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperDB
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test program");

            Console.WriteLine("Creating manager:");
            DBManager db = new DBManager(4, 2);
            db.Insert(new int[] { 2, 11 }, 0);
            Console.WriteLine(db.Dump(db.Root));
            var s=db.Search(new int[] { 2, 11 }, 0);
            var n= db.Search(new int[] { 2, 10 }, 0);

        }
    }

}
