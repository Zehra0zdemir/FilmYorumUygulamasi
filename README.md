# 🎬 Film Yorum Uygulaması

Modern ve kullanıcı dostu Windows Forms uygulaması ile filmlerinizi yönetin, puanlayın ve yorumlayın.

![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2+-512BD4?style=flat-square&logo=.net)
![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=c-sharp)

## ✨ Özellikler

- 🎬 **Film Yönetimi** - Ekleme, düzenleme, silme
- 🖼️ **Poster Desteği** - Her film için görsel
- ⭐ **Puan Sistemi** - 1-10 arası puanlama
- 💬 **Yorum Sistemi** - Detaylı film yorumları
- 🔍 **Gelişmiş Arama** - Ad, tür, yıl filtreleme
- 📊 **Otomatik Rapor** - TXT formatında kayıt

## 🚀 Kurulum

### Gereksinimler
- Windows 10/11
- .NET Framework 4.7.2+
- Visual Studio 2019+ (geliştirme için)

### Kurulum Adımları
```bash
# Repository'yi klonlayın
git clone https://github.com/Zehra0zdemir/FilmYorumUygulamasi.git

# Visual Studio ile açın
start MovieReviewApp.sln

# NuGet paketlerini yükleyin (Package Manager Console)
Install-Package Newtonsoft.Json

# Derleyin ve çalıştırın (F5)
```
## 📖 Kullanım
### Film Ekleme

"Yeni Film Ekle" butonuna tıklayın
Film bilgilerini girin (Ad, Tür, Yıl, Poster)
"Tamam" ile kaydedin

### Puan & Yorum

Film posterine tıklayın
Puan verin (1-10) ve yorum yazın
"Ekle" butonlarına tıklayın

### Düzenleme/Silme

Film kartına sağ tıklayın
"Düzenle" veya "Sil" seçin

### Arama
Üst panelden film adı, tür veya yıla göre filtreleyin.

## 🏗️ Proje Yapısı

| Klasör | Açıklama | İçerik |
|--------|----------|--------|
| 📁 **Models/** | Domain Entities | `Movie.cs`, `Review.cs`, `Rating.cs`, `MovieGenre.cs` |
| 📁 **Services/** | Business Logic | `MovieService.cs`, `ReviewService.cs`, `PosterService.cs` |
| 📁 **Services/Interfaces/** | Service Contracts | `IMovieService.cs`, `IReviewService.cs`, `IPosterService.cs` |
| 📁 **Controls/** | Custom UI Components | `MoviePanel.cs` |
| 📁 **Forms/** | Application Windows | `MainForm.cs`, `AddMovieForm.cs`, `EditMovieForm.cs`, `ReviewDetailForm.cs` |
| 📁 **Utils/** | Helper Classes | `EnumHelper.cs` |

### Kullanılan Teknolojiler
- C# Windows Forms - Desktop UI
- Newtonsoft.Json - JSON serialization
- SOLID Principles - Clean architecture
- Repository Pattern - Data access

### 🎯 SOLID Principles
- Single Responsibility - Her sınıf tek sorumluluk:
public class MovieService { }
public class ReviewService { }

- Dependency Inversion - Interface'lere bağımlılık:
public MainForm(IMovieService movieService) { }

- Open/Closed - Genişlemeye açık:
public interface IMovieService { }
public class MovieService : IMovieService { }
