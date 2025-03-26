// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookHaven.Web.Areas.Identity.Pages.Account.Manage
{
	public class IndexModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IWebHostEnvironment _webHostEnvironment;
		List<string> ImgAllowedExtension = new() { ".png", ".jpg", ".jpeg" };
		int ImgAllowedSize = 2097152;
		public IndexModel(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IWebHostEnvironment webHostEnvironment)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_webHostEnvironment = webHostEnvironment;
		}

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[TempData]
		public string StatusMessage { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[BindProperty]
		public InputModel Input { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public class InputModel
		{
			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			/// 	public string  FullName { get; set; }
			/// 			public string  FullName { get; set; }
			/// 			
			public string? Img { get; set; }
			[RegularExpression(RejexPatterns.OnlyEnglishLitter, ErrorMessage = "Only English letter allowed")]
			[MaxLength(30, ErrorMessage = "max length of name is 30 character"), Display(Name = "Full Name")]
			public string FullName { get; set; }
			[RegularExpression(RejexPatterns.ValidphoneNumber, ErrorMessage = "Invalid phone Number")]
			[Display(Name = "Phone number")]
			public string PhoneNumber { get; set; }
		}

		private async Task LoadAsync(ApplicationUser user)
		{
			var userName = await _userManager.GetUserNameAsync(user);
			var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
			Username = userName;

			Input = new InputModel
			{
				PhoneNumber = phoneNumber,
				FullName = user.FullName

			};
		}

		public async Task<IActionResult> OnGetAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			await LoadAsync(user);
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(IFormFile Image)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			if (!ModelState.IsValid)
			{
				await LoadAsync(user);
				return Page();
			}
			if (Image != null)
			{
				var Extension = Path.GetExtension(Image.FileName);
				if (!ImgAllowedExtension.Contains(Extension))
				{
					//diff from view(typeOf(model.Img))
					ModelState.AddModelError("Input.Img", "Allowed only image with extension .jpg, .jpeg, .png");
					await LoadAsync(user);
					return Page();
				}
				if (ImgAllowedSize < Image.Length)
				{
					//diff from view(typeOf(model.Img))
					ModelState.AddModelError("Input.Img", "Allowed only image with size 2:MB");
					await LoadAsync(user);
					return Page();
				}
				//مش محتاج احذف الصورة القديمه لاني بخزنا بالاي دي وده ثابت لنفس اليوز فهيعمل ربليس
				var rootPath = _webHostEnvironment.WebRootPath;
				var ImageName = $"{user.Id}.png";
				var ImagePath = Path.Combine($"{rootPath}/Images/User", ImageName);
				using var stream = System.IO.File.Create(ImagePath);
				await Image.CopyToAsync(stream);
				user.Img = ImageName;
				await _userManager.UpdateAsync(user);
			}


			var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
			if (Input.PhoneNumber != phoneNumber)
			{
				var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
				if (!setPhoneResult.Succeeded)
				{
					StatusMessage = "Unexpected error when trying to set phone number.";
					return RedirectToPage();
				}
			}
			var FullName = user.FullName;
			if (Input.FullName != FullName)
			{
				user.FullName = Input.FullName;
				var result = await _userManager.UpdateAsync(user);

				if (!result.Succeeded)
				{
					StatusMessage = "Unexpected error when trying to set Full Name.";
					return RedirectToPage();
				}
			}

			await _signInManager.RefreshSignInAsync(user);
			StatusMessage = "Your profile has been updated";
			return RedirectToPage();
		}
	}
}
