using Hangfire;
using Microsoft.AspNetCore.DataProtection;
using WhatsAppCloudApi;
using WhatsAppCloudApi.Services;

namespace BookHaven.Web.Controllers
{
	public class SubscripersController : Controller
	{
		private readonly IUnitOfWord _unitOfWord;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IDataProtector _dataProtector;
		private readonly IWhatsAppClient _whatsAppClient;
		private readonly IEmailSender _emailSender;
		private readonly int ImgMaxAllowdSize = 2097152; //2MB
		private readonly List<string> ImgMaxAllowdExtension = new() { ".jpg", ".jpeg", ".png" };
		public SubscripersController(IUnitOfWord unitOfWord, IMapper mapper, IWebHostEnvironment webHostEnvironment, IDataProtectionProvider dataProtector, IWhatsAppClient whatsAppClient, IEmailSender emailSender)
		{
			_unitOfWord = unitOfWord;
			_mapper = mapper;
			_webHostEnvironment = webHostEnvironment;
			_dataProtector = dataProtector.CreateProtector("MySecurKey");
			_whatsAppClient = whatsAppClient;
			_emailSender = emailSender;
		}

		public async Task<IActionResult> Index()
		{
			return View();
		}
		[HttpGet]
		public IActionResult Create()
		{
			SelectedList();
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(SubscriperFormVM subscriperFormVM)
		{
			if (!ModelState.IsValid)
			{
				SelectedList();
				return View();
			}
			var Email = _unitOfWord.Subscriper.FindMatch(s => s.Email == subscriperFormVM.Email);
			var NaltionalId = _unitOfWord.Subscriper.FindMatch(s => s.NationalId == subscriperFormVM.NationalId);
			var MobilNumber = _unitOfWord.Subscriper.FindMatch(s => s.MobileNumber == subscriperFormVM.MobileNumber);
			if (Email != null)
			{
				TempData["ErrorMessage"] = "The subscriber's Email that you try to add existed!";
				SelectedList();
				return View();
			}
			if (NaltionalId != null)
			{
				TempData["ErrorMessage"] = "The subscriber's National Id that you try to add existed!";
				SelectedList();
				return View();
			}
			if (MobilNumber != null)
			{
				TempData["ErrorMessage"] = "The subscriber's MobilNumber that you try to add existed!";
				SelectedList();
				return View();
			}
			if (subscriperFormVM.Img != null)
			{
				var extensions = Path.GetExtension(subscriperFormVM.Img.FileName);
				if (!ImgMaxAllowdExtension.Contains(extensions))
				{
					ModelState.AddModelError(nameof(subscriperFormVM.Img), "Allowed only image with extension .jpg, .jpeg, .png");
					SelectedList();
					return View();
				}
				if (subscriperFormVM.Img.Length > ImgMaxAllowdSize)
				{
					ModelState.AddModelError(nameof(subscriperFormVM.Img), "Allowed only image with size 2:MB");
					SelectedList();
					return View();
				}

				var RootPath = _webHostEnvironment.WebRootPath;
				var ImageName = $"{Guid.NewGuid()}{extensions}";
				var ImagePath = Path.Combine($"{RootPath}/Images/Subscriper", ImageName);
				using var stream = System.IO.File.Create(ImagePath);
				await subscriperFormVM.Img.CopyToAsync(stream);
				subscriperFormVM.ImgUrl = ImageName;
			}

			var subscriper = _mapper.Map<Subscriper>(subscriperFormVM);
			subscriper.subcribtions.Add(
				new Subcribtion
				{
					StartDate = DateTime.Today,
					EndDate = DateTime.Today.AddYears(1),
					CreatedOn = DateTime.Today
				}
				);
			_unitOfWord.Subscriper.Create(subscriper);
			_unitOfWord.Commit();
			TempData["SuccessMessage"] = "Saved successfully";
			//Start Email Sender
			var TempPath = $"{_webHostEnvironment.WebRootPath}/Templates/{EmailTemplates.Notification}.html";
			StreamReader streamReader = new StreamReader(TempPath);
			var body = streamReader.ReadToEnd();
			streamReader.Close();
			body = body
				.Replace("[ImgUrl]", "https://res.cloudinary.com/moshawky/image/upload/v1736097419/positive-vote_1533908_uyd3zi.png")
					.Replace("[Header]", $"Welcom {subscriper.LName}")
					.Replace("[Body]", "thanks for joining us 🥰");
			BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(subscriper.Email, "Welcome To BooKHaven", body));
			//End Email Sender

			//Start WhatsApp Integration
			if (subscriper.HasWatsApp)
			{
				List<WhatsAppComponent> components = new List<WhatsAppComponent>()//initialize parameter in my own template
				{
					new WhatsAppComponent()
					{
						Type="body",
						Parameters=new List<object>()
						{
							new WhatsAppTextParameter{Text=subscriper.FName}
						}
					}
				};
				var phonNumber = _webHostEnvironment.IsDevelopment() ? "01091743467" : subscriper.MobileNumber;

				//var res=_whatsAppClient.SendMessage($"2{phonNumber}", WhatsAppLanguageCode.English, WhatsAppTemplates.WelcomMessage, components);//TODO:welcome message template when meta team approved it(component)
				var res = BackgroundJob.Enqueue(() => _whatsAppClient.SendMessage($"2{phonNumber}", WhatsAppLanguageCode.English, WhatsAppTemplates.WelcomMessage, components));//TODO:welcome message template when meta team approved it(component)

			}
			//End WhatsApp Integration

			return View(nameof(Index));
		}
		public async Task<IActionResult> Edite(string id)//متشفر
		{
			var subscriperId = int.Parse(_dataProtector.Unprotect(id));//decrypt
			var subscriper = _unitOfWord.Subscriper.GetById(subscriperId);
			if (subscriper is null) return BadRequest();
			var subscriperFormVM = _mapper.Map<SubscriperFormVM>(subscriper);
			SelectedList();
			subscriperFormVM.Id = id;
			return View(nameof(Create), subscriperFormVM);
		}
		[HttpPost]
		public async Task<IActionResult> Edite(SubscriperFormVM subscriperFormVM)
		{
			if (!ModelState.IsValid)
			{
				SelectedList();
				return View(nameof(Create), subscriperFormVM);
			}
			subscriperFormVM.Id = _dataProtector.Unprotect(subscriperFormVM.Id);//decrypt
			var Email = _unitOfWord.Subscriper.FindMatch(s => s.Email == subscriperFormVM.Email && s.Id != int.Parse(subscriperFormVM.Id));
			var NaltionalId = _unitOfWord.Subscriper.FindMatch(s => s.NationalId == subscriperFormVM.NationalId && s.Id != int.Parse(subscriperFormVM.Id));
			var MobilNumber = _unitOfWord.Subscriper.FindMatch(s => s.MobileNumber == subscriperFormVM.MobileNumber && s.Id != int.Parse(subscriperFormVM.Id));
			if (Email != null)
			{
				TempData["ErrorMessage"] = "The subscriber's Email that you try to add existed!";
				SelectedList();
				return View();
			}
			if (NaltionalId != null)
			{
				TempData["ErrorMessage"] = "The subscriber's National Id that you try to add existed!";
				SelectedList();
				return View();
			}
			if (MobilNumber != null)
			{
				TempData["ErrorMessage"] = "The subscriber's MobilNumber that you try to add existed!";
				SelectedList();
				return View();
			}
			if (subscriperFormVM.Img != null)
			{
				var RootPath = _webHostEnvironment.WebRootPath;
				if (!string.IsNullOrEmpty(subscriperFormVM.ImgUrl))// ممكن بعدل ومضفتش من البدايه 
				{
					var OldImgpath = Path.Combine($"{RootPath}/Images/Subscriper", subscriperFormVM.ImgUrl);
					if (System.IO.File.Exists(OldImgpath))//ممكن ضفت بس اتحذفت من علي السرفر بالغلط فلو متأكدتش حتحذف نل فهتبلع نل
						System.IO.File.Delete(OldImgpath);
				}
				var extensions = Path.GetExtension(subscriperFormVM.Img.FileName);
				if (!ImgMaxAllowdExtension.Contains(extensions))
				{
					ModelState.AddModelError(nameof(subscriperFormVM.Img), "Allowed only image with extension .jpg, .jpeg, .png");
					SelectedList();
					return View();
				}
				if (subscriperFormVM.Img.Length > ImgMaxAllowdSize)
				{
					ModelState.AddModelError(nameof(subscriperFormVM.Img), "Allowed only image with size 2:MB");
					SelectedList();
					return View();
				}

				var ImageName = $"{Guid.NewGuid()}{extensions}";
				var ImagePath = Path.Combine($"{RootPath}/Images/Subscriper", ImageName);
				using var stream = System.IO.File.Create(ImagePath);
				await subscriperFormVM.Img.CopyToAsync(stream);
				subscriperFormVM.ImgUrl = ImageName;
			}
			var subscriper = _mapper.Map<Subscriper>(subscriperFormVM);
			subscriper.LastUpdatedOn = DateTime.Now;
			_unitOfWord.Subscriper.Update(subscriper);
			_unitOfWord.Commit();
			TempData["SuccessMessage"] = "Edited successfully";
			var SubscribeId = _dataProtector.Protect(subscriper.Id.ToString());//encrypt
			return RedirectToAction(nameof(Details), new { id = SubscribeId });
		}
		public IActionResult Srearch(SearchVM searchVM)
		{
			if (!ModelState.IsValid)
				return View(nameof(Index));
			var Subscriper = _unitOfWord.Subscriper.FindMatch(s => s.Email == searchVM.value
			|| s.MobileNumber == searchVM.value
			|| s.NationalId == searchVM.value);
			if (Subscriper != null)
			{
				searchVM.FullName = $"{Subscriper?.FName} {Subscriper.LName}";
				searchVM.Email = Subscriper.Email;
				searchVM.ImgUrl = Subscriper.ImgUrl;
				// searchVM.Id =Subscriper.Id;
			}
			if (Subscriper is not null)
				searchVM.Id = _dataProtector.Protect(Subscriper.Id.ToString());//encrypt id that will sent to details (1:dfs#%sdf#$)
			return View("Index", searchVM);
		}
		public IActionResult Details(string id)
		{
			var SubscribIdDyc = int.Parse(_dataProtector.Unprotect(id)); //decrypt id (dfs#%sdf#$:1)
																		 //var test = id; id still encrypt
			var subscriber = _unitOfWord.Subscriper.GetAll()
				.Include(s => s.Area)
				.Include(s => s.Governorate)
				.Include(s => s.Rentals)
				.Include(s => s.subcribtions)
				.SingleOrDefault(s => s.Id == SubscribIdDyc);//              <=====
			if (subscriber is null) return NotFound();

			var subscriperFormVM = _mapper.Map<SubscriperVM>(subscriber);
			subscriperFormVM.UcryptId = SubscribIdDyc;
			subscriperFormVM.Id = id;
			return View(subscriperFormVM);
		}
		//[HttpPost]
		public IActionResult RenewSubscription(string id)
		{
			var DycId = int.Parse(_dataProtector.Unprotect(id));
			var Subscriber = _unitOfWord.Subscriper.GetAll().Include(s => s.subcribtions).SingleOrDefault(s => s.Id == DycId);
			if (Subscriber is null) return NotFound();
			if (Subscriber.IsBlackList) return BadRequest();
			var LastSubscribtion = Subscriber.subcribtions.Last();
			var NewSubcribtion = new Subcribtion()
			{
				StartDate = LastSubscribtion.EndDate < DateTime.Today ? DateTime.Today : LastSubscribtion.EndDate.AddDays(1),
				EndDate = DateTime.Today.AddYears(1),//اشتراك سنوي
				CreatedOn = DateTime.Today
			};
			Subscriber.subcribtions.Add(NewSubcribtion);
			_unitOfWord.Subscriper.Update(Subscriber);
			_unitOfWord.Commit();
			var EncId = id;
			TempData["SuccessMessage"] = "Your subscribtion renewed successfully";

			//Start Email
			var TempPath = $"{_webHostEnvironment.WebRootPath}/Templates/{EmailTemplates.Notification}.html";
			StreamReader streamReader = new StreamReader(TempPath);
			var body = streamReader.ReadToEnd();
			streamReader.Close();
			body = body
				.Replace("[ImgUrl]", "https://res.cloudinary.com/moshawky/image/upload/v1736097419/positive-vote_1533908_uyd3zi.png")
				.Replace("[Header]", $"Hey {Subscriber.LName}")
				.Replace("[Body]", $"Your subscribtion has been renewed through {NewSubcribtion.EndDate}");
			//ضيفه في الكيو علشان يشتغل مع نفسه في الباك جراوند والتطبيق يوشف اكل عيشه
			BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(Subscriber.Email, "Renew Message", body));
			//إبعت الإميل بعد دقيقه من الأن
			//BackgroundJob.Schedule(() =>_emailSender.SendEmailAsync(Subscriber.Email, "Renew Message", body),TimeSpan.FromMinutes(1));

			//End Email

			if (Subscriber.HasWatsApp)
			{
				//Start whats app
				List<WhatsAppComponent> Component = new()
				{
					new WhatsAppComponent()
					{
						Type = "body",
						Parameters=new List<object>()
						{
							new WhatsAppTextParameter{Text=Subscriber.LName},
							new WhatsAppTextParameter{Text=NewSubcribtion.EndDate.ToString("dd,MMM,yyyy")}

						}
					}
				};
				var PhoneNumber = _webHostEnvironment.IsDevelopment() ? "01091743467" : Subscriber.MobileNumber;
				BackgroundJob.Enqueue(() => _whatsAppClient.SendMessage($"2{PhoneNumber}", WhatsAppLanguageCode.English, WhatsAppTemplates.renew_subscription, Component));
				//End whats app
			}
			return RedirectToAction(nameof(Details), new { id = EncId });
		}
		public void SelectedList()
		{
			ViewBag.Governoretes = _unitOfWord.Governorate.GetAll();
			ViewBag.Areas = _unitOfWord.Area.GetAll();
		}
	}
}
