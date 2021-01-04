using System.Collections.Generic;

namespace MWebBrowser.ViewModel
{
    public class TreeNode
    {
        public int NodeID { get; set; }
        public int ParentID { get; set; }
        public string NodeName { get; set; }
        public string Url { get; set; } = "http://www.baidu.com";
        public List<TreeNode> ChildNodes { get; set; }
        public int Type { get; set; } //0-文件，1-文件夹
        public TreeNode()
        {
            ChildNodes = new List<TreeNode>();
        }
    }
}
