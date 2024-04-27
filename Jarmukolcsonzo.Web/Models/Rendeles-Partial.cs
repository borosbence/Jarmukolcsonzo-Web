using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jarmukolcsonzo.Web.Models;

[ModelMetadataType(typeof(RendelesMeta))]
public partial class Rendeles
{
    [NotMapped]
    public bool Arkalkulacio { get; set; }
}

public class RendelesMeta
{
    [DisplayName("Dátum")]
    [DataType(DataType.Date)]
    public DateTime datum { get; set; }

    [DisplayName("Napok száma")]
    public int napok_szama { get; set; }

    [DisplayName("Ár")]
    [DataType(DataType.Currency)]
    public decimal ar { get; set; }

    [DisplayName("Jármű")]
    public int jarmu_id { get; set; }
    [DisplayName("Jármű")]
    public Jarmu? jarmu { get; set; }

    [DisplayName("Ügyfél")]
    public int ugyfel_id { get; set; }
    [DisplayName("Ügyfél")]
    public Ugyfel? ugyfel { get; set; }
}

