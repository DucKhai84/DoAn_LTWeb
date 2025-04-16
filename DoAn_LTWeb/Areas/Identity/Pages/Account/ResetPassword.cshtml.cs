// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel.DataAnnotations;

public class ResetPasswordModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMemoryCache _memoryCache;

    public ResetPasswordModel(UserManager<IdentityUser> userManager, IMemoryCache memoryCache)
    {
        _userManager = userManager;
        _memoryCache = memoryCache;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Code { get; set; } // short code nhập từ user
    }

    public async Task<IActionResult> OnGetAsync(string code = null, string email = null)
    {
        if (email == null)
            return BadRequest("Phải có mã xác thực và email.");

        Input = new InputModel
        {
            Email = email,
            Code = code
        };

        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var user = await _userManager.FindByEmailAsync(Input.Email);
        if (user == null)
            return RedirectToPage("./ResetPasswordConfirmation");

        var cacheKey = $"reset:{Input.Email}";
        if (!_memoryCache.TryGetValue(cacheKey, out dynamic cacheData) || cacheData == null)
        {
            ModelState.AddModelError(string.Empty, "Mã xác thực không hợp lệ hoặc đã hết hạn.");
            return Page();
        }

        if ((string)cacheData.ShortCode != Input.Code)
        {
            ModelState.AddModelError(string.Empty, "Mã xác thực không chính xác.");
            return Page();
        }

        var token = (string)cacheData.Token;
        var result = await _userManager.ResetPasswordAsync(user, token, Input.Password);

        if (result.Succeeded)
        {
            _memoryCache.Remove(cacheKey);
            return RedirectToPage("./ResetPasswordConfirmation");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return Page();
    }
}



