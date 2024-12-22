namespace LocMvc.Models
{
    public class StateModel
    {
        public int? StateId { get; set; }

        public string StateName { get; set; }

        public int CountryID { get; set; }
        public string? CountryName { get; set; }

        public string StateCode { get; set; }

        public int? CityCount { get; set; }
    }

    public class CountryList
    {
        public int CountryId { get; set; }

        public string? CountryName { get; set; }

    }


}
