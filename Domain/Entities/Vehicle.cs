using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalsApi.Domain.Entities
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = default;

        [Required]
        [StringLength(150)]
        public string Nome { get; set; } = default;

        [Required]
        [StringLength(100)]
        public string Marca { get; set; } = default;

        [Required]
        [StringLength(4)]
        public int Ano { get; set; } = default;
    }
}