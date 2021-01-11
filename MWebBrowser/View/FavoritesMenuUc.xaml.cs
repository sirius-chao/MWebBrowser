using Cys_Common;
using Cys_CustomControls.Controls;
using Cys_Model;
using MWebBrowser.Code.Helpers;
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
        private void GetFavoritesInfo()
        {
            List<TreeNode> root = GetNodes(-1, GlobalInfo.FavoritesSetting.FavoritesInfos);
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
