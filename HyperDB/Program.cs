using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperDB
{
    class TestNode : DBNode
    {
        string createMessage = null;
        public TestNode(DBManager manager, int level = -1) : base(manager, level)
        {
        }
        public override string ToString()
        {
            return "Test Node "+createMessage;
        }
        public override void OnInsert(object userData)
        {
            createMessage = "" + userData;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test program");

            Console.WriteLine("Creating manager:");
            DBManager db = new DBManager(31, 3, typeof(TestNode));
            
            for(int i = 0; i < 4; i++)
            {
                for(int j=0;j<4;j++)
                {
                    db.Insert(new int[] { i<<6, 0, j<<6 }, 6);
                }
            }
            Console.WriteLine(db.Dump(db.Root));
            //Console.WriteLine("Test insertion");
            //var r1 = db.Insert(new int[] { 2, 11 }, 0, "Insertion Test");
            //var r2 = db.Insert(new int[] { 0, 1 }, 0, "Insertion Test");
            //db.Insert(new int[] { 3, 4 }, 0, "Insertion Test");
            //db.Insert(new int[] { 7, 6 }, 0, "Insertion Test");
            //db.Insert(new int[] { 15, 8 }, 2, "Insertion Test");
            //Console.WriteLine(db.Dump(db.Root));

            //Console.WriteLine("Test Search");
            //var s1 = db.Search(new int[] { 3, 4 }, 0);
            //var s2 = db.Search(new int[] { 3, 4 }, 1);
            //var s3 = db.Search(new int[] { 3, 3 }, 0);
            //Console.WriteLine(db.Dump(s1.Result));
            //Console.WriteLine(db.Dump(s2.Result));
            //Console.WriteLine(db.Dump(s3.Result));

            //Console.WriteLine("Test leaf deletion");
            //db.Delete(s1.Result);
            //Console.WriteLine(db.Dump(db.Root));

            //Console.WriteLine("Test non-leaf deletion");
            //db.Delete(db.Search(new int[] { 0, 8 }, 3).Result);
            //Console.WriteLine(db.Dump(db.Root));

            //Console.WriteLine("Test full subdivision");
            //var sd = db.Search(new int[] { 12, 8 }, 2);
            //db.SubDivide(sd.Result, null, 0, "Full subdivision");
            //Console.WriteLine(db.Dump(db.Root));

            //Console.WriteLine("Test single subdivision");
            //db.Delete(sd.Result);
            //db.Insert(new int[] { 12, 8 }, 2);
            //sd = db.Search(new int[] { 12, 8 }, 2);
            //db.SubDivide(sd.Result, new int[] { 14, 9 }, 0, "Single subdivision");
            //Console.WriteLine(db.Dump(db.Root));

            //Console.WriteLine("Test subexclude");
            //db.Delete(sd.Result);
            //db.Insert(new int[] { 12, 8 }, 2);
            //sd = db.Search(new int[] { 12, 8 }, 2);
            //db.SubExclude(sd.Result, new int[] { 14, 9 }, 0, "Subexclude");
            //Console.WriteLine(db.Dump(db.Root));
        }
    }

}
