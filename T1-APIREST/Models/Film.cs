using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace T1_APIREST.Models
{
    public enum EFilmGenre { action, terror, comedy, romantic, scrifi}
    public class Film
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EFilmGenre FilmGenre { get; set; }
    }
}
