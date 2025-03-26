
using BookHaven.Web.ExtensionMethod;
using ClosedXML.Excel;
using OpenHtmlToPdf;
using System.Net.Mime;
using X.PagedList.Extensions;

namespace BookHaven.Web.Controllers
{
	public class ReportsController : Controller
	{
		private readonly IUnitOfWord _unitOfWord;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ReportsController(IUnitOfWord unitOfWord, IMapper mapper, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWord = unitOfWord;
			_mapper = mapper;
			_webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index()
		{
			return View();
		}
		#region Books
		public IActionResult FilterBooks(int? page)
		{
			var pageNumbr = page ?? 1; //if page==null
			var pageSize = 10;
			var Authors = _unitOfWord.Authors.GetAll();
			var Categories = _unitOfWord.Categories.GetAll();
			ViewBag.Authors = Authors;
			ViewBag.Categories = Categories;

			var Books = _unitOfWord.Books.GetAll()
			.Include(b => b.Author)
			.Include(b => b.Categories!)
			.ThenInclude(c => c.Category!);
			var viewModel = new ReportBooksVM()
			{
				Books = _mapper.Map<IEnumerable<Book>>(Books).ToPagedList(pageNumbr, pageSize)//cast
			};
			return View(viewModel);
		}
		[HttpPost]
		public IActionResult Books(int? page, ReportBooksVM viewModel)
		{
			IQueryable<Book> books = _unitOfWord.Books.GetAll()
				.Include(b => b.Author)
				.Include(b => b.Categories!)
				.ThenInclude(b => b.Category);

			if (viewModel.AuthorId.Count > 0)
				books = books.Where(b => viewModel.AuthorId.Contains(b.AuthorId));//assign

			if (viewModel.CategoryId.Count > 0)//M to M
				books = books.Where(b => b.Categories!.Any(c => viewModel.CategoryId.Contains(c.CategoryId)));//so beurifull fashkh tarbee
			var pageNumbr = page ?? 1; //if page==null
			var pageSize = 10;
			viewModel.Books = books.ToPagedList(pageNumbr, pageSize);

			var Authors = _unitOfWord.Authors.GetAll();
			var Categories = _unitOfWord.Categories.GetAll();
			ViewBag.Authors = Authors;
			ViewBag.Categories = Categories;

			return View("FilterBooks", viewModel);
		}
		public async Task<IActionResult> BookExcelReport(ReportBooksVM viewModel)
		{
			IEnumerable<Book> books = _unitOfWord.Books.GetAll()
				.Include(b => b.Author)
				.Include(b => b.Categories!)
				.ThenInclude(b => b.Category);
			//filter by auther
			if (viewModel.AuthorId != null && viewModel.AuthorId.Count > 0)
				books = books.Where(b => viewModel.AuthorId.Contains(b.AuthorId));//assign
																				  //filter by category
			if (viewModel.CategoryId != null && viewModel.CategoryId.Count > 0)//M to M
				books = books.Where(b => b.Categories!.Any(c => viewModel.CategoryId.Contains(c.CategoryId)));//so beurifull fashkh tarbee
																											  //Start Report
			using var Workbook = new XLWorkbook();//عملت ورك بوك
			var sheet = Workbook.AddWorksheet("Books");// ضفت فيه شيت وإدتله إسم;

			/*//Add Picture
			sheet.AddPicture($"{_webHostEnvironment.WebRootPath}/Mecatronic/Img/Logo.png")
				.MoveTo(sheet.Cell("A1"))
				.Scale(.2);//size*/
			//header
			string[] Header = { "Title", "Author", "Categories", "publisher", "publishing Date", "Hall", "Available for rental", "Status" };
			sheet.AddHeader(Header);//my Extension method
									//sheet.AddFormatHeader();//format header                        

			//body
			var row = 2;
			foreach (var item in books)
			{
				sheet.Cell(row, 1).SetValue(item.Title);
				sheet.Cell(row, 2).SetValue(item.Author!.Name);
				sheet.Cell(row, 3).SetValue(string.Join(", ", item.Categories!.Select(c => c.Category!.Name)));
				sheet.Cell(row, 4).SetValue(item.Publisher);
				sheet.Cell(row, 5).SetValue(item.PublishingDate.ToString("dd MMM yyyy"));
				sheet.Cell(row, 6).SetValue(item.Hall);
				sheet.Cell(row, 7).SetValue(item.IsAvailableForRental ? "Yes" : "NO");
				sheet.Cell(row, 8).SetValue(item.IsDeleted ? "Available" : "Deleted");
				row++;
			}
			//format body
			sheet.AddFormatBody();//My Extension method
			sheet.AddStyleTable(books.Count() + 1, 8);
			await using var stream = new MemoryStream();
			Workbook.SaveAs(stream);
			//لازم الإمتداد    //النوع ده شغال اكسل وبي دي اف
			return File(stream.ToArray(), MediaTypeNames.Application.Octet, "Book.xlsx");
			//End Report
		}
		public async Task<IActionResult> BookPDFReport(ReportBooksVM viewModel)
		{
			IEnumerable<Book> books = _unitOfWord.Books.GetAll()
				.Include(b => b.Author)
				.Include(b => b.Categories!)
				.ThenInclude(b => b.Category);

			if (viewModel.AuthorId != null && viewModel.AuthorId.Count > 0)
				books = books.Where(b => viewModel.AuthorId.Contains(b.AuthorId));//assign

			if (viewModel.CategoryId != null && viewModel.CategoryId.Count > 0)//M to M
				books = books.Where(b => b.Categories!.Any(c => viewModel.CategoryId.Contains(c.CategoryId)));//so beurifull fashkh tarbee

			//Start Report
			var html = await System.IO.File.ReadAllTextAsync($"{_webHostEnvironment.WebRootPath}/Templates/PDFBookReport.html");
			html = html.Replace("[Title]", "Books");

			//var body = "<table><thead><tr><th>Title</th><th>Author</th><th>Categories</th><th>Publisher</th><th>publishing Date</th><th>Hall</th> <th>Available for rental</th> <th>Status</th></tr></thead><tbody>";
			var body = "";
			foreach (var book in books)
			{
				body += $"<tr><td>{book.Title}</td><td>{book.Author.Name}</td><td>{string.Join(", ", book.Categories.Select(c => c.Category.Name))}</td><td>{book.Publisher}</td><td>{book.PublishingDate.ToString("dd MMM yyyy")}</td><td>{book.Hall}</td><td>{(!book.IsAvailableForRental ? "Yes" : "NO")}</td><td>{(!book.IsDeleted ? "Available" : "Deleted")}</td></tr>";
			}
			html = html.Replace("[body]", body);
			var pdf = Pdf.From(html)
				//.Landscape() الصفحه بالعرض
				.Content();
			return File(pdf.ToArray(), "application/octet-stream", "Book.pdf");
			//End Report
		}
		#endregion
		#region Rentals
		public IActionResult Rentals(int? page, ReportRentalsVM? viewModel)
		{
			if (viewModel.Range != null)
			{
				string s = viewModel.Range;
				string range1 = "";
				string range2 = "";
				int dash = s.IndexOf('-');
				for (int i = 0; i < s.Length; i++)
				{
					if (i < dash) range1 += s[i];
					else if (i > dash) range2 += s[i];
				}
				DateTime d1 = DateTime.Parse(range1);
				DateTime d2 = DateTime.Parse(range2);
				IQueryable<Rental> rentals = _unitOfWord.Rental.GetAll()
					.Include(r => r.Subscriper)
					.Where(r => r.ReturnDate >= d1 && r.ReturnDate <= d2);
				//TODO:Book Auther Name by Booktitle

				int pageNumber = page ?? 1;
				int pageSize = 10;
				viewModel.rentals = _mapper.Map<IEnumerable<Rental>>(rentals).ToPagedList(pageNumber, pageSize);
			}
			return View(viewModel);
		}
		public async Task<IActionResult> RentalExcelReport(ReportRentalsVM viewModel)
		{
			string s = viewModel.Range;
			string range1 = "";
			string range2 = "";
			int dash = s.IndexOf('-');
			for (int i = 0; i < s.Length; i++)
			{
				if (i < dash) range1 += s[i];
				else if (i > dash) range2 += s[i];
			}
			DateTime d1 = DateTime.Parse(range1);
			DateTime d2 = DateTime.Parse(range2);
			IQueryable<Rental> rentals = _unitOfWord.Rental.GetAll()
				.Include(r => r.Subscriper)
				.Where(r => r.ReturnDate >= d1 && r.ReturnDate <= d2);
			using var workbook = new XLWorkbook();
			var sheet = workbook.AddWorksheet("Rentals");
			string[] Header = { "Subscrier ID", "Subscrier Name", "Subscrier Phone", "Book Title", "Rental Date", "End Date", "Return Date" };
			sheet.AddHeader(Header);
			//sheet.AddFormatHeader();
			var row = 2;
			foreach (var rental in rentals)
			{
				sheet.Cell(row, 1).SetValue(rental.Subscriper.Id);
				sheet.Cell(row, 2).SetValue(rental.Subscriper.FName + " " + rental.Subscriper.FName);
				sheet.Cell(row, 3).SetValue(rental.Subscriper.MobileNumber);
				sheet.Cell(row, 4).SetValue(rental.BookTitle);
				sheet.Cell(row, 5).SetValue(rental.StartDate.ToString("dd MM yyy"));
				sheet.Cell(row, 6).SetValue(rental.EndDate.ToString("dd MM yyy"));
				sheet.Cell(row, 7).SetValue(rental.ReturnDate?.ToString("dd MMM yyyy"));
				row++;
			}
			sheet.AddFormatBody();
			sheet.AddStyleTable(rentals.Count() + 1, 7);
			await using var stream = new MemoryStream();
			workbook.SaveAs(stream);
			return File(stream.ToArray(), MediaTypeNames.Application.Octet, "Rentals.xlsx");
		}
		public async Task<IActionResult> RentalPDFReport(ReportRentalsVM viewModel)
		{
			string s = viewModel.Range;
			string range1 = "";
			string range2 = "";
			int dash = s.IndexOf('-');
			for (int i = 0; i < s.Length; i++)
			{
				if (i < dash) range1 += s[i];
				else if (i > dash) range2 += s[i];
			}
			DateTime d1 = DateTime.Parse(range1);
			DateTime d2 = DateTime.Parse(range2);
			IQueryable<Rental> rentals = _unitOfWord.Rental.GetAll()
				.Include(r => r.Subscriper)
				.Where(r => r.ReturnDate >= d1 && r.ReturnDate <= d2);
			var html = await System.IO.File.ReadAllTextAsync($"{_webHostEnvironment.WebRootPath}/Templates/PDFRentalReport.html");
			html = html.Replace("[Title]", "Rentals");
			var body = "";
			foreach (var rental in rentals)
			{
				body += $"<tr><td>{rental.Subscriper.Id}</td><td>{rental.Subscriper.FName} {rental.Subscriper.LName}</td><td>{rental.Subscriper.MobileNumber}</td><td>{rental.BookTitle}</td><td>{rental.StartDate.ToString("dd MMM yyyy")}</td><td>{rental.EndDate.ToString("dd MMM yyyy")}</td><td>{rental.ReturnDate?.ToString("dd MMM yyyy")}</td></tr>";

			}
			html = html.Replace("[body]", body);
			var pdf = Pdf.From(html).Content();

			return File(pdf.ToArray(), "application/octet-stream", "Rentals.pdf");
		}
		#endregion
		#region Delay
		public IActionResult DelayInRentals(ReportRentalsVM? viewModel)
		{
			var rentals = _unitOfWord.Rental.GetAll()
				.Include(r => r.Subscriper)
				.Where(r => !r.ReturnDate.HasValue && r.EndDate < DateTime.Today)
				.ToList();

			viewModel.rentals = _mapper.Map<IEnumerable<Rental>>(rentals);
			foreach (var rental in viewModel.rentals)
			{
				viewModel.Delay.Add((int)(DateTime.Today - rental.EndDate).TotalDays);

			}
			return View(viewModel);
		}
		public async Task<IActionResult> DelayExcelReport(ReportRentalsVM viewModel)
		{
			var rentals = _unitOfWord.Rental.GetAll()
				.Include(r => r.Subscriper)
				.Where(r => !r.ReturnDate.HasValue && r.EndDate < DateTime.Today)
				.ToList();

			viewModel.rentals = _mapper.Map<IEnumerable<Rental>>(rentals);
			foreach (var rental in viewModel.rentals)
			{
				viewModel.Delay.Add((int)(DateTime.Today - rental.EndDate).TotalDays);

			}
			using var workbook = new XLWorkbook();
			var sheet = workbook.AddWorksheet("Rentals");
			string[] Header = { "Subscrier ID", "Subscrier Name", "Subscrier Phone", "Book Title", "Rental Date", "End Date", "Delay In Rentals" };
			sheet.AddHeader(Header);
			//sheet.AddFormatHeader();
			var row = 2;
			var Idx = 0;
			foreach (var rental in rentals)
			{
				sheet.Cell(row, 1).SetValue(rental.Subscriper.Id);
				sheet.Cell(row, 2).SetValue(rental.Subscriper.FName + " " + rental.Subscriper.FName);
				sheet.Cell(row, 3).SetValue(rental.Subscriper.MobileNumber);
				sheet.Cell(row, 4).SetValue(rental.BookTitle);
				sheet.Cell(row, 5).SetValue(rental.StartDate.ToString("dd MM yyy"));
				sheet.Cell(row, 6).SetValue(rental.EndDate.ToString("dd MM yyy"));
				sheet.Cell(row, 7).SetValue(viewModel.Delay[Idx++]);
				row++;
			}
			sheet.AddFormatBody();
			sheet.AddStyleTable(rentals.Count() + 1, 7);
			await using var stream = new MemoryStream();
			workbook.SaveAs(stream);
			return File(stream.ToArray(), MediaTypeNames.Application.Octet, "Rentals.xlsx");
		}
		public async Task<IActionResult> DelayPDFReport(ReportRentalsVM viewModel)
		{
			var rentals = _unitOfWord.Rental.GetAll()
				.Include(r => r.Subscriper)
				.Where(r => !r.ReturnDate.HasValue && r.EndDate < DateTime.Today)
				.ToList();

			viewModel.rentals = _mapper.Map<IEnumerable<Rental>>(rentals);
			foreach (var rental in viewModel.rentals)
			{
				viewModel.Delay.Add((int)(DateTime.Today - rental.EndDate).TotalDays);

			}
			var Idx = 0;
			var html = await System.IO.File.ReadAllTextAsync($"{_webHostEnvironment.WebRootPath}/Templates/PDFDelayReport.html");
			html = html.Replace("[Title]", "Rentals");
			var body = "";
			foreach (var rental in rentals)
			{
				body += $"<tr><td>{rental.Subscriper.Id}</td><td>{rental.Subscriper.FName} {rental.Subscriper.LName}</td><td>{rental.Subscriper.MobileNumber}</td><td>{rental.BookTitle}</td><td>{rental.StartDate.ToString("dd MMM yyyy")}</td><td>{rental.EndDate.ToString("dd MMM yyyy")}</td><td>{viewModel.Delay[Idx++]}</td></tr>";

			}
			html = html.Replace("[body]", body);
			var pdf = Pdf.From(html).Content();

			return File(pdf.ToArray(), "application/octet-stream", "Rentals.pdf");
		}
		#endregion
	}
}
