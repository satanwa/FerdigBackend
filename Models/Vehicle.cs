using System.ComponentModel.DataAnnotations;

namespace Defence22.Models
{

    //ulike properties til klassene
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public int AmountPeople { get; set; }
        public int Weight { get; set; }
        public bool Maintenance { get; set; }
    }
}
