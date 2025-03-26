using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookHaven.Web.Core.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Category, CategoryVM>(); //Source , Destination
			CreateMap<CategoryFormVM, Category>().ReverseMap(); //(Source , Destination)   and   (Destination   Source)
																//Authors
			CreateMap<Author, AuthorVM>();
			CreateMap<AuthorFormVM, Author>().ReverseMap();
			CreateMap<Author, SelectListItem>()
				.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

			//Books
			CreateMap<BookFormVM, Book>()
				.ReverseMap()
				.ForMember(dest => dest.Category, opt => opt.Ignore());

			CreateMap<Book, BookVM>()
				.ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author!.Name))
				.ForMember(dest => dest.Categories,
					opt => opt.MapFrom(src => src.Categories.Select(c => c.Category!.Name).ToList()));

			CreateMap<CopyBook, CopyBookVM>()
				.ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.book!.Title));
			CreateMap<CopyBook, CopyBookFormVM>()

				.ForMember(dest => dest.Edition, opt => opt.MapFrom(src => src.EditionNumber));
			CreateMap<CopyBookFormVM, CopyBook>()
				.ForMember(dest => dest.EditionNumber, opt => opt.MapFrom(src => src.Edition));
			//User
			CreateMap<ApplicationUser, UserVM>()
				.ForMember(dest => dest.LockedOutEnd, opt => opt.MapFrom(src => src.LockoutEnd));
			CreateMap<ApplicationUser, UserFormVM>();
			CreateMap<UserFormVM, ApplicationUser>();

			//Subscriper
			CreateMap<SubscriperFormVM, Subscriper>().ReverseMap();
			CreateMap<Subscriper, SubscriperVM>()
				.ForMember(des => des.FullName, opt => opt.MapFrom(src => $"{src.FName} {src.LName}"))
				.ForMember(des => des.Area, opt => opt.MapFrom(src => src.Area.Name))
				.ForMember(des => des.Governorate, opt => opt.MapFrom(src => src.Governorate.Name));

			CreateMap<Subcribtion, SubscribtionVM>();
			//Rental
			CreateMap<Rental, RentalVM>();
			CreateMap<Book, RentalFormVM>();
			CreateMap<Book, RentalCopyBookVM>();

			//Report
			CreateMap<Book, ReportBooksVM>();
			//.ForMember(des => des.CategoryId, opt => opt.MapFrom(src => src.Categories!.Select(c => c.Category!.Id).ToList()));
			CreateMap<Rental, ReportRentalsVM>();





		}
	}
}
