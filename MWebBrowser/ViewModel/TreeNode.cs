﻿using System.Collections.Generic;

namespace MWebBrowser.ViewModel
{
    public class TreeNode
    {
        public int NodeId { get; set; }
        public int ParentId { get; set; }
        public string NodeName { get; set; }
        public string Url { get; set; } = "http://www.baidu.com";
        public List<TreeNode> ChildNodes { get; set; } = new List<TreeNode>();
        public int Type { get; set; } //0-文件，1-文件夹
    }
}