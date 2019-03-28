using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitiesInfo.Entities
{
    public class PointOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [MaxLength(50)]
        public string name { get; set; }

        [MaxLength(200)]
        public string description { get; set; }

        [ForeignKey("cityId")]
        public City city { get; set; }

        public int cityId { get; set; }



    }
}
