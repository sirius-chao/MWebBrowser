using System.Collections.Generic;

namespace MWebBrowser.ViewModel
{
    public class TreeNode
    {
        public int NodeID { get; set; }
        public int ParentID { get; set; }
        public string NodeName { get; set; }
        public List<TreeNode> ChildNodes { get; set; }
        public TreeNode()
        {
            ChildNodes = new List<TreeNode>();
        }
    }
}
