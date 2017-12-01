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

    /// <summary>
    /// Tree Node
    /// </summary>
    public class DBNode
    {
        public DBManager Manager { get; private set; }
        public DBNode[] ChildNodes { get; private set; }
        public int ChildCount
        {
            get
            {
                int c = 0;
                foreach (var ch in ChildNodes)
                    if (ch != null)
                        c++;
                return c;
            }
        }

        public bool HasChild { get { return ChildCount > 0; } }
        public bool HasParent { get { return Parent != null; } }

        public int Index { get; private set; }
        public DBNode Parent { get; private set; }

        public void SetParent(DBNode parent, int id)
        {
            if(parent!=null && id>=0 && id<Manager.DivisionCount)
            Parent = parent;
        }

        public virtual void OnCreate(int[] keys, int level, object userData) { }
        public virtual void OnDelete(int[] keys, int level, object userData) { }
        public virtual void OnSubDivision(int[][] childKeys, int childLevel, object userData) { }

        public DBNode(DBManager manager)
        {
            this.Manager = manager;
            this.ChildNodes = new DBNode[manager.DivisionCount];
            Index = -1;
            Parent = null;
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

    /// <summary>
    /// Tree Manager
    /// </summary>
    public class DBManager
    {
        /// <summary>
        /// Number of keys of a node
        /// </summary>
        public int Dimension { get; private set; }

        /// <summary>
        /// Uplimit number of child nodes of a node
        /// </summary>
        public int DivisionCount { get{ return 1 << (Dimension - 1); } }

        /// <summary>
        /// 
        /// </summary>
        public int MaxLevel { get; private set; }

        public DBNode CreateNode()
        {
            return new DBNode(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        public void CheckKeysSize(int[] keys)
        {
            int size = Dimension;
            if (keys.Length != size)
                throw new Exception(String.Format(
                    "Wrong array size, expected size is {0} but got {1}",
                    size, keys.Length));
        }

        /// <summary>
        /// Mix specific bits of keys to get sublevel index
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetLevelIndex(int[] keys, int level)
        {
            int size = DivisionCount;
            int result = 0;
            for (int i = 0; i < size; i++)
            {
                result |= ((keys[i] >> level) & 1) << i;
            }
            return result;
        }

        /// <summary>
        /// Find the node with given key array
        /// </summary>
        /// <param name="root"></param>
        /// <param name="rootLevel"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public QueryResult Search(DBNode root, int rootLevel, int[] keys)
        {
            if (rootLevel == 0 || root.ChildNodes == null)
                return new QueryResult(root, rootLevel);

            var child = root.ChildNodes[GetLevelIndex(keys, rootLevel - 1)];
            if (child == null)
                return new QueryResult(root, rootLevel);
            return Search(child, rootLevel - 1, keys);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="rootLevel"></param>
        /// <param name="keys"></param>
        /// <param name="insertLevel"></param>
        /// <returns></returns>
        public QueryResult Insert(DBNode root, int rootLevel, int[] keys, int insertLevel, object userData=null)
        {
            if (rootLevel < insertLevel)
            {
                throw new DBTreeErrorException();
            }
            if (rootLevel == insertLevel) //Insertion fail
            {
                throw new DBInsertNodeExistException();
            }
            
            int index = GetLevelIndex(keys, rootLevel - 1);
            //Debug.Assert(rootLevel >= insertLevel);
            

            if (root.ChildNodes[index] == null && rootLevel == insertLevel + 1) //Insert success
            {
                var node = root.ChildNodes[index] = CreateNode();
                node.OnCreate(keys, insertLevel, userData);
                return new QueryResult(node, insertLevel);
            }

            if (root.ChildNodes[index] == null)
                root.ChildNodes[index] = CreateNode();
            return Insert(root.ChildNodes[index], rootLevel - 1, keys, insertLevel);
        }
        class DBTreeErrorException : Exception { }
        class DBInsertNodeExistException : Exception { }

        public void SubDivide(DBNode root, int rootLevel, int[] keys, int devideLevel, object userData = null)
        {
            if (rootLevel < devideLevel)
            {
                throw new DBTreeErrorException();
            }
            if (rootLevel == devideLevel) 
                return;

            //TODO: Check if Subdividable
            //Assume ture if no child
                       
            for(int i = 0; i < DivisionCount; i++)
            {
                var node = CreateNode();
                node.OnCreate(keys, rootLevel - 1, userData);
                node.SetParent(root, i);
            }

            int index = GetLevelIndex(keys, rootLevel - 1);
            SubDivide(root.ChildNodes[index], rootLevel - 1, keys, devideLevel, userData);
        }
    }
}
