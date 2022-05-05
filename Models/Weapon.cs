using System.ComponentModel.DataAnnotations;

namespace Defence22.Models
{
    //ulike properties til klassene
    public class Weapon
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int Stock { get; set; }
        public int BulletsPS { get; set; }
    }
}
