namespace LocMvc.Models
{
    public class CityModel
    {
        public int? CityID { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public int CountryId { get; set; }
        public string? CountryName { get; set; }


        public int StateID { get; set; }
        public string? StateName { get; set; }
    }

    public class StateList
    {
        public int StateID { get; set; }

        public string? StateName { get; set; }

    }
}
