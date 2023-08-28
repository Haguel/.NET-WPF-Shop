using DOTNET_WPF_Shop.Utils;
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
            ViewUtils viewUtils = new ViewUtils();

            viewUtils.SetCenterAlignment(this);
            viewUtils.SetFontSize(this, ViewUtils.FontSizes.Default);
            viewUtils.SetSizes(this, properties.textBoxWidth, properties.textBoxHeight);
        }
    }

    public class AuthTitle : TextBlock
    {
        public AuthTitle()
        {
            ViewUtils viewUtils = new ViewUtils();

            viewUtils.SetCenterAlignment(this);
            viewUtils.SetFontSize(this, ViewUtils.FontSizes.Big);
        }
    }
}
