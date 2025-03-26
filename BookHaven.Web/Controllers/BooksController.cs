using BookHaven.Domain.Common;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace BookHaven.Web.Controllers
{
	[Authorize(Roles = AppRoles.Achieve)]
	public class BooksController : Controller
	{
		private readonly IUnitOfWord _unitOfWord;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly List<string> ImgMaxAllowdExtension = new() { ".jpg", ".jpeg", ".png" };
		private readonly int ImgMaxAllowdSize = 2097152; //2MB
		private readonly Cloudinary _cloudinary;    //To Access services cloudinary                         
													//IOptions<> To read data that binding from section to class
		public BooksController(IMapper mapper, IUnitOfWord unitOfWord, IWebHostEnvironment webHostEnvironment, IOptions<CloudinarySettings> cloudinarySettings)
		{
			_mapper = mapper;
			_unitOfWord = unitOfWord;
			_webHostEnvironment = webHostEnvironment;
			Account account = new Account()
			{
				Cloud = cloudinarySettings.Value.CloudName,
				ApiKey = cloudinarySettings.Value.APIKey,
				ApiSecret = cloudinarySettings.Value.APISecrect
			};
			_cloudinary = new Cloudinary(account);  //pass account to cloudinary
		}
		public IActionResult Index()
		{
			var books = _unitOfWord.Books.GetAll()
		   .Include(b => b.Author)
		   .Include(b => b.Categories)
		   .ThenInclude(b => b.Category).ToList();//immediate execution
			var bookVMs = _mapper.Map<IEnumerable<BookVM>>(books);
			return View(bookVMs);

		}

		public IActionResult Details(int id)
		{
			var book = _unitOfWord.Books.GetAll()
				.Include(b => b.Author)
				.Include(b => b.CopiesBook)
				.Include(b => b.Categories)
				.ThenInclude(b => b.Category)
				.SingleOrDefault(b => b.Id == id);
			//bookAuthor = _unitOfWord.Categories.FindAllMatch(a => a.Id == bookAuthor.);
			if (book is null) return NotFound();
			var bookVm = _mapper.Map<BookVM>(book);
			return View(bookVm);
		}
		[HttpGet]
		public IActionResult Create()
		{
			SelectedList();
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(BookFormVM model)//(Image) the same name in the name="" in input image
		{
			if (!ModelState.IsValid)
			{
				SelectedList();
				return View();
			}
			var BookIsExitsted = _unitOfWord.Books.FindMatch(ab => ab.Title == model.Title);
			var AuthorIsExitsted = _unitOfWord.Books.FindMatch(ab => ab.AuthorId == model.AuthorId);
			if (BookIsExitsted != null && AuthorIsExitsted != null)
			{
				TempData["ErrorMessage"] = "The book with the same Author that you try to add existed!";   //SweetAlert
				SelectedList();
				return View();
			}
			string imgPublicId = null;
			if (model.FormImg != null)
			{
				// Logo2.png
				var Extension = Path.GetExtension(model.FormImg.FileName); //.jpg, .jpeg, .png
				if (!ImgMaxAllowdExtension.Contains(Extension))
				{
					ModelState.AddModelError(nameof(model.Img), "Allowed only image with extension .jpg, .jpeg, .png");
					SelectedList();
					return View();
				}
				if (model.FormImg.Length > ImgMaxAllowdSize)
				{
					ModelState.AddModelError(nameof(model.Img), "Allowed only image with size 2:MB");
					SelectedList();
					return View();
				}

				string RootPath = _webHostEnvironment.WebRootPath; //...wwwroot
				var ImageName = $"{Guid.NewGuid()}{Extension}";  //[random name] /3456sd23rf.png(generate GUID To be uninq in db) 
				string ImgPath = Path.Combine($"{RootPath}/Images/Book", ImageName); // الباث كله-----> wwwroot\Images\Book\rt4wfj.png
				using var stream = System.IO.File.Create(ImgPath);//حولي الباث ده لبيتس علشان اعرف استقبل فيه صوره 
				await model.FormImg.CopyToAsync(stream);// (هنا بكلم الاوبريتنج سيستم يبقا يفضل Async)// إستقبل فيه الصورة
				model.Img = ImageName;// لازم تاخد قيمه في الداتابيز فهنديها اسمها 

				//Start Save To Cloudinary
				/*var stream = Image.OpenReadStream(); //make stream for image to pass it to cloudinary
				var ImageParams = new ImageUploadParams() // هتحط هنا الفايل اللي عايز تبعته بالخصائص بتعته زي طوله عرضه حجمه إلخ
				{
					File = new FileDescription(ImageName, stream)
				};
				var UrlParams = await _cloudinary.UploadAsync(ImageParams); //upload url for params to cloudinary
				model.Img = UrlParams.SecureUrl.ToString();  // set url from cloudinary to database
				imgPublicId = UrlParams.PublicId;*/
				//End Save To Cloudinary
			}
			//model.ImgPublicId = imgPublicId; //save publicId for delete
			var book = _mapper.Map<Book>(model);//her categories (obj from BookCategory) is null because diff data type
			/*foreach (var category in model.CategoryId)
			{
				_unitOfWord.BookCategories.Create(new BookCategory { BookId = book.Id, CategoryId = category });
				_unitOfWord.Commit();
			}*/
			//manually mapping because diff data type 
			foreach (var item in model.CategoryId)
			{
				book.Categories.Add(new BookCategory { CategoryId = item });
			}
			_unitOfWord.Books.Create(book);
			_unitOfWord.Commit();
			TempData["SuccessMessage"] = "Saved successfully";
			return RedirectToAction(nameof(Details), new { id = book.Id });
		}
		[HttpGet]
		public IActionResult Edite(int id)
		{
			Book book = _unitOfWord.Books.GetAll()
				.Include(b => b.Author)
				.Include(b => b.Categories)
				.ThenInclude(b => b.Category)
				.SingleOrDefault(b => b.Id == id);
			//var resss = book.Categories.Select(c => c.BookId == id);
			if (book is null) return NotFound();
			var bookFormVM = _mapper.Map<BookFormVM>(book);
			// not return selected category i don't how pro
			foreach (var item in book.Categories)
			{
				bookFormVM.CategoryId.Add(item.CategoryId);
			}
			SelectedList();
			return View(nameof(Create), bookFormVM);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edite(BookFormVM model)//Don't forget ===> ? nullable
		{
			//Tracking error
			var book = _unitOfWord.Books.GetAll()
				.Include(b => b.Categories)
				.ThenInclude(c => c.Category)
				.SingleOrDefault(b => b.Id == model.Id);
			if (book == null) return NotFound();
			string imgPublicId = null;
			if (ModelState.IsValid)
			{
				if (model.FormImg != null)
				{
					string RootPath = _webHostEnvironment.WebRootPath; //wwwroot
																	   // Delete Old image
					if (model.Img != null)
					{
						//هل موجوده علي السيرفر لأنها ممكن تتحزف بالغلط تيجي تحزفها تاني تتفقع واحده نل
						var OldImage = Path.Combine($"{RootPath}/Images/Book", model.Img);
						if (System.IO.File.Exists(OldImage)) System.IO.File.Delete(OldImage);
						//Delet Img from cloudinary
						//_cloudinary.DeleteResourcesAsync(book.ImgPublicId);
					}
					//Start save image
					var Extension = Path.GetExtension(model.FormImg.FileName); //.jpg, .jpeg, .png
					if (!ImgMaxAllowdExtension.Contains(Extension))
					{
						ModelState.AddModelError(nameof(model.Img), "Allowed only image with extension .jpg, .jpeg, .png");
						SelectedList();
						return View(nameof(Create), model);
					}
					if (model.FormImg.Length > ImgMaxAllowdSize)
					{
						ModelState.AddModelError(nameof(model.Img), "Allowed only image with size 2:MB");
						SelectedList();
						return View(nameof(Create), model);
					}


					var ImageName = $"{Guid.NewGuid()}{Extension}";  // /3456sd23rf.png
					string ImgPath = Path.Combine($"{RootPath}/Images/Book", ImageName); // الباث كله-----> wwwroot\Images\Book\...png
					using var stream = System.IO.File.Create(ImgPath);
					await model.FormImg.CopyToAsync(stream);  // خزن في الداتابيز
					model.Img = ImageName;// لازم تاخد قيمه في الداتابيز فهنديها اسمها 
										  //End save image

					//Start Save Img To Cloudinary
					/*var stream = Image.OpenReadStream();
					var ImageParams = new ImageUploadParams()
					{
						File = new FileDescription(ImageName, stream)
					};
					var UrlParams = await _cloudinary.UploadAsync(ImageParams);
					model.Img = UrlParams.SecureUrl.ToString();
					imgPublicId = UrlParams.PublicId;*/
					//End Save Img To Cloudinary	
				}
			}
			else
			{
				SelectedList();
				return View(nameof(Create), model);
			}
			//if copy avaialble for rental it's copies  avaialble for rental too//if copy not avaialble for rental it's copies not avaialble for rental too
			if (book.IsAvailableForRental != model.IsAvailableForRental)
			{
				var BookCopy = _unitOfWord.CopiesBook.FindAllMatch(c => c.BookId == book.Id);
				foreach (var copy in BookCopy)
				{
					copy.IsAvailableForRental = !copy.IsAvailableForRental;
					_unitOfWord.CopiesBook.Update(copy);
					_unitOfWord.Commit();
				}

			}
			var DbCategories = book.Categories;
			book = _mapper.Map<Book>(model);
			book.Categories = DbCategories;
			//Don't save new categories i don't know pro
			foreach (var item in model.CategoryId)
			{
				if (!book.Categories.Any(bc => bc.CategoryId == item))//To not dublicate 
				{
					book.Categories.Add(new BookCategory { CategoryId = item });
				}
			}
			/*foreach (var item in book.Categories)
			{
				if (!model.CategoryId.Contains(item.CategoryId))//To not dublicate 
				{
					book.Categories.Remove(new BookCategory { CategoryId=item.CategoryId});
				}
			}*/
			book.AuthorId = model.AuthorId;
			book.LastUpdatedOn = DateTime.Now;
			book.Id = model.Id;
			//save publicId for delete
			//if(model.FormImg != null)book.ImgPublicId = imgPublicId;
			_unitOfWord.Books.Update(book);
			_unitOfWord.Commit();
			TempData["SuccessMessage"] = "Edite successfully";
			return RedirectToAction(nameof(Details), new { id = book.Id });
		}
		public IActionResult ToggleStatus(int id)
		{
			var book = _unitOfWord.Books.GetById(id);
			if (book is null) return NotFound();
			book.IsDeleted = !book.IsDeleted;
			_unitOfWord.Books.Update(book);
			_unitOfWord.Commit();
			return Ok();
		}
		//Don't repeat your self
		private void SelectedList()
		{
			ViewBag.Authors = _unitOfWord.Authors.FindAllMatch(a => a.IsDeleted == false);
			ViewBag.Categories = _unitOfWord.Categories.FindAllMatch(c => c.IsDeleted == false);
		}

	}
}
