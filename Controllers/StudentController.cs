using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OgrenciKarneLocal.Data;
using OgrenciKarneLocal.Models;

namespace OgrenciKarneLocal.Controllers
{
    /// <summary>
    /// Öğrenci işlemlerini yöneten controller
    /// Listeleme, kayıt, not girişi ve karne görüntüleme işlemlerini içerir
    /// </summary>
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================================
        // 1. ÖĞRENCI LİSTELEME - Index
        // ============================================

        /// <summary>
        /// Tüm öğrencileri listeler
        /// GET: /Student/Index
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Tüm öğrencileri veritabanından getir
            var students = await _context.Students.ToListAsync();
            return View(students);
        }

        // ============================================
        // 2. ÖĞRENCİ KAYIT - Create
        // ============================================

        /// <summary>
        /// Öğrenci kayıt formunu gösterir
        /// GET: /Student/Create
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Yeni öğrenciyi veritabanına kaydeder
        /// POST: /Student/Create
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ad,Soyad,Numara")] Student student)
        {
            // Model validasyonu kontrol et
            if (ModelState.IsValid)
            {
                // Varsayılan notlar 0 olarak ayarlanır (model'de tanımlı)
                _context.Add(student);
                await _context.SaveChangesAsync();
                
                // Başarılı kayıt sonrası listeye yönlendir
                TempData["SuccessMessage"] = "Öğrenci başarıyla kaydedildi.";
                return RedirectToAction(nameof(Index));
            }
            
            // Validasyon hatası varsa formu tekrar göster
            return View(student);
        }

        // ============================================
        // 3. NOT GİRİŞİ - Edit
        // ============================================

        /// <summary>
        /// Not giriş formunu gösterir
        /// GET: /Student/Edit/5
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Öğrenciyi id'ye göre bul
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        /// <summary>
        /// Matematik ve Türkçe notlarını kaydeder
        /// Notlar 0-100 arasında doğrulanır
        /// POST: /Student/Edit/5
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,Soyad,Numara,MatematikNotu,TurkceNotu")] Student student)
        {
            // ID kontrolü
            if (id != student.Id)
            {
                return NotFound();
            }

            // Not validasyonu - 0-100 arasında olmalı
            if (student.MatematikNotu < 0 || student.MatematikNotu > 100)
            {
                ModelState.AddModelError("MatematikNotu", "Matematik notu 0-100 arasında olmalıdır.");
            }

            if (student.TurkceNotu < 0 || student.TurkceNotu > 100)
            {
                ModelState.AddModelError("TurkceNotu", "Türkçe notu 0-100 arasında olmalıdır.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Öğrenci bilgilerini güncelle
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Notlar başarıyla kaydedildi.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(student);
        }

        // ============================================
        // 4. KARNE / RAPOR - Karne
        // ============================================

        /// <summary>
        /// Öğrencinin karnesini görüntüler
        /// Ortalama ve geçme durumu hesaplanır
        /// GET: /Student/Karne/5
        /// </summary>
        public async Task<IActionResult> Karne(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Öğrenciyi bul
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            // === ORTALAMA HESAPLAMA ===
            // Ortalama = (MatematikNotu + TurkceNotu) / 2
            double ortalama = (student.MatematikNotu + student.TurkceNotu) / 2.0;
            
            // === SONUÇ BELİRLEME ===
            // Ortalama >= 50 ise "Geçti", değilse "Kaldı"
            string sonuc = ortalama >= 50 ? "Geçti" : "Kaldı";
            bool gectiMi = ortalama >= 50;

            // View'a gönderilecek veriler
            ViewBag.Ortalama = ortalama;
            ViewBag.Sonuc = sonuc;
            ViewBag.GectiMi = gectiMi;

            return View(student);
        }

        // ============================================
        // YARDIMCI METODLAR
        // ============================================

        /// <summary>
        /// Öğrencinin veritabanında var olup olmadığını kontrol eder
        /// </summary>
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
