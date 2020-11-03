using Cys_Controls.Code;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

// ReSharper disable once CheckNamespace
namespace Cys_CustomControls.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.Carousel"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.Carousel;assembly=Cys_CustomControls.Controls.Carousel"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:MCarouselV2/>
    ///
    /// </summary>

    public partial class MMultiItemCarousel : Control
    {
        static MMultiItemCarousel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MMultiItemCarousel), new FrameworkPropertyMetadata(typeof(MMultiItemCarousel)));
        }
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(5000);


        #region UI元素
        private Grid _partContainer;
        private Grid _partParent;
        private Canvas _partCanvasBoard;
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitControl();
            this.Loaded += MMultiItemCarousel_Loaded;
        }

        private void MMultiItemCarousel_Loaded(object sender, RoutedEventArgs e)
        {
            InitDisplayContent();
            Canvas.SetTop(_partContainer, 0);
        }

        private void InitControl()
        {
            _partContainer = GetTemplateChild("PART_Container") as Grid;
            _partParent = GetTemplateChild("PART_Parent") as Grid;
            _partCanvasBoard = GetTemplateChild("PART_CanvasBoard") as Canvas;
        }

        /// <summary>
        /// 自动滚动
        /// </summary>
        private bool _auto;

        /// <summary>
        /// 消息展示数量
        /// </summary>
        private int _displayCount = 4;

        private readonly List<MTextBlock> _courseMessage = new List<MTextBlock>();

        private void InitDisplayContent()
        {
            if (_items == null)
                return;
            var initCount = _items.Count <= _displayCount ? _displayCount : _items.Count;
            if (_partContainer.RowDefinitions.Count > 0)
                return;
            for (var i = 0; i < _displayCount + 1; i++)
            {
                _partContainer.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) { } });
            }

            var itemHeight = _partCanvasBoard.ActualHeight / (_displayCount);
            _rollHeight = itemHeight;
            _partContainer.Height = itemHeight * (_displayCount + 1);

            for (var i = 0; i < initCount; i++)
            {
                var textBlock = new MTextBlock { Text = Items[i], VerticalAlignment = VerticalAlignment.Center, StyleType = StyleType.Success, Height = 35, Width = 120,CornerRadius = new CornerRadius(10)};
                Grid.SetRow(textBlock, i);
                _courseMessage.Add(textBlock);
                _partContainer.Children.Add(textBlock);
            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Begin();
        }

        private List<string> _items;
        /// <summary>
        /// 轮播子项
        /// </summary>
        public List<string> Items
        {
            set
            {
                //此处应避免重复赋值待优化
                _items = value ?? new List<string>();
                InitData();
            }
            get => _items;
        }

        /// <summary>
        /// 滚动高度
        /// </summary>
        private double _rollHeight = 32;

        /// <summary>
        ///  滚动动画板(2D)
        /// </summary>
        private Storyboard _storyboardR2L;

        /// <summary>
        /// 根据图片列表初始数据
        /// </summary>
        private void InitData()
        {
            //当图片数量小于等于1时不滚动
            if (_items.Count <= _displayCount)
                return;
            ResetMessage();
            _timer.Elapsed -= Timer_Elapsed;
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }
        private void CreateStoryboard()
        {
            _storyboardR2L = new Storyboard();
            InitAnimation(-_rollHeight, _partContainer);
            _storyboardR2L.FillBehavior = FillBehavior.Stop;
            _storyboardR2L.Completed -= StoryboardCompleted;
            _storyboardR2L.Completed += StoryboardCompleted;
        }

        /// <summary>
        /// 初始化关键帧动画
        /// </summary>
        /// <param name="rollHeight"></param>
        /// <param name="image"></param>
        private void InitAnimation(double rollHeight, Grid image)
        {
            var keyFrames = new DoubleAnimationUsingKeyFrames { Name = "Auto" };
            var waitKeyFrame = new LinearDoubleKeyFrame(rollHeight, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2)));//停留等待
            var endKeyFrame = new LinearDoubleKeyFrame(rollHeight, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3)));
            keyFrames.KeyFrames.Add(waitKeyFrame);
            keyFrames.KeyFrames.Add(endKeyFrame);
            _storyboardR2L.Children.Add(keyFrames);
            Storyboard.SetTarget(keyFrames, image);
            Storyboard.SetTargetProperty(keyFrames, new PropertyPath("(Canvas.Top)"));
        }

        private void StoryboardCompleted(object sender, EventArgs e)
        {
            try
            {
                ResetStory();
                _storyboardR2L.Begin();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 开始滚动动画
        /// </summary>
        private void Begin()
        {
            if (_auto) return;

            if (Items.Count <= 1)
            {
                return;
            }
            Dispatcher.Invoke(() =>
            {
                _auto = true;
                CreateStoryboard();
                _storyboardR2L.Begin();
            });
        }
        private void ResetStory()
        {
            _partContainer.SetValue(Canvas.TopProperty, 0.0);
            var message = Items[0];
            Items.RemoveAt(0);
            Items.Add(message);
            ResetMessage();
        }

        private void ResetMessage()
        {
            for (var i = 0; i < _courseMessage.Count; i++)
            {
                _courseMessage[i].Text = Items[i];
            }
        }
    }
}
