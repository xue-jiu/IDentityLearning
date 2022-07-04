using IDentityLearning.DataBase;
using IDentityLearning.JwtModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDentityLearning.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class MyUserController : ControllerBase
    {
        IOptionsSnapshot<JwtSettings> _jwtSettings;
        private readonly UserManager<MyUser> _userManager;
        private readonly RoleManager<MyRole> _roleManager;
        IRoleStore<MyRole> _roleStore;
        public MyUserController(UserManager<MyUser> userManager, RoleManager<MyRole> roleManager, IRoleStore<MyRole> roleStore, IOptionsSnapshot<JwtSettings> jwtSettings)//JwtSettings jwtSettings
        {
           _jwtSettings = jwtSettings;
            _userManager = userManager;
            _roleManager = roleManager;
            _roleStore = roleStore;//里面的方法都需要token
        }
        [HttpGet]
        public IActionResult ShowControllerExist()
        {
            MyRole myNewRole = new MyRole { Name = "NoAdmin" };
            return Ok(_jwtSettings);
        }


        [HttpPost("newRole")]
        public async Task<IActionResult> CreateNewRoleAsync()
        {
           var RoleNameExist =  await _roleManager.RoleExistsAsync("admin");
            if (!RoleNameExist)
            {
                MyRole myRole = new MyRole { Name = "admin" };
                
                var result = await _roleManager.CreateAsync(myRole);
                if (!result.Succeeded)
                {
                    return BadRequest("CreateAsync Failed");
                }
            }
            var user = await _userManager.FindByNameAsync("bsx");
            if (user==null)
            {
                var newUser = new MyUser { UserName= "bsx" };
                
                if (!(await _userManager.CreateAsync(newUser)).Succeeded)
                {
                    return BadRequest("创建失败");
                }
            }
            if (await _userManager.IsInRoleAsync(user, "admin"))
            {
                if (!(await _userManager.AddToRoleAsync(user, "admin")).Succeeded)
                {
                    return BadRequest("创建失败");
                }
            }
            return Ok();
        }
    }
}
