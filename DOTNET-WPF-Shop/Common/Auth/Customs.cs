using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace DOTNET_WPF_Shop.Common.Auth
{
    public class AuthTextBox : TextBox
    {
        public AuthTextBox()
        {
            Properties properties = new Properties();
            Utils utils = new Utils();

            utils.SetCenterAlignment(this);
            utils.SetFontSize(this, Utils.FontSizes.Default);
            utils.SetSizes(this, properties.textBoxWidth, properties.textBoxHeight);
        }
    }

    public class AuthTitle : TextBlock
    {
        public AuthTitle()
        {
            Utils utils = new Utils();

            utils.SetCenterAlignment(this);
            utils.SetFontSize(this, Utils.FontSizes.Big);
        }
    }
}
