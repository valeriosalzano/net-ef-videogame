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
    [Index(nameof(TaxId), IsUnique = true)]
    public class SoftwareHouse
    {
        public long SoftwareHouseId { get; private set; }
        [Column(TypeName = "VARCHAR(100)")]
        public string Name { get; private set; }
        [Column(TypeName = "VARCHAR(50)")]
        public string TaxId { get; private set; }
        [Column(TypeName = "VARCHAR(50)")]
        public string City { get; private set; }
        [Column(TypeName = "VARCHAR(50)")]
        public string Country { get; private set; }


        public List<Videogame> Videogames { get; private set; }

        public override string ToString()
        {
            return $"ID {SoftwareHouseId} - {Name} - VAT {TaxId} - registered address {City}, {Country}.";
        }
    }
}
