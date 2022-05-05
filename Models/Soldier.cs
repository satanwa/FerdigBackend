using System;
using System.ComponentModel.DataAnnotations;

namespace Defence22.Models
{
    //ulike properties til klassene
    public class Soldier
    {
        [Key]
        public int Id { get; set; }
        public string WholeName { get; set; }
        public string Rank { get; set; }
        public string ImagePath { get; set; } //denne er vi dessverre usikker på om er riktig

    }
}
