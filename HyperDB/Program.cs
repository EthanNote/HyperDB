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

            Console.WriteLine("Test insertion");
            var r1=db.Insert(new int[] { 2, 11 }, 0);
            var r2=db.Insert(new int[] { 0, 1 }, 0);
            db.Insert(new int[] { 3, 4 }, 0);
            db.Insert(new int[] { 7, 6 }, 0);
            db.Insert(new int[] { 15, 8 }, 2);
            Console.WriteLine(db.Dump(db.Root));

            Console.WriteLine("Test Search");
            var s1 = db.Search(new int[] { 3, 4 }, 0);
            var s2 = db.Search(new int[] { 3, 4 }, 1);
            var s3 = db.Search(new int[] { 3, 3 }, 0);
            Console.WriteLine(db.Dump(s1.Result));
            Console.WriteLine(db.Dump(s2.Result));
            Console.WriteLine(db.Dump(s3.Result));

            Console.WriteLine("Test leaf deletion");
            db.Delete(s1.Result);
            Console.WriteLine(db.Dump(db.Root));

            Console.WriteLine("Test non-leaf deletion");
            db.Delete(db.Search(new int[] { 0, 8 }, 3).Result);
            Console.WriteLine(db.Dump(db.Root));
        }
    }

}
