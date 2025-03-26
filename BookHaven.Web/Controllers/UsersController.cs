using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace BookHaven.Web.Controllers
{
	[Authorize(Roles = AppRoles.Admin)]
	public class UsersController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IMapper _mapper;
		private readonly IEmailSender _emailSender;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public UsersController(UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment)
		{
			_userManager = userManager;
			_mapper = mapper;
			_roleManager = roleManager;
			_emailSender = emailSender;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task<IActionResult> Index()
		{
			var users = await _userManager.Users.ToListAsync();
			var userVM = _mapper.Map<IEnumerable<UserVM>>(users);
			return View(userVM);
		}
		[HttpGet]//default
		public async Task<IActionResult> Create()
		{
			ViewBag.Roles = _roleManager.Roles;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(UserFormVM userFormVM)
		{
			if (!ModelState.IsValid) return BadRequest();
			var user = new ApplicationUser()
			{
				FullName = userFormVM.FullName,
				UserName = userFormVM.UserName,
				Email = userFormVM.Email,
				//CreatedById = User.FindFirst(ClaimTypes.NameIdentifier).Value //Idاليوزر اللي عامل لوقن دلوقتي
			};
			if (await _userManager.FindByEmailAsync(userFormVM.Email) != null)
			{
				ViewBag.Roles = _roleManager.Roles;
				TempData["ErrorMessage"] = "User with the same email that you try to add existed!";
				return View();
			}
			var result = await _userManager.CreateAsync(user, userFormVM.Password);
			if (result.Succeeded)// UserName is Unique in table ASP.NETUser
			{
				//إختار الاسم من الرولز اللي الاي دي بتاعهم ده
				var roles = await _roleManager.Roles.Where(roles => userFormVM.RolesId.Contains(roles.Name)).Select(roles => roles.Name).ToListAsync();
				await _userManager.AddToRolesAsync(user, roles);

				//Start send email to confirm email [This code from register page]
				var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
				code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
				var callbackUrl = Url.Page(
					"/Account/ConfirmEmail",
					pageHandler: null,
					values: new { area = "Identity", userId = user.Id, code = code },
					protocol: Request.Scheme);
				//End send email to confirm email

				var TempPath = $"{_webHostEnvironment.WebRootPath}/Templates/{EmailTemplates.Email}.html";//مكان التمبلت اللي هتتبعت
				StreamReader streamReader = new StreamReader(TempPath);//للتعامل مع هذه التمبلت
				var body = streamReader.ReadToEnd();//إقراها للاخر
				streamReader.Close();
				body = body
					.Replace("[ImgUrl]", "https://res.cloudinary.com/moshawky/image/upload/v1736097419/positive-vote_1533908_uyd3zi.png")
					.Replace("[Header]", $"Hey {user.FullName} thanks for joining us")
					.Replace("[Body]", "Please,Confirm your email")
					.Replace("[AncorUrl]", HtmlEncoder.Default.Encode(callbackUrl))
					.Replace("[AncorTitle]", "Active Account");

				await _emailSender.SendEmailAsync(user.Email, "Confirm your email", body);


				TempData["SuccessMessage"] = "Saved successfully";
			}
			else
			{
				ViewBag.Roles = _roleManager.Roles;
				TempData["ErrorMessage"] = "User name that you try to add existed!";
				return View();
			}
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> ToggleStatus(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user is null) return BadRequest();
			user.IsDeleted = !user.IsDeleted;
			user.LastUpdatedOn = DateTime.Now;
			user.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			await _userManager.UpdateAsync(user);
			await _userManager.UpdateSecurityStampAsync(user);
			return Ok(user.LastUpdatedOn.ToString());
		}
		public IActionResult ResetPassword(string id)
		{
			var user = _userManager.FindByIdAsync(id);
			ResetPassword resetPassword = new()
			{
				Id = id
			};
			if (user is null) return NotFound();
			return View(resetPassword);
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
		{
			if (!ModelState.IsValid) return BadRequest();
			var user = await _userManager.FindByIdAsync(resetPassword.Id);
			if (user is null) return NotFound();

			var CurrentPasswordHash = user.PasswordHash;
			await _userManager.RemovePasswordAsync(user); // لو محزفتش القديمه لما تيجي تضيف هيقولك رايح فين يانورم مافيه باس بالفعل
			var result = await _userManager.AddPasswordAsync(user, resetPassword.Password);
			if (result.Succeeded)
			{
				user.LastUpdatedOn = DateTime.Now;
				//user.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier).Value;
				await _userManager.UpdateAsync(user);
				TempData["SuccessMessage"] = "Saved successfully";
				return RedirectToAction(nameof(Index));
			}
			user.PasswordHash = CurrentPasswordHash;// في حالة الجديدية متضافتس لسبب ما
			await _userManager.UpdateAsync(user);
			return BadRequest();
		}
		public async Task<IActionResult> Edite(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user is null) return NotFound();
			var userFormVm = _mapper.Map<UserFormVM>(user);
			userFormVm.RolesId = await _userManager.GetRolesAsync(user);

			ViewBag.Roles = _roleManager.Roles;
			return View(nameof(Create), userFormVm);
		}
		[HttpPost]
		public async Task<IActionResult> Edite(UserFormVM userFormVM)
		{
			if (!ModelState.IsValid) return BadRequest();
			var user = await _userManager.FindByIdAsync(userFormVM.Id);
			if (user is null) return NotFound();
			var NewUser = await _userManager.FindByEmailAsync(userFormVM.Email);
			if (NewUser != null && user.Email != NewUser.Email)
			{
				ViewBag.Roles = _roleManager.Roles;
				TempData["ErrorMessage"] = "User with the same email that you try to add existed!";
				return View(nameof(Create), userFormVM);
			}

			//when user mapper appears tracking error
			user.FullName = userFormVM.FullName;
			user.UserName = userFormVM.UserName;
			user.Email = userFormVM.Email;
			user.LastUpdatedOn = DateTime.Now;
			user.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var result = await _userManager.UpdateAsync(user);
			//var user =  _mapper.Map<ApplicationUser>(userFormVM);
			if (result.Succeeded)
			{
				//ممكن في المستقبل تقارن الرول الموجوده بالجديده ممكن تطلع هي هي بدل متحذف ثم تضيف
				var CurrentRole = await _userManager.GetRolesAsync(user);
				await _userManager.RemoveFromRolesAsync(user, CurrentRole);
				await _userManager.AddToRolesAsync(user, userFormVM.RolesId);
				await _userManager.UpdateSecurityStampAsync(user);
				TempData["SuccessMessage"] = "Saved successfully";
				return RedirectToAction(nameof(Index));
			}
			return BadRequest();
		}
		public async Task<IActionResult> UnLock(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user is null) return NotFound();

			var IsLocked = await _userManager.IsLockedOutAsync(user);
			if (IsLocked) await _userManager.SetLockoutEndDateAsync(user, null);
			return Ok();
		}

	}
}
