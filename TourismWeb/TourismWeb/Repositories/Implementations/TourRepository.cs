using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TourismWeb.Data;
using TourismWeb.DTOs.Tour;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;
using Dapper;


namespace TourismWeb.Repositories.Implementations
{
    public class TourRepository : ITourRepository
    {
        private readonly TourismDbContext _context;
        private readonly IMapper _mapper;

        public TourRepository(TourismDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Tour> CreateTour(Tour tour)
        {
            var result = _context.Tours.FromSqlRaw("Exec CreateTour @Name = {0}, @ShortDescription = {1}, @DetailDescription = {2}, @ScheduleDescription = {3}, @CategoryId = {4}, @Duration = {5}, @Price = {6}, @MaxCapacity = {7}",
                tour.Name, tour.Short_description, tour.Detail_description, tour.Schedule_description, tour.Category_Id, tour.Duration, tour.Price, tour.Max_capacity).AsEnumerable().FirstOrDefault();

            if (result == null)
            {
                throw new Exception("Tour not found!");
            }
            return result;
        }

        public async Task DeleteTourById(string id)
        {
            var account = await _context.Tours.FindAsync(id);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found!");
            }
            _context.Tours.Remove(account);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tour>> GetAllTours()
        {
            return await _context.Tours.ToListAsync();
        }

        //public Task<IEnumerable<Tour>> GetDiscountedTours()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Tour> GetTourById(string id)
        {
            var result = await _context.Tours.FindAsync(id);
            if (result == null)
            {
                throw new KeyNotFoundException("Tour not found!");
            }
            return result;
        }

        public async Task<IEnumerable<Tour>> GetToursByCategory(string categoryId)
        {
            var result = await _context.Tours
                .FromSqlRaw("Select * from Tour where @CategoryId = {0}", categoryId)
                .ToListAsync();
            if (result == null)
            {
                throw new KeyNotFoundException("Tours not found!");
            }
            return result;
        }

        public async Task<Tour> UpdateTourById(string id, Tour tourUpdate)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                throw new KeyNotFoundException("Tour not found!");
            }
            tour.Name = tourUpdate.Name;
            tour.Location = tourUpdate.Location;
            tour.Short_description = tourUpdate.Short_description;
            tour.Detail_description = tourUpdate.Detail_description;
            tour.Schedule_description = tourUpdate.Schedule_description;
            tour.Category_Id = tourUpdate.Category_Id;
            tour.Duration = tourUpdate.Duration;
            tour.Price = tourUpdate.Price;
            tour.Max_capacity = tourUpdate.Max_capacity;
            tour.Create_at = tourUpdate.Create_at;
            tour.Update_at = tourUpdate.Update_at;

            await _context.SaveChangesAsync();
            return tour;
        }

        public async Task<IEnumerable<TourCardDTO>> GetAllPromotionTours()
        {
            var sql = @"
            SELECT 
                t.Id AS Id,
                t.Name AS Name,
                t.Short_description AS ShortDescription,
                t.Price AS TourPrice,
                MIN(s.Adult_price) AS LowestAdultPrice,
                i.Url AS ImageUrl,
                COALESCE(AVG(CAST(r.Rating AS FLOAT)), 0) AS AverageRating
            FROM 
                Tours t
            LEFT JOIN 
                Schedules s ON t.Id = s.Tour_Id
            LEFT JOIN 
                Reviews r ON t.Id = r.Tour_Id
            OUTER APPLY (
                SELECT TOP 1 Url
                FROM Images img
                WHERE img.Tour_Id = t.Id
                ORDER BY img.Id
            ) i
            GROUP BY 
                t.Id, t.Name, t.Short_description, t.Price, i.Url
            HAVING 
                MIN(s.Adult_price) IS NOT NULL AND MIN(s.Adult_price) != t.Price
            ORDER BY 
                t.Id;";

            using (var connection = _context.Database.GetDbConnection())
            {
                return (await connection.QueryAsync<TourCardDTO>(sql)).ToList();
            }
        }

        public async Task<IEnumerable<TourCardDTO>> SearchTours(TourSearchDTO searchData)
        {
            var query = @"
                    SELECT 
                        t.Id AS Id,
                        t.Name AS Name,
                        t.Short_description AS ShortDescription,
                        t.Price AS TourPrice,
                        MIN(s.Adult_price) AS LowestAdultPrice,
                        i.Url AS ImageUrl,
                        COALESCE(AVG(CAST(r.Rating AS FLOAT)), 0) AS AverageRating
                    FROM 
                        Tours t
                    INNER JOIN 
                        Categories c ON t.Category_Id = c.Id
                    INNER JOIN 
                        Locations l ON t.Location_Id = l.Id
                    LEFT JOIN 
                        Schedules s ON t.Id = s.Tour_Id
                    LEFT JOIN 
                        Reviews r ON t.Id = r.Tour_Id
                    OUTER APPLY (
                        SELECT TOP 1 Url
                        FROM Images img
                        WHERE img.Tour_Id = t.Id
                        ORDER BY img.Id
                    ) i
                    WHERE 
                        (@CategoryName IS NULL OR c.Name = @CategoryName) AND
                        (@MinPrice IS NULL OR s.Adult_price >= @MinPrice) AND
                        (@MaxPrice IS NULL OR s.Adult_price <= @MaxPrice) AND
                        (@LocationName IS NULL OR l.Name = @LocationName) AND
                        (@DepartureTime = '1990-01-01' 
                        OR EXISTS (
                            SELECT 1 
                            FROM Schedules s2 
                            WHERE s2.Tour_Id = t.Id 
                            AND CAST(s2.Start_date AS DATE) = CAST(@DepartureTime AS DATE)
                        )
                    )
                    GROUP BY 
                        t.Id, t.Name, t.Short_description, t.Price, i.Url
                    HAVING 
                        MIN(s.Adult_price) IS NOT NULL
                    ORDER BY 
                        t.Id;
                ";
            using (var connection = _context.Database.GetDbConnection())
            {
                var parameters = new
                {
                    CategoryName = string.IsNullOrWhiteSpace(searchData.category) ? null : searchData.category,
                    LocationName = string.IsNullOrWhiteSpace(searchData.departureLocation) ? null : searchData.departureLocation,
                    MinPrice = searchData.minPrice ?? 0,
                    MaxPrice = searchData.maxPrice ?? int.MaxValue,
                    DepartureTime = searchData.departureTime ?? new DateTime(1990, 1, 1)
                };
                return (await connection.QueryAsync<TourCardDTO>(query, parameters)).ToList();
            }
        }

    }
}
