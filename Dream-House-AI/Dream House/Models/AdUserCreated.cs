using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dream_House.Models;
using Microsoft.AspNetCore.Identity;

namespace hackaton_asp_project.Models;

[Table("ad_user_created")]
public partial class AdUserCreated
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("type_id")]
    public int TypeId { get; set; }

    [Column("district_id")]
    public int? DistrictId { get; set; }

    [Required]
    [Column("city_id")]
    public int CityId { get; set; }

    [Column("city_district_id")]
    public int? CityDistrictId { get; set; }

    [Column("microdistrict_id")]
    public int? MicrodistrictId { get; set; }

    [Column("street_id")]
    public int? StreetId { get; set; }

    [Column("area_obj")]
    public decimal? AreaObj { get; set; }

    [Column("cost")]
    public decimal? Cost { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Required]
    [Column("count_of_rooms")]
    public int CountOfRooms { get; set; }

    [Required]
    [Column("stage")]
    public int Stage { get; set; }

    [Column("image_urls")]
    public string? ImageUrls { get; set; }

    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    public virtual city City { get; set; } = null!;
    public virtual city_district? CityDistrict { get; set; }
    public virtual district? District { get; set; }
    public virtual microdistrict? Microdistrict { get; set; }
    public virtual street? Street { get; set; }
    public virtual type_obj Type { get; set; } = null!;
    public virtual User Users { get; set; } = null!;
}