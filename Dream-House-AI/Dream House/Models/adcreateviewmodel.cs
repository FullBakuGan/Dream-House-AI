using System.ComponentModel.DataAnnotations;
using Dream_House.Models;

namespace hackaton_asp_project.Models;

public class AdCreateViewModel
{
    [Required(ErrorMessage = "Тип недвижимости обязателен")]
    [Display(Name = "Тип недвижимости")]
    public int TypeId { get; set; }

    [Display(Name = "Район")]
    public int? DistrictId { get; set; }

    [Required(ErrorMessage = "Город обязателен")]
    [Display(Name = "Город")]
    public int CityId { get; set; }

    [Display(Name = "Городской район")]
    public int? CityDistrictId { get; set; }

    [Display(Name = "Микрорайон")]
    public int? MicrodistrictId { get; set; }

    [Display(Name = "Улица")]
    public int? StreetId { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Площадь должна быть положительным числом")]
    [Display(Name = "Площадь объекта (кв.м)")]
    public decimal? AreaObj { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Стоимость должна быть положительным числом")]
    [Display(Name = "Стоимость")]
    public decimal? Cost { get; set; }

    [Display(Name = "Описание")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Количество комнат обязательно")]
    [Range(1, int.MaxValue, ErrorMessage = "Количество комнат должно быть положительным числом")]
    [Display(Name = "Количество комнат")]
    public int CountOfRooms { get; set; }

    [Required(ErrorMessage = "Этаж обязателен")]
    [Range(0, int.MaxValue, ErrorMessage = "Этаж должен быть неотрицательным числом")]
    [Display(Name = "Этаж")]
    public int Stage { get; set; }

    [Display(Name = "Фотографии (URL, разделённые запятыми)")]
    public string? ImageUrls { get; set; }
}