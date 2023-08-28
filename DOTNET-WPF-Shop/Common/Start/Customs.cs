using DOTNET_WPF_Shop.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DOTNET_WPF_Shop.Common.Start
{
    public class StartButton : Button
    {
        public StartButton()
        {
            Properties properties = new Properties();
            ViewUtils viewUtils = new ViewUtils();

            viewUtils.SetCenterAlignment(this);
            viewUtils.SetFontSize(this, ViewUtils.FontSizes.Default);
            viewUtils.SetSizes(this, properties.buttonWidth, properties.buttonHeight);
        }
    }
}
