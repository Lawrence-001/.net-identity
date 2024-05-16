using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Identity.Data;
using MVC.Identity.ViewModels;
using MVC.Identity.ViewModels.Administration;

namespace MVC.Identity.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Super Admin")]
        //[AllowAnonymous]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> CreateRole(CreateRoleVM createRoleVM)
        {
            if (ModelState.IsValid)
            {
                bool roleExists = await _roleManager.RoleExistsAsync(createRoleVM.Name);
                if (roleExists)
                {
                    ModelState.AddModelError("", $"Role {createRoleVM.Name} already exixts");
                }

                IdentityRole role = new IdentityRole()
                {
                    Name = createRoleVM.Name,
                };

                IdentityResult result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("AllRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(createRoleVM);
        }

        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> AllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpGet]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return View("NotFound");
            }
            EditRoleVM editRoleVM = new EditRoleVM()
            {
                Id = role.Id,
                Name = role.Name,
            };

            editRoleVM.Users = new List<string>();

            foreach (var user in _userManager.Users.ToList())
            {
                //if user is in role, add user email to editrolevm users property
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    editRoleVM.Users.Add(user.Email);
                }
            }
            return View(editRoleVM);

        }

        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> EditRole(EditRoleVM editRoleVM)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(editRoleVM.Id);
                if (role == null)
                {
                    return View("NotFound");
                }
                role.Name = editRoleVM.Name;

                IdentityResult result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("AllRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(editRoleVM);
        }

        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                try
                {
                    var result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("AllRoles");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("AllRoles");
                }
                // If the exception is DbUpdateException, we know we are not able to
                // delete the role as there are users in the role being deleted
                catch (DbUpdateException ex)
                {
                    ViewBag.Error = ex.Message;

                    // Pass the ErrorTitle and ErrorMessage that you want to show to the user using ViewBag.
                    // The Error view retrieves this data from the ViewBag and displays to the user.
                    ViewBag.ErrorTitle = $"{role.Name} Role is in Use";
                    ViewBag.ErrorMessage = $"{role.Name} Role cannot be deleted as there are users in this role. " +
                        $"If you want to delete this role, please remove the users from the role and then try to delete.";
                    return View("Error");
                    throw;
                }

            }

        }

        [HttpGet]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return View("NotFound");
            }
            ViewBag.RoleName = role.Name;
            var userRoleVM = new List<UserRoleVm>();
            foreach (var user in _userManager.Users.ToList())
            {
                UserRoleVm model = new UserRoleVm()
                {
                    UserId = user.Id,
                    Email = user.Email,
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.IsSelected = true;
                }
                else
                {
                    model.IsSelected = false;
                }
                userRoleVM.Add(model);
            }
            return View(userRoleVM);

        }

        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleVm> userRoleVm, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return View("NotFound");
            }

            for (var i = 0; i < userRoleVm.Count; i++)
            {
                IdentityResult? result;
                var user = await _userManager.FindByIdAsync(userRoleVm[i].UserId);
                if (userRoleVm[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!userRoleVm[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < userRoleVm.Count - 1)
                    {
                        continue;
                    }
                    return RedirectToAction("EditRole", new { roleId });
                }
            }
            return RedirectToAction("EditRole", new { roleId });
        }

        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var claims = await _userManager.GetClaimsAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            if (user != null)
            {
                EditUserVM editUserVM = new EditUserVM()
                {
                    Id = user.Id,
                    FName = user.FName,
                    LName = user.LName,
                    Email = user.Email,
                    Claims = claims.Select(x => x.Value).ToList(),
                    Roles = roles
                };
                return View(editUserVM);
            }
            return View("NotFound");
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserVM editUserVM)
        {
            var user = await _userManager.FindByIdAsync(editUserVM.Id);
            if (user != null)
            {
                user.FName = editUserVM.FName;
                user.LName = editUserVM.LName;
                user.Email = editUserVM.Email;

                user.UserName = editUserVM.Email;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["Success"] = "User updated successfully";
                    return RedirectToAction("AllUsers", "Administration");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(editUserVM);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var userToDelete = await _userManager.FindByIdAsync(id);
            if (userToDelete != null)
            {
                var result = await _userManager.DeleteAsync(userToDelete);
                if (result.Succeeded)
                {
                    return RedirectToAction("AllUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View("NotFound");
        }
    }
}
