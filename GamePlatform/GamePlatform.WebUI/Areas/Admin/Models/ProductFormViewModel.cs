using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GamePlatform.WebUI.Areas.Admin.Models
{
    public class ProductFormViewModel
    {
        public int Id { get; set; }

        [Display(Name = "İsim")]
        [Required(ErrorMessage = "Oyun ismi girmek zorunludur.")]
        public string Name { get; set; }


        [Display(Name = "Açıklama")]
        public string? Description { get; set; }


        [Display(Name = "Oyun Fiyatı")]
        public decimal? UnitPrice { get; set; }

        [Display(Name = "Oyunun Popülerliği (en az 1-10 en fazla)")]
        public int? Popularity { get; set; }

        [Display(Name = "Saklama Alanı (GB)")]
        public string Storage { get; set; }

        [Display(Name = "Kategori")]
        [Required(ErrorMessage = "Bir kategori seçmek zorunludur.")]
        public int CategoryId { get; set; }

        [Display(Name = "Ürün Görseli")]
        public IFormFile? File { get; set; }
    }
}
