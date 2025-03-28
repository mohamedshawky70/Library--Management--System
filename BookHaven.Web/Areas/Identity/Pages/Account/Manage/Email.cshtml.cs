﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace BookHaven.Web.Areas.Identity.Pages.Account.Manage
{
	public class EmailModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IEmailSender _emailSender;

		public EmailModel(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IEmailSender emailSender,
			IWebHostEnvironment webHostEnvironment)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_emailSender = emailSender;
			_webHostEnvironment = webHostEnvironment;
		}

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public bool IsEmailConfirmed { get; set; }

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
			[Required]
			[EmailAddress]
			[Display(Name = "New email")]
			public string NewEmail { get; set; }
		}

		private async Task LoadAsync(ApplicationUser user)
		{
			var email = await _userManager.GetEmailAsync(user);
			Email = email;

			Input = new InputModel
			{
				NewEmail = email,
			};

			IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
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

		public async Task<IActionResult> OnPostChangeEmailAsync()
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

			var email = await _userManager.GetEmailAsync(user);
			if (Input.NewEmail != email)
			{
				var userId = await _userManager.GetUserIdAsync(user);
				var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
				code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
				var callbackUrl = Url.Page(
					"/Account/ConfirmEmailChange",
					pageHandler: null,
					values: new { area = "Identity", userId = userId, email = Input.NewEmail, code = code },
					protocol: Request.Scheme);

				var TempPath = $"{_webHostEnvironment.WebRootPath}/Templates/Email.html";
				StreamReader streamReader = new StreamReader(TempPath);
				var body = streamReader.ReadToEnd();
				streamReader.Close();
				body = body
					.Replace("[ImgUrl]", "https://res.cloudinary.com/moshawky/image/upload/v1736097419/positive-vote_1533908_uyd3zi.png")
					.Replace("[Header]", $"Hey {user.FullName}")
					.Replace("[Body]", "Please,Confirm your email")
					.Replace("[AncorUrl]", HtmlEncoder.Default.Encode(callbackUrl))
					.Replace("[AncorTitle]", "Confirm Email");

				await _emailSender.SendEmailAsync(Input.NewEmail, "Confirm your email", body);


				StatusMessage = "Confirmation link to change email sent. Please check your email.";
				return RedirectToPage();
			}

			StatusMessage = "Your email is unchanged.";
			return RedirectToPage();
		}

		public async Task<IActionResult> OnPostSendVerificationEmailAsync()
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

			var userId = await _userManager.GetUserIdAsync(user);
			var email = await _userManager.GetEmailAsync(user);
			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
			var callbackUrl = Url.Page(
				"/Account/ConfirmEmail",
				pageHandler: null,
				values: new { area = "Identity", userId = userId, code = code },
				protocol: Request.Scheme);

			var TempPath = $"{_webHostEnvironment.WebRootPath}/Templates/Email.html";
			StreamReader streamReader = new StreamReader(TempPath);
			var body = streamReader.ReadToEnd();
			streamReader.Close();
			body = body
				.Replace("[ImgUrl]", "https://res.cloudinary.com/moshawky/image/upload/v1736097419/positive-vote_1533908_uyd3zi.png")
				.Replace("[Header]", $"Hey {user.FullName}")
				.Replace("[Body]", "Please,Confirm your email")
				.Replace("[AncorUrl]", HtmlEncoder.Default.Encode(callbackUrl))
				.Replace("[AncorTitle]", "Confirm Email");


			await _emailSender.SendEmailAsync(email, "Confirm your email", body);

			StatusMessage = "Verification email sent. Please check your email.";
			return RedirectToPage();
		}
	}
}
