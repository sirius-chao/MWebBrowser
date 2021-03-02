using Cys_Common;
using Cys_Controls.Code;
using Cys_CustomControls.Controls;
using Cys_Model;
using MWebBrowser.Code.Helpers;
using MWebBrowser.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MWebBrowser.View
{
    /// <summary>
    /// FavourMenuUc.xaml 的交互逻辑
    /// </summary>
    public partial class FavoritesMenuUc : UserControl
    {
        public Func<WebTabControlViewModel> GetWebUrlEvent;
        public Action<string> OpenNewTabEvent;

        public Action RefreshFavoritesBarEvent;
        /// <summary>
        /// 记录当前右键选中的Item;
        /// </summary>
        private MTreeViewItem _currentRightItem;
        private readonly double _textMaxWidth = 300;
        public FavoritesMenuUc()
        {
            InitializeComponent();
            this.Loaded += FavoritesMenuUc_Loaded;
        }

        private void FavoritesMenuUc_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsInDesignMode())
                return;
            GetFavoritesInfo();
        }

        private void GetFavoritesInfo()
        {
            List<TreeNode> root = GetNodes(-1, GlobalInfo.FavoritesSetting.FavoritesInfos);
            AddTreeViewItems(null, root[0], true);
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
        private void AddTreeViewItems(MTreeViewItem parent, TreeNode treeNode, bool isRoot)
        {
            double left = treeNode.Level * 10;
            var treeViewItem = GetNewFavoritesItem(treeNode,left);
            if (treeNode.ChildNodes.Count <= 0)
            {
                if (treeNode.Type == 0)
                {
                    treeViewItem.Icon = "\ueb1e";
                    treeViewItem.IsExpandedIcon = "\ueb1e";
                    treeViewItem.IconForeground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
            }
            else
            {
                foreach (var child in treeNode.ChildNodes)
                {
                    AddTreeViewItems(treeViewItem, child, false);
                }
            }
            if (!isRoot)
                parent.Items.Add(treeViewItem);
            else
                FavoritesTree.Items.Add(treeViewItem);
        }


        /// <summary>
        /// 获取FavoritesItem
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        private MTreeViewItem GetNewFavoritesItem(TreeNode treeNode, double left)
        {
            return new MTreeViewItem
            {
                Header = treeNode.NodeName,
                Type = treeNode.Type,
                NodeId = treeNode.NodeId,
                Level = treeNode.Level,
                ItemMargin = new Thickness(left, 0, 0, 0),
                TextMaxWidth = _textMaxWidth - left,
            };
        }

        private void FavoritesButton_OnChecked(object sender, RoutedEventArgs e)
        {
            FavoritesPop.IsOpen = true;
        }

        private void FavoritesButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            FavoritesPop.IsOpen = false;
            if (_currentRightItem == null)
                return;
            _currentRightItem.IsEdit = false;
        }

        /// <summary>
        /// 处理右键菜单打开前的行为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FavoritesTree_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            _currentRightItem = ControlHelper.FindVisualParent<MTreeViewItem>(e.OriginalSource as DependencyObject);
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
            DeleteNode.Visibility = _currentRightItem.NodeId == 0 ? Visibility.Collapsed : Visibility.Visible;
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
           
            if (sender is Button)
            {
                if (!(FavoritesTree.Items[0] is MTreeViewItem item)) return;
                var newTreeNode = GetNewTreeNodeInfo(true, 0, model.Title, model.CurrentUrl);
                GlobalInfo.FavoritesSetting.FavoritesInfos.Add(newTreeNode.Item1);
                item.Items.Add(newTreeNode.Item2);
            }
            else if (sender is MMenuItem)
            {
                if (_currentRightItem != null && _currentRightItem.Type == 1)
                {
                    var newTreeNode = GetNewTreeNodeInfo(false, 0, model.Title, model.CurrentUrl);
                    _currentRightItem.Items.Add(newTreeNode.Item2);
                    GlobalInfo.FavoritesSetting.FavoritesInfos.Add(newTreeNode.Item1);
                }
            }
            RefreshFavoritesBarEvent?.Invoke();
        }
        /// <summary>
        /// 添加文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFolder_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                var newTreeNode = GetNewTreeNodeInfo(true, 1, "新建文件夹", null);
                if (null == FavoritesTree || FavoritesTree.Items.Count <= 0) return;
                var treeNodeUc = FavoritesTree.Items[0];
                if (!(treeNodeUc is MTreeViewItem item)) return;
                item.Items.Add(newTreeNode.Item2);
                GlobalInfo.FavoritesSetting.FavoritesInfos.Add(newTreeNode.Item1);
            }
            else if (sender is MMenuItem)
            {
                var newTreeNode = GetNewTreeNodeInfo(false, 1, "新建文件夹", null);
                if (_currentRightItem != null && _currentRightItem.Type == 1)
                {
                    _currentRightItem.Items.Add(newTreeNode.Item2);
                    GlobalInfo.FavoritesSetting.FavoritesInfos.Add(newTreeNode.Item1);
                }
            }
            RefreshFavoritesBarEvent?.Invoke();
        }

        /// <summary>
        /// 删除当前节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (_currentRightItem?.Parent == null) return;

            foreach (var item in _currentRightItem.Items)
            {
                _currentRightItem.Items.Remove(item);
                if (!GlobalInfo.FavoritesSetting.FavoritesInfos.Exists(x => x.NodeId == _currentRightItem.NodeId))
                    continue;
                var currentNode = (GlobalInfo.FavoritesSetting.FavoritesInfos.FirstOrDefault(x => x.NodeId == _currentRightItem.NodeId));
                GlobalInfo.FavoritesSetting.FavoritesInfos.Remove(currentNode);
            }

            if (_currentRightItem.Parent is MTreeViewItem items)
            {
                if (GlobalInfo.FavoritesSetting.FavoritesInfos.Exists(x => x.NodeId == _currentRightItem.NodeId))
                {
                    var currentNode = (GlobalInfo.FavoritesSetting.FavoritesInfos.FirstOrDefault(x => x.NodeId == _currentRightItem.NodeId));
                    GlobalInfo.FavoritesSetting.FavoritesInfos.Remove(currentNode);
                }
                items.Items.Remove(_currentRightItem);
            }
            RefreshFavoritesBarEvent?.Invoke();
        }
        /// <summary>
        /// 创建新节点
        /// </summary>
        /// <param name="isRoot"></param>
        /// <param name="type"></param>
        /// <param name="nodeName"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private Tuple<TreeNode, MTreeViewItem> GetNewTreeNodeInfo(bool isRoot, int type, string nodeName, string url)
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
            double left = treeNode.Level * 10;
            var treeViewItem = GetNewFavoritesItem(treeNode, left);
            if (type != 0) return new Tuple<TreeNode, MTreeViewItem>(treeNode, treeViewItem);

            treeViewItem.Icon = "\ueb1e";
            treeViewItem.IsExpandedIcon = "\ueb1e";
            treeViewItem.IconForeground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            return new Tuple<TreeNode, MTreeViewItem>(treeNode, treeViewItem);
        }
        private void FavoritesTree_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (null == FavoritesTree.SelectedItem) return;
            if (!(FavoritesTree.SelectedItem is MTreeViewItem item)) return;
            if (item.Type == 1) return;
            if (item.IsEdit) return;
            if (!GlobalInfo.FavoritesSetting.FavoritesInfos.Exists(x => x.NodeId == item.NodeId)) return;
            var treeNode = GlobalInfo.FavoritesSetting.FavoritesInfos.First(x => x.NodeId == item.NodeId);
            OpenNewTabEvent?.Invoke(treeNode.Url);
        }

        private void ReName_OnClick(object sender, RoutedEventArgs e)
        {
            if (null == _currentRightItem) return;
            if (_currentRightItem.NodeId == 0) return;
            _currentRightItem.IsEdit = true;
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
                RefreshFavoritesBarEvent?.Invoke();
            }
        }
    }
}
