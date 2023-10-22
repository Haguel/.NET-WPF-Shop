using System;
using System.Text.Json;
using System.Windows;

namespace DOTNET_WPF_Shop.Config
{
    class Config
    {
        private static string jsonFilename = "Config/config.json";
        private static JsonElement? config = null;

        /** 
        If there are subproprties in property use ":" as separator. For example: 
        -- "parentPropert:childPropert" 
        equals 
        -- {
        --    parentPropert: {
        --        childPropert: ...
        --    }
        -- }
        **/
        public static string GetJsonData(string property)
        {
            if (config == null)
            {
                if (!System.IO.File.Exists(jsonFilename))
                {
                    MessageBox.Show("Wrong configuration file path");

                    return null;
                }

                try
                {
                    config = JsonSerializer.Deserialize<dynamic>
                        (System.IO.File.ReadAllText(jsonFilename));
                }
                catch
                {
                    MessageBox.Show("Invalid configuration file");
                }
            }

            JsonElement? jsonProperty = config;

            try
            {
                foreach (string key in property.Split(':'))
                {
                    jsonProperty = jsonProperty?.GetProperty(key);
                }
            }
            catch
            {
                throw new Exception("Invalid property: " + property);
            }

            return jsonProperty?.GetString();
        }
    }
}
