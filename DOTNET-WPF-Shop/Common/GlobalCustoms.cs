using DOTNET_WPF_Shop.Utils;
using System.Windows.Controls;
using System.Windows.Media;

namespace DOTNET_WPF_Shop.Common
{
    public class AcceptButton : Button
    {
        public AcceptButton()
        {
            GlobalProperties globalProperties = new GlobalProperties();
            ViewUtils viewUtils = new ViewUtils();

            viewUtils.SetCenterAlignment(this);
            viewUtils.SetFontSize(this, ViewUtils.FontSizes.Default);

            this.Background = viewUtils.getSolidColorBrushFromHex(globalProperties.acceptColor);
            this.Foreground = new SolidColorBrush(Colors.White);
        }
    }

    public class CancelButton : Button
    {
        public CancelButton()
        {
            ViewUtils viewUtils = new ViewUtils();

            viewUtils.SetFontSize(this, ViewUtils.FontSizes.Small);

            this.Background = new SolidColorBrush(Colors.Transparent);
            this.BorderBrush = new SolidColorBrush(Colors.Transparent);
            this.Foreground = new SolidColorBrush(Colors.DarkGray);
        }
    }

    public class NegativeButton : Button
    {
        public NegativeButton()
        {
            ViewUtils viewUtils = new ViewUtils();

            viewUtils.SetFontSize(this, ViewUtils.FontSizes.Default);

            this.Background = new SolidColorBrush(Colors.IndianRed);
            this.BorderBrush = new SolidColorBrush(Colors.IndianRed);
            this.Foreground = new SolidColorBrush(Colors.White);
            this.Padding = new System.Windows.Thickness(5, 0, 5, 0);
        }
    }

    public class HorizontalPanel : StackPanel
    {
        public HorizontalPanel()
        {
            this.Orientation = Orientation.Horizontal;
        }
    }

    public class HorizontalCenteredPanel : HorizontalPanel
    {
        public HorizontalCenteredPanel()
        {
            ViewUtils viewUtils = new ViewUtils();

            viewUtils.SetCenterAlignment(this);
        }
    }
}
