using System.Collections.Generic;

namespace CitiesInfo.Models
{
    public class CityDto
    {
        private int _id;
        private string _name;
        private string _description;

        public ICollection<PointOfInterestDto> pointofinterestdto = new List<PointOfInterestDto>();

        public int id { get { return _id; } set { _id = value; } }
        public string name { get { return _name; } set { _name = value; } }
        public string description { get { return _description; } set { _description = value; } }
        public int numberofpointsofinterest { get { return pointofinterestdto.Count; } }

    }
}
