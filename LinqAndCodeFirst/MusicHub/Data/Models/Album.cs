using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    public class Album
    {
        public Album()
        {
            Songs = new HashSet<Song>();
        }

        [Key] 
        public int Id { get; set; }

        [MaxLength(ValidationConstants.AlbumNameMaxLength)]
        [Required]
        public string Name { get; set; } = null!;

        public DateTime ReleaseDate { get; set; }

        [NotMapped]
        public decimal Price
            => this.Songs.Count > 0 ? this.Songs.Sum(s => s.Price) : 0m;


        public int? ProducerId { get; set; }
        [ForeignKey(nameof(ProducerId))]
        public Producer? Producer { get; set; }

        public ICollection<Song> Songs { get; set; }

    }
}
