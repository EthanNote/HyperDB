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
        }
    }

    //public interface DBUserData
    //{
    //    void OnCreate(int keys, int level);
    //    void OnDelete(int keys, int level);
    //    void OnSubDivision(int[][] childKeys, int childLevel);
    //}

    public class DBNode
    {
        public DBManager manager { get; }
        public DBNode[] childNodes;
        public object userData;

        public virtual void OnCreate(int[] keys, int level) { }
        public virtual void OnDelete(int[] keys, int level) { }
        public virtual void OnSubDivision(int[][] childKeys, int childLevel) { }


        public void EnableChildArray(int dim)
        {
            if (childNodes == null)
                childNodes = new DBNode[1 << (dim - 1)];
        }
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
        public int Keysize { get { return 1 << (Dimension - 1); } }
        public int MaxLevel { get; private set; }

        delegate DBNode CreateNode();
        CreateNode OnCreateNode = () => { return new DBNode(); };

        public void CheckKeysSize(int[] keys)
        {
            int size = Keysize;
            if (keys.Length != size)
                throw new Exception(String.Format(
                    "Wrong array size, expected size is {0} but got {1}",
                    size, keys.Length));
        }


        public int MixIndex(int[] keys, int level)
        {
            int size = Keysize;
            int result = 0;
            for (int i = 0; i < size; i++)
            {
                result |= ((keys[i] >> level) & 1) << i;
            }
            return result;
        }


        public QueryResult Search(DBNode root, int rootLevel, int[] keys)
        {
            if (rootLevel == 0 || root.childNodes == null)
                return new QueryResult(root, rootLevel);

            var child = root.childNodes[MixIndex(keys, rootLevel - 1)];
            if (child == null)
                return new QueryResult(root, rootLevel);
            return Search(child, rootLevel - 1, keys);
        }


        public QueryResult Insert(DBNode root, int rootLevel, int[] keys, int insertLevel)
        {

            root.EnableChildArray(Dimension);
            int index = MixIndex(keys, rootLevel - 1);
            Debug.Assert(rootLevel >= insertLevel);

            if (rootLevel < insertLevel)
            {
                throw new Exception("DB TREE ERROR");
            }
            if (rootLevel == insertLevel) //Insertion fail
            {
                return new QueryResult(null, rootLevel);
            }

            if (root.childNodes[index] == null && rootLevel == insertLevel + 1) //Insert success
            {
                var node = root.childNodes[index] = OnCreateNode();
                node.OnCreate(keys, insertLevel);
                return new QueryResult(node, insertLevel);
            }

            if (root.childNodes[index] == null)
                root.childNodes[index] = OnCreateNode();
            return Insert(root.childNodes[index], rootLevel - 1, keys, insertLevel);
        }

    }
}
