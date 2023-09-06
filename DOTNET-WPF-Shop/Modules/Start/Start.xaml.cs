using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DOTNET_WPF_Shop.Modules.Start
{
    public partial class Start : Window
    {
        StartProvider provider = new StartProvider();

        public Start()
        {
            InitializeComponent();

            DataContext dc = new DataContext();
            ProductEntity p1 = new ProductEntity()
            {
                Id = Guid.NewGuid(),
                Title = "Apple",
                Price = 1.28,
                ImageSrc = "https://static.vecteezy.com/system/resources/thumbnails/023/290/773/small/fresh-red-apple-isolated-on-transparent-background-generative-ai-png.png",
                IsRemoved = false,
            };

            dc.Products.Add(p1);
            dc.SaveChanges();

        }

        private void Signup_Click(object sender, RoutedEventArgs e)
        {
            provider.RedirectToSignupPage(this);
        }

        private void Signin_Click(object sender, RoutedEventArgs e)
        {
            provider.RedirectToSigninPage(this);
        }
    }
}
