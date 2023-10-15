using DOTNET_WPF_Shop.Utils;
using System.Windows;
using System.Windows.Controls;

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

    public class AuthDoneButton : AcceptButton
    {
        public AuthDoneButton()
        {
            Properties properties = new Properties();
            ViewUtils viewUtils = new ViewUtils();

            viewUtils.SetSizes(this, properties.buttonWidth, properties.buttonHeight);
            this.Margin = new Thickness(0, 0, 20, 0);
        }
    }
}
