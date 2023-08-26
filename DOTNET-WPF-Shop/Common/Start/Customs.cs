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
            Utils utils = new Utils();

            utils.SetCenterAlignment(this);
            utils.SetFontSize(this, Utils.FontSizes.Default);
            utils.SetSizes(this, properties.buttonWidth, properties.buttonHeight);
        }
    }
}
