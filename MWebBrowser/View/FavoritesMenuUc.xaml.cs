﻿using Cys_Common;
using Cys_CustomControls.Controls;
using Cys_Model;
using MWebBrowser.Code.Helpers;
using MWebBrowser.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public Func<WebTabControlViewModel> GetWebUrlEvent;

        /// <summary>
        /// 记录当前右键选中的Item;
        /// </summary>
        private MTreeViewItem _currentRightItem;
        public FavoritesMenuUc()
        {
            InitializeComponent();
            this.Loaded += FavoritesMenuUc_Loaded;
        }

        private void FavoritesMenuUc_Loaded(object sender, RoutedEventArgs e)
        {
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
                    treeViewItem.Type = treeNode.Type;
                    treeViewItem.NodeId = treeNode.NodeId;
                }
                else
                {
                    treeViewItem.Header = treeNode.NodeName;
                    treeViewItem.Type = treeNode.Type;
                    treeViewItem.NodeId = treeNode.NodeId;
                }
            }
            else
            {
                treeViewItem.Header = treeNode.NodeName;
                treeViewItem.Type = treeNode.Type;
                treeViewItem.NodeId = treeNode.NodeId;
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
        /// 添加到当前网页到根目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFavorites_OnClick(object sender, RoutedEventArgs e)
        {
            var model = GetWebUrlEvent?.Invoke();
            if (null == model) return;

            var newTreeNode = GetNewTreeNodeInfo(true, 0, model.Title, model.CurrentUrl);
            if (!(FavoritesTree.Items[0] is MTreeViewItem item)) return;

            if (IsExistNode(item, model.CurrentUrl)) return;

            GlobalInfo.FavoritesSetting.FavoritesInfos.Add(newTreeNode.Item1);
            item.Items.Add(newTreeNode.Item2);
        }

        private bool IsExistNode(MTreeViewItem treeViewItem, string url)
        {
            foreach (var child in treeViewItem.Items)
            {
                if (child is MTreeViewItem item && item.Header.ToString() == url)
                {
                    return true;
                }
            }
            return false;
        }

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
        }

        private void AddFolder_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                var newTreeNode = GetNewTreeNodeInfo(true,1, "新建文件夹", null);
                if (null == FavoritesTree || FavoritesTree.Items.Count <= 0) return;
                var treeNodeUc = FavoritesTree.Items[0];
                if (!(treeNodeUc is MTreeViewItem item)) return;
                item.Items.Add(newTreeNode.Item2);
                GlobalInfo.FavoritesSetting.FavoritesInfos.Add(newTreeNode.Item1);
            }
            else if(sender is MMenuItem)
            {
                var newTreeNode = GetNewTreeNodeInfo(false,1, "新建文件夹", null);
                if (_currentRightItem != null&&_currentRightItem.Type==1)
                {
                    _currentRightItem.Items.Add(newTreeNode.Item2);
                    GlobalInfo.FavoritesSetting.FavoritesInfos.Add(newTreeNode.Item1);
                }
            }
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
            if (!isRoot)
            {
                parentId = _currentRightItem.NodeId;
            }
            int nodeMax = GlobalInfo.FavoritesSetting.FavoritesInfos.Max(x => x.NodeId);
            var treeNode = new TreeNode
            {
                Url = url,
                ParentId = parentId,
                NodeId = nodeMax + 1,
                NodeName = nodeName,
                Type = type,
            };
            var treeViewItem = new MTreeViewItem
            {
                Header = treeNode.NodeName,
                Type = type,
                NodeId = treeNode.NodeId,
            };

            if (type == 0)
            {
                treeViewItem.Icon = "\ueb1e";
                treeViewItem.IsExpandedIcon = "\ueb1e";
                treeViewItem.IconForeground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            return new Tuple<TreeNode, MTreeViewItem>(treeNode, treeViewItem);
        }
    }
}
