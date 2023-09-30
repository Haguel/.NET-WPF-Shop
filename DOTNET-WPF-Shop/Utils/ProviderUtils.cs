﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DOTNET_WPF_Shop.Utils
{
    public class ProviderUtils
    {
        public bool ValidateDto(object obj)
        {
            var validationContext = new ValidationContext(obj, null, null);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    MessageBox.Show(validationResult.ErrorMessage);
                }

                return false;
            }

            return true;
        }

        public void HandleTextBoxUnfocus(TextBox textBox)
        {
            if (textBox.Text == "") textBox.Text = textBox.Tag as string;
        }

        public void HandleTextBoxFocus(TextBox textBox)
        {
            if (textBox.Text == textBox.Tag as string) textBox.Text = "";
        }
    }
}
