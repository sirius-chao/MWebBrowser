using Cys_CustomControls.Controls;
using MWebBrowser.Code.Helpers;
using MWebBrowser.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MWebBrowser.View
{
    /// <summary>
    /// FavourMenuUc.xaml 的交互逻辑
    /// </summary>
    public partial class FavoritesMenuUc : UserControl
    {
        /// <summary>
        /// 记录当前右键选中的Item;
        /// </summary>
        private MTreeViewItem _currentRightItem;
        public FavoritesMenuUc()
        {
            InitializeComponent();
            GetFavoritesInfo();
        }
        private List<TreeNode> nodes;
        private void GetFavoritesInfo()
        {
            nodes = new List<TreeNode>()
            {
                new TreeNode(){ParentId=-1, NodeId=0, NodeName = "收藏夹",Type = 1},
                new TreeNode(){ParentId=0, NodeId=1, NodeName = "文本",Type = 1},
                new TreeNode(){ParentId=0, NodeId=2, NodeName = "音频",Type = 1},
                new TreeNode(){ParentId=0, NodeId=3, NodeName = "视频",Type = 1},

                new TreeNode(){ParentId=1, NodeId=11, NodeName = "文本1",Type = 0},
                new TreeNode(){ParentId=1, NodeId=12, NodeName = "文本2",Type = 0},
                new TreeNode(){ParentId=1, NodeId=13, NodeName = "文本3",Type = 0},
            };
            List<TreeNode> root = GetNodes(-1, nodes);
            AddTreeViewItems(null, root[0], true);
        }

        private List<TreeNode> GetNodes(int parentId, List<TreeNode> nodes)
        {
            List<TreeNode> mainNodes = nodes.Where(x => x.ParentId == parentId).OrderByDescending(x=>x.Type).ToList();
            List<TreeNode> otherNodes = nodes.Where(x => x.ParentId != parentId).OrderByDescending(x => x.Type).ToList();
            foreach (TreeNode node in mainNodes)
                node.ChildNodes = GetNodes(node.NodeId, otherNodes);
            return mainNodes;
        }

        private void AddTreeViewItems(MTreeViewItem parent, TreeNode treeNode, bool isRoot)
        {
            var treeViewItem = new MTreeViewItem();
            if (treeNode.ChildNodes.Count <= 0)
            {
                if (treeNode.Type == 0)
                {
                    treeViewItem.Header = treeNode.Url;
                    treeViewItem.Icon = "\ueb1e";
                    treeViewItem.IsExpandedIcon = "\ueb1e";
                    treeViewItem.IconForeground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
                else
                {
                    treeViewItem.Header = treeNode.NodeName;
                }
            }
            else
            {
                treeViewItem.Header = treeNode.NodeName;
                foreach (var child in treeNode.ChildNodes)
                {
                    AddTreeViewItems(treeViewItem, child, false);
                }
            }
            if (!isRoot)
                parent.Items.Add(treeViewItem);
            else
            {
                FavoritesTree.Items.Add(treeViewItem);
            }
        }

        private void FavoritesButton_OnChecked(object sender, RoutedEventArgs e)
        {
            FavoritesPop.IsOpen = true;
        }

        private void FavoritesButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            FavoritesPop.IsOpen = false;
        }

        private void FavoritesTree_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            _currentRightItem = ControlHelper.FindVisualParent<MTreeViewItem>(e.OriginalSource as DependencyObject);
            if (null == _currentRightItem)
            {
                e.Handled = true;
            }
        }
    }
}
