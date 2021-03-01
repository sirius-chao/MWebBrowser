using Cys_Common;
using Cys_CustomControls.Controls;
using Cys_Model;
using MWebBrowser.Code.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace MWebBrowser.View
{
    /// <summary>
    /// Interaction logic for FavoritesBarUc.xaml
    /// </summary>
    public partial class FavoritesBarUc : UserControl
    {
        public FavoritesBarUc()
        {
            InitializeComponent();
            this.Loaded += FavoritesBarUc_Loaded;
        }

        private void FavoritesBarUc_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.IsInDesignMode()) return;
            GetFavoritesInfo();
        }

        private void GetFavoritesInfo()
        {
            List<TreeNode> root = GetNodes(-1, GlobalInfo.FavoritesSetting.FavoritesInfos);
            if (root == null || root.Count <= 0 || root[0].ChildNodes.Count <= 0) return;
            foreach (var child in root[0].ChildNodes)
            {
                AddTreeViewItems(null, child, true);
            }
        }

        private List<TreeNode> GetNodes(int parentId, List<TreeNode> nodes)
        {
            List<TreeNode> mainNodes = nodes.Where(x => x.ParentId == parentId).OrderByDescending(x => x.Type).ToList();
            List<TreeNode> otherNodes = nodes.Where(x => x.ParentId != parentId).OrderByDescending(x => x.Type).ToList();
            foreach (TreeNode node in mainNodes)
                node.ChildNodes = GetNodes(node.NodeId, otherNodes);
            return mainNodes;
        }

        private void AddTreeViewItems(MFavoritesItem parent, TreeNode treeNode, bool isRoot)
        {
            double left = treeNode.Level * 10;
            var favoritesItem = new MFavoritesItem
            {
                Header = treeNode.NodeName
            };

            if (treeNode.Type == 0)
            {
                favoritesItem.Icon = "\ueb1e";
                favoritesItem.IconForeground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            else
            {
                favoritesItem.Icon = "\ue903";
                favoritesItem.IconForeground = new SolidColorBrush(Color.FromRgb(255, 205, 44));
            }
            if (treeNode.ChildNodes.Count > 0)
            {
                foreach (var child in treeNode.ChildNodes)
                {
                    AddTreeViewItems(favoritesItem, child, false);
                }
            }
            if (!isRoot)
                parent.Items.Add(favoritesItem);
            else
            {
                MenuParent.Items.Add(favoritesItem);
            }
        }
    }
}
