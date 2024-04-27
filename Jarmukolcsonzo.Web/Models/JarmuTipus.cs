using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jarmukolcsonzo.Web.Models;

[Table("jarmu_tipusok")]
public partial class JarmuTipus
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int id { get; set; }

    [StringLength(50)]
    public string megnevezes { get; set; } = null!;

    [Column(TypeName = "int(2)")]
    public int ferohely { get; set; }

    [InverseProperty("tipus")]
    public virtual ICollection<Jarmu> jarmuvek { get; set; } = new List<Jarmu>();
}
