using System.ComponentModel.DataAnnotations;

namespace OgrenciKarneLocal.Models
{
    /// <summary>
    /// Öğrenci bilgilerini ve notlarını tutan model sınıfı
    /// </summary>
    public class Student
    {
        // Primary Key
        public int Id { get; set; }

        // Öğrencinin adı - Zorunlu alan
        [Required(ErrorMessage = "Ad alanı zorunludur")]
        [Display(Name = "Ad")]
        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir")]
        public string Ad { get; set; } = string.Empty;

        // Öğrencinin soyadı - Zorunlu alan
        [Required(ErrorMessage = "Soyad alanı zorunludur")]
        [Display(Name = "Soyad")]
        [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir")]
        public string Soyad { get; set; } = string.Empty;

        // Öğrenci numarası - Zorunlu alan
        [Required(ErrorMessage = "Numara alanı zorunludur")]
        [Display(Name = "Öğrenci Numarası")]
        [StringLength(20, ErrorMessage = "Numara en fazla 20 karakter olabilir")]
        public string Numara { get; set; } = string.Empty;

        // Matematik notu - 0-100 arası değer
        [Display(Name = "Matematik Notu")]
        [Range(0, 100, ErrorMessage = "Matematik notu 0-100 arasında olmalıdır")]
        public int MatematikNotu { get; set; } = 0;

        // Türkçe notu - 0-100 arası değer
        [Display(Name = "Türkçe Notu")]
        [Range(0, 100, ErrorMessage = "Türkçe notu 0-100 arasında olmalıdır")]
        public int TurkceNotu { get; set; } = 0;

        // Hesaplanan özellik: Ad ve Soyad birleşimi
        [Display(Name = "Ad Soyad")]
        public string AdSoyad => $"{Ad} {Soyad}";
    }
}
