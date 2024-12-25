using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = CONST_Roles.Admin)]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(ILogger<UserController> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }


        #region ACTIONS

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManager(string userId)
        {
            var user = _unitOfWork.ApplicationUserRepo.Get(u => u.Id == userId);
            if (user == null) return NotFound();

            var userVM = new UserRoleVM()
            {
                ApplicationUser = user,
                CompanyListItems = PopulateCompanyList(),
                RoleListItems = PopulateRoleList(),
            };
            var roleNameList = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();
            if (roleNameList.Any()) userVM.ApplicationUser.Role = roleNameList.First();

            return View(userVM);
        }

        [HttpPost]
        public IActionResult RoleManager(UserRoleVM userRoleVM)
        {
            try
            {
                var userDb = _unitOfWork.ApplicationUserRepo.Get(u => u.Id == userRoleVM.ApplicationUser.Id);
                if (userDb == null) return NotFound();
                var oldRoleName = _userManager.GetRolesAsync(userDb).GetAwaiter().GetResult().First();
                if (oldRoleName == null) return NotFound();

                if (oldRoleName != userRoleVM.ApplicationUser.Role)
                {
                    _userManager.RemoveFromRoleAsync(userRoleVM.ApplicationUser, oldRoleName).GetAwaiter().GetResult();
                    _userManager.AddToRoleAsync(userRoleVM.ApplicationUser, userRoleVM.ApplicationUser.Role).GetAwaiter().GetResult();
                }

                if (userRoleVM.ApplicationUser.Role == CONST_Roles.Company 
                    && userRoleVM.ApplicationUser.CompanyId != userDb.CompanyId)
                {
                    userDb.CompanyId = userRoleVM.ApplicationUser.CompanyId;
                    _unitOfWork.ApplicationUserRepo.Update(userDb);
                    _unitOfWork.Save();
                }
                if (userRoleVM.ApplicationUser.Role != CONST_Roles.Company)
                {
                    userDb.CompanyId = null;
                    _unitOfWork.ApplicationUserRepo.Update(userDb);
                    _unitOfWork.Save();
                }

                TempData["successMessage"] = "User role updated successfuly.";
                userRoleVM.RoleListItems = PopulateRoleList();
                userRoleVM.CompanyListItems = PopulateCompanyList();
                return View(userRoleVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                TempData["errorMessage"] = "Operation update role failed";
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion


        #region API's

        [HttpGet]
        [Route("API/[area]/[controller]/GetAll")]
        public IActionResult GetAll()
        {
            var users = _unitOfWork.ApplicationUserRepo.GetAll(includeProperties: $"{nameof(ApplicationUser.Company)}");
            if (users == null) return NotFound();

            foreach (var user in users)
            {
                user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().First();
                user.Company ??= new Company() { Name = "" };
            }

            return Json(new { data = users });
        }

        [HttpPost]
        [Route("API/[area]/[controller]/LockUnlockAccount")]
        public IActionResult LockUnlockAccount([FromBody] string id)
        {
            try
            {
                var userDb = _unitOfWork.ApplicationUserRepo.Get(u => u.Id == id);
                if (userDb == null)
                    return Json(new { success = false, message = "User not found" });

                var userIsLocked = userDb.LockoutEnd != null && userDb.LockoutEnd > DateTime.Now;
                if (userIsLocked) userDb.LockoutEnd = DateTime.Now;
                else userDb.LockoutEnd = DateTime.Now.AddYears(1000);
                
                _unitOfWork.ApplicationUserRepo.Update(userDb);
                _unitOfWork.Save();

                var message = userIsLocked ? $"Account {userDb.Email} (UNLOCK) successfully." : $"Account {userDb.Email} (LOCKED) successfully.";
                return Json(new { success = true, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, ex);
                return Json(new { success = false, message = "Operation Lock/Unlock Account failed" });
            }
        }

        #endregion


        #region PRIVATE FUNCIONS

        private IEnumerable<SelectListItem> PopulateRoleList()
        {
            return _roleManager.Roles.ToList().Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Name,
            });
        }

        private IEnumerable<SelectListItem> PopulateCompanyList()
        {
            return _unitOfWork.CompanyRepo.GetAll().Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            });
        }

        #endregion
    }
}
