﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace BookHaven.Web.Areas.Identity.Pages.Account
{
	public class ForgotPasswordModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IEmailSender _emailSender;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment = null)
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
			[EmailAddress]
			public string Email { get; set; }
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(Input.Email);
				if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
				{
					// Don't reveal that the user does not exist or is not confirmed
					return RedirectToPage("./ForgotPasswordConfirmation");
				}

				// For more information on how to enable account confirmation and password reset please
				// visit https://go.microsoft.com/fwlink/?LinkID=532713
				var code = await _userManager.GeneratePasswordResetTokenAsync(user);
				code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
				var callbackUrl = Url.Page(
					"/Account/ResetPassword",
					pageHandler: null,
					values: new { area = "Identity", code },
					protocol: Request.Scheme);

				var TempPath = $"{_webHostEnvironment.WebRootPath}/Templates/Email.html";
				StreamReader streamReader = new StreamReader(TempPath);
				var body = streamReader.ReadToEnd();
				streamReader.Close();
				body = body
					.Replace("[ImgUrl]", "https://res.cloudinary.com/moshawky/image/upload/v1736171342/reset-password_Icon_efo2dq.png")
					.Replace("[Header]", $"Hey {user.FullName}")
					.Replace("[Body]", "Please,click the button to reset password")
					.Replace("[AncorUrl]", HtmlEncoder.Default.Encode(callbackUrl))
					.Replace("[AncorTitle]", "Reset passowrd");


				await _emailSender.SendEmailAsync(user.Email, "Reset Password", body);

				return RedirectToPage("./ForgotPasswordConfirmation");
			}

			return Page();
		}
	}
}
