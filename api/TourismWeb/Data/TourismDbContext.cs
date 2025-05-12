using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Reflection;
using TourismWeb.Models;
using System.Text.Json;

namespace TourismWeb.Data
{
    public class TourismDbContext : DbContext
    {
        public TourismDbContext(DbContextOptions options) : base(options){ }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Review> Reviews { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Cau hinh do dai cac truong
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Username)
                .IsUnique();  

            // Đảm bảo rằng Email là duy nhất
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Email)
                .IsUnique();

            modelBuilder.Entity<Booking>()
                .Property(b => b.Total_price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Schedule>()
                .Property(s => s.Adult_price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Schedule>()
                .Property(s => s.Children_price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Tour>()
                .Property(t => t.Price)
                .HasColumnType("decimal(18,2)");

        // Mối quan hệ giữa các Entity
        modelBuilder.Entity<Account>()
                .HasOne(a => a.Customer)
                .WithOne(c => c.Account)
                .HasForeignKey<Customer>(c => c.Email)
                .HasPrincipalKey<Account>(a => a.Email);

            modelBuilder.Entity<Tour>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tours)
                .HasForeignKey(t => t.Category_Id);

            modelBuilder.Entity<Tour>()
                .HasOne(t => t.Location)
                .WithMany(l => l.Tours)
                .HasForeignKey(t => t.Location_Id);

            modelBuilder.Entity<Image>()
                .HasOne(i => i.Tour)
                .WithMany(t => t.Images)
                .HasForeignKey(i => i.Tour_Id);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Tour)
                .WithMany(t => t.Schedules)
                .HasForeignKey(s => s.Tour_Id);

            modelBuilder.Entity<Booking>()  
                .HasOne(b => b.Customer)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.Customer_Id);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Schedule)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.Schedule_Id);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.Booking_Id);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Tour)
                .WithMany(t => t.Reviews)
                .HasForeignKey(r => r.Tour_Id);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.Customer_Id);


            //seed data
            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id = "ACC001",
                    Username = "admin",
                    Password = "admin",
                    Email = "admin@gmail.com",
                    OTP = null,
                    Role = 0,
                    isConfirmed = 1
                },
                new Account
                {
                    Id = "ACC002",
                    Username = "customer",
                    Password = "cust1234",
                    Email = "customer1@gmail.com",
                    OTP = null,
                    Role = 1,
                    isConfirmed = 1
                }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = "CATE001",
                    Name = "Du lịch biển",
                    Description = "Khám phá các bãi biển tuyệt đẹp, tận hưởng nắng vàng và sóng biển, tham gia thể thao dưới nước. Phù hợp cho gia đình, cặp đôi hoặc những ai muốn thư giãn."
                },
                new Category
                {
                    Id = "CATE002",
                    Name = "Du lịch văn hóa",
                    Description = "Trải nghiệm di sản văn hóa phong phú, tham quan đền chùa, bảo tàng, tìm hiểu phong tục tập quán địa phương. Lý tưởng cho người yêu lịch sử và nghệ thuật."
                },
                new Category
                {
                    Id = "CATE003",
                    Name = "Du lịch phiêu lưu",
                    Description = "Dành cho những người thích thử thách và khám phá. Tham gia các hoạt động mạo hiểm như leo núi, trekking, chèo thuyền kayak, zipline trong môi trường tự nhiên."
                },
                new Category
                {
                    Id = "CATE004",
                    Name = "Du lịch nghỉ dưỡng",
                    Description = "Thoát khỏi cuộc sống bộn bề, thư giãn tại các resort, spa sang trọng. Tận hưởng dịch vụ cao cấp và không gian yên bình để phục hồi năng lượng, tìm lại cân bằng."
                },
                new Category
                {
                    Id = "CATE005",
                    Name = "Du lịch ẩm thực",
                    Description = "Khám phá hương vị độc đáo của các vùng miền. Thưởng thức đặc sản địa phương, học cách chế biến món ăn truyền thống, trải nghiệm văn hóa ẩm thực sâu sắc."
                },
                new Category
                {
                    Id = "CATE006",
                    Name = "Du lịch sinh thái",
                    Description = "Hòa mình vào thiên nhiên hoang sơ. Tham quan rừng quốc gia, công viên bảo tồn, tìm hiểu hệ động thực vật đa dạng và tham gia các hoạt động thân thiện môi trường."
                },
                new Category
                {
                    Id = "CATE007",
                    Name = "Du lịch tâm linh",
                    Description = "Tìm hiểu về tín ngưỡng và tôn giáo. Viếng thăm các ngôi chùa, nhà thờ, thánh địa linh thiêng, tham gia các nghi lễ truyền thống để tìm kiếm sự bình an trong tâm hồn."
                },
                new Category
                {
                    Id = "CATE008",
                    Name = "Du lịch thể thao",
                    Description = "Kết hợp du lịch với các hoạt động thể thao. Tham gia giải chạy, đạp xe, golf, lướt ván, hoặc chỉ đơn giản là duy trì lối sống năng động ngay cả trong kỳ nghỉ."
                },
                new Category
                {
                    Id = "CATE009",
                    Name = "Du lịch miền núi",
                    Description = "Chinh phục đỉnh cao, khám phá bản làng vùng cao. Tận hưởng không khí trong lành, cảnh quan hùng vĩ của núi rừng, tìm hiểu văn hóa các dân tộc thiểu số đặc sắc."
                },
                new Category
                {
                    Id = "CATE010",
                    Name = "Du lịch thành phố",
                    Description = "Khám phá nhịp sống sôi động của các đô thị lớn. Tham quan kiến trúc hiện đại và cổ kính, mua sắm tại các trung tâm thương mại, thưởng thức ẩm thực đường phố."
                },
                new Category
                {
                    Id = "CATE011",
                    Name = "Du lịch gia đình",
                    Description = "Thiết kế đặc biệt cho các gia đình có trẻ nhỏ hoặc đa thế hệ. Bao gồm các hoạt động vui chơi giải trí phù hợp mọi lứa tuổi, tạo kỷ niệm đáng nhớ."
                },
                new Category
                {
                    Id = "CATE012",
                    Name = "Du lịch cặp đôi",
                    Description = "Dành riêng cho các cặp đôi. Trải nghiệm lãng mạn, không gian riêng tư, các bữa tối dưới ánh nến, hoạt động thư giãn và tận hưởng thời gian chất lượng bên nhau."
                },
                new Category
                {
                    Id = "CATE013",
                    Name = "Du lịch bụi",
                    Description = "Tự do khám phá theo cách riêng. Lên kế hoạch linh hoạt, di chuyển tiết kiệm, hòa nhập với người dân địa phương, trải nghiệm chân thực và đầy ngẫu hứng."
                },
                new Category
                {
                    Id = "CATE014",
                    Name = "Du lịch sang trọng",
                    Description = "Tận hưởng dịch vụ đẳng cấp và tiện nghi hoàn hảo. Nghỉ tại khách sạn 5 sao, di chuyển bằng phương tiện cao cấp, trải nghiệm ẩm thực tinh tế và các hoạt động độc quyền."
                },
                new Category
                {
                    Id = "CATE015",
                    Name = "Du lịch nông nghiệp",
                    Description = "Trải nghiệm cuộc sống làng quê. Tham gia các hoạt động canh tác, thu hoạch nông sản, tìm hiểu quy trình sản xuất, thưởng thức sản phẩm tươi ngon ngay tại vườn."
                },
                new Category
                {
                    Id = "CATE016",
                    Name = "Du lịch lịch sử",
                    Description = "Quay ngược thời gian khám phá các di tích lịch sử, chiến trường xưa, bảo tàng. Tìm hiểu về quá khứ hào hùng và những câu chuyện làm nên bản sắc dân tộc."
                },
                new Category
                {
                    Id = "CATE017",
                    Name = "Du lịch sông nước",
                    Description = "Du ngoạn trên sông, kênh rạch, khám phá cuộc sống miền Tây sông nước (hoặc các vùng sông nước khác). Tham quan chợ nổi, vườn cây ăn trái, làng nghề truyền thống."
                },
                new Category
                {
                    Id = "CATE018",
                    Name = "Du lịch hang động",
                    Description = "Khám phá vẻ đẹp kỳ vĩ và bí ẩn của các hệ thống hang động. Trải nghiệm đi bộ, leo trèo, bơi lội trong lòng hang, chiêm ngưỡng thạch nhũ hàng triệu năm tuổi."
                },
                new Category
                {
                    Id = "CATE019",
                    Name = "Du lịch nhiếp ảnh",
                    Description = "Dành cho những người đam mê ghi lại khoảnh khắc. Khám phá các địa điểm có cảnh quan đẹp, văn hóa độc đáo, và con người thú vị để sáng tạo nên những bức ảnh ấn tượng."
                },
                new Category
                {
                    Id = "CATE020",
                    Name = "Du lịch sức khỏe",
                    Description = "Kết hợp nghỉ dưỡng với chăm sóc sức khỏe và sắc đẹp. Tham gia yoga, thiền, tắm bùn, suối khoáng nóng, trị liệu spa để thư giãn, tái tạo năng lượng và cải thiện sức khỏe."
                }
            );

            modelBuilder.Entity<Location>().HasData(
                new Location
                {
                    Id = "LOC001",
                    Name = "Vịnh Hạ Long"
                },
                new Location
                {
                    Id = "LOC002",
                    Name = "Phố cổ Hội An"
                },
                new Location
                {
                    Id = "LOC003",
                    Name = "Cố đô Huế"
                },
                new Location
                {
                    Id = "LOC004",
                    Name = "Phong Nha-Kẻ Bàng"
                },
                new Location
                {
                    Id = "LOC005",
                    Name = "Thánh địa Mỹ Sơn"
                },
                new Location
                {
                    Id = "LOC006",
                    Name = "Khu di tích Hoàng thành Thăng Long"
                },
                new Location
                {
                    Id = "LOC007",
                    Name = "Thành nhà Hồ"
                },
                new Location
                {
                    Id = "LOC008",
                    Name = "Quần thể danh thắng Tràng An"
                },
                new Location
                {
                    Id = "LOC009",
                    Name = "Hà Nội"
                },
                new Location
                {
                    Id = "LOC010",
                    Name = "TP. Hồ Chí Minh"
                },
                new Location
                {
                    Id = "LOC011",
                    Name = "Đà Nẵng"
                },
                new Location
                {
                    Id = "LOC012",
                    Name = "Nha Trang"
                },
                new Location
                {
                    Id = "LOC013",
                    Name = "Phú Quốc"
                },
                new Location
                {
                    Id = "LOC014",
                    Name = "Đà Lạt"
                },
                new Location
                {
                    Id = "LOC015",
                    Name = "Sa Pa"
                },
                new Location
                {
                    Id = "LOC016",
                    Name = "Mũi Né"
                },
                new Location
                {
                    Id = "LOC017",
                    Name = "Cần Thơ"
                },
                new Location
                {
                    Id = "LOC018",
                    Name = "Buôn Ma Thuột"
                },
                new Location
                {
                    Id = "LOC019",
                    Name = "Vũng Tàu"
                },
                new Location
                {
                    Id = "LOC020",
                    Name = "Côn Đảo"
                }
            );

            modelBuilder.Entity<Tour>().HasData(
                new Tour
                {
                    Id = "TOUR001",
                    Name = "Vịnh Hạ Long: Du thuyền ngủ đêm và khám phá hang động",
                    Short_description = "Trải nghiệm du thuyền cao cấp, ngắm cảnh vịnh huyền ảo.",
                    Detail_description = @"Tham gia hành trình 2 ngày 1 đêm trên du thuyền sang trọng khám phá Vịnh Hạ Long, Di sản Thiên nhiên Thế giới. Chèo thuyền kayak qua các đảo đá vôi kỳ vĩ, ghé thăm hang Sửng Sốt, tắm biển và thưởng thức bữa tối hải sản lãng mạn trên boong tàu. Tour bao gồm phòng nghỉ riêng trên du thuyền, các bữa ăn và hoạt động theo lịch trình.",
                    Schedule_description = @"Ngày 1: Đón tại Hà Nội (hoặc Hạ Long), lên du thuyền, ăn trưa, tham quan hang Sửng Sốt, chèo kayak, tắm biển Ti Tốp, ăn tối. Ngày 2: Học Thái Cực Quyền, ăn sáng, thăm làng chài, check-out, ăn trưa, về lại điểm đón.",
                    Category_Id = "CATE001",
                    Duration = "2N1D",
                    Price = 8050000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC001",
                    Create_at = new DateTime(2023, 8, 20, 10, 15, 0),
                    Update_at = new DateTime(2023, 9, 5, 11, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR002",
                    Name = "Vịnh Hạ Long: Hành trình 3 ngày trên Vịnh Lan Hạ",
                    Short_description = "Khám phá vẻ đẹp hoang sơ của Vịnh Lan Hạ kế cận Vịnh Hạ Long.",
                    Detail_description = @"Hành trình 3 ngày 2 đêm khám phá khu vực ít người biết đến hơn của Vịnh Hạ Long - Vịnh Lan Hạ. Tận hưởng không gian yên tĩnh, chèo kayak qua các hẻm núi nhỏ, khám phá đảo Khỉ và bãi Cát Dứa. Thử thách bản thân với các hoạt động như leo núi nhỏ hoặc đạp xe trên đảo Cát Bà. Trải nghiệm cuộc sống địa phương và ẩm thực tươi ngon.",
                    Schedule_description = @"Ngày 1: Đón khách, lên du thuyền, đi Lan Hạ, chèo kayak, tắm biển. Ngày 2: Thăm đảo Khỉ, leo núi (tùy chọn), đạp xe Cát Bà (tùy chọn), tối thư giãn. Ngày 3: Ăn sáng, check-out, thăm hang Sáng Tối, về lại.",
                    Category_Id = "CATE006",
                    Duration = "3N2D",
                    Price = 11960000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC001",
                    Create_at = new DateTime(2024, 1, 10, 9, 30, 0),
                    Update_at = new DateTime(2024, 1, 25, 14, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR003",
                    Name = "Vịnh Hạ Long: Ngắm hoàng hôn và ẩm thực trên vịnh",
                    Short_description = "Tour ngắn thư giãn, ngắm cảnh và thưởng thức ẩm thực độc đáo.",
                    Detail_description = @"Tour du lịch biển ngắn ngày tập trung vào trải nghiệm thư giãn và ẩm thực trên Vịnh Hạ Long. Chiều lên du thuyền, thưởng thức tiệc trà chiều ngắm hoàng hôn rực rỡ trên vịnh. Buổi tối, thưởng thức bữa tối hải sản đặc biệt với các món ăn chế biến từ nguyên liệu tươi sống. Tour thích hợp cho những ai có ít thời gian nhưng muốn tận hưởng không khí đặc trưng của Hạ Long.",
                    Schedule_description = @"Ngày 1: Chiều lên du thuyền, ngắm cảnh, tiệc trà, ngắm hoàng hôn. Tối: Ăn tối hải sản đặc biệt. Sau bữa tối, tàu về bến, kết thúc tour.",
                    Category_Id = "CATE005",
                    Duration = "1N0D",
                    Price = 4140000.0m,
                    Max_capacity = 30,
                    Location_Id = "LOC001",
                    Create_at = new DateTime(2023, 5, 1, 15, 0, 0),
                    Update_at = new DateTime(2023, 5, 1, 15, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR004",
                    Name = "Vịnh Hạ Long: Thám hiểm hang động và câu mực đêm",
                    Short_description = "Trải nghiệm khám phá hang động và hoạt động câu mực về đêm.",
                    Detail_description = @"Tour 2 ngày 1 đêm kết hợp khám phá sâu hơn các hang động kỳ vĩ và trải nghiệm hoạt động câu mực đêm thú vị trên Vịnh Hạ Long. Bên cạnh việc ngắm cảnh trên vịnh, bạn sẽ có cơ hội thám hiểm các hang động ít du khách biết đến và thử tài câu mực cùng ngư dân địa phương vào buổi tối. Thành quả có thể được chế biến ngay trên tàu.",
                    Schedule_description = @"Ngày 1: Lên tàu, ăn trưa, tham quan hang động (khác), chèo kayak. Tối: Ăn tối, hoạt động câu mực đêm. Ngày 2: Ăn sáng, thăm quan điểm cuối, về bến, kết thúc.",
                    Category_Id = "CATE003",
                    Duration = "2N1D",
                    Price = 8740000.0m,
                    Max_capacity = 22,
                    Location_Id = "LOC001",
                    Create_at = new DateTime(2024, 3, 15, 11, 0, 0),
                    Update_at = new DateTime(2024, 4, 1, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR005",
                    Name = "Vịnh Hạ Long: Tour chụp ảnh kỳ vĩ",
                    Short_description = "Lưu giữ khoảnh khắc tuyệt đẹp của Vịnh Hạ Long qua ống kính.",
                    Detail_description = @"Tour chuyên biệt dành cho những người yêu nhiếp ảnh, đưa bạn đến những góc đẹp nhất của Vịnh Hạ Long vào các thời điểm lý tưởng trong ngày (bình minh, hoàng hôn) để ghi lại những bức hình ấn tượng. Có hướng dẫn viên am hiểu các điểm chụp ảnh đẹp và điều kiện ánh sáng. Phù hợp cho cả người mới bắt đầu và nhiếp ảnh gia chuyên nghiệp.",
                    Schedule_description = @"Ngày 1: Đón khách, lên tàu, đến điểm chụp hoàng hôn, ăn tối. Ngày 2: Dậy sớm chụp bình minh, ăn sáng, thăm hang động/làng chài để chụp ảnh đời sống, về bến.",
                    Category_Id = "CATE019",
                    Duration = "2N1D",
                    Price = 9200000.0m,
                    Max_capacity = 15,
                    Location_Id = "LOC001",
                    Create_at = new DateTime(2023, 11, 11, 8, 0, 0),
                    Update_at = new DateTime(2023, 11, 11, 8, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR006",
                    Name = "Hội An: Dạo bước phố cổ và trải nghiệm nghề truyền thống",
                    Short_description = "Hòa mình vào không gian cổ kính và văn hóa Hội An.",
                    Detail_description = @"Khám phá vẻ đẹp vượt thời gian của Phố cổ Hội An, Di sản Văn hóa Thế giới. Đi bộ qua các con phố đèn lồng rực rỡ, thăm các hội quán, nhà cổ, chùa Cầu. Tham gia các lớp học làm đèn lồng, làm gốm hoặc học nấu món ăn địa phương. Tour mang đến trải nghiệm sâu sắc về lịch sử, văn hóa và đời sống người dân Hội An.",
                    Schedule_description = @"Ngày 1: Thăm phố cổ, nhà cổ Tấn Ký, Hội quán Phúc Kiến, Chùa Cầu, đi thuyền sông Hoài, thả đèn hoa đăng. Ngày 2: Tham gia lớp học làm đèn lồng/gốm, thăm làng rau Trà Quế (tùy chọn), mua sắm đặc sản.",
                    Category_Id = "CATE002",
                    Duration = "2N1D",
                    Price = 3450000.0m,
                    Max_capacity = 35,
                    Location_Id = "LOC002",
                    Create_at = new DateTime(2022, 7, 5, 14, 0, 0),
                    Update_at = new DateTime(2022, 7, 10, 10, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR007",
                    Name = "Hội An: Tour ẩm thực đường phố và làng quê thanh bình",
                    Short_description = "Khám phá hương vị đặc trưng và cuộc sống bình dị Hội An.",
                    Detail_description = @"Tour đặc biệt dành cho tín đồ ẩm thực và muốn tìm hiểu cuộc sống yên bình vùng quê Hội An. Dạo quanh các gánh hàng rong, thưởng thức các món ăn đường phố nổi tiếng như Cao Lầu, Mì Quảng, Bánh Mì Phượng. Sau đó, đạp xe khám phá các làng nghề truyền thống hoặc làng rau hữu cơ quanh phố cổ, tìm hiểu quy trình canh tác và giao lưu với người dân địa phương.",
                    Schedule_description = @"Ngày 1: Tour ẩm thực đường phố buổi chiều/tối, thử các món đặc sản. Ngày 2: Đạp xe ra vùng quê, thăm làng rau Trà Quế/làng nghề, ăn trưa tại nhà dân (tùy chọn), về lại phố cổ.",
                    Category_Id = "CATE005",
                    Duration = "2N1D",
                    Price = 4140000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC002",
                    Create_at = new DateTime(2023, 3, 20, 9, 0, 0),
                    Update_at = new DateTime(2023, 4, 15, 16, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR008",
                    Name = "Hội An: Lịch sử và Kiến trúc của Thương cảng xưa",
                    Short_description = "Tìm hiểu lịch sử vàng son của Thương cảng quốc tế Hội An.",
                    Detail_description = @"Tour tập trung vào khía cạnh lịch sử và kiến trúc của Hội An trong thời kỳ là một thương cảng sầm uất. Thăm các công trình kiến trúc cổ mang đậm dấu ấn giao thoa văn hóa Việt Nam, Nhật Bản, Trung Quốc. Nghe kể chuyện về con đường tơ lụa trên biển, cuộc sống của các thương nhân nước ngoài và sự phát triển của đô thị cổ. Bao gồm vé tham quan các điểm di tích chính.",
                    Schedule_description = @"Ngày 1: Giới thiệu tổng quan, thăm Chùa Cầu, các Hội Quán Trung Hoa, nhà cổ tiêu biểu. Ngày 2: Thăm Bảo tàng Văn hóa Hội An, xưởng thủ công mỹ nghệ, nghe thuyết minh về lịch sử thương mại.",
                    Category_Id = "CATE016",
                    Duration = "2N1D",
                    Price = 2990000.0m,
                    Max_capacity = 30,
                    Location_Id = "LOC002",
                    Create_at = new DateTime(2024, 2, 1, 10, 0, 0),
                    Update_at = new DateTime(2024, 2, 20, 11, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR009",
                    Name = "Hội An: Trải nghiệm lãng mạn cho cặp đôi",
                    Short_description = "Không gian lãng mạn và trải nghiệm độc đáo tại Hội An.",
                    Detail_description = @"Thiết kế dành riêng cho các cặp đôi, tour mang đến không gian lãng mạn và những trải nghiệm đáng nhớ tại Hội An. Dạo bộ dưới ánh đèn lồng lung linh, đi thuyền ngắm hoàng hôn trên sông Hoài, thả đèn hoa đăng cầu nguyện. Thưởng thức bữa tối lãng mạn bên sông, tham gia lớp học nấu ăn riêng hoặc đạp xe thư giãn qua những cánh đồng lúa xanh mướt. Lưu trú tại resort hoặc khách sạn boutique có không gian riêng tư.",
                    Schedule_description = @"Ngày 1: Nhận phòng, dạo phố cổ, đi thuyền ngắm hoàng hôn, thả đèn hoa đăng, ăn tối lãng mạn. Ngày 2: Tùy chọn (lớp nấu ăn/đạp xe), mua sắm quà lưu niệm, thư giãn.",
                    Category_Id = "CATE012",
                    Duration = "2N1D",
                    Price = 11500000.0m,
                    Max_capacity = 10,
                    Location_Id = "LOC002",
                    Create_at = new DateTime(2023, 9, 10, 15, 0, 0),
                    Update_at = new DateTime(2023, 9, 10, 15, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR010",
                    Name = "Hội An: Kết hợp phố cổ và biển An Bàng",
                    Short_description = "Trải nghiệm sự kết hợp độc đáo giữa phố cổ và biển.",
                    Detail_description = @"Tour kết hợp khám phá vẻ đẹp cổ kính của Phố cổ Hội An và thư giãn tại bãi biển An Bàng gần đó. Dành một phần thời gian để dạo bộ, mua sắm, thưởng thức ẩm thực trong phố cổ. Phần còn lại tận hưởng không khí biển trong lành, tắm nắng, bơi lội hoặc tham gia các hoạt động thể thao dưới nước tại bãi biển An Bàng xinh đẹp. Mang đến sự cân bằng giữa văn hóa và nghỉ dưỡng.",
                    Schedule_description = @"Ngày 1: Thăm phố cổ Hội An (nhà cổ, chùa Cầu, hội quán), ăn tối đặc sản. Ngày 2: Di chuyển ra biển An Bàng, tắm biển, thư giãn, ăn trưa hải sản, về lại.",
                    Category_Id = "CATE004",
                    Duration = "2N1D",
                    Price = 4600000.0m,
                    Max_capacity = 28,
                    Location_Id = "LOC002",
                    Create_at = new DateTime(2024, 4, 5, 9, 30, 0),
                    Update_at = new DateTime(2024, 4, 20, 14, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR011",
                    Name = "Huế: Hành trình khám phá Cố đô và Di sản",
                    Short_description = "Tìm hiểu lịch sử, văn hóa của Kinh thành Huế xưa.",
                    Detail_description = @"Hành trình 3 ngày 2 đêm đưa bạn về với Cố đô Huế trầm mặc, nơi từng là kinh đô của triều Nguyễn. Thăm Đại Nội, Hoàng thành, Tử Cấm thành, khám phá các cung điện, lăng tẩm uy nghiêm của các vị vua. Ngồi thuyền Rồng nghe ca Huế trên sông Hương, ghé thăm chùa Thiên Mụ và tìm hiểu về đời sống cung đình xưa. Bao gồm vé tham quan các di tích chính.",
                    Schedule_description = @"Ngày 1: Thăm Đại Nội (Hoàng thành, Tử Cấm thành), Bảo tàng Cổ vật Cung đình Huế. Tối: Nghe ca Huế trên sông Hương. Ngày 2: Thăm lăng Minh Mạng, lăng Khải Định, chùa Thiên Mụ. Ngày 3: Thăm lăng Tự Đức, chợ Đông Ba, mua sắm đặc sản.",
                    Category_Id = "CATE002",
                    Duration = "3N2D",
                    Price = 5750000.0m,
                    Max_capacity = 40,
                    Location_Id = "LOC003",
                    Create_at = new DateTime(2022, 11, 15, 10, 0, 0),
                    Update_at = new DateTime(2022, 12, 1, 11, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR012",
                    Name = "Huế: Du lịch tâm linh và ẩm thực chay",
                    Short_description = "Khám phá nét đẹp tâm linh và thưởng thức ẩm thực chay Huế.",
                    Detail_description = @"Huế không chỉ nổi tiếng với di sản cung đình mà còn là trung tâm Phật giáo với nhiều chùa cổ kính. Tour này tập trung vào khám phá các ngôi chùa nổi tiếng như chùa Thiên Mụ, Từ Hiếu, và tìm hiểu về đời sống tâm linh của người dân xứ Huế. Đồng thời, bạn sẽ có cơ hội thưởng thức ẩm thực chay phong phú và tinh tế - một nét văn hóa đặc sắc của Huế.",
                    Schedule_description = @"Ngày 1: Thăm chùa Thiên Mụ, chùa Từ Hiếu, tìm hiểu về Phật giáo Huế. Tối: Ăn tối buffet chay đặc sắc. Ngày 2: Thăm các am/tịnh thất nhỏ (nếu có), giao lưu với Phật tử, học làm một món chay đơn giản (tùy chọn).",
                    Category_Id = "CATE007",
                    Duration = "2N1D",
                    Price = 4140000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC003",
                    Create_at = new DateTime(2023, 1, 20, 8, 30, 0),
                    Update_at = new DateTime(2023, 1, 20, 8, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR013",
                    Name = "Huế: Làng nghề truyền thống và đời sống ven sông Hương",
                    Short_description = "Tìm hiểu các làng nghề và cuộc sống bình dị ven sông Hương.",
                    Detail_description = @"Khác với các tour cung đình, tour này đưa bạn ra ngoại thành Huế để khám phá các làng nghề truyền thống như làm nón lá, làm hương trầm, làm hoa giấy Thanh Tiên. Đi thuyền trên sông Hương đến các làng nghề, tìm hiểu quy trình làm sản phẩm, giao lưu với những người thợ thủ công tài hoa. Trải nghiệm cuộc sống bình dị, yên ả của người dân ven sông Hương.",
                    Schedule_description = @"Ngày 1: Đi thuyền sông Hương, thăm làng nghề làm nón lá/hương trầm. Ngày 2: Thăm làng hoa giấy Thanh Tiên, thưởng thức ẩm thực địa phương tại làng.",
                    Category_Id = "CATE002",
                    Duration = "2N1D",
                    Price = 3680000.0m,
                    Max_capacity = 30,
                    Location_Id = "LOC003",
                    Create_at = new DateTime(2024, 3, 10, 14, 0, 0),
                    Update_at = new DateTime(2024, 3, 18, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR014",
                    Name = "Huế: Khám phá lăng tẩm cổ kính và kiến trúc độc đáo",
                    Short_description = "Hành trình sâu vào kiến trúc và lịch sử các lăng tẩm triều Nguyễn.",
                    Detail_description = @"Tour tập trung vào việc khám phá các lăng tẩm của các vị vua Nguyễn, mỗi lăng mang một phong cách kiến trúc và câu chuyện lịch sử riêng. Thăm lăng Gia Long (lăng Thiên Thọ) uy nghiêm giữa thiên nhiên, lăng Minh Mạng (Hiếu lăng) với bố cục cân đối, lăng Tự Đức (Khiêm lăng) trữ tình thơ mộng, và lăng Khải Định (Ứng lăng) với sự kết hợp Đông-Tây phức tạp. Tìm hiểu về quan niệm sống và chết của các vị vua qua kiến trúc lăng mộ của họ.",
                    Schedule_description = @"Ngày 1: Thăm lăng Minh Mạng, lăng Thiệu Trị. Ngày 2: Thăm lăng Tự Đức, lăng Đồng Khánh. Ngày 3: Thăm lăng Khải Định, lăng Gia Long.",
                    Category_Id = "CATE016",
                    Duration = "3N2D",
                    Price = 6440000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC003",
                    Create_at = new DateTime(2023, 7, 1, 9, 30, 0),
                    Update_at = new DateTime(2023, 7, 25, 15, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR015",
                    Name = "Huế: Kết hợp Cố đô và Phá Tam Giang",
                    Short_description = "Trải nghiệm di sản Huế và vẻ đẹp bình minh/hoàng hôn Phá Tam Giang.",
                    Detail_description = @"Tour kết hợp khám phá các di tích lịch sử trong Cố đô Huế và trải nghiệm vẻ đẹp hoang sơ, yên bình của Phá Tam Giang - đầm phá lớn nhất Đông Nam Á. Buổi chiều hoặc sáng sớm ra phá ngắm bình minh hoặc hoàng hôn tuyệt đẹp, đi thuyền trên phá, tìm hiểu đời sống ngư dân địa phương và thưởng thức hải sản tươi sống. Mang đến sự kết hợp giữa lịch sử và sinh thái.",
                    Schedule_description = @"Ngày 1: Thăm Đại Nội/lăng tẩm (buổi sáng), di chuyển ra Phá Tam Giang buổi chiều, đi thuyền, ngắm hoàng hôn, ăn tối hải sản. Ngày 2: Buổi sáng tự do/mua sắm tại Huế, kết thúc tour.",
                    Category_Id = "CATE006",
                    Duration = "2N1D",
                    Price = 5060000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC003",
                    Create_at = new DateTime(2024, 1, 5, 10, 0, 0),
                    Update_at = new DateTime(2024, 1, 15, 14, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR016",
                    Name = "Phong Nha: Thám hiểm động Phong Nha và Tiên Sơn",
                    Short_description = "Khám phá vẻ đẹp kỳ vĩ của hai động nổi tiếng nhất Phong Nha.",
                    Detail_description = @"Hành trình khám phá Vườn quốc gia Phong Nha-Kẻ Bàng, Di sản Thiên nhiên Thế giới. Đi thuyền trên sông Son vào động Phong Nha - động nước dài nhất Việt Nam, ngắm nhìn hệ thống thạch nhũ và măng đá kỳ ảo. Sau đó, leo bộ khám phá động Tiên Sơn với vẻ đẹp khô ráo, lung linh như chốn bồng lai tiên cảnh. Tour cơ bản và phổ biến nhất tại Phong Nha.",
                    Schedule_description = @"Ngày 1: Đến Phong Nha, đi thuyền sông Son, thăm động Phong Nha. Buổi chiều: Thăm động Tiên Sơn. Tối: Nghỉ ngơi tại Phong Nha.",
                    Category_Id = "CATE003",
                    Duration = "2N1D",
                    Price = 4370000.0m,
                    Max_capacity = 30,
                    Location_Id = "LOC004",
                    Create_at = new DateTime(2023, 4, 10, 11, 0, 0),
                    Update_at = new DateTime(2023, 4, 20, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR017",
                    Name = "Phong Nha: Phiêu lưu Zipline và tắm bùn hang Tối",
                    Short_description = "Trải nghiệm mạo hiểm và thư giãn tại hang Tối.",
                    Detail_description = @"Tour phiêu lưu mạo hiểm tại Vườn quốc gia Phong Nha-Kẻ Bàng. Tham gia Zipline vượt sông Chày dài nhất Việt Nam để đến hang Tối. Khám phá hang Tối bằng đèn pin đội đầu, lội bùn khoáng tự nhiên trong hang (có lợi cho sức khỏe). Sau đó, tham gia các hoạt động vui chơi dưới nước trên sông Chày như bơi lội, chèo kayak. Phù hợp với người trẻ và yêu thích thử thách.",
                    Schedule_description = @"Ngày 1: Đến Phong Nha, di chuyển đến sông Chày/hang Tối. Tham gia Zipline, khám phá hang Tối (lội bùn), tắm sông Chày, vui chơi dưới nước.",
                    Category_Id = "CATE003",
                    Duration = "1N0D",
                    Price = 2760000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC004",
                    Create_at = new DateTime(2024, 5, 1, 8, 0, 0),
                    Update_at = new DateTime(2024, 5, 1, 8, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR018",
                    Name = "Phong Nha: Thám hiểm Vườn quốc gia và Hang Én (3 ngày)",
                    Short_description = "Hành trình trekking và cắm trại đầy thử thách đến Hang Én.",
                    Detail_description = @"Tour trekking và thám hiểm 3 ngày 2 đêm đưa bạn vào sâu trong Vườn quốc gia Phong Nha-Kẻ Bàng, khám phá Hang Én - hang động lớn thứ 3 thế giới và nơi cắm trại tuyệt đẹp bên hồ nước trong hang. Đi bộ xuyên rừng, băng suối, tìm hiểu hệ sinh thái độc đáo và thử thách giới hạn bản thân. Tour yêu cầu thể lực tốt và tinh thần phiêu lưu. Bao gồm trang thiết bị cắm trại và hướng dẫn viên chuyên nghiệp.",
                    Schedule_description = @"Ngày 1: Trekking vào rừng, ăn trưa, tiếp tục đi bộ đến Hang Én, dựng trại, ăn tối, cắm trại trong hang. Ngày 2: Khám phá sâu hơn Hang Én, tắm hồ trong hang (tùy chọn), ăn sáng/trưa/tối tại hang. Ngày 3: Ăn sáng, thu dọn, trekking ra ngoài, kết thúc.",
                    Category_Id = "CATE003",
                    Duration = "3N2D",
                    Price = 19550000.0m,
                    Max_capacity = 15,
                    Location_Id = "LOC004",
                    Create_at = new DateTime(2023, 6, 1, 9, 0, 0),
                    Update_at = new DateTime(2023, 6, 1, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR019",
                    Name = "Phong Nha: Khám phá Động Thiên Đường và Suối Nước Moọc",
                    Short_description = "Ngắm vẻ đẹp kỳ vĩ của động khô và thư giãn tại suối.",
                    Detail_description = @"Tour kết hợp khám phá Động Thiên Đường, được mệnh danh là 'hoàng cung trong lòng đất' với hệ thống thạch nhũ tráng lệ và quy mô rộng lớn. Sau đó, thư giãn và vui chơi tại Suối Nước Moọc, một điểm du lịch sinh thái với dòng suối trong xanh, cầu tre và các hoạt động như chèo kayak, tắm suối. Phù hợp cho gia đình và nhóm bạn.",
                    Schedule_description = @"Ngày 1: Thăm Động Thiên Đường (đi bộ hoặc xe golf), ăn trưa. Chiều: Đến Suối Nước Moọc, tắm suối, vui chơi, chèo kayak.",
                    Category_Id = "CATE006",
                    Duration = "1N0D",
                    Price = 2990000.0m,
                    Max_capacity = 35,
                    Location_Id = "LOC004",
                    Create_at = new DateTime(2022, 9, 1, 10, 30, 0),
                    Update_at = new DateTime(2022, 9, 1, 10, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR020",
                    Name = "Phong Nha: Trải nghiệm đạp xe xuyên vườn quốc gia",
                    Short_description = "Đạp xe khám phá cảnh quan thiên nhiên hùng vĩ Phong Nha.",
                    Detail_description = @"Tour đạp xe khám phá các cung đường đẹp nhất trong Vườn quốc gia Phong Nha-Kẻ Bàng. Đạp xe qua những cánh đồng lúa, làng quê yên bình, men theo bờ sông Son và vào sâu trong rừng. Ngắm nhìn cảnh quan núi đá vôi hùng vĩ, gặp gỡ người dân địa phương. Tour yêu cầu thể lực vừa phải, phù hợp với người yêu thiên nhiên và hoạt động ngoài trời. Bao gồm xe đạp và hướng dẫn viên địa phương.",
                    Schedule_description = @"Ngày 1: Bắt đầu đạp xe từ Phong Nha, đi qua các làng quê, cánh đồng, đến điểm nghỉ trưa. Chiều: Đạp xe vào khu vực rừng (tùy chọn), về lại Phong Nha.",
                    Category_Id = "CATE006",
                    Duration = "1N0D",
                    Price = 2300000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC004",
                    Create_at = new DateTime(2023, 10, 20, 9, 0, 0),
                    Update_at = new DateTime(2023, 10, 20, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR021",
                    Name = "Mỹ Sơn: Khám phá Thánh địa Ấn Độ giáo cổ",
                    Short_description = "Hành trình về quá khứ huy hoàng của Vương quốc Champa.",
                    Detail_description = @"Thăm Thánh địa Mỹ Sơn, Di sản Văn hóa Thế giới - quần thể đền tháp Champa cổ kính ẩn mình trong thung lũng. Tìm hiểu về lịch sử, kiến trúc, tôn giáo của Vương quốc Champa từng tồn tại ở miền Trung Việt Nam. Ngắm nhìn các công trình kiến trúc độc đáo bằng gạch nung và đá sa thạch, xem biểu diễn múa Chăm truyền thống. Tour văn hóa lịch sử sâu sắc.",
                    Schedule_description = @"Ngày 1: Di chuyển đến Mỹ Sơn (từ Đà Nẵng/Hội An), thăm quan khu di tích, nghe thuyết minh, xem múa Chăm. Kết thúc tour vào buổi trưa/chiều.",
                    Category_Id = "CATE016",
                    Duration = "1N0D",
                    Price = 1840000.0m,
                    Max_capacity = 40,
                    Location_Id = "LOC005",
                    Create_at = new DateTime(2022, 8, 10, 8, 30, 0),
                    Update_at = new DateTime(2022, 8, 10, 8, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR022",
                    Name = "Mỹ Sơn: Bình minh huyền ảo tại Thánh địa",
                    Short_description = "Ngắm bình minh và khám phá Thánh địa trong không khí tĩnh lặng.",
                    Detail_description = @"Tour đặc biệt khởi hành sớm để đến Thánh địa Mỹ Sơn vào lúc bình minh. Ngắm nhìn ánh nắng đầu tiên chiếu rọi lên các đền tháp cổ, tạo nên khung cảnh huyền ảo và linh thiêng. Trải nghiệm không khí yên bình, tĩnh lặng của di tích trước khi có nhiều khách du lịch. Cơ hội tuyệt vời cho những người yêu thích nhiếp ảnh và không gian trầm mặc.",
                    Schedule_description = @"Ngày 1: Khởi hành sớm (trước bình minh), đến Mỹ Sơn, ngắm bình minh, thăm quan khu di tích, ăn sáng nhẹ. Kết thúc tour buổi sáng.",
                    Category_Id = "CATE019",
                    Duration = "1N0D",
                    Price = 2300000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC005",
                    Create_at = new DateTime(2024, 1, 18, 5, 0, 0),
                    Update_at = new DateTime(2024, 1, 18, 5, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR023",
                    Name = "Mỹ Sơn: Kết hợp văn hóa Champa và làng nghề truyền thống",
                    Short_description = "Tìm hiểu văn hóa Champa và các làng nghề lân cận Mỹ Sơn.",
                    Detail_description = @"Tour kết hợp thăm Thánh địa Mỹ Sơn và khám phá các làng nghề truyền thống xung quanh như làng gốm Thanh Hà (Hội An) hoặc các làng dệt lụa. Tìm hiểu về mối liên hệ lịch sử giữa Thánh địa và các khu vực dân cư, quan sát quy trình làm ra các sản phẩm thủ công truyền thống và mua sắm quà lưu niệm độc đáo. Mang đến cái nhìn toàn diện hơn về khu vực này.",
                    Schedule_description = @"Ngày 1: Thăm Thánh địa Mỹ Sơn. Chiều: Di chuyển đến làng gốm Thanh Hà/làng dệt lụa, tìm hiểu quy trình, giao lưu, mua sắm.",
                    Category_Id = "CATE002",
                    Duration = "1N0D",
                    Price = 2530000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC005",
                    Create_at = new DateTime(2023, 5, 25, 9, 0, 0),
                    Update_at = new DateTime(2023, 5, 25, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR024",
                    Name = "Mỹ Sơn: Lịch sử và câu chuyện những ngôi đền cổ",
                    Short_description = "Tìm hiểu sâu về lịch sử và ý nghĩa các đền tháp Champa.",
                    Detail_description = @"Tour tập trung vào việc cung cấp thông tin chi tiết và chuyên sâu về lịch sử xây dựng, chức năng, ý nghĩa tôn giáo và các kỹ thuật xây dựng bí ẩn của các đền tháp tại Thánh địa Mỹ Sơn. Có hướng dẫn viên chuyên môn cao, am hiểu sâu về văn hóa Champa. Phù hợp với những người yêu thích lịch sử, khảo cổ và muốn tìm hiểu kỹ lưỡng về di tích.",
                    Schedule_description = @"Ngày 1: Thăm quan khu di tích Mỹ Sơn theo các cụm tháp, nghe thuyết minh chi tiết về lịch sử, kiến trúc, điêu khắc. Thảo luận và giải đáp thắc mắc.",
                    Category_Id = "CATE016",
                    Duration = "1N0D",
                    Price = 2070000.0m,
                    Max_capacity = 15,
                    Location_Id = "LOC005",
                    Create_at = new DateTime(2024, 4, 10, 10, 0, 0),
                    Update_at = new DateTime(2024, 4, 10, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR025",
                    Name = "Mỹ Sơn: Trải nghiệm văn hóa Chăm và múa Apsara",
                    Short_description = "Thưởng thức nghệ thuật biểu diễn và tìm hiểu văn hóa Chăm.",
                    Detail_description = @"Bên cạnh việc thăm Thánh địa Mỹ Sơn, tour này nhấn mạnh vào trải nghiệm văn hóa Chăm đương đại. Thưởng thức chương trình biểu diễn múa Chăm truyền thống (đặc biệt là điệu múa Apsara) tại khu vực Mỹ Sơn. Có cơ hội giao lưu với các nghệ nhân, tìm hiểu về trang phục, nhạc cụ và đời sống văn hóa của cộng đồng người Chăm hiện nay. Mang đến trải nghiệm đa chiều về văn hóa Champa.",
                    Schedule_description = @"Ngày 1: Thăm quan khu di tích Mỹ Sơn, nghe thuyết minh. Buổi chiều: Xem biểu diễn múa Chăm truyền thống, tìm hiểu về văn hóa Chăm.",
                    Category_Id = "CATE002",
                    Duration = "1N0D",
                    Price = 2185000.0m,
                    Max_capacity = 30,
                    Location_Id = "LOC005",
                    Create_at = new DateTime(2023, 12, 1, 8, 45, 0),
                    Update_at = new DateTime(2023, 12, 1, 8, 45, 0)
                },
                new Tour
                {
                    Id = "TOUR026",
                    Name = "Hà Nội: Khám phá 36 phố phường và nét cổ kính",
                    Short_description = "Dạo bộ và tìm hiểu về lịch sử khu phố cổ Hà Nội.",
                    Detail_description = @"Tour đi bộ khám phá Khu phố cổ Hà Nội (36 phố phường) với lịch sử nghìn năm văn hiến. Lạc bước qua những con phố nhỏ với kiến trúc nhà cổ, thăm các đền, chùa, hội quán linh thiêng. Thưởng thức ẩm thực đường phố đặc sắc của Hà Nội. Tìm hiểu về sự hình thành và phát triển của khu phố cổ, đời sống và văn hóa của người dân nơi đây. Bao gồm hướng dẫn viên am hiểu văn hóa Hà Nội.",
                    Schedule_description = @"Ngày 1: Thăm Hồ Gươm, đền Ngọc Sơn. Đi bộ khu phố cổ: Hàng Mã, Hàng Gai, Mã Mây (Nhà cổ), Đền Bạch Mã. Ăn tối món ăn đường phố (phở, bún chả...).",
                    Category_Id = "CATE010",
                    Duration = "1N0D",
                    Price = 1610000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC009",
                    Create_at = new DateTime(2022, 10, 5, 9, 0, 0),
                    Update_at = new DateTime(2022, 10, 5, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR027",
                    Name = "Hà Nội: Di sản Hoàng thành Thăng Long và Lăng Bác",
                    Short_description = "Tìm hiểu về lịch sử và di sản quan trọng của Hà Nội.",
                    Detail_description = @"Tour tập trung vào các di tích lịch sử và văn hóa quan trọng của Hà Nội. Thăm Khu di tích Hoàng thành Thăng Long - Di sản Văn hóa Thế giới, tìm hiểu về các triều đại phong kiến Việt Nam. Viếng Lăng Chủ tịch Hồ Chí Minh (tùy theo thời gian mở cửa), thăm Quảng trường Ba Đình và Bảo tàng Hồ Chí Minh. Tour cung cấp cái nhìn sâu sắc về lịch sử cận đại và hiện đại của Việt Nam.",
                    Schedule_description = @"Ngày 1: Thăm Hoàng thành Thăng Long. Chiều: Thăm Lăng Bác (bên ngoài/viếng nếu mở cửa), Quảng trường Ba Đình, Bảo tàng Hồ Chí Minh, Chùa Một Cột.",
                    Category_Id = "CATE016",
                    Duration = "1N0D",
                    Price = 2070000.0m,
                    Max_capacity = 30,
                    Location_Id = "LOC009",
                    Create_at = new DateTime(2023, 11, 20, 10, 30, 0),
                    Update_at = new DateTime(2023, 11, 20, 10, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR028",
                    Name = "Hà Nội: Ẩm thực tinh hoa và lớp học nấu ăn Việt",
                    Short_description = "Thưởng thức và tự tay chế biến món ngon Hà Nội.",
                    Detail_description = @"Tour trải nghiệm ẩm thực chuyên sâu tại Hà Nội. Bắt đầu bằng chuyến đi chợ địa phương để mua nguyên liệu tươi ngon. Sau đó, tham gia lớp học nấu ăn do đầu bếp chuyên nghiệp hướng dẫn, tự tay chế biến các món ăn truyền thống của Hà Nội như phở cuốn, nem rán, bún chả. Kết thúc bằng việc thưởng thức thành quả của mình. Tour mang tính tương tác cao và phù hợp với người yêu ẩm thực.",
                    Schedule_description = @"Ngày 1: Sáng đi chợ địa phương, tham gia lớp học nấu ăn (lý thuyết và thực hành), ăn trưa với món tự nấu.",
                    Category_Id = "CATE005",
                    Duration = "1N0D",
                    Price = 3450000.0m,
                    Max_capacity = 15,
                    Location_Id = "LOC009",
                    Create_at = new DateTime(2024, 2, 14, 9, 0, 0),
                    Update_at = new DateTime(2024, 2, 14, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR029",
                    Name = "Hà Nội: Về miền di sản quanh Hà Nội (Hoa Lư - Tam Cốc)",
                    Short_description = "Khám phá cố đô Hoa Lư và cảnh sắc Tam Cốc Bích Động.",
                    Detail_description = @"Tour một ngày từ Hà Nội đi Ninh Bình (khu vực Tràng An), khám phá cố đô Hoa Lư thời Đinh-Tiền Lê và cảnh quan Tam Cốc Bích Động được ví như 'Vịnh Hạ Long trên cạn'. Đi thuyền qua các hang động và cánh đồng lúa hai bên dòng sông Ngô Đồng. Thăm chùa Bích Động và các di tích khác trong khu vực. Tour mang đến trải nghiệm thiên nhiên và lịch sử độc đáo gần Hà Nội.",
                    Schedule_description = @"Ngày 1: Khởi hành từ Hà Nội đi Ninh Bình. Thăm Hoa Lư, Tam Cốc (đi thuyền). Ăn trưa. Chiều thăm chùa Bích Động. Trở về Hà Nội.",
                    Category_Id = "CATE006",
                    Duration = "1N0D",
                    Price = 2530000.0m,
                    Max_capacity = 40,
                    Location_Id = "LOC009",
                    Create_at = new DateTime(2023, 4, 1, 8, 0, 0),
                    Update_at = new DateTime(2023, 4, 1, 8, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR030",
                    Name = "Hà Nội: Trải nghiệm đời sống dân dã ngoại thành",
                    Short_description = "Khám phá cuộc sống bình dị và làng nghề ngoại ô Hà Nội.",
                    Detail_description = @"Tour đưa bạn ra khỏi trung tâm ồn ào để khám phá cuộc sống dân dã và các làng nghề truyền thống ở ngoại ô Hà Nội. Thăm làng gốm Bát Tràng, tìm hiểu quy trình làm gốm và tự tay nặn thử. Ghé thăm làng lụa Vạn Phúc hoặc làng mây tre đan. Giao lưu với người dân địa phương, tìm hiểu về nghề truyền thống của họ. Tour mang đến cái nhìn chân thực về văn hóa và đời sống ngoại thành.",
                    Schedule_description = @"Ngày 1: Di chuyển đến làng gốm Bát Tràng, thăm quan, học làm gốm. Ăn trưa. Chiều: Thăm làng lụa Vạn Phúc/làng nghề khác, mua sắm quà lưu niệm.",
                    Category_Id = "CATE002",
                    Duration = "1N0D",
                    Price = 1955000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC009",
                    Create_at = new DateTime(2023, 8, 8, 9, 30, 0),
                    Update_at = new DateTime(2023, 8, 8, 9, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR031",
                    Name = "TP. Hồ Chí Minh: Khám phá trung tâm Sài Gòn hiện đại và cổ kính",
                    Short_description = "Dạo quanh các điểm nổi bật của Sài Gòn xưa và nay.",
                    Detail_description = @"Tour khám phá trung tâm TP. Hồ Chí Minh, kết hợp giữa các công trình kiến trúc Pháp cổ và nét hiện đại sôi động. Thăm Dinh Độc Lập, Bưu điện Trung tâm Sài Gòn, Nhà thờ Đức Bà (bên ngoài do sửa chữa), Bảo tàng Chứng tích Chiến tranh, chợ Bến Thành. Tìm hiểu về lịch sử phát triển của Sài Gòn từ thời Pháp thuộc đến nay. Tour phù hợp cho những người muốn có cái nhìn tổng quan về thành phố.",
                    Schedule_description = @"Ngày 1: Thăm Dinh Độc Lập, Bưu điện, Nhà thờ Đức Bà, Đường sách. Ăn trưa. Chiều: Thăm Bảo tàng Chứng tích Chiến tranh, mua sắm tại chợ Bến Thành.",
                    Category_Id = "CATE010",
                    Duration = "1N0D",
                    Price = 1840000.0m,
                    Max_capacity = 35,
                    Location_Id = "LOC010",
                    Create_at = new DateTime(2022, 12, 10, 10, 0, 0),
                    Update_at = new DateTime(2022, 12, 10, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR032",
                    Name = "TP. Hồ Chí Minh: Ẩm thực đường phố Sài Gòn về đêm",
                    Short_description = "Thưởng thức hương vị đặc sắc của Sài Gòn về đêm.",
                    Detail_description = @"Tour ẩm thực về đêm sôi động tại TP. Hồ Chí Minh. Dạo quanh các khu vực tập trung nhiều món ăn đường phố nổi tiếng như Quận 4, Quận 5, hoặc các khu chợ đêm. Thử các món như bánh xèo, cơm tấm, hủ tiếu gõ, các loại ốc, phá lấu, và tráng miệng. Trải nghiệm không khí nhộn nhịp và phong cách ẩm thực đa dạng của Sài Gòn. Thường di chuyển bằng xe máy (có người lái).",
                    Schedule_description = @"Ngày 1: Tối bắt đầu tour, di chuyển qua các khu vực ẩm thực, thử các món ăn đặc trưng tại các quán nổi tiếng, kết thúc tour vào buổi tối muộn.",
                    Category_Id = "CATE005",
                    Duration = "1N0D",
                    Price = 1380000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC010",
                    Create_at = new DateTime(2023, 6, 18, 18, 0, 0),
                    Update_at = new DateTime(2023, 6, 18, 18, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR033",
                    Name = "TP. Hồ Chí Minh: Tham quan Địa đạo Củ Chi",
                    Short_description = "Tìm hiểu về hệ thống địa đạo trong chiến tranh.",
                    Detail_description = @"Tour đưa bạn đến Địa đạo Củ Chi, một di tích lịch sử quan trọng nằm cách trung tâm TP. Hồ Chí Minh khoảng 70km. Khám phá hệ thống đường hầm dưới lòng đất được sử dụng trong chiến tranh Việt Nam. Tìm hiểu về cuộc sống và cách chiến đấu của quân giải phóng. Có cơ hội xem phim tư liệu, chui thử địa đạo (tùy chọn), và trải nghiệm bắn súng (chi phí tự túc). Tour mang tính giáo dục và lịch sử sâu sắc.",
                    Schedule_description = @"Ngày 1: Di chuyển đến Củ Chi. Thăm khu di tích Địa đạo Củ Chi, xem phim, nghe thuyết minh, khám phá đường hầm, xem bẫy chông, khu tái hiện đời sống.",
                    Category_Id = "CATE016",
                    Duration = "1N0D",
                    Price = 1150000.0m,
                    Max_capacity = 45,
                    Location_Id = "LOC010",
                    Create_at = new DateTime(2024, 3, 1, 8, 30, 0),
                    Update_at = new DateTime(2024, 3, 1, 8, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR034",
                    Name = "TP. Hồ Chí Minh: Kết hợp thành phố và sông nước miền Tây",
                    Short_description = "Trải nghiệm sự khác biệt giữa đô thị và miền Tây sông nước.",
                    Detail_description = @"Tour kết hợp khám phá các điểm nổi bật của TP. Hồ Chí Minh và một chuyến đi ngắn đến khu vực Đồng bằng sông Cửu Long gần đó (ví dụ: Mỹ Tho, Bến Tre). Thăm quan thành phố buổi sáng, sau đó di chuyển đến miền Tây, đi thuyền trên sông, thăm các cù lao, vườn cây ăn trái, lò kẹo dừa, nghe đờn ca tài tử. Mang đến sự đa dạng về cảnh quan và văn hóa chỉ trong một hành trình ngắn.",
                    Schedule_description = @"Ngày 1: Thăm các điểm trung tâm Sài Gòn (sáng). Chiều: Di chuyển xuống miền Tây, đi thuyền, thăm cù lao, làng nghề, nghe nhạc truyền thống. Tối: Trở về TP.HCM.",
                    Category_Id = "CATE006",
                    Duration = "1N0D",
                    Price = 2760000.0m,
                    Max_capacity = 30,
                    Location_Id = "LOC010",
                    Create_at = new DateTime(2023, 9, 5, 7, 30, 0),
                    Update_at = new DateTime(2023, 9, 5, 7, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR035",
                    Name = "TP. Hồ Chí Minh: Nghệ thuật đường phố và ẩm thực khu Chợ Lớn",
                    Short_description = "Khám phá nét văn hóa độc đáo và ẩm thực khu Chợ Lớn.",
                    Detail_description = @"Tour đi bộ khám phá khu Chợ Lớn (Quận 5), trung tâm của cộng đồng người Hoa tại TP. Hồ Chí Minh. Thăm các hội quán cổ kính, chùa miếu linh thiêng mang đậm kiến trúc Trung Hoa. Dạo quanh các con phố, chợ truyền thống, ngắm nhìn nghệ thuật đường phố (các bức vẽ, bảng hiệu cổ). Đặc biệt, thưởng thức ẩm thực đặc sắc của khu vực này như mì vịt tiềm, sủi cảo, dimsum... Tour mang đến trải nghiệm văn hóa và ẩm thực đa dạng.",
                    Schedule_description = @"Ngày 1: Thăm quan các hội quán, chùa miếu tại Chợ Lớn. Đi bộ khám phá khu vực. Tối: Tour ẩm thực đường phố Chợ Lớn, thử các món ăn đặc trưng.",
                    Category_Id = "CATE002",
                    Duration = "1N0D",
                    Price = 1725000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC010",
                    Create_at = new DateTime(2024, 4, 22, 9, 0, 0),
                    Update_at = new DateTime(2024, 4, 22, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR036",
                    Name = "Hoàng thành Thăng Long: Hành trình Di sản nghìn năm",
                    Short_description = "Khám phá trung tâm quyền lực của Việt Nam xưa.",
                    Detail_description = @"Tham quan Khu di tích Hoàng thành Thăng Long, Di sản Văn hóa Thế giới. Tìm hiểu về lịch sử hình thành và phát triển của kinh đô Thăng Long qua các triều đại Đinh, Tiền Lê, Lý, Trần, Lê, Mạc, Nguyễn. Khám phá các di tích khảo cổ học, các công trình kiến trúc còn sót lại như Đoan Môn, Điện Kính Thiên, Hậu Lâu, Cột cờ Hà Nội. Tour mang tính lịch sử và văn hóa sâu sắc.",
                    Schedule_description = @"Ngày 1: Thăm Đoan Môn, Điện Kính Thiên, Hậu Lâu. Khám phá khu khảo cổ 18 Hoàng Diệu. Thăm Cột cờ Hà Nội.",
                    Category_Id = "CATE016",
                    Duration = "1N0D",
                    Price = 1380000.0m,
                    Max_capacity = 35,
                    Location_Id = "LOC006",
                    Create_at = new DateTime(2022, 11, 1, 10, 0, 0),
                    Update_at = new DateTime(2022, 11, 1, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR037",
                    Name = "Hoàng thành Thăng Long: Khám phá hầm chỉ huy D67 và nhà con Rồng",
                    Short_description = "Tìm hiểu lịch sử hiện đại tại Hoàng thành.",
                    Detail_description = @"Bên cạnh các di tích cổ, Hoàng thành Thăng Long còn lưu giữ các di tích lịch sử hiện đại quan trọng. Tour này tập trung vào việc thăm quan và tìm hiểu về Hầm chỉ huy Tác chiến D67, nơi làm việc của các vị tướng trong chiến tranh Việt Nam, và Nhà con Rồng (Long House) - khu làm việc của Bộ Chính trị và Quân ủy Trung ương. Tour mang đến góc nhìn về lịch sử cách mạng Việt Nam.",
                    Schedule_description = @"Ngày 1: Thăm khu trung tâm Hoàng thành, sau đó tập trung vào Hầm chỉ huy D67 và Nhà con Rồng. Nghe thuyết minh về lịch sử khu vực này.",
                    Category_Id = "CATE016",
                    Duration = "1N0D",
                    Price = 1265000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC006",
                    Create_at = new DateTime(2023, 3, 15, 9, 0, 0),
                    Update_at = new DateTime(2023, 3, 15, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR038",
                    Name = "Hoàng thành Thăng Long: Khảo cổ và những tầng di tích",
                    Short_description = "Tìm hiểu về công tác khảo cổ tại Hoàng thành.",
                    Detail_description = @"Tour dành cho những người quan tâm đến khảo cổ học và quá trình khai quật tại Hoàng thành Thăng Long. Thăm khu di tích khảo cổ 18 Hoàng Diệu, nơi phát hiện ra nhiều tầng văn hóa chồng xếp của các thời kỳ lịch sử khác nhau. Tìm hiểu về các hiện vật được khai quật, ý nghĩa của chúng và quá trình nghiên cứu di tích. Có thể có buổi nói chuyện ngắn với nhà nghiên cứu (tùy lịch trình).",
                    Schedule_description = @"Ngày 1: Thăm khu di tích khảo cổ 18 Hoàng Diệu, nghe thuyết minh chuyên sâu về các tầng di tích và hiện vật. Thăm Bảo tàng Hoàng thành Thăng Long.",
                    Category_Id = "CATE016",
                    Duration = "1N0D",
                    Price = 1610000.0m,
                    Max_capacity = 15,
                    Location_Id = "LOC006",
                    Create_at = new DateTime(2024, 1, 30, 10, 0, 0),
                    Update_at = new DateTime(2024, 1, 30, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR039",
                    Name = "Hoàng thành Thăng Long: Đêm huyền ảo tại di sản",
                    Short_description = "Trải nghiệm tham quan Hoàng thành Thăng Long về đêm.",
                    Detail_description = @"Tour đặc biệt khám phá Hoàng thành Thăng Long vào buổi tối. Ngắm nhìn các công trình được chiếu sáng lung linh, trải nghiệm không khí trang nghiêm và huyền ảo của di tích về đêm. Có các hoạt động tái hiện lịch sử, biểu diễn nghệ thuật truyền thống (tùy theo lịch của Ban quản lý di tích). Tour mang đến góc nhìn mới lạ và hấp dẫn về Hoàng thành.",
                    Schedule_description = @"Ngày 1: Chiều tối đến Hoàng thành, thăm các khu vực chính dưới ánh đèn, xem các hoạt động biểu diễn (nếu có), nghe kể chuyện lịch sử về đêm.",
                    Category_Id = "CATE002",
                    Duration = "1N0D",
                    Price = 1840000.0m,
                    Max_capacity = 30,
                    Location_Id = "LOC006",
                    Create_at = new DateTime(2023, 7, 7, 18, 30, 0),
                    Update_at = new DateTime(2023, 7, 7, 18, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR040",
                    Name = "Hoàng thành Thăng Long: Trải nghiệm không gian di sản cho gia đình",
                    Short_description = "Tour học mà chơi, phù hợp cho cả gia đình có trẻ nhỏ.",
                    Detail_description = @"Tour thiết kế dành cho gia đình, giúp trẻ em và người lớn cùng nhau tìm hiểu về lịch sử Hoàng thành Thăng Long một cách vui nhộn và tương tác. Tham gia các trò chơi giáo dục, hoạt động thực tế (như mặc thử trang phục cổ, làm đồ thủ công liên quan), nghe kể chuyện lịch sử bằng hình ảnh, thăm các khu vực phù hợp với trẻ nhỏ. Tạo sự hứng thú và kiến thức bổ ích cho cả gia đình.",
                    Schedule_description = @"Ngày 1: Các hoạt động nhập môn, trò chơi tại khu di tích. Thăm các điểm chính với cách tiếp cận thân thiện với trẻ em. Ăn trưa nhẹ. Buổi chiều: Hoạt động thực hành, tổng kết.",
                    Category_Id = "CATE011",
                    Duration = "1N0D",
                    Price = 2760000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC006",
                    Create_at = new DateTime(2024, 4, 15, 9, 0, 0),
                    Update_at = new DateTime(2024, 4, 15, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR041",
                    Name = "Thành nhà Hồ: Di sản kiến trúc đá cổ",
                    Short_description = "Khám phá tòa thành đá hùng vĩ duy nhất tại Việt Nam.",
                    Detail_description = @"Thăm Thành nhà Hồ (Thành Tây Đô), Di sản Văn hóa Thế giới tại Thanh Hóa. Đây là tòa thành được xây dựng bằng đá nguyên khối quy mô lớn, minh chứng cho kỹ thuật xây dựng độc đáo đầu thế kỷ 15. Tìm hiểu về lịch sử triều Hồ và ý nghĩa của công trình này. Ngắm nhìn các cổng thành và bức tường đá còn sót lại, chiêm ngưỡng kỹ thuật ghép đá tinh xảo.",
                    Schedule_description = @"Ngày 1: Di chuyển đến Thành nhà Hồ. Thăm quan 4 cổng thành, các đoạn tường thành còn lại, tìm hiểu lịch sử tại nhà trưng bày. Kết thúc tour buổi trưa/chiều.",
                    Category_Id = "CATE016",
                    Duration = "1N0D",
                    Price = 1380000.0m,
                    Max_capacity = 30,
                    Location_Id = "LOC007",
                    Create_at = new DateTime(2023, 2, 10, 10, 0, 0),
                    Update_at = new DateTime(2023, 2, 10, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR042",
                    Name = "Thành nhà Hồ: Kết hợp di sản và cảnh quan Ninh Bình",
                    Short_description = "Khám phá Thành nhà Hồ và các điểm du lịch Ninh Bình lân cận.",
                    Detail_description = @"Tour kết hợp thăm Thành nhà Hồ (Thanh Hóa) với các điểm du lịch nổi tiếng của Ninh Bình gần đó như Tam Cốc-Bích Động hoặc Tràng An. Buổi sáng thăm Thành nhà Hồ, tìm hiểu lịch sử. Buổi chiều di chuyển sang Ninh Bình để trải nghiệm cảnh quan thiên nhiên hùng vĩ 'Vịnh Hạ Long trên cạn' hoặc các khu tâm linh nổi tiếng. Mang đến sự đa dạng trong một chuyến đi.",
                    Schedule_description = @"Ngày 1: Thăm Thành nhà Hồ. Ăn trưa. Chiều: Di chuyển sang Ninh Bình, thăm Tam Cốc/Tràng An (đi thuyền). Tối: Nghỉ đêm tại Ninh Bình (tùy chọn) hoặc trở về điểm xuất phát.",
                    Category_Id = "CATE006",
                    Duration = "1N0D",
                    Price = 2990000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC007",
                    Create_at = new DateTime(2024, 5, 5, 8, 0, 0),
                    Update_at = new DateTime(2024, 5, 5, 8, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR043",
                    Name = "Thành nhà Hồ: Kiến trúc và kỹ thuật xây dựng cổ",
                    Short_description = "Tìm hiểu chuyên sâu về kỹ thuật xây dựng độc đáo của thành đá.",
                    Detail_description = @"Tour dành cho những người quan tâm đến kiến trúc và kỹ thuật xây dựng cổ. Tập trung vào việc phân tích cấu trúc, vật liệu, kỹ thuật chế tác và ghép đá khổng lồ để xây dựng Thành nhà Hồ. Có hướng dẫn viên am hiểu về lĩnh vực này, giải thích chi tiết về quy trình xây dựng cách đây 600 năm mà không dùng vật liệu kết dính. So sánh kỹ thuật xây dựng với các công trình đá khác trên thế giới.",
                    Schedule_description = @"Ngày 1: Thăm quan Thành nhà Hồ, tập trung phân tích cấu trúc và kỹ thuật tại các cổng và tường thành. Nghe thuyết minh chuyên sâu. Thảo luận.",
                    Category_Id = "CATE002",
                    Duration = "1N0D",
                    Price = 1610000.0m,
                    Max_capacity = 15,
                    Location_Id = "LOC007",
                    Create_at = new DateTime(2023, 10, 1, 9, 30, 0),
                    Update_at = new DateTime(2023, 10, 1, 9, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR044",
                    Name = "Thành nhà Hồ: Lịch sử và Bối cảnh Triều Hồ",
                    Short_description = "Tìm hiểu sâu về triều đại ngắn ngủi và cải cách của nhà Hồ.",
                    Detail_description = @"Tour tập trung vào bối cảnh lịch sử đặc biệt của Thành nhà Hồ và triều đại nhà Hồ. Tìm hiểu về cuộc cải cách của Hồ Quý Ly, sự kiện dời đô từ Thăng Long về Tây Đô, và giai đoạn kháng chiến chống quân Minh. Thăm các điểm di tích phụ cận (nếu có) liên quan đến triều Hồ. Tour mang đến cái nhìn toàn diện về một giai đoạn lịch sử đầy biến động.",
                    Schedule_description = @"Ngày 1: Thăm Thành nhà Hồ, nghe thuyết minh chi tiết về lịch sử triều Hồ và các sự kiện quan trọng. Thăm nhà trưng bày.",
                    Category_Id = "CATE016",
                    Duration = "1N0D",
                    Price = 1495000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC007",
                    Create_at = new DateTime(2024, 2, 28, 10, 0, 0),
                    Update_at = new DateTime(2024, 2, 28, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR045",
                    Name = "Thành nhà Hồ: Ngắm toàn cảnh và chụp ảnh Di sản",
                    Short_description = "Cơ hội chụp ảnh những góc đẹp nhất của Thành nhà Hồ.",
                    Detail_description = @"Tour dành cho những người yêu thích nhiếp ảnh, đưa bạn đến các điểm vantage point xung quanh Thành nhà Hồ để ghi lại những bức ảnh toàn cảnh và chi tiết về kiến trúc đá độc đáo. Có hướng dẫn viên am hiểu về các góc chụp đẹp và thời điểm ánh sáng tốt nhất trong ngày. Phù hợp cho cả người mới bắt đầu và nhiếp ảnh gia chuyên nghiệp muốn ghi lại hình ảnh di sản này.",
                    Schedule_description = @"Ngày 1: Buổi sáng/chiều thăm Thành nhà Hồ, di chuyển đến các điểm chụp ảnh quanh thành (gần cổng Nam, cổng Bắc, khu vực khai quật...).",
                    Category_Id = "CATE019",
                    Duration = "1N0D",
                    Price = 1725000.0m,
                    Max_capacity = 10,
                    Location_Id = "LOC007",
                    Create_at = new DateTime(2023, 11, 5, 9, 0, 0),
                    Update_at = new DateTime(2023, 11, 5, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR046",
                    Name = "Tràng An: Du thuyền khám phá Quần thể danh thắng",
                    Short_description = "Đi thuyền qua hang động và núi đá vôi kỳ vĩ tại Tràng An.",
                    Detail_description = @"Tour khám phá Quần thể danh thắng Tràng An, Di sản Thế giới hỗn hợp đầu tiên của Việt Nam. Ngồi thuyền truyền thống đi qua các hang động tự nhiên, thung lũng ngập nước và các dãy núi đá vôi hùng vĩ. Thăm các đền, chùa cổ kính nằm giữa thiên nhiên. Trải nghiệm không gian yên bình, thơ mộng và vẻ đẹp non nước hữu tình của Tràng An. Có nhiều tuyến thuyền khác nhau để lựa chọn.",
                    Schedule_description = @"Ngày 1: Đến bến thuyền Tràng An, lựa chọn tuyến thuyền, bắt đầu hành trình đi thuyền qua hang động, thung lũng, thăm đền, chùa. Kết thúc tour sau khoảng 3-4 tiếng.",
                    Category_Id = "CATE006",
                    Duration = "1N0D",
                    Price = 2070000.0m,
                    Max_capacity = 50,
                    Location_Id = "LOC008",
                    Create_at = new DateTime(2022, 6, 15, 9, 0, 0),
                    Update_at = new DateTime(2022, 6, 15, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR047",
                    Name = "Tràng An: Kết hợp Bái Đính và Tràng An linh thiêng",
                    Short_description = "Hành trình tâm linh và khám phá di sản thiên nhiên.",
                    Detail_description = @"Tour kết hợp thăm Chùa Bái Đính, quần thể tâm linh lớn nhất Việt Nam với nhiều kỷ lục. Sau đó, khám phá Quần thể danh thắng Tràng An bằng thuyền. Tour mang đến trải nghiệm đa dạng, từ không gian tâm linh trang nghiêm của chùa Bái Đính đến vẻ đẹp thiên nhiên và các đền thờ cổ kính nằm trong lòng Tràng An. Phù hợp cho những người muốn kết hợp du lịch tâm linh và ngắm cảnh.",
                    Schedule_description = @"Ngày 1: Thăm Chùa Bái Đính (đi xe điện). Ăn trưa. Chiều: Đi thuyền khám phá Tràng An. Kết thúc tour vào cuối ngày.",
                    Category_Id = "CATE007",
                    Duration = "1N0D",
                    Price = 2530000.0m,
                    Max_capacity = 45,
                    Location_Id = "LOC008",
                    Create_at = new DateTime(2023, 1, 10, 8, 0, 0),
                    Update_at = new DateTime(2023, 1, 10, 8, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR048",
                    Name = "Tràng An: Đạp xe khám phá vùng lõi di sản",
                    Short_description = "Đạp xe qua các làng quê và cánh đồng lúa quanh Tràng An.",
                    Detail_description = @"Tour đạp xe thư giãn khám phá khu vực ngoại vi và vùng lõi của Quần thể danh thắng Tràng An. Đạp xe qua những con đường làng yên bình, cánh đồng lúa xanh mướt hoặc vàng óng (tùy mùa), men theo chân núi đá vôi. Ngắm nhìn cảnh quan nông thôn thanh bình, giao lưu với người dân địa phương. Tour nhẹ nhàng, phù hợp với mọi lứa tuổi và yêu thiên nhiên.",
                    Schedule_description = @"Ngày 1: Bắt đầu đạp xe từ khu vực gần Tràng An, đi qua các làng quê, cánh đồng, dừng chân chụp ảnh và nghỉ ngơi. Ăn trưa tại nhà hàng địa phương.",
                    Category_Id = "CATE006",
                    Duration = "1N0D",
                    Price = 1955000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC008",
                    Create_at = new DateTime(2024, 4, 1, 9, 0, 0),
                    Update_at = new DateTime(2024, 4, 1, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR049",
                    Name = "Tràng An: Thám hiểm hang Múa và view toàn cảnh",
                    Short_description = "Chinh phục hang Múa và ngắm nhìn toàn cảnh Tràng An từ trên cao.",
                    Detail_description = @"Tour kết hợp khám phá Quần thể danh thắng Tràng An bằng thuyền và chinh phục Hang Múa. Leo bộ lên đỉnh núi Ngọa Long tại Hang Múa để ngắm nhìn toàn cảnh tuyệt đẹp của Tràng An, Tam Cốc và các cánh đồng lúa từ trên cao. Khung cảnh được ví như 'thiên đường sống ảo'. Tour yêu cầu một chút thể lực để leo núi, phù hợp với người yêu thích khám phá và chụp ảnh.",
                    Schedule_description = @"Ngày 1: Buổi sáng đi thuyền khám phá Tràng An. Ăn trưa. Chiều: Di chuyển đến Hang Múa, leo núi Ngọa Long. Kết thúc tour.",
                    Category_Id = "CATE003",
                    Duration = "1N0D",
                    Price = 2300000.0m,
                    Max_capacity = 30,
                    Location_Id = "LOC008",
                    Create_at = new DateTime(2023, 5, 10, 8, 30, 0),
                    Update_at = new DateTime(2023, 5, 10, 8, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR050",
                    Name = "Tràng An: Du lịch sang trọng và nghỉ dưỡng Eco",
                    Short_description = "Trải nghiệm Tràng An đẳng cấp với nghỉ dưỡng và dịch vụ cao cấp.",
                    Detail_description = @"Tour du lịch nghỉ dưỡng cao cấp tại khu vực Tràng An. Lưu trú tại các resort hoặc homestay sinh thái sang trọng ẩn mình giữa thiên nhiên. Trải nghiệm Tràng An bằng thuyền với dịch vụ riêng hoặc nhóm nhỏ. Tận hưởng không gian yên bình, các bữa ăn đặc sản tinh tế, và các hoạt động thư giãn như spa, yoga giữa khung cảnh núi non. Phù hợp cho cặp đôi hoặc gia đình muốn nghỉ dưỡng và tận hưởng sự riêng tư.",
                    Schedule_description = @"Ngày 1: Đến khu nghỉ dưỡng, nhận phòng. Buổi chiều đi thuyền Tràng An theo tuyến riêng. Tối: Ăn tối cao cấp, thư giãn. Ngày 2: Ăn sáng, thư giãn tại resort, có thể tham gia hoạt động nhẹ (yoga/spa). Check-out.",
                    Category_Id = "CATE004",
                    Duration = "2N1D",
                    Price = 16100000.0m,
                    Max_capacity = 10,
                    Location_Id = "LOC008",
                    Create_at = new DateTime(2024, 1, 1, 14, 0, 0),
                    Update_at = new DateTime(2024, 1, 20, 11, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR051",
                    Name = "Đà Nẵng: Nghỉ dưỡng Biển Mỹ Khê và Khám phá Sơn Trà 3 ngày",
                    Short_description = "Tận hưởng biển Đà Nẵng và vẻ đẹp bán đảo Sơn Trà.",
                    Detail_description = @"Hành trình 3 ngày 2 đêm tại Đà Nẵng kết hợp nghỉ dưỡng tại bãi biển Mỹ Khê xinh đẹp và khám phá thiên nhiên bán đảo Sơn Trà. Tham quan Chùa Linh Ứng với tượng Phật Bà Quan Âm uy nghiêm, chinh phục đỉnh Bàn Cờ để ngắm toàn cảnh thành phố. Thưởng thức hải sản tươi ngon và không khí sôi động của thành phố biển. Phù hợp cho gia đình và cặp đôi.",
                    Schedule_description = @"Ngày 1: Đến Đà Nẵng, nhận phòng khách sạn gần biển, tắm biển Mỹ Khê. Tối: Dạo cầu Rồng/Cầu Tình Yêu. Ngày 2: Thăm Bán đảo Sơn Trà: Chùa Linh Ứng, đỉnh Bàn Cờ. Chiều: Tự do tắm biển/mua sắm. Ngày 3: Tham quan Ngũ Hành Sơn (nếu còn thời gian), mua sắm đặc sản, ra sân bay.",
                    Category_Id = "CATE004",
                    Duration = "3N2D",
                    Price = 8740000.0m,
                    Max_capacity = 30,
                    Location_Id = "LOC011",
                    Create_at = new DateTime(2023, 9, 15, 10, 0, 0),
                    Update_at = new DateTime(2023, 10, 1, 11, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR052",
                    Name = "Đà Nẵng: Khám phá Bà Nà Hills và Di sản Hội An 4 ngày",
                    Short_description = "Kết hợp trải nghiệm Bà Nà tiên cảnh và phố cổ Hội An.",
                    Detail_description = @"Tour 4 ngày 3 đêm từ Đà Nẵng, đưa bạn lên cáp treo kỷ lục thế giới để khám phá Bà Nà Hills với Cầu Vàng nổi tiếng, Làng Pháp cổ kính và khu vui chơi Fantasy Park. Sau đó, dành thời gian khám phá Phố cổ Hội An trầm mặc, di sản UNESCO. Dạo bộ, đi thuyền sông Hoài, thưởng thức ẩm thực và tìm hiểu văn hóa địa phương. Tour đa dạng trải nghiệm.",
                    Schedule_description = @"Ngày 1: Đến Đà Nẵng, nhận phòng. Tối: Tự do. Ngày 2: Tham quan Bà Nà Hills cả ngày. Ngày 3: Di chuyển Hội An, thăm phố cổ (Chùa Cầu, nhà cổ, hội quán), đi thuyền/thả đèn hoa đăng. Ngày 4: Buổi sáng Hội An (làng nghề/mua sắm), về Đà Nẵng, ra sân bay.",
                    Category_Id = "CATE002",
                    Duration = "4N3D",
                    Price = 12650000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC011",
                    Create_at = new DateTime(2024, 1, 25, 9, 30, 0),
                    Update_at = new DateTime(2024, 2, 10, 14, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR053",
                    Name = "Đà Nẵng: Hành trình 5 ngày Biển - Núi - Phố cổ",
                    Short_description = "Trải nghiệm toàn diện Đà Nẵng và vùng lân cận.",
                    Detail_description = @"Hành trình 5 ngày 4 đêm khám phá trọn vẹn Đà Nẵng và các điểm lân cận. Bao gồm thời gian thư giãn tại biển, tham quan các danh thắng như Ngũ Hành Sơn, Bán đảo Sơn Trà, trải nghiệm Bà Nà Hills, và đắm mình trong không gian cổ kính của Hội An. Lịch trình cân bằng giữa nghỉ dưỡng, khám phá thiên nhiên, văn hóa và lịch sử. Phù hợp cho những ai muốn có chuyến đi đầy đủ.",
                    Schedule_description = @"Ngày 1: Đến Đà Nẵng, nhận phòng, tắm biển. Ngày 2: Tham quan Ngũ Hành Sơn, Làng đá Non Nước, Bán đảo Sơn Trà. Ngày 3: Tham quan Bà Nà Hills cả ngày. Ngày 4: Di chuyển Hội An, thăm phố cổ, trải nghiệm ẩm thực/làng nghề. Ngày 5: Mua sắm đặc sản Đà Nẵng, ra sân bay.",
                    Category_Id = "CATE010",
                    Duration = "5N4D",
                    Price = 15640000.0m,
                    Max_capacity = 35,
                    Location_Id = "LOC011",
                    Create_at = new DateTime(2023, 7, 1, 11, 0, 0),
                    Update_at = new DateTime(2023, 7, 20, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR054",
                    Name = "Đà Nẵng: Tour Phiêu lưu và Trekking 4 ngày",
                    Short_description = "Thử thách bản thân với các hoạt động mạo hiểm quanh Đà Nẵng.",
                    Detail_description = @"Tour 4 ngày 3 đêm dành cho người yêu thích phiêu lưu. Trekking và khám phá Vườn quốc gia Bạch Mã (gần Đà Nẵng), vượt suối, ngắm cảnh rừng núi hùng vĩ. Trải nghiệm Zipline hoặc Canyoning tại các khu du lịch mạo hiểm. Có thể kết hợp với leo núi Ngũ Hành Sơn hoặc khám phá các hang động tự nhiên (nếu có). Yêu cầu thể lực tốt. Bao gồm hướng dẫn viên chuyên nghiệp và thiết bị an toàn.",
                    Schedule_description = @"Ngày 1: Đến Đà Nẵng, chuẩn bị. Ngày 2: Trekking Vườn quốc gia Bạch Mã (hoặc khu vực khác), cắm trại/nghỉ tại khu vực. Ngày 3: Tham gia Zipline/Canyoning (hoặc hoạt động mạo hiểm khác). Ngày 4: Hoạt động nhẹ (leo Ngũ Hành Sơn) hoặc thư giãn, ra sân bay.",
                    Category_Id = "CATE003",
                    Duration = "4N3D",
                    Price = 18400000.0m,
                    Max_capacity = 15,
                    Location_Id = "LOC011",
                    Create_at = new DateTime(2024, 3, 5, 14, 0, 0),
                    Update_at = new DateTime(2024, 3, 5, 14, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR055",
                    Name = "Đà Nẵng: Tour Nhiếp ảnh Bình minh và Hoàng hôn 3 ngày",
                    Short_description = "Lưu giữ vẻ đẹp Đà Nẵng qua ống kính máy ảnh.",
                    Detail_description = @"Tour 3 ngày 2 đêm dành cho những người đam mê nhiếp ảnh, tập trung vào việc chụp các khoảnh khắc đẹp nhất tại Đà Nẵng và lân cận. Chụp bình minh tại biển Mỹ Khê hoặc Bán đảo Sơn Trà, hoàng hôn tại cầu Rồng hoặc các điểm view đẹp khác. Có thể di chuyển đến Hội An để chụp ảnh phố cổ về đêm. Hướng dẫn viên am hiểu về các điểm chụp ảnh đẹp và thời gian lý tưởng.",
                    Schedule_description = @"Ngày 1: Đến Đà Nẵng, nhận phòng. Chiều: Chụp hoàng hôn cầu Rồng/Sông Hàn. Tối: Chụp ảnh đêm thành phố. Ngày 2: Dậy sớm chụp bình minh. Chiều: Chụp ảnh tại Ngũ Hành Sơn/Sơn Trà. Tối: Tự do hoặc di chuyển Hội An chụp ảnh đèn lồng. Ngày 3: Sáng tự do chụp ảnh, ra sân bay.",
                    Category_Id = "CATE019",
                    Duration = "3N2D",
                    Price = 10350000.0m,
                    Max_capacity = 10,
                    Location_Id = "LOC011",
                    Create_at = new DateTime(2023, 10, 18, 8, 0, 0),
                    Update_at = new DateTime(2023, 10, 18, 8, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR056",
                    Name = "Nha Trang: Thiên đường Biển đảo và Thư giãn 3 ngày",
                    Short_description = "Nghỉ dưỡng, tắm biển và khám phá các đảo nhỏ Nha Trang.",
                    Detail_description = @"Hành trình 3 ngày 2 đêm tận hưởng vẻ đẹp biển Nha Trang. Tham quan các đảo nổi tiếng như Hòn Mun (lặn ngắm san hô), Hòn Tằm (tắm bùn, thư giãn), hoặc các đảo khác tùy chọn. Tắm biển Bãi Dài hoặc bãi biển trung tâm. Thưởng thức hải sản tươi sống. Tour kết hợp giữa nghỉ dưỡng và khám phá các điểm đến đặc trưng của vịnh Nha Trang.",
                    Schedule_description = @"Ngày 1: Đến Nha Trang, nhận phòng, tắm biển. Tối: Dạo phố/Chợ đêm. Ngày 2: Đi thuyền tham quan các đảo (Hòn Mun, Hòn Tằm, Hòn Một...). Hoạt động lặn biển/tắm bùn (tùy chọn). Ngày 3: Thăm Tháp Bà Ponagar, Chợ Đầm (mua sắm), ra sân bay/ga.",
                    Category_Id = "CATE001",
                    Duration = "3N2D",
                    Price = 7360000.0m,
                    Max_capacity = 30,
                    Location_Id = "LOC012",
                    Create_at = new DateTime(2022, 7, 20, 11, 0, 0),
                    Update_at = new DateTime(2022, 7, 20, 11, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR057",
                    Name = "Nha Trang: Du lịch Sinh thái và Suối khoáng nóng 4 ngày",
                    Short_description = "Trải nghiệm thiên nhiên và thư giãn tại các điểm sinh thái Nha Trang.",
                    Detail_description = @"Tour 4 ngày 3 đêm tập trung vào các hoạt động sinh thái và thư giãn. Thăm Khu du lịch Suối khoáng nóng Tháp Bà hoặc I-Resort để tắm bùn, ngâm khoáng phục hồi sức khỏe. Khám phá Đảo Khỉ, Hòn Lao với các loài động vật hoang dã. Có thể kết hợp thăm Vườn quốc gia Hòn Bà (nếu có thời gian). Tour mang đến không gian yên bình và gần gũi với thiên nhiên.",
                    Schedule_description = @"Ngày 1: Đến Nha Trang, nhận phòng. Tối: Thư giãn. Ngày 2: Thăm Đảo Khỉ (đi cáp treo/thuyền). Chiều: Tắm biển/Tự do. Ngày 3: Tham quan Suối khoáng nóng, trải nghiệm tắm bùn/ngâm khoáng. Ngày 4: Mua sắm, ra sân bay/ga.",
                    Category_Id = "CATE006",
                    Duration = "4N3D",
                    Price = 10350000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC012",
                    Create_at = new DateTime(2023, 12, 10, 9, 0, 0),
                    Update_at = new DateTime(2023, 12, 25, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR058",
                    Name = "Nha Trang: Khám phá Vịnh Nha Trang và Hòn Tằm 3 ngày",
                    Short_description = "Đi thuyền khám phá vịnh và nghỉ dưỡng tại Hòn Tằm.",
                    Detail_description = @"Tour 3 ngày 2 đêm tập trung vào việc khám phá Vịnh Nha Trang bằng thuyền. Thăm các hòn đảo đẹp trong vịnh như Hòn Tằm với khu tắm bùn khoáng trên đảo, Hòn Mun với khu bảo tồn biển và lặn ngắm san hô. Tận hưởng các hoạt động dưới nước và nghỉ ngơi trên các đảo. Tour mang đến trải nghiệm biển đảo đặc trưng của Nha Trang.",
                    Schedule_description = @"Ngày 1: Đến Nha Trang, nhận phòng. Chiều: Thăm Hòn Chồng, Chùa Long Sơn. Tối: Tự do. Ngày 2: Đi thuyền tham quan Vịnh: Hòn Mun (lặn/bơi), Hòn Tằm (tắm bùn/thư giãn). Ăn trưa hải sản trên tàu hoặc đảo. Ngày 3: Mua sắm, ra sân bay/ga.",
                    Category_Id = "CATE001",
                    Duration = "3N2D",
                    Price = 8280000.0m,
                    Max_capacity = 30,
                    Location_Id = "LOC012",
                    Create_at = new DateTime(2024, 3, 18, 10, 0, 0),
                    Update_at = new DateTime(2024, 3, 18, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR059",
                    Name = "Nha Trang: Lịch sử và Văn hóa Chăm tại Tháp Bà 3 ngày",
                    Short_description = "Tìm hiểu lịch sử và văn hóa cổ tại Nha Trang.",
                    Detail_description = @"Tour 3 ngày 2 đêm khám phá các di tích lịch sử và văn hóa tại Nha Trang, đặc biệt là di sản của người Chăm. Thăm quần thể Tháp Bà Ponagar cổ kính, tìm hiểu về kiến trúc và tín ngưỡng của Vương quốc Champa xưa. Thăm các điểm văn hóa khác như Chùa Long Sơn với tượng Phật trắng khổng lồ. Tour mang đến góc nhìn sâu sắc về lịch sử và sự đa dạng văn hóa của vùng đất này.",
                    Schedule_description = @"Ngày 1: Đến Nha Trang, nhận phòng. Chiều: Thăm Chùa Long Sơn, Nhà thờ Núi. Tối: Tự do. Ngày 2: Thăm Tháp Bà Ponagar, tìm hiểu lịch sử và văn hóa Chăm. Buổi chiều: Tắm biển/thư giãn. Ngày 3: Mua sắm, ra sân bay/ga.",
                    Category_Id = "CATE016",
                    Duration = "3N2D",
                    Price = 6900000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC012",
                    Create_at = new DateTime(2023, 8, 5, 14, 0, 0),
                    Update_at = new DateTime(2023, 8, 5, 14, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR060",
                    Name = "Nha Trang: Tour Sang trọng 4 ngày tại Resort 5 sao",
                    Short_description = "Trải nghiệm kỳ nghỉ đẳng cấp và dịch vụ hoàn hảo.",
                    Detail_description = @"Tour 4 ngày 3 đêm nghỉ dưỡng tại một trong những resort 5 sao hàng đầu tại Nha Trang hoặc khu vực lân cận (như Cam Ranh). Tận hưởng không gian sang trọng, bãi biển riêng, hồ bơi vô cực, các dịch vụ spa, ẩm thực cao cấp. Tour bao gồm thời gian riêng tư tối đa, phù hợp cho những ai muốn một kỳ nghỉ thư giãn tuyệt đối và đẳng cấp.",
                    Schedule_description = @"Ngày 1: Đến Nha Trang/Cam Ranh, di chuyển đến resort, nhận phòng, thư giãn. Ngày 2: Tự do tận hưởng tiện ích resort (hồ bơi, spa, bãi biển riêng). Ngày 3: Tự do hoặc tham gia hoạt động nhẹ của resort (yoga, thể thao dưới nước). Ngày 4: Thư giãn buổi sáng, check-out, ra sân bay.",
                    Category_Id = "CATE014",
                    Duration = "4N3D",
                    Price = 34500000.0m,
                    Max_capacity = 10,
                    Location_Id = "LOC012",
                    Create_at = new DateTime(2024, 2, 1, 10, 30, 0),
                    Update_at = new DateTime(2024, 2, 1, 10, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR061",
                    Name = "Phú Quốc: Thiên đường Đảo ngọc và Vui chơi 3 ngày",
                    Short_description = "Tận hưởng bãi biển đẹp và các khu vui chơi hiện đại.",
                    Detail_description = @"Hành trình 3 ngày 2 đêm tại Phú Quốc, hòn đảo lớn nhất Việt Nam. Tắm biển tại các bãi biển nổi tiếng như Bãi Sao, Bãi Trường. Tham quan các điểm du lịch như nhà tù Phú Quốc (di tích lịch sử), Dinh Cậu (ngắm hoàng hôn). Trải nghiệm các khu vui chơi giải trí đẳng cấp quốc tế như VinWonders hoặc Sun World Hòn Thơm. Tour phù hợp cho gia đình và nhóm bạn.",
                    Schedule_description = @"Ngày 1: Đến Phú Quốc, nhận phòng, tắm biển Bãi Trường. Tối: Dạo chợ đêm Phú Quốc. Ngày 2: Thăm Nam Đảo: Nhà tù Phú Quốc, Bãi Sao. Chiều: Đi cáp treo Hòn Thơm (tùy chọn). Ngày 3: Thăm Dinh Cậu, Chùa Hộ Quốc, mua sắm đặc sản (nước mắm, tiêu, ngọc trai), ra sân bay.",
                    Category_Id = "CATE001",
                    Duration = "3N2D",
                    Price = 11040000.0m,
                    Max_capacity = 30,
                    Location_Id = "LOC013",
                    Create_at = new DateTime(2023, 11, 18, 11, 0, 0),
                    Update_at = new DateTime(2023, 12, 5, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR062",
                    Name = "Phú Quốc: Khám phá Bắc Đảo và Rừng nguyên sinh 4 ngày",
                    Short_description = "Tìm hiểu thiên nhiên hoang sơ và các điểm du lịch Bắc Đảo.",
                    Detail_description = @"Tour 4 ngày 3 đêm khám phá khu vực Bắc Đảo Phú Quốc còn giữ nét hoang sơ. Thăm Vườn quốc gia Phú Quốc (trekking nhẹ), đi Gành Dầu để ngắm biên giới Campuchia. Thăm Mũi Gành Dầu, đền thờ Nguyễn Trung Trực. Trải nghiệm tắm biển tại Bãi Ông Lang hoặc các bãi biển phía Bắc. Tour phù hợp cho người yêu thiên nhiên và muốn tìm hiểu về phía Bắc Đảo.",
                    Schedule_description = @"Ngày 1: Đến Phú Quốc, nhận phòng. Tối: Tự do. Ngày 2: Thăm Bắc Đảo: Vườn quốc gia (trekking nhẹ), Gành Dầu, đền thờ Nguyễn Trung Trực. Ngày 3: Thăm Bãi Ông Lang, tham gia hoạt động biển (tùy chọn: kayak, SUP). Chiều: Thư giãn. Ngày 4: Mua sắm, ra sân bay.",
                    Category_Id = "CATE006",
                    Duration = "4N3D",
                    Price = 12650000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC013",
                    Create_at = new DateTime(2024, 4, 8, 9, 0, 0),
                    Update_at = new DateTime(2024, 4, 8, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR063",
                    Name = "Phú Quốc: Tour 5 ngày Nghỉ dưỡng và Khám phá 4 đảo",
                    Short_description = "Kỳ nghỉ dài hơn kết hợp nghỉ dưỡng và khám phá các đảo phía Nam.",
                    Detail_description = @"Hành trình 5 ngày 4 đêm thư giãn tại Phú Quốc, kết hợp thời gian nghỉ dưỡng thoải mái và chuyến đi khám phá 4 hòn đảo đẹp phía Nam (Hòn Gầm Ghì, Hòn Móng Tay, Hòn Mây Rút Trong, Hòn Mây Rút Ngoài). Lặn ngắm san hô, bơi lội, chụp ảnh tại các hòn đảo nhỏ hoang sơ. Tour mang đến trải nghiệm đa dạng và đủ thời gian để tận hưởng.",
                    Schedule_description = @"Ngày 1: Đến Phú Quốc, nhận phòng, thư giãn/tắm biển. Ngày 2: Tự do nghỉ dưỡng tại resort/khách sạn. Ngày 3: Tour khám phá 4 đảo Nam Phú Quốc (đi cano), lặn ngắm san hô, bơi lội. Ngày 4: Tự do nghỉ dưỡng hoặc mua sắm/tham quan nhẹ nhàng. Ngày 5: Mua sắm đặc sản, ra sân bay.",
                    Category_Id = "CATE004",
                    Duration = "5N4D",
                    Price = 17250000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC013",
                    Create_at = new DateTime(2023, 6, 20, 10, 0, 0),
                    Update_at = new DateTime(2023, 7, 10, 15, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR064",
                    Name = "Phú Quốc: Tour Sang trọng 4 ngày tại Resort Đẳng cấp",
                    Short_description = "Trải nghiệm kỳ nghỉ xa hoa tại đảo ngọc Phú Quốc.",
                    Detail_description = @"Tour 4 ngày 3 đêm nghỉ dưỡng tại một trong những resort 5 sao hoặc villa biển sang trọng nhất Phú Quốc. Tận hưởng sự riêng tư, dịch vụ hoàn hảo, ẩm thực tinh tế và các tiện ích đẳng cấp. Có thể bao gồm du thuyền riêng ngắm hoàng hôn hoặc các trải nghiệm độc quyền khác. Tour dành cho những người tìm kiếm sự sang trọng, thư giãn tối đa và không gian nghỉ dưỡng biệt lập.",
                    Schedule_description = @"Ngày 1: Đến Phú Quốc, di chuyển đến resort/villa, nhận phòng, thư giãn. Ngày 2: Tự do tận hưởng tiện ích và bãi biển riêng của resort. Ngày 3: Tự do hoặc tham gia hoạt động của resort, có thể đặt bữa tối lãng mạn riêng. Ngày 4: Thư giãn buổi sáng, check-out, ra sân bay.",
                    Category_Id = "CATE014",
                    Duration = "4N3D",
                    Price = 57500000.0m,
                    Max_capacity = 8,
                    Location_Id = "LOC013",
                    Create_at = new DateTime(2024, 1, 5, 14, 30, 0),
                    Update_at = new DateTime(2024, 1, 5, 14, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR065",
                    Name = "Phú Quốc: Tour Ẩm thực và Cuộc sống địa phương 3 ngày",
                    Short_description = "Khám phá hương vị đặc trưng và đời sống dân dã Phú Quốc.",
                    Detail_description = @"Tour 3 ngày 2 đêm tập trung vào trải nghiệm ẩm thực và tìm hiểu cuộc sống địa phương tại Phú Quốc. Thăm các nhà thùng nước mắm truyền thống, vườn tiêu, cơ sở nuôi cấy ngọc trai. Thưởng thức hải sản tươi sống tại làng chài Hàm Ninh hoặc chợ đêm. Tham gia lớp học nấu ăn món Phú Quốc (tùy chọn). Tour mang đến cái nhìn chân thực về văn hóa và đặc sản của đảo.",
                    Schedule_description = @"Ngày 1: Đến Phú Quốc, nhận phòng. Chiều: Thăm nhà thùng nước mắm, vườn tiêu. Tối: Ăn tối hải sản. Ngày 2: Buổi sáng thăm Làng chài Hàm Ninh/Chợ đêm. Chiều: Thăm cơ sở nuôi cấy ngọc trai. Tối: Tự do khám phá ẩm thực. Ngày 3: Mua sắm đặc sản, ra sân bay.",
                    Category_Id = "CATE005",
                    Duration = "3N2D",
                    Price = 9200000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC013",
                    Create_at = new DateTime(2023, 5, 8, 9, 0, 0),
                    Update_at = new DateTime(2023, 5, 8, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR066",
                    Name = "Đà Lạt: Thành phố Ngàn hoa và Cảnh quan 3 ngày",
                    Short_description = "Khám phá vẻ đẹp lãng mạn và kiến trúc độc đáo Đà Lạt.",
                    Detail_description = @"Hành trình 3 ngày 2 đêm khám phá Đà Lạt, thành phố của ngàn hoa và sương mù. Thăm các điểm du lịch nổi tiếng như Hồ Xuân Hương, Thung Lũng Tình Yêu, Vườn hoa Thành phố, Thiền viện Trúc Lâm, Dinh Bảo Đại. Ngắm nhìn kiến trúc Pháp cổ kính, tận hưởng không khí trong lành và lãng mạn. Tour phù hợp cho cặp đôi và gia đình.",
                    Schedule_description = @"Ngày 1: Đến Đà Lạt, nhận phòng. Chiều: Thăm Hồ Xuân Hương, Vườn hoa Thành phố. Tối: Dạo chợ đêm Đà Lạt. Ngày 2: Thăm Dinh Bảo Đại, Thiền viện Trúc Lâm, Hồ Tuyền Lâm. Chiều: Thung Lũng Tình Yêu hoặc Vườn dâu. Ngày 3: Thăm Ga Đà Lạt cổ, Nhà thờ Con Gà, mua sắm đặc sản, ra sân bay/bến xe.",
                    Category_Id = "CATE009",
                    Duration = "3N2D",
                    Price = 6440000.0m,
                    Max_capacity = 35,
                    Location_Id = "LOC014",
                    Create_at = new DateTime(2022, 10, 10, 10, 0, 0),
                    Update_at = new DateTime(2022, 10, 10, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR067",
                    Name = "Đà Lạt: Phiêu lưu Canyoning Vượt thác 3 ngày",
                    Short_description = "Thử thách bản thân với hoạt động Canyoning mạo hiểm.",
                    Detail_description = @"Tour 3 ngày 2 đêm dành cho những người yêu thích mạo hiểm thực sự. Tham gia hoạt động Canyoning (vượt thác bằng dây) tại khu vực thác Datanla hoặc một thác khác phù hợp. Trải nghiệm đi bộ trong rừng, leo vách đá, trượt thác tự nhiên, đu dây qua thác. Tour yêu cầu thể lực và tinh thần thép. Bao gồm hướng dẫn viên chuyên nghiệp, thiết bị an toàn và bảo hiểm.",
                    Schedule_description = @"Ngày 1: Đến Đà Lạt, nhận phòng. Buổi chiều: Chuẩn bị và làm quen với thiết bị. Tối: Nghỉ ngơi. Ngày 2: Tham gia tour Canyoning cả ngày. Tối: Thư giãn, hồi phục. Ngày 3: Buổi sáng tự do hoặc tham quan nhẹ nhàng, ra sân bay/bến xe.",
                    Category_Id = "CATE003",
                    Duration = "3N2D",
                    Price = 13800000.0m,
                    Max_capacity = 12,
                    Location_Id = "LOC014",
                    Create_at = new DateTime(2024, 3, 10, 9, 0, 0),
                    Update_at = new DateTime(2024, 3, 10, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR068",
                    Name = "Đà Lạt: Tour Ẩm thực và Nông nghiệp Công nghệ cao 4 ngày",
                    Short_description = "Khám phá nông nghiệp hiện đại và ẩm thực Đà Lạt.",
                    Detail_description = @"Tour 4 ngày 3 đêm tìm hiểu về nông nghiệp công nghệ cao phát triển mạnh mẽ tại Đà Lạt và thưởng thức ẩm thực địa phương. Thăm các trang trại trồng dâu, rau sạch, hoa theo mô hình hiện đại. Tìm hiểu quy trình sản xuất và thưởng thức sản phẩm tươi ngon. Tham gia các tour ẩm thực đường phố hoặc lớp nấu ăn (tùy chọn). Tour kết hợp giữa nông nghiệp và ẩm thực.",
                    Schedule_description = @"Ngày 1: Đến Đà Lạt, nhận phòng. Chiều: Thăm Vườn dâu/Trang trại rau sạch. Tối: Thăm chợ đêm, thử các món ăn đường phố. Ngày 2: Thăm Khu du lịch nông nghiệp công nghệ cao (nếu có). Buổi chiều: Thăm Vườn hoa Vạn Thành/Làng hoa Thái Phiên. Ngày 3: Thăm đồi chè Cầu Đất (nếu có thời gian), Nhà máy chè cổ. Chiều: Tự do mua sắm/thư giãn. Ngày 4: Mua sắm đặc sản, ra sân bay/bến xe.",
                    Category_Id = "CATE015",
                    Duration = "4N3D",
                    Price = 8740000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC014",
                    Create_at = new DateTime(2023, 5, 20, 10, 30, 0),
                    Update_at = new DateTime(2023, 6, 1, 14, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR069",
                    Name = "Đà Lạt: Tour Lãng mạn cho Cặp đôi 3 ngày",
                    Short_description = "Không gian ngọt ngào và trải nghiệm lãng mạn tại Đà Lạt.",
                    Detail_description = @"Thiết kế riêng cho các cặp đôi, tour 3 ngày 2 đêm mang đến không gian lãng mạn và những trải nghiệm đáng nhớ tại Đà Lạt. Lưu trú tại homestay/villa lãng mạn, dạo quanh Hồ Xuân Hương, Thung Lũng Tình Yêu, ngắm hoàng hôn từ đồi thông. Có thể bao gồm bữa tối lãng mạn, chụp ảnh cưới (pre-wedding) hoặc các hoạt động riêng tư khác. Tour tập trung vào sự lãng mạn và riêng tư.",
                    Schedule_description = @"Ngày 1: Đến Đà Lạt, nhận phòng homestay/villa lãng mạn. Chiều: Dạo Hồ Xuân Hương, Quảng trường Lâm Viên. Tối: Ăn tối lãng mạn. Ngày 2: Thăm Thung Lũng Tình Yêu/Đồi thông 2 mộ (tùy chọn), Cafe với view đẹp. Chiều: Tự do hoặc chụp ảnh lãng mạn. Ngày 3: Mua sắm quà, ra sân bay/bến xe.",
                    Category_Id = "CATE012",
                    Duration = "3N2D",
                    Price = 16100000.0m,
                    Max_capacity = 10,
                    Location_Id = "LOC014",
                    Create_at = new DateTime(2023, 11, 1, 8, 0, 0),
                    Update_at = new DateTime(2023, 11, 1, 8, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR070",
                    Name = "Đà Lạt: Trekking Rừng và Thác nước 4 ngày",
                    Short_description = "Khám phá thiên nhiên hoang sơ qua các cung đường trekking.",
                    Detail_description = @"Tour 4 ngày 3 đêm dành cho những người yêu thích trekking và khám phá thiên nhiên. Đi bộ xuyên qua các khu rừng thông bạt ngàn, vượt qua các con suối và ghé thăm các thác nước hùng vĩ ít người biết đến quanh Đà Lạt. Có thể bao gồm cắm trại qua đêm trong rừng hoặc nghỉ tại homestay bản địa. Tour yêu cầu thể lực tốt. Bao gồm hướng dẫn viên địa phương am hiểu địa hình và hệ sinh thái.",
                    Schedule_description = @"Ngày 1: Đến Đà Lạt, chuẩn bị. Ngày 2: Bắt đầu cung trekking (ví dụ: Bidoup Núi Bà hoặc khu vực khác), ăn trưa picnic, cắm trại/nghỉ đêm. Ngày 3: Tiếp tục trekking, khám phá thác nước, về lại điểm xuất phát hoặc nghỉ tại bản. Ngày 4: Thư giãn, mua sắm (nếu còn thời gian), ra sân bay/bến xe.",
                    Category_Id = "CATE003",
                    Duration = "4N3D",
                    Price = 20700000.0m,
                    Max_capacity = 15,
                    Location_Id = "LOC014",
                    Create_at = new DateTime(2024, 4, 20, 9, 30, 0),
                    Update_at = new DateTime(2024, 4, 20, 9, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR071",
                    Name = "Sa Pa: Trekking Bản làng và Văn hóa dân tộc 3 ngày",
                    Short_description = "Đi bộ qua các bản làng, tìm hiểu đời sống người dân tộc.",
                    Detail_description = @"Hành trình 3 ngày 2 đêm trekking qua các bản làng nổi tiếng của Sa Pa như Cát Cát, Lao Chải, Tả Van, Bản Hồ. Ngắm nhìn những thửa ruộng bậc thang hùng vĩ, khám phá phong tục tập quán, trang phục truyền thống và đời sống sinh hoạt của các dân tộc thiểu số (H'Mông, Dao, Tày...). Nghỉ đêm tại homestay bản địa để trải nghiệm chân thực. Tour văn hóa và sinh thái.",
                    Schedule_description = @"Ngày 1: Đến Sa Pa, nhận phòng. Chiều: Trekking bản Cát Cát. Tối: Thăm thị trấn Sa Pa. Ngày 2: Trekking Lao Chải - Tả Van, ngắm ruộng bậc thang, nghỉ đêm tại homestay bản Tả Van. Ngày 3: Trekking từ Tả Van về, thăm bản khác (nếu có), về Sa Pa, mua sắm, ra ga/bến xe.",
                    Category_Id = "CATE003",
                    Duration = "3N2D",
                    Price = 8050000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC015",
                    Create_at = new DateTime(2023, 9, 1, 10, 0, 0),
                    Update_at = new DateTime(2023, 9, 1, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR072",
                    Name = "Sa Pa: Chinh phục Fansipan và Ngắm cảnh 4 ngày",
                    Short_description = "Trải nghiệm đỉnh Fansipan và cảnh sắc Sa Pa hùng vĩ.",
                    Detail_description = @"Tour 4 ngày 3 đêm kết hợp chinh phục Fansipan - 'Nóc nhà Đông Dương' (bằng cáp treo hoặc trekking - tùy chọn tour cụ thể) và khám phá cảnh quan Sa Pa. Ngắm nhìn toàn cảnh dãy Hoàng Liên Sơn và thung lũng Mường Hoa từ trên cao. Thăm các điểm như Hàm Rồng, Nhà thờ đá. Có thời gian nghỉ ngơi và tận hưởng không khí núi rừng.",
                    Schedule_description = @"Ngày 1: Đến Sa Pa, nhận phòng. Chiều: Thăm Núi Hàm Rồng, Nhà thờ đá. Tối: Tự do. Ngày 2: Chinh phục Fansipan (đi cáp treo). Ngày 3: Trekking nhẹ nhàng đến thung lũng Mường Hoa, ngắm cảnh ruộng bậc thang. Ngày 4: Mua sắm tại chợ Sa Pa, ra ga/bến xe.",
                    Category_Id = "CATE003",
                    Duration = "4N3D",
                    Price = 14950000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC015",
                    Create_at = new DateTime(2024, 1, 12, 9, 0, 0),
                    Update_at = new DateTime(2024, 1, 12, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR073",
                    Name = "Sa Pa: Tour Chụp ảnh Mùa vàng Ruộng bậc thang 3 ngày",
                    Short_description = "Ghi lại khoảnh khắc tuyệt đẹp của ruộng bậc thang Sa Pa.",
                    Detail_description = @"Tour 3 ngày 2 đêm đặc biệt dành cho người yêu nhiếp ảnh vào mùa lúa chín (khoảng tháng 9-10). Đưa bạn đến những góc chụp đẹp nhất của ruộng bậc thang tại thung lũng Mường Hoa, Lao Chải, Tả Van, hoặc xa hơn như Y Tý (nếu tour nâng cao). Chụp ảnh bình minh/hoàng hôn trên ruộng, hình ảnh người dân địa phương làm việc. Hướng dẫn viên am hiểu về địa điểm và ánh sáng chụp ảnh.",
                    Schedule_description = @"Ngày 1: Đến Sa Pa, nhận phòng. Chiều: Chụp ảnh tại thung lũng Mường Hoa. Tối: Chụp ảnh thị trấn về đêm. Ngày 2: Dậy sớm di chuyển đến Lao Chải/Tả Van để chụp bình minh ruộng bậc thang. Chiều: Tiếp tục chụp ảnh tại các điểm view khác. Ngày 3: Sáng chụp ảnh, mua sắm, ra ga/bến xe.",
                    Category_Id = "CATE019",
                    Duration = "3N2D",
                    Price = 11040000.0m,
                    Max_capacity = 10,
                    Location_Id = "LOC015",
                    Create_at = new DateTime(2023, 9, 10, 8, 30, 0),
                    Update_at = new DateTime(2023, 9, 10, 8, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR074",
                    Name = "Sa Pa: Kết hợp Bản làng và chợ phiên vùng cao 4 ngày",
                    Short_description = "Khám phá văn hóa đa dạng qua các chợ phiên độc đáo.",
                    Detail_description = @"Tour 4 ngày 3 đêm kết hợp trekking nhẹ nhàng qua các bản làng quanh Sa Pa với việc tham quan các chợ phiên nổi tiếng của các dân tộc vùng cao như chợ phiên Bắc Hà (Chủ Nhật), chợ phiên Cốc Ly (Thứ Ba), hoặc chợ phiên khác tùy ngày khởi hành. Trải nghiệm không khí tấp nập, độc đáo của chợ phiên, mua sắm thổ cẩm, đồ thủ công và thưởng thức ẩm thực địa phương. Tour văn hóa sâu sắc.",
                    Schedule_description = @"Ngày 1: Đến Sa Pa, nhận phòng. Chiều: Thăm bản Cát Cát. Tối: Tự do. Ngày 2: Di chuyển tham quan chợ phiên (phù hợp với ngày trong tuần). Ngày 3: Trekking nhẹ nhàng đến Lao Chải - Tả Van, thăm bản, nghỉ ngơi. Ngày 4: Về lại Sa Pa, mua sắm, ra ga/bến xe.",
                    Category_Id = "CATE002",
                    Duration = "4N3D",
                    Price = 9660000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC015",
                    Create_at = new DateTime(2023, 4, 5, 10, 0, 0),
                    Update_at = new DateTime(2023, 4, 5, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR075",
                    Name = "Sa Pa: Nghỉ dưỡng Sang trọng giữa núi rừng 3 ngày",
                    Short_description = "Thư giãn và tận hưởng không khí trong lành tại Sa Pa.",
                    Detail_description = @"Tour 3 ngày 2 đêm nghỉ dưỡng tại các resort hoặc khách sạn boutique cao cấp tại Sa Pa hoặc khu vực thung lũng Mường Hoa. Tận hưởng không gian yên tĩnh, view núi non hùng vĩ và ruộng bậc thang tuyệt đẹp. Dịch vụ spa, ẩm thực tinh tế, và các hoạt động nhẹ nhàng như yoga, thiền, tản bộ quanh khu nghỉ dưỡng. Phù hợp cho những ai muốn một kỳ nghỉ thư thái và sang trọng giữa thiên nhiên.",
                    Schedule_description = @"Ngày 1: Đến Sa Pa, di chuyển đến resort, nhận phòng, thư giãn. Ngày 2: Tự do tận hưởng tiện ích resort, có thể tham gia tản bộ nhẹ nhàng hoặc spa. Ngày 3: Thư giãn buổi sáng, check-out, ra ga/bến xe.",
                    Category_Id = "CATE004",
                    Duration = "3N2D",
                    Price = 18400000.0m,
                    Max_capacity = 10,
                    Location_Id = "LOC015",
                    Create_at = new DateTime(2024, 2, 20, 14, 0, 0),
                    Update_at = new DateTime(2024, 2, 20, 14, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR076",
                    Name = "Mũi Né: Biển Xanh, Cát Đỏ và Resort 3 ngày",
                    Short_description = "Tận hưởng bãi biển và khám phá đồi cát đặc trưng Mũi Né.",
                    Detail_description = @"Hành trình 3 ngày 2 đêm tại Mũi Né, Phan Thiết. Thư giãn tại bãi biển, tắm nắng, bơi lội. Khám phá Đồi Cát Đỏ (đồi Cát Bay) và Đồi Cát Trắng (tiểu sa mạc Sahara Việt Nam), trải nghiệm trượt cát hoặc đi xe Jeep/ATV. Thăm Làng Chài Mũi Né, Suối Tiên (Suối Hồng). Tour kết hợp nghỉ dưỡng biển và khám phá cảnh quan độc đáo.",
                    Schedule_description = @"Ngày 1: Đến Mũi Né, nhận phòng resort, tắm biển. Tối: Dạo phố. Ngày 2: Sáng sớm ngắm bình minh Đồi Cát Trắng, trải nghiệm trượt cát/xe địa hình. Buổi chiều: Thăm Suối Tiên, Làng chài, ngắm hoàng hôn Đồi Cát Đỏ. Ngày 3: Thư giãn, mua sắm đặc sản (nước mắm, thanh long), ra ga/bến xe.",
                    Category_Id = "CATE001",
                    Duration = "3N2D",
                    Price = 6900000.0m,
                    Max_capacity = 30,
                    Location_Id = "LOC016",
                    Create_at = new DateTime(2022, 8, 25, 10, 0, 0),
                    Update_at = new DateTime(2022, 8, 25, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR077",
                    Name = "Mũi Né: Tour Thể thao Biển và Nghỉ dưỡng 4 ngày",
                    Short_description = "Trải nghiệm lướt ván diều/buồm và thư giãn tại Mũi Né.",
                    Detail_description = @"Tour 4 ngày 3 đêm dành cho những người yêu thích thể thao dưới nước. Mũi Né là điểm đến lý tưởng cho lướt ván diều (kitesurfing) và lướt ván buồm (windsurfing). Tham gia các buổi học cơ bản hoặc nâng cao với hướng dẫn viên chuyên nghiệp. Bên cạnh đó là thời gian thư giãn tại resort, tận hưởng không khí biển trong lành. Tour kết hợp thể thao và nghỉ dưỡng.",
                    Schedule_description = @"Ngày 1: Đến Mũi Né, nhận phòng. Chiều: Làm quen với bãi biển, chuẩn bị. Ngày 2: Tham gia buổi học lướt ván (sáng/chiều tùy điều kiện gió). Ngày 3: Tiếp tục luyện tập lướt ván hoặc tự do tắm biển/thư giãn. Ngày 4: Thư giãn buổi sáng, mua sắm, ra ga/bến xe.",
                    Category_Id = "CATE008",
                    Duration = "4N3D",
                    Price = 16100000.0m,
                    Max_capacity = 15,
                    Location_Id = "LOC016",
                    Create_at = new DateTime(2023, 3, 1, 9, 30, 0),
                    Update_at = new DateTime(2023, 3, 1, 9, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR078",
                    Name = "Mũi Né: Khám phá Đồi cát và Bàu Trắng 3 ngày",
                    Short_description = "Du ngoạn trên những cồn cát trắng trải dài như sa mạc.",
                    Detail_description = @"Tour 3 ngày 2 đêm tập trung khám phá khu vực Bàu Trắng và Đồi Cát Trắng rộng lớn, được mệnh danh là 'Tiểu sa mạc Sahara của Việt Nam'. Ngắm cảnh Bàu Sen (hồ sen giữa đồi cát), trải nghiệm đi xe địa hình (ATV) chinh phục cồn cát. Có thể kết hợp thăm Hải đăng Kê Gà (gần đó) hoặc các điểm du lịch khác của Phan Thiết. Tour thiên nhiên và phiêu lưu nhẹ.",
                    Schedule_description = @"Ngày 1: Đến Mũi Né, nhận phòng. Chiều: Thăm Đồi Cát Đỏ, Làng chài. Tối: Tự do. Ngày 2: Sáng sớm di chuyển Bàu Trắng/Đồi Cát Trắng, trải nghiệm xe địa hình, ngắm cảnh Bàu Sen. Buổi chiều: Thư giãn tại resort/tắm biển. Ngày 3: Thư giãn, mua sắm, ra ga/bến xe.",
                    Category_Id = "CATE006",
                    Duration = "3N2D",
                    Price = 8050000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC016",
                    Create_at = new DateTime(2024, 4, 12, 10, 0, 0),
                    Update_at = new DateTime(2024, 4, 12, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR079",
                    Name = "Mũi Né: Nghỉ dưỡng Sang trọng và Golf 4 ngày",
                    Short_description = "Kỳ nghỉ đẳng cấp kết hợp chơi golf và thư giãn biển.",
                    Detail_description = @"Tour 4 ngày 3 đêm dành cho những người yêu thích golf và nghỉ dưỡng sang trọng. Lưu trú tại các resort cao cấp có sân golf riêng hoặc gần sân golf nổi tiếng. Dành thời gian chơi golf trên sân tiêu chuẩn quốc tế, sau đó thư giãn tại resort, tận hưởng bãi biển và các dịch vụ spa. Tour kết hợp thể thao, nghỉ dưỡng và sang trọng.",
                    Schedule_description = @"Ngày 1: Đến Mũi Né, nhận phòng resort, thư giãn. Ngày 2: Chơi golf tại sân golf (sáng), chiều thư giãn tại resort. Ngày 3: Chơi golf (tùy chọn) hoặc tự do tận hưởng tiện ích resort, tắm biển. Ngày 4: Thư giãn buổi sáng, check-out, ra ga/bến xe.",
                    Category_Id = "CATE014",
                    Duration = "4N3D",
                    Price = 41400000.0m,
                    Max_capacity = 10,
                    Location_Id = "LOC016",
                    Create_at = new DateTime(2023, 10, 25, 14, 30, 0),
                    Update_at = new DateTime(2023, 10, 25, 14, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR080",
                    Name = "Mũi Né: Tour Ẩm thực Hải sản và Văn hóa Chăm 3 ngày",
                    Short_description = "Thưởng thức hải sản và tìm hiểu văn hóa Chăm Bình Thuận.",
                    Detail_description = @"Tour 3 ngày 2 đêm kết hợp trải nghiệm ẩm thực hải sản tươi ngon tại Mũi Né và tìm hiểu về văn hóa của đồng bào Chăm tại tỉnh Bình Thuận. Thăm Làng Chài Mũi Né, thưởng thức các món hải sản đặc trưng. Ghé thăm Tháp Po Sah Inu, một di tích Chăm cổ nằm trên đồi Bà Nài. Tìm hiểu về phong tục, tín ngưỡng và nghệ thuật của người Chăm. Tour kết hợp ẩm thực và văn hóa.",
                    Schedule_description = @"Ngày 1: Đến Mũi Né, nhận phòng. Chiều: Thăm Tháp Po Sah Inu, tìm hiểu văn hóa Chăm. Tối: Thưởng thức hải sản tại Làng chài hoặc nhà hàng. Ngày 2: Thư giãn tại bãi biển. Buổi chiều: Thăm Đồi Cát Đỏ, Suối Tiên. Ngày 3: Mua sắm đặc sản, ra ga/bến xe.",
                    Category_Id = "CATE005",
                    Duration = "3N2D",
                    Price = 7360000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC016",
                    Create_at = new DateTime(2024, 1, 1, 9, 0, 0),
                    Update_at = new DateTime(2024, 1, 1, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR081",
                    Name = "Cần Thơ: Chợ nổi Cái Răng và Cuộc sống Sông nước 3 ngày",
                    Short_description = "Trải nghiệm chợ nổi sầm uất và đời sống bình dị miền Tây.",
                    Detail_description = @"Hành trình 3 ngày 2 đêm khám phá Cần Thơ, thủ phủ miền Tây sông nước. Đi thuyền tham quan Chợ nổi Cái Răng - một trong những chợ nổi lớn nhất Đồng bằng sông Cửu Long vào buổi sáng sớm. Thăm các làng nghề truyền thống ven sông như làm hủ tiếu, làm cốm. Điền thuyền qua các kênh rạch nhỏ, thăm vườn cây ăn trái, tìm hiểu về cuộc sống và văn hóa của người dân miền Tây.",
                    Schedule_description = @"Ngày 1: Đến Cần Thơ, nhận phòng. Chiều: Thăm Chùa Ông, Bến Ninh Kiều. Tối: Dạo bến Ninh Kiều/Cầu Cần Thơ. Ngày 2: Sáng sớm đi thuyền chợ nổi Cái Răng, thăm làng nghề hủ tiếu/cốm. Chiều: Thăm Vườn cây ăn trái (Mỹ Khánh hoặc khác), nghe đờn ca tài tử (tùy chọn). Ngày 3: Mua sắm đặc sản, ra sân bay/bến xe.",
                    Category_Id = "CATE017",
                    Duration = "3N2D",
                    Price = 6440000.0m,
                    Max_capacity = 35,
                    Location_Id = "LOC017",
                    Create_at = new DateTime(2023, 7, 15, 8, 30, 0),
                    Update_at = new DateTime(2023, 7, 15, 8, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR082",
                    Name = "Cần Thơ: Du lịch Sinh thái Cồn Sơn và Miệt vườn 3 ngày",
                    Short_description = "Khám phá Cồn Sơn và trải nghiệm sinh thái miệt vườn.",
                    Detail_description = @"Tour 3 ngày 2 đêm tập trung vào du lịch sinh thái tại Cần Thơ, đặc biệt là khu vực Cồn Sơn - một cù lao nhỏ yên bình trên sông Hậu. Khám phá Cồn Sơn với mô hình du lịch cộng đồng, trải nghiệm làm bánh xèo, xem cá lóc bay, thăm vườn trái cây. Kết hợp thăm các vườn trái cây lớn khác hoặc khu du lịch sinh thái khác quanh Cần Thơ. Tour mang đến không gian yên bình, gần gũi thiên nhiên.",
                    Schedule_description = @"Ngày 1: Đến Cần Thơ, nhận phòng. Chiều: Thăm Bến Ninh Kiều. Tối: Tự do. Ngày 2: Đi thuyền ra Cồn Sơn, trải nghiệm các hoạt động sinh thái (làm bánh, xem cá bay, thăm vườn). Ăn trưa tại Cồn Sơn. Chiều: Về lại đất liền, thư giãn. Ngày 3: Thăm Chợ Cần Thơ, mua sắm, ra sân bay/bến xe.",
                    Category_Id = "CATE006",
                    Duration = "3N2D",
                    Price = 7360000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC017",
                    Create_at = new DateTime(2024, 4, 28, 9, 0, 0),
                    Update_at = new DateTime(2024, 4, 28, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR083",
                    Name = "Cần Thơ: Tour Ẩm thực Miền Tây 4 ngày",
                    Short_description = "Hành trình khám phá hương vị độc đáo của ẩm thực miền Tây.",
                    Detail_description = @"Tour 4 ngày 3 đêm dành cho tín đồ ẩm thực, tập trung vào khám phá và trải nghiệm các món ăn đặc sắc của miền Tây tại Cần Thơ và các tỉnh lân cận (nếu có thời gian di chuyển). Thăm các khu chợ truyền thống, tìm hiểu về nguyên liệu và cách chế biến. Tham gia các lớp học nấu ăn món miền Tây, thưởng thức các món đặc sản như lẩu mắm, bánh xèo, nem nướng, trái cây. Tour ẩm thực chuyên sâu.",
                    Schedule_description = @"Ngày 1: Đến Cần Thơ, nhận phòng. Chiều/Tối: Khám phá ẩm thực đường phố Cần Thơ. Ngày 2: Sáng sớm đi chợ nổi (thử món ăn trên thuyền), thăm làng nghề (hủ tiếu, bánh hỏi). Trưa: Ăn trưa món đặc sản. Chiều: Thăm vườn cây trái (thưởng thức tại vườn). Tối: Thưởng thức lẩu mắm hoặc món khác. Ngày 3: Tham gia lớp học nấu ăn món miền Tây. Chiều: Tự do khám phá thêm. Ngày 4: Mua sắm đặc sản ẩm thực, ra sân bay/bến xe.",
                    Category_Id = "CATE005",
                    Duration = "4N3D",
                    Price = 10350000.0m,
                    Max_capacity = 15,
                    Location_Id = "LOC017",
                    Create_at = new DateTime(2023, 11, 10, 10, 0, 0),
                    Update_at = new DateTime(2023, 11, 10, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR084",
                    Name = "Cần Thơ: Nghỉ dưỡng Homestay Sông nước 3 ngày",
                    Short_description = "Trải nghiệm cuộc sống dân dã và yên bình tại homestay.",
                    Detail_description = @"Tour 3 ngày 2 đêm trải nghiệm nghỉ dưỡng tại các homestay ven sông tại Cần Thơ hoặc các cù lao yên bình. Hòa mình vào cuộc sống của người dân địa phương, tham gia các hoạt động hàng ngày như đi chợ nổi, bơi xuồng trong kênh rạch, bắt cá (tùy homestay), học làm các món ăn đơn giản. Tận hưởng không khí trong lành, yên tĩnh và sự hiếu khách của người miền Tây. Phù hợp cho người muốn tìm kiếm sự bình yên và trải nghiệm văn hóa địa phương chân thực.",
                    Schedule_description = @"Ngày 1: Đến Cần Thơ, di chuyển đến homestay, nhận phòng, nghỉ ngơi. Chiều: Đi bộ/đạp xe quanh làng. Tối: Ăn tối cùng gia đình chủ nhà, nghe kể chuyện. Ngày 2: Sáng sớm đi thuyền chợ nổi. Buổi sáng: Tham gia hoạt động tại homestay/vườn (làm vườn, câu cá). Chiều: Đi thuyền khám phá kênh rạch xung quanh. Ngày 3: Ăn sáng, thư giãn, chia tay gia đình chủ nhà, ra sân bay/bến xe.",
                    Category_Id = "CATE004",
                    Duration = "3N2D",
                    Price = 8050000.0m,
                    Max_capacity = 15,
                    Location_Id = "LOC017",
                    Create_at = new DateTime(2024, 1, 20, 14, 0, 0),
                    Update_at = new DateTime(2024, 1, 20, 14, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR085",
                    Name = "Cần Thơ: Tour 4 ngày Kết hợp Cần Thơ - Sóc Trăng",
                    Short_description = "Khám phá Cần Thơ và văn hóa Khmer tại Sóc Trăng.",
                    Detail_description = @"Tour 4 ngày 3 đêm kết hợp khám phá Cần Thơ và mở rộng hành trình đến Sóc Trăng để tìm hiểu về văn hóa đặc sắc của đồng bào Khmer. Tại Cần Thơ, thăm chợ nổi, miệt vườn. Tại Sóc Trăng, thăm các chùa Khmer nổi tiếng (Chùa Dơi, Chùa Đất Sét, Chùa Chén Kiểu), tìm hiểu về kiến trúc và đời sống văn hóa của người Khmer. Tour đa dạng về văn hóa và cảnh quan miền Tây.",
                    Schedule_description = @"Ngày 1: Đến Cần Thơ, thăm Bến Ninh Kiều. Ngày 2: Sáng sớm đi chợ nổi Cái Răng, thăm làng nghề. Chiều: Thăm Vườn cây ăn trái. Ngày 3: Di chuyển Sóc Trăng, thăm các chùa Khmer nổi tiếng. Chiều: Thăm Bảo tàng Khmer (tùy chọn). Tối: Về lại Cần Thơ hoặc nghỉ đêm Sóc Trăng (tùy tour). Ngày 4: Mua sắm đặc sản, ra sân bay/bến xe.",
                    Category_Id = "CATE002",
                    Duration = "4N3D",
                    Price = 9200000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC017",
                    Create_at = new DateTime(2023, 9, 25, 10, 30, 0),
                    Update_at = new DateTime(2023, 9, 25, 10, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR086",
                    Name = "Buôn Ma Thuột: Thủ phủ Cà phê và Các Thác nước 3 ngày",
                    Short_description = "Khám phá vùng đất đỏ Bazan và hương vị cà phê.",
                    Detail_description = @"Hành trình 3 ngày 2 đêm khám phá Buôn Ma Thuột, thủ phủ cà phê của Việt Nam. Thăm Bảo tàng Cà phê Thế giới, các vườn cà phê (vào mùa), tìm hiểu quy trình trồng trọt và chế biến. Ghé thăm các thác nước hùng vĩ nổi tiếng như Thác Dray Nur, Thác Dray Sáp. Tìm hiểu về văn hóa các dân tộc thiểu số Tây Nguyên. Tour kết hợp nông nghiệp, sinh thái và văn hóa.",
                    Schedule_description = @"Ngày 1: Đến Buôn Ma Thuột, nhận phòng. Chiều: Thăm Bảo tàng Cà phê Thế giới, thưởng thức cà phê. Tối: Tự do. Ngày 2: Thăm các thác nước: Dray Nur, Dray Sáp (có thể đi cầu treo). Buổi chiều: Thăm Làng cà phê Trung Nguyên hoặc một vườn cà phê. Ngày 3: Thăm Chùa Sắc Tứ Khải Đoan, mua sắm đặc sản (cà phê, bơ...), ra sân bay.",
                    Category_Id = "CATE006",
                    Duration = "3N2D",
                    Price = 6900000.0m,
                    Max_capacity = 30,
                    Location_Id = "LOC018",
                    Create_at = new DateTime(2023, 8, 12, 11, 0, 0),
                    Update_at = new DateTime(2023, 8, 12, 11, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR087",
                    Name = "Buôn Ma Thuột: Văn hóa Cồng Chiêng và Bản làng Tây Nguyên 4 ngày",
                    Short_description = "Tìm hiểu sâu về văn hóa đặc sắc của các dân tộc Tây Nguyên.",
                    Detail_description = @"Tour 4 ngày 3 đêm tập trung vào khám phá văn hóa của các dân tộc thiểu số tại Tây Nguyên, xung quanh Buôn Ma Thuột. Thăm các buôn làng của người Ê Đê, M'Nông. Tìm hiểu về nhà sàn, kiến trúc, phong tục tập quán, trang phục truyền thống. Đặc biệt, trải nghiệm Không gian Văn hóa Cồng Chiêng Tây Nguyên (Di sản UNESCO) qua các buổi biểu diễn và giao lưu. Tour văn hóa chuyên sâu và trải nghiệm.",
                    Schedule_description = @"Ngày 1: Đến Buôn Ma Thuột, nhận phòng. Chiều: Thăm Bảo tàng Dân tộc Đắk Lắk. Tối: Tự do. Ngày 2: Thăm Buôn Đôn (cầu treo, nhà cổ, mộ Vua Voi). Buổi chiều: Thăm một buôn làng khác, tìm hiểu về nhà dài, nghề dệt thổ cẩm. Ngày 3: Trải nghiệm Không gian Văn hóa Cồng Chiêng (biểu diễn cồng chiêng, ăn cơm lam, uống rượu cần). Ngày 4: Mua sắm đặc sản, ra sân bay.",
                    Category_Id = "CATE002",
                    Duration = "4N3D",
                    Price = 9660000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC018",
                    Create_at = new DateTime(2024, 3, 8, 9, 30, 0),
                    Update_at = new DateTime(2024, 3, 8, 9, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR088",
                    Name = "Buôn Ma Thuột: Phiêu lưu Vườn quốc gia và Thác nước 3 ngày",
                    Short_description = "Trekking, khám phá hệ sinh thái đa dạng của Tây Nguyên.",
                    Detail_description = @"Tour 3 ngày 2 đêm dành cho người yêu thích phiêu lưu và khám phá thiên nhiên. Trekking trong Vườn quốc gia Yok Đôn (gần Buôn Ma Thuột), tìm hiểu về hệ thực vật và động vật (có thể thấy Voi hoang dã nếu may mắn). Khám phá các thác nước hùng vĩ và hoang sơ. Có thể bao gồm cắm trại trong rừng hoặc ngủ tại trạm kiểm lâm. Tour yêu cầu thể lực tốt. Bao gồm hướng dẫn viên vườn quốc gia.",
                    Schedule_description = @"Ngày 1: Đến Buôn Ma Thuột, chuẩn bị. Ngày 2: Di chuyển đến Vườn quốc gia Yok Đôn, bắt đầu trekking, thăm thác nước trong vườn, cắm trại/nghỉ đêm. Ngày 3: Tiếp tục trekking nhẹ nhàng hoặc tham gia hoạt động khác (như đạp xe), về lại Buôn Ma Thuột, ra sân bay.",
                    Category_Id = "CATE003",
                    Duration = "3N2D",
                    Price = 16100000.0m,
                    Max_capacity = 15,
                    Location_Id = "LOC018",
                    Create_at = new DateTime(2023, 10, 5, 10, 0, 0),
                    Update_at = new DateTime(2023, 10, 5, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR089",
                    Name = "Buôn Ma Thuột: Nghỉ dưỡng Cao nguyên và Cà phê 3 ngày",
                    Short_description = "Thư giãn và tận hưởng không khí trong lành trên cao nguyên.",
                    Detail_description = @"Tour 3 ngày 2 đêm nghỉ dưỡng tại các resort hoặc homestay yên tĩnh giữa vườn cà phê hoặc gần các thác nước tại Buôn Ma Thuột. Tận hưởng không gian trong lành, yên bình của cao nguyên. Thăm các quán cà phê đẹp và độc đáo, thưởng thức hương vị cà phê Tây Nguyên nguyên bản. Tour phù hợp cho người muốn tìm kiếm sự thư giãn và không gian yên tĩnh.",
                    Schedule_description = @"Ngày 1: Đến Buôn Ma Thuột, di chuyển đến khu nghỉ dưỡng, nhận phòng, thư giãn. Chiều: Thăm một quán cà phê đẹp. Ngày 2: Thư giãn tại khu nghỉ dưỡng hoặc tham gia hoạt động nhẹ nhàng (tản bộ vườn cà phê, thăm thác gần đó). Tối: Thưởng thức ẩm thực địa phương. Ngày 3: Thư giãn buổi sáng, check-out, ra sân bay.",
                    Category_Id = "CATE004",
                    Duration = "3N2D",
                    Price = 11500000.0m,
                    Max_capacity = 15,
                    Location_Id = "LOC018",
                    Create_at = new DateTime(2024, 2, 18, 14, 0, 0),
                    Update_at = new DateTime(2024, 2, 18, 14, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR090",
                    Name = "Buôn Ma Thuột: Lịch sử Chiến tranh và Văn hóa 3 ngày",
                    Short_description = "Tìm hiểu về lịch sử và các di tích chiến tranh tại Tây Nguyên.",
                    Detail_description = @"Tour 3 ngày 2 đêm tập trung vào việc tìm hiểu lịch sử chiến tranh Việt Nam qua các di tích tại Buôn Ma Thuột và khu vực Tây Nguyên. Thăm Bảo tàng Chiến thắng Buôn Ma Thuột, các cứ điểm lịch sử quan trọng (nếu có). Kết hợp thăm các buôn làng để tìm hiểu về cuộc sống của người dân trong chiến tranh và hiện tại. Tour mang tính giáo dục và lịch sử sâu sắc.",
                    Schedule_description = @"Ngày 1: Đến Buôn Ma Thuột, nhận phòng. Chiều: Thăm Bảo tàng Chiến thắng Buôn Ma Thuột. Tối: Tự do. Ngày 2: Thăm một số di tích lịch sử (nếu có) hoặc thăm buôn làng gần đó để tìm hiểu về ảnh hưởng của chiến tranh. Ngày 3: Thư giãn, mua sắm, ra sân bay.",
                    Category_Id = "CATE016",
                    Duration = "3N2D",
                    Price = 6440000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC018",
                    Create_at = new DateTime(2023, 6, 10, 10, 30, 0),
                    Update_at = new DateTime(2023, 6, 10, 10, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR091",
                    Name = "Vũng Tàu: Biển và Các Điểm tham quan 3 ngày",
                    Short_description = "Tận hưởng biển và khám phá các điểm nổi bật Vũng Tàu.",
                    Detail_description = @"Hành trình 3 ngày 2 đêm tại thành phố biển Vũng Tàu. Thư giãn tại bãi biển (Bãi Trước, Bãi Sau). Thăm Tượng Chúa Kitô Vua trên đỉnh núi Nhỏ, Hải đăng Vũng Tàu, Bạch Dinh, Chùa Thích Ca Phật Đài. Thưởng thức hải sản tươi ngon. Tour phù hợp cho gia đình và nhóm bạn muốn có chuyến đi biển ngắn ngày gần TP. Hồ Chí Minh.",
                    Schedule_description = @"Ngày 1: Đến Vũng Tàu, nhận phòng, tắm biển Bãi Sau. Tối: Dạo Bãi Trước. Ngày 2: Sáng leo Tượng Chúa Kitô Vua. Chiều: Thăm Hải đăng Vũng Tàu, Bạch Dinh. Tối: Thưởng thức hải sản. Ngày 3: Thăm Chùa Thích Ca Phật Đài, mua sắm, về lại.",
                    Category_Id = "CATE001",
                    Duration = "3N2D",
                    Price = 5750000.0m,
                    Max_capacity = 40,
                    Location_Id = "LOC019",
                    Create_at = new DateTime(2022, 12, 15, 11, 0, 0),
                    Update_at = new DateTime(2022, 12, 15, 11, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR092",
                    Name = "Vũng Tàu: Nghỉ dưỡng Resort và Vui chơi 3 ngày",
                    Short_description = "Thư giãn tại resort và các khu vui chơi Vũng Tàu.",
                    Detail_description = @"Tour 3 ngày 2 đêm tập trung vào nghỉ dưỡng và vui chơi tại Vũng Tàu. Lưu trú tại các resort ven biển, tận hưởng tiện ích hồ bơi, bãi biển riêng. Tham gia các hoạt động giải trí tại Hồ Mây Park (trên núi Lớn) với khu vui chơi, công viên động vật. Tour phù hợp cho gia đình có trẻ nhỏ hoặc những người muốn kết hợp thư giãn và giải trí.",
                    Schedule_description = @"Ngày 1: Đến Vũng Tàu, nhận phòng resort, thư giãn/tắm biển. Ngày 2: Cả ngày vui chơi tại Hồ Mây Park (lên bằng cáp treo). Tối: Tự do. Ngày 3: Thư giãn buổi sáng tại resort, check-out, mua sắm, về lại.",
                    Category_Id = "CATE004",
                    Duration = "3N2D",
                    Price = 9200000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC019",
                    Create_at = new DateTime(2023, 4, 18, 9, 0, 0),
                    Update_at = new DateTime(2023, 4, 18, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR093",
                    Name = "Vũng Tàu: Tour Ẩm thực Hải sản và Lịch sử 3 ngày",
                    Short_description = "Thưởng thức hải sản và tìm hiểu lịch sử Vũng Tàu.",
                    Detail_description = @"Tour 3 ngày 2 đêm kết hợp trải nghiệm ẩm thực hải sản và tìm hiểu lịch sử của Vũng Tàu. Khám phá các khu chợ hải sản, nhà hàng địa phương để thưởng thức các món ăn tươi ngon. Thăm các di tích lịch sử như Bạch Dinh (Dinh Toàn quyền Đông Dương cũ), Bảo tàng tỉnh Bà Rịa - Vũng Tàu, Tượng đài Chiến thắng (nếu có). Tour kết hợp ẩm thực, lịch sử và văn hóa.",
                    Schedule_description = @"Ngày 1: Đến Vũng Tàu, nhận phòng. Chiều/Tối: Khám phá ẩm thực hải sản Vũng Tàu. Ngày 2: Thăm Bạch Dinh, Bảo tàng tỉnh (tùy chọn), Tượng đài Chiến thắng. Chiều: Thư giãn/tắm biển. Ngày 3: Thăm chợ Vũng Tàu mua sắm hải sản/đặc sản, về lại.",
                    Category_Id = "CATE005",
                    Duration = "3N2D",
                    Price = 6440000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC019",
                    Create_at = new DateTime(2024, 3, 25, 10, 0, 0),
                    Update_at = new DateTime(2024, 3, 25, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR094",
                    Name = "Vũng Tàu: Tour 4 ngày Nghỉ dưỡng Cao cấp",
                    Short_description = "Trải nghiệm kỳ nghỉ sang trọng tại các khu nghỉ dưỡng Vũng Tàu.",
                    Detail_description = @"Tour 4 ngày 3 đêm nghỉ dưỡng tại các resort hoặc villa biển cao cấp tại Vũng Tàu hoặc khu vực lân cận (như Hồ Tràm). Tận hưởng không gian riêng tư, dịch vụ hoàn hảo, hồ bơi riêng, spa, và các tiện ích sang trọng khác. Tour tập trung vào sự thư giãn tuyệt đối và trải nghiệm nghỉ dưỡng đẳng cấp bên bờ biển.",
                    Schedule_description = @"Ngày 1: Đến Vũng Tàu/Hồ Tràm, di chuyển đến resort, nhận phòng, thư giãn. Ngày 2: Tự do tận hưởng tiện ích resort (hồ bơi, bãi biển riêng, spa). Ngày 3: Tự do hoặc tham gia hoạt động nhẹ nhàng của resort (yoga, gym). Ngày 4: Thư giãn buổi sáng, check-out, về lại.",
                    Category_Id = "CATE014",
                    Duration = "4N3D",
                    Price = 27600000.0m,
                    Max_capacity = 10,
                    Location_Id = "LOC019",
                    Create_at = new DateTime(2023, 11, 28, 14, 0, 0),
                    Update_at = new DateTime(2023, 11, 28, 14, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR095",
                    Name = "Vũng Tàu: Tour Tâm linh và Cảnh quan 3 ngày",
                    Short_description = "Khám phá các điểm tâm linh và cảnh đẹp thiên nhiên Vũng Tàu.",
                    Detail_description = @"Tour 3 ngày 2 đêm kết hợp thăm các điểm tâm linh nổi tiếng của Vũng Tàu và ngắm nhìn cảnh quan biển núi. Thăm Tượng Chúa Kitô Vua, Chùa Thích Ca Phật Đài, Niết Bàn Tịnh Xá, Chùa Quan Âm. Leo núi Nhỏ hoặc núi Lớn để ngắm cảnh toàn thành phố và biển. Tour phù hợp với những người muốn kết hợp du lịch tâm linh và ngắm cảnh thiên nhiên.",
                    Schedule_description = @"Ngày 1: Đến Vũng Tàu, nhận phòng. Chiều: Thăm Tượng Chúa Kitô Vua. Tối: Dạo Bãi Trước. Ngày 2: Thăm Chùa Thích Ca Phật Đài, Niết Bàn Tịnh Xá, Chùa Quan Âm. Buổi chiều: Thư giãn/tắm biển. Ngày 3: Leo Hải đăng Vũng Tàu, mua sắm, về lại.",
                    Category_Id = "CATE007",
                    Duration = "3N2D",
                    Price = 5980000.0m,
                    Max_capacity = 30,
                    Location_Id = "LOC019",
                    Create_at = new DateTime(2024, 1, 15, 8, 30, 0),
                    Update_at = new DateTime(2024, 1, 15, 8, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR096",
                    Name = "Côn Đảo: Hành trình Lịch sử và Tâm linh 3 ngày",
                    Short_description = "Tìm hiểu về quá khứ và viếng thăm các di tích Côn Đảo.",
                    Detail_description = @"Hành trình 3 ngày 2 đêm tại Côn Đảo, tìm hiểu về lịch sử bi tráng của nơi đây. Thăm hệ thống nhà tù Côn Đảo, Bảo tàng Côn Đảo, Dinh Chúa Đảo. Viếng Nghĩa trang Hàng Dương và mộ Cô Sáu (Võ Thị Sáu). Tour mang tính giáo dục và tâm linh sâu sắc. Bao gồm vé tham quan các di tích chính.",
                    Schedule_description = @"Ngày 1: Đến Côn Đảo, nhận phòng. Chiều: Thăm Dinh Chúa Đảo, trại tù Phú Hải, Phú Sơn. Tối: Tự do/thăm Nghĩa trang Hàng Dương. Ngày 2: Sáng sớm viếng mộ Cô Sáu (nếu muốn). Thăm trại tù Chuồng Cọp kiểu Pháp/Mỹ. Chiều: Thăm Bảo tàng Côn Đảo. Ngày 3: Thư giãn/mua sắm đặc sản, ra sân bay.",
                    Category_Id = "CATE016",
                    Duration = "3N2D",
                    Price = 11500000.0m,
                    Max_capacity = 25,
                    Location_Id = "LOC020",
                    Create_at = new DateTime(2023, 8, 28, 10, 0, 0),
                    Update_at = new DateTime(2023, 8, 28, 10, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR097",
                    Name = "Côn Đảo: Sinh thái Biển, Rừng và Rùa biển 4 ngày",
                    Short_description = "Khám phá thiên nhiên hoang sơ và đa dạng sinh học Côn Đảo.",
                    Detail_description = @"Tour 4 ngày 3 đêm khám phá vẻ đẹp sinh thái của Côn Đảo. Trekking nhẹ trong Vườn quốc gia Côn Đảo để khám phá rừng nguyên sinh. Đi thuyền tham quan các đảo nhỏ như Hòn Bảy Cạnh (khu bảo tồn rùa biển, nếu vào mùa sinh sản có thể xem rùa đẻ trứng/thả rùa con), Hòn Tre. Lặn ngắm san hô tại các rạn san hô đẹp. Tour phù hợp cho người yêu thiên nhiên và môi trường.",
                    Schedule_description = @"Ngày 1: Đến Côn Đảo, nhận phòng. Chiều: Thư giãn/tắm biển bãi An Hải. Tối: Tự do. Ngày 2: Trekking Vườn quốc gia Côn Đảo (đường Mũi Chim Chim hoặc Bãi Ông Đụng). Ngày 3: Đi thuyền tham quan Hòn Bảy Cạnh (thăm trạm kiểm lâm, xem rùa - tùy mùa), lặn ngắm san hô. Ngày 4: Thư giãn, mua sắm đặc sản, ra sân bay.",
                    Category_Id = "CATE006",
                    Duration = "4N3D",
                    Price = 18400000.0m,
                    Max_capacity = 20,
                    Location_Id = "LOC020",
                    Create_at = new DateTime(2024, 4, 10, 9, 0, 0),
                    Update_at = new DateTime(2024, 4, 10, 9, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR098",
                    Name = "Côn Đảo: Nghỉ dưỡng Sang trọng tại Đảo 3 ngày",
                    Short_description = "Trải nghiệm kỳ nghỉ đẳng cấp và riêng tư tại Côn Đảo.",
                    Detail_description = @"Tour 3 ngày 2 đêm nghỉ dưỡng tại các resort 5 sao biệt lập trên Côn Đảo. Tận hưởng không gian sang trọng, yên bình, bãi biển riêng tuyệt đẹp và dịch vụ hoàn hảo. Các bữa ăn tinh tế, spa cao cấp, và các hoạt động thư giãn. Phù hợp cho cặp đôi hoặc những người tìm kiếm một kỳ nghỉ xa hoa và tách biệt khỏi thế giới bên ngoài.",
                    Schedule_description = @"Ngày 1: Đến Côn Đảo, di chuyển đến resort, nhận phòng, thư giãn. Ngày 2: Tự do tận hưởng tiện ích resort (hồ bơi, bãi biển riêng, spa, ẩm thực). Ngày 3: Thư giãn buổi sáng, check-out, ra sân bay.",
                    Category_Id = "CATE014",
                    Duration = "3N2D",
                    Price = 64400000.0m,
                    Max_capacity = 8,
                    Location_Id = "LOC020",
                    Create_at = new DateTime(2023, 12, 20, 14, 0, 0),
                    Update_at = new DateTime(2023, 12, 20, 14, 0, 0)
                },
                new Tour
                {
                    Id = "TOUR099",
                    Name = "Côn Đảo: Lặn biển và Khám phá Đảo nhỏ 4 ngày",
                    Short_description = "Khám phá thế giới dưới nước và các hòn đảo hoang sơ Côn Đảo.",
                    Detail_description = @"Tour 4 ngày 3 đêm dành cho những người yêu thích lặn biển và khám phá các hòn đảo nhỏ xung quanh Côn Đảo. Tham gia các buổi lặn (scuba diving hoặc snorkeling) tại các điểm có rạn san hô đẹp và đa dạng sinh vật biển. Đi thuyền khám phá Hòn Tài, Hòn Cau, Hòn Tre (nếu có thời gian), tìm hiểu về hệ sinh thái biển. Tour yêu cầu chứng chỉ lặn (cho scuba) hoặc phù hợp với người bơi tốt (snorkeling). Bao gồm thiết bị lặn và hướng dẫn viên.",
                    Schedule_description = @"Ngày 1: Đến Côn Đảo, nhận phòng. Chiều: Chuẩn bị và làm quen (hoặc lặn buổi chiều). Tối: Tự do. Ngày 2: Đi thuyền ra các điểm lặn (Hòn Bảy Cạnh, Hòn Tài...), lặn/snorkeling. Ngày 3: Tiếp tục lặn/snorkeling tại các điểm khác hoặc thăm đảo nhỏ. Ngày 4: Thư giãn, mua sắm, ra sân bay.",
                    Category_Id = "CATE008",
                    Duration = "4N3D",
                    Price = 27600000.0m,
                    Max_capacity = 15,
                    Location_Id = "LOC020",
                    Create_at = new DateTime(2024, 5, 1, 9, 30, 0),
                    Update_at = new DateTime(2024, 5, 1, 9, 30, 0)
                },
                new Tour
                {
                    Id = "TOUR100",
                    Name = "Côn Đảo: Du lịch Sức khỏe và Thiền định 3 ngày",
                    Short_description = "Tìm kiếm sự bình yên và cân bằng tại Côn Đảo.",
                    Detail_description = @"Tour 3 ngày 2 đêm tập trung vào du lịch sức khỏe và tinh thần tại Côn Đảo yên bình. Tham gia các buổi tập yoga, thiền định tại những địa điểm có khung cảnh thiên nhiên đẹp (ven biển, trên đồi). Thưởng thức ẩm thực lành mạnh, các liệu pháp spa thư giãn. Dành thời gian tản bộ trong rừng, bơi lội trong không gian tĩnh lặng. Tour phù hợp cho những người muốn tái tạo năng lượng và tìm kiếm sự bình yên.",
                    Schedule_description = @"Ngày 1: Đến Côn Đảo, nhận phòng. Chiều: Buổi tập yoga/thiền đầu tiên. Tối: Ăn tối lành mạnh, thư giãn. Ngày 2: Buổi sáng tập yoga/thiền. Buổi chiều: Tản bộ trong rừng hoặc bơi lội, spa (tùy chọn). Tối: Thiền định/nghỉ ngơi sớm. Ngày 3: Buổi tập cuối, ăn sáng, check-out, ra sân bay.",
                    Category_Id = "CATE020",
                    Duration = "3N2D",
                    Price = 21850000.0m,
                    Max_capacity = 10,
                    Location_Id = "LOC020",
                    Create_at = new DateTime(2023, 7, 28, 14, 0, 0),
                    Update_at = new DateTime(2023, 7, 28, 14, 0, 0)
                }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = "CUS001",
                    Name = "Nguyễn Văn A",
                    DateOfBirth = new DateOnly(1995, 5, 15),
                    Gender = 1,
                    Email = "customer1@gmail.com",
                    Phone = "0912345678"
                }
             );

            modelBuilder.Entity<Schedule>().HasData(
                new Schedule
                {
                    Id = "SCHE001",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 7084400.0m,
                    Children_price = 4959080.0m,
                    Discount = 12,
                    Create_at = new DateTime(2024, 3, 20, 14, 30, 15),
                    Tour_Id = "TOUR001"
                },
                new Schedule
                {
                    Id = "SCHE002",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 0,
                    Status = 0,
                    Adult_price = 8050000.0m,
                    Children_price = 5635000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 1, 10, 9, 0, 0),
                    Tour_Id = "TOUR001"
                },
                new Schedule
                {
                    Id = "SCHE003",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 8,
                    Status = 1,
                    Adult_price = 6037500.0m,
                    Children_price = 4226250.0m,
                    Discount = 25,
                    Create_at = new DateTime(2024, 6, 1, 11, 15, 45),
                    Tour_Id = "TOUR001"
                },
                new Schedule
                {
                    Id = "SCHE004",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 20,
                    Status = 1,
                    Adult_price = 6601000.0m,
                    Children_price = 4620700.0m,
                    Discount = 18,
                    Create_at = new DateTime(2025, 4, 20, 16, 0, 0),
                    Tour_Id = "TOUR001"
                },
                new Schedule
                {
                    Id = "SCHE005",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 3,
                    Status = 1,
                    Adult_price = 5635000.0m,
                    Children_price = 3944500.0m,
                    Discount = 30,
                    Create_at = new DateTime(2024, 10, 15, 8, 0, 0),
                    Tour_Id = "TOUR001"
                },
                new Schedule
                {
                    Id = "SCHE006",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 25,
                    Status = 1,
                    Adult_price = 10764000.0m,
                    Children_price = 7534800.0m,
                    Discount = 10,
                    Create_at = new DateTime(2024, 2, 1, 10, 0, 0),
                    Tour_Id = "TOUR002"
                },
                new Schedule
                {
                    Id = "SCHE007",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 10,
                    Status = 1,
                    Adult_price = 10990000.0m,
                    Children_price = 7693000.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 5, 18, 15, 0, 0),
                    Tour_Id = "TOUR002"
                },
                new Schedule
                {
                    Id = "SCHE008",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 8372000.0m,
                    Children_price = 5860400.0m,
                    Discount = 30,
                    Create_at = new DateTime(2025, 3, 11, 9, 30, 0),
                    Tour_Id = "TOUR002"
                },
                new Schedule
                {
                    Id = "SCHE009",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 18,
                    Status = 1,
                    Adult_price = 10166000.0m,
                    Children_price = 7116200.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 8, 22, 11, 0, 0),
                    Tour_Id = "TOUR002"
                },
                new Schedule
                {
                    Id = "SCHE010",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 7176000.0m,
                    Children_price = 5023200.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 1, 29, 17, 0, 0),
                    Tour_Id = "TOUR002"
                },
                new Schedule
                {
                    Id = "SCHE011",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 30,
                    Status = 1,
                    Adult_price = 4015800.0m,
                    Children_price = 2811060.0m,
                    Discount = 3,
                    Create_at = new DateTime(2024, 1, 5, 10, 0, 0),
                    Tour_Id = "TOUR003"
                },
                new Schedule
                {
                    Id = "SCHE012",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 3436200.0m,
                    Children_price = 2405340.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 4, 30, 14, 0, 0),
                    Tour_Id = "TOUR003"
                },
                new Schedule
                {
                    Id = "SCHE013",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 3229200.0m,
                    Children_price = 2260440.0m,
                    Discount = 22,
                    Create_at = new DateTime(2024, 7, 10, 9, 0, 0),
                    Tour_Id = "TOUR003"
                },
                new Schedule
                {
                    Id = "SCHE014",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 25,
                    Status = 1,
                    Adult_price = 3891600.0m,
                    Children_price = 2724120.0m,
                    Discount = 6,
                    Create_at = new DateTime(2025, 2, 14, 11, 30, 0),
                    Tour_Id = "TOUR003"
                },
                new Schedule
                {
                    Id = "SCHE015",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 8,
                    Status = 1,
                    Adult_price = 2732400.0m,
                    Children_price = 1912680.0m,
                    Discount = 34,
                    Create_at = new DateTime(2024, 11, 20, 16, 45, 0),
                    Tour_Id = "TOUR003"
                },
                new Schedule
                {
                    Id = "SCHE016",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 22,
                    Status = 1,
                    Adult_price = 8128200.0m,
                    Children_price = 5689740.0m,
                    Discount = 7,
                    Create_at = new DateTime(2024, 3, 1, 9, 10, 0),
                    Tour_Id = "TOUR004"
                },
                new Schedule
                {
                    Id = "SCHE017",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 7429000.0m,
                    Children_price = 5200300.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 6, 25, 14, 0, 0),
                    Tour_Id = "TOUR004"
                },
                new Schedule
                {
                    Id = "SCHE018",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 6992000.0m,
                    Children_price = 4894400.0m,
                    Discount = 20,
                    Create_at = new DateTime(2025, 1, 20, 10, 0, 0),
                    Tour_Id = "TOUR004"
                },
                new Schedule
                {
                    Id = "SCHE019",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 8740000.0m,
                    Children_price = 6118000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 5, 16, 30, 0),
                    Tour_Id = "TOUR004"
                },
                new Schedule
                {
                    Id = "SCHE020",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 10,
                    Status = 1,
                    Adult_price = 6118800.0m,
                    Children_price = 4283160.0m,
                    Discount = 30,
                    Create_at = new DateTime(2025, 4, 1, 8, 45, 0),
                    Tour_Id = "TOUR004"
                },
                new Schedule
                {
                    Id = "SCHE021",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 8372000.0m,
                    Children_price = 5860400.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 4, 10, 11, 0, 0),
                    Tour_Id = "TOUR005"
                },
                new Schedule
                {
                    Id = "SCHE022",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 7,
                    Status = 1,
                    Adult_price = 7636000.0m,
                    Children_price = 5345200.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 7, 3, 15, 0, 0),
                    Tour_Id = "TOUR005"
                },
                new Schedule
                {
                    Id = "SCHE023",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 6624000.0m,
                    Children_price = 4636800.0m,
                    Discount = 28,
                    Create_at = new DateTime(2025, 2, 28, 9, 0, 0),
                    Tour_Id = "TOUR005"
                },
                new Schedule
                {
                    Id = "SCHE024",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 9200000.0m,
                    Children_price = 6440000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 18, 14, 30, 0),
                    Tour_Id = "TOUR005"
                },
                new Schedule
                {
                    Id = "SCHE025",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 4,
                    Status = 1,
                    Adult_price = 5520000.0m,
                    Children_price = 3864000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 3, 19, 10, 0, 0),
                    Tour_Id = "TOUR005"
                },
                new Schedule
                {
                    Id = "SCHE026",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 35,
                    Status = 1,
                    Adult_price = 3312000.0m,
                    Children_price = 2318400.0m,
                    Discount = 4,
                    Create_at = new DateTime(2024, 1, 25, 10, 0, 0),
                    Tour_Id = "TOUR006"
                },
                new Schedule
                {
                    Id = "SCHE027",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 2794500.0m,
                    Children_price = 1956150.0m,
                    Discount = 19,
                    Create_at = new DateTime(2024, 5, 1, 14, 0, 0),
                    Tour_Id = "TOUR006"
                },
                new Schedule
                {
                    Id = "SCHE028",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 2656500.0m,
                    Children_price = 1859550.0m,
                    Discount = 23,
                    Create_at = new DateTime(2024, 8, 10, 9, 30, 0),
                    Tour_Id = "TOUR006"
                },
                new Schedule
                {
                    Id = "SCHE029",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 28,
                    Status = 1,
                    Adult_price = 3174000.0m,
                    Children_price = 2221800.0m,
                    Discount = 8,
                    Create_at = new DateTime(2025, 3, 5, 11, 0, 0),
                    Tour_Id = "TOUR006"
                },
                new Schedule
                {
                    Id = "SCHE030",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 10,
                    Status = 1,
                    Adult_price = 2208000.0m,
                    Children_price = 1545600.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 12, 1, 16, 0, 0),
                    Tour_Id = "TOUR006"
                },
                new Schedule
                {
                    Id = "SCHE031",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 20,
                    Status = 1,
                    Adult_price = 3684600.0m,
                    Children_price = 2579220.0m,
                    Discount = 11,
                    Create_at = new DateTime(2024, 2, 14, 10, 30, 0),
                    Tour_Id = "TOUR007"
                },
                new Schedule
                {
                    Id = "SCHE032",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 3477600.0m,
                    Children_price = 2434320.0m,
                    Discount = 16,
                    Create_at = new DateTime(2024, 5, 20, 15, 0, 0),
                    Tour_Id = "TOUR007"
                },
                new Schedule
                {
                    Id = "SCHE033",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 3146400.0m,
                    Children_price = 2202480.0m,
                    Discount = 24,
                    Create_at = new DateTime(2024, 8, 5, 9, 0, 0),
                    Tour_Id = "TOUR007"
                },
                new Schedule
                {
                    Id = "SCHE034",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 4140000.0m,
                    Children_price = 2898000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 3, 28, 11, 45, 0),
                    Tour_Id = "TOUR007"
                },
                new Schedule
                {
                    Id = "SCHE035",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 2610000.0m,
                    Children_price = 1827000.0m,
                    Discount = 37,
                    Create_at = new DateTime(2024, 10, 3, 17, 0, 0),
                    Tour_Id = "TOUR007"
                },
                new Schedule
                {
                    Id = "SCHE036",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 30,
                    Status = 1,
                    Adult_price = 2930200.0m,
                    Children_price = 2051140.0m,
                    Discount = 2,
                    Create_at = new DateTime(2024, 1, 10, 10, 0, 0),
                    Tour_Id = "TOUR008"
                },
                new Schedule
                {
                    Id = "SCHE037",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 18,
                    Status = 1,
                    Adult_price = 2421900.0m,
                    Children_price = 1695330.0m,
                    Discount = 19,
                    Create_at = new DateTime(2024, 4, 22, 14, 30, 0),
                    Tour_Id = "TOUR008"
                },
                new Schedule
                {
                    Id = "SCHE038",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 2202600.0m,
                    Children_price = 1541820.0m,
                    Discount = 26,
                    Create_at = new DateTime(2024, 7, 20, 9, 0, 0),
                    Tour_Id = "TOUR008"
                },
                new Schedule
                {
                    Id = "SCHE039",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 25,
                    Status = 1,
                    Adult_price = 2750800.0m,
                    Children_price = 1925560.0m,
                    Discount = 8,
                    Create_at = new DateTime(2025, 2, 1, 11, 0, 0),
                    Tour_Id = "TOUR008"
                },
                new Schedule
                {
                    Id = "SCHE040",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 7,
                    Status = 1,
                    Adult_price = 1853800.0m,
                    Children_price = 1297660.0m,
                    Discount = 38,
                    Create_at = new DateTime(2024, 11, 5, 16, 0, 0),
                    Tour_Id = "TOUR008"
                },
                new Schedule
                {
                    Id = "SCHE041",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 10,
                    Status = 1,
                    Adult_price = 9950000.0m,
                    Children_price = 6965000.0m,
                    Discount = 13,
                    Create_at = new DateTime(2024, 3, 8, 10, 30, 0),
                    Tour_Id = "TOUR009"
                },
                new Schedule
                {
                    Id = "SCHE042",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 3,
                    Status = 1,
                    Adult_price = 10810000.0m,
                    Children_price = 7567000.0m,
                    Discount = 6,
                    Create_at = new DateTime(2024, 6, 18, 15, 0, 0),
                    Tour_Id = "TOUR009"
                },
                new Schedule
                {
                    Id = "SCHE043",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 8165000.0m,
                    Children_price = 5715500.0m,
                    Discount = 28,
                    Create_at = new DateTime(2025, 1, 14, 9, 30, 0),
                    Tour_Id = "TOUR009"
                },
                new Schedule
                {
                    Id = "SCHE044",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 10350000.0m,
                    Children_price = 7245000.0m,
                    Discount = 10,
                    Create_at = new DateTime(2024, 9, 1, 14, 0, 0),
                    Tour_Id = "TOUR009"
                },
                new Schedule
                {
                    Id = "SCHE045",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 1,
                    Status = 1,
                    Adult_price = 7590000.0m,
                    Children_price = 5313000.0m,
                    Discount = 34,
                    Create_at = new DateTime(2025, 4, 25, 17, 30, 0),
                    Tour_Id = "TOUR009"
                },
                new Schedule
                {
                    Id = "SCHE046",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 28,
                    Status = 1,
                    Adult_price = 4370000.0m,
                    Children_price = 3059000.0m,
                    Discount = 5,
                    Create_at = new DateTime(2024, 1, 30, 10, 0, 0),
                    Tour_Id = "TOUR010"
                },
                new Schedule
                {
                    Id = "SCHE047",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 3772000.0m,
                    Children_price = 2640400.0m,
                    Discount = 18,
                    Create_at = new DateTime(2024, 5, 15, 14, 0, 0),
                    Tour_Id = "TOUR010"
                },
                new Schedule
                {
                    Id = "SCHE048",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 3634000.0m,
                    Children_price = 2543800.0m,
                    Discount = 21,
                    Create_at = new DateTime(2024, 8, 8, 9, 0, 0),
                    Tour_Id = "TOUR010"
                },
                new Schedule
                {
                    Id = "SCHE049",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 20,
                    Status = 1,
                    Adult_price = 4600000.0m,
                    Children_price = 3220000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 3, 14, 11, 30, 0),
                    Tour_Id = "TOUR010"
                },
                new Schedule
                {
                    Id = "SCHE050",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 2944000.0m,
                    Children_price = 2060800.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 11, 10, 16, 0, 0),
                    Tour_Id = "TOUR010"
                },
                new Schedule
                {
                    Id = "SCHE051",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 40,
                    Status = 1,
                    Adult_price = 5587500.0m,
                    Children_price = 3911250.0m,
                    Discount = 3,
                    Create_at = new DateTime(2024, 1, 15, 10, 0, 0),
                    Tour_Id = "TOUR011"
                },
                new Schedule
                {
                    Id = "SCHE052",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 20,
                    Status = 1,
                    Adult_price = 4772500.0m,
                    Children_price = 3340750.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 4, 28, 14, 30, 0),
                    Tour_Id = "TOUR011"
                },
                new Schedule
                {
                    Id = "SCHE053",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 4485000.0m,
                    Children_price = 3139500.0m,
                    Discount = 22,
                    Create_at = new DateTime(2024, 7, 25, 9, 0, 0),
                    Tour_Id = "TOUR011"
                },
                new Schedule
                {
                    Id = "SCHE054",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 30,
                    Status = 1,
                    Adult_price = 5405000.0m,
                    Children_price = 3783500.0m,
                    Discount = 6,
                    Create_at = new DateTime(2025, 2, 8, 11, 0, 0),
                    Tour_Id = "TOUR011"
                },
                new Schedule
                {
                    Id = "SCHE055",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 3502500.0m,
                    Children_price = 2451750.0m,
                    Discount = 39,
                    Create_at = new DateTime(2024, 12, 10, 16, 0, 0),
                    Tour_Id = "TOUR011"
                },
                new Schedule
                {
                    Id = "SCHE056",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 25,
                    Status = 1,
                    Adult_price = 3726000.0m,
                    Children_price = 2608200.0m,
                    Discount = 10,
                    Create_at = new DateTime(2024, 2, 20, 10, 30, 0),
                    Tour_Id = "TOUR012"
                },
                new Schedule
                {
                    Id = "SCHE057",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 10,
                    Status = 1,
                    Adult_price = 3519000.0m,
                    Children_price = 2463300.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 5, 25, 15, 0, 0),
                    Tour_Id = "TOUR012"
                },
                new Schedule
                {
                    Id = "SCHE058",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 3022200.0m,
                    Children_price = 2115540.0m,
                    Discount = 27,
                    Create_at = new DateTime(2024, 8, 15, 9, 30, 0),
                    Tour_Id = "TOUR012"
                },
                new Schedule
                {
                    Id = "SCHE059",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 20,
                    Status = 1,
                    Adult_price = 3767400.0m,
                    Children_price = 2637180.0m,
                    Discount = 9,
                    Create_at = new DateTime(2025, 3, 10, 11, 0, 0),
                    Tour_Id = "TOUR012"
                },
                new Schedule
                {
                    Id = "SCHE060",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 2691000.0m,
                    Children_price = 1883700.0m,
                    Discount = 35,
                    Create_at = new DateTime(2024, 11, 25, 16, 30, 0),
                    Tour_Id = "TOUR012"
                },
                new Schedule
                {
                    Id = "SCHE061",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 30,
                    Status = 1,
                    Adult_price = 3459200.0m,
                    Children_price = 2421440.0m,
                    Discount = 6,
                    Create_at = new DateTime(2024, 1, 1, 10, 0, 0),
                    Tour_Id = "TOUR013"
                },
                new Schedule
                {
                    Id = "SCHE062",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 3164800.0m,
                    Children_price = 2215360.0m,
                    Discount = 14,
                    Create_at = new DateTime(2024, 4, 15, 14, 0, 0),
                    Tour_Id = "TOUR013"
                },
                new Schedule
                {
                    Id = "SCHE063",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 2649600.0m,
                    Children_price = 1854720.0m,
                    Discount = 28,
                    Create_at = new DateTime(2024, 7, 5, 9, 0, 0),
                    Tour_Id = "TOUR013"
                },
                new Schedule
                {
                    Id = "SCHE064",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 22,
                    Status = 1,
                    Adult_price = 3496000.0m,
                    Children_price = 2447200.0m,
                    Discount = 5,
                    Create_at = new DateTime(2025, 2, 20, 11, 30, 0),
                    Tour_Id = "TOUR013"
                },
                new Schedule
                {
                    Id = "SCHE065",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 8,
                    Status = 1,
                    Adult_price = 2318400.0m,
                    Children_price = 1622880.0m,
                    Discount = 37,
                    Create_at = new DateTime(2024, 10, 28, 16, 45, 0),
                    Tour_Id = "TOUR013"
                },
                new Schedule
                {
                    Id = "SCHE066",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 20,
                    Status = 1,
                    Adult_price = 5667200.0m,
                    Children_price = 3967040.0m,
                    Discount = 12,
                    Create_at = new DateTime(2024, 3, 12, 10, 0, 0),
                    Tour_Id = "TOUR014"
                },
                new Schedule
                {
                    Id = "SCHE067",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 7,
                    Status = 1,
                    Adult_price = 5924800.0m,
                    Children_price = 4147360.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 6, 28, 15, 0, 0),
                    Tour_Id = "TOUR014"
                },
                new Schedule
                {
                    Id = "SCHE068",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 4829800.0m,
                    Children_price = 3380860.0m,
                    Discount = 25,
                    Create_at = new DateTime(2025, 1, 23, 9, 30, 0),
                    Tour_Id = "TOUR014"
                },
                new Schedule
                {
                    Id = "SCHE069",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 6440000.0m,
                    Children_price = 4508000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 10, 14, 0, 0),
                    Tour_Id = "TOUR014"
                },
                new Schedule
                {
                    Id = "SCHE070",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 3,
                    Status = 1,
                    Adult_price = 4314800.0m,
                    Children_price = 3020360.0m,
                    Discount = 33,
                    Create_at = new DateTime(2025, 4, 10, 17, 0, 0),
                    Tour_Id = "TOUR014"
                },
                new Schedule
                {
                    Id = "SCHE071",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 25,
                    Status = 1,
                    Adult_price = 4705800.0m,
                    Children_price = 3294060.0m,
                    Discount = 7,
                    Create_at = new DateTime(2024, 2, 25, 10, 0, 0),
                    Tour_Id = "TOUR015"
                },
                new Schedule
                {
                    Id = "SCHE072",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 10,
                    Status = 1,
                    Adult_price = 4351600.0m,
                    Children_price = 3046120.0m,
                    Discount = 14,
                    Create_at = new DateTime(2024, 5, 10, 15, 0, 0),
                    Tour_Id = "TOUR015"
                },
                new Schedule
                {
                    Id = "SCHE073",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 3643200.0m,
                    Children_price = 2550240.0m,
                    Discount = 28,
                    Create_at = new DateTime(2024, 8, 1, 9, 30, 0),
                    Tour_Id = "TOUR015"
                },
                new Schedule
                {
                    Id = "SCHE074",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 18,
                    Status = 1,
                    Adult_price = 4756400.0m,
                    Children_price = 3329480.0m,
                    Discount = 6,
                    Create_at = new DateTime(2025, 3, 22, 11, 0, 0),
                    Tour_Id = "TOUR015"
                },
                new Schedule
                {
                    Id = "SCHE075",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 3228400.0m,
                    Children_price = 2259880.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 11, 15, 16, 45, 0),
                    Tour_Id = "TOUR015"
                },
                new Schedule
                {
                    Id = "SCHE076",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 30,
                    Status = 1,
                    Adult_price = 4195200.0m,
                    Children_price = 2936640.0m,
                    Discount = 4,
                    Create_at = new DateTime(2024, 1, 8, 10, 0, 0),
                    Tour_Id = "TOUR016"
                },
                new Schedule
                {
                    Id = "SCHE077",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 3539700.0m,
                    Children_price = 2477790.0m,
                    Discount = 19,
                    Create_at = new DateTime(2024, 4, 20, 14, 30, 0),
                    Tour_Id = "TOUR016"
                },
                new Schedule
                {
                    Id = "SCHE078",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 3364900.0m,
                    Children_price = 2355430.0m,
                    Discount = 23,
                    Create_at = new DateTime(2024, 7, 18, 9, 0, 0),
                    Tour_Id = "TOUR016"
                },
                new Schedule
                {
                    Id = "SCHE079",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 25,
                    Status = 1,
                    Adult_price = 4020400.0m,
                    Children_price = 2814280.0m,
                    Discount = 8,
                    Create_at = new DateTime(2025, 2, 10, 11, 0, 0),
                    Tour_Id = "TOUR016"
                },
                new Schedule
                {
                    Id = "SCHE080",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 8,
                    Status = 1,
                    Adult_price = 2665700.0m,
                    Children_price = 1865990.0m,
                    Discount = 39,
                    Create_at = new DateTime(2024, 11, 1, 16, 0, 0),
                    Tour_Id = "TOUR016"
                },
                new Schedule
                {
                    Id = "SCHE081",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 25,
                    Status = 1,
                    Adult_price = 2622000.0m,
                    Children_price = 1835400.0m,
                    Discount = 5,
                    Create_at = new DateTime(2024, 2, 5, 10, 30, 0),
                    Tour_Id = "TOUR017"
                },
                new Schedule
                {
                    Id = "SCHE082",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 10,
                    Status = 1,
                    Adult_price = 2263200.0m,
                    Children_price = 1584240.0m,
                    Discount = 18,
                    Create_at = new DateTime(2024, 5, 5, 15, 0, 0),
                    Tour_Id = "TOUR017"
                },
                new Schedule
                {
                    Id = "SCHE083",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 2170800.0m,
                    Children_price = 1519560.0m,
                    Discount = 21,
                    Create_at = new DateTime(2024, 8, 20, 9, 0, 0),
                    Tour_Id = "TOUR017"
                },
                new Schedule
                {
                    Id = "SCHE084",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 18,
                    Status = 1,
                    Adult_price = 2539200.0m,
                    Children_price = 1777440.0m,
                    Discount = 8,
                    Create_at = new DateTime(2025, 3, 1, 11, 30, 0),
                    Tour_Id = "TOUR017"
                },
                new Schedule
                {
                    Id = "SCHE085",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 1766400.0m,
                    Children_price = 1236480.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 10, 20, 16, 0, 0),
                    Tour_Id = "TOUR017"
                },
                new Schedule
                {
                    Id = "SCHE086",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 16813000.0m,
                    Children_price = 11769100.0m,
                    Discount = 14,
                    Create_at = new DateTime(2024, 3, 18, 10, 0, 0),
                    Tour_Id = "TOUR018"
                },
                new Schedule
                {
                    Id = "SCHE087",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 17204000.0m,
                    Children_price = 12042800.0m,
                    Discount = 12,
                    Create_at = new DateTime(2024, 6, 10, 15, 0, 0),
                    Tour_Id = "TOUR018"
                },
                new Schedule
                {
                    Id = "SCHE088",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 14271500.0m,
                    Children_price = 9990050.0m,
                    Discount = 27,
                    Create_at = new DateTime(2025, 1, 18, 9, 30, 0),
                    Tour_Id = "TOUR018"
                },
                new Schedule
                {
                    Id = "SCHE089",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 10,
                    Status = 1,
                    Adult_price = 19550000.0m,
                    Children_price = 13685000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 12, 14, 0, 0),
                    Tour_Id = "TOUR018"
                },
                new Schedule
                {
                    Id = "SCHE090",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 2,
                    Status = 1,
                    Adult_price = 11730000.0m,
                    Children_price = 8211000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 4, 29, 17, 30, 0),
                    Tour_Id = "TOUR018"
                },
                new Schedule
                {
                    Id = "SCHE091",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 35,
                    Status = 1,
                    Adult_price = 2870400.0m,
                    Children_price = 2009280.0m,
                    Discount = 4,
                    Create_at = new DateTime(2024, 1, 20, 10, 0, 0),
                    Tour_Id = "TOUR019"
                },
                new Schedule
                {
                    Id = "SCHE092",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 2421900.0m,
                    Children_price = 1695330.0m,
                    Discount = 19,
                    Create_at = new DateTime(2024, 5, 3, 14, 30, 0),
                    Tour_Id = "TOUR019"
                },
                new Schedule
                {
                    Id = "SCHE093",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 2202600.0m,
                    Children_price = 1541820.0m,
                    Discount = 26,
                    Create_at = new DateTime(2024, 8, 12, 9, 0, 0),
                    Tour_Id = "TOUR019"
                },
                new Schedule
                {
                    Id = "SCHE094",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 28,
                    Status = 1,
                    Adult_price = 2750800.0m,
                    Children_price = 1925560.0m,
                    Discount = 8,
                    Create_at = new DateTime(2025, 3, 8, 11, 0, 0),
                    Tour_Id = "TOUR019"
                },
                new Schedule
                {
                    Id = "SCHE095",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 10,
                    Status = 1,
                    Adult_price = 1853800.0m,
                    Children_price = 1297660.0m,
                    Discount = 38,
                    Create_at = new DateTime(2024, 11, 3, 16, 0, 0),
                    Tour_Id = "TOUR019"
                },
                new Schedule
                {
                    Id = "SCHE096",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 20,
                    Status = 1,
                    Adult_price = 2093000.0m,
                    Children_price = 1465100.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 2, 18, 10, 0, 0),
                    Tour_Id = "TOUR020"
                },
                new Schedule
                {
                    Id = "SCHE097",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 1932000.0m,
                    Children_price = 1352400.0m,
                    Discount = 16,
                    Create_at = new DateTime(2024, 5, 22, 15, 0, 0),
                    Tour_Id = "TOUR020"
                },
                new Schedule
                {
                    Id = "SCHE098",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1748000.0m,
                    Children_price = 1223600.0m,
                    Discount = 24,
                    Create_at = new DateTime(2024, 8, 6, 9, 30, 0),
                    Tour_Id = "TOUR020"
                },
                new Schedule
                {
                    Id = "SCHE099",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 2139000.0m,
                    Children_price = 1497300.0m,
                    Discount = 7,
                    Create_at = new DateTime(2025, 3, 25, 11, 0, 0),
                    Tour_Id = "TOUR020"
                },
                new Schedule
                {
                    Id = "SCHE100",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 1449000.0m,
                    Children_price = 1014300.0m,
                    Discount = 37,
                    Create_at = new DateTime(2024, 10, 6, 16, 0, 0),
                    Tour_Id = "TOUR020"
                },
                new Schedule
                {
                    Id = "SCHE101",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 32,
                    Status = 1,
                    Adult_price = 1692800.0m,
                    Children_price = 1184960.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 3, 25, 10, 15, 30),
                    Tour_Id = "TOUR021"
                },
                new Schedule
                {
                    Id = "SCHE102",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 10,
                    Status = 1,
                    Adult_price = 1840000.0m,
                    Children_price = 1288000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 1, 15, 9, 30, 0),
                    Tour_Id = "TOUR021"
                },
                new Schedule
                {
                    Id = "SCHE103",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1380000.0m,
                    Children_price = 966000.0m,
                    Discount = 25,
                    Create_at = new DateTime(2024, 6, 5, 11, 45, 10),
                    Tour_Id = "TOUR021"
                },
                new Schedule
                {
                    Id = "SCHE104",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 40,
                    Status = 1,
                    Adult_price = 1508800.0m,
                    Children_price = 1056160.0m,
                    Discount = 18,
                    Create_at = new DateTime(2025, 4, 22, 16, 20, 0),
                    Tour_Id = "TOUR021"
                },
                new Schedule
                {
                    Id = "SCHE105",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 1288000.0m,
                    Children_price = 901600.0m,
                    Discount = 30,
                    Create_at = new DateTime(2024, 10, 20, 8, 30, 0),
                    Tour_Id = "TOUR021"
                },
                new Schedule
                {
                    Id = "SCHE106",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 20,
                    Status = 1,
                    Adult_price = 2070000.0m,
                    Children_price = 1449000.0m,
                    Discount = 10,
                    Create_at = new DateTime(2024, 2, 5, 10, 30, 0),
                    Tour_Id = "TOUR022"
                },
                new Schedule
                {
                    Id = "SCHE107",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 2116000.0m,
                    Children_price = 1481200.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 5, 20, 15, 0, 0),
                    Tour_Id = "TOUR022"
                },
                new Schedule
                {
                    Id = "SCHE108",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1610000.0m,
                    Children_price = 1127000.0m,
                    Discount = 30,
                    Create_at = new DateTime(2025, 3, 15, 9, 0, 0),
                    Tour_Id = "TOUR022"
                },
                new Schedule
                {
                    Id = "SCHE109",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 1955000.0m,
                    Children_price = 1368500.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 8, 25, 11, 10, 0),
                    Tour_Id = "TOUR022"
                },
                new Schedule
                {
                    Id = "SCHE110",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 4,
                    Status = 1,
                    Adult_price = 1380000.0m,
                    Children_price = 966000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 2, 1, 17, 0, 0),
                    Tour_Id = "TOUR022"
                },
                new Schedule
                {
                    Id = "SCHE111",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 25,
                    Status = 1,
                    Adult_price = 2454100.0m,
                    Children_price = 1717870.0m,
                    Discount = 3,
                    Create_at = new DateTime(2024, 1, 10, 10, 0, 0),
                    Tour_Id = "TOUR023"
                },
                new Schedule
                {
                    Id = "SCHE112",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 2109900.0m,
                    Children_price = 1476930.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 5, 5, 14, 0, 0),
                    Tour_Id = "TOUR023"
                },
                new Schedule
                {
                    Id = "SCHE113",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1973400.0m,
                    Children_price = 1381380.0m,
                    Discount = 22,
                    Create_at = new DateTime(2024, 7, 15, 9, 0, 0),
                    Tour_Id = "TOUR023"
                },
                new Schedule
                {
                    Id = "SCHE114",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 20,
                    Status = 1,
                    Adult_price = 2378200.0m,
                    Children_price = 1664740.0m,
                    Discount = 6,
                    Create_at = new DateTime(2025, 2, 20, 11, 30, 0),
                    Tour_Id = "TOUR023"
                },
                new Schedule
                {
                    Id = "SCHE115",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 1669800.0m,
                    Children_price = 1168860.0m,
                    Discount = 34,
                    Create_at = new DateTime(2024, 11, 25, 16, 45, 0),
                    Tour_Id = "TOUR023"
                },
                new Schedule
                {
                    Id = "SCHE116",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 1925100.0m,
                    Children_price = 1347570.0m,
                    Discount = 7,
                    Create_at = new DateTime(2024, 3, 5, 9, 10, 0),
                    Tour_Id = "TOUR024"
                },
                new Schedule
                {
                    Id = "SCHE117",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 1759500.0m,
                    Children_price = 1231650.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 6, 28, 14, 0, 0),
                    Tour_Id = "TOUR024"
                },
                new Schedule
                {
                    Id = "SCHE118",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1656000.0m,
                    Children_price = 1159200.0m,
                    Discount = 20,
                    Create_at = new DateTime(2025, 1, 25, 10, 0, 0),
                    Tour_Id = "TOUR024"
                },
                new Schedule
                {
                    Id = "SCHE119",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 2070000.0m,
                    Children_price = 1449000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 10, 16, 30, 0),
                    Tour_Id = "TOUR024"
                },
                new Schedule
                {
                    Id = "SCHE120",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 3,
                    Status = 1,
                    Adult_price = 1449000.0m,
                    Children_price = 1014300.0m,
                    Discount = 30,
                    Create_at = new DateTime(2025, 4, 5, 8, 45, 0),
                    Tour_Id = "TOUR024"
                },
                new Schedule
                {
                    Id = "SCHE121",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 30,
                    Status = 1,
                    Adult_price = 1988350.0m,
                    Children_price = 1391845.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 4, 15, 11, 0, 0),
                    Tour_Id = "TOUR025"
                },
                new Schedule
                {
                    Id = "SCHE122",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 10,
                    Status = 1,
                    Adult_price = 1820550.0m,
                    Children_price = 1274385.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 7, 8, 15, 0, 0),
                    Tour_Id = "TOUR025"
                },
                new Schedule
                {
                    Id = "SCHE123",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1573200.0m,
                    Children_price = 1101240.0m,
                    Discount = 28,
                    Create_at = new DateTime(2025, 3, 5, 9, 0, 0),
                    Tour_Id = "TOUR025"
                },
                new Schedule
                {
                    Id = "SCHE124",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 22,
                    Status = 1,
                    Adult_price = 2185000.0m,
                    Children_price = 1529500.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 23, 14, 30, 0),
                    Tour_Id = "TOUR025"
                },
                new Schedule
                {
                    Id = "SCHE125",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 1311000.0m,
                    Children_price = 917700.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 3, 24, 10, 0, 0),
                    Tour_Id = "TOUR025"
                },
                new Schedule
                {
                    Id = "SCHE126",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 20,
                    Status = 1,
                    Adult_price = 1545600.0m,
                    Children_price = 1081920.0m,
                    Discount = 4,
                    Create_at = new DateTime(2024, 1, 30, 10, 0, 0),
                    Tour_Id = "TOUR026"
                },
                new Schedule
                {
                    Id = "SCHE127",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 1304100.0m,
                    Children_price = 912870.0m,
                    Discount = 19,
                    Create_at = new DateTime(2024, 5, 8, 14, 0, 0),
                    Tour_Id = "TOUR026"
                },
                new Schedule
                {
                    Id = "SCHE128",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1240300.0m,
                    Children_price = 868210.0m,
                    Discount = 23,
                    Create_at = new DateTime(2024, 8, 15, 9, 30, 0),
                    Tour_Id = "TOUR026"
                },
                new Schedule
                {
                    Id = "SCHE129",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 1481200.0m,
                    Children_price = 1036840.0m,
                    Discount = 8,
                    Create_at = new DateTime(2025, 3, 10, 11, 0, 0),
                    Tour_Id = "TOUR026"
                },
                new Schedule
                {
                    Id = "SCHE130",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 4,
                    Status = 1,
                    Adult_price = 1030400.0m,
                    Children_price = 721280.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 12, 8, 16, 0, 0),
                    Tour_Id = "TOUR026"
                },
                new Schedule
                {
                    Id = "SCHE131",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 30,
                    Status = 1,
                    Adult_price = 1842300.0m,
                    Children_price = 1289610.0m,
                    Discount = 11,
                    Create_at = new DateTime(2024, 2, 20, 10, 30, 0),
                    Tour_Id = "TOUR027"
                },
                new Schedule
                {
                    Id = "SCHE132",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 1738800.0m,
                    Children_price = 1217160.0m,
                    Discount = 16,
                    Create_at = new DateTime(2024, 5, 28, 15, 0, 0),
                    Tour_Id = "TOUR027"
                },
                new Schedule
                {
                    Id = "SCHE133",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1573200.0m,
                    Children_price = 1101240.0m,
                    Discount = 24,
                    Create_at = new DateTime(2024, 8, 10, 9, 0, 0),
                    Tour_Id = "TOUR027"
                },
                new Schedule
                {
                    Id = "SCHE134",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 25,
                    Status = 1,
                    Adult_price = 2070000.0m,
                    Children_price = 1449000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 4, 1, 11, 45, 0),
                    Tour_Id = "TOUR027"
                },
                new Schedule
                {
                    Id = "SCHE135",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 8,
                    Status = 1,
                    Adult_price = 1304100.0m,
                    Children_price = 912870.0m,
                    Discount = 37,
                    Create_at = new DateTime(2024, 10, 8, 17, 0, 0),
                    Tour_Id = "TOUR027"
                },
                new Schedule
                {
                    Id = "SCHE136",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 3105000.0m,
                    Children_price = 2173500.0m,
                    Discount = 10,
                    Create_at = new DateTime(2024, 1, 12, 10, 0, 0),
                    Tour_Id = "TOUR028"
                },
                new Schedule
                {
                    Id = "SCHE137",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 2932500.0m,
                    Children_price = 2052750.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 4, 25, 14, 30, 0),
                    Tour_Id = "TOUR028"
                },
                new Schedule
                {
                    Id = "SCHE138",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 2760000.0m,
                    Children_price = 1932000.0m,
                    Discount = 20,
                    Create_at = new DateTime(2024, 7, 23, 9, 0, 0),
                    Tour_Id = "TOUR028"
                },
                new Schedule
                {
                    Id = "SCHE139",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 3174000.0m,
                    Children_price = 2221800.0m,
                    Discount = 8,
                    Create_at = new DateTime(2025, 2, 5, 11, 0, 0),
                    Tour_Id = "TOUR028"
                },
                new Schedule
                {
                    Id = "SCHE140",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 3,
                    Status = 1,
                    Adult_price = 2070000.0m,
                    Children_price = 1449000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2024, 11, 8, 16, 0, 0),
                    Tour_Id = "TOUR028"
                },
                new Schedule
                {
                    Id = "SCHE141",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 40,
                    Status = 1,
                    Adult_price = 2300000.0m,
                    Children_price = 1610000.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 3, 10, 10, 30, 0),
                    Tour_Id = "TOUR029"
                },
                new Schedule
                {
                    Id = "SCHE142",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 20,
                    Status = 1,
                    Adult_price = 2109900.0m,
                    Children_price = 1476930.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 6, 20, 15, 0, 0),
                    Tour_Id = "TOUR029"
                },
                new Schedule
                {
                    Id = "SCHE143",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1806300.0m,
                    Children_price = 1264410.0m,
                    Discount = 28,
                    Create_at = new DateTime(2025, 1, 20, 9, 30, 0),
                    Tour_Id = "TOUR029"
                },
                new Schedule
                {
                    Id = "SCHE144",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 35,
                    Status = 1,
                    Adult_price = 2277000.0m,
                    Children_price = 1593900.0m,
                    Discount = 10,
                    Create_at = new DateTime(2024, 9, 5, 14, 0, 0),
                    Tour_Id = "TOUR029"
                },
                new Schedule
                {
                    Id = "SCHE145",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 10,
                    Status = 1,
                    Adult_price = 1669800.0m,
                    Children_price = 1168860.0m,
                    Discount = 34,
                    Create_at = new DateTime(2025, 4, 28, 17, 30, 0),
                    Tour_Id = "TOUR029"
                },
                new Schedule
                {
                    Id = "SCHE146",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 25,
                    Status = 1,
                    Adult_price = 1876800.0m,
                    Children_price = 1313760.0m,
                    Discount = 4,
                    Create_at = new DateTime(2024, 1, 5, 10, 0, 0),
                    Tour_Id = "TOUR030"
                },
                new Schedule
                {
                    Id = "SCHE147",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 1583550.0m,
                    Children_price = 1108485.0m,
                    Discount = 19,
                    Create_at = new DateTime(2024, 4, 18, 14, 0, 0),
                    Tour_Id = "TOUR030"
                },
                new Schedule
                {
                    Id = "SCHE148",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1505850.0m,
                    Children_price = 1054095.0m,
                    Discount = 23,
                    Create_at = new DateTime(2024, 7, 8, 9, 0, 0),
                    Tour_Id = "TOUR030"
                },
                new Schedule
                {
                    Id = "SCHE149",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 20,
                    Status = 1,
                    Adult_price = 1798600.0m,
                    Children_price = 1259020.0m,
                    Discount = 8,
                    Create_at = new DateTime(2025, 2, 23, 11, 30, 0),
                    Tour_Id = "TOUR030"
                },
                new Schedule
                {
                    Id = "SCHE150",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 1251200.0m,
                    Children_price = 875840.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 10, 30, 16, 45, 0),
                    Tour_Id = "TOUR030"
                },
                new Schedule
                {
                    Id = "SCHE151",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 35,
                    Status = 1,
                    Adult_price = 1784800.0m,
                    Children_price = 1249360.0m,
                    Discount = 3,
                    Create_at = new DateTime(2024, 1, 18, 10, 0, 0),
                    Tour_Id = "TOUR031"
                },
                new Schedule
                {
                    Id = "SCHE152",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 18,
                    Status = 1,
                    Adult_price = 1527200.0m,
                    Children_price = 1069040.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 5, 1, 14, 30, 0),
                    Tour_Id = "TOUR031"
                },
                new Schedule
                {
                    Id = "SCHE153",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1435200.0m,
                    Children_price = 1004640.0m,
                    Discount = 22,
                    Create_at = new DateTime(2024, 7, 28, 9, 0, 0),
                    Tour_Id = "TOUR031"
                },
                new Schedule
                {
                    Id = "SCHE154",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 30,
                    Status = 1,
                    Adult_price = 1729600.0m,
                    Children_price = 1210720.0m,
                    Discount = 6,
                    Create_at = new DateTime(2025, 2, 10, 11, 0, 0),
                    Tour_Id = "TOUR031"
                },
                new Schedule
                {
                    Id = "SCHE155",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 10,
                    Status = 1,
                    Adult_price = 1122400.0m,
                    Children_price = 785680.0m,
                    Discount = 39,
                    Create_at = new DateTime(2024, 12, 15, 16, 0, 0),
                    Tour_Id = "TOUR031"
                },
                new Schedule
                {
                    Id = "SCHE156",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 20,
                    Status = 1,
                    Adult_price = 1242000.0m,
                    Children_price = 869400.0m,
                    Discount = 10,
                    Create_at = new DateTime(2024, 2, 25, 10, 30, 0),
                    Tour_Id = "TOUR032"
                },
                new Schedule
                {
                    Id = "SCHE157",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 1173000.0m,
                    Children_price = 821100.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 6, 2, 15, 0, 0),
                    Tour_Id = "TOUR032"
                },
                new Schedule
                {
                    Id = "SCHE158",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1007400.0m,
                    Children_price = 705180.0m,
                    Discount = 27,
                    Create_at = new DateTime(2024, 8, 20, 9, 30, 0),
                    Tour_Id = "TOUR032"
                },
                new Schedule
                {
                    Id = "SCHE159",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 1269600.0m,
                    Children_price = 888720.0m,
                    Discount = 8,
                    Create_at = new DateTime(2025, 3, 12, 11, 0, 0),
                    Tour_Id = "TOUR032"
                },
                new Schedule
                {
                    Id = "SCHE160",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 4,
                    Status = 1,
                    Adult_price = 883200.0m,
                    Children_price = 618240.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 11, 28, 16, 30, 0),
                    Tour_Id = "TOUR032"
                },
                new Schedule
                {
                    Id = "SCHE161",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 45,
                    Status = 1,
                    Adult_price = 1115500.0m,
                    Children_price = 780850.0m,
                    Discount = 3,
                    Create_at = new DateTime(2024, 1, 3, 10, 0, 0),
                    Tour_Id = "TOUR033"
                },
                new Schedule
                {
                    Id = "SCHE162",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 22,
                    Status = 1,
                    Adult_price = 954500.0m,
                    Children_price = 668150.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 4, 16, 14, 0, 0),
                    Tour_Id = "TOUR033"
                },
                new Schedule
                {
                    Id = "SCHE163",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 897000.0m,
                    Children_price = 627900.0m,
                    Discount = 22,
                    Create_at = new DateTime(2024, 7, 6, 9, 0, 0),
                    Tour_Id = "TOUR033"
                },
                new Schedule
                {
                    Id = "SCHE164",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 35,
                    Status = 1,
                    Adult_price = 1081000.0m,
                    Children_price = 756700.0m,
                    Discount = 6,
                    Create_at = new DateTime(2025, 2, 21, 11, 30, 0),
                    Tour_Id = "TOUR033"
                },
                new Schedule
                {
                    Id = "SCHE165",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 10,
                    Status = 1,
                    Adult_price = 701500.0m,
                    Children_price = 491050.0m,
                    Discount = 39,
                    Create_at = new DateTime(2024, 10, 29, 16, 45, 0),
                    Tour_Id = "TOUR033"
                },
                new Schedule
                {
                    Id = "SCHE166",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 30,
                    Status = 1,
                    Adult_price = 2566800.0m,
                    Children_price = 1796760.0m,
                    Discount = 7,
                    Create_at = new DateTime(2024, 3, 15, 10, 0, 0),
                    Tour_Id = "TOUR034"
                },
                new Schedule
                {
                    Id = "SCHE167",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 2346000.0m,
                    Children_price = 1642200.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 6, 30, 15, 0, 0),
                    Tour_Id = "TOUR034"
                },
                new Schedule
                {
                    Id = "SCHE168",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 2208000.0m,
                    Children_price = 1545600.0m,
                    Discount = 20,
                    Create_at = new DateTime(2025, 1, 28, 9, 30, 0),
                    Tour_Id = "TOUR034"
                },
                new Schedule
                {
                    Id = "SCHE169",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 22,
                    Status = 1,
                    Adult_price = 2760000.0m,
                    Children_price = 1932000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 15, 14, 0, 0),
                    Tour_Id = "TOUR034"
                },
                new Schedule
                {
                    Id = "SCHE170",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 1932000.0m,
                    Children_price = 1352400.0m,
                    Discount = 30,
                    Create_at = new DateTime(2025, 5, 2, 17, 0, 0),
                    Tour_Id = "TOUR034"
                },
                new Schedule
                {
                    Id = "SCHE171",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 25,
                    Status = 1,
                    Adult_price = 1610000.0m,
                    Children_price = 1127000.0m,
                    Discount = 7,
                    Create_at = new DateTime(2024, 2, 8, 10, 0, 0),
                    Tour_Id = "TOUR035"
                },
                new Schedule
                {
                    Id = "SCHE172",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 10,
                    Status = 1,
                    Adult_price = 1453500.0m,
                    Children_price = 1017450.0m,
                    Discount = 16,
                    Create_at = new DateTime(2024, 5, 13, 15, 0, 0),
                    Tour_Id = "TOUR035"
                },
                new Schedule
                {
                    Id = "SCHE173",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1311000.0m,
                    Children_price = 917700.0m,
                    Discount = 24,
                    Create_at = new DateTime(2024, 8, 4, 9, 30, 0),
                    Tour_Id = "TOUR035"
                },
                new Schedule
                {
                    Id = "SCHE174",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 18,
                    Status = 1,
                    Adult_price = 1725000.0m,
                    Children_price = 1207500.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 3, 27, 11, 0, 0),
                    Tour_Id = "TOUR035"
                },
                new Schedule
                {
                    Id = "SCHE175",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 1086750.0m,
                    Children_price = 760725.0m,
                    Discount = 37,
                    Create_at = new DateTime(2024, 11, 18, 16, 45, 0),
                    Tour_Id = "TOUR035"
                },
                new Schedule
                {
                    Id = "SCHE176",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 35,
                    Status = 1,
                    Adult_price = 1324800.0m,
                    Children_price = 927360.0m,
                    Discount = 4,
                    Create_at = new DateTime(2024, 1, 11, 10, 0, 0),
                    Tour_Id = "TOUR036"
                },
                new Schedule
                {
                    Id = "SCHE177",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 18,
                    Status = 1,
                    Adult_price = 1117800.0m,
                    Children_price = 782460.0m,
                    Discount = 19,
                    Create_at = new DateTime(2024, 4, 24, 14, 30, 0),
                    Tour_Id = "TOUR036"
                },
                new Schedule
                {
                    Id = "SCHE178",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1062600.0m,
                    Children_price = 743820.0m,
                    Discount = 23,
                    Create_at = new DateTime(2024, 7, 20, 9, 0, 0),
                    Tour_Id = "TOUR036"
                },
                new Schedule
                {
                    Id = "SCHE179",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 28,
                    Status = 1,
                    Adult_price = 1269600.0m,
                    Children_price = 888720.0m,
                    Discount = 8,
                    Create_at = new DateTime(2025, 2, 13, 11, 0, 0),
                    Tour_Id = "TOUR036"
                },
                new Schedule
                {
                    Id = "SCHE180",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 10,
                    Status = 1,
                    Adult_price = 883200.0m,
                    Children_price = 618240.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 11, 6, 16, 0, 0),
                    Tour_Id = "TOUR036"
                },
                new Schedule
                {
                    Id = "SCHE181",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 25,
                    Status = 1,
                    Adult_price = 1163800.0m,
                    Children_price = 814660.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 2, 2, 10, 30, 0),
                    Tour_Id = "TOUR037"
                },
                new Schedule
                {
                    Id = "SCHE182",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 10,
                    Status = 1,
                    Adult_price = 1075250.0m,
                    Children_price = 752675.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 5, 10, 15, 0, 0),
                    Tour_Id = "TOUR037"
                },
                new Schedule
                {
                    Id = "SCHE183",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 923450.0m,
                    Children_price = 646415.0m,
                    Discount = 27,
                    Create_at = new DateTime(2024, 8, 23, 9, 30, 0),
                    Tour_Id = "TOUR037"
                },
                new Schedule
                {
                    Id = "SCHE184",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 18,
                    Status = 1,
                    Adult_price = 1151150.0m,
                    Children_price = 805805.0m,
                    Discount = 9,
                    Create_at = new DateTime(2025, 3, 7, 11, 0, 0),
                    Tour_Id = "TOUR037"
                },
                new Schedule
                {
                    Id = "SCHE185",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 809600.0m,
                    Children_price = 566720.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 11, 30, 16, 30, 0),
                    Tour_Id = "TOUR037"
                },
                new Schedule
                {
                    Id = "SCHE186",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 1384600.0m,
                    Children_price = 969220.0m,
                    Discount = 14,
                    Create_at = new DateTime(2024, 3, 20, 10, 0, 0),
                    Tour_Id = "TOUR038"
                },
                new Schedule
                {
                    Id = "SCHE187",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 1416800.0m,
                    Children_price = 991760.0m,
                    Discount = 12,
                    Create_at = new DateTime(2024, 6, 12, 15, 0, 0),
                    Tour_Id = "TOUR038"
                },
                new Schedule
                {
                    Id = "SCHE188",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1175300.0m,
                    Children_price = 822710.0m,
                    Discount = 27,
                    Create_at = new DateTime(2025, 1, 21, 9, 30, 0),
                    Tour_Id = "TOUR038"
                },
                new Schedule
                {
                    Id = "SCHE189",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 10,
                    Status = 1,
                    Adult_price = 1610000.0m,
                    Children_price = 1127000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 14, 14, 0, 0),
                    Tour_Id = "TOUR038"
                },
                new Schedule
                {
                    Id = "SCHE190",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 2,
                    Status = 1,
                    Adult_price = 966000.0m,
                    Children_price = 676200.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 5, 1, 17, 30, 0),
                    Tour_Id = "TOUR038"
                },
                new Schedule
                {
                    Id = "SCHE191",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 30,
                    Status = 1,
                    Adult_price = 1766400.0m,
                    Children_price = 1236480.0m,
                    Discount = 4,
                    Create_at = new DateTime(2024, 1, 23, 10, 0, 0),
                    Tour_Id = "TOUR039"
                },
                new Schedule
                {
                    Id = "SCHE192",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 1490400.0m,
                    Children_price = 1043280.0m,
                    Discount = 19,
                    Create_at = new DateTime(2024, 5, 6, 14, 30, 0),
                    Tour_Id = "TOUR039"
                },
                new Schedule
                {
                    Id = "SCHE193",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1417000.0m,
                    Children_price = 991900.0m,
                    Discount = 23,
                    Create_at = new DateTime(2024, 8, 15, 9, 0, 0),
                    Tour_Id = "TOUR039"
                },
                new Schedule
                {
                    Id = "SCHE194",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 25,
                    Status = 1,
                    Adult_price = 1692800.0m,
                    Children_price = 1184960.0m,
                    Discount = 8,
                    Create_at = new DateTime(2025, 3, 11, 11, 0, 0),
                    Tour_Id = "TOUR039"
                },
                new Schedule
                {
                    Id = "SCHE195",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 8,
                    Status = 1,
                    Adult_price = 1177600.0m,
                    Children_price = 824320.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 11, 6, 16, 0, 0),
                    Tour_Id = "TOUR039"
                },
                new Schedule
                {
                    Id = "SCHE196",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 20,
                    Status = 1,
                    Adult_price = 2566800.0m,
                    Children_price = 1796760.0m,
                    Discount = 7,
                    Create_at = new DateTime(2024, 2, 10, 10, 0, 0),
                    Tour_Id = "TOUR040"
                },
                new Schedule
                {
                    Id = "SCHE197",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 2346000.0m,
                    Children_price = 1642200.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 5, 15, 15, 0, 0),
                    Tour_Id = "TOUR040"
                },
                new Schedule
                {
                    Id = "SCHE198",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 2208000.0m,
                    Children_price = 1545600.0m,
                    Discount = 20,
                    Create_at = new DateTime(2024, 8, 8, 9, 30, 0),
                    Tour_Id = "TOUR040"
                },
                new Schedule
                {
                    Id = "SCHE199",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 2760000.0m,
                    Children_price = 1932000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 3, 30, 11, 0, 0),
                    Tour_Id = "TOUR040"
                },
                new Schedule
                {
                    Id = "SCHE200",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 1656000.0m,
                    Children_price = 1159200.0m,
                    Discount = 40,
                    Create_at = new DateTime(2024, 10, 9, 16, 0, 0),
                    Tour_Id = "TOUR040"
                },
                new Schedule
                {
                    Id = "SCHE201",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 22,
                    Status = 1,
                    Adult_price = 1269600.0m,
                    Children_price = 888720.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 3, 28, 11, 25, 40),
                    Tour_Id = "TOUR041"
                },
                new Schedule
                {
                    Id = "SCHE202",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 1380000.0m,
                    Children_price = 966000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 1, 17, 10, 0, 0),
                    Tour_Id = "TOUR041"
                },
                new Schedule
                {
                    Id = "SCHE203",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1035000.0m,
                    Children_price = 724500.0m,
                    Discount = 25,
                    Create_at = new DateTime(2024, 6, 8, 14, 0, 30),
                    Tour_Id = "TOUR041"
                },
                new Schedule
                {
                    Id = "SCHE204",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 30,
                    Status = 1,
                    Adult_price = 1131600.0m,
                    Children_price = 792120.0m,
                    Discount = 18,
                    Create_at = new DateTime(2025, 4, 25, 16, 40, 0),
                    Tour_Id = "TOUR041"
                },
                new Schedule
                {
                    Id = "SCHE205",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 4,
                    Status = 1,
                    Adult_price = 966000.0m,
                    Children_price = 676200.0m,
                    Discount = 30,
                    Create_at = new DateTime(2024, 10, 23, 9, 0, 0),
                    Tour_Id = "TOUR041"
                },
                new Schedule
                {
                    Id = "SCHE206",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 25,
                    Status = 1,
                    Adult_price = 2691000.0m,
                    Children_price = 1883700.0m,
                    Discount = 10,
                    Create_at = new DateTime(2024, 2, 9, 10, 45, 0),
                    Tour_Id = "TOUR042"
                },
                new Schedule
                {
                    Id = "SCHE207",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 10,
                    Status = 1,
                    Adult_price = 2750800.0m,
                    Children_price = 1925560.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 5, 22, 15, 20, 0),
                    Tour_Id = "TOUR042"
                },
                new Schedule
                {
                    Id = "SCHE208",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 2093000.0m,
                    Children_price = 1465100.0m,
                    Discount = 30,
                    Create_at = new DateTime(2025, 3, 18, 9, 10, 0),
                    Tour_Id = "TOUR042"
                },
                new Schedule
                {
                    Id = "SCHE209",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 18,
                    Status = 1,
                    Adult_price = 2541500.0m,
                    Children_price = 1779050.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 8, 28, 11, 30, 0),
                    Tour_Id = "TOUR042"
                },
                new Schedule
                {
                    Id = "SCHE210",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 1794000.0m,
                    Children_price = 1255800.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 2, 5, 17, 15, 0),
                    Tour_Id = "TOUR042"
                },
                new Schedule
                {
                    Id = "SCHE211",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 1561700.0m,
                    Children_price = 1093190.0m,
                    Discount = 3,
                    Create_at = new DateTime(2024, 1, 13, 10, 10, 0),
                    Tour_Id = "TOUR043"
                },
                new Schedule
                {
                    Id = "SCHE212",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 7,
                    Status = 1,
                    Adult_price = 1336300.0m,
                    Children_price = 935410.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 5, 8, 14, 10, 0),
                    Tour_Id = "TOUR043"
                },
                new Schedule
                {
                    Id = "SCHE213",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1255800.0m,
                    Children_price = 879060.0m,
                    Discount = 22,
                    Create_at = new DateTime(2024, 7, 18, 9, 5, 0),
                    Tour_Id = "TOUR043"
                },
                new Schedule
                {
                    Id = "SCHE214",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 1513400.0m,
                    Children_price = 1059380.0m,
                    Discount = 6,
                    Create_at = new DateTime(2025, 2, 24, 11, 40, 0),
                    Tour_Id = "TOUR043"
                },
                new Schedule
                {
                    Id = "SCHE215",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 3,
                    Status = 1,
                    Adult_price = 1062600.0m,
                    Children_price = 743820.0m,
                    Discount = 34,
                    Create_at = new DateTime(2024, 11, 28, 16, 50, 0),
                    Tour_Id = "TOUR043"
                },
                new Schedule
                {
                    Id = "SCHE216",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 20,
                    Status = 1,
                    Adult_price = 1380350.0m,
                    Children_price = 966245.0m,
                    Discount = 7,
                    Create_at = new DateTime(2024, 3, 7, 9, 15, 0),
                    Tour_Id = "TOUR044"
                },
                new Schedule
                {
                    Id = "SCHE217",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 1270750.0m,
                    Children_price = 889525.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 7, 1, 14, 5, 0),
                    Tour_Id = "TOUR044"
                },
                new Schedule
                {
                    Id = "SCHE218",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1196000.0m,
                    Children_price = 837200.0m,
                    Discount = 20,
                    Create_at = new DateTime(2025, 1, 28, 10, 5, 0),
                    Tour_Id = "TOUR044"
                },
                new Schedule
                {
                    Id = "SCHE219",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 1495000.0m,
                    Children_price = 1046500.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 13, 16, 35, 0),
                    Tour_Id = "TOUR044"
                },
                new Schedule
                {
                    Id = "SCHE220",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 4,
                    Status = 1,
                    Adult_price = 1046500.0m,
                    Children_price = 732550.0m,
                    Discount = 30,
                    Create_at = new DateTime(2025, 4, 8, 8, 50, 0),
                    Tour_Id = "TOUR044"
                },
                new Schedule
                {
                    Id = "SCHE221",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 10,
                    Status = 1,
                    Adult_price = 1579750.0m,
                    Children_price = 1105825.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 4, 18, 11, 0, 0),
                    Tour_Id = "TOUR045"
                },
                new Schedule
                {
                    Id = "SCHE222",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 1431750.0m,
                    Children_price = 1002225.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 7, 11, 15, 0, 0),
                    Tour_Id = "TOUR045"
                },
                new Schedule
                {
                    Id = "SCHE223",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1242000.0m,
                    Children_price = 869400.0m,
                    Discount = 28,
                    Create_at = new DateTime(2025, 3, 8, 9, 0, 0),
                    Tour_Id = "TOUR045"
                },
                new Schedule
                {
                    Id = "SCHE224",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 1725000.0m,
                    Children_price = 1207500.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 26, 14, 30, 0),
                    Tour_Id = "TOUR045"
                },
                new Schedule
                {
                    Id = "SCHE225",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 2,
                    Status = 1,
                    Adult_price = 1035000.0m,
                    Children_price = 724500.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 3, 27, 10, 0, 0),
                    Tour_Id = "TOUR045"
                },
                new Schedule
                {
                    Id = "SCHE226",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 45,
                    Status = 1,
                    Adult_price = 1987200.0m,
                    Children_price = 1391040.0m,
                    Discount = 4,
                    Create_at = new DateTime(2024, 2, 2, 10, 0, 0),
                    Tour_Id = "TOUR046"
                },
                new Schedule
                {
                    Id = "SCHE227",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 20,
                    Status = 1,
                    Adult_price = 1676700.0m,
                    Children_price = 1173690.0m,
                    Discount = 19,
                    Create_at = new DateTime(2024, 5, 11, 14, 0, 0),
                    Tour_Id = "TOUR046"
                },
                new Schedule
                {
                    Id = "SCHE228",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1593900.0m,
                    Children_price = 1115730.0m,
                    Discount = 23,
                    Create_at = new DateTime(2024, 8, 18, 9, 30, 0),
                    Tour_Id = "TOUR046"
                },
                new Schedule
                {
                    Id = "SCHE229",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 50,
                    Status = 1,
                    Adult_price = 1904400.0m,
                    Children_price = 1333080.0m,
                    Discount = 8,
                    Create_at = new DateTime(2025, 3, 15, 11, 0, 0),
                    Tour_Id = "TOUR046"
                },
                new Schedule
                {
                    Id = "SCHE230",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 1324800.0m,
                    Children_price = 927360.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 12, 11, 16, 0, 0),
                    Tour_Id = "TOUR046"
                },
                new Schedule
                {
                    Id = "SCHE231",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 45,
                    Status = 1,
                    Adult_price = 2352900.0m,
                    Children_price = 1647030.0m,
                    Discount = 7,
                    Create_at = new DateTime(2024, 2, 28, 10, 30, 0),
                    Tour_Id = "TOUR047"
                },
                new Schedule
                {
                    Id = "SCHE232",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 20,
                    Status = 1,
                    Adult_price = 2150500.0m,
                    Children_price = 1505350.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 6, 5, 15, 0, 0),
                    Tour_Id = "TOUR047"
                },
                new Schedule
                {
                    Id = "SCHE233",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 2024000.0m,
                    Children_price = 1416800.0m,
                    Discount = 20,
                    Create_at = new DateTime(2024, 8, 23, 9, 0, 0),
                    Tour_Id = "TOUR047"
                },
                new Schedule
                {
                    Id = "SCHE234",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 35,
                    Status = 1,
                    Adult_price = 2530000.0m,
                    Children_price = 1771000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 4, 4, 11, 45, 0),
                    Tour_Id = "TOUR047"
                },
                new Schedule
                {
                    Id = "SCHE235",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 10,
                    Status = 1,
                    Adult_price = 1644500.0m,
                    Children_price = 1151150.0m,
                    Discount = 35,
                    Create_at = new DateTime(2024, 10, 10, 17, 0, 0),
                    Tour_Id = "TOUR047"
                },
                new Schedule
                {
                    Id = "SCHE236",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 20,
                    Status = 1,
                    Adult_price = 1807300.0m,
                    Children_price = 1265110.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 1, 6, 10, 0, 0),
                    Tour_Id = "TOUR048"
                },
                new Schedule
                {
                    Id = "SCHE237",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 1583550.0m,
                    Children_price = 1108485.0m,
                    Discount = 19,
                    Create_at = new DateTime(2024, 4, 26, 14, 30, 0),
                    Tour_Id = "TOUR048"
                },
                new Schedule
                {
                    Id = "SCHE238",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1505850.0m,
                    Children_price = 1054095.0m,
                    Discount = 23,
                    Create_at = new DateTime(2024, 7, 25, 9, 0, 0),
                    Tour_Id = "TOUR048"
                },
                new Schedule
                {
                    Id = "SCHE239",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 1955000.0m,
                    Children_price = 1368500.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 2, 17, 11, 0, 0),
                    Tour_Id = "TOUR048"
                },
                new Schedule
                {
                    Id = "SCHE240",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 1251200.0m,
                    Children_price = 875840.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 11, 9, 16, 0, 0),
                    Tour_Id = "TOUR048"
                },
                new Schedule
                {
                    Id = "SCHE241",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 30,
                    Status = 1,
                    Adult_price = 2093000.0m,
                    Children_price = 1465100.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 3, 12, 10, 30, 0),
                    Tour_Id = "TOUR049"
                },
                new Schedule
                {
                    Id = "SCHE242",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 1932000.0m,
                    Children_price = 1352400.0m,
                    Discount = 16,
                    Create_at = new DateTime(2024, 6, 23, 15, 0, 0),
                    Tour_Id = "TOUR049"
                },
                new Schedule
                {
                    Id = "SCHE243",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 1748000.0m,
                    Children_price = 1223600.0m,
                    Discount = 24,
                    Create_at = new DateTime(2025, 1, 24, 9, 30, 0),
                    Tour_Id = "TOUR049"
                },
                new Schedule
                {
                    Id = "SCHE244",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 25,
                    Status = 1,
                    Adult_price = 2300000.0m,
                    Children_price = 1610000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 7, 14, 0, 0),
                    Tour_Id = "TOUR049"
                },
                new Schedule
                {
                    Id = "SCHE245",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 1449000.0m,
                    Children_price = 1014300.0m,
                    Discount = 37,
                    Create_at = new DateTime(2025, 4, 30, 17, 30, 0),
                    Tour_Id = "TOUR049"
                },
                new Schedule
                {
                    Id = "SCHE246",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 10,
                    Status = 1,
                    Adult_price = 14490000.0m,
                    Children_price = 10143000.0m,
                    Discount = 10,
                    Create_at = new DateTime(2024, 1, 9, 10, 0, 0),
                    Tour_Id = "TOUR050"
                },
                new Schedule
                {
                    Id = "SCHE247",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 3,
                    Status = 1,
                    Adult_price = 13685000.0m,
                    Children_price = 9579500.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 4, 29, 14, 0, 0),
                    Tour_Id = "TOUR050"
                },
                new Schedule
                {
                    Id = "SCHE248",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 12880000.0m,
                    Children_price = 9016000.0m,
                    Discount = 20,
                    Create_at = new DateTime(2024, 7, 28, 9, 0, 0),
                    Tour_Id = "TOUR050"
                },
                new Schedule
                {
                    Id = "SCHE249",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 14812000.0m,
                    Children_price = 10368400.0m,
                    Discount = 8,
                    Create_at = new DateTime(2025, 2, 20, 11, 30, 0),
                    Tour_Id = "TOUR050"
                },
                new Schedule
                {
                    Id = "SCHE250",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 2,
                    Status = 1,
                    Adult_price = 9660000.0m,
                    Children_price = 6762000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2024, 10, 31, 16, 45, 0),
                    Tour_Id = "TOUR050"
                },
                new Schedule
                {
                    Id = "SCHE251",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 22,
                    Status = 1,
                    Adult_price = 8040800.0m,
                    Children_price = 5628560.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 3, 3, 10, 10, 0),
                    Tour_Id = "TOUR051"
                },
                new Schedule
                {
                    Id = "SCHE252",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 8740000.0m,
                    Children_price = 6118000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 6, 15, 15, 0, 0),
                    Tour_Id = "TOUR051"
                },
                new Schedule
                {
                    Id = "SCHE253",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 6555000.0m,
                    Children_price = 4588500.0m,
                    Discount = 25,
                    Create_at = new DateTime(2025, 1, 22, 9, 30, 0),
                    Tour_Id = "TOUR051"
                },
                new Schedule
                {
                    Id = "SCHE254",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 30,
                    Status = 1,
                    Adult_price = 7166800.0m,
                    Children_price = 5016760.0m,
                    Discount = 18,
                    Create_at = new DateTime(2024, 9, 4, 16, 30, 0),
                    Tour_Id = "TOUR051"
                },
                new Schedule
                {
                    Id = "SCHE255",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 6118000.0m,
                    Children_price = 4282600.0m,
                    Discount = 30,
                    Create_at = new DateTime(2025, 4, 27, 8, 45, 0),
                    Tour_Id = "TOUR051"
                },
                new Schedule
                {
                    Id = "SCHE256",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 25,
                    Status = 1,
                    Adult_price = 11385000.0m,
                    Children_price = 7969500.0m,
                    Discount = 10,
                    Create_at = new DateTime(2024, 1, 7, 10, 0, 0),
                    Tour_Id = "TOUR052"
                },
                new Schedule
                {
                    Id = "SCHE257",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 10,
                    Status = 1,
                    Adult_price = 11638000.0m,
                    Children_price = 8146600.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 4, 20, 14, 0, 0),
                    Tour_Id = "TOUR052"
                },
                new Schedule
                {
                    Id = "SCHE258",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 8855000.0m,
                    Children_price = 6198500.0m,
                    Discount = 30,
                    Create_at = new DateTime(2024, 7, 20, 9, 30, 0),
                    Tour_Id = "TOUR052"
                },
                new Schedule
                {
                    Id = "SCHE259",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 18,
                    Status = 1,
                    Adult_price = 10752500.0m,
                    Children_price = 7526750.0m,
                    Discount = 15,
                    Create_at = new DateTime(2025, 3, 1, 11, 0, 0),
                    Tour_Id = "TOUR052"
                },
                new Schedule
                {
                    Id = "SCHE260",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 7590000.0m,
                    Children_price = 5313000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2024, 11, 12, 16, 0, 0),
                    Tour_Id = "TOUR052"
                },
                new Schedule
                {
                    Id = "SCHE261",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 35,
                    Status = 1,
                    Adult_price = 15170800.0m,
                    Children_price = 10619560.0m,
                    Discount = 3,
                    Create_at = new DateTime(2024, 1, 16, 10, 15, 0),
                    Tour_Id = "TOUR053"
                },
                new Schedule
                {
                    Id = "SCHE262",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 18,
                    Status = 1,
                    Adult_price = 12981200.0m,
                    Children_price = 9086840.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 5, 3, 14, 30, 0),
                    Tour_Id = "TOUR053"
                },
                new Schedule
                {
                    Id = "SCHE263",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 12199200.0m,
                    Children_price = 8539440.0m,
                    Discount = 22,
                    Create_at = new DateTime(2024, 7, 12, 9, 0, 0),
                    Tour_Id = "TOUR053"
                },
                new Schedule
                {
                    Id = "SCHE264",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 30,
                    Status = 1,
                    Adult_price = 14691600.0m,
                    Children_price = 10284120.0m,
                    Discount = 6,
                    Create_at = new DateTime(2025, 2, 26, 11, 30, 0),
                    Tour_Id = "TOUR053"
                },
                new Schedule
                {
                    Id = "SCHE265",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 8,
                    Status = 1,
                    Adult_price = 10166000.0m,
                    Children_price = 7116200.0m,
                    Discount = 35,
                    Create_at = new DateTime(2024, 11, 22, 16, 45, 0),
                    Tour_Id = "TOUR053"
                },
                new Schedule
                {
                    Id = "SCHE266",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 17128000.0m,
                    Children_price = 11989600.0m,
                    Discount = 7,
                    Create_at = new DateTime(2024, 3, 17, 10, 0, 0),
                    Tour_Id = "TOUR054"
                },
                new Schedule
                {
                    Id = "SCHE267",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 15640000.0m,
                    Children_price = 10948000.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 6, 30, 15, 0, 0),
                    Tour_Id = "TOUR054"
                },
                new Schedule
                {
                    Id = "SCHE268",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 14720000.0m,
                    Children_price = 10304000.0m,
                    Discount = 20,
                    Create_at = new DateTime(2025, 1, 30, 9, 30, 0),
                    Tour_Id = "TOUR054"
                },
                new Schedule
                {
                    Id = "SCHE269",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 18400000.0m,
                    Children_price = 12880000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 12, 14, 0, 0),
                    Tour_Id = "TOUR054"
                },
                new Schedule
                {
                    Id = "SCHE270",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 3,
                    Status = 1,
                    Adult_price = 11040000.0m,
                    Children_price = 7728000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 5, 5, 17, 30, 0),
                    Tour_Id = "TOUR054"
                },
                new Schedule
                {
                    Id = "SCHE271",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 10,
                    Status = 1,
                    Adult_price = 9418500.0m,
                    Children_price = 6592950.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 4, 21, 10, 30, 0),
                    Tour_Id = "TOUR055"
                },
                new Schedule
                {
                    Id = "SCHE272",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 4,
                    Status = 1,
                    Adult_price = 8590500.0m,
                    Children_price = 6013350.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 7, 14, 15, 0, 0),
                    Tour_Id = "TOUR055"
                },
                new Schedule
                {
                    Id = "SCHE273",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 7452000.0m,
                    Children_price = 5216400.0m,
                    Discount = 28,
                    Create_at = new DateTime(2025, 3, 11, 9, 0, 0),
                    Tour_Id = "TOUR055"
                },
                new Schedule
                {
                    Id = "SCHE274",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 10350000.0m,
                    Children_price = 7245000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 29, 14, 30, 0),
                    Tour_Id = "TOUR055"
                },
                new Schedule
                {
                    Id = "SCHE275",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 1,
                    Status = 1,
                    Adult_price = 6210000.0m,
                    Children_price = 4347000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 3, 30, 10, 0, 0),
                    Tour_Id = "TOUR055"
                },
                new Schedule
                {
                    Id = "SCHE276",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 25,
                    Status = 1,
                    Adult_price = 7065600.0m,
                    Children_price = 4945920.0m,
                    Discount = 4,
                    Create_at = new DateTime(2024, 1, 14, 10, 0, 0),
                    Tour_Id = "TOUR056"
                },
                new Schedule
                {
                    Id = "SCHE277",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 5961600.0m,
                    Children_price = 4173120.0m,
                    Discount = 19,
                    Create_at = new DateTime(2024, 5, 14, 14, 0, 0),
                    Tour_Id = "TOUR056"
                },
                new Schedule
                {
                    Id = "SCHE278",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 5667200.0m,
                    Children_price = 3967040.0m,
                    Discount = 23,
                    Create_at = new DateTime(2024, 8, 21, 9, 30, 0),
                    Tour_Id = "TOUR056"
                },
                new Schedule
                {
                    Id = "SCHE279",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 30,
                    Status = 1,
                    Adult_price = 6760800.0m,
                    Children_price = 4732560.0m,
                    Discount = 8,
                    Create_at = new DateTime(2025, 3, 13, 11, 0, 0),
                    Tour_Id = "TOUR056"
                },
                new Schedule
                {
                    Id = "SCHE280",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 4710400.0m,
                    Children_price = 3297280.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 12, 18, 16, 0, 0),
                    Tour_Id = "TOUR056"
                },
                new Schedule
                {
                    Id = "SCHE281",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 20,
                    Status = 1,
                    Adult_price = 9625500.0m,
                    Children_price = 6737850.0m,
                    Discount = 7,
                    Create_at = new DateTime(2024, 2, 12, 10, 30, 0),
                    Tour_Id = "TOUR057"
                },
                new Schedule
                {
                    Id = "SCHE282",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 8797500.0m,
                    Children_price = 6158250.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 5, 18, 15, 0, 0),
                    Tour_Id = "TOUR057"
                },
                new Schedule
                {
                    Id = "SCHE283",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 8280000.0m,
                    Children_price = 5796000.0m,
                    Discount = 20,
                    Create_at = new DateTime(2024, 8, 14, 9, 0, 0),
                    Tour_Id = "TOUR057"
                },
                new Schedule
                {
                    Id = "SCHE284",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 25,
                    Status = 1,
                    Adult_price = 10350000.0m,
                    Children_price = 7245000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 4, 7, 11, 45, 0),
                    Tour_Id = "TOUR057"
                },
                new Schedule
                {
                    Id = "SCHE285",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 6624000.0m,
                    Children_price = 4636800.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 10, 13, 17, 0, 0),
                    Tour_Id = "TOUR057"
                },
                new Schedule
                {
                    Id = "SCHE286",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 30,
                    Status = 1,
                    Adult_price = 7700400.0m,
                    Children_price = 5390280.0m,
                    Discount = 7,
                    Create_at = new DateTime(2024, 3, 5, 10, 0, 0),
                    Tour_Id = "TOUR058"
                },
                new Schedule
                {
                    Id = "SCHE287",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 7038000.0m,
                    Children_price = 4926600.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 6, 26, 15, 0, 0),
                    Tour_Id = "TOUR058"
                },
                new Schedule
                {
                    Id = "SCHE288",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 6624000.0m,
                    Children_price = 4636800.0m,
                    Discount = 20,
                    Create_at = new DateTime(2025, 1, 26, 9, 30, 0),
                    Tour_Id = "TOUR058"
                },
                new Schedule
                {
                    Id = "SCHE289",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 22,
                    Status = 1,
                    Adult_price = 8280000.0m,
                    Children_price = 5796000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 9, 14, 0, 0),
                    Tour_Id = "TOUR058"
                },
                new Schedule
                {
                    Id = "SCHE290",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 5382000.0m,
                    Children_price = 3767400.0m,
                    Discount = 35,
                    Create_at = new DateTime(2025, 5, 3, 17, 30, 0),
                    Tour_Id = "TOUR058"
                },
                new Schedule
                {
                    Id = "SCHE291",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 25,
                    Status = 1,
                    Adult_price = 6348000.0m,
                    Children_price = 4443600.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 2, 19, 10, 30, 0),
                    Tour_Id = "TOUR059"
                },
                new Schedule
                {
                    Id = "SCHE292",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 10,
                    Status = 1,
                    Adult_price = 5865000.0m,
                    Children_price = 4105500.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 5, 26, 15, 0, 0),
                    Tour_Id = "TOUR059"
                },
                new Schedule
                {
                    Id = "SCHE293",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 5451000.0m,
                    Children_price = 3815700.0m,
                    Discount = 21,
                    Create_at = new DateTime(2024, 8, 17, 9, 30, 0),
                    Tour_Id = "TOUR059"
                },
                new Schedule
                {
                    Id = "SCHE294",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 18,
                    Status = 1,
                    Adult_price = 6900000.0m,
                    Children_price = 4830000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 3, 19, 11, 0, 0),
                    Tour_Id = "TOUR059"
                },
                new Schedule
                {
                    Id = "SCHE295",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 4416000.0m,
                    Children_price = 3091200.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 11, 20, 16, 30, 0),
                    Tour_Id = "TOUR059"
                },
                new Schedule
                {
                    Id = "SCHE296",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 10,
                    Status = 1,
                    Adult_price = 31405500.0m,
                    Children_price = 21983850.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 2, 15, 10, 0, 0),
                    Tour_Id = "TOUR060"
                },
                new Schedule
                {
                    Id = "SCHE297",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 3,
                    Status = 1,
                    Adult_price = 28635000.0m,
                    Children_price = 20044500.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 5, 20, 15, 0, 0),
                    Tour_Id = "TOUR060"
                },
                new Schedule
                {
                    Id = "SCHE298",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 24840000.0m,
                    Children_price = 17388000.0m,
                    Discount = 28,
                    Create_at = new DateTime(2024, 8, 11, 9, 30, 0),
                    Tour_Id = "TOUR060"
                },
                new Schedule
                {
                    Id = "SCHE299",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 34500000.0m,
                    Children_price = 24150000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 4, 14, 11, 0, 0),
                    Tour_Id = "TOUR060"
                },
                new Schedule
                {
                    Id = "SCHE300",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 2,
                    Status = 1,
                    Adult_price = 20700000.0m,
                    Children_price = 14490000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2024, 11, 4, 16, 0, 0),
                    Tour_Id = "TOUR060"
                },
                new Schedule
                {
                    Id = "SCHE301",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 24,
                    Status = 1,
                    Adult_price = 10156800.0m,
                    Children_price = 7109760.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 3, 30, 11, 30, 0),
                    Tour_Id = "TOUR061"
                },
                new Schedule
                {
                    Id = "SCHE302",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 9,
                    Status = 1,
                    Adult_price = 11040000.0m,
                    Children_price = 7728000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 1, 19, 10, 5, 0),
                    Tour_Id = "TOUR061"
                },
                new Schedule
                {
                    Id = "SCHE303",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 8280000.0m,
                    Children_price = 5796000.0m,
                    Discount = 25,
                    Create_at = new DateTime(2024, 6, 10, 14, 15, 0),
                    Tour_Id = "TOUR061"
                },
                new Schedule
                {
                    Id = "SCHE304",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 30,
                    Status = 1,
                    Adult_price = 9042800.0m,
                    Children_price = 6330000.0m,
                    Discount = 18,
                    Create_at = new DateTime(2025, 4, 27, 16, 50, 0),
                    Tour_Id = "TOUR061"
                },
                new Schedule
                {
                    Id = "SCHE305",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 6,
                    Status = 1,
                    Adult_price = 7728000.0m,
                    Children_price = 5409600.0m,
                    Discount = 30,
                    Create_at = new DateTime(2024, 10, 25, 9, 10, 0),
                    Tour_Id = "TOUR061"
                },
                new Schedule
                {
                    Id = "SCHE306",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 18,
                    Status = 1,
                    Adult_price = 11385000.0m,
                    Children_price = 7969500.0m,
                    Discount = 10,
                    Create_at = new DateTime(2024, 2, 11, 10, 50, 0),
                    Tour_Id = "TOUR062"
                },
                new Schedule
                {
                    Id = "SCHE307",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 7,
                    Status = 1,
                    Adult_price = 11638000.0m,
                    Children_price = 8146600.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 5, 24, 15, 30, 0),
                    Tour_Id = "TOUR062"
                },
                new Schedule
                {
                    Id = "SCHE308",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 8855000.0m,
                    Children_price = 6198500.0m,
                    Discount = 30,
                    Create_at = new DateTime(2025, 3, 20, 9, 20, 0),
                    Tour_Id = "TOUR062"
                },
                new Schedule
                {
                    Id = "SCHE309",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 10752500.0m,
                    Children_price = 7526750.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 8, 30, 11, 40, 0),
                    Tour_Id = "TOUR062"
                },
                new Schedule
                {
                    Id = "SCHE310",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 4,
                    Status = 1,
                    Adult_price = 7590000.0m,
                    Children_price = 5313000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 2, 7, 17, 20, 0),
                    Tour_Id = "TOUR062"
                },
                new Schedule
                {
                    Id = "SCHE311",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 25,
                    Status = 1,
                    Adult_price = 16732500.0m,
                    Children_price = 11712750.0m,
                    Discount = 3,
                    Create_at = new DateTime(2024, 1, 15, 10, 20, 0),
                    Tour_Id = "TOUR063"
                },
                new Schedule
                {
                    Id = "SCHE312",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 10,
                    Status = 1,
                    Adult_price = 14317500.0m,
                    Children_price = 10022250.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 5, 1, 14, 20, 0),
                    Tour_Id = "TOUR063"
                },
                new Schedule
                {
                    Id = "SCHE313",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 13455000.0m,
                    Children_price = 9418500.0m,
                    Discount = 22,
                    Create_at = new DateTime(2024, 7, 21, 9, 10, 0),
                    Tour_Id = "TOUR063"
                },
                new Schedule
                {
                    Id = "SCHE314",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 20,
                    Status = 1,
                    Adult_price = 16215000.0m,
                    Children_price = 11350500.0m,
                    Discount = 6,
                    Create_at = new DateTime(2025, 2, 28, 11, 50, 0),
                    Tour_Id = "TOUR063"
                },
                new Schedule
                {
                    Id = "SCHE315",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 11385000.0m,
                    Children_price = 7969500.0m,
                    Discount = 34,
                    Create_at = new DateTime(2024, 11, 24, 17, 0, 0),
                    Tour_Id = "TOUR063"
                },
                new Schedule
                {
                    Id = "SCHE316",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 8,
                    Status = 1,
                    Adult_price = 16928000.0m,
                    Children_price = 11849600.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 3, 9, 10, 0, 0),
                    Tour_Id = "TOUR064"
                },
                new Schedule
                {
                    Id = "SCHE317",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 3,
                    Status = 1,
                    Adult_price = 57500000.0m,
                    Children_price = 40250000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 7, 3, 14, 0, 0),
                    Tour_Id = "TOUR064"
                },
                new Schedule
                {
                    Id = "SCHE318",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 43125000.0m,
                    Children_price = 30187500.0m,
                    Discount = 25,
                    Create_at = new DateTime(2025, 1, 31, 10, 10, 0),
                    Tour_Id = "TOUR064"
                },
                new Schedule
                {
                    Id = "SCHE319",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 47150000.0m,
                    Children_price = 33005000.0m,
                    Discount = 18,
                    Create_at = new DateTime(2024, 9, 15, 16, 40, 0),
                    Tour_Id = "TOUR064"
                },
                new Schedule
                {
                    Id = "SCHE320",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 1,
                    Status = 1,
                    Adult_price = 40250000.0m,
                    Children_price = 28175000.0m,
                    Discount = 30,
                    Create_at = new DateTime(2025, 5, 7, 8, 55, 0),
                    Tour_Id = "TOUR064"
                },
                new Schedule
                {
                    Id = "SCHE321",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 8372000.0m,
                    Children_price = 5860400.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 4, 24, 11, 10, 0),
                    Tour_Id = "TOUR065"
                },
                new Schedule
                {
                    Id = "SCHE322",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 7,
                    Status = 1,
                    Adult_price = 7636000.0m,
                    Children_price = 5345200.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 7, 17, 15, 10, 0),
                    Tour_Id = "TOUR065"
                },
                new Schedule
                {
                    Id = "SCHE323",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 6624000.0m,
                    Children_price = 4636800.0m,
                    Discount = 28,
                    Create_at = new DateTime(2025, 3, 14, 9, 5, 0),
                    Tour_Id = "TOUR065"
                },
                new Schedule
                {
                    Id = "SCHE324",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 20,
                    Status = 1,
                    Adult_price = 9200000.0m,
                    Children_price = 6440000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 10, 2, 14, 40, 0),
                    Tour_Id = "TOUR065"
                },
                new Schedule
                {
                    Id = "SCHE325",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 4,
                    Status = 1,
                    Adult_price = 5520000.0m,
                    Children_price = 3864000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 3, 27, 10, 10, 0),
                    Tour_Id = "TOUR065"
                },
                new Schedule
                {
                    Id = "SCHE326",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 35,
                    Status = 1,
                    Adult_price = 6182400.0m,
                    Children_price = 4327680.0m,
                    Discount = 4,
                    Create_at = new DateTime(2024, 1, 28, 10, 0, 0),
                    Tour_Id = "TOUR066"
                },
                new Schedule
                {
                    Id = "SCHE327",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 5216400.0m,
                    Children_price = 3651480.0m,
                    Discount = 19,
                    Create_at = new DateTime(2024, 5, 5, 14, 0, 0),
                    Tour_Id = "TOUR066"
                },
                new Schedule
                {
                    Id = "SCHE328",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 4969600.0m,
                    Children_price = 3478720.0m,
                    Discount = 23,
                    Create_at = new DateTime(2024, 8, 18, 9, 30, 0),
                    Tour_Id = "TOUR066"
                },
                new Schedule
                {
                    Id = "SCHE329",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 28,
                    Status = 1,
                    Adult_price = 5924800.0m,
                    Children_price = 4147360.0m,
                    Discount = 8,
                    Create_at = new DateTime(2025, 3, 17, 11, 0, 0),
                    Tour_Id = "TOUR066"
                },
                new Schedule
                {
                    Id = "SCHE330",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 10,
                    Status = 1,
                    Adult_price = 4121600.0m,
                    Children_price = 2885120.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 12, 15, 16, 0, 0),
                    Tour_Id = "TOUR066"
                },
                new Schedule
                {
                    Id = "SCHE331",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 12,
                    Status = 1,
                    Adult_price = 12558000.0m,
                    Children_price = 8790600.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 2, 24, 10, 30, 0),
                    Tour_Id = "TOUR067"
                },
                new Schedule
                {
                    Id = "SCHE332",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 4,
                    Status = 1,
                    Adult_price = 11454000.0m,
                    Children_price = 8017800.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 6, 3, 15, 0, 0),
                    Tour_Id = "TOUR067"
                },
                new Schedule
                {
                    Id = "SCHE333",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 9936000.0m,
                    Children_price = 6955200.0m,
                    Discount = 28,
                    Create_at = new DateTime(2024, 8, 26, 9, 0, 0),
                    Tour_Id = "TOUR067"
                },
                new Schedule
                {
                    Id = "SCHE334",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 13800000.0m,
                    Children_price = 9660000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 4, 9, 11, 45, 0),
                    Tour_Id = "TOUR067"
                },
                new Schedule
                {
                    Id = "SCHE335",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 2,
                    Status = 1,
                    Adult_price = 8280000.0m,
                    Children_price = 5796000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2024, 10, 15, 17, 0, 0),
                    Tour_Id = "TOUR067"
                },
                new Schedule
                {
                    Id = "SCHE336",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 8040800.0m,
                    Children_price = 5628560.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 3, 10, 10, 0, 0),
                    Tour_Id = "TOUR068"
                },
                new Schedule
                {
                    Id = "SCHE337",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 7429000.0m,
                    Children_price = 5200300.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 6, 28, 15, 0, 0),
                    Tour_Id = "TOUR068"
                },
                new Schedule
                {
                    Id = "SCHE338",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 6992000.0m,
                    Children_price = 4894400.0m,
                    Discount = 20,
                    Create_at = new DateTime(2025, 1, 30, 9, 30, 0),
                    Tour_Id = "TOUR068"
                },
                new Schedule
                {
                    Id = "SCHE339",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 8740000.0m,
                    Children_price = 6118000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 14, 14, 0, 0),
                    Tour_Id = "TOUR068"
                },
                new Schedule
                {
                    Id = "SCHE340",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 3,
                    Status = 1,
                    Adult_price = 6118000.0m,
                    Children_price = 4282600.0m,
                    Discount = 30,
                    Create_at = new DateTime(2025, 5, 1, 17, 30, 0),
                    Tour_Id = "TOUR068"
                },
                new Schedule
                {
                    Id = "SCHE341",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 10,
                    Status = 1,
                    Adult_price = 14792000.0m,
                    Children_price = 10354400.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 3, 13, 10, 30, 0),
                    Tour_Id = "TOUR069"
                },
                new Schedule
                {
                    Id = "SCHE342",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 3,
                    Status = 1,
                    Adult_price = 13685000.0m,
                    Children_price = 9579500.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 6, 21, 15, 0, 0),
                    Tour_Id = "TOUR069"
                },
                new Schedule
                {
                    Id = "SCHE343",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 12880000.0m,
                    Children_price = 9016000.0m,
                    Discount = 20,
                    Create_at = new DateTime(2025, 1, 25, 9, 30, 0),
                    Tour_Id = "TOUR069"
                },
                new Schedule
                {
                    Id = "SCHE344",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 16100000.0m,
                    Children_price = 11270000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 6, 14, 0, 0),
                    Tour_Id = "TOUR069"
                },
                new Schedule
                {
                    Id = "SCHE345",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 1,
                    Status = 1,
                    Adult_price = 9660000.0m,
                    Children_price = 6762000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 4, 29, 17, 30, 0),
                    Tour_Id = "TOUR069"
                },
                new Schedule
                {
                    Id = "SCHE346",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 18837000.0m,
                    Children_price = 13185900.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 1, 8, 10, 0, 0),
                    Tour_Id = "TOUR070"
                },
                new Schedule
                {
                    Id = "SCHE347",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 17201000.0m,
                    Children_price = 12040700.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 4, 22, 14, 0, 0),
                    Tour_Id = "TOUR070"
                },
                new Schedule
                {
                    Id = "SCHE348",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 14904000.0m,
                    Children_price = 10432800.0m,
                    Discount = 28,
                    Create_at = new DateTime(2024, 7, 11, 9, 0, 0),
                    Tour_Id = "TOUR070"
                },
                new Schedule
                {
                    Id = "SCHE349",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 20700000.0m,
                    Children_price = 14490000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 2, 24, 11, 30, 0),
                    Tour_Id = "TOUR070"
                },
                new Schedule
                {
                    Id = "SCHE350",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 3,
                    Status = 1,
                    Adult_price = 12420000.0m,
                    Children_price = 8694000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2024, 10, 28, 16, 45, 0),
                    Tour_Id = "TOUR070"
                },
                new Schedule
                {
                    Id = "SCHE351",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 20,
                    Status = 1,
                    Adult_price = 7326000.0m,
                    Children_price = 5128200.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 4, 17, 11, 0, 0),
                    Tour_Id = "TOUR071"
                },
                new Schedule
                {
                    Id = "SCHE352",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 6681500.0m,
                    Children_price = 4677050.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 7, 10, 15, 0, 0),
                    Tour_Id = "TOUR071"
                },
                new Schedule
                {
                    Id = "SCHE353",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 5796000.0m,
                    Children_price = 4057200.0m,
                    Discount = 28,
                    Create_at = new DateTime(2025, 3, 10, 9, 0, 0),
                    Tour_Id = "TOUR071"
                },
                new Schedule
                {
                    Id = "SCHE354",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 8050000.0m,
                    Children_price = 5635000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 25, 14, 30, 0),
                    Tour_Id = "TOUR071"
                },
                new Schedule
                {
                    Id = "SCHE355",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 4830000.0m,
                    Children_price = 3381000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 3, 29, 10, 0, 0),
                    Tour_Id = "TOUR071"
                },
                new Schedule
                {
                    Id = "SCHE356",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 13754000.0m,
                    Children_price = 9627800.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 1, 31, 10, 0, 0),
                    Tour_Id = "TOUR072"
                },
                new Schedule
                {
                    Id = "SCHE357",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 12707500.0m,
                    Children_price = 8895250.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 5, 8, 14, 0, 0),
                    Tour_Id = "TOUR072"
                },
                new Schedule
                {
                    Id = "SCHE358",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 11960000.0m,
                    Children_price = 8372000.0m,
                    Discount = 20,
                    Create_at = new DateTime(2024, 8, 20, 9, 30, 0),
                    Tour_Id = "TOUR072"
                },
                new Schedule
                {
                    Id = "SCHE359",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 14950000.0m,
                    Children_price = 10465000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 3, 20, 11, 0, 0),
                    Tour_Id = "TOUR072"
                },
                new Schedule
                {
                    Id = "SCHE360",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 3,
                    Status = 1,
                    Adult_price = 8970000.0m,
                    Children_price = 6279000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2024, 12, 17, 16, 0, 0),
                    Tour_Id = "TOUR072"
                },
                new Schedule
                {
                    Id = "SCHE361",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 8,
                    Status = 1,
                    Adult_price = 10046400.0m,
                    Children_price = 7032480.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 2, 7, 10, 30, 0),
                    Tour_Id = "TOUR073"
                },
                new Schedule
                {
                    Id = "SCHE362",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 3,
                    Status = 1,
                    Adult_price = 9163200.0m,
                    Children_price = 6414240.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 5, 13, 15, 0, 0),
                    Tour_Id = "TOUR073"
                },
                new Schedule
                {
                    Id = "SCHE363",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 7948800.0m,
                    Children_price = 5564160.0m,
                    Discount = 28,
                    Create_at = new DateTime(2024, 8, 4, 9, 0, 0),
                    Tour_Id = "TOUR073"
                },
                new Schedule
                {
                    Id = "SCHE364",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 10,
                    Status = 1,
                    Adult_price = 11040000.0m,
                    Children_price = 7728000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 4, 2, 11, 45, 0),
                    Tour_Id = "TOUR073"
                },
                new Schedule
                {
                    Id = "SCHE365",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 1,
                    Status = 1,
                    Adult_price = 6624000.0m,
                    Children_price = 4636800.0m,
                    Discount = 40,
                    Create_at = new DateTime(2024, 10, 11, 17, 0, 0),
                    Tour_Id = "TOUR073"
                },
                new Schedule
                {
                    Id = "SCHE366",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 8887200.0m,
                    Children_price = 6221040.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 3, 1, 10, 0, 0),
                    Tour_Id = "TOUR074"
                },
                new Schedule
                {
                    Id = "SCHE367",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 8211000.0m,
                    Children_price = 5747700.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 6, 25, 14, 30, 0),
                    Tour_Id = "TOUR074"
                },
                new Schedule
                {
                    Id = "SCHE368",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 7728000.0m,
                    Children_price = 5409600.0m,
                    Discount = 20,
                    Create_at = new DateTime(2025, 1, 27, 9, 30, 0),
                    Tour_Id = "TOUR074"
                },
                new Schedule
                {
                    Id = "SCHE369",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 9660000.0m,
                    Children_price = 6762000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 10, 16, 0, 0),
                    Tour_Id = "TOUR074"
                },
                new Schedule
                {
                    Id = "SCHE370",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 3,
                    Status = 1,
                    Adult_price = 5796000.0m,
                    Children_price = 4057200.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 5, 4, 17, 30, 0),
                    Tour_Id = "TOUR074"
                },
                new Schedule
                {
                    Id = "SCHE371",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 8,
                    Status = 1,
                    Adult_price = 16928000.0m,
                    Children_price = 11849600.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 3, 15, 10, 30, 0),
                    Tour_Id = "TOUR075"
                },
                new Schedule
                {
                    Id = "SCHE372",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 3,
                    Status = 1,
                    Adult_price = 15640000.0m,
                    Children_price = 10948000.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 6, 26, 15, 0, 0),
                    Tour_Id = "TOUR075"
                },
                new Schedule
                {
                    Id = "SCHE373",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 14720000.0m,
                    Children_price = 10304000.0m,
                    Discount = 20,
                    Create_at = new DateTime(2025, 1, 29, 9, 30, 0),
                    Tour_Id = "TOUR075"
                },
                new Schedule
                {
                    Id = "SCHE374",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 18400000.0m,
                    Children_price = 12880000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 11, 14, 0, 0),
                    Tour_Id = "TOUR075"
                },
                new Schedule
                {
                    Id = "SCHE375",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 1,
                    Status = 1,
                    Adult_price = 11040000.0m,
                    Children_price = 7728000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 5, 6, 17, 30, 0),
                    Tour_Id = "TOUR075"
                },
                new Schedule
                {
                    Id = "SCHE376",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 25,
                    Status = 1,
                    Adult_price = 6348000.0m,
                    Children_price = 4443600.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 2, 10, 10, 0, 0),
                    Tour_Id = "TOUR076"
                },
                new Schedule
                {
                    Id = "SCHE377",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 5865000.0m,
                    Children_price = 4105500.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 5, 16, 14, 0, 0),
                    Tour_Id = "TOUR076"
                },
                new Schedule
                {
                    Id = "SCHE378",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 5451000.0m,
                    Children_price = 3815700.0m,
                    Discount = 21,
                    Create_at = new DateTime(2024, 8, 9, 9, 30, 0),
                    Tour_Id = "TOUR076"
                },
                new Schedule
                {
                    Id = "SCHE379",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 30,
                    Status = 1,
                    Adult_price = 6900000.0m,
                    Children_price = 4830000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 3, 15, 11, 0, 0),
                    Tour_Id = "TOUR076"
                },
                new Schedule
                {
                    Id = "SCHE380",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 4416000.0m,
                    Children_price = 3091200.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 11, 10, 16, 0, 0),
                    Tour_Id = "TOUR076"
                },
                new Schedule
                {
                    Id = "SCHE381",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 10,
                    Status = 1,
                    Adult_price = 14792000.0m,
                    Children_price = 10354400.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 3, 5, 10, 30, 0),
                    Tour_Id = "TOUR077"
                },
                new Schedule
                {
                    Id = "SCHE382",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 3,
                    Status = 1,
                    Adult_price = 13685000.0m,
                    Children_price = 9579500.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 6, 28, 15, 0, 0),
                    Tour_Id = "TOUR077"
                },
                new Schedule
                {
                    Id = "SCHE383",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 12880000.0m,
                    Children_price = 9016000.0m,
                    Discount = 20,
                    Create_at = new DateTime(2025, 1, 30, 9, 30, 0),
                    Tour_Id = "TOUR077"
                },
                new Schedule
                {
                    Id = "SCHE384",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 16100000.0m,
                    Children_price = 11270000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 13, 14, 0, 0),
                    Tour_Id = "TOUR077"
                },
                new Schedule
                {
                    Id = "SCHE385",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 2,
                    Status = 1,
                    Adult_price = 9660000.0m,
                    Children_price = 6762000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 5, 2, 17, 30, 0),
                    Tour_Id = "TOUR077"
                },
                new Schedule
                {
                    Id = "SCHE386",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 20,
                    Status = 1,
                    Adult_price = 7406000.0m,
                    Children_price = 5184200.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 1, 7, 10, 0, 0),
                    Tour_Id = "TOUR078"
                },
                new Schedule
                {
                    Id = "SCHE387",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 6842500.0m,
                    Children_price = 4789750.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 4, 20, 14, 30, 0),
                    Tour_Id = "TOUR078"
                },
                new Schedule
                {
                    Id = "SCHE388",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 6440000.0m,
                    Children_price = 4508000.0m,
                    Discount = 20,
                    Create_at = new DateTime(2024, 7, 19, 9, 0, 0),
                    Tour_Id = "TOUR078"
                },
                new Schedule
                {
                    Id = "SCHE389",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 8050000.0m,
                    Children_price = 5635000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 2, 15, 11, 0, 0),
                    Tour_Id = "TOUR078"
                },
                new Schedule
                {
                    Id = "SCHE390",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 4830000.0m,
                    Children_price = 3381000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2024, 11, 8, 16, 0, 0),
                    Tour_Id = "TOUR078"
                },
                new Schedule
                {
                    Id = "SCHE391",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 8,
                    Status = 1,
                    Adult_price = 37008000.0m,
                    Children_price = 25905600.0m,
                    Discount = 11,
                    Create_at = new DateTime(2024, 3, 18, 10, 30, 0),
                    Tour_Id = "TOUR079"
                },
                new Schedule
                {
                    Id = "SCHE392",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 3,
                    Status = 1,
                    Adult_price = 35190000.0m,
                    Children_price = 24633000.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 6, 20, 15, 0, 0),
                    Tour_Id = "TOUR079"
                },
                new Schedule
                {
                    Id = "SCHE393",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 31878000.0m,
                    Children_price = 22314600.0m,
                    Discount = 23,
                    Create_at = new DateTime(2025, 1, 23, 9, 30, 0),
                    Tour_Id = "TOUR079"
                },
                new Schedule
                {
                    Id = "SCHE394",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 41400000.0m,
                    Children_price = 28980000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 5, 14, 0, 0),
                    Tour_Id = "TOUR079"
                },
                new Schedule
                {
                    Id = "SCHE395",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 1,
                    Status = 1,
                    Adult_price = 24840000.0m,
                    Children_price = 17388000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 5, 8, 17, 30, 0),
                    Tour_Id = "TOUR079"
                },
                new Schedule
                {
                    Id = "SCHE396",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 6884800.0m,
                    Children_price = 4819360.0m,
                    Discount = 6,
                    Create_at = new DateTime(2024, 2, 14, 10, 0, 0),
                    Tour_Id = "TOUR080"
                },
                new Schedule
                {
                    Id = "SCHE397",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 6256000.0m,
                    Children_price = 4379200.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 5, 19, 14, 30, 0),
                    Tour_Id = "TOUR080"
                },
                new Schedule
                {
                    Id = "SCHE398",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 5961600.0m,
                    Children_price = 4173120.0m,
                    Discount = 19,
                    Create_at = new DateTime(2024, 8, 10, 9, 30, 0),
                    Tour_Id = "TOUR080"
                },
                new Schedule
                {
                    Id = "SCHE399",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 7360000.0m,
                    Children_price = 5152000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 3, 12, 11, 0, 0),
                    Tour_Id = "TOUR080"
                },
                new Schedule
                {
                    Id = "SCHE400",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 3,
                    Status = 1,
                    Adult_price = 4416000.0m,
                    Children_price = 3091200.0m,
                    Discount = 40,
                    Create_at = new DateTime(2024, 11, 5, 16, 0, 0),
                    Tour_Id = "TOUR080"
                },
                new Schedule
                {
                    Id = "SCHE401",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 28,
                    Status = 1,
                    Adult_price = 5924800.0m,
                    Children_price = 4147360.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 3, 31, 10, 40, 0),
                    Tour_Id = "TOUR081"
                },
                new Schedule
                {
                    Id = "SCHE402",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 10,
                    Status = 1,
                    Adult_price = 6440000.0m,
                    Children_price = 4508000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 1, 20, 9, 45, 0),
                    Tour_Id = "TOUR081"
                },
                new Schedule
                {
                    Id = "SCHE403",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 4830000.0m,
                    Children_price = 3381000.0m,
                    Discount = 25,
                    Create_at = new DateTime(2024, 6, 11, 14, 20, 0),
                    Tour_Id = "TOUR081"
                },
                new Schedule
                {
                    Id = "SCHE404",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 35,
                    Status = 1,
                    Adult_price = 5281000.0m,
                    Children_price = 3696700.0m,
                    Discount = 18,
                    Create_at = new DateTime(2025, 4, 29, 17, 0, 0),
                    Tour_Id = "TOUR081"
                },
                new Schedule
                {
                    Id = "SCHE405",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 4508000.0m,
                    Children_price = 3155600.0m,
                    Discount = 30,
                    Create_at = new DateTime(2024, 10, 26, 9, 20, 0),
                    Tour_Id = "TOUR081"
                },
                new Schedule
                {
                    Id = "SCHE406",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 20,
                    Status = 1,
                    Adult_price = 6624000.0m,
                    Children_price = 4636800.0m,
                    Discount = 10,
                    Create_at = new DateTime(2024, 2, 14, 11, 0, 0),
                    Tour_Id = "TOUR082"
                },
                new Schedule
                {
                    Id = "SCHE407",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 6771200.0m,
                    Children_price = 4739840.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 5, 27, 15, 40, 0),
                    Tour_Id = "TOUR082"
                },
                new Schedule
                {
                    Id = "SCHE408",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 5152000.0m,
                    Children_price = 3606400.0m,
                    Discount = 30,
                    Create_at = new DateTime(2025, 3, 23, 9, 30, 0),
                    Tour_Id = "TOUR082"
                },
                new Schedule
                {
                    Id = "SCHE409",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 6256000.0m,
                    Children_price = 4379200.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 9, 1, 11, 50, 0),
                    Tour_Id = "TOUR082"
                },
                new Schedule
                {
                    Id = "SCHE410",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 4,
                    Status = 1,
                    Adult_price = 4416000.0m,
                    Children_price = 3091200.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 2, 10, 17, 30, 0),
                    Tour_Id = "TOUR082"
                },
                new Schedule
                {
                    Id = "SCHE411",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 10039500.0m,
                    Children_price = 7027650.0m,
                    Discount = 3,
                    Create_at = new DateTime(2024, 1, 17, 10, 25, 0),
                    Tour_Id = "TOUR083"
                },
                new Schedule
                {
                    Id = "SCHE412",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 8580000.0m,
                    Children_price = 6006000.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 5, 6, 14, 35, 0),
                    Tour_Id = "TOUR083"
                },
                new Schedule
                {
                    Id = "SCHE413",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 8073000.0m,
                    Children_price = 5651100.0m,
                    Discount = 22,
                    Create_at = new DateTime(2024, 7, 23, 9, 15, 0),
                    Tour_Id = "TOUR083"
                },
                new Schedule
                {
                    Id = "SCHE414",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 9730000.0m,
                    Children_price = 6811000.0m,
                    Discount = 6,
                    Create_at = new DateTime(2025, 3, 3, 11, 55, 0),
                    Tour_Id = "TOUR083"
                },
                new Schedule
                {
                    Id = "SCHE415",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 3,
                    Status = 1,
                    Adult_price = 6831000.0m,
                    Children_price = 4781700.0m,
                    Discount = 34,
                    Create_at = new DateTime(2024, 11, 26, 17, 10, 0),
                    Tour_Id = "TOUR083"
                },
                new Schedule
                {
                    Id = "SCHE416",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 7486500.0m,
                    Children_price = 5240550.0m,
                    Discount = 7,
                    Create_at = new DateTime(2024, 3, 11, 9, 25, 0),
                    Tour_Id = "TOUR084"
                },
                new Schedule
                {
                    Id = "SCHE417",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 6842500.0m,
                    Children_price = 4789750.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 7, 4, 14, 10, 0),
                    Tour_Id = "TOUR084"
                },
                new Schedule
                {
                    Id = "SCHE418",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 6440000.0m,
                    Children_price = 4508000.0m,
                    Discount = 20,
                    Create_at = new DateTime(2025, 2, 1, 10, 15, 0),
                    Tour_Id = "TOUR084"
                },
                new Schedule
                {
                    Id = "SCHE419",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 8050000.0m,
                    Children_price = 5635000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 16, 16, 45, 0),
                    Tour_Id = "TOUR084"
                },
                new Schedule
                {
                    Id = "SCHE420",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 3,
                    Status = 1,
                    Adult_price = 5635000.0m,
                    Children_price = 3944500.0m,
                    Discount = 30,
                    Create_at = new DateTime(2025, 5, 9, 8, 55, 0),
                    Tour_Id = "TOUR084"
                },
                new Schedule
                {
                    Id = "SCHE421",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 20,
                    Status = 1,
                    Adult_price = 8372000.0m,
                    Children_price = 5860400.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 4, 19, 11, 20, 0),
                    Tour_Id = "TOUR085"
                },
                new Schedule
                {
                    Id = "SCHE422",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 7636000.0m,
                    Children_price = 5345200.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 7, 12, 15, 15, 0),
                    Tour_Id = "TOUR085"
                },
                new Schedule
                {
                    Id = "SCHE423",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 6624000.0m,
                    Children_price = 4636800.0m,
                    Discount = 28,
                    Create_at = new DateTime(2025, 3, 15, 9, 10, 0),
                    Tour_Id = "TOUR085"
                },
                new Schedule
                {
                    Id = "SCHE424",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 25,
                    Status = 1,
                    Adult_price = 9200000.0m,
                    Children_price = 6440000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 10, 3, 14, 50, 0),
                    Tour_Id = "TOUR085"
                },
                new Schedule
                {
                    Id = "SCHE425",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 5520000.0m,
                    Children_price = 3864000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 3, 28, 10, 20, 0),
                    Tour_Id = "TOUR085"
                },
                new Schedule
                {
                    Id = "SCHE426",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 25,
                    Status = 1,
                    Adult_price = 6624000.0m,
                    Children_price = 4636800.0m,
                    Discount = 4,
                    Create_at = new DateTime(2024, 1, 29, 10, 5, 0),
                    Tour_Id = "TOUR086"
                },
                new Schedule
                {
                    Id = "SCHE427",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 5589000.0m,
                    Children_price = 3912300.0m,
                    Discount = 19,
                    Create_at = new DateTime(2024, 5, 7, 14, 5, 0),
                    Tour_Id = "TOUR086"
                },
                new Schedule
                {
                    Id = "SCHE428",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 5313000.0m,
                    Children_price = 3719100.0m,
                    Discount = 23,
                    Create_at = new DateTime(2024, 8, 19, 9, 35, 0),
                    Tour_Id = "TOUR086"
                },
                new Schedule
                {
                    Id = "SCHE429",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 30,
                    Status = 1,
                    Adult_price = 6348000.0m,
                    Children_price = 4443600.0m,
                    Discount = 8,
                    Create_at = new DateTime(2025, 3, 18, 11, 5, 0),
                    Tour_Id = "TOUR086"
                },
                new Schedule
                {
                    Id = "SCHE430",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 4416000.0m,
                    Children_price = 3091200.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 12, 16, 16, 10, 0),
                    Tour_Id = "TOUR086"
                },
                new Schedule
                {
                    Id = "SCHE431",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 18,
                    Status = 1,
                    Adult_price = 8887200.0m,
                    Children_price = 6221040.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 2, 25, 10, 40, 0),
                    Tour_Id = "TOUR087"
                },
                new Schedule
                {
                    Id = "SCHE432",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 7,
                    Status = 1,
                    Adult_price = 8211000.0m,
                    Children_price = 5747700.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 6, 4, 15, 10, 0),
                    Tour_Id = "TOUR087"
                },
                new Schedule
                {
                    Id = "SCHE433",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 7728000.0m,
                    Children_price = 5409600.0m,
                    Discount = 20,
                    Create_at = new DateTime(2024, 8, 27, 9, 0, 0),
                    Tour_Id = "TOUR087"
                },
                new Schedule
                {
                    Id = "SCHE434",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 9660000.0m,
                    Children_price = 6762000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 4, 10, 11, 50, 0),
                    Tour_Id = "TOUR087"
                },
                new Schedule
                {
                    Id = "SCHE435",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 4,
                    Status = 1,
                    Adult_price = 5796000.0m,
                    Children_price = 4057200.0m,
                    Discount = 40,
                    Create_at = new DateTime(2024, 10, 16, 17, 20, 0),
                    Tour_Id = "TOUR087"
                },
                new Schedule
                {
                    Id = "SCHE436",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 10,
                    Status = 1,
                    Adult_price = 14792000.0m,
                    Children_price = 10354400.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 3, 19, 10, 0, 0),
                    Tour_Id = "TOUR088"
                },
                new Schedule
                {
                    Id = "SCHE437",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 3,
                    Status = 1,
                    Adult_price = 13685000.0m,
                    Children_price = 9579500.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 7, 2, 14, 0, 0),
                    Tour_Id = "TOUR088"
                },
                new Schedule
                {
                    Id = "SCHE438",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 12880000.0m,
                    Children_price = 9016000.0m,
                    Discount = 20,
                    Create_at = new DateTime(2025, 2, 3, 10, 0, 0),
                    Tour_Id = "TOUR088"
                },
                new Schedule
                {
                    Id = "SCHE439",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 16100000.0m,
                    Children_price = 11270000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 17, 16, 30, 0),
                    Tour_Id = "TOUR088"
                },
                new Schedule
                {
                    Id = "SCHE440",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 2,
                    Status = 1,
                    Adult_price = 9660000.0m,
                    Children_price = 6762000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 5, 10, 8, 50, 0),
                    Tour_Id = "TOUR088"
                },
                new Schedule
                {
                    Id = "SCHE441",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 10415000.0m,
                    Children_price = 7290500.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 4, 20, 11, 25, 0),
                    Tour_Id = "TOUR089"
                },
                new Schedule
                {
                    Id = "SCHE442",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 9545000.0m,
                    Children_price = 6681500.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 7, 13, 15, 10, 0),
                    Tour_Id = "TOUR089"
                },
                new Schedule
                {
                    Id = "SCHE443",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 8280000.0m,
                    Children_price = 5796000.0m,
                    Discount = 28,
                    Create_at = new DateTime(2025, 3, 16, 9, 5, 0),
                    Tour_Id = "TOUR089"
                },
                new Schedule
                {
                    Id = "SCHE444",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 11500000.0m,
                    Children_price = 8050000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 10, 4, 14, 45, 0),
                    Tour_Id = "TOUR089"
                },
                new Schedule
                {
                    Id = "SCHE445",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 3,
                    Status = 1,
                    Adult_price = 6900000.0m,
                    Children_price = 4830000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 3, 29, 10, 15, 0),
                    Tour_Id = "TOUR089"
                },
                new Schedule
                {
                    Id = "SCHE446",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 20,
                    Status = 1,
                    Adult_price = 6182400.0m,
                    Children_price = 4327680.0m,
                    Discount = 4,
                    Create_at = new DateTime(2024, 1, 30, 10, 0, 0),
                    Tour_Id = "TOUR090"
                },
                new Schedule
                {
                    Id = "SCHE447",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 5216400.0m,
                    Children_price = 3651480.0m,
                    Discount = 19,
                    Create_at = new DateTime(2024, 5, 9, 14, 0, 0),
                    Tour_Id = "TOUR090"
                },
                new Schedule
                {
                    Id = "SCHE448",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 4969600.0m,
                    Children_price = 3478720.0m,
                    Discount = 23,
                    Create_at = new DateTime(2024, 8, 22, 9, 30, 0),
                    Tour_Id = "TOUR090"
                },
                new Schedule
                {
                    Id = "SCHE449",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 25,
                    Status = 1,
                    Adult_price = 5924800.0m,
                    Children_price = 4147360.0m,
                    Discount = 8,
                    Create_at = new DateTime(2025, 3, 20, 11, 0, 0),
                    Tour_Id = "TOUR090"
                },
                new Schedule
                {
                    Id = "SCHE450",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 4121600.0m,
                    Children_price = 2885120.0m,
                    Discount = 36,
                    Create_at = new DateTime(2024, 12, 18, 16, 0, 0),
                    Tour_Id = "TOUR090"
                },
                new Schedule
                {
                    Id = "SCHE451",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 32,
                    Status = 1,
                    Adult_price = 5290000.0m,
                    Children_price = 3703000.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 3, 4, 10, 45, 0),
                    Tour_Id = "TOUR091"
                },
                new Schedule
                {
                    Id = "SCHE452",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 10,
                    Status = 1,
                    Adult_price = 5750000.0m,
                    Children_price = 4025000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 6, 16, 15, 0, 0),
                    Tour_Id = "TOUR091"
                },
                new Schedule
                {
                    Id = "SCHE453",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 4312500.0m,
                    Children_price = 3018750.0m,
                    Discount = 25,
                    Create_at = new DateTime(2025, 1, 24, 9, 30, 0),
                    Tour_Id = "TOUR091"
                },
                new Schedule
                {
                    Id = "SCHE454",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 40,
                    Status = 1,
                    Adult_price = 4715000.0m,
                    Children_price = 3300500.0m,
                    Discount = 18,
                    Create_at = new DateTime(2024, 9, 7, 16, 30, 0),
                    Tour_Id = "TOUR091"
                },
                new Schedule
                {
                    Id = "SCHE455",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 6,
                    Status = 1,
                    Adult_price = 4025000.0m,
                    Children_price = 2817500.0m,
                    Discount = 30,
                    Create_at = new DateTime(2025, 4, 30, 8, 45, 0),
                    Tour_Id = "TOUR091"
                },
                new Schedule
                {
                    Id = "SCHE456",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 20,
                    Status = 1,
                    Adult_price = 8280000.0m,
                    Children_price = 5796000.0m,
                    Discount = 10,
                    Create_at = new DateTime(2024, 2, 17, 10, 30, 0),
                    Tour_Id = "TOUR092"
                },
                new Schedule
                {
                    Id = "SCHE457",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 8464000.0m,
                    Children_price = 5924800.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 5, 29, 15, 0, 0),
                    Tour_Id = "TOUR092"
                },
                new Schedule
                {
                    Id = "SCHE458",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 6440000.0m,
                    Children_price = 4508000.0m,
                    Discount = 30,
                    Create_at = new DateTime(2025, 3, 25, 9, 30, 0),
                    Tour_Id = "TOUR092"
                },
                new Schedule
                {
                    Id = "SCHE459",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 7820000.0m,
                    Children_price = 5474000.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 9, 3, 11, 0, 0),
                    Tour_Id = "TOUR092"
                },
                new Schedule
                {
                    Id = "SCHE460",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 4,
                    Status = 1,
                    Adult_price = 5520000.0m,
                    Children_price = 3864000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 2, 12, 17, 0, 0),
                    Tour_Id = "TOUR092"
                },
                new Schedule
                {
                    Id = "SCHE461",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 5924800.0m,
                    Children_price = 4147360.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 1, 20, 10, 0, 0),
                    Tour_Id = "TOUR093"
                },
                new Schedule
                {
                    Id = "SCHE462",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 5474000.0m,
                    Children_price = 3831800.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 5, 4, 14, 0, 0),
                    Tour_Id = "TOUR093"
                },
                new Schedule
                {
                    Id = "SCHE463",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 5152000.0m,
                    Children_price = 3606400.0m,
                    Discount = 20,
                    Create_at = new DateTime(2024, 8, 17, 9, 30, 0),
                    Tour_Id = "TOUR093"
                },
                new Schedule
                {
                    Id = "SCHE464",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 6440000.0m,
                    Children_price = 4508000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 3, 16, 11, 0, 0),
                    Tour_Id = "TOUR093"
                },
                new Schedule
                {
                    Id = "SCHE465",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 3,
                    Status = 1,
                    Adult_price = 3864000.0m,
                    Children_price = 2704800.0m,
                    Discount = 40,
                    Create_at = new DateTime(2024, 12, 13, 16, 0, 0),
                    Tour_Id = "TOUR093"
                },
                new Schedule
                {
                    Id = "SCHE466",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 8,
                    Status = 1,
                    Adult_price = 25196000.0m,
                    Children_price = 17637200.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 3, 22, 10, 30, 0),
                    Tour_Id = "TOUR094"
                },
                new Schedule
                {
                    Id = "SCHE467",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 3,
                    Status = 1,
                    Adult_price = 22908000.0m,
                    Children_price = 16035600.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 6, 25, 15, 0, 0),
                    Tour_Id = "TOUR094"
                },
                new Schedule
                {
                    Id = "SCHE468",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 19872000.0m,
                    Children_price = 13910400.0m,
                    Discount = 28,
                    Create_at = new DateTime(2025, 1, 27, 9, 30, 0),
                    Tour_Id = "TOUR094"
                },
                new Schedule
                {
                    Id = "SCHE469",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 27600000.0m,
                    Children_price = 19320000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 8, 14, 0, 0),
                    Tour_Id = "TOUR094"
                },
                new Schedule
                {
                    Id = "SCHE470",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 1,
                    Status = 1,
                    Adult_price = 16560000.0m,
                    Children_price = 11592000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 5, 1, 17, 30, 0),
                    Tour_Id = "TOUR094"
                },
                new Schedule
                {
                    Id = "SCHE471",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 25,
                    Status = 1,
                    Adult_price = 5621200.0m,
                    Children_price = 3934840.0m,
                    Discount = 6,
                    Create_at = new DateTime(2024, 2, 11, 10, 0, 0),
                    Tour_Id = "TOUR095"
                },
                new Schedule
                {
                    Id = "SCHE472",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 10,
                    Status = 1,
                    Adult_price = 5083000.0m,
                    Children_price = 3558100.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 5, 17, 14, 30, 0),
                    Tour_Id = "TOUR095"
                },
                new Schedule
                {
                    Id = "SCHE473",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 4784000.0m,
                    Children_price = 3348800.0m,
                    Discount = 20,
                    Create_at = new DateTime(2024, 8, 10, 9, 30, 0),
                    Tour_Id = "TOUR095"
                },
                new Schedule
                {
                    Id = "SCHE474",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 18,
                    Status = 1,
                    Adult_price = 5980000.0m,
                    Children_price = 4186000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 3, 19, 11, 0, 0),
                    Tour_Id = "TOUR095"
                },
                new Schedule
                {
                    Id = "SCHE475",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 5,
                    Status = 1,
                    Adult_price = 3588000.0m,
                    Children_price = 2511600.0m,
                    Discount = 40,
                    Create_at = new DateTime(2024, 11, 12, 16, 0, 0),
                    Tour_Id = "TOUR095"
                },
                new Schedule
                {
                    Id = "SCHE476",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 20,
                    Status = 1,
                    Adult_price = 10415000.0m,
                    Children_price = 7290500.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 3, 7, 10, 30, 0),
                    Tour_Id = "TOUR096"
                },
                new Schedule
                {
                    Id = "SCHE477",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 8,
                    Status = 1,
                    Adult_price = 9545000.0m,
                    Children_price = 6681500.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 6, 29, 15, 0, 0),
                    Tour_Id = "TOUR096"
                },
                new Schedule
                {
                    Id = "SCHE478",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 8280000.0m,
                    Children_price = 5796000.0m,
                    Discount = 28,
                    Create_at = new DateTime(2025, 2, 1, 9, 30, 0),
                    Tour_Id = "TOUR096"
                },
                new Schedule
                {
                    Id = "SCHE479",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 15,
                    Status = 1,
                    Adult_price = 11500000.0m,
                    Children_price = 8050000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 13, 14, 0, 0),
                    Tour_Id = "TOUR096"
                },
                new Schedule
                {
                    Id = "SCHE480",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 4,
                    Status = 1,
                    Adult_price = 6900000.0m,
                    Children_price = 4830000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 5, 7, 17, 30, 0),
                    Tour_Id = "TOUR096"
                },
                new Schedule
                {
                    Id = "SCHE481",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 16928000.0m,
                    Children_price = 11849600.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 3, 20, 10, 0, 0),
                    Tour_Id = "TOUR097"
                },
                new Schedule
                {
                    Id = "SCHE482",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 15640000.0m,
                    Children_price = 10948000.0m,
                    Discount = 15,
                    Create_at = new DateTime(2024, 6, 22, 15, 0, 0),
                    Tour_Id = "TOUR097"
                },
                new Schedule
                {
                    Id = "SCHE483",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 14720000.0m,
                    Children_price = 10304000.0m,
                    Discount = 20,
                    Create_at = new DateTime(2025, 1, 25, 9, 30, 0),
                    Tour_Id = "TOUR097"
                },
                new Schedule
                {
                    Id = "SCHE484",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 18400000.0m,
                    Children_price = 12880000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 6, 14, 0, 0),
                    Tour_Id = "TOUR097"
                },
                new Schedule
                {
                    Id = "SCHE485",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 3,
                    Status = 1,
                    Adult_price = 11040000.0m,
                    Children_price = 7728000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 4, 29, 17, 30, 0),
                    Tour_Id = "TOUR097"
                },
                new Schedule
                {
                    Id = "SCHE486",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 8,
                    Status = 1,
                    Adult_price = 59248000.0m,
                    Children_price = 41473600.0m,
                    Discount = 8,
                    Create_at = new DateTime(2024, 1, 8, 10, 0, 0),
                    Tour_Id = "TOUR098"
                },
                new Schedule
                {
                    Id = "SCHE487",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 3,
                    Status = 1,
                    Adult_price = 64400000.0m,
                    Children_price = 45080000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 4, 20, 14, 0, 0),
                    Tour_Id = "TOUR098"
                },
                new Schedule
                {
                    Id = "SCHE488",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 48300000.0m,
                    Children_price = 33810000.0m,
                    Discount = 25,
                    Create_at = new DateTime(2024, 7, 19, 9, 0, 0),
                    Tour_Id = "TOUR098"
                },
                new Schedule
                {
                    Id = "SCHE489",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 52810000.0m,
                    Children_price = 36967000.0m,
                    Discount = 18,
                    Create_at = new DateTime(2025, 2, 15, 11, 0, 0),
                    Tour_Id = "TOUR098"
                },
                new Schedule
                {
                    Id = "SCHE490",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 1,
                    Status = 1,
                    Adult_price = 45080000.0m,
                    Children_price = 31556000.0m,
                    Discount = 30,
                    Create_at = new DateTime(2024, 11, 8, 16, 0, 0),
                    Tour_Id = "TOUR098"
                },
                new Schedule
                {
                    Id = "SCHE491",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 15,
                    Status = 1,
                    Adult_price = 25196000.0m,
                    Children_price = 17637200.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 3, 12, 10, 30, 0),
                    Tour_Id = "TOUR099"
                },
                new Schedule
                {
                    Id = "SCHE492",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 22908000.0m,
                    Children_price = 16035600.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 6, 21, 15, 0, 0),
                    Tour_Id = "TOUR099"
                },
                new Schedule
                {
                    Id = "SCHE493",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 19872000.0m,
                    Children_price = 13910400.0m,
                    Discount = 28,
                    Create_at = new DateTime(2025, 1, 25, 9, 30, 0),
                    Tour_Id = "TOUR099"
                },
                new Schedule
                {
                    Id = "SCHE494",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 12,
                    Status = 1,
                    Adult_price = 27600000.0m,
                    Children_price = 19320000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2024, 9, 6, 14, 0, 0),
                    Tour_Id = "TOUR099"
                },
                new Schedule
                {
                    Id = "SCHE495",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 3,
                    Status = 1,
                    Adult_price = 16560000.0m,
                    Children_price = 11592000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2025, 4, 29, 17, 30, 0),
                    Tour_Id = "TOUR099"
                },
                new Schedule
                {
                    Id = "SCHE496",
                    Start_date = new DateOnly(2025, 6, 1),
                    Available = 8,
                    Status = 1,
                    Adult_price = 19883500.0m,
                    Children_price = 13918450.0m,
                    Discount = 9,
                    Create_at = new DateTime(2024, 2, 14, 10, 0, 0),
                    Tour_Id = "TOUR100"
                },
                new Schedule
                {
                    Id = "SCHE497",
                    Start_date = new DateOnly(2025, 7, 15),
                    Available = 3,
                    Status = 1,
                    Adult_price = 18135500.0m,
                    Children_price = 12694850.0m,
                    Discount = 17,
                    Create_at = new DateTime(2024, 5, 19, 14, 30, 0),
                    Tour_Id = "TOUR100"
                },
                new Schedule
                {
                    Id = "SCHE498",
                    Start_date = new DateOnly(2025, 9, 1),
                    Available = 0,
                    Status = 0,
                    Adult_price = 15732000.0m,
                    Children_price = 11012400.0m,
                    Discount = 28,
                    Create_at = new DateTime(2024, 8, 11, 9, 30, 0),
                    Tour_Id = "TOUR100"
                },
                new Schedule
                {
                    Id = "SCHE499",
                    Start_date = new DateOnly(2025, 10, 15),
                    Available = 5,
                    Status = 1,
                    Adult_price = 21850000.0m,
                    Children_price = 15295000.0m,
                    Discount = 0,
                    Create_at = new DateTime(2025, 3, 12, 11, 0, 0),
                    Tour_Id = "TOUR100"
                },
                new Schedule
                {
                    Id = "SCHE500",
                    Start_date = new DateOnly(2025, 12, 1),
                    Available = 1,
                    Status = 1,
                    Adult_price = 13110000.0m,
                    Children_price = 9177000.0m,
                    Discount = 40,
                    Create_at = new DateTime(2024, 11, 5, 16, 0, 0),
                    Tour_Id = "TOUR100"
                }
            );

            modelBuilder.Entity<Image>().HasData(
                new Image
                {
                    Id = "IMG001",
                    Tour_Id = "TOUR001",
                    Url = "upload-tour/zljfmdgrwg7xu4ubfn2b"
                },
                new Image
                {
                    Id = "IMG002",
                    Tour_Id = "TOUR001",
                    Url = "upload-tour/flcmzqc63smhnusnevbe"
                },
                new Image
                {
                    Id = "IMG003",
                    Tour_Id = "TOUR001",
                    Url = "upload-tour/peqpqv71ilcudy3yhre9"
                },
                new Image
                {
                    Id = "IMG004",
                    Tour_Id = "TOUR001",
                    Url = "upload-tour/xgjsdmi2qbyuvsy5tfw0"
                },
                new Image
                {
                    Id = "IMG005",
                    Tour_Id = "TOUR001",
                    Url = "upload-tour/s2vmup2uni0omxisfq7w"
                },
                new Image
                {
                    Id = "IMG006",
                    Tour_Id = "TOUR002",
                    Url = "upload-tour/bpscjkl29ra3inbmc5me"
                },
                new Image
                {
                    Id = "IMG007",
                    Tour_Id = "TOUR002",
                    Url = "upload-tour/ibtkorjrpzt7tv4cctre"
                },
                new Image
                {
                    Id = "IMG008",
                    Tour_Id = "TOUR002",
                    Url = "upload-tour/jtoksgoz5ngclnxtcdms"
                },
                new Image
                {
                    Id = "IMG009",
                    Tour_Id = "TOUR002",
                    Url = "upload-tour/btwytoikcxyjiboybcxm"
                },
                new Image
                {
                    Id = "IMG010",
                    Tour_Id = "TOUR002",
                    Url = "upload-tour/oih6v1osas5pvfmzhvap"
                },
                new Image
                {
                    Id = "IMG011",
                    Tour_Id = "TOUR003",
                    Url = "upload-tour/qgnnx1zbfejmhtl69daj"
                },
                new Image
                {
                    Id = "IMG012",
                    Tour_Id = "TOUR003",
                    Url = "upload-tour/jpugbrj6u1ilsuty6yo1"
                },
                new Image
                {
                    Id = "IMG013",
                    Tour_Id = "TOUR003",
                    Url = "upload-tour/kse8pptzc0jxuylugvrh"
                },
                new Image
                {
                    Id = "IMG014",
                    Tour_Id = "TOUR003",
                    Url = "upload-tour/v2fjt1izryu3oblwycx6"
                },
                new Image
                {
                    Id = "IMG015",
                    Tour_Id = "TOUR003",
                    Url = "upload-tour/mhgh8eq4slrvyjigqu3q"
                },
                new Image
                {
                    Id = "IMG016",
                    Tour_Id = "TOUR004",
                    Url = "upload-tour/gudggcy0hidgwf3aovt6"
                },
                new Image
                {
                    Id = "IMG017",
                    Tour_Id = "TOUR004",
                    Url = "upload-tour/a0cr0j07ucq8stjno8iz"
                },
                new Image
                {
                    Id = "IMG018",
                    Tour_Id = "TOUR004",
                    Url = "upload-tour/ngicfsln4vl3tp7djac2"
                },
                new Image
                {
                    Id = "IMG019",
                    Tour_Id = "TOUR004",
                    Url = "upload-tour/m3ig1baooll4dmhnhvuw"
                },
                new Image
                {
                    Id = "IMG020",
                    Tour_Id = "TOUR004",
                    Url = "upload-tour/gajemduyob6szme3cjjh"
                },
                new Image
                {
                    Id = "IMG021",
                    Tour_Id = "TOUR005",
                    Url = "upload-tour/oyiymwuw2arzqyeqlhmy"
                },
                new Image
                {
                    Id = "IMG022",
                    Tour_Id = "TOUR005",
                    Url = "upload-tour/rolcfeqkq3gh8a5pjawn"
                },
                new Image
                {
                    Id = "IMG023",
                    Tour_Id = "TOUR005",
                    Url = "upload-tour/kjfd9t90xthwexvdzvzw"
                },
                new Image
                {
                    Id = "IMG024",
                    Tour_Id = "TOUR005",
                    Url = "upload-tour/mf2bi8vwb5e4fphxfyyq"
                },
                new Image
                {
                    Id = "IMG025",
                    Tour_Id = "TOUR005",
                    Url = "upload-tour/ei2vsvwrmhmjonzne09b"
                },
                new Image
                {
                    Id = "IMG026",
                    Tour_Id = "TOUR006",
                    Url = "upload-tour/x2j6g2ebovqj5oeanccv"
                },
                new Image
                {
                    Id = "IMG027",
                    Tour_Id = "TOUR006",
                    Url = "upload-tour/d0ixw7cvuoevxcpdkfqf"
                },
                new Image
                {
                    Id = "IMG028",
                    Tour_Id = "TOUR006",
                    Url = "upload-tour/eyrrkiatdc0l3vnsmiua"
                },
                new Image
                {
                    Id = "IMG029",
                    Tour_Id = "TOUR006",
                    Url = "upload-tour/ck4rfhemofsx3qn6alu0"
                },
                new Image
                {
                    Id = "IMG030",
                    Tour_Id = "TOUR006",
                    Url = "upload-tour/sklutiy9ltndoou13wcx"
                },
                new Image
                {
                    Id = "IMG031",
                    Tour_Id = "TOUR007",
                    Url = "upload-tour/npvycmti41s4m8xlsfqs"
                },
                new Image
                {
                    Id = "IMG032",
                    Tour_Id = "TOUR007",
                    Url = "upload-tour/bdroixddnjfm4ttbuitk"
                },
                new Image
                {
                    Id = "IMG033",
                    Tour_Id = "TOUR007",
                    Url = "upload-tour/lwubam9eurlcwleszhub"
                },
                new Image
                {
                    Id = "IMG034",
                    Tour_Id = "TOUR007",
                    Url = "upload-tour/kylzixwvgdrrm1j5ftyt"
                },
                new Image
                {
                    Id = "IMG035",
                    Tour_Id = "TOUR007",
                    Url = "upload-tour/hqkaguc3ra7vopgjhrbf"
                },
                new Image
                {
                    Id = "IMG036",
                    Tour_Id = "TOUR008",
                    Url = "upload-tour/culjn6gf29yeu9nb4v0j"
                },
                new Image
                {
                    Id = "IMG037",
                    Tour_Id = "TOUR008",
                    Url = "upload-tour/syeakmmowjlqxtybqt9t"
                },
                new Image
                {
                    Id = "IMG038",
                    Tour_Id = "TOUR008",
                    Url = "upload-tour/x45schq2sidx73pvisbi"
                },
                new Image
                {
                    Id = "IMG039",
                    Tour_Id = "TOUR008",
                    Url = "upload-tour/n6jfxs6kyolyo1xnovtx"
                },
                new Image
                {
                    Id = "IMG040",
                    Tour_Id = "TOUR008",
                    Url = "upload-tour/u4xcazxgtwnuvsnq5rxn"
                },
                new Image
                {
                    Id = "IMG041",
                    Tour_Id = "TOUR009",
                    Url = "upload-tour/xmcsci5ihzk7egznejf2"
                },
                new Image
                {
                    Id = "IMG042",
                    Tour_Id = "TOUR009",
                    Url = "upload-tour/yjl2uxuv6nvxsgenj6af"
                },
                new Image
                {
                    Id = "IMG043",
                    Tour_Id = "TOUR009",
                    Url = "upload-tour/ubxp41pgfn8gpxs2khj9"
                },
                new Image
                {
                    Id = "IMG044",
                    Tour_Id = "TOUR009",
                    Url = "upload-tour/pfplbqoz16gce5twbzir"
                },
                new Image
                {
                    Id = "IMG045",
                    Tour_Id = "TOUR009",
                    Url = "upload-tour/x5jaz0xbagcg6wva8rea"
                },
                new Image
                {
                    Id = "IMG046",
                    Tour_Id = "TOUR010",
                    Url = "upload-tour/uxnfziiaqxjwyx5faryl"
                },
                new Image
                {
                    Id = "IMG047",
                    Tour_Id = "TOUR010",
                    Url = "upload-tour/o509kzexsluubo348r5i"
                },
                new Image
                {
                    Id = "IMG048",
                    Tour_Id = "TOUR010",
                    Url = "upload-tour/hyquz8mgnt6ykfn7vajz"
                },
                new Image
                {
                    Id = "IMG049",
                    Tour_Id = "TOUR010",
                    Url = "upload-tour/uykdai0wj54rsjcj0e9c"
                },
                new Image
                {
                    Id = "IMG050",
                    Tour_Id = "TOUR010",
                    Url = "upload-tour/cpaok7u8wbf6qnmkrvuk"
                },
                new Image
                {
                    Id = "IMG051",
                    Tour_Id = "TOUR011",
                    Url = "upload-tour/s9c66jdpqzjclfnzn9ai"
                },
                new Image
                {
                    Id = "IMG052",
                    Tour_Id = "TOUR011",
                    Url = "upload-tour/rgly95vmjejnsucn9p8q"
                },
                new Image
                {
                    Id = "IMG053",
                    Tour_Id = "TOUR011",
                    Url = "upload-tour/bbsh79kft8ltzuooeim3"
                },
                new Image
                {
                    Id = "IMG054",
                    Tour_Id = "TOUR011",
                    Url = "upload-tour/p3phv4jhsuyqcag2hjr4"
                },
                new Image
                {
                    Id = "IMG055",
                    Tour_Id = "TOUR011",
                    Url = "upload-tour/vsjcj3apjf1zed3xbdjs"
                },
                new Image
                {
                    Id = "IMG056",
                    Tour_Id = "TOUR012",
                    Url = "upload-tour/yjmcmy1bt4qodyjj3kta"
                },
                new Image
                {
                    Id = "IMG057",
                    Tour_Id = "TOUR012",
                    Url = "upload-tour/cfkjcaqwwtsxggtxbewe"
                },
                new Image
                {
                    Id = "IMG058",
                    Tour_Id = "TOUR012",
                    Url = "upload-tour/yf6imlftn5rmzfqrb06u"
                },
                new Image
                {
                    Id = "IMG059",
                    Tour_Id = "TOUR012",
                    Url = "upload-tour/ymfwp782xgwjxb9sdbwg"
                },
                new Image
                {
                    Id = "IMG060",
                    Tour_Id = "TOUR012",
                    Url = "upload-tour/ewxtwscr3dfbsgi6zzn2"
                },
                new Image
                {
                    Id = "IMG061",
                    Tour_Id = "TOUR013",
                    Url = "upload-tour/ihdfamwn0oaupuphoh3b"
                },
                new Image
                {
                    Id = "IMG062",
                    Tour_Id = "TOUR013",
                    Url = "upload-tour/vriznvejvoeu0qfiybcg"
                },
                new Image
                {
                    Id = "IMG063",
                    Tour_Id = "TOUR013",
                    Url = "upload-tour/g7lidxp1tw7e5uew0vnc"
                },
                new Image
                {
                    Id = "IMG064",
                    Tour_Id = "TOUR013",
                    Url = "upload-tour/khqesgcfluq9roaajsk4"
                },
                new Image
                {
                    Id = "IMG065",
                    Tour_Id = "TOUR013",
                    Url = "upload-tour/aghvqzd0rehs9fkoovxi"
                },
                new Image
                {
                    Id = "IMG066",
                    Tour_Id = "TOUR014",
                    Url = "upload-tour/zmpp5brlmmihmuoplccs"
                },
                new Image
                {
                    Id = "IMG067",
                    Tour_Id = "TOUR014",
                    Url = "upload-tour/sn6fzreicmoebrbimren"
                },
                new Image
                {
                    Id = "IMG068",
                    Tour_Id = "TOUR014",
                    Url = "upload-tour/bhaxmb0wxe7wfdjzigaq"
                },
                new Image
                {
                    Id = "IMG069",
                    Tour_Id = "TOUR014",
                    Url = "upload-tour/qhl0gi04qzvljoswv8mf"
                },
                new Image
                {
                    Id = "IMG070",
                    Tour_Id = "TOUR014",
                    Url = "upload-tour/e8yot8zt4nmskpssebys"
                },
                new Image
                {
                    Id = "IMG071",
                    Tour_Id = "TOUR015",
                    Url = "upload-tour/ydlvpikbulacic3l9rqm"
                },
                new Image
                {
                    Id = "IMG072",
                    Tour_Id = "TOUR015",
                    Url = "upload-tour/ogqwvnj5fm746eiwd0gm"
                },
                new Image
                {
                    Id = "IMG073",
                    Tour_Id = "TOUR015",
                    Url = "upload-tour/pmqe2zf1xwv9s2z0vuy8"
                },
                new Image
                {
                    Id = "IMG074",
                    Tour_Id = "TOUR015",
                    Url = "upload-tour/yasfipu6ddpflew7u55o"
                },
                new Image
                {
                    Id = "IMG075",
                    Tour_Id = "TOUR015",
                    Url = "upload-tour/dtmjxhfjltx15lqtp4e1"
                },
                new Image
                {
                    Id = "IMG076",
                    Tour_Id = "TOUR016",
                    Url = "upload-tour/vtahamlbxvwzii6bteaw"
                },
                new Image
                {
                    Id = "IMG077",
                    Tour_Id = "TOUR016",
                    Url = "upload-tour/jqp2fuaqipeglnslydzt"
                },
                new Image
                {
                    Id = "IMG078",
                    Tour_Id = "TOUR016",
                    Url = "upload-tour/t3tfkwbtysyueo6fvonv"
                },
                new Image
                {
                    Id = "IMG079",
                    Tour_Id = "TOUR016",
                    Url = "upload-tour/kcnjpq2n2uv7ihzleote"
                },
                new Image
                {
                    Id = "IMG080",
                    Tour_Id = "TOUR016",
                    Url = "upload-tour/bz5o1qj77amymu8xbxvu"
                },
                new Image
                {
                    Id = "IMG081",
                    Tour_Id = "TOUR017",
                    Url = "upload-tour/p2t2zqk7gngjelfsyg6s"
                },
                new Image
                {
                    Id = "IMG082",
                    Tour_Id = "TOUR017",
                    Url = "upload-tour/j6cclbp4qivvplgwln55"
                },
                new Image
                {
                    Id = "IMG083",
                    Tour_Id = "TOUR017",
                    Url = "upload-tour/gcadk2mcvbty6b9b9pjg"
                },
                new Image
                {
                    Id = "IMG084",
                    Tour_Id = "TOUR017",
                    Url = "upload-tour/tjfmj2s71qr8a2cmuj8r"
                },
                new Image
                {
                    Id = "IMG085",
                    Tour_Id = "TOUR017",
                    Url = "upload-tour/wbsvblzzf7siuafuuaj7"
                },
                new Image
                {
                    Id = "IMG086",
                    Tour_Id = "TOUR018",
                    Url = "upload-tour/fd7dvkbmic7tuq6ftj7w"
                },
                new Image
                {
                    Id = "IMG087",
                    Tour_Id = "TOUR018",
                    Url = "upload-tour/miso0nhwcdgong5tbztr"
                },
                new Image
                {
                    Id = "IMG088",
                    Tour_Id = "TOUR018",
                    Url = "upload-tour/vld9tn8ly0s8aexron35"
                },
                new Image
                {
                    Id = "IMG089",
                    Tour_Id = "TOUR018",
                    Url = "upload-tour/zowdg6uedys7e16q72tn"
                },
                new Image
                {
                    Id = "IMG090",
                    Tour_Id = "TOUR018",
                    Url = "upload-tour/dkmi9hmng4gh6y2da11h"
                },
                new Image
                {
                    Id = "IMG091",
                    Tour_Id = "TOUR019",
                    Url = "upload-tour/ncf7osl3cpcgjozu1qlu"
                },
                new Image
                {
                    Id = "IMG092",
                    Tour_Id = "TOUR019",
                    Url = "upload-tour/qndgzrav8dyxwifxz5fh"
                },
                new Image
                {
                    Id = "IMG093",
                    Tour_Id = "TOUR019",
                    Url = "upload-tour/nwnbwbahj5z9qq6w7ygg"
                },
                new Image
                {
                    Id = "IMG094",
                    Tour_Id = "TOUR019",
                    Url = "upload-tour/uvjl6sffxjqewsgjlxwv"
                },
                new Image
                {
                    Id = "IMG095",
                    Tour_Id = "TOUR019",
                    Url = "upload-tour/xczqhy2oxjkb8tqraogc"
                },
                new Image
                {
                    Id = "IMG096",
                    Tour_Id = "TOUR020",
                    Url = "upload-tour/i0umkkyopog6yyulp0cu"
                },
                new Image
                {
                    Id = "IMG097",
                    Tour_Id = "TOUR020",
                    Url = "upload-tour/cdfeqjjluowyes5jxzxj"
                },
                new Image
                {
                    Id = "IMG098",
                    Tour_Id = "TOUR020",
                    Url = "upload-tour/suhtyzpbrevfdriucoyi"
                },
                new Image
                {
                    Id = "IMG099",
                    Tour_Id = "TOUR020",
                    Url = "upload-tour/o2hwxr21bebrbzknvmhe"
                },
                new Image
                {
                    Id = "IMG100",
                    Tour_Id = "TOUR020",
                    Url = "upload-tour/zh5juaaq4kmpn9spcgfk"
                },
                new Image
                {
                    Id = "IMG101",
                    Tour_Id = "TOUR021",
                    Url = "upload-tour/vewcq8jcxfapsb6ugqku"
                },
                new Image
                {
                    Id = "IMG102",
                    Tour_Id = "TOUR021",
                    Url = "upload-tour/wlcazuvosu3jfrm4dwve"
                },
                new Image
                {
                    Id = "IMG103",
                    Tour_Id = "TOUR021",
                    Url = "upload-tour/izdt9pbihb0vwl4uhvph"
                },
                new Image
                {
                    Id = "IMG104",
                    Tour_Id = "TOUR021",
                    Url = "upload-tour/fawvhdt0kj7tiqfcj4x4"
                },
                new Image
                {
                    Id = "IMG105",
                    Tour_Id = "TOUR021",
                    Url = "upload-tour/cps2qaatngxsagwch8qg"
                },
                new Image
                {
                    Id = "IMG106",
                    Tour_Id = "TOUR022",
                    Url = "upload-tour/wdqcnai9gznxh68baagl"
                },
                new Image
                {
                    Id = "IMG107",
                    Tour_Id = "TOUR022",
                    Url = "upload-tour/zavpw52owkpjoolftj0q"
                },
                new Image
                {
                    Id = "IMG108",
                    Tour_Id = "TOUR022",
                    Url = "upload-tour/w01gghwli0lh4ttglvif"
                },
                new Image
                {
                    Id = "IMG109",
                    Tour_Id = "TOUR022",
                    Url = "upload-tour/glu0gtohdguq5oxv0jzf"
                },
                new Image
                {
                    Id = "IMG110",
                    Tour_Id = "TOUR022",
                    Url = "upload-tour/vge4mbllowlcfn5apdo0"
                },
                new Image
                {
                    Id = "IMG111",
                    Tour_Id = "TOUR023",
                    Url = "upload-tour/vra1qvfc3iev9yavxcod"
                },
                new Image
                {
                    Id = "IMG112",
                    Tour_Id = "TOUR023",
                    Url = "upload-tour/a58oxzknfkh6anoij9x7"
                },
                new Image
                {
                    Id = "IMG113",
                    Tour_Id = "TOUR023",
                    Url = "upload-tour/etlhfp4gq5ozwbszliex"
                },
                new Image
                {
                    Id = "IMG114",
                    Tour_Id = "TOUR023",
                    Url = "upload-tour/heuoggjylf1gwv4zaz2b"
                },
                new Image
                {
                    Id = "IMG115",
                    Tour_Id = "TOUR023",
                    Url = "upload-tour/nafry7h6mpazznlro9ph"
                },
                new Image
                {
                    Id = "IMG116",
                    Tour_Id = "TOUR024",
                    Url = "upload-tour/eohs8nhpi54r79lob1rx"
                },
                new Image
                {
                    Id = "IMG117",
                    Tour_Id = "TOUR024",
                    Url = "upload-tour/aqufppg8edppq81byero"
                },
                new Image
                {
                    Id = "IMG118",
                    Tour_Id = "TOUR024",
                    Url = "upload-tour/mulrkskpiyxpmcy3ngx0"
                },
                new Image
                {
                    Id = "IMG119",
                    Tour_Id = "TOUR024",
                    Url = "upload-tour/ry0kb4mvpj37dumsvnru"
                },
                new Image
                {
                    Id = "IMG120",
                    Tour_Id = "TOUR024",
                    Url = "upload-tour/zo6b7fcbpg26dy7q81he"
                },
                new Image
                {
                    Id = "IMG121",
                    Tour_Id = "TOUR025",
                    Url = "upload-tour/kwav53bdceponruw1d25"
                },
                new Image
                {
                    Id = "IMG122",
                    Tour_Id = "TOUR025",
                    Url = "upload-tour/qzt7nc1wla4mzyt8xdl7"
                },
                new Image
                {
                    Id = "IMG123",
                    Tour_Id = "TOUR025",
                    Url = "upload-tour/uyimobgvkjbyjey6jyed"
                },
                new Image
                {
                    Id = "IMG124",
                    Tour_Id = "TOUR025",
                    Url = "upload-tour/i3yjggb2fwnqvgjisnci"
                },
                new Image
                {
                    Id = "IMG125",
                    Tour_Id = "TOUR025",
                    Url = "upload-tour/rwzmqe93g6ipjkho8njr"
                },
                new Image
                {
                    Id = "IMG126",
                    Tour_Id = "TOUR026",
                    Url = "upload-tour/rkldkldcahfp0air3rq8"
                },
                new Image
                {
                    Id = "IMG127",
                    Tour_Id = "TOUR026",
                    Url = "upload-tour/fd3nzvrekng4axzlfatr"
                },
                new Image
                {
                    Id = "IMG128",
                    Tour_Id = "TOUR026",
                    Url = "upload-tour/dh9fwtkpm385ennowfn8"
                },
                new Image
                {
                    Id = "IMG129",
                    Tour_Id = "TOUR026",
                    Url = "upload-tour/t2aznjubvfjbqubbno5w"
                },
                new Image
                {
                    Id = "IMG130",
                    Tour_Id = "TOUR026",
                    Url = "upload-tour/hi6rayazyqqxkj2xvhim"
                },
                new Image
                {
                    Id = "IMG131",
                    Tour_Id = "TOUR027",
                    Url = "upload-tour/gfe7bvraxibxbdecsrjm"
                },
                new Image
                {
                    Id = "IMG132",
                    Tour_Id = "TOUR027",
                    Url = "upload-tour/oyqtasx5a9unc7cuvjjo"
                },
                new Image
                {
                    Id = "IMG133",
                    Tour_Id = "TOUR027",
                    Url = "upload-tour/gzyqih8vqgppeoguhkbg"
                },
                new Image
                {
                    Id = "IMG134",
                    Tour_Id = "TOUR027",
                    Url = "upload-tour/k0ryhxsifqshgychligl"
                },
                new Image
                {
                    Id = "IMG135",
                    Tour_Id = "TOUR027",
                    Url = "upload-tour/yld2qgkqiz9ohw18xelp"
                },
                new Image
                {
                    Id = "IMG136",
                    Tour_Id = "TOUR028",
                    Url = "upload-tour/dmi83ia3l3jzqkovruem"
                },
                new Image
                {
                    Id = "IMG137",
                    Tour_Id = "TOUR028",
                    Url = "upload-tour/vwxknoh6aqj4fezjklbn"
                },
                new Image
                {
                    Id = "IMG138",
                    Tour_Id = "TOUR028",
                    Url = "upload-tour/lrqu6pj6dne3pdqm2zsb"
                },
                new Image
                {
                    Id = "IMG139",
                    Tour_Id = "TOUR028",
                    Url = "upload-tour/szmtcsjkjmilsaraum8e"
                },
                new Image
                {
                    Id = "IMG140",
                    Tour_Id = "TOUR028",
                    Url = "upload-tour/hkkxqbtr3bkfkxezuavz"
                },
                new Image
                {
                    Id = "IMG141",
                    Tour_Id = "TOUR029",
                    Url = "upload-tour/nc071vubhcqa0o5pquaz"
                },
                new Image
                {
                    Id = "IMG142",
                    Tour_Id = "TOUR029",
                    Url = "upload-tour/b0rsr3c3fmxpqcgxxkph"
                },
                new Image
                {
                    Id = "IMG143",
                    Tour_Id = "TOUR029",
                    Url = "upload-tour/zyps3csyvftzar370d4f"
                },
                new Image
                {
                    Id = "IMG144",
                    Tour_Id = "TOUR029",
                    Url = "upload-tour/pqleibkdpxzocydujos3"
                },
                new Image
                {
                    Id = "IMG145",
                    Tour_Id = "TOUR029",
                    Url = "upload-tour/k0pww4s3yov7qp93n9yq"
                },
                new Image
                {
                    Id = "IMG146",
                    Tour_Id = "TOUR030",
                    Url = "upload-tour/ojh3qvf28bzaxxwjf2d9"
                },
                new Image
                {
                    Id = "IMG147",
                    Tour_Id = "TOUR030",
                    Url = "upload-tour/pjc5dxfq75odhebfsfcv"
                },
                new Image
                {
                    Id = "IMG148",
                    Tour_Id = "TOUR030",
                    Url = "upload-tour/yxxob3jupjnzh8aj1ne2"
                },
                new Image
                {
                    Id = "IMG149",
                    Tour_Id = "TOUR030",
                    Url = "upload-tour/jpe4r8ivvoxprz1gz6h1"
                },
                new Image
                {
                    Id = "IMG150",
                    Tour_Id = "TOUR030",
                    Url = "upload-tour/aefxs4uvf3njvmtxwkkq"
                },
                new Image
                {
                    Id = "IMG151",
                    Tour_Id = "TOUR031",
                    Url = "upload-tour/kbayi8uoj6fbjwpemuu1"
                },
                new Image
                {
                    Id = "IMG152",
                    Tour_Id = "TOUR031",
                    Url = "upload-tour/dszjufzhpdwtgjrbzgal"
                },
                new Image
                {
                    Id = "IMG153",
                    Tour_Id = "TOUR031",
                    Url = "upload-tour/l4zqmkuspqaxoehli1xg"
                },
                new Image
                {
                    Id = "IMG154",
                    Tour_Id = "TOUR031",
                    Url = "upload-tour/tvmej1rdt5pr5qlwhsdh"
                },
                new Image
                {
                    Id = "IMG155",
                    Tour_Id = "TOUR031",
                    Url = "upload-tour/xfkqpipbvcs4dgkscgxp"
                },
                new Image
                {
                    Id = "IMG156",
                    Tour_Id = "TOUR032",
                    Url = "upload-tour/zzmudvzcl8tzme4faksr"
                },
                new Image
                {
                    Id = "IMG157",
                    Tour_Id = "TOUR032",
                    Url = "upload-tour/oyhqtq8ai0b5f8hx5khj"
                },
                new Image
                {
                    Id = "IMG158",
                    Tour_Id = "TOUR032",
                    Url = "upload-tour/rn9m7sp6yhqddeyvq6ri"
                },
                new Image
                {
                    Id = "IMG159",
                    Tour_Id = "TOUR032",
                    Url = "upload-tour/ytfqlv3it6vqt6lgzsnz"
                },
                new Image
                {
                    Id = "IMG160",
                    Tour_Id = "TOUR032",
                    Url = "upload-tour/toa3h0aqv71vqytoucxe"
                },
                new Image
                {
                    Id = "IMG161",
                    Tour_Id = "TOUR033",
                    Url = "upload-tour/ota7jzbwirbhempryso3"
                },
                new Image
                {
                    Id = "IMG162",
                    Tour_Id = "TOUR033",
                    Url = "upload-tour/neug1boesiash17ho7bg"
                },
                new Image
                {
                    Id = "IMG163",
                    Tour_Id = "TOUR033",
                    Url = "upload-tour/bbxy642lzijahjjpyaqt"
                },
                new Image
                {
                    Id = "IMG164",
                    Tour_Id = "TOUR033",
                    Url = "upload-tour/gwcgd0lm7i8zljd1zpsc"
                },
                new Image
                {
                    Id = "IMG165",
                    Tour_Id = "TOUR033",
                    Url = "upload-tour/twrimqw49gtycmi6v6lp"
                },
                new Image
                {
                    Id = "IMG166",
                    Tour_Id = "TOUR034",
                    Url = "upload-tour/qkq3avoca6nmo5rclyuy"
                },
                new Image
                {
                    Id = "IMG167",
                    Tour_Id = "TOUR034",
                    Url = "upload-tour/yvojzgilve17edzszuts"
                },
                new Image
                {
                    Id = "IMG168",
                    Tour_Id = "TOUR034",
                    Url = "upload-tour/qmwwoqeeydiommht1g1q"
                },
                new Image
                {
                    Id = "IMG169",
                    Tour_Id = "TOUR034",
                    Url = "upload-tour/tzg4ieffv0uxvee488rl"
                },
                new Image
                {
                    Id = "IMG170",
                    Tour_Id = "TOUR034",
                    Url = "upload-tour/dujz8okungkkdxjg4wdi"
                },
                new Image
                {
                    Id = "IMG171",
                    Tour_Id = "TOUR035",
                    Url = "upload-tour/x8vzr8wz3nhdxs9pdz8e"
                },
                new Image
                {
                    Id = "IMG172",
                    Tour_Id = "TOUR035",
                    Url = "upload-tour/w970jpwwlw3tonnrumm9"
                },
                new Image
                {
                    Id = "IMG173",
                    Tour_Id = "TOUR035",
                    Url = "upload-tour/qsjdexxbzdljbbdjvztm"
                },
                new Image
                {
                    Id = "IMG174",
                    Tour_Id = "TOUR035",
                    Url = "upload-tour/e51uaee11ywhjfovoeox"
                },
                new Image
                {
                    Id = "IMG175",
                    Tour_Id = "TOUR035",
                    Url = "upload-tour/x0bwabyvks4ll30tpedp"
                },
                new Image
                {
                    Id = "IMG176",
                    Tour_Id = "TOUR036",
                    Url = "upload-tour/qcu421uk0i0icfpjycaj"
                },
                new Image
                {
                    Id = "IMG177",
                    Tour_Id = "TOUR036",
                    Url = "upload-tour/xyzzye16xe7l16npegil"
                },
                new Image
                {
                    Id = "IMG178",
                    Tour_Id = "TOUR036",
                    Url = "upload-tour/siyhuvjqnikgvmqhyzaa"
                },
                new Image
                {
                    Id = "IMG179",
                    Tour_Id = "TOUR036",
                    Url = "upload-tour/ytic1dxfcvoeasowjvwg"
                },
                new Image
                {
                    Id = "IMG180",
                    Tour_Id = "TOUR036",
                    Url = "upload-tour/jwsiuq9kafust7ifupij"
                },
                new Image
                {
                    Id = "IMG181",
                    Tour_Id = "TOUR037",
                    Url = "upload-tour/jmylxrxpcqcqh2koycj3"
                },
                new Image
                {
                    Id = "IMG182",
                    Tour_Id = "TOUR037",
                    Url = "upload-tour/k4owxljymurbwveyxy7l"
                },
                new Image
                {
                    Id = "IMG183",
                    Tour_Id = "TOUR037",
                    Url = "upload-tour/ptvuidsjiyrp10ih196d"
                },
                new Image
                {
                    Id = "IMG184",
                    Tour_Id = "TOUR037",
                    Url = "upload-tour/px1yqq2tzjfesfzuokxp"
                },
                new Image
                {
                    Id = "IMG185",
                    Tour_Id = "TOUR037",
                    Url = "upload-tour/sodikrxybncbpx7kyzh6"
                },
                new Image
                {
                    Id = "IMG186",
                    Tour_Id = "TOUR038",
                    Url = "upload-tour/iirttcztygbmkltsxkoa"
                },
                new Image
                {
                    Id = "IMG187",
                    Tour_Id = "TOUR038",
                    Url = "upload-tour/vuyr99jutmxoudxhlmwf"
                },
                new Image
                {
                    Id = "IMG188",
                    Tour_Id = "TOUR038",
                    Url = "upload-tour/mlkmbw3obqvvapgvtwes"
                },
                new Image
                {
                    Id = "IMG189",
                    Tour_Id = "TOUR038",
                    Url = "upload-tour/fwyiwfc3c2y6jzfbglz5"
                },
                new Image
                {
                    Id = "IMG190",
                    Tour_Id = "TOUR038",
                    Url = "upload-tour/dagmy32fqvahfmwzdygh"
                },
                new Image
                {
                    Id = "IMG191",
                    Tour_Id = "TOUR039",
                    Url = "upload-tour/lyqttrk73qghhr0meqq7"
                },
                new Image
                {
                    Id = "IMG192",
                    Tour_Id = "TOUR039",
                    Url = "upload-tour/ulv1haklbsgstr2f9cjx"
                },
                new Image
                {
                    Id = "IMG193",
                    Tour_Id = "TOUR039",
                    Url = "upload-tour/jhfc3wpy0jrs6rxyu10z"
                },
                new Image
                {
                    Id = "IMG194",
                    Tour_Id = "TOUR039",
                    Url = "upload-tour/fsgqkjy2sk01pbtfl0zg"
                },
                new Image
                {
                    Id = "IMG195",
                    Tour_Id = "TOUR039",
                    Url = "upload-tour/ptidbztzbgwiqctfnsdu"
                },
                new Image
                {
                    Id = "IMG196",
                    Tour_Id = "TOUR040",
                    Url = "upload-tour/f00vlujjhmyqwdeoroln"
                },
                new Image
                {
                    Id = "IMG197",
                    Tour_Id = "TOUR040",
                    Url = "upload-tour/be6sfs43lqgdqdjmm0ep"
                },
                new Image
                {
                    Id = "IMG198",
                    Tour_Id = "TOUR040",
                    Url = "upload-tour/lpfckgyrer08c4wbqzlh"
                },
                new Image
                {
                    Id = "IMG199",
                    Tour_Id = "TOUR040",
                    Url = "upload-tour/xukrnhdolpgqkmist6lz"
                },
                new Image
                {
                    Id = "IMG200",
                    Tour_Id = "TOUR040",
                    Url = "upload-tour/hgzsvexdqhz6jvewoeaf"
                },
                new Image
                {
                    Id = "IMG201",
                    Tour_Id = "TOUR041",
                    Url = "upload-tour/jhhmaq4fp7orsc8xfz6c"
                },
                new Image
                {
                    Id = "IMG202",
                    Tour_Id = "TOUR041",
                    Url = "upload-tour/ld7txdsyuhxsquzwzevw"
                },
                new Image
                {
                    Id = "IMG203",
                    Tour_Id = "TOUR041",
                    Url = "upload-tour/kgqs1hbbtdt4zfhvuqum"
                },
                new Image
                {
                    Id = "IMG204",
                    Tour_Id = "TOUR041",
                    Url = "upload-tour/xjd3jkqtjlucey3mr6jj"
                },
                new Image
                {
                    Id = "IMG205",
                    Tour_Id = "TOUR041",
                    Url = "upload-tour/w1dx79vnp7yfbye14bhr"
                },
                new Image
                {
                    Id = "IMG206",
                    Tour_Id = "TOUR042",
                    Url = "upload-tour/aa5mdrcowb1ewwb0nvbu"
                },
                new Image
                {
                    Id = "IMG207",
                    Tour_Id = "TOUR042",
                    Url = "upload-tour/m6oltubm7yw4oclczyhl"
                },
                new Image
                {
                    Id = "IMG208",
                    Tour_Id = "TOUR042",
                    Url = "upload-tour/uxmtlj6xdqqislhufker"
                },
                new Image
                {
                    Id = "IMG209",
                    Tour_Id = "TOUR042",
                    Url = "upload-tour/bgzg5vy86di3qjx7yst6"
                },
                new Image
                {
                    Id = "IMG210",
                    Tour_Id = "TOUR042",
                    Url = "upload-tour/uqrgpbhxtf3oebka0bp8"
                },
                new Image
                {
                    Id = "IMG211",
                    Tour_Id = "TOUR043",
                    Url = "upload-tour/m20mh3hybn1vsgjot1cz"
                },
                new Image
                {
                    Id = "IMG212",
                    Tour_Id = "TOUR043",
                    Url = "upload-tour/rde8ckk1b9u32d3abg13"
                },
                new Image
                {
                    Id = "IMG213",
                    Tour_Id = "TOUR043",
                    Url = "upload-tour/tkifppzsaar8aiqxzwyf"
                },
                new Image
                {
                    Id = "IMG214",
                    Tour_Id = "TOUR043",
                    Url = "upload-tour/ghk71artpspcxmvplzve"
                },
                new Image
                {
                    Id = "IMG215",
                    Tour_Id = "TOUR043",
                    Url = "upload-tour/p2ckuah7jbrlyerbmtll"
                },
                new Image
                {
                    Id = "IMG216",
                    Tour_Id = "TOUR044",
                    Url = "upload-tour/a7zpsesuii8apxqb5edq"
                },
                new Image
                {
                    Id = "IMG217",
                    Tour_Id = "TOUR044",
                    Url = "upload-tour/lrh2eog6hn1y5aigrmau"
                },
                new Image
                {
                    Id = "IMG218",
                    Tour_Id = "TOUR044",
                    Url = "upload-tour/s8iq9bnstapva7zbiso6"
                },
                new Image
                {
                    Id = "IMG219",
                    Tour_Id = "TOUR044",
                    Url = "upload-tour/gwvzlk4akztgwfx9x8wt"
                },
                new Image
                {
                    Id = "IMG220",
                    Tour_Id = "TOUR044",
                    Url = "upload-tour/ckt4o49s5mtk6hu8fhhn"
                },
                new Image
                {
                    Id = "IMG221",
                    Tour_Id = "TOUR045",
                    Url = "upload-tour/jjuf67t6fzd0sdl7dybf"
                },
                new Image
                {
                    Id = "IMG222",
                    Tour_Id = "TOUR045",
                    Url = "upload-tour/owxk5jhdzmqiulxt9wvo"
                },
                new Image
                {
                    Id = "IMG223",
                    Tour_Id = "TOUR045",
                    Url = "upload-tour/vuqivxf0auvpj4lowecu"
                },
                new Image
                {
                    Id = "IMG224",
                    Tour_Id = "TOUR045",
                    Url = "upload-tour/wcokr1ompyzhsty5q1fb"
                },
                new Image
                {
                    Id = "IMG225",
                    Tour_Id = "TOUR045",
                    Url = "upload-tour/fpqjsoxiimkffjd1snyt"
                },
                new Image
                {
                    Id = "IMG226",
                    Tour_Id = "TOUR046",
                    Url = "upload-tour/zhz4pvoaayramhoksmcn"
                },
                new Image
                {
                    Id = "IMG227",
                    Tour_Id = "TOUR046",
                    Url = "upload-tour/v8pdcjsj7qzndfv9o62e"
                },
                new Image
                {
                    Id = "IMG228",
                    Tour_Id = "TOUR046",
                    Url = "upload-tour/ncnlbpg1nbaaedyq1xzr"
                },
                new Image
                {
                    Id = "IMG229",
                    Tour_Id = "TOUR046",
                    Url = "upload-tour/ashqc32lfsfkxfqga5km"
                },
                new Image
                {
                    Id = "IMG230",
                    Tour_Id = "TOUR046",
                    Url = "upload-tour/pwhnxahsfhyq4aym2c5q"
                },
                new Image
                {
                    Id = "IMG231",
                    Tour_Id = "TOUR047",
                    Url = "upload-tour/myl4k9d741e4qwpx0e6i"
                },
                new Image
                {
                    Id = "IMG232",
                    Tour_Id = "TOUR047",
                    Url = "upload-tour/esz5dbfjxh7bbyl9minl"
                },
                new Image
                {
                    Id = "IMG233",
                    Tour_Id = "TOUR047",
                    Url = "upload-tour/gzgsj4dxmhgxhvtk7o6e"
                },
                new Image
                {
                    Id = "IMG234",
                    Tour_Id = "TOUR047",
                    Url = "upload-tour/adglhij25ry27d4ef2rf"
                },
                new Image
                {
                    Id = "IMG235",
                    Tour_Id = "TOUR047",
                    Url = "upload-tour/ybqpr7zxrrqto4btsiy3"
                },
                new Image
                {
                    Id = "IMG236",
                    Tour_Id = "TOUR048",
                    Url = "upload-tour/dgtfvzujagduzurvruef"
                },
                new Image
                {
                    Id = "IMG237",
                    Tour_Id = "TOUR048",
                    Url = "upload-tour/osynrdwdccxhgpwurgbw"
                },
                new Image
                {
                    Id = "IMG238",
                    Tour_Id = "TOUR048",
                    Url = "upload-tour/qmqxncldbaawbjca8zpg"
                },
                new Image
                {
                    Id = "IMG239",
                    Tour_Id = "TOUR048",
                    Url = "upload-tour/aki9mo9kg6vhhpxxw3ad"
                },
                new Image
                {
                    Id = "IMG240",
                    Tour_Id = "TOUR048",
                    Url = "upload-tour/msofu9fu7qhgaxtftabf"
                },
                new Image
                {
                    Id = "IMG241",
                    Tour_Id = "TOUR049",
                    Url = "upload-tour/gkb42cp9f3n5p8las4xy"
                },
                new Image
                {
                    Id = "IMG242",
                    Tour_Id = "TOUR049",
                    Url = "upload-tour/kwl36mkz8cf0noti2jog"
                },
                new Image
                {
                    Id = "IMG243",
                    Tour_Id = "TOUR049",
                    Url = "upload-tour/e6kbsp7sumrb46xuzb2c"
                },
                new Image
                {
                    Id = "IMG244",
                    Tour_Id = "TOUR049",
                    Url = "upload-tour/g5v5qruhtiijhg5fozz9"
                },
                new Image
                {
                    Id = "IMG245",
                    Tour_Id = "TOUR049",
                    Url = "upload-tour/cz8s9v3cw9weneez82bb"
                },
                new Image
                {
                    Id = "IMG246",
                    Tour_Id = "TOUR050",
                    Url = "upload-tour/wwtwonqgsrxmbtw7tsgw"
                },
                new Image
                {
                    Id = "IMG247",
                    Tour_Id = "TOUR050",
                    Url = "upload-tour/l7nwxqbhbiesvaa0t0vo"
                },
                new Image
                {
                    Id = "IMG248",
                    Tour_Id = "TOUR050",
                    Url = "upload-tour/o62tdjgxdh4in7nxip1k"
                },
                new Image
                {
                    Id = "IMG249",
                    Tour_Id = "TOUR050",
                    Url = "upload-tour/ynmcim4dfbaue4tuocbg"
                },
                new Image
                {
                    Id = "IMG250",
                    Tour_Id = "TOUR050",
                    Url = "upload-tour/c3vqfqigg4obhwqpzmhs"
                },
                new Image
                {
                    Id = "IMG251",
                    Tour_Id = "TOUR051",
                    Url = "upload-tour/hhpvw9cgtwofdzxhk8qe"
                },
                new Image
                {
                    Id = "IMG252",
                    Tour_Id = "TOUR051",
                    Url = "upload-tour/ifjrratvnyowxjnwutlj"
                },
                new Image
                {
                    Id = "IMG253",
                    Tour_Id = "TOUR051",
                    Url = "upload-tour/ggmympevgzvsltql6zwq"
                },
                new Image
                {
                    Id = "IMG254",
                    Tour_Id = "TOUR051",
                    Url = "upload-tour/f1wwuo4vyeu4hrszvvy3"
                },
                new Image
                {
                    Id = "IMG255",
                    Tour_Id = "TOUR051",
                    Url = "upload-tour/illle4pzjogw7rcrgky3"
                },
                new Image
                {
                    Id = "IMG256",
                    Tour_Id = "TOUR052",
                    Url = "upload-tour/vvmgbng8odrey1agc7ds"
                },
                new Image
                {
                    Id = "IMG257",
                    Tour_Id = "TOUR052",
                    Url = "upload-tour/fdv6cmp62tapulugl80z"
                },
                new Image
                {
                    Id = "IMG258",
                    Tour_Id = "TOUR052",
                    Url = "upload-tour/gjujmsyav4ndany2bnk3"
                },
                new Image
                {
                    Id = "IMG259",
                    Tour_Id = "TOUR052",
                    Url = "upload-tour/u92a2ijoliv0sv1vynio"
                },
                new Image
                {
                    Id = "IMG260",
                    Tour_Id = "TOUR052",
                    Url = "upload-tour/gnyibn4jyrbrnrqzu5en"
                },
                new Image
                {
                    Id = "IMG261",
                    Tour_Id = "TOUR053",
                    Url = "upload-tour/yjjtogdshlfltcscziux"
                },
                new Image
                {
                    Id = "IMG262",
                    Tour_Id = "TOUR053",
                    Url = "upload-tour/dcxpf1yskp7fhiau9sfz"
                },
                new Image
                {
                    Id = "IMG263",
                    Tour_Id = "TOUR053",
                    Url = "upload-tour/rsbm4jxm4i6qlttmtcgd"
                },
                new Image
                {
                    Id = "IMG264",
                    Tour_Id = "TOUR053",
                    Url = "upload-tour/uftt4tpucc1n7wxaepog"
                },
                new Image
                {
                    Id = "IMG265",
                    Tour_Id = "TOUR053",
                    Url = "upload-tour/lzsgyzga0aloftg59zvx"
                },
                new Image
                {
                    Id = "IMG266",
                    Tour_Id = "TOUR054",
                    Url = "upload-tour/zkl6mqymexqwgsawwtsd"
                },
                new Image
                {
                    Id = "IMG267",
                    Tour_Id = "TOUR054",
                    Url = "upload-tour/bm6bev8z1kyh72uayshe"
                },
                new Image
                {
                    Id = "IMG268",
                    Tour_Id = "TOUR054",
                    Url = "upload-tour/agutyvij3wgxqpixiae2"
                },
                new Image
                {
                    Id = "IMG269",
                    Tour_Id = "TOUR054",
                    Url = "upload-tour/pkxiywcw8pbrvftfhnei"
                },
                new Image
                {
                    Id = "IMG270",
                    Tour_Id = "TOUR054",
                    Url = "upload-tour/ftza3cpnyagojesebzmm"
                },
                new Image
                {
                    Id = "IMG271",
                    Tour_Id = "TOUR055",
                    Url = "upload-tour/luntgs50gi5dogslec8i"
                },
                new Image
                {
                    Id = "IMG272",
                    Tour_Id = "TOUR055",
                    Url = "upload-tour/wwnot7vcmltsppwynevc"
                },
                new Image
                {
                    Id = "IMG273",
                    Tour_Id = "TOUR055",
                    Url = "upload-tour/jzdehpnm83w18pwwgzsw"
                },
                new Image
                {
                    Id = "IMG274",
                    Tour_Id = "TOUR055",
                    Url = "upload-tour/hhutxbdzklxon83ezabf"
                },
                new Image
                {
                    Id = "IMG275",
                    Tour_Id = "TOUR055",
                    Url = "upload-tour/hkwfy4aghcpvhxtp0hmn"
                },
                new Image
                {
                    Id = "IMG276",
                    Tour_Id = "TOUR056",
                    Url = "upload-tour/jk7e33yeiyamipngoxho"
                },
                new Image
                {
                    Id = "IMG277",
                    Tour_Id = "TOUR056",
                    Url = "upload-tour/earwhexeo8jilubh3gyg"
                },
                new Image
                {
                    Id = "IMG278",
                    Tour_Id = "TOUR056",
                    Url = "upload-tour/ok9icglnqmedm9jo8s0z"
                },
                new Image
                {
                    Id = "IMG279",
                    Tour_Id = "TOUR056",
                    Url = "upload-tour/wbujnim9dts6pophxmtf"
                },
                new Image
                {
                    Id = "IMG280",
                    Tour_Id = "TOUR056",
                    Url = "upload-tour/tozek5uext0czasdzrs2"
                },
                new Image
                {
                    Id = "IMG281",
                    Tour_Id = "TOUR057",
                    Url = "upload-tour/rwkw2xvb9rrg2ohnvfwa"
                },
                new Image
                {
                    Id = "IMG282",
                    Tour_Id = "TOUR057",
                    Url = "upload-tour/rqqd5lovblbjvvptpnhp"
                },
                new Image
                {
                    Id = "IMG283",
                    Tour_Id = "TOUR057",
                    Url = "upload-tour/nsetcjoemqba9cgyr6ln"
                },
                new Image
                {
                    Id = "IMG284",
                    Tour_Id = "TOUR057",
                    Url = "upload-tour/odvlahco39mb8fqdtyu0"
                },
                new Image
                {
                    Id = "IMG285",
                    Tour_Id = "TOUR057",
                    Url = "upload-tour/aircm1aaaarqzxqxpkco"
                },
                new Image
                {
                    Id = "IMG286",
                    Tour_Id = "TOUR058",
                    Url = "upload-tour/b9jndvi3znpo7xbzyeeb"
                },
                new Image
                {
                    Id = "IMG287",
                    Tour_Id = "TOUR058",
                    Url = "upload-tour/dns0nhhqb26xkqaf73iv"
                },
                new Image
                {
                    Id = "IMG288",
                    Tour_Id = "TOUR058",
                    Url = "upload-tour/cuw7ztbzz7i8kmwxqdrr"
                },
                new Image
                {
                    Id = "IMG289",
                    Tour_Id = "TOUR058",
                    Url = "upload-tour/sp0hddfmpivnwxlpovge"
                },
                new Image
                {
                    Id = "IMG290",
                    Tour_Id = "TOUR058",
                    Url = "upload-tour/ewoe0maddo8dazbqfam4"
                },
                new Image
                {
                    Id = "IMG291",
                    Tour_Id = "TOUR059",
                    Url = "upload-tour/euqym5qwcegtjqt5k9fq"
                },
                new Image
                {
                    Id = "IMG292",
                    Tour_Id = "TOUR059",
                    Url = "upload-tour/afalwv6komp27hwd3scn"
                },
                new Image
                {
                    Id = "IMG293",
                    Tour_Id = "TOUR059",
                    Url = "upload-tour/ndkn893kb0p5zeg8pgzd"
                },
                new Image
                {
                    Id = "IMG294",
                    Tour_Id = "TOUR059",
                    Url = "upload-tour/t3pfubahyisngopvcchh"
                },
                new Image
                {
                    Id = "IMG295",
                    Tour_Id = "TOUR059",
                    Url = "upload-tour/ko1t3htvfwunu3aozfwu"
                },
                new Image
                {
                    Id = "IMG296",
                    Tour_Id = "TOUR060",
                    Url = "upload-tour/uofzy39owgxcuqkv7nac"
                },
                new Image
                {
                    Id = "IMG297",
                    Tour_Id = "TOUR060",
                    Url = "upload-tour/x7moyy1mo00k6ypmdvik"
                },
                new Image
                {
                    Id = "IMG298",
                    Tour_Id = "TOUR060",
                    Url = "upload-tour/ek2eyfhhujn5rqasrskp"
                },
                new Image
                {
                    Id = "IMG299",
                    Tour_Id = "TOUR060",
                    Url = "upload-tour/cmay0mc1nbjjurh0l169"
                },
                new Image
                {
                    Id = "IMG300",
                    Tour_Id = "TOUR060",
                    Url = "upload-tour/a1hvpvzh9usyrlfdqpdn"
                },
                new Image
                {
                    Id = "IMG301",
                    Tour_Id = "TOUR061",
                    Url = "upload-tour/hztkvr6flrutjp0bbeex"
                },
                new Image
                {
                    Id = "IMG302",
                    Tour_Id = "TOUR061",
                    Url = "upload-tour/dubvjuf6fevn6pc0kri2"
                },
                new Image
                {
                    Id = "IMG303",
                    Tour_Id = "TOUR061",
                    Url = "upload-tour/rqqfczm8middjsju2sms"
                },
                new Image
                {
                    Id = "IMG304",
                    Tour_Id = "TOUR061",
                    Url = "upload-tour/xyzmnpw5swddkaudxksl"
                },
                new Image
                {
                    Id = "IMG305",
                    Tour_Id = "TOUR061",
                    Url = "upload-tour/u9uthnahfo3k8z98trfb"
                },
                new Image
                {
                    Id = "IMG306",
                    Tour_Id = "TOUR062",
                    Url = "upload-tour/sioi6oi7rwjirlsnid3f"
                },
                new Image
                {
                    Id = "IMG307",
                    Tour_Id = "TOUR062",
                    Url = "upload-tour/vy9qxleovhc0jsi6crau"
                },
                new Image
                {
                    Id = "IMG308",
                    Tour_Id = "TOUR062",
                    Url = "upload-tour/elkecxxpb2ppbacacpr7"
                },
                new Image
                {
                    Id = "IMG309",
                    Tour_Id = "TOUR062",
                    Url = "upload-tour/jowdtqeck7o6eivs9rvr"
                },
                new Image
                {
                    Id = "IMG310",
                    Tour_Id = "TOUR062",
                    Url = "upload-tour/aqzr00kafq4ttdarmfgh"
                },
                new Image
                {
                    Id = "IMG311",
                    Tour_Id = "TOUR063",
                    Url = "upload-tour/mvxst7pkgfagajhomakj"
                },
                new Image
                {
                    Id = "IMG312",
                    Tour_Id = "TOUR063",
                    Url = "upload-tour/wkmn7e5ggak7lrawf9m7"
                },
                new Image
                {
                    Id = "IMG313",
                    Tour_Id = "TOUR063",
                    Url = "upload-tour/ydhqkv44jwb7cmqk2bo8"
                },
                new Image
                {
                    Id = "IMG314",
                    Tour_Id = "TOUR063",
                    Url = "upload-tour/cnlxvi9wcpnr22mzdcxv"
                },
                new Image
                {
                    Id = "IMG315",
                    Tour_Id = "TOUR063",
                    Url = "upload-tour/kcv6r7jocflpwbxvh68w"
                },
                new Image
                {
                    Id = "IMG316",
                    Tour_Id = "TOUR064",
                    Url = "upload-tour/shvudixwf1cvvhylef2t"
                },
                new Image
                {
                    Id = "IMG317",
                    Tour_Id = "TOUR064",
                    Url = "upload-tour/bp1ce554iliubgmlkd5g"
                },
                new Image
                {
                    Id = "IMG318",
                    Tour_Id = "TOUR064",
                    Url = "upload-tour/qmzk5rwe1ua5lab7rqbp"
                },
                new Image
                {
                    Id = "IMG319",
                    Tour_Id = "TOUR064",
                    Url = "upload-tour/toixjon26vfsvmkjkgpf"
                },
                new Image
                {
                    Id = "IMG320",
                    Tour_Id = "TOUR064",
                    Url = "upload-tour/zo18zl8ugjfywdma8pmj"
                },
                new Image
                {
                    Id = "IMG321",
                    Tour_Id = "TOUR065",
                    Url = "upload-tour/amzwcggdr3xlogztfjwq"
                },
                new Image
                {
                    Id = "IMG322",
                    Tour_Id = "TOUR065",
                    Url = "upload-tour/jwforxkhdijtzseu1afj"
                },
                new Image
                {
                    Id = "IMG323",
                    Tour_Id = "TOUR065",
                    Url = "upload-tour/nkrtx1pkhfpz5ubuwtin"
                },
                new Image
                {
                    Id = "IMG324",
                    Tour_Id = "TOUR065",
                    Url = "upload-tour/ot8vixzggvqzhqbgedb2"
                },
                new Image
                {
                    Id = "IMG325",
                    Tour_Id = "TOUR065",
                    Url = "upload-tour/gcriwd6vnrd3ba7ecibs"
                },
                new Image
                {
                    Id = "IMG326",
                    Tour_Id = "TOUR066",
                    Url = "upload-tour/vooz7bmabe0houkmbr3p"
                },
                new Image
                {
                    Id = "IMG327",
                    Tour_Id = "TOUR066",
                    Url = "upload-tour/b5obede7edmzppk0twob"
                },
                new Image
                {
                    Id = "IMG328",
                    Tour_Id = "TOUR066",
                    Url = "upload-tour/hvedbmg6sbjfi5uylrsu"
                },
                new Image
                {
                    Id = "IMG329",
                    Tour_Id = "TOUR066",
                    Url = "upload-tour/y27xdnmzul4zpie74gke"
                },
                new Image
                {
                    Id = "IMG330",
                    Tour_Id = "TOUR066",
                    Url = "upload-tour/ky701cwvtgmwahsurqm2"
                },
                new Image
                {
                    Id = "IMG331",
                    Tour_Id = "TOUR067",
                    Url = "upload-tour/yoxitqksxvag0vdnd9of"
                },
                new Image
                {
                    Id = "IMG332",
                    Tour_Id = "TOUR067",
                    Url = "upload-tour/rxluuo89hupwsos7re3i"
                },
                new Image
                {
                    Id = "IMG333",
                    Tour_Id = "TOUR067",
                    Url = "upload-tour/n7cknwxujdbbrbf5pa0g"
                },
                new Image
                {
                    Id = "IMG334",
                    Tour_Id = "TOUR067",
                    Url = "upload-tour/fe9pg2mviubmkmluvtuv"
                },
                new Image
                {
                    Id = "IMG335",
                    Tour_Id = "TOUR067",
                    Url = "upload-tour/etsajt5htagev6p4zlsc"
                },
                new Image
                {
                    Id = "IMG336",
                    Tour_Id = "TOUR068",
                    Url = "upload-tour/aimoc0am1xo5gtdueppa"
                },
                new Image
                {
                    Id = "IMG337",
                    Tour_Id = "TOUR068",
                    Url = "upload-tour/yong35dsvr7ea1qamduv"
                },
                new Image
                {
                    Id = "IMG338",
                    Tour_Id = "TOUR068",
                    Url = "upload-tour/aqsb3gyh2ge6iu7a5l7r"
                },
                new Image
                {
                    Id = "IMG339",
                    Tour_Id = "TOUR068",
                    Url = "upload-tour/bqp4lxg0stllnl3itpul"
                },
                new Image
                {
                    Id = "IMG340",
                    Tour_Id = "TOUR068",
                    Url = "upload-tour/kunprdu7pwjpt7fdgjzi"
                },
                new Image
                {
                    Id = "IMG341",
                    Tour_Id = "TOUR069",
                    Url = "upload-tour/ffxttyqeuwv8nd0pzsa9"
                },
                new Image
                {
                    Id = "IMG342",
                    Tour_Id = "TOUR069",
                    Url = "upload-tour/rvsx6o7jsgrfo9xblvr8"
                },
                new Image
                {
                    Id = "IMG343",
                    Tour_Id = "TOUR069",
                    Url = "upload-tour/jxcq8usn01xrym8cyo02"
                },
                new Image
                {
                    Id = "IMG344",
                    Tour_Id = "TOUR069",
                    Url = "upload-tour/dybgp1cmyu6cv5ayslht"
                },
                new Image
                {
                    Id = "IMG345",
                    Tour_Id = "TOUR069",
                    Url = "upload-tour/icbteksxxmrc2yuuljfp"
                },
                new Image
                {
                    Id = "IMG346",
                    Tour_Id = "TOUR070",
                    Url = "upload-tour/sicx7zsjm0cnldoimcgf"
                },
                new Image
                {
                    Id = "IMG347",
                    Tour_Id = "TOUR070",
                    Url = "upload-tour/wxav1ukrrrj7bvtzjiux"
                },
                new Image
                {
                    Id = "IMG348",
                    Tour_Id = "TOUR070",
                    Url = "upload-tour/ptcww8vj2wqkyudaohij"
                },
                new Image
                {
                    Id = "IMG349",
                    Tour_Id = "TOUR070",
                    Url = "upload-tour/czscqf2itrnjgogmgsbl"
                },
                new Image
                {
                    Id = "IMG350",
                    Tour_Id = "TOUR070",
                    Url = "upload-tour/ytv5krbtheqbeo7nymnv"
                },
                new Image
                {
                    Id = "IMG351",
                    Tour_Id = "TOUR071",
                    Url = "upload-tour/v9siu82tipk5m7f6zll6"
                },
                new Image
                {
                    Id = "IMG352",
                    Tour_Id = "TOUR071",
                    Url = "upload-tour/wptumjkiebide9t5guws"
                },
                new Image
                {
                    Id = "IMG353",
                    Tour_Id = "TOUR071",
                    Url = "upload-tour/ha2e9z4ll4xltxswnrvo"
                },
                new Image
                {
                    Id = "IMG354",
                    Tour_Id = "TOUR071",
                    Url = "upload-tour/eqgguce9nuunaxvxu6v6"
                },
                new Image
                {
                    Id = "IMG355",
                    Tour_Id = "TOUR071",
                    Url = "upload-tour/mkaaxtumxim2yvatuhrg"
                },
                new Image
                {
                    Id = "IMG356",
                    Tour_Id = "TOUR072",
                    Url = "upload-tour/sv4aremnz4chfyzfninq"
                },
                new Image
                {
                    Id = "IMG357",
                    Tour_Id = "TOUR072",
                    Url = "upload-tour/s6fwdsfeh4nqrw9lzay5"
                },
                new Image
                {
                    Id = "IMG358",
                    Tour_Id = "TOUR072",
                    Url = "upload-tour/tsfs2miwugbduqvxmjiu"
                },
                new Image
                {
                    Id = "IMG359",
                    Tour_Id = "TOUR072",
                    Url = "upload-tour/dip23kpjpfli8cxrhuqx"
                },
                new Image
                {
                    Id = "IMG360",
                    Tour_Id = "TOUR072",
                    Url = "upload-tour/h5ahu5pctaxnouzbxor4"
                },
                new Image
                {
                    Id = "IMG361",
                    Tour_Id = "TOUR073",
                    Url = "upload-tour/gml6fv1exdkducutfudf"
                },
                new Image
                {
                    Id = "IMG362",
                    Tour_Id = "TOUR073",
                    Url = "upload-tour/nwvc2voiyifzzjkrsnzz"
                },
                new Image
                {
                    Id = "IMG363",
                    Tour_Id = "TOUR073",
                    Url = "upload-tour/osngmivcd0txtakxeyok"
                },
                new Image
                {
                    Id = "IMG364",
                    Tour_Id = "TOUR073",
                    Url = "upload-tour/q4swslqcbis1mb9pltiw"
                },
                new Image
                {
                    Id = "IMG365",
                    Tour_Id = "TOUR073",
                    Url = "upload-tour/wbmilehorp7rr2y6g2ud"
                },
                new Image
                {
                    Id = "IMG366",
                    Tour_Id = "TOUR074",
                    Url = "upload-tour/zi5svlxh68n4qcazkzn7"
                },
                new Image
                {
                    Id = "IMG367",
                    Tour_Id = "TOUR074",
                    Url = "upload-tour/vvdr8l0ktvw36a6xmnox"
                },
                new Image
                {
                    Id = "IMG368",
                    Tour_Id = "TOUR074",
                    Url = "upload-tour/pzxor5medhmr5ompl63m"
                },
                new Image
                {
                    Id = "IMG369",
                    Tour_Id = "TOUR074",
                    Url = "upload-tour/w9cvzmoqfpemufkxg5au"
                },
                new Image
                {
                    Id = "IMG370",
                    Tour_Id = "TOUR074",
                    Url = "upload-tour/kg6u2okghmcctwtubqv2"
                },
                new Image
                {
                    Id = "IMG371",
                    Tour_Id = "TOUR075",
                    Url = "upload-tour/qr65c6pmztsdy0y0soby"
                },
                new Image
                {
                    Id = "IMG372",
                    Tour_Id = "TOUR075",
                    Url = "upload-tour/ejpmewdkro1rtniqpmab"
                },
                new Image
                {
                    Id = "IMG373",
                    Tour_Id = "TOUR075",
                    Url = "upload-tour/zfwyzso0oxgdx5adyghv"
                },
                new Image
                {
                    Id = "IMG374",
                    Tour_Id = "TOUR075",
                    Url = "upload-tour/gael9gxbdlervbryydxr"
                },
                new Image
                {
                    Id = "IMG375",
                    Tour_Id = "TOUR075",
                    Url = "upload-tour/ovgmm85id95m0oox3zeg"
                },
                new Image
                {
                    Id = "IMG376",
                    Tour_Id = "TOUR076",
                    Url = "upload-tour/h8xhdwz6qlgcv9youidz"
                },
                new Image
                {
                    Id = "IMG377",
                    Tour_Id = "TOUR076",
                    Url = "upload-tour/bqvz6yk6f3qqan7mj2nt"
                },
                new Image
                {
                    Id = "IMG378",
                    Tour_Id = "TOUR076",
                    Url = "upload-tour/nb7ydsp3szgla2p1tlfw"
                },
                new Image
                {
                    Id = "IMG379",
                    Tour_Id = "TOUR076",
                    Url = "upload-tour/bxkdz3blgnrz8yorfnx3"
                },
                new Image
                {
                    Id = "IMG380",
                    Tour_Id = "TOUR076",
                    Url = "upload-tour/wkt9o0cajflslqucipt8"
                },
                new Image
                {
                    Id = "IMG381",
                    Tour_Id = "TOUR077",
                    Url = "upload-tour/t03ctm7lozo2riqidrqq"
                },
                new Image
                {
                    Id = "IMG382",
                    Tour_Id = "TOUR077",
                    Url = "upload-tour/jpssoiwpoc7d7dhmkyw3"
                },
                new Image
                {
                    Id = "IMG383",
                    Tour_Id = "TOUR077",
                    Url = "upload-tour/hk2jny2emltgrqtfahts"
                },
                new Image
                {
                    Id = "IMG384",
                    Tour_Id = "TOUR077",
                    Url = "upload-tour/uipukc7ydelvuxy1s5r2"
                },
                new Image
                {
                    Id = "IMG385",
                    Tour_Id = "TOUR077",
                    Url = "upload-tour/kubi9hnb2hntf9qxnpyf"
                },
                new Image
                {
                    Id = "IMG386",
                    Tour_Id = "TOUR078",
                    Url = "upload-tour/gtsrdrrkomewkyycvoc9"
                },
                new Image
                {
                    Id = "IMG387",
                    Tour_Id = "TOUR078",
                    Url = "upload-tour/nwurpqsniyyycell2lqu"
                },
                new Image
                {
                    Id = "IMG388",
                    Tour_Id = "TOUR078",
                    Url = "upload-tour/kt9xqcydpdeepfpkhncj"
                },
                new Image
                {
                    Id = "IMG389",
                    Tour_Id = "TOUR078",
                    Url = "upload-tour/earbsauyiqk55o88jx1b"
                },
                new Image
                {
                    Id = "IMG390",
                    Tour_Id = "TOUR078",
                    Url = "upload-tour/axdeho8xupik2y9kzg0b"
                },
                new Image
                {
                    Id = "IMG391",
                    Tour_Id = "TOUR079",
                    Url = "upload-tour/sc8m2lvuhphrbywmyhoh"
                },
                new Image
                {
                    Id = "IMG392",
                    Tour_Id = "TOUR079",
                    Url = "upload-tour/bda7yxhudl61776b2qcr"
                },
                new Image
                {
                    Id = "IMG393",
                    Tour_Id = "TOUR079",
                    Url = "upload-tour/tmmvx8ku1hhzflyyhl42"
                },
                new Image
                {
                    Id = "IMG394",
                    Tour_Id = "TOUR079",
                    Url = "upload-tour/pmvaie3euxllpmsbfabw"
                },
                new Image
                {
                    Id = "IMG395",
                    Tour_Id = "TOUR079",
                    Url = "upload-tour/oxtfaoarolbo4wy9fsjb"
                },
                new Image
                {
                    Id = "IMG396",
                    Tour_Id = "TOUR080",
                    Url = "upload-tour/huum0y3rewweljpp9siu"
                },
                new Image
                {
                    Id = "IMG397",
                    Tour_Id = "TOUR080",
                    Url = "upload-tour/lri2ybtf2qxlgaiuzife"
                },
                new Image
                {
                    Id = "IMG398",
                    Tour_Id = "TOUR080",
                    Url = "upload-tour/so22dbt9gchbz7pwi1pd"
                },
                new Image
                {
                    Id = "IMG399",
                    Tour_Id = "TOUR080",
                    Url = "upload-tour/feqioqboorcydqqakfre"
                },
                new Image
                {
                    Id = "IMG400",
                    Tour_Id = "TOUR080",
                    Url = "upload-tour/fmckbmapbhwy0ezkacnb"
                },
                new Image
                {
                    Id = "IMG401",
                    Tour_Id = "TOUR081",
                    Url = "upload-tour/orrphjd1l8sowjosy8sc"
                },
                new Image
                {
                    Id = "IMG402",
                    Tour_Id = "TOUR081",
                    Url = "upload-tour/otggg8iwy9dngxpnxly9"
                },
                new Image
                {
                    Id = "IMG403",
                    Tour_Id = "TOUR081",
                    Url = "upload-tour/dlrvzsugn9vwid4kccku"
                },
                new Image
                {
                    Id = "IMG404",
                    Tour_Id = "TOUR081",
                    Url = "upload-tour/b04y1wn2gqwwosyms254"
                },
                new Image
                {
                    Id = "IMG405",
                    Tour_Id = "TOUR081",
                    Url = "upload-tour/exbqegfod9r2erhm0tcp"
                },
                new Image
                {
                    Id = "IMG406",
                    Tour_Id = "TOUR082",
                    Url = "upload-tour/ir34oyfraehxici0l0cl"
                },
                new Image
                {
                    Id = "IMG407",
                    Tour_Id = "TOUR082",
                    Url = "upload-tour/wzdveegfcu1jpru1o9hn"
                },
                new Image
                {
                    Id = "IMG408",
                    Tour_Id = "TOUR082",
                    Url = "upload-tour/pwzttxvhvitsvcny5i5d"
                },
                new Image
                {
                    Id = "IMG409",
                    Tour_Id = "TOUR082",
                    Url = "upload-tour/lqajtpfdkn4kqpunz0te"
                },
                new Image
                {
                    Id = "IMG410",
                    Tour_Id = "TOUR082",
                    Url = "upload-tour/cdaxgfqwuzjjbeo7snhy"
                },
                new Image
                {
                    Id = "IMG411",
                    Tour_Id = "TOUR083",
                    Url = "upload-tour/dcaima4yywap60hmhjrh"
                },
                new Image
                {
                    Id = "IMG412",
                    Tour_Id = "TOUR083",
                    Url = "upload-tour/grsiwzfmaqvpqyi9m4nu"
                },
                new Image
                {
                    Id = "IMG413",
                    Tour_Id = "TOUR083",
                    Url = "upload-tour/roihu78edrv3z7zrriys"
                },
                new Image
                {
                    Id = "IMG414",
                    Tour_Id = "TOUR083",
                    Url = "upload-tour/tiec77sjmfe4m4wihxb0"
                },
                new Image
                {
                    Id = "IMG415",
                    Tour_Id = "TOUR083",
                    Url = "upload-tour/wrcw3t58kiht7y9fsffe"
                },
                new Image
                {
                    Id = "IMG416",
                    Tour_Id = "TOUR084",
                    Url = "upload-tour/m6jokqrjifnbjk4dthp1"
                },
                new Image
                {
                    Id = "IMG417",
                    Tour_Id = "TOUR084",
                    Url = "upload-tour/wyksyb2u9nhw8h1modfl"
                },
                new Image
                {
                    Id = "IMG418",
                    Tour_Id = "TOUR084",
                    Url = "upload-tour/ref8hntnqeu1c4usap9a"
                },
                new Image
                {
                    Id = "IMG419",
                    Tour_Id = "TOUR084",
                    Url = "upload-tour/z2n2j8tmj8pasllph97d"
                },
                new Image
                {
                    Id = "IMG420",
                    Tour_Id = "TOUR084",
                    Url = "upload-tour/feaegnjqicdgt3pggis6"
                },
                new Image
                {
                    Id = "IMG421",
                    Tour_Id = "TOUR085",
                    Url = "upload-tour/vphznvllhkb4mwpdwqir"
                },
                new Image
                {
                    Id = "IMG422",
                    Tour_Id = "TOUR085",
                    Url = "upload-tour/dx4pmkchdo2jouq5g7uj"
                },
                new Image
                {
                    Id = "IMG423",
                    Tour_Id = "TOUR085",
                    Url = "upload-tour/yzuxzvhsmg77bvx5rp8c"
                },
                new Image
                {
                    Id = "IMG424",
                    Tour_Id = "TOUR085",
                    Url = "upload-tour/mnnm5ynhl014kzhb5bd7"
                },
                new Image
                {
                    Id = "IMG425",
                    Tour_Id = "TOUR085",
                    Url = "upload-tour/jm84efawwziezhy28yvd"
                },
                new Image
                {
                    Id = "IMG426",
                    Tour_Id = "TOUR086",
                    Url = "upload-tour/iu16tpipx4ajsowj2ab3"
                },
                new Image
                {
                    Id = "IMG427",
                    Tour_Id = "TOUR086",
                    Url = "upload-tour/hqoexe5rdeprtquflmqe"
                },
                new Image
                {
                    Id = "IMG428",
                    Tour_Id = "TOUR086",
                    Url = "upload-tour/h3l1apqmtjhwmrh7sv9g"
                },
                new Image
                {
                    Id = "IMG429",
                    Tour_Id = "TOUR086",
                    Url = "upload-tour/tw9gl1ig8s9ui86my1fq"
                },
                new Image
                {
                    Id = "IMG430",
                    Tour_Id = "TOUR086",
                    Url = "upload-tour/oc01xklsv91tvk4gkohu"
                },
                new Image
                {
                    Id = "IMG431",
                    Tour_Id = "TOUR087",
                    Url = "upload-tour/jg2swtr4zugdk1ixzqm6"
                },
                new Image
                {
                    Id = "IMG432",
                    Tour_Id = "TOUR087",
                    Url = "upload-tour/aaqtkzpltjxqifxpdovf"
                },
                new Image
                {
                    Id = "IMG433",
                    Tour_Id = "TOUR087",
                    Url = "upload-tour/jthvljyswwq9uhlhsysa"
                },
                new Image
                {
                    Id = "IMG434",
                    Tour_Id = "TOUR087",
                    Url = "upload-tour/zt73cg7vsohubesgewxj"
                },
                new Image
                {
                    Id = "IMG435",
                    Tour_Id = "TOUR087",
                    Url = "upload-tour/isf349q5qgwyehrvxl4s"
                },
                new Image
                {
                    Id = "IMG436",
                    Tour_Id = "TOUR088",
                    Url = "upload-tour/uqn7gggkqlp7vdr8xrk9"
                },
                new Image
                {
                    Id = "IMG437",
                    Tour_Id = "TOUR088",
                    Url = "upload-tour/rqtgtgniogxwxgnancfu"
                },
                new Image
                {
                    Id = "IMG438",
                    Tour_Id = "TOUR088",
                    Url = "upload-tour/htqnn4mqmxlpbulsq83t"
                },
                new Image
                {
                    Id = "IMG439",
                    Tour_Id = "TOUR088",
                    Url = "upload-tour/ofivq4g6ryfqfnevcajx"
                },
                new Image
                {
                    Id = "IMG440",
                    Tour_Id = "TOUR088",
                    Url = "upload-tour/ohg4vvmf4uh8tttykrtx"
                },
                new Image
                {
                    Id = "IMG441",
                    Tour_Id = "TOUR089",
                    Url = "upload-tour/mimkngpuqriqs0vdx1m3"
                },
                new Image
                {
                    Id = "IMG442",
                    Tour_Id = "TOUR089",
                    Url = "upload-tour/xfxnplwc54dhjkoenvf0"
                },
                new Image
                {
                    Id = "IMG443",
                    Tour_Id = "TOUR089",
                    Url = "upload-tour/xsorxh1vltf9svtbbx6b"
                },
                new Image
                {
                    Id = "IMG444",
                    Tour_Id = "TOUR089",
                    Url = "upload-tour/oeqjprewfv1lqne5lmgk"
                },
                new Image
                {
                    Id = "IMG445",
                    Tour_Id = "TOUR089",
                    Url = "upload-tour/xpg0ad5cfjaxyahdgoz5"
                },
                new Image
                {
                    Id = "IMG446",
                    Tour_Id = "TOUR090",
                    Url = "upload-tour/qbghi6xehznattr3skcy"
                },
                new Image
                {
                    Id = "IMG447",
                    Tour_Id = "TOUR090",
                    Url = "upload-tour/cb5kkvlcpgcqnzpxxjet"
                },
                new Image
                {
                    Id = "IMG448",
                    Tour_Id = "TOUR090",
                    Url = "upload-tour/ij1uth1fks6ovuilomqz"
                },
                new Image
                {
                    Id = "IMG449",
                    Tour_Id = "TOUR090",
                    Url = "upload-tour/x4uqke9ucqmspghqobco"
                },
                new Image
                {
                    Id = "IMG450",
                    Tour_Id = "TOUR090",
                    Url = "upload-tour/hribclm74wkxbao64ovi"
                },
                new Image
                {
                    Id = "IMG451",
                    Tour_Id = "TOUR091",
                    Url = "upload-tour/n1scnikfr0kmdf3a0knb"
                },
                new Image
                {
                    Id = "IMG452",
                    Tour_Id = "TOUR091",
                    Url = "upload-tour/gzq6ebbu8krxppllzcht"
                },
                new Image
                {
                    Id = "IMG453",
                    Tour_Id = "TOUR091",
                    Url = "upload-tour/pxcf4tx7yqu1uf9wnzyb"
                },
                new Image
                {
                    Id = "IMG454",
                    Tour_Id = "TOUR091",
                    Url = "upload-tour/tjyuphsgkflkn4b9snw8"
                },
                new Image
                {
                    Id = "IMG455",
                    Tour_Id = "TOUR091",
                    Url = "upload-tour/zqjjntaizvnhn91odkfo"
                },
                new Image
                {
                    Id = "IMG456",
                    Tour_Id = "TOUR092",
                    Url = "upload-tour/q0eu8dsmlazzifrjxuej"
                },
                new Image
                {
                    Id = "IMG457",
                    Tour_Id = "TOUR092",
                    Url = "upload-tour/ty9lscyf5yoxhqih9w9t"
                },
                new Image
                {
                    Id = "IMG458",
                    Tour_Id = "TOUR092",
                    Url = "upload-tour/k5m5nmcipycgruetd7k7"
                },
                new Image
                {
                    Id = "IMG459",
                    Tour_Id = "TOUR092",
                    Url = "upload-tour/wocbue49w3tja0lrd3xf"
                },
                new Image
                {
                    Id = "IMG460",
                    Tour_Id = "TOUR092",
                    Url = "upload-tour/p8foodtj7ifmqzpwxnsj"
                },
                new Image
                {
                    Id = "IMG461",
                    Tour_Id = "TOUR093",
                    Url = "upload-tour/xkfyljkmr3jrqxhmpzrt"
                },
                new Image
                {
                    Id = "IMG462",
                    Tour_Id = "TOUR093",
                    Url = "upload-tour/zuazrgzmhiji0qpnx6cv"
                },
                new Image
                {
                    Id = "IMG463",
                    Tour_Id = "TOUR093",
                    Url = "upload-tour/go7siyc4rso5lh3zxorb"
                },
                new Image
                {
                    Id = "IMG464",
                    Tour_Id = "TOUR093",
                    Url = "upload-tour/kuy4j49vkhhdhfbii24s"
                },
                new Image
                {
                    Id = "IMG465",
                    Tour_Id = "TOUR093",
                    Url = "upload-tour/mrpg91y0ob0sdejdlg0u"
                },
                new Image
                {
                    Id = "IMG466",
                    Tour_Id = "TOUR094",
                    Url = "upload-tour/vwj1ymocs8gu7moazx5b"
                },
                new Image
                {
                    Id = "IMG467",
                    Tour_Id = "TOUR094",
                    Url = "upload-tour/g332m3btgwuwyzduwbpr"
                },
                new Image
                {
                    Id = "IMG468",
                    Tour_Id = "TOUR094",
                    Url = "upload-tour/qhrf7tkmcyg7jdq8xvan"
                },
                new Image
                {
                    Id = "IMG469",
                    Tour_Id = "TOUR094",
                    Url = "upload-tour/ohhg14aedj61ebp9vehz"
                },
                new Image
                {
                    Id = "IMG470",
                    Tour_Id = "TOUR094",
                    Url = "upload-tour/zz5je3x1bpl4qhvpjiwz"
                },
                new Image
                {
                    Id = "IMG471",
                    Tour_Id = "TOUR095",
                    Url = "upload-tour/xyby8j4idowuxszbejuc"
                },
                new Image
                {
                    Id = "IMG472",
                    Tour_Id = "TOUR095",
                    Url = "upload-tour/ssz02jbotbqocopza20v"
                },
                new Image
                {
                    Id = "IMG473",
                    Tour_Id = "TOUR095",
                    Url = "upload-tour/m3xadpqccxrssohrc8fb"
                },
                new Image
                {
                    Id = "IMG474",
                    Tour_Id = "TOUR095",
                    Url = "upload-tour/xu64zug9pzobwkmuaidq"
                },
                new Image
                {
                    Id = "IMG475",
                    Tour_Id = "TOUR095",
                    Url = "upload-tour/wvpt1yk7ufhu0hhafno9"
                },
                new Image
                {
                    Id = "IMG476",
                    Tour_Id = "TOUR096",
                    Url = "upload-tour/tpj2vnfnbdkp4tyrss6u"
                },
                new Image
                {
                    Id = "IMG477",
                    Tour_Id = "TOUR096",
                    Url = "upload-tour/qpldqzlgtqubwnnk8ndi"
                },
                new Image
                {
                    Id = "IMG478",
                    Tour_Id = "TOUR096",
                    Url = "upload-tour/dhawwwmmirpm4ovrtjuq"
                },
                new Image
                {
                    Id = "IMG479",
                    Tour_Id = "TOUR096",
                    Url = "upload-tour/veefagmbmzyxsoheawqi"
                },
                new Image
                {
                    Id = "IMG480",
                    Tour_Id = "TOUR096",
                    Url = "upload-tour/qnkogn26nydne7iwftvd"
                },
                new Image
                {
                    Id = "IMG481",
                    Tour_Id = "TOUR097",
                    Url = "upload-tour/xhq7pszfuzcwinpmaabi"
                },
                new Image
                {
                    Id = "IMG482",
                    Tour_Id = "TOUR097",
                    Url = "upload-tour/gfheyorcpyjuchjk15bn"
                },
                new Image
                {
                    Id = "IMG483",
                    Tour_Id = "TOUR097",
                    Url = "upload-tour/zk8pwjt7kkowzs0gywlv"
                },
                new Image
                {
                    Id = "IMG484",
                    Tour_Id = "TOUR097",
                    Url = "upload-tour/oamsz1aplvsa5v8snwzm"
                },
                new Image
                {
                    Id = "IMG485",
                    Tour_Id = "TOUR097",
                    Url = "upload-tour/pcnmoqvadh7fgd610gdo"
                },
                new Image
                {
                    Id = "IMG486",
                    Tour_Id = "TOUR098",
                    Url = "upload-tour/s41urnz72d5wblzyih0u"
                },
                new Image
                {
                    Id = "IMG487",
                    Tour_Id = "TOUR098",
                    Url = "upload-tour/b8c0fdesvnqfgjpcehtm"
                },
                new Image
                {
                    Id = "IMG488",
                    Tour_Id = "TOUR098",
                    Url = "upload-tour/qrfvq2uepf8zcnclvqku"
                },
                new Image
                {
                    Id = "IMG489",
                    Tour_Id = "TOUR098",
                    Url = "upload-tour/kmc8ppxiptmyh2vxldji"
                },
                new Image
                {
                    Id = "IMG490",
                    Tour_Id = "TOUR098",
                    Url = "upload-tour/nvp8iqccquchjh27pkym"
                },
                new Image
                {
                    Id = "IMG491",
                    Tour_Id = "TOUR099",
                    Url = "upload-tour/g0kznk5qrejrws8pfe7t"
                },
                new Image
                {
                    Id = "IMG492",
                    Tour_Id = "TOUR099",
                    Url = "upload-tour/ryrlg52dt6p7xquawmy1"
                },
                new Image
                {
                    Id = "IMG493",
                    Tour_Id = "TOUR099",
                    Url = "upload-tour/kcp3ncjzucwl7sfkjupc"
                },
                new Image
                {
                    Id = "IMG494",
                    Tour_Id = "TOUR099",
                    Url = "upload-tour/irmnnttv9zerj2tct6qs"
                },
                new Image
                {
                    Id = "IMG495",
                    Tour_Id = "TOUR099",
                    Url = "upload-tour/wqsrmjlt6s6nvvuk3xru"
                },
                new Image
                {
                    Id = "IMG496",
                    Tour_Id = "TOUR100",
                    Url = "upload-tour/aiqnysfnxiohgxgo8iwg"
                },
                new Image
                {
                    Id = "IMG497",
                    Tour_Id = "TOUR100",
                    Url = "upload-tour/z09gtcrbgimjke1btuth"
                },
                new Image
                {
                    Id = "IMG498",
                    Tour_Id = "TOUR100",
                    Url = "upload-tour/a9igdoavhzctyetd3lx2"
                },
                new Image
                {
                    Id = "IMG499",
                    Tour_Id = "TOUR100",
                    Url = "upload-tour/pocjxi6rocz6zmfplabr"
                },
                new Image
                {
                    Id = "IMG500",
                    Tour_Id = "TOUR100",
                    Url = "upload-tour/hk988q0wir5w7m0tulab"
                }
            );


            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    Id = "REV001",
                    Tour_Id = "TOUR001",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ, hướng dẫn viên nhiệt tình.",
                    Create_at = new DateTime(2024, 7, 22, 9, 45, 11)
                },
                new Review
                {
                    Id = "REV002",
                    Tour_Id = "TOUR001",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Rất hài lòng với chuyến đi này. Khách sạn sạch sẽ, thoải mái, lịch trình hợp lý.",
                    Create_at = new DateTime(2025, 3, 10, 16, 20, 5)
                },
                new Review
                {
                    Id = "REV003",
                    Tour_Id = "TOUR001",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, không có gì nổi bật nhưng cũng không tệ. Mọi thứ khá ổn.",
                    Create_at = new DateTime(2024, 11, 1, 11, 0, 55)
                },
                new Review
                {
                    Id = "REV004",
                    Tour_Id = "TOUR001",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Hướng dẫn viên có kiến thức sâu rộng.",
                    Create_at = new DateTime(2025, 1, 5, 8, 10, 30)
                },
                new Review
                {
                    Id = "REV005",
                    Tour_Id = "TOUR001",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Tuyệt vời cho gia đình có trẻ nhỏ. An toàn và vui vẻ suốt chuyến đi.",
                    Create_at = new DateTime(2024, 4, 18, 14, 35, 49)
                },
                new Review
                {
                    Id = "REV006",
                    Tour_Id = "TOUR002",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Cảnh đẹp tuyệt vời! Hướng dẫn viên nhiệt tình và kiến thức.",
                    Create_at = new DateTime(2024, 9, 2, 9, 0, 0)
                },
                new Review
                {
                    Id = "REV007",
                    Tour_Id = "TOUR002",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Rất hài lòng với chuyến đi này. Mọi thứ đều hoàn hảo.",
                    Create_at = new DateTime(2025, 2, 15, 10, 30, 22)
                },
                new Review
                {
                    Id = "REV008",
                    Tour_Id = "TOUR002",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém, không đáng tiền. Khách sạn bẩn, không như quảng cáo.",
                    Create_at = new DateTime(2024, 5, 20, 17, 0, 10)
                },
                new Review
                {
                    Id = "REV009",
                    Tour_Id = "TOUR002",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Tour rất tốt! Sẽ giới thiệu cho bạn bè.",
                    Create_at = new DateTime(2024, 7, 10, 12, 50, 33)
                },
                new Review
                {
                    Id = "REV010",
                    Tour_Id = "TOUR002",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Một kỳ nghỉ thư giãn và đáng nhớ. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2025, 4, 1, 11, 15, 0)
                },
                new Review
                {
                    Id = "REV011",
                    Tour_Id = "TOUR003",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm đáng tiền. Lịch trình hợp lý.",
                    Create_at = new DateTime(2024, 8, 15, 14, 0, 0)
                },
                new Review
                {
                    Id = "REV012",
                    Tour_Id = "TOUR003",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Đồ ăn ngon. Mọi thứ đều hoàn hảo.",
                    Create_at = new DateTime(2025, 1, 1, 9, 0, 45)
                },
                new Review
                {
                    Id = "REV013",
                    Tour_Id = "TOUR003",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Hướng dẫn viên thân thiện nhưng kiến thức hạn chế. Tour tạm được.",
                    Create_at = new DateTime(2024, 6, 20, 10, 0, 11)
                },
                new Review
                {
                    Id = "REV014",
                    Tour_Id = "TOUR003",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Rất chuyên nghiệp từ khâu đặt tour đến kết thúc. Sẽ quay lại.",
                    Create_at = new DateTime(2025, 4, 20, 15, 0, 0)
                },
                new Review
                {
                    Id = "REV015",
                    Tour_Id = "TOUR003",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Cảnh đẹp, nhưng dịch vụ bình thường. Tour này phù hợp cho người không kén chọn.",
                    Create_at = new DateTime(2024, 10, 5, 13, 20, 18)
                },
                new Review
                {
                    Id = "REV016",
                    Tour_Id = "TOUR004",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Một kỳ nghỉ thư giãn và đáng nhớ. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 9, 18, 8, 30, 0)
                },
                new Review
                {
                    Id = "REV017",
                    Tour_Id = "TOUR004",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Hướng dẫn viên nhiệt tình.",
                    Create_at = new DateTime(2025, 2, 1, 11, 40, 11)
                },
                new Review
                {
                    Id = "REV018",
                    Tour_Id = "TOUR004",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Tour tệ quá! Trải nghiệm đáng quên.",
                    Create_at = new DateTime(2024, 6, 1, 16, 0, 0)
                },
                new Review
                {
                    Id = "REV019",
                    Tour_Id = "TOUR004",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Bãi biển sạch và đẹp. Hoạt động rất cuốn hút.",
                    Create_at = new DateTime(2024, 8, 5, 9, 10, 25)
                },
                new Review
                {
                    Id = "REV020",
                    Tour_Id = "TOUR004",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Rất hài lòng với chuyến đi này. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 4, 5, 14, 50, 0)
                },
                new Review
                {
                    Id = "REV021",
                    Tour_Id = "TOUR005",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Sẽ giới thiệu cho bạn bè.",
                    Create_at = new DateTime(2024, 7, 1, 9, 0, 0)
                },
                new Review
                {
                    Id = "REV022",
                    Tour_Id = "TOUR005",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều hoàn hảo. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2025, 3, 15, 10, 20, 0)
                },
                new Review
                {
                    Id = "REV023",
                    Tour_Id = "TOUR005",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được. Không có gì đặc biệt.",
                    Create_at = new DateTime(2024, 11, 10, 15, 30, 50)
                },
                new Review
                {
                    Id = "REV024",
                    Tour_Id = "TOUR005",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 1, 10, 8, 0, 0)
                },
                new Review
                {
                    Id = "REV025",
                    Tour_Id = "TOUR005",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Tuyệt vời cho gia đình có trẻ nhỏ. An toàn và vui vẻ suốt chuyến đi.",
                    Create_at = new DateTime(2024, 4, 22, 14, 0, 0)
                },
                new Review
                {
                    Id = "REV026",
                    Tour_Id = "TOUR006",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Tour rất tốt! Cảnh đẹp tuyệt vời!",
                    Create_at = new DateTime(2024, 9, 5, 9, 15, 0)
                },
                new Review
                {
                    Id = "REV027",
                    Tour_Id = "TOUR006",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Khách sạn và dịch vụ đều xuất sắc. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 2, 20, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV028",
                    Tour_Id = "TOUR006",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Hướng dẫn viên thiếu chuyên nghiệp. Lịch trình tour quá gấp gáp, mệt mỏi.",
                    Create_at = new DateTime(2024, 5, 25, 16, 30, 0)
                },
                new Review
                {
                    Id = "REV029",
                    Tour_Id = "TOUR006",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình tour rất hợp lý và thú vị. Đồ ăn ngon.",
                    Create_at = new DateTime(2024, 7, 15, 13, 0, 0)
                },
                new Review
                {
                    Id = "REV030",
                    Tour_Id = "TOUR006",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu tour này cho bạn bè và người thân. Mọi thứ đều vượt ngoài mong đợi.",
                    Create_at = new DateTime(2025, 4, 8, 10, 40, 0)
                },
                new Review
                {
                    Id = "REV031",
                    Tour_Id = "TOUR007",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Hướng dẫn viên nhiệt tình. Trải nghiệm đáng tiền.",
                    Create_at = new DateTime(2024, 8, 20, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV032",
                    Tour_Id = "TOUR007",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Rất chuyên nghiệp từ khâu đặt tour đến kết thúc. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2025, 1, 6, 9, 5, 0)
                },
                new Review
                {
                    Id = "REV033",
                    Tour_Id = "TOUR007",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, nhưng cần cải thiện lịch trình. Khách sạn ở mức chấp nhận được.",
                    Create_at = new DateTime(2024, 6, 25, 10, 10, 0)
                },
                new Review
                {
                    Id = "REV034",
                    Tour_Id = "TOUR007",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 4, 25, 15, 10, 0)
                },
                new Review
                {
                    Id = "REV035",
                    Tour_Id = "TOUR007",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2024, 10, 10, 13, 30, 22)
                },
                new Review
                {
                    Id = "REV036",
                    Tour_Id = "TOUR008",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Cảnh đẹp như mơ, hướng dẫn viên nhiệt tình. Trải nghiệm tuyệt vời, rất đáng tiền!",
                    Create_at = new DateTime(2024, 9, 20, 8, 40, 0)
                },
                new Review
                {
                    Id = "REV037",
                    Tour_Id = "TOUR008",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Khách sạn sạch sẽ, thoải mái, lịch trình hợp lý. Sẽ giới thiệu cho bạn bè.",
                    Create_at = new DateTime(2025, 2, 5, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV038",
                    Tour_Id = "TOUR008",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Đồ ăn dở, ít sự lựa chọn. Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 3, 16, 10, 0)
                },
                new Review
                {
                    Id = "REV039",
                    Tour_Id = "TOUR008",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Hướng dẫn viên có kiến thức sâu rộng.",
                    Create_at = new DateTime(2024, 8, 8, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV040",
                    Tour_Id = "TOUR008",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Một kỳ nghỉ thư giãn và đáng nhớ. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2025, 4, 10, 14, 40, 0)
                },
                new Review
                {
                    Id = "REV041",
                    Tour_Id = "TOUR009",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Tuyệt vời cho gia đình có trẻ nhỏ. An toàn và vui vẻ suốt chuyến đi.",
                    Create_at = new DateTime(2024, 7, 5, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV042",
                    Tour_Id = "TOUR009",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Bãi biển sạch và đẹp. Hoạt động rất cuốn hút.",
                    Create_at = new DateTime(2025, 3, 20, 10, 40, 0)
                },
                new Review
                {
                    Id = "REV043",
                    Tour_Id = "TOUR009",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Đồ ăn tạm ổn. Tour này phù hợp cho người không kén chọn.",
                    Create_at = new DateTime(2024, 11, 15, 15, 40, 0)
                },
                new Review
                {
                    Id = "REV044",
                    Tour_Id = "TOUR009",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Rất hài lòng với chuyến đi này. Mọi thứ đều hoàn hảo.",
                    Create_at = new DateTime(2025, 1, 12, 8, 5, 0)
                },
                new Review
                {
                    Id = "REV045",
                    Tour_Id = "TOUR009",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm đáng tiền. Lịch trình hợp lý.",
                    Create_at = new DateTime(2024, 4, 29, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV046",
                    Tour_Id = "TOUR010",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Cảnh đẹp tuyệt vời! Hướng dẫn viên nhiệt tình.",
                    Create_at = new DateTime(2024, 9, 10, 9, 30, 0)
                },
                new Review
                {
                    Id = "REV047",
                    Tour_Id = "TOUR010",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Khách sạn và dịch vụ đều xuất sắc. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2025, 2, 25, 11, 20, 0)
                },
                new Review
                {
                    Id = "REV048",
                    Tour_Id = "TOUR010",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Xe đưa đón không đúng giờ. Dịch vụ rất tệ.",
                    Create_at = new DateTime(2024, 6, 8, 16, 20, 0)
                },
                new Review
                {
                    Id = "REV049",
                    Tour_Id = "TOUR010",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình tour rất hợp lý và thú vị. Sẽ giới thiệu tour này cho bạn bè.",
                    Create_at = new DateTime(2024, 7, 18, 13, 10, 0)
                },
                new Review
                {
                    Id = "REV050",
                    Tour_Id = "TOUR010",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 4, 15, 10, 50, 0)
                },
                new Review
                {
                    Id = "REV051",
                    Tour_Id = "TOUR011",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 8, 25, 14, 20, 0)
                },
                new Review
                {
                    Id = "REV052",
                    Tour_Id = "TOUR011",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 1, 7, 9, 15, 0)
                },
                new Review
                {
                    Id = "REV053",
                    Tour_Id = "TOUR011",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour này không quá ấn tượng. Mọi thứ diễn ra khá bình thường.",
                    Create_at = new DateTime(2024, 6, 28, 10, 20, 0)
                },
                new Review
                {
                    Id = "REV054",
                    Tour_Id = "TOUR011",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 4, 28, 15, 20, 0)
                },
                new Review
                {
                    Id = "REV055",
                    Tour_Id = "TOUR011",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 10, 13, 13, 40, 0)
                },
                new Review
                {
                    Id = "REV056",
                    Tour_Id = "TOUR012",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 9, 22, 8, 50, 0)
                },
                new Review
                {
                    Id = "REV057",
                    Tour_Id = "TOUR012",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 2, 7, 11, 10, 0)
                },
                new Review
                {
                    Id = "REV058",
                    Tour_Id = "TOUR012",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Khách sạn bẩn, không như quảng cáo. Lịch trình dày đặc, mệt mỏi.",
                    Create_at = new DateTime(2024, 6, 5, 16, 30, 0)
                },
                new Review
                {
                    Id = "REV059",
                    Tour_Id = "TOUR012",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 10, 9, 40, 0)
                },
                new Review
                {
                    Id = "REV060",
                    Tour_Id = "TOUR012",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 4, 13, 14, 50, 0)
                },
                new Review
                {
                    Id = "REV061",
                    Tour_Id = "TOUR013",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 8, 9, 30, 0)
                },
                new Review
                {
                    Id = "REV062",
                    Tour_Id = "TOUR013",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 23, 10, 50, 0)
                },
                new Review
                {
                    Id = "REV063",
                    Tour_Id = "TOUR013",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Chỉ ở mức trung bình. Tour này phù hợp cho người không kén chọn.",
                    Create_at = new DateTime(2024, 11, 18, 15, 50, 0)
                },
                new Review
                {
                    Id = "REV064",
                    Tour_Id = "TOUR013",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 15, 8, 15, 0)
                },
                new Review
                {
                    Id = "REV065",
                    Tour_Id = "TOUR013",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 2, 14, 20, 0)
                },
                new Review
                {
                    Id = "REV066",
                    Tour_Id = "TOUR014",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 9, 25, 8, 40, 0)
                },
                new Review
                {
                    Id = "REV067",
                    Tour_Id = "TOUR014",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 2, 10, 11, 10, 0)
                },
                new Review
                {
                    Id = "REV068",
                    Tour_Id = "TOUR014",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Trải nghiệm đáng quên. Giá quá đắt.",
                    Create_at = new DateTime(2024, 6, 10, 16, 40, 0)
                },
                new Review
                {
                    Id = "REV069",
                    Tour_Id = "TOUR014",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 13, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV070",
                    Tour_Id = "TOUR014",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 4, 18, 14, 40, 0)
                },
                new Review
                {
                    Id = "REV071",
                    Tour_Id = "TOUR015",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 11, 9, 40, 0)
                },
                new Review
                {
                    Id = "REV072",
                    Tour_Id = "TOUR015",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 26, 10, 55, 0)
                },
                new Review
                {
                    Id = "REV073",
                    Tour_Id = "TOUR015",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, không có gì nổi bật.",
                    Create_at = new DateTime(2024, 11, 21, 15, 55, 0)
                },
                new Review
                {
                    Id = "REV074",
                    Tour_Id = "TOUR015",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 18, 8, 20, 0)
                },
                new Review
                {
                    Id = "REV075",
                    Tour_Id = "TOUR015",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 5, 14, 25, 0)
                },
                new Review
                {
                    Id = "REV076",
                    Tour_Id = "TOUR016",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 9, 28, 8, 55, 0)
                },
                new Review
                {
                    Id = "REV077",
                    Tour_Id = "TOUR016",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 2, 13, 11, 15, 0)
                },
                new Review
                {
                    Id = "REV078",
                    Tour_Id = "TOUR016",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Lịch trình dày đặc, mệt mỏi. Dịch vụ rất kém.",
                    Create_at = new DateTime(2024, 6, 13, 16, 45, 0)
                },
                new Review
                {
                    Id = "REV079",
                    Tour_Id = "TOUR016",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 16, 9, 55, 0)
                },
                new Review
                {
                    Id = "REV080",
                    Tour_Id = "TOUR016",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 4, 21, 14, 55, 0)
                },
                new Review
                {
                    Id = "REV081",
                    Tour_Id = "TOUR017",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 14, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV082",
                    Tour_Id = "TOUR017",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 29, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV083",
                    Tour_Id = "TOUR017",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Cảnh đẹp, nhưng dịch trình cần cải thiện.",
                    Create_at = new DateTime(2024, 11, 24, 16, 0, 0)
                },
                new Review
                {
                    Id = "REV084",
                    Tour_Id = "TOUR017",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 21, 8, 25, 0)
                },
                new Review
                {
                    Id = "REV085",
                    Tour_Id = "TOUR017",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 8, 14, 30, 0)
                },
                new Review
                {
                    Id = "REV086",
                    Tour_Id = "TOUR018",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 9, 30, 9, 0, 0)
                },
                new Review
                {
                    Id = "REV087",
                    Tour_Id = "TOUR018",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 2, 15, 11, 20, 0)
                },
                new Review
                {
                    Id = "REV088",
                    Tour_Id = "TOUR018",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Điểm tham quan quá đông đúc. Trải nghiệm đáng quên.",
                    Create_at = new DateTime(2024, 6, 15, 16, 50, 0)
                },
                new Review
                {
                    Id = "REV089",
                    Tour_Id = "TOUR018",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 18, 10, 0, 0)
                },
                new Review
                {
                    Id = "REV090",
                    Tour_Id = "TOUR018",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 4, 23, 15, 0, 0)
                },
                new Review
                {
                    Id = "REV091",
                    Tour_Id = "TOUR019",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 17, 10, 0, 0)
                },
                new Review
                {
                    Id = "REV092",
                    Tour_Id = "TOUR019",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 4, 1, 11, 5, 0)
                },
                new Review
                {
                    Id = "REV093",
                    Tour_Id = "TOUR019",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được. Không có gì đặc biệt.",
                    Create_at = new DateTime(2024, 11, 26, 16, 5, 0)
                },
                new Review
                {
                    Id = "REV094",
                    Tour_Id = "TOUR019",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 24, 8, 30, 0)
                },
                new Review
                {
                    Id = "REV095",
                    Tour_Id = "TOUR019",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 11, 14, 35, 0)
                },
                new Review
                {
                    Id = "REV096",
                    Tour_Id = "TOUR020",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 3, 9, 5, 0)
                },
                new Review
                {
                    Id = "REV097",
                    Tour_Id = "TOUR020",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 2, 18, 11, 25, 0)
                },
                new Review
                {
                    Id = "REV098",
                    Tour_Id = "TOUR020",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Không có hoạt động nào thú vị. Dịch vụ rất kém.",
                    Create_at = new DateTime(2024, 6, 18, 16, 55, 0)
                },
                new Review
                {
                    Id = "REV099",
                    Tour_Id = "TOUR020",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 21, 10, 5, 0)
                },
                new Review
                {
                    Id = "REV100",
                    Tour_Id = "TOUR020",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 4, 26, 15, 5, 0)
                },
                new Review
                {
                    Id = "REV101",
                    Tour_Id = "TOUR021",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 20, 10, 0, 0)
                },
                new Review
                {
                    Id = "REV102",
                    Tour_Id = "TOUR021",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 1, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV103",
                    Tour_Id = "TOUR021",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Mọi thứ diễn ra khá bình thường.",
                    Create_at = new DateTime(2024, 11, 5, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV104",
                    Tour_Id = "TOUR021",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 27, 8, 30, 0)
                },
                new Review
                {
                    Id = "REV105",
                    Tour_Id = "TOUR021",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 15, 14, 0, 0)
                },
                new Review
                {
                    Id = "REV106",
                    Tour_Id = "TOUR022",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 6, 9, 0, 0)
                },
                new Review
                {
                    Id = "REV107",
                    Tour_Id = "TOUR022",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 2, 21, 11, 30, 0)
                },
                new Review
                {
                    Id = "REV108",
                    Tour_Id = "TOUR022",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Tour tệ quá! Trải nghiệm đáng quên.",
                    Create_at = new DateTime(2024, 6, 21, 16, 0, 0)
                },
                new Review
                {
                    Id = "REV109",
                    Tour_Id = "TOUR022",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 24, 9, 30, 0)
                },
                new Review
                {
                    Id = "REV110",
                    Tour_Id = "TOUR022",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 4, 29, 14, 0, 0)
                },
                new Review
                {
                    Id = "REV111",
                    Tour_Id = "TOUR023",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 23, 9, 30, 0)
                },
                new Review
                {
                    Id = "REV112",
                    Tour_Id = "TOUR023",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 4, 10, 45, 0)
                },
                new Review
                {
                    Id = "REV113",
                    Tour_Id = "TOUR023",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được. Không có gì đặc biệt.",
                    Create_at = new DateTime(2024, 11, 8, 15, 35, 0)
                },
                new Review
                {
                    Id = "REV114",
                    Tour_Id = "TOUR023",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 30, 8, 40, 0)
                },
                new Review
                {
                    Id = "REV115",
                    Tour_Id = "TOUR023",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 18, 14, 5, 0)
                },
                new Review
                {
                    Id = "REV116",
                    Tour_Id = "TOUR024",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 9, 9, 10, 0)
                },
                new Review
                {
                    Id = "REV117",
                    Tour_Id = "TOUR024",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 2, 24, 11, 40, 0)
                },
                new Review
                {
                    Id = "REV118",
                    Tour_Id = "TOUR024",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Giá quá đắt so với chất lượng nhận được. Dịch vụ rất kém.",
                    Create_at = new DateTime(2024, 6, 24, 16, 10, 0)
                },
                new Review
                {
                    Id = "REV119",
                    Tour_Id = "TOUR024",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 27, 9, 40, 0)
                },
                new Review
                {
                    Id = "REV120",
                    Tour_Id = "TOUR024",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 2, 14, 0, 0)
                },
                new Review
                {
                    Id = "REV121",
                    Tour_Id = "TOUR025",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 26, 9, 45, 0)
                },
                new Review
                {
                    Id = "REV122",
                    Tour_Id = "TOUR025",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 7, 10, 50, 0)
                },
                new Review
                {
                    Id = "REV123",
                    Tour_Id = "TOUR025",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour này không quá ấn tượng.",
                    Create_at = new DateTime(2024, 11, 11, 15, 45, 0)
                },
                new Review
                {
                    Id = "REV124",
                    Tour_Id = "TOUR025",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 2, 2, 8, 45, 0)
                },
                new Review
                {
                    Id = "REV125",
                    Tour_Id = "TOUR025",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 21, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV126",
                    Tour_Id = "TOUR026",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 12, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV127",
                    Tour_Id = "TOUR026",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 2, 27, 11, 55, 0)
                },
                new Review
                {
                    Id = "REV128",
                    Tour_Id = "TOUR026",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Hướng dẫn viên thiếu chuyên nghiệp. Trải nghiệm đáng quên.",
                    Create_at = new DateTime(2024, 6, 27, 16, 25, 0)
                },
                new Review
                {
                    Id = "REV129",
                    Tour_Id = "TOUR026",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 30, 10, 0, 0)
                },
                new Review
                {
                    Id = "REV130",
                    Tour_Id = "TOUR026",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 5, 15, 10, 0)
                },
                new Review
                {
                    Id = "REV131",
                    Tour_Id = "TOUR027",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 29, 10, 10, 0)
                },
                new Review
                {
                    Id = "REV132",
                    Tour_Id = "TOUR027",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 10, 11, 15, 0)
                },
                new Review
                {
                    Id = "REV133",
                    Tour_Id = "TOUR027",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, nhưng cần cải thiện lịch trình.",
                    Create_at = new DateTime(2024, 11, 14, 15, 40, 0)
                },
                new Review
                {
                    Id = "REV134",
                    Tour_Id = "TOUR027",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 2, 5, 8, 50, 0)
                },
                new Review
                {
                    Id = "REV135",
                    Tour_Id = "TOUR027",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 24, 14, 20, 0)
                },
                new Review
                {
                    Id = "REV136",
                    Tour_Id = "TOUR028",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 15, 9, 30, 0)
                },
                new Review
                {
                    Id = "REV137",
                    Tour_Id = "TOUR028",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 3, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV138",
                    Tour_Id = "TOUR028",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Dịch vụ kém, không đáng tiền. Trải nghiệm đáng quên.",
                    Create_at = new DateTime(2024, 6, 30, 16, 35, 0)
                },
                new Review
                {
                    Id = "REV139",
                    Tour_Id = "TOUR028",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 31, 9, 55, 0)
                },
                new Review
                {
                    Id = "REV140",
                    Tour_Id = "TOUR028",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 8, 14, 0, 0)
                },
                new Review
                {
                    Id = "REV141",
                    Tour_Id = "TOUR029",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 2, 10, 5, 0)
                },
                new Review
                {
                    Id = "REV142",
                    Tour_Id = "TOUR029",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 21, 10, 50, 0)
                },
                new Review
                {
                    Id = "REV143",
                    Tour_Id = "TOUR029",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Cảnh đẹp, nhưng dịch vụ bình thường.",
                    Create_at = new DateTime(2024, 11, 3, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV144",
                    Tour_Id = "TOUR029",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 29, 8, 35, 0)
                },
                new Review
                {
                    Id = "REV145",
                    Tour_Id = "TOUR029",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 13, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV146",
                    Tour_Id = "TOUR030",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 1, 9, 15, 0)
                },
                new Review
                {
                    Id = "REV147",
                    Tour_Id = "TOUR030",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 2, 26, 11, 40, 0)
                },
                new Review
                {
                    Id = "REV148",
                    Tour_Id = "TOUR030",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Tour tệ quá!",
                    Create_at = new DateTime(2024, 6, 26, 16, 15, 0)
                },
                new Review
                {
                    Id = "REV149",
                    Tour_Id = "TOUR030",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 29, 9, 45, 0)
                },
                new Review
                {
                    Id = "REV150",
                    Tour_Id = "TOUR030",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 4, 14, 15, 0)
                },
                new Review
                {
                    Id = "REV151",
                    Tour_Id = "TOUR031",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 4, 10, 15, 0)
                },
                new Review
                {
                    Id = "REV152",
                    Tour_Id = "TOUR031",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 24, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV153",
                    Tour_Id = "TOUR031",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Khách sạn ở mức chấp nhận được. Tour tạm được.",
                    Create_at = new DateTime(2024, 11, 6, 15, 35, 0)
                },
                new Review
                {
                    Id = "REV154",
                    Tour_Id = "TOUR031",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 50, 0)
                },
                new Review
                {
                    Id = "REV155",
                    Tour_Id = "TOUR031",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 16, 14, 15, 0)
                },
                new Review
                {
                    Id = "REV156",
                    Tour_Id = "TOUR032",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 4, 9, 25, 0)
                },
                new Review
                {
                    Id = "REV157",
                    Tour_Id = "TOUR032",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 1, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV158",
                    Tour_Id = "TOUR032",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Dịch vụ kém.",
                    Create_at = new DateTime(2024, 6, 23, 16, 40, 0)
                },
                new Review
                {
                    Id = "REV159",
                    Tour_Id = "TOUR032",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 26, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV160",
                    Tour_Id = "TOUR032",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 7, 15, 0, 0)
                },
                new Review
                {
                    Id = "REV161",
                    Tour_Id = "TOUR033",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 25, 10, 5, 0)
                },
                new Review
                {
                    Id = "REV162",
                    Tour_Id = "TOUR033",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 6, 11, 5, 0)
                },
                new Review
                {
                    Id = "REV163",
                    Tour_Id = "TOUR033",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, không có gì nổi bật.",
                    Create_at = new DateTime(2024, 11, 9, 15, 40, 0)
                },
                new Review
                {
                    Id = "REV164",
                    Tour_Id = "TOUR033",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 2, 1, 8, 55, 0)
                },
                new Review
                {
                    Id = "REV165",
                    Tour_Id = "TOUR033",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 20, 14, 15, 0)
                },
                new Review
                {
                    Id = "REV166",
                    Tour_Id = "TOUR034",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 7, 9, 35, 0)
                },
                new Review
                {
                    Id = "REV167",
                    Tour_Id = "TOUR034",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 4, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV168",
                    Tour_Id = "TOUR034",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Tour tệ quá!",
                    Create_at = new DateTime(2024, 6, 28, 16, 20, 0)
                },
                new Review
                {
                    Id = "REV169",
                    Tour_Id = "TOUR034",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 30, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV170",
                    Tour_Id = "TOUR034",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 9, 14, 5, 0)
                },
                new Review
                {
                    Id = "REV171",
                    Tour_Id = "TOUR035",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 1, 10, 0, 0)
                },
                new Review
                {
                    Id = "REV172",
                    Tour_Id = "TOUR035",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 19, 10, 55, 0)
                },
                new Review
                {
                    Id = "REV173",
                    Tour_Id = "TOUR035",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Đồ ăn tạm ổn.",
                    Create_at = new DateTime(2024, 11, 1, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV174",
                    Tour_Id = "TOUR035",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 28, 8, 40, 0)
                },
                new Review
                {
                    Id = "REV175",
                    Tour_Id = "TOUR035",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 10, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV176",
                    Tour_Id = "TOUR036",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 10, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV177",
                    Tour_Id = "TOUR036",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 6, 11, 45, 0)
                },
                new Review
                {
                    Id = "REV178",
                    Tour_Id = "TOUR036",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Điểm tham quan quá đông đúc.",
                    Create_at = new DateTime(2024, 6, 29, 16, 15, 0)
                },
                new Review
                {
                    Id = "REV179",
                    Tour_Id = "TOUR036",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 31, 9, 40, 0)
                },
                new Review
                {
                    Id = "REV180",
                    Tour_Id = "TOUR036",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 8, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV181",
                    Tour_Id = "TOUR037",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 3, 10, 10, 0)
                },
                new Review
                {
                    Id = "REV182",
                    Tour_Id = "TOUR037",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 22, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV183",
                    Tour_Id = "TOUR037",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, nhưng cần cải thiện lịch trình.",
                    Create_at = new DateTime(2024, 11, 4, 15, 35, 0)
                },
                new Review
                {
                    Id = "REV184",
                    Tour_Id = "TOUR037",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 30, 8, 45, 0)
                },
                new Review
                {
                    Id = "REV185",
                    Tour_Id = "TOUR037",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 19, 14, 20, 0)
                },
                new Review
                {
                    Id = "REV186",
                    Tour_Id = "TOUR038",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 8, 9, 25, 0)
                },
                new Review
                {
                    Id = "REV187",
                    Tour_Id = "TOUR038",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 5, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV188",
                    Tour_Id = "TOUR038",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém.",
                    Create_at = new DateTime(2024, 6, 28, 16, 30, 0)
                },
                new Review
                {
                    Id = "REV189",
                    Tour_Id = "TOUR038",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 30, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV190",
                    Tour_Id = "TOUR038",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 9, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV191",
                    Tour_Id = "TOUR039",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 5, 10, 0, 0)
                },
                new Review
                {
                    Id = "REV192",
                    Tour_Id = "TOUR039",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 24, 10, 55, 0)
                },
                new Review
                {
                    Id = "REV193",
                    Tour_Id = "TOUR039",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, không có gì nổi bật.",
                    Create_at = new DateTime(2024, 11, 7, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV194",
                    Tour_Id = "TOUR039",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 40, 0)
                },
                new Review
                {
                    Id = "REV195",
                    Tour_Id = "TOUR039",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 12, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV196",
                    Tour_Id = "TOUR040",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 9, 9, 10, 0)
                },
                new Review
                {
                    Id = "REV197",
                    Tour_Id = "TOUR040",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 6, 11, 40, 0)
                },
                new Review
                {
                    Id = "REV198",
                    Tour_Id = "TOUR040",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 29, 16, 10, 0)
                },
                new Review
                {
                    Id = "REV199",
                    Tour_Id = "TOUR040",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 31, 9, 35, 0)
                },
                new Review
                {
                    Id = "REV200",
                    Tour_Id = "TOUR040",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 8, 14, 5, 0)
                },
                new Review
                {
                    Id = "REV201",
                    Tour_Id = "TOUR041",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 7, 10, 0, 0)
                },
                new Review
                {
                    Id = "REV202",
                    Tour_Id = "TOUR041",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 26, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV203",
                    Tour_Id = "TOUR041",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được. Không có gì đặc biệt.",
                    Create_at = new DateTime(2024, 11, 8, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV204",
                    Tour_Id = "TOUR041",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 2, 1, 8, 50, 0)
                },
                new Review
                {
                    Id = "REV205",
                    Tour_Id = "TOUR041",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 13, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV206",
                    Tour_Id = "TOUR042",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 6, 9, 10, 0)
                },
                new Review
                {
                    Id = "REV207",
                    Tour_Id = "TOUR042",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 3, 11, 45, 0)
                },
                new Review
                {
                    Id = "REV208",
                    Tour_Id = "TOUR042",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Trải nghiệm đáng quên.",
                    Create_at = new DateTime(2024, 6, 28, 16, 30, 0)
                },
                new Review
                {
                    Id = "REV209",
                    Tour_Id = "TOUR042",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 31, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV210",
                    Tour_Id = "TOUR042",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 9, 14, 0, 0)
                },
                new Review
                {
                    Id = "REV211",
                    Tour_Id = "TOUR043",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 5, 10, 5, 0)
                },
                new Review
                {
                    Id = "REV212",
                    Tour_Id = "TOUR043",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 24, 11, 5, 0)
                },
                new Review
                {
                    Id = "REV213",
                    Tour_Id = "TOUR043",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour này không quá ấn tượng.",
                    Create_at = new DateTime(2024, 11, 7, 15, 35, 0)
                },
                new Review
                {
                    Id = "REV214",
                    Tour_Id = "TOUR043",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 45, 0)
                },
                new Review
                {
                    Id = "REV215",
                    Tour_Id = "TOUR043",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 12, 14, 15, 0)
                },
                new Review
                {
                    Id = "REV216",
                    Tour_Id = "TOUR044",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 10, 9, 25, 0)
                },
                new Review
                {
                    Id = "REV217",
                    Tour_Id = "TOUR044",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 7, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV218",
                    Tour_Id = "TOUR044",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém. Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 29, 16, 20, 0)
                },
                new Review
                {
                    Id = "REV219",
                    Tour_Id = "TOUR044",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 31, 9, 45, 0)
                },
                new Review
                {
                    Id = "REV220",
                    Tour_Id = "TOUR044",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 8, 14, 15, 0)
                },
                new Review
                {
                    Id = "REV221",
                    Tour_Id = "TOUR045",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 8, 10, 0, 0)
                },
                new Review
                {
                    Id = "REV222",
                    Tour_Id = "TOUR045",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 27, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV223",
                    Tour_Id = "TOUR045",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Đồ ăn tạm ổn.",
                    Create_at = new DateTime(2024, 11, 9, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV224",
                    Tour_Id = "TOUR045",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 2, 2, 8, 50, 0)
                },
                new Review
                {
                    Id = "REV225",
                    Tour_Id = "TOUR045",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 14, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV226",
                    Tour_Id = "TOUR046",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 11, 9, 15, 0)
                },
                new Review
                {
                    Id = "REV227",
                    Tour_Id = "TOUR046",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 8, 11, 45, 0)
                },
                new Review
                {
                    Id = "REV228",
                    Tour_Id = "TOUR046",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Không có hoạt động nào thú vị.",
                    Create_at = new DateTime(2024, 6, 30, 16, 15, 0)
                },
                new Review
                {
                    Id = "REV229",
                    Tour_Id = "TOUR046",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 9, 2, 9, 40, 0)
                },
                new Review
                {
                    Id = "REV230",
                    Tour_Id = "TOUR046",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 6, 14, 0, 0)
                },
                new Review
                {
                    Id = "REV231",
                    Tour_Id = "TOUR047",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 4, 10, 5, 0)
                },
                new Review
                {
                    Id = "REV232",
                    Tour_Id = "TOUR047",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 23, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV233",
                    Tour_Id = "TOUR047",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được.",
                    Create_at = new DateTime(2024, 11, 5, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV234",
                    Tour_Id = "TOUR047",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 50, 0)
                },
                new Review
                {
                    Id = "REV235",
                    Tour_Id = "TOUR047",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 16, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV236",
                    Tour_Id = "TOUR048",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 5, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV237",
                    Tour_Id = "TOUR048",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 2, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV238",
                    Tour_Id = "TOUR048",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém. Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 25, 16, 40, 0)
                },
                new Review
                {
                    Id = "REV239",
                    Tour_Id = "TOUR048",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 27, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV240",
                    Tour_Id = "TOUR048",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 7, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV241",
                    Tour_Id = "TOUR049",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 3, 10, 10, 0)
                },
                new Review
                {
                    Id = "REV242",
                    Tour_Id = "TOUR049",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 22, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV243",
                    Tour_Id = "TOUR049",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, nhưng cần cải thiện lịch trình.",
                    Create_at = new DateTime(2024, 11, 4, 15, 35, 0)
                },
                new Review
                {
                    Id = "REV244",
                    Tour_Id = "TOUR049",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 30, 8, 45, 0)
                },
                new Review
                {
                    Id = "REV245",
                    Tour_Id = "TOUR049",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 19, 14, 20, 0)
                },
                new Review
                {
                    Id = "REV246",
                    Tour_Id = "TOUR050",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 8, 9, 25, 0)
                },
                new Review
                {
                    Id = "REV247",
                    Tour_Id = "TOUR050",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 5, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV248",
                    Tour_Id = "TOUR050",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Dịch vụ rất tệ.",
                    Create_at = new DateTime(2024, 6, 28, 16, 30, 0)
                },
                new Review
                {
                    Id = "REV249",
                    Tour_Id = "TOUR050",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 30, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV250",
                    Tour_Id = "TOUR050",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 9, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV251",
                    Tour_Id = "TOUR051",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 5, 10, 0, 0)
                },
                new Review
                {
                    Id = "REV252",
                    Tour_Id = "TOUR051",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 24, 10, 55, 0)
                },
                new Review
                {
                    Id = "REV253",
                    Tour_Id = "TOUR051",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, không có gì nổi bật.",
                    Create_at = new DateTime(2024, 11, 7, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV254",
                    Tour_Id = "TOUR051",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 40, 0)
                },
                new Review
                {
                    Id = "REV255",
                    Tour_Id = "TOUR051",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 12, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV256",
                    Tour_Id = "TOUR052",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 12, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV257",
                    Tour_Id = "TOUR052",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 8, 11, 45, 0)
                },
                new Review
                {
                    Id = "REV258",
                    Tour_Id = "TOUR052",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 30, 16, 15, 0)
                },
                new Review
                {
                    Id = "REV259",
                    Tour_Id = "TOUR052",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 9, 2, 9, 40, 0)
                },
                new Review
                {
                    Id = "REV260",
                    Tour_Id = "TOUR052",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 6, 14, 0, 0)
                },
                new Review
                {
                    Id = "REV261",
                    Tour_Id = "TOUR053",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 4, 10, 5, 0)
                },
                new Review
                {
                    Id = "REV262",
                    Tour_Id = "TOUR053",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 23, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV263",
                    Tour_Id = "TOUR053",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được.",
                    Create_at = new DateTime(2024, 11, 5, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV264",
                    Tour_Id = "TOUR053",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 50, 0)
                },
                new Review
                {
                    Id = "REV265",
                    Tour_Id = "TOUR053",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 16, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV266",
                    Tour_Id = "TOUR054",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 5, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV267",
                    Tour_Id = "TOUR054",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 2, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV268",
                    Tour_Id = "TOUR054",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém. Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 25, 16, 40, 0)
                },
                new Review
                {
                    Id = "REV269",
                    Tour_Id = "TOUR054",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 27, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV270",
                    Tour_Id = "TOUR054",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 7, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV271",
                    Tour_Id = "TOUR055",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 3, 10, 10, 0)
                },
                new Review
                {
                    Id = "REV272",
                    Tour_Id = "TOUR055",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 22, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV273",
                    Tour_Id = "TOUR055",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, nhưng cần cải thiện lịch trình.",
                    Create_at = new DateTime(2024, 11, 4, 15, 35, 0)
                },
                new Review
                {
                    Id = "REV274",
                    Tour_Id = "TOUR055",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 30, 8, 45, 0)
                },
                new Review
                {
                    Id = "REV275",
                    Tour_Id = "TOUR055",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 19, 14, 20, 0)
                },
                new Review
                {
                    Id = "REV276",
                    Tour_Id = "TOUR056",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 8, 9, 25, 0)
                },
                new Review
                {
                    Id = "REV277",
                    Tour_Id = "TOUR056",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 5, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV278",
                    Tour_Id = "TOUR056",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém.",
                    Create_at = new DateTime(2024, 6, 28, 16, 30, 0)
                },
                new Review
                {
                    Id = "REV279",
                    Tour_Id = "TOUR056",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 30, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV280",
                    Tour_Id = "TOUR056",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 9, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV281",
                    Tour_Id = "TOUR057",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 5, 10, 0, 0)
                },
                new Review
                {
                    Id = "REV282",
                    Tour_Id = "TOUR057",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 24, 10, 55, 0)
                },
                new Review
                {
                    Id = "REV283",
                    Tour_Id = "TOUR057",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, không có gì nổi bật.",
                    Create_at = new DateTime(2024, 11, 7, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV284",
                    Tour_Id = "TOUR057",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 40, 0)
                },
                new Review
                {
                    Id = "REV285",
                    Tour_Id = "TOUR057",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 12, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV286",
                    Tour_Id = "TOUR058",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 12, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV287",
                    Tour_Id = "TOUR058",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 8, 11, 45, 0)
                },
                new Review
                {
                    Id = "REV288",
                    Tour_Id = "TOUR058",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 30, 16, 15, 0)
                },
                new Review
                {
                    Id = "REV289",
                    Tour_Id = "TOUR058",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 9, 2, 9, 40, 0)
                },
                new Review
                {
                    Id = "REV290",
                    Tour_Id = "TOUR058",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 6, 14, 0, 0)
                },
                new Review
                {
                    Id = "REV291",
                    Tour_Id = "TOUR059",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 4, 10, 5, 0)
                },
                new Review
                {
                    Id = "REV292",
                    Tour_Id = "TOUR059",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 23, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV293",
                    Tour_Id = "TOUR059",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được.",
                    Create_at = new DateTime(2024, 11, 5, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV294",
                    Tour_Id = "TOUR059",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 50, 0)
                },
                new Review
                {
                    Id = "REV295",
                    Tour_Id = "TOUR059",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 16, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV296",
                    Tour_Id = "TOUR060",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 5, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV297",
                    Tour_Id = "TOUR060",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 2, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV298",
                    Tour_Id = "TOUR060",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Dịch vụ rất kém. Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 25, 16, 40, 0)
                },
                new Review
                {
                    Id = "REV299",
                    Tour_Id = "TOUR060",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 27, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV300",
                    Tour_Id = "TOUR060",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 7, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV301",
                    Tour_Id = "TOUR061",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 3, 10, 10, 0)
                },
                new Review
                {
                    Id = "REV302",
                    Tour_Id = "TOUR061",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 22, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV303",
                    Tour_Id = "TOUR061",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, nhưng cần cải thiện lịch trình.",
                    Create_at = new DateTime(2024, 11, 4, 15, 35, 0)
                },
                new Review
                {
                    Id = "REV304",
                    Tour_Id = "TOUR061",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 30, 8, 45, 0)
                },
                new Review
                {
                    Id = "REV305",
                    Tour_Id = "TOUR061",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 19, 14, 20, 0)
                },
                new Review
                {
                    Id = "REV306",
                    Tour_Id = "TOUR062",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 8, 9, 25, 0)
                },
                new Review
                {
                    Id = "REV307",
                    Tour_Id = "TOUR062",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 5, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV308",
                    Tour_Id = "TOUR062",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém.",
                    Create_at = new DateTime(2024, 6, 28, 16, 30, 0)
                },
                new Review
                {
                    Id = "REV309",
                    Tour_Id = "TOUR062",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 30, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV310",
                    Tour_Id = "TOUR062",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 9, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV311",
                    Tour_Id = "TOUR063",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 5, 10, 0, 0)
                },
                new Review
                {
                    Id = "REV312",
                    Tour_Id = "TOUR063",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 24, 10, 55, 0)
                },
                new Review
                {
                    Id = "REV313",
                    Tour_Id = "TOUR063",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, không có gì nổi bật.",
                    Create_at = new DateTime(2024, 11, 7, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV314",
                    Tour_Id = "TOUR063",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 40, 0)
                },
                new Review
                {
                    Id = "REV315",
                    Tour_Id = "TOUR063",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 12, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV316",
                    Tour_Id = "TOUR064",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 12, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV317",
                    Tour_Id = "TOUR064",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 8, 11, 45, 0)
                },
                new Review
                {
                    Id = "REV318",
                    Tour_Id = "TOUR064",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 30, 16, 15, 0)
                },
                new Review
                {
                    Id = "REV319",
                    Tour_Id = "TOUR064",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 9, 2, 9, 40, 0)
                },
                new Review
                {
                    Id = "REV320",
                    Tour_Id = "TOUR064",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 6, 14, 0, 0)
                },
                new Review
                {
                    Id = "REV321",
                    Tour_Id = "TOUR065",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 4, 10, 5, 0)
                },
                new Review
                {
                    Id = "REV322",
                    Tour_Id = "TOUR065",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 23, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV323",
                    Tour_Id = "TOUR065",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được.",
                    Create_at = new DateTime(2024, 11, 5, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV324",
                    Tour_Id = "TOUR065",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 50, 0)
                },
                new Review
                {
                    Id = "REV325",
                    Tour_Id = "TOUR065",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 16, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV326",
                    Tour_Id = "TOUR066",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 5, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV327",
                    Tour_Id = "TOUR066",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 2, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV328",
                    Tour_Id = "TOUR066",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém. Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 25, 16, 40, 0)
                },
                new Review
                {
                    Id = "REV329",
                    Tour_Id = "TOUR066",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 27, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV330",
                    Tour_Id = "TOUR066",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 7, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV331",
                    Tour_Id = "TOUR067",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 3, 10, 10, 0)
                },
                new Review
                {
                    Id = "REV332",
                    Tour_Id = "TOUR067",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 22, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV333",
                    Tour_Id = "TOUR067",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, nhưng cần cải thiện lịch trình.",
                    Create_at = new DateTime(2024, 11, 4, 15, 35, 0)
                },
                new Review
                {
                    Id = "REV334",
                    Tour_Id = "TOUR067",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 30, 8, 45, 0)
                },
                new Review
                {
                    Id = "REV335",
                    Tour_Id = "TOUR067",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 19, 14, 20, 0)
                },
                new Review
                {
                    Id = "REV336",
                    Tour_Id = "TOUR068",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 8, 9, 25, 0)
                },
                new Review
                {
                    Id = "REV337",
                    Tour_Id = "TOUR068",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 5, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV338",
                    Tour_Id = "TOUR068",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém.",
                    Create_at = new DateTime(2024, 6, 28, 16, 30, 0)
                },
                new Review
                {
                    Id = "REV339",
                    Tour_Id = "TOUR068",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 30, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV340",
                    Tour_Id = "TOUR068",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 9, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV341",
                    Tour_Id = "TOUR069",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 5, 10, 0, 0)
                },
                new Review
                {
                    Id = "REV342",
                    Tour_Id = "TOUR069",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 24, 10, 55, 0)
                },
                new Review
                {
                    Id = "REV343",
                    Tour_Id = "TOUR069",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, không có gì nổi bật.",
                    Create_at = new DateTime(2024, 11, 7, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV344",
                    Tour_Id = "TOUR069",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 40, 0)
                },
                new Review
                {
                    Id = "REV345",
                    Tour_Id = "TOUR069",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 12, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV346",
                    Tour_Id = "TOUR070",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 12, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV347",
                    Tour_Id = "TOUR070",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 8, 11, 45, 0)
                },
                new Review
                {
                    Id = "REV348",
                    Tour_Id = "TOUR070",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 30, 16, 15, 0)
                },
                new Review
                {
                    Id = "REV349",
                    Tour_Id = "TOUR070",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 9, 2, 9, 40, 0)
                },
                new Review
                {
                    Id = "REV350",
                    Tour_Id = "TOUR070",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 6, 14, 0, 0)
                },
                new Review
                {
                    Id = "REV351",
                    Tour_Id = "TOUR071",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 4, 10, 5, 0)
                },
                new Review
                {
                    Id = "REV352",
                    Tour_Id = "TOUR071",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 23, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV353",
                    Tour_Id = "TOUR071",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được.",
                    Create_at = new DateTime(2024, 11, 5, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV354",
                    Tour_Id = "TOUR071",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 50, 0)
                },
                new Review
                {
                    Id = "REV355",
                    Tour_Id = "TOUR071",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 16, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV356",
                    Tour_Id = "TOUR072",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 5, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV357",
                    Tour_Id = "TOUR072",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 2, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV358",
                    Tour_Id = "TOUR072",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém. Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 25, 16, 40, 0)
                },
                new Review
                {
                    Id = "REV359",
                    Tour_Id = "TOUR072",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 27, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV360",
                    Tour_Id = "TOUR072",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 7, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV361",
                    Tour_Id = "TOUR073",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 3, 10, 10, 0)
                },
                new Review
                {
                    Id = "REV362",
                    Tour_Id = "TOUR073",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 22, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV363",
                    Tour_Id = "TOUR073",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, nhưng cần cải thiện lịch trình.",
                    Create_at = new DateTime(2024, 11, 4, 15, 35, 0)
                },
                new Review
                {
                    Id = "REV364",
                    Tour_Id = "TOUR073",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 30, 8, 45, 0)
                },
                new Review
                {
                    Id = "REV365",
                    Tour_Id = "TOUR073",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 19, 14, 20, 0)
                },
                new Review
                {
                    Id = "REV366",
                    Tour_Id = "TOUR074",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 8, 9, 25, 0)
                },
                new Review
                {
                    Id = "REV367",
                    Tour_Id = "TOUR074",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 5, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV368",
                    Tour_Id = "TOUR074",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém.",
                    Create_at = new DateTime(2024, 6, 28, 16, 30, 0)
                },
                new Review
                {
                    Id = "REV369",
                    Tour_Id = "TOUR074",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 30, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV370",
                    Tour_Id = "TOUR074",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 9, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV371",
                    Tour_Id = "TOUR075",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 5, 10, 0, 0)
                },
                new Review
                {
                    Id = "REV372",
                    Tour_Id = "TOUR075",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 24, 10, 55, 0)
                },
                new Review
                {
                    Id = "REV373",
                    Tour_Id = "TOUR075",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, không có gì nổi bật.",
                    Create_at = new DateTime(2024, 11, 7, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV374",
                    Tour_Id = "TOUR075",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 40, 0)
                },
                new Review
                {
                    Id = "REV375",
                    Tour_Id = "TOUR075",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 12, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV376",
                    Tour_Id = "TOUR076",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 12, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV377",
                    Tour_Id = "TOUR076",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 8, 11, 45, 0)
                },
                new Review
                {
                    Id = "REV378",
                    Tour_Id = "TOUR076",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 30, 16, 15, 0)
                },
                new Review
                {
                    Id = "REV379",
                    Tour_Id = "TOUR076",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 9, 2, 9, 40, 0)
                },
                new Review
                {
                    Id = "REV380",
                    Tour_Id = "TOUR076",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 6, 14, 0, 0)
                },
                new Review
                {
                    Id = "REV381",
                    Tour_Id = "TOUR077",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 4, 10, 5, 0)
                },
                new Review
                {
                    Id = "REV382",
                    Tour_Id = "TOUR077",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 23, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV383",
                    Tour_Id = "TOUR077",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được.",
                    Create_at = new DateTime(2024, 11, 5, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV384",
                    Tour_Id = "TOUR077",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 50, 0)
                },
                new Review
                {
                    Id = "REV385",
                    Tour_Id = "TOUR077",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 16, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV386",
                    Tour_Id = "TOUR078",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 5, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV387",
                    Tour_Id = "TOUR078",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 2, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV388",
                    Tour_Id = "TOUR078",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém. Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 25, 16, 40, 0)
                },
                new Review
                {
                    Id = "REV389",
                    Tour_Id = "TOUR078",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 27, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV390",
                    Tour_Id = "TOUR078",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 7, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV391",
                    Tour_Id = "TOUR079",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 3, 10, 10, 0)
                },
                new Review
                {
                    Id = "REV392",
                    Tour_Id = "TOUR079",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 22, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV393",
                    Tour_Id = "TOUR079",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, nhưng cần cải thiện lịch trình.",
                    Create_at = new DateTime(2024, 11, 4, 15, 35, 0)
                },
                new Review
                {
                    Id = "REV394",
                    Tour_Id = "TOUR079",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 30, 8, 45, 0)
                },
                new Review
                {
                    Id = "REV395",
                    Tour_Id = "TOUR079",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 19, 14, 20, 0)
                },
                new Review
                {
                    Id = "REV396",
                    Tour_Id = "TOUR080",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 8, 9, 25, 0)
                },
                new Review
                {
                    Id = "REV397",
                    Tour_Id = "TOUR080",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 5, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV398",
                    Tour_Id = "TOUR080",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém.",
                    Create_at = new DateTime(2024, 6, 28, 16, 30, 0)
                },
                new Review
                {
                    Id = "REV399",
                    Tour_Id = "TOUR080",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 30, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV400",
                    Tour_Id = "TOUR080",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 9, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV401",
                    Tour_Id = "TOUR081",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 5, 10, 0, 0)
                },
                new Review
                {
                    Id = "REV402",
                    Tour_Id = "TOUR081",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 24, 10, 55, 0)
                },
                new Review
                {
                    Id = "REV403",
                    Tour_Id = "TOUR081",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, không có gì nổi bật.",
                    Create_at = new DateTime(2024, 11, 7, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV404",
                    Tour_Id = "TOUR081",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 40, 0)
                },
                new Review
                {
                    Id = "REV405",
                    Tour_Id = "TOUR081",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 12, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV406",
                    Tour_Id = "TOUR082",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 12, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV407",
                    Tour_Id = "TOUR082",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 8, 11, 45, 0)
                },
                new Review
                {
                    Id = "REV408",
                    Tour_Id = "TOUR082",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 30, 16, 15, 0)
                },
                new Review
                {
                    Id = "REV409",
                    Tour_Id = "TOUR082",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 9, 2, 9, 40, 0)
                },
                new Review
                {
                    Id = "REV410",
                    Tour_Id = "TOUR082",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 6, 14, 0, 0)
                },
                new Review
                {
                    Id = "REV411",
                    Tour_Id = "TOUR083",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 4, 10, 5, 0)
                },
                new Review
                {
                    Id = "REV412",
                    Tour_Id = "TOUR083",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 23, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV413",
                    Tour_Id = "TOUR083",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được.",
                    Create_at = new DateTime(2024, 11, 5, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV414",
                    Tour_Id = "TOUR083",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 50, 0)
                },
                new Review
                {
                    Id = "REV415",
                    Tour_Id = "TOUR083",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 16, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV416",
                    Tour_Id = "TOUR084",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 5, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV417",
                    Tour_Id = "TOUR084",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 2, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV418",
                    Tour_Id = "TOUR084",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém. Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 25, 16, 40, 0)
                },
                new Review
                {
                    Id = "REV419",
                    Tour_Id = "TOUR084",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 27, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV420",
                    Tour_Id = "TOUR084",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 7, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV421",
                    Tour_Id = "TOUR085",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 3, 10, 10, 0)
                },
                new Review
                {
                    Id = "REV422",
                    Tour_Id = "TOUR085",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 22, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV423",
                    Tour_Id = "TOUR085",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, nhưng cần cải thiện lịch trình.",
                    Create_at = new DateTime(2024, 11, 4, 15, 35, 0)
                },
                new Review
                {
                    Id = "REV424",
                    Tour_Id = "TOUR085",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 30, 8, 45, 0)
                },
                new Review
                {
                    Id = "REV425",
                    Tour_Id = "TOUR085",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 19, 14, 20, 0)
                },
                new Review
                {
                    Id = "REV426",
                    Tour_Id = "TOUR086",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 8, 9, 25, 0)
                },
                new Review
                {
                    Id = "REV427",
                    Tour_Id = "TOUR086",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 5, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV428",
                    Tour_Id = "TOUR086",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém.",
                    Create_at = new DateTime(2024, 6, 28, 16, 30, 0)
                },
                new Review
                {
                    Id = "REV429",
                    Tour_Id = "TOUR086",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 30, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV430",
                    Tour_Id = "TOUR086",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 9, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV431",
                    Tour_Id = "TOUR087",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 5, 10, 0, 0)
                },
                new Review
                {
                    Id = "REV432",
                    Tour_Id = "TOUR087",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 24, 10, 55, 0)
                },
                new Review
                {
                    Id = "REV433",
                    Tour_Id = "TOUR087",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, không có gì nổi bật.",
                    Create_at = new DateTime(2024, 11, 7, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV434",
                    Tour_Id = "TOUR087",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 40, 0)
                },
                new Review
                {
                    Id = "REV435",
                    Tour_Id = "TOUR087",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 12, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV436",
                    Tour_Id = "TOUR088",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 12, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV437",
                    Tour_Id = "TOUR088",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 8, 11, 45, 0)
                },
                new Review
                {
                    Id = "REV438",
                    Tour_Id = "TOUR088",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 30, 16, 15, 0)
                },
                new Review
                {
                    Id = "REV439",
                    Tour_Id = "TOUR088",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 9, 2, 9, 40, 0)
                },
                new Review
                {
                    Id = "REV440",
                    Tour_Id = "TOUR088",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 6, 14, 0, 0)
                },
                new Review
                {
                    Id = "REV441",
                    Tour_Id = "TOUR089",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 4, 10, 5, 0)
                },
                new Review
                {
                    Id = "REV442",
                    Tour_Id = "TOUR089",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 23, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV443",
                    Tour_Id = "TOUR089",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được.",
                    Create_at = new DateTime(2024, 11, 5, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV444",
                    Tour_Id = "TOUR089",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 50, 0)
                },
                new Review
                {
                    Id = "REV445",
                    Tour_Id = "TOUR089",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 16, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV446",
                    Tour_Id = "TOUR090",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 5, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV447",
                    Tour_Id = "TOUR090",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 2, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV448",
                    Tour_Id = "TOUR090",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém. Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 25, 16, 40, 0)
                },
                new Review
                {
                    Id = "REV449",
                    Tour_Id = "TOUR090",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 27, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV450",
                    Tour_Id = "TOUR090",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 7, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV451",
                    Tour_Id = "TOUR091",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 3, 10, 10, 0)
                },
                new Review
                {
                    Id = "REV452",
                    Tour_Id = "TOUR091",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 22, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV453",
                    Tour_Id = "TOUR091",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, nhưng cần cải thiện lịch trình.",
                    Create_at = new DateTime(2024, 11, 4, 15, 35, 0)
                },
                new Review
                {
                    Id = "REV454",
                    Tour_Id = "TOUR091",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 30, 8, 45, 0)
                },
                new Review
                {
                    Id = "REV455",
                    Tour_Id = "TOUR091",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 19, 14, 20, 0)
                },
                new Review
                {
                    Id = "REV456",
                    Tour_Id = "TOUR092",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 8, 9, 25, 0)
                },
                new Review
                {
                    Id = "REV457",
                    Tour_Id = "TOUR092",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 5, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV458",
                    Tour_Id = "TOUR092",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém.",
                    Create_at = new DateTime(2024, 6, 28, 16, 30, 0)
                },
                new Review
                {
                    Id = "REV459",
                    Tour_Id = "TOUR092",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 30, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV460",
                    Tour_Id = "TOUR092",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 9, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV461",
                    Tour_Id = "TOUR093",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 5, 10, 0, 0)
                },
                new Review
                {
                    Id = "REV462",
                    Tour_Id = "TOUR093",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 24, 10, 55, 0)
                },
                new Review
                {
                    Id = "REV463",
                    Tour_Id = "TOUR093",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, không có gì nổi bật.",
                    Create_at = new DateTime(2024, 11, 7, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV464",
                    Tour_Id = "TOUR093",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 40, 0)
                },
                new Review
                {
                    Id = "REV465",
                    Tour_Id = "TOUR093",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 12, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV466",
                    Tour_Id = "TOUR094",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 12, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV467",
                    Tour_Id = "TOUR094",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 8, 11, 45, 0)
                },
                new Review
                {
                    Id = "REV468",
                    Tour_Id = "TOUR094",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 30, 16, 15, 0)
                },
                new Review
                {
                    Id = "REV469",
                    Tour_Id = "TOUR094",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 9, 2, 9, 40, 0)
                },
                new Review
                {
                    Id = "REV470",
                    Tour_Id = "TOUR094",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 6, 14, 0, 0)
                },
                new Review
                {
                    Id = "REV471",
                    Tour_Id = "TOUR095",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 4, 10, 5, 0)
                },
                new Review
                {
                    Id = "REV472",
                    Tour_Id = "TOUR095",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 23, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV473",
                    Tour_Id = "TOUR095",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được.",
                    Create_at = new DateTime(2024, 11, 5, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV474",
                    Tour_Id = "TOUR095",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 50, 0)
                },
                new Review
                {
                    Id = "REV475",
                    Tour_Id = "TOUR095",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 16, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV476",
                    Tour_Id = "TOUR096",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 5, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV477",
                    Tour_Id = "TOUR096",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 2, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV478",
                    Tour_Id = "TOUR096",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém. Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 25, 16, 40, 0)
                },
                new Review
                {
                    Id = "REV479",
                    Tour_Id = "TOUR096",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 27, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV480",
                    Tour_Id = "TOUR096",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 7, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV481",
                    Tour_Id = "TOUR097",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 3, 10, 10, 0)
                },
                new Review
                {
                    Id = "REV482",
                    Tour_Id = "TOUR097",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 22, 11, 0, 0)
                },
                new Review
                {
                    Id = "REV483",
                    Tour_Id = "TOUR097",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, nhưng cần cải thiện lịch trình.",
                    Create_at = new DateTime(2024, 11, 4, 15, 35, 0)
                },
                new Review
                {
                    Id = "REV484",
                    Tour_Id = "TOUR097",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 30, 8, 45, 0)
                },
                new Review
                {
                    Id = "REV485",
                    Tour_Id = "TOUR097",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 19, 14, 20, 0)
                },
                new Review
                {
                    Id = "REV486",
                    Tour_Id = "TOUR098",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 8, 9, 25, 0)
                },
                new Review
                {
                    Id = "REV487",
                    Tour_Id = "TOUR098",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 5, 11, 50, 0)
                },
                new Review
                {
                    Id = "REV488",
                    Tour_Id = "TOUR098",
                    Customer_Id = "CUS001",
                    Rating = 2,
                    Comment = @"Dịch vụ rất kém.",
                    Create_at = new DateTime(2024, 6, 28, 16, 30, 0)
                },
                new Review
                {
                    Id = "REV489",
                    Tour_Id = "TOUR098",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 8, 30, 9, 50, 0)
                },
                new Review
                {
                    Id = "REV490",
                    Tour_Id = "TOUR098",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 9, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV491",
                    Tour_Id = "TOUR099",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên có kiến thức sâu rộng. Tuyệt vời cho gia đình có trẻ nhỏ.",
                    Create_at = new DateTime(2024, 7, 5, 10, 0, 0)
                },
                new Review
                {
                    Id = "REV492",
                    Tour_Id = "TOUR099",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"An toàn và vui vẻ suốt chuyến đi. Bãi biển sạch và đẹp.",
                    Create_at = new DateTime(2025, 3, 24, 10, 55, 0)
                },
                new Review
                {
                    Id = "REV493",
                    Tour_Id = "TOUR099",
                    Customer_Id = "CUS001",
                    Rating = 3,
                    Comment = @"Tour tạm được, không có gì nổi bật.",
                    Create_at = new DateTime(2024, 11, 7, 15, 30, 0)
                },
                new Review
                {
                    Id = "REV494",
                    Tour_Id = "TOUR099",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hoạt động rất cuốn hút. Rất hài lòng với chuyến đi này.",
                    Create_at = new DateTime(2025, 1, 31, 8, 40, 0)
                },
                new Review
                {
                    Id = "REV495",
                    Tour_Id = "TOUR099",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Trải nghiệm tuyệt vời, rất đáng tiền! Cảnh đẹp như mơ.",
                    Create_at = new DateTime(2024, 5, 12, 14, 10, 0)
                },
                new Review
                {
                    Id = "REV496",
                    Tour_Id = "TOUR100",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Hướng dẫn viên nhiệt tình. Khách sạn sạch sẽ, thoải mái.",
                    Create_at = new DateTime(2024, 10, 12, 9, 20, 0)
                },
                new Review
                {
                    Id = "REV497",
                    Tour_Id = "TOUR100",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Lịch trình hợp lý. Đồ ăn ngon.",
                    Create_at = new DateTime(2025, 3, 8, 11, 45, 0)
                },
                new Review
                {
                    Id = "REV498",
                    Tour_Id = "TOUR100",
                    Customer_Id = "CUS001",
                    Rating = 1,
                    Comment = @"Giá quá đắt so với chất lượng.",
                    Create_at = new DateTime(2024, 6, 30, 16, 15, 0)
                },
                new Review
                {
                    Id = "REV499",
                    Tour_Id = "TOUR100",
                    Customer_Id = "CUS001",
                    Rating = 5,
                    Comment = @"Sẽ giới thiệu cho bạn bè. Rất chuyên nghiệp từ khâu đặt tour đến kết thúc.",
                    Create_at = new DateTime(2024, 9, 2, 9, 40, 0)
                },
                new Review
                {
                    Id = "REV500",
                    Tour_Id = "TOUR100",
                    Customer_Id = "CUS001",
                    Rating = 4,
                    Comment = @"Mọi thứ đều vượt ngoài mong đợi. Một kỳ nghỉ thư giãn và đáng nhớ.",
                    Create_at = new DateTime(2025, 5, 6, 14, 0, 0)
                }
            );
        }
    }
}
