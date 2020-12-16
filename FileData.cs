namespace ConsoleApp50
{
    class FileData
    {
        public string WebSite { get; set; }

        public string IP { get; set; }

        public string TotalOrderCost { get; set; }

        public string ShippingCountry { get; set; }

        public string IPLocationName { get; set; }

        public FileData()
        {
        }

        public override string ToString()
        {
            return $"Website:{WebSite}| IP:{IP}|Total Order Cost:{TotalOrderCost}|Shipping Country:{ShippingCountry}|IP Location:{IPLocationName}";
        }
    }
}