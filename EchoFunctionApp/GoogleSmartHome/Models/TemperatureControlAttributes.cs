namespace GoogleSmartHome.Models
{
    public class TemperatureControlAttributes
    {
        public TemperatureRange TemperatureRange { get; set; }
        public decimal TemperatureStepCelsius { get; set; }
        public string TemperatureUnitForUx => "C";
        public bool CommandOnlyTemperatureControl { get; set; }
    }
}