using System.ComponentModel.DataAnnotations;


namespace CitiesInfo.Models
{
    public class PointOfInterestForUpdateDto
    {
        private string _name;
        private string _description;

        [Required(ErrorMessage = "You need to provide a name value.")]
        [MaxLength(50)]
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        [MaxLength(200, ErrorMessage = "Your description exceed 200 characters.")]
        public string description
        {
            get { return _description; }
            set { _description = value; }
        }
    }
}
