using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace DOTNET_WPF_Shop.Common
{
    public class AcceptButton : Button
    {
        public AcceptButton()
        {
            GlobalProperties globalProperties = new GlobalProperties();
            Utils utils = new Utils();

            utils.SetCenterAlignment(this);
            utils.SetFontSize(this, Utils.FontSizes.Default);

            this.Background = utils.getSolidColorBrushFromHex(globalProperties.acceptColor);
        }
    }

    public class CancelButton : Button
    {
        public CancelButton()
        {
            GlobalProperties globalProperties = new GlobalProperties();
            Utils utils = new Utils();

            utils.SetFontSize(this, Utils.FontSizes.Small);

            this.Background = new SolidColorBrush(Colors.Transparent);
            this.BorderBrush = new SolidColorBrush(Colors.Transparent);
            this.Foreground = new SolidColorBrush(Colors.DarkGray);
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
            Utils utils = new Utils();

            utils.SetCenterAlignment(this);
        }
    }
}
