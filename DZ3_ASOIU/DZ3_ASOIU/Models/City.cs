using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DZ3_ASOIU.Models;
public class City
{
    private int _populationK;
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public int CountryId { get; set; }
    [ForeignKey(nameof(CountryId))]
    public Country? Country { get; set; }
    [Required]
    public string Name { get; set; } = "";
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Население не может быть отрицательным")]
    public int PopulationK
    {
        get => _populationK;
        set
        {
            if (value < 0)
                throw new ArgumentException("Население не может быть отрицательным");
            _populationK = value;
        }
    }
}
