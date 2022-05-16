using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplaterView
{
    public class GlobalConfig
    {
        private static GlobalConfig _instnstance;

        public string ConnectionString
        {
            get;
            private set;
        }

        private GlobalConfig()
        {
            ConfigureConfig();
        }

        public static GlobalConfig Instnstance
        {
            get 
            {
                if (_instnstance == null)
                    _instnstance = new GlobalConfig();
                return _instnstance;
            } 
        }

        private void ConfigureConfig()
        {
            string connection = ConfigurationManager.ConnectionStrings["connection_string"].ConnectionString;

            ConnectionString = connection;
        }
    }
}
