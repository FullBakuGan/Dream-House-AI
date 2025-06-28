using Microsoft.EntityFrameworkCore;
using hackaton_asp_project.Models;
using Dream_House.Models;

namespace Dream_House.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ad> ads { get; set; }
        public DbSet<ad_parametre> ad_parametres { get; set; }
        public DbSet<city> cities { get; set; }
        public DbSet<city_district> city_districts { get; set; }
        public DbSet<district> districts { get; set; }
        public DbSet<favorite> favorites { get; set; }
        public DbSet<image_ad> image_ads { get; set; }
        public DbSet<microdistrict> microdistricts { get; set; }
        public DbSet<parsing_log> parsing_logs { get; set; }
        public DbSet<price_history> price_histories { get; set; }
        public DbSet<source> sources { get; set; }
        public DbSet<status> statuses { get; set; }
        public DbSet<street> streets { get; set; }
        public DbSet<type_obj> type_objs { get; set; }
        public DbSet<AdUserCreated> AdUserCreateds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация для ad (без изменений)
            modelBuilder.Entity<ad>(entity =>
            {
                entity.HasKey(e => e.id).HasName("ad_pkey");
                entity.ToTable("ad", tb => tb.HasComment("Хранит информацию об объявлениях о квартирах"));
                entity.HasIndex(e => e.city_id, "idx_ad_city_id");
                entity.HasIndex(e => e.source_id, "idx_ad_source_id");
                entity.HasIndex(e => e.status_id, "idx_ad_status_id");
                entity.HasIndex(e => e.type_id, "idx_ad_type_id");
                entity.HasIndex(e => new { e.source_id, e.ad_num }, "unique_source_ad_num").IsUnique();
                entity.Property(e => e.id).HasComment("Уникальный идентификатор объявления (автоинкремент)");
                entity.Property(e => e.ad_num).HasMaxLength(100).HasComment("Номер дома");
                entity.Property(e => e.area_obj).HasPrecision(10, 2).HasComment("Площадь объекта в квадратных метрах");
                entity.Property(e => e.city_district_id).HasComment("Ссылка на городской район (таблица city_district)");
                entity.Property(e => e.city_id).HasComment("Ссылка на город (таблица city)");
                entity.Property(e => e.cost).HasPrecision(15, 2).HasComment("Стоимость объекта в указанной валюте");
                entity.Property(e => e.description).HasComment("Описание объявления");
                entity.Property(e => e.district_id).HasComment("Ссылка на район (таблица district)");
                entity.Property(e => e.microdistrict_id).HasComment("Ссылка на микрорайон (таблица microdistrict)");
                entity.Property(e => e.source_id).HasComment("Ссылка на источник объявления (таблица source)");
                entity.Property(e => e.status_id).HasComment("Ссылка на статус объявления (таблица status)");
                entity.Property(e => e.street_id).HasComment("Ссылка на улицу (таблица street)");
                entity.Property(e => e.type_id).HasComment("Ссылка на тип объекта (таблица type_obj)");
                entity.HasOne(d => d.city_district).WithMany(p => p.ads).HasForeignKey(d => d.city_district_id).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_city_district");
                entity.HasOne(d => d.city).WithMany(p => p.ads).HasForeignKey(d => d.city_id).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_city");
                entity.HasOne(d => d.district).WithMany(p => p.ads).HasForeignKey(d => d.district_id).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_district");
                entity.HasOne(d => d.microdistrict).WithMany(p => p.ads).HasForeignKey(d => d.microdistrict_id).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_microdistrict");
                entity.HasOne(d => d.source).WithMany(p => p.ads).HasForeignKey(d => d.source_id).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_source");
                entity.HasOne(d => d.status).WithMany(p => p.ads).HasForeignKey(d => d.status_id).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_status");
                entity.HasOne(d => d.street).WithMany(p => p.ads).HasForeignKey(d => d.street_id).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_street");
                entity.HasOne(d => d.type).WithMany(p => p.ads).HasForeignKey(d => d.type_id).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_type_obj");
            });

            // Конфигурация для AdUserCreated
            modelBuilder.Entity<AdUserCreated>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("ad_user_created_pkey");
                entity.ToTable("ad_user_created", tb => tb.HasComment("Хранит пользовательские объявления"));
                entity.Property(e => e.Id).HasComment("Уникальный идентификатор объявления (автоинкремент)");
                entity.Property(e => e.UserId).HasComment("Ссылка на пользователя (таблица users)");
                entity.HasOne(a => a.Users).WithMany(u => u.Ads).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_user");
            });

            // Конфигурация для ad_parametre, city, city_district, district, favorite, image_ad, microdistrict, parsing_log, price_history, source, status, street, type_obj
            modelBuilder.Entity<ad_parametre>(entity =>
            {
                entity.HasKey(e => e.id).HasName("ad_parametres_pkey");
                entity.ToTable(tb => tb.HasComment("Хранит дополнительные параметры объявлений (например, этаж, наличие балкона)"));
                entity.HasIndex(e => e.id_ad, "idx_ad_parametres_id_ad");
                entity.Property(e => e.id).HasComment("Уникальный идентификатор параметра (автоинкремент)");
                entity.Property(e => e.id_ad).HasComment("Ссылка на объявление (таблица ad)");
                entity.Property(e => e.param_name).HasMaxLength(100).HasComment("Название параметра (например, \"этаж\", \"балкон\")");
                entity.Property(e => e.value_param).HasMaxLength(255).HasComment("Значение параметра (например, \"5\", \"true\")");
                entity.HasOne(d => d.id_adNavigation).WithMany(p => p.ad_parametres).HasForeignKey(d => d.id_ad).HasConstraintName("fk_ad");
            });

            modelBuilder.Entity<city>(entity =>
            {
                entity.HasKey(e => e.id).HasName("city_pkey");
                entity.ToTable("city", tb => tb.HasComment("Хранит информацию о городах"));
                entity.HasIndex(e => e.city_name, "unique_city_name").IsUnique();
                entity.Property(e => e.id).HasComment("Уникальный идентификатор города (автоинкремент)");
                entity.Property(e => e.city_name).HasMaxLength(100).HasComment("Название города (например, \"Москва\")");
            });

            modelBuilder.Entity<city_district>(entity =>
            {
                entity.HasKey(e => e.id).HasName("city_district_pkey");
                entity.ToTable("city_district", tb => tb.HasComment("Хранит информацию о городских районах (например, подрайоны внутри города)"));
                entity.HasIndex(e => e.city_district_name, "unique_city_district_name").IsUnique();
                entity.Property(e => e.id).HasComment("Уникальный идентификатор городского района (автоинкремент)");
                entity.Property(e => e.city_district_name).HasMaxLength(100).HasComment("Название городского района (например, \"Тверской\")");
            });

            modelBuilder.Entity<district>(entity =>
            {
                entity.HasKey(e => e.id).HasName("district_pkey");
                entity.ToTable("district", tb => tb.HasComment("Хранит информацию о районах (например, административные районы города)"));
                entity.HasIndex(e => e.district_name, "unique_district_name").IsUnique();
                entity.Property(e => e.id).HasComment("Уникальный идентификатор района (автоинкремент)");
                entity.Property(e => e.district_name).HasMaxLength(100).HasComment("Название района (например, \"Центральный\")");
            });

            modelBuilder.Entity<favorite>(entity =>
            {
                entity.HasKey(e => e.id).HasName("favorites_pkey");
                entity.ToTable(tb => tb.HasComment("Хранит информацию об объявлениях, добавленных пользователями в избранное"));
                entity.HasIndex(e => e.id_ad, "idx_favorites_id_ad");
                entity.HasIndex(e => e.id_user, "idx_favorites_id_user");
                entity.HasIndex(e => new { e.id_user, e.id_ad }, "unique_user_ad").IsUnique();
                entity.Property(e => e.id).HasComment("Уникальный идентификатор записи избранного (автоинкремент)");
                entity.Property(e => e.id_ad).HasComment("Ссылка на объявление (таблица ad)");
                entity.Property(e => e.id_user).HasComment("Ссылка на пользователя (таблица user)");
                entity.HasOne(d => d.id_adNavigation).WithMany(p => p.favorites).HasForeignKey(d => d.id_ad).HasConstraintName("fk_ad");
                entity.HasOne(d => d.id_userNavigation).WithMany(p => p.favorites).HasForeignKey(d => d.id_user).HasConstraintName("fk_user");
            });

            modelBuilder.Entity<image_ad>(entity =>
            {
                entity.HasKey(e => e.id).HasName("image_ad_pkey");
                entity.ToTable("image_ad", tb => tb.HasComment("Хранит изображения, связанные с объявлениями"));
                entity.HasIndex(e => e.id_ad, "idx_image_ad_id_ad");
                entity.Property(e => e.id).HasComment("Уникальный идентификатор изображения (автоинкремент)");
                entity.Property(e => e.id_ad).HasComment("Ссылка на объявление (таблица ad)");
                entity.Property(e => e.url_img).HasMaxLength(255).HasComment("URL изображения на сайте-источнике или в хранилище");
                entity.Property(e => e.основное).HasDefaultValue(false).HasComment("Флаг, указывающий, является ли изображение основным для превью");
                entity.HasOne(d => d.id_adNavigation).WithMany(p => p.image_ads).HasForeignKey(d => d.id_ad).HasConstraintName("fk_ad");
            });

            modelBuilder.Entity<microdistrict>(entity =>
            {
                entity.HasKey(e => e.id).HasName("microdistrict_pkey");
                entity.ToTable("microdistrict", tb => tb.HasComment("Хранит информацию о микрорайонах"));
                entity.HasIndex(e => e.microdistrict_name, "unique_microdistrict_name").IsUnique();
                entity.Property(e => e.id).HasComment("Уникальный идентификатор микрорайона (автоинкремент)");
                entity.Property(e => e.microdistrict_name).HasMaxLength(100).HasComment("Название микрорайона (например, \"Патриаршие пруды\")");
            });

            modelBuilder.Entity<parsing_log>(entity =>
            {
                entity.HasKey(e => e.id).HasName("parsing_logs_pkey");
                entity.ToTable(tb => tb.HasComment("Хранит логи работы парсера для мониторинга и отладки"));
                entity.HasIndex(e => e.id_source, "idx_parsing_logs_id_source");
                entity.Property(e => e.id).HasComment("Уникальный идентификатор записи лога (автоинкремент)");
                entity.Property(e => e.added_record).HasDefaultValue(0).HasComment("Количество добавленных объявлений");
                entity.Property(e => e.deleted_record).HasDefaultValue(0).HasComment("Количество помеченных как неактивные объявлений");
                entity.Property(e => e.end_time).HasComment("Время окончания парсинга").HasColumnType("timestamp without time zone");
                entity.Property(e => e.error_msg).HasComment("Сообщение об ошибке, если парсинг не удался");
                entity.Property(e => e.id_source).HasComment("Ссылка на источник (таблица source)");
                entity.Property(e => e.start_time).HasDefaultValueSql("CURRENT_TIMESTAMP").HasComment("Время начала парсинга").HasColumnType("timestamp without time zone");
                entity.Property(e => e.status).HasMaxLength(20).HasComment("Статус выполнения парсинга (success, failed, partial)");
                entity.Property(e => e.update_record).HasDefaultValue(0).HasComment("Количество обновлённых объявлений");
                entity.HasOne(d => d.id_sourceNavigation).WithMany(p => p.parsing_logs).HasForeignKey(d => d.id_source).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_source");
            });

            modelBuilder.Entity<price_history>(entity =>
            {
                entity.HasKey(e => e.id).HasName("price_history_pkey");
                entity.ToTable("price_history", tb => tb.HasComment("Хранит историю изменений цен для каждого объявления"));
                entity.HasIndex(e => e.id_ad, "idx_price_history_id_ad");
                entity.Property(e => e.id).HasComment("Уникальный идентификатор записи в истории (автоинкремент)");
                entity.Property(e => e.change_date).HasDefaultValueSql("CURRENT_TIMESTAMP").HasComment("Дата изменения цены").HasColumnType("timestamp without time zone");
                entity.Property(e => e.id_ad).HasComment("Ссылка на объявление (таблица ad)");
                entity.Property(e => e.price).HasPrecision(15, 2).HasComment("Цена на момент записи");
                entity.HasOne(d => d.id_adNavigation).WithMany(p => p.price_histories).HasForeignKey(d => d.id_ad).HasConstraintName("fk_ad");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("role_pkey");
                entity.ToTable("role", tb => tb.HasComment("Хранит роли пользователей (например, администратор, пользователь)"));
                entity.HasIndex(e => e.Name, "unique_role_name").IsUnique();
                entity.Property(e => e.Id).HasComment("Уникальный идентификатор роли (автоинкремент)");
                entity.Property(e => e.Name).HasMaxLength(50).HasComment("Название роли (например, \"admin\", \"user\")");
                
            });

            modelBuilder.Entity<source>(entity =>
            {
                entity.HasKey(e => e.id).HasName("source_pkey");
                entity.ToTable("source", tb => tb.HasComment("Хранит информацию о сайтах-источниках объявлений"));
                entity.HasIndex(e => e.source_name, "unique_source_name").IsUnique();
                entity.Property(e => e.id).HasComment("Уникальный идентификатор источника (автоинкремент)");
                entity.Property(e => e.last_parsing_date).HasComment("Дата и время последнего парсинга источника").HasColumnType("timestamp without time zone");
                entity.Property(e => e.source_name).HasMaxLength(100).HasComment("Название сайта-источника (например, \"Авито\", \"ЦИАН\")");
                entity.Property(e => e.url).HasMaxLength(255).HasComment("Базовый URL сайта-источника");
            });

            modelBuilder.Entity<status>(entity =>
            {
                entity.HasKey(e => e.id).HasName("status_pkey");
                entity.ToTable("status", tb => tb.HasComment("Хранит статусы объявлений"));
                entity.HasIndex(e => e.status_name, "unique_status_name").IsUnique();
                entity.Property(e => e.id).HasComment("Уникальный идентификатор статуса (автоинкремент)");
                entity.Property(e => e.status_name).HasMaxLength(50).HasComment("Название статуса (например, \"активно\", \"продано\", \"удалено\")");
            });

            modelBuilder.Entity<street>(entity =>
            {
                entity.HasKey(e => e.id).HasName("street_pkey");
                entity.ToTable("street", tb => tb.HasComment("Хранит информацию об улицах"));
                entity.HasIndex(e => e.street_name, "unique_street_name").IsUnique();
                entity.Property(e => e.id).HasComment("Уникальный идентификатор улицы (автоинкремент)");
                entity.Property(e => e.street_name).HasMaxLength(255).HasComment("Название улицы (например, \"Тверская улица\")");
            });

            modelBuilder.Entity<type_obj>(entity =>
            {
                entity.HasKey(e => e.id).HasName("type_obj_pkey");
                entity.ToTable("type_obj", tb => tb.HasComment("Хранит типы объектов недвижимости (например, аренда, продажа)"));
                entity.HasIndex(e => e.type_name, "unique_type_name").IsUnique();
                entity.Property(e => e.id).HasComment("Уникальный идентификатор типа объекта (автоинкремент)");
                entity.Property(e => e.type_name).HasMaxLength(50).HasComment("Название типа объекта (например, \"аренда\", \"продажа\")");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("user_pkey");
                entity.ToTable("users", tb => tb.HasComment("Хранит информацию о зарегистрированных пользователях сервиса"));
                entity.HasIndex(e => e.Email, "unique_email").IsUnique();
                entity.Property(e => e.Id).HasComment("Уникальный идентификатор пользователя (автоинкремент)");
                entity.Property(e => e.DateOfBirth).HasColumnType("timestamp without time zone").HasComment("Дата рождения пользователя");
                entity.Property(e => e.Email).HasMaxLength(255).HasComment("Email пользователя, используется для входа");
                entity.Property(e => e.HashPassword).HasMaxLength(255).HasComment("Хэшированный пароль пользователя");
                entity.Property(e => e.Name).HasMaxLength(100).HasComment("Имя пользователя");
                entity.Property(e => e.PhoneNumber).HasMaxLength(20).HasComment("Номер телефона пользователя");
                entity.Property(e => e.RegistrationDate).HasDefaultValueSql("CURRENT_TIMESTAMP").HasComment("Дата регистрации пользователя").HasColumnType("timestamp without time zone");
                entity.Property(e => e.Surname).HasMaxLength(100).HasComment("Фамилия пользователя");
                entity.Property(e => e.RoleId).HasComment("Ссылка на роль пользователя (таблица role)");
                entity.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_role");
                entity.HasData(
                    new User
                    {
                        Id = 100,
                        Name = "Test",
                        Surname = "User",
                        Email = "test@example.com",
                        HashPassword = "hashed_password", // Замените на реальный хэш
                        RoleId = 1,
                        RegistrationDate = new DateTime(2025, 6, 28, 0, 0, 0, DateTimeKind.Unspecified),
                        DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
                    }
                );
            });
        }
    }
}