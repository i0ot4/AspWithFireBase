using Asp.FireStore.Models.FirebaseModel;
using Asp.FireStore.Repository.IRepository;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace Asp.FireStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private static int maxFailedLoginAttempts = 3;
        private IMemoryCache _cache;
        private static string ApiKey = "AIzaSyBEyD0Y6E-guN-dFVc2EorSprGSlGMq0Ns";
        private static string Bucket = "malik-project-419023.web.app";

        FirebaseAuthClient authClient = new FirebaseAuthClient(new FirebaseAuthConfig
        {
            ApiKey = ApiKey,
            AuthDomain = Bucket,
            Providers = new FirebaseAuthProvider[]
            {
                new EmailProvider()
            }
        });

        public HomeController(IUsersRepository _usersRepository, IMemoryCache cache)
        {
            this._usersRepository = _usersRepository;
            _cache = cache;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            try
            {
                if (loginModel.Email == null || loginModel.Password == null)
                {
                    return View(loginModel);
                }

                // Check if the account is locked
                if (_cache.TryGetValue(loginModel.Email, out DateTime lockoutEnd))
                {
                    if (DateTime.Now < lockoutEnd)
                    {
                        var lockoutTimeRemaining = lockoutEnd.Subtract(DateTime.Now);
                        ModelState.AddModelError(String.Empty, $"الحساب مقفل يرجى المحاولة مرة أخرى بعد {lockoutTimeRemaining.Minutes} دقائق و {lockoutTimeRemaining.Seconds} ثوان");
                        return View(loginModel);
                    }
                }
                //log in the new user
                var fbAuthLink = await authClient
                                .SignInWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
                var userType = await _usersRepository.GetById("Users", fbAuthLink.User.Uid);
                if (userType.UserType != "Admin")
                {
                    if (userType.UserType != "SuperAdmin")
                    {
                        await HttpContext.SignOutAsync();
                        return RedirectToAction("NotFound404", "Error");
                    }
                }

                string token = await fbAuthLink.User.GetIdTokenAsync(false);
                //saving the token in a session variable
                if (token != null)
                {
                    HttpContext.Session.SetString("_UserToken", token);
                    /*
                                        userType.DeviceToken = token;
                                        await _usersRepository.Update("Users", fbAuthLink.User.Uid, userType);*/

                    // Set the user identity
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, loginModel.Email));

                    // Add role to the user identity
                    identity.AddClaim(new Claim(ClaimTypes.Role, userType.UserType));

                    // Add user data to the user identity
                    identity.AddClaim(new Claim("UserId", fbAuthLink.User.Uid));
                    identity.AddClaim(new Claim("UserEmail", fbAuthLink.User.Info.Email));
                    identity.AddClaim(new Claim("UserProfilePicture", userType.ProfilePicture));
                    identity.AddClaim(new Claim("UserFirstName", userType.FirstName));
                    identity.AddClaim(new Claim("UserLastName", userType.LastName));
                    identity.AddClaim(new Claim("UserType", userType.UserType));

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Users");
                }
            }

            catch (FirebaseAuthException ex)
            {
                if (_cache.TryGetValue(loginModel.Email, out int failedLoginAttempts))
                {
                    failedLoginAttempts++;
                    if (failedLoginAttempts >= maxFailedLoginAttempts)
                    {
                        // Lock the account for 10 minutes
                        _cache.Set(loginModel.Email, DateTime.Now.AddMinutes(10), new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
                        ModelState.AddModelError(String.Empty, $"لقد تم قفل الحساب لمدة 10 دقائق");
                        return View(loginModel);
                    }
                    else
                    {
                        _cache.Set(loginModel.Email, failedLoginAttempts, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)));
                        ModelState.AddModelError(String.Empty, $"يتبقى لديك {maxFailedLoginAttempts - failedLoginAttempts} محاولات.");
                        return View(loginModel);
                    }
                }
                else
                {
                    int co = _cache.Set(loginModel.Email, 1, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)));
                    ModelState.AddModelError(String.Empty, $"يتبقى لديك {maxFailedLoginAttempts - co} محاولات.");
                    return View(loginModel);
                }

            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Remove the user token from the session
            HttpContext.Session.Remove("_UserToken");

            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {

            var email = User.FindFirst("UserEmail")?.Value;
            try
            {
                if (changePasswordModel.Email == null || changePasswordModel.NewPassword == null || changePasswordModel.ConfirmPassword == null)
                {
                    return View(changePasswordModel);
                }

                // Check if NewPassword and ConfirmPassword match
                if (changePasswordModel.NewPassword != changePasswordModel.ConfirmPassword)
                {
                    ModelState.AddModelError(String.Empty, "كلمة المرور الجديدة وتأكيد كلمة المرور لا تتطابق.");
                    return View(changePasswordModel);
                }

                if (_cache.TryGetValue(email, out DateTime lockoutEnd))
                {
                    if (DateTime.Now < lockoutEnd)
                    {
                        var lockoutTimeRemaining = lockoutEnd.Subtract(DateTime.Now);
                        ModelState.AddModelError(String.Empty, $"الحساب مقفل يرجى المحاولة مرة أخرى بعد {lockoutTimeRemaining.Minutes} دقائق و {lockoutTimeRemaining.Seconds} ثوان");
                        return View(changePasswordModel);
                    }
                }

                if (changePasswordModel.Email != email)
                {

                    ModelState.AddModelError(String.Empty, "ان الايميل المدخل لا يتطابق مع لايميل الذي تملكة.");
                    if (_cache.TryGetValue(email, out int failedLoginAttempts))
                    {
                        failedLoginAttempts++;
                        if (failedLoginAttempts >= maxFailedLoginAttempts)
                        {
                            // Lock the account for 10 minutes
                            _cache.Set(email, DateTime.Now.AddMinutes(10), new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
                            ModelState.AddModelError(String.Empty, $"لقد تم قفل الحساب لمدة 10 دقائق");

                            await HttpContext.SignOutAsync();
                            return RedirectToAction("Login");
                        }
                        else
                        {
                            _cache.Set(email, failedLoginAttempts, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)));
                            ModelState.AddModelError(String.Empty, $"يتبقى لديك {maxFailedLoginAttempts - failedLoginAttempts} محاولات.");
                            return View(changePasswordModel);
                        }
                    }
                    else
                    {
                        int co = _cache.Set(email, 1, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)));
                        ModelState.AddModelError(String.Empty, $"يتبقى لديك {maxFailedLoginAttempts - co} محاولات.");
                        return View(changePasswordModel);
                    }
                }
                // Change the password
                await authClient.User.ChangePasswordAsync(changePasswordModel.NewPassword);

                // Log out the user
                await HttpContext.SignOutAsync();

                return RedirectToAction("Login");
            }
            catch (FirebaseAuthException ex)
            {

                ModelState.AddModelError(String.Empty, "حدث خطأ أثناء تغيير كلمة المرور. يرجى التأكد من أن البريد الإلكتروني صحيح.");
                return View(changePasswordModel);
            }
        }



    }
}
