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
        public long VideogameId { get; private set; }
        [Column(TypeName = "VARCHAR(100)")]
        public string Name { get; private set; }
        [Column(TypeName = "VARCHAR(2000)")]
        public string Overview { get; private set; }
        public DateTime ReleaseDate { get; private set; }


        public long SoftwareHouseId { get; private set; }
        public SoftwareHouse SoftwareHouse { get; private set; }

        public override string ToString()
        {
            return $"ID: {VideogameId}, {Name} - released on {ReleaseDate.ToString("dd/MM/yyyy")} by SoftwareHouse with ID: {SoftwareHouseId}.";
        }
    }
}
