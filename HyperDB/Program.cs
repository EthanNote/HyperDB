using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperDB
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public abstract class DBNode
    {
        public DBManager manager { get; }
        public DBNode[] childNodes;
        public object userData;
    }

    public class QueryResult
    {
        public DBNode node;
        public int level;
        public QueryResult(DBNode node, int level)
        {
            this.node = node;
            this.level = level;
        }
    }

    public class DBManager
    {
        public int Dimension { get; private set; }
        public int MaxLevel { get; private set; }

        public int MixIndex(int[] keys, int level)
        {
            int size = 1 << (Dimension - 1);
            if (keys.Length != size)
                throw new Exception(String.Format(
                    "Wrong array size, expected size is {0} but got {1}", 
                    size, keys.Length));

            int result = 0;

            for (int i = 0; i < size; i++)
            {
                result |= ((keys[i] >> level) & 1) << i;
            }
            return result;
        }

        public QueryResult Query(DBNode root, int rootLevel, int[] keys)
        {
            if (rootLevel == 0 || root.childNodes == null)
                return new QueryResult(root, rootLevel);

            var child = root.childNodes[MixIndex(keys, rootLevel - 1)];
            if (child == null)
                return new QueryResult(root, rootLevel);
            return Query(child, rootLevel - 1, keys);
        }

    }
}
