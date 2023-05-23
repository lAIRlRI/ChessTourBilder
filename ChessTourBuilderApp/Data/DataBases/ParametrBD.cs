using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.DataBases
{
    internal class ParametrBD
    {
        public string ParameterName { get; set; }

        public object? ParameterValue { get; set; }

        public ParametrBD(string parameterName, object? parameterValue)
        {
            ParameterName = parameterName;
            ParameterValue = parameterValue;
        }

        public ParametrBD()
        {

        }
    }
}