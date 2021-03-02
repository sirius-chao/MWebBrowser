using Cys_Common;
using Cys_Controls.Code;
using Cys_CustomControls.Controls;
using Cys_Model;
using MWebBrowser.Code.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MWebBrowser.ViewModel;

namespace MWebBrowser.View
{
    /// <summary>
    /// Interaction logic for FavoritesBarUc.xaml
    /// </summary>
    public partial class FavoritesBarUc : UserControl
    {
        private const double _textMaxWidth = 300;
        /// <summary>
        /// 记录当前右键选中的Item;
        /// </summary>
        private MFavoritesItem _currentRightItem;
        public Func<WebTabControlViewModel> GetWebUrlEvent;
        public Action<string> OpenNewTabEvent;
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
                AddFavoritesItem(null, child, true);
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

        /// <summary>
        /// 递归添加子集
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="treeNode"></param>
        /// <param name="isRoot"></param>
        private void AddFavoritesItem(MFavoritesItem parent, TreeNode treeNode, bool isRoot)
        {
            double left = treeNode.Level * 10;
            var favoritesItem = new MFavoritesItem
            {
                Header = treeNode.NodeName,
                Type = treeNode.Type,
                NodeId = treeNode.NodeId,
                Level = treeNode.Level,
                TextMaxWidth = _textMaxWidth
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
                    AddFavoritesItem(favoritesItem, child, false);
                }
            }
            if (!isRoot)
                parent.Items.Add(favoritesItem);
            else
            {
                MenuParent.Items.Add(favoritesItem);
            }
        }

        /// <summary>
        /// 处理右键菜单打开前的行为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FavoritesTree_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            _currentRightItem = ControlHelper.FindVisualParent<MFavoritesItem>(e.OriginalSource as DependencyObject);
            if (null == _currentRightItem)
            {
                e.Handled = true;
                return;
            }
            if (_currentRightItem.Type == 0)
            {
                OpenAllFolder.Visibility = Visibility.Collapsed;
                OpenNewAllFolder.Visibility = Visibility.Collapsed;
            }
            else
            {
                OpenAllFolder.Visibility = Visibility.Visible;
                OpenNewAllFolder.Visibility = Visibility.Visible;
            }
        }

        private void FavoritesTree_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(MenuParent.Items.CurrentItem is MFavoritesItem item)) return;
            if (item.Type == 1) return;
            if (item.IsEdit) return;
            if (!GlobalInfo.FavoritesSetting.FavoritesInfos.Exists(x => x.NodeId == item.NodeId)) return;
            var treeNode = GlobalInfo.FavoritesSetting.FavoritesInfos.First(x => x.NodeId == item.NodeId);
            //OpenNewTabEvent?.Invoke(treeNode.Url);
        }

        private void FavoritesTree_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            if (_currentRightItem.IsEdit)
            {
                _currentRightItem.IsEdit = false;
                _currentRightItem.Header = _currentRightItem.EditText;

                if (!GlobalInfo.FavoritesSetting.FavoritesInfos.Exists(x => x.NodeId == _currentRightItem.NodeId)) return;
                var treeNode = GlobalInfo.FavoritesSetting.FavoritesInfos.First(x => x.NodeId == _currentRightItem.NodeId);
                treeNode.NodeName = _currentRightItem.EditText;
                _currentRightItem.EditText = null;
            }
        }

        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="sender"></param> 
        /// <param name="e"></param>
        private void AddFavorites_OnClick(object sender, RoutedEventArgs e)
        {
            var model = GetWebUrlEvent?.Invoke();
            if (null == model) return;
            if (_currentRightItem?.Type != 1) return;
            var newTreeNode = GetNewTreeNodeInfo(false, 0, model.Title, model.CurrentUrl);
            _currentRightItem.Items.Add(newTreeNode.Item2);
            GlobalInfo.FavoritesSetting.FavoritesInfos.Add(newTreeNode.Item1);
        }
        /// <summary>
        /// 添加文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFolder_OnClick(object sender, RoutedEventArgs e)
        {
            var newTreeNode = GetNewTreeNodeInfo(false, 1, "新建文件夹", null);
            if (_currentRightItem != null && _currentRightItem.Type == 1)
            {
                _currentRightItem.Items.Add(newTreeNode.Item2);
                GlobalInfo.FavoritesSetting.FavoritesInfos.Add(newTreeNode.Item1);
            }
        }

        private Tuple<TreeNode, MFavoritesItem> GetNewTreeNodeInfo(bool isRoot, int type, string nodeName, string url)
        {
            int parentId = 0;
            int level = 1;
            if (!isRoot)
            {
                parentId = _currentRightItem.NodeId;

                if (parentId == -1)
                {
                    level = +1;
                }
                else
                {
                    level = _currentRightItem.Level + 1;
                }
            }
            int nodeMax = GlobalInfo.FavoritesSetting.FavoritesInfos.Max(x => x.NodeId);
            var treeNode = new TreeNode
            {
                Url = url,
                ParentId = parentId,
                NodeId = nodeMax + 1,
                NodeName = nodeName,
                Type = type,
                Level = level,
            };
            var favoritesItem = new MFavoritesItem
            {
                Header = treeNode.NodeName,
                Type = type,
                NodeId = treeNode.NodeId,
                Level = level,
                TextMaxWidth = _textMaxWidth
            };
            if (type == 0)
            {
                favoritesItem.Icon = "\ueb1e";
                favoritesItem.IconForeground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            else
            {
                favoritesItem.Icon = "\ue903";
                favoritesItem.IconForeground = new SolidColorBrush(Color.FromRgb(255, 205, 44));
            }
            return new Tuple<TreeNode, MFavoritesItem>(treeNode, favoritesItem);
        }

        /// <summary>
        /// 删除当前节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (_currentRightItem?.Parent == null) return;
            for (int i = _currentRightItem.Items.Count; i > 0; i--)
            {
                _currentRightItem.Items.Remove(_currentRightItem.Items[^1]);
                if (!GlobalInfo.FavoritesSetting.FavoritesInfos.Exists(x => x.NodeId == _currentRightItem.NodeId))
                    continue;
            }

            if (_currentRightItem.Parent is MFavoritesItem items)
            {
                if (GlobalInfo.FavoritesSetting.FavoritesInfos.Exists(x => x.NodeId == _currentRightItem.NodeId))
                {
                    var currentNode = (GlobalInfo.FavoritesSetting.FavoritesInfos.FirstOrDefault(x => x.NodeId == _currentRightItem.NodeId));
                    GlobalInfo.FavoritesSetting.FavoritesInfos.Remove(currentNode);
                }
                items.Items.Remove(_currentRightItem);
            }

            if (_currentRightItem.Parent is MFavorites parent)
            {
                if (GlobalInfo.FavoritesSetting.FavoritesInfos.Exists(x => x.NodeId == _currentRightItem.NodeId))
                {
                    var currentNode = (GlobalInfo.FavoritesSetting.FavoritesInfos.FirstOrDefault(x => x.NodeId == _currentRightItem.NodeId));
                    GlobalInfo.FavoritesSetting.FavoritesInfos.Remove(currentNode);
                }
                parent.Items.Remove(_currentRightItem);
            }
        }

        private void ReName_OnClick(object sender, RoutedEventArgs e)
        {
            if (null == _currentRightItem) return;
            if (_currentRightItem.NodeId == 0) return;
            _currentRightItem.IsEdit = true;
        }
    }
}
