using System.ComponentModel.DataAnnotations;

namespace Defence22.Models
{
    //ulike properties til klassene
    public class Mission
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string Date { get; set; }
        public int Importance { get; set; }
    }
}