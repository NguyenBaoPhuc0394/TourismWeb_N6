using AutoMapper;
using TourismWeb.DTOs.Account;
using TourismWeb.DTOs.Booking;
using TourismWeb.DTOs.Category;
using TourismWeb.DTOs.Customer;
using TourismWeb.DTOs.Image;
using TourismWeb.DTOs.Location;
using TourismWeb.DTOs.Payment;
using TourismWeb.DTOs.Review;
using TourismWeb.DTOs.Schedule;
using TourismWeb.DTOs.Tours;
using TourismWeb.Models;

namespace TourismWeb.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Account
            CreateMap<AccountDTO, Account>()
                .ReverseMap();

            CreateMap<AccountRegisterDTO, Account>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OTP, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<AccountLoginDTO, Account>()
                .ReverseMap();

            CreateMap<AccountConfirmDTO, Account>()
                .ReverseMap();

            CreateMap<AccountResetpassDTO, Account>()
                .ReverseMap();

            // Category
            CreateMap<CategoryCreateDTO, Category>()
                .ReverseMap();

            CreateMap<CategoryDTO, Category>()
                .ReverseMap();

            // Tour
            CreateMap<TourCreateDTO, Tour>()
                .ReverseMap();

            CreateMap<TourDTO, Tour>()
                .ReverseMap();

            // Location
            CreateMap<LocationCreateDTO, Location>()
                .ReverseMap();

            CreateMap<LocationDTO, Location>()
                .ReverseMap();


            // Customer
            CreateMap<CustomerDTO, Customer>()
                .ReverseMap();

            CreateMap<CustomerCreateDTO, Customer>()
                .ReverseMap();

            CreateMap<CustomerUpdateDTO, Customer>()
                .ReverseMap();

            // Image
            CreateMap<ImageDTO, Image>()
                .ReverseMap();

            // Schedule
            CreateMap<ScheduleCreateDTO, Schedule>()
                .ReverseMap();

            CreateMap<ScheduleDTO, Schedule>()
                .ReverseMap();

            CreateMap<ScheduleCalendarDTO, Schedule>()
                .ReverseMap();

            // Booking
            CreateMap<BookingCreateDTO, Booking>()
                .ReverseMap();

            CreateMap<BookingDTO, Booking>()
                .ReverseMap();

            //Review
            CreateMap<ReviewTourDTO, Review>()
                .ReverseMap();

            CreateMap<ReviewDTO, Review>()
                .ReverseMap();

            // Payment
            CreateMap<PaymentCreateDTO, Payment>()
                .ReverseMap();

            CreateMap<PaymentDTO, Payment>()
                .ReverseMap();
        }   
    }
}
