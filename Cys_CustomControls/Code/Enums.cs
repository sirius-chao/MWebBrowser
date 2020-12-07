// ReSharper disable once CheckNamespace
namespace Cys_Controls.Code
{
    /// <summary>
    /// 控件Level
    /// </summary>
    public enum StyleType
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default,
        /// <summary>
        /// 主要
        /// </summary>
        Primary,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 信息
        /// </summary>
        Info,
        /// <summary>
        /// 警告
        /// </summary>
        Warning,
        /// <summary>
        /// 危险
        /// </summary>
        Danger,
        /// <summary>
        /// 自定义
        /// </summary>
        Customize
    }

    public enum ResizeDirection
    {
        Left = 1,
        Right = 2,
        Top = 3,
        TopLeft = 4,
        TopRight = 5,
        Bottom = 6,
        BottomLeft = 7,
        BottomRight = 8,
    }

    public enum ZoomType
    {
        None,
        In,
        Out,
    }
}
