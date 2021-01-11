using System.Collections.Generic;

namespace Cys_Model
{
    public class TreeNode
    {
        public int NodeId { get; set; }
        public int ParentId { get; set; }
        public string NodeName { get; set; }
        public string Url { get; set; }
        public List<TreeNode> ChildNodes { get; set; } = new List<TreeNode>();
        /// <summary>
        /// //0-文件，1-文件夹
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; } 
    }
}