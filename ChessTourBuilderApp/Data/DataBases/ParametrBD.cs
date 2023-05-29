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