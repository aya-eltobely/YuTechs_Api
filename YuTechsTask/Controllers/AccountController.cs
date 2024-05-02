using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YuTechsTask.DTOs;
using YuTechsTask.Helpers;
using YuTechsTask.Models;

namespace YuTechsTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;


        public AccountController(UserManager<ApplicationUser> _userManager, IConfiguration _configuration)
        {
            userManager = _userManager;
            configuration = _configuration;
        }




        [HttpPost("login")] //api/account/login
        public async Task<IActionResult> LogIn([FromBody] LoginUserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                //check username
                ApplicationUser user = await userManager.FindByNameAsync(userDTO.UserName);
                if (user != null)
                {
                    //check pass
                    bool res = await userManager.CheckPasswordAsync(user, userDTO.Password);
                    if (res)
                    {
                        //(2)
                        var Allclaims = new List<Claim>();
                        Allclaims.Add(new Claim(ClaimTypes.Name, user.UserName)); //custom claim


                        if (await userManager.IsInRoleAsync(user, WebSiteRoles.SiteAdmin))
                        {
                            Allclaims.Add(new Claim(ClaimTypes.Role, WebSiteRoles.SiteAdmin));
                        }
                        else if (await userManager.IsInRoleAsync(user, WebSiteRoles.SiteAuthor))
                        {
                            Allclaims.Add(new Claim(ClaimTypes.Role, WebSiteRoles.SiteAuthor));
                        }

                        Allclaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id)); //custom claim
                        Allclaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); //predifne claims ==> token id

                        //(3)
                        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:secretKey"]));
                        SigningCredentials signingCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        //create token (1)
                        JwtSecurityToken myToken = new JwtSecurityToken(
                            issuer: configuration["JWT:issuer"], // web api server url
                            audience: configuration["JWT:audiance"], //angular url
                            claims: Allclaims,
                            expires: DateTime.Now.AddDays(2),
                            signingCredentials: signingCredential
                            );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(myToken),
                            expiration = myToken.ValidTo
                        }
                            );
                    }
                    else
                    {
                        return Unauthorized("Password Wrong");
                    }
                }
                else
                {
                    return Unauthorized("User Name not Found");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


    }
}
