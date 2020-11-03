using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
    ///     <MyNamespace:Carousel/>
    ///
    /// </summary>
    public class MCarousel : Control
    {
        static MCarousel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MCarousel), new FrameworkPropertyMetadata(typeof(MCarousel)));
        }

        readonly List<Ellipse> _ellipseLists = new List<Ellipse>();
        private readonly SolidColorBrush _activeColor = new SolidColorBrush(Color.FromArgb(255, 6, 172, 237));
        private readonly SolidColorBrush _defaultColor = new SolidColorBrush(Color.FromArgb(255, 216, 224, 224));
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(5000);


        /// <summary>
        /// 自动滚动
        /// </summary>
        private bool _auto;

        /// <summary>
        /// 滚动索引
        /// </summary>
        private int _currentIndex;

        #region UI元素
        private Image _partLeftImage;
        private Image _partRightImage;
        private Grid _partBtnGrid;
        private Grid _partParent;
        private Canvas _partCanvasBoard;
        private Button _partBtnPre;
        private Button _partBtnNext;
        private StackPanel _partEllipses;
        #endregion


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitControl();
        }

        private void InitControl()
        {
            _partLeftImage = GetTemplateChild("PART_LeftImage") as Image;
            _partRightImage = GetTemplateChild("PART_RightImage") as Image;
            _partBtnGrid = GetTemplateChild("PART_BtnGrid") as Grid;
            _partParent = GetTemplateChild("PART_Parent") as Grid;
            _partCanvasBoard = GetTemplateChild("PART_CanvasBoard") as Canvas;
            _partBtnPre = GetTemplateChild("PART_BtnPre") as Button;
            _partBtnNext = GetTemplateChild("PART_BtnNext") as Button;
            _partEllipses = GetTemplateChild("PART_Ellipses") as StackPanel;
            _partLeftImage.MouseUp += LeftImage_OnMouseUp;
            _partBtnPre.Click += Pre_Click;
            _partBtnNext.Click += Next_Click;
            _partParent.MouseEnter += Parent_MouseEnter;
            _partParent.MouseLeave += Parent_MouseLeave;
        }


        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Begin();
        }

        #region ==ImagesLists 图片列表==
        /// <summary>
        /// CornerRadius
        /// </summary>
        public static readonly DependencyProperty ImagesListsProperty =
            DependencyProperty.Register("ImagesLists", typeof(List<BitmapImage>), typeof(MCarousel));
        public List<BitmapImage> ImagesLists
        {
            get => (List<BitmapImage>)GetValue(ImagesListsProperty); 
            set => SetValue(ImagesListsProperty, value);
        }

        #endregion ==ImagesLists==

        /// <summary>
        /// 滚动宽度
        /// </summary>
        private double RollWidth => _partCanvasBoard.Width;

        /// <summary>
        ///  滚动动画板(2D)
        /// </summary>
        private Storyboard _storyboardR2L;

        /// <summary>
        /// 根据图片列表初始数据
        /// </summary>
        public void InitData()
        {
            for (var i = 0; i < ImagesLists?.Count; i++)
            {
                var ellipse = new Ellipse
                {
                    Width = 10,
                    Height = 10,
                    Fill = i == 0 ? _activeColor : _defaultColor,
                    Margin = new Thickness(5, 0, 5, 0)
                };
                _ellipseLists.Add(ellipse);
                _partEllipses.Children.Add((ellipse));
            }

            //当图片数量小于等于1时不滚动
            if (ImagesLists.Count <= 1)
                return;
            ResetStory();
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }
        private void CreateStoryboard(RollStatus rollStatus = RollStatus.Auto)
        {
            _storyboardR2L = new Storyboard();
            if (rollStatus != RollStatus.Back)
            {
                InitAnimation(0.0, -RollWidth, _partLeftImage, rollStatus);
                InitAnimation(RollWidth, 0.0, _partRightImage, rollStatus);
            }
            else
            {
                InitAnimation(-RollWidth, 0, _partLeftImage, rollStatus);
                InitAnimation(0, RollWidth, _partRightImage, rollStatus);
            }
            _storyboardR2L.FillBehavior = FillBehavior.Stop;
            _storyboardR2L.Completed -= StoryboardCompleted;
            _storyboardR2L.Completed += StoryboardCompleted;
        }

        /// <summary>
        /// 初始化关键帧动画
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="image"></param>
        /// <param name="rollStatus"></param>
        private void InitAnimation(double startPoint, double endPoint, Image image, RollStatus rollStatus)
        {
            var keyFrames = new DoubleAnimationUsingKeyFrames { Name = rollStatus.ToString() };
            var startKeyFrame = new LinearDoubleKeyFrame(startPoint, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2)));
            var endKeyFrame = new LinearDoubleKeyFrame(endPoint, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(4)));
            keyFrames.KeyFrames.Add(startKeyFrame);
            keyFrames.KeyFrames.Add(endKeyFrame);
            _storyboardR2L.Children.Add(keyFrames);
            Storyboard.SetTarget(keyFrames, image);
            Storyboard.SetTargetProperty(keyFrames, new PropertyPath("(Canvas.Left)"));
        }

        private void StoryboardCompleted(object sender, EventArgs e)
        {
            ClockGroup cg = (ClockGroup)sender;
            var name = cg.Children[0].Timeline.Name;
            if (name == RollStatus.Auto.ToString())
            {
                ResetStory();
                _storyboardR2L.Begin();
            }
            else if (name == RollStatus.Back.ToString())
            {
                _partBtnPre.IsEnabled = true;
                ResetStory(RollStatus.Back);
            }
            else if (name == RollStatus.Next.ToString())
            {
                _partBtnNext.IsEnabled = true;
                ResetStory(RollStatus.Next);
            }
        }

        /// <summary>
        /// 开始滚动动画
        /// </summary>
        private void Begin()
        {
            if (_auto) return;
            Dispatcher.Invoke(() =>
            {
                if (ImagesLists.Count <= 1)
                {
                    _partBtnGrid.Visibility = Visibility.Collapsed;
                    _partEllipses.Visibility = Visibility.Collapsed;
                    return;
                }
            });
           
            Dispatcher.Invoke(() =>
            {
                _auto = true;
                CreateStoryboard();
                _storyboardR2L.Begin();
                //开始动画
            });
        }

        /// <summary>
        /// 初始化动画版，显示动画中的图片
        /// </summary>
        private void ResetStory(RollStatus rollStatus = RollStatus.Auto)
        {
            PointMove();
            if (rollStatus != RollStatus.Back)
            {
                _partLeftImage.SetValue(Canvas.LeftProperty, 0.0);
                _partRightImage.SetValue(Canvas.LeftProperty, RollWidth);
                _partLeftImage.Source = ImagesLists[_currentIndex];
                if (_currentIndex == ImagesLists.Count - 1)
                    _currentIndex = 0;
                else
                    _currentIndex++;
                _partRightImage.Source = ImagesLists[_currentIndex];
            }
            else
            {
                _partLeftImage.SetValue(Canvas.LeftProperty, -RollWidth);
                _partRightImage.SetValue(Canvas.LeftProperty, 0.0);

                _partRightImage.Source = ImagesLists[_currentIndex];
                if (_currentIndex == 0)
                    _currentIndex = ImagesLists.Count - 1;
                else
                    _currentIndex--;
                _partLeftImage.Source = ImagesLists[_currentIndex];
            }
        }
        private void Pre_Click(object sender, RoutedEventArgs e)
        {
            _partBtnPre.IsEnabled = false;
            _auto = false;
            try
            {
                _storyboardR2L?.Stop();
                CreateStoryboard(RollStatus.Back);
                _storyboardR2L.SpeedRatio = 10;
                _storyboardR2L.Begin();
            }
            catch (Exception)
            {
                _partBtnPre.IsEnabled = true;
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            _partBtnNext.IsEnabled = false;
            _auto = false;
            try
            {
                _storyboardR2L?.Stop();
                CreateStoryboard(RollStatus.Next);
                _storyboardR2L.SpeedRatio = 10;
                _storyboardR2L.Begin();
            }
            catch (Exception)
            {
                _partBtnNext.IsEnabled = true;
            }
        }

        private void PointMove()
        {
            _ellipseLists.ForEach(x => x.Fill = _defaultColor);
            _ellipseLists[_currentIndex].Fill = _activeColor;
        }

        private void LeftImage_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("莫点哥!!!!!!!!!");
        }

        private void Parent_MouseEnter(object sender, MouseEventArgs e)
        {
            _partBtnGrid.Visibility = Visibility.Visible;
        }

        private void Parent_MouseLeave(object sender, MouseEventArgs e)
        {
            _partBtnGrid.Visibility = Visibility.Collapsed;
        }
    }

    public enum RollStatus
    {
        Auto,
        Back,
        Next
    }
}
