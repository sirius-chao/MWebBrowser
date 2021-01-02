using MWebBrowser.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace MWebBrowser.View
{
    /// <summary>
    /// FavourMenuUc.xaml 的交互逻辑
    /// </summary>
    public partial class FavoritesMenuUc : UserControl
    {
        public FavoritesMenuUc()
        {
            InitializeComponent();
           // GetFavoritesInfo();
            //FavoritesTree.ItemsSource = getNodes(0, nodes);
        }
        private List<TreeNode> nodes;
        private void GetFavoritesInfo()
        {
            nodes = new List<TreeNode>()
            {
                new TreeNode(){ParentID=0, NodeID=1, NodeName = "Chapter1" },
                new TreeNode(){ParentID=0, NodeID=2, NodeName="Chapter2"},
                new TreeNode(){ParentID=0,NodeID=3, NodeName="Chapter3"},
                new TreeNode(){ParentID=1, NodeID=4, NodeName="Section1.1"},
                new TreeNode(){ParentID=1, NodeID=5, NodeName="Section1.2"},
                new TreeNode(){ParentID=2, NodeID=6, NodeName="Section2.1"},
                new TreeNode(){ParentID=3, NodeID=7, NodeName="Section3.1"},
                new TreeNode(){ParentID=6, NodeID=8, NodeName="SubSection2.1.1"},
                new TreeNode(){ParentID=6, NodeID=9, NodeName="SubSection2.1.2"},
                new TreeNode(){ParentID=2, NodeID=10,NodeName="Section2.2"},
                new TreeNode(){ParentID=3, NodeID=11, NodeName="Section3.2"}
            };
        }

        private List<TreeNode> getNodes(int parentID, List<TreeNode> nodes)
        {
            List<TreeNode> mainNodes = nodes.Where(x => x.ParentID == parentID).ToList();
            List<TreeNode> otherNodes = nodes.Where(x => x.ParentID != parentID).ToList();
            foreach (TreeNode node in mainNodes)
                node.ChildNodes = getNodes(node.NodeID, otherNodes);
            return mainNodes;
        }
    }
}
