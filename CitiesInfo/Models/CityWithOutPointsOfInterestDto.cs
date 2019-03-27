using System;
namespace CitiesInfo.Models
{
    public class CityWithOutPointsOfInterestDto
    {
        private int _id;
        private string _name;
        private string _description;

        public int id { get { return _id; } set { _id = value; } }
        public string name { get { return _name; } set { _name = value; } }
        public string description { get { return _description; } set { _description = value; } }
    }
}
