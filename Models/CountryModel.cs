using System.ComponentModel.DataAnnotations;

namespace LocMvc.Models
{
    public class CountryModel
    {
        
        public int? countryId { get; set; }

        public string countryName { get; set; }

        public string countryCode { get; set; }

        public string? createdDate { get; set; }

        public string? modifiededDate { get; set; }

        public int? StateCount { get; set; }

    }
}
