// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DoAn_LTWeb.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;

namespace DoAn_LTWeb.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IMemoryCache _memoryCache;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender, IMemoryCache memoryCache)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _memoryCache = memoryCache;
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
        public string GenerateShortCode(int length = 6)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"; // tránh 0, O, I, 1
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Không tiết lộ user tồn tại hay không
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            // Tạo mã xác thực ngắn và mã reset token
            string shortCode = GenerateShortCode();
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Lưu cache 15 phút
            _memoryCache.Set($"reset:{Input.Email}", new { ShortCode = shortCode, Token = resetToken },
                TimeSpan.FromMinutes(15));

            // Tạo link (nếu cần)
            var resetLink = Url.Page("/Account/ResetPassword", null,
                new { area = "Identity", email = Input.Email }, Request.Scheme);

            // Gửi email
            await _emailSender.SendEmailAsync(Input.Email, "Đặt lại mật khẩu", $@"
                Chào bạn,<br><br>
                👉 <a href='{resetLink}'>Bấm vào đây để đặt lại mật khẩu</a><br><br>
                Hoặc dùng mã xác thực sau:<br>
                <strong>{shortCode}</strong><br><br>
                Mã này sẽ hết hạn sau 15 phút.
            ");

            return RedirectToPage("./ForgotPasswordConfirmation");
        }

    }
}
