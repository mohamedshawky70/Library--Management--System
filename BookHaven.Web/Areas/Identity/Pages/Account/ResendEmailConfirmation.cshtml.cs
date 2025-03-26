﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace BookHaven.Web.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class ResendEmailConfirmationModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IEmailSender _emailSender;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ResendEmailConfirmationModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment)
		{
			_userManager = userManager;
			_emailSender = emailSender;
			_webHostEnvironment = webHostEnvironment;
		}

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
			//[EmailAddress]
			public string Email { get; set; }
		}

		public void OnGet(string Email)
		{
			Input = new InputModel()
			{
				Email = Email
			};
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}
			var user = _userManager.Users.SingleOrDefault(u => u.Email == Input.Email && !u.IsDeleted || u.UserName == Input.Email && !u.IsDeleted);
			//var user = await _userManager.FindByEmailAsync(Input.Email);
			if (user == null)
			{
				ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
				return Page();
			}

			var userId = await _userManager.GetUserIdAsync(user);
			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
			var callbackUrl = Url.Page(
				"/Account/ConfirmEmail",
				pageHandler: null,
				values: new { userId = userId, code = code },
				protocol: Request.Scheme);

			var TempPath = $"{_webHostEnvironment.WebRootPath}/Templates/Email.html";
			StreamReader streamReader = new StreamReader(TempPath);
			var body = streamReader.ReadToEnd();
			streamReader.Close();
			body = body
				.Replace("[ImgUrl]", "https://res.cloudinary.com/moshawky/image/upload/v1736097419/positive-vote_1533908_uyd3zi.png")
				.Replace("[Header]", $"Hey {user.FullName} thanks for joining us")
				.Replace("[Body]", "Please,Confirm your email")
				.Replace("[AncorUrl]", HtmlEncoder.Default.Encode(callbackUrl))
				.Replace("[AncorTitle]", "Active Account");
			await _emailSender.SendEmailAsync(user.Email, "Confirm your email", body);


			ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
			return Page();
		}
	}
}
