using DOTNET_WPF_Shop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DOTNET_WPF_Shop.Utils
{
    class ViewUtils
    {
        private GlobalProperties globalProperties = new GlobalProperties();
        public enum FontSizes
        {
            Small,
            Default,
            Big
        }

        public void SetCenterAlignment(FrameworkElement element)
        {
            element.HorizontalAlignment = HorizontalAlignment.Center;
            element.VerticalAlignment = VerticalAlignment.Center;
        }

        public void SetFontSize(FrameworkElement element, FontSizes fontSizes)
        {
            int currentFontSize = globalProperties.defaultFontSize;

            if (fontSizes == FontSizes.Small) currentFontSize = globalProperties.smallFontSize;
            else if (fontSizes == FontSizes.Default) currentFontSize = globalProperties.defaultFontSize;
            else if (fontSizes == FontSizes.Big) currentFontSize = globalProperties.bigFontSize;

            if (element is Control control) control.FontSize = currentFontSize;
            if (element is TextBlock textBlock) textBlock.FontSize = currentFontSize;
        }

        public void SetSizes(FrameworkElement element, int width, int height)
        {
            element.Width = width;
            element.Height = height;
        }

        public SolidColorBrush getSolidColorBrushFromHex(string hex)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
        }
    }
}
