using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net_ef_videogame.Models
{
    public class Videogame
    {
        public long VideogameId { get; set; }
        [Column(TypeName = "VARCHAR(100)")]
        public string Name { get; set; }
        [Column(TypeName = "VARCHAR(2000)")]
        public string Overview { get; set; }
        public DateTime ReleaseDate { get; set; }


        public long SoftwareHouseId { get; set; }
        public SoftwareHouse SoftwareHouse { get; set; }

        public override string ToString()
        {
            return $"ID: {VideogameId} - {Name} - Release date: {ReleaseDate.ToString("dd/MM/yyyy")} - Software house id: {SoftwareHouseId}.";
        }
    }
}
