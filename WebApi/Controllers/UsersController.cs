using BLLLibrary.IService;
using DataLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.DTO.Request;

namespace WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController(IUsersService usersService) : ControllerBase
    {
        private readonly IUsersService _usersService = usersService;

        [HttpGet]
        public List<User> GetAllUsers()
        {
            return _usersService.GetAllUsersAsync().Result;
        }

        [HttpGet("{id}")]
        public User? GetUserById(int id)
        {
            return _usersService.GetUserByIdAsync(id).Result;
        }

        [HttpPost]
        public bool AddUser([FromBody] UserRequest userRequest)
        {
            User user = new()
            {
                Email = userRequest.Email,
                Firstname = userRequest.FirstName,
                Surname = userRequest.LastName,
                Login = userRequest.Login,
                Password = userRequest.Password,
                PhoneNumber = userRequest.PhoneNumber,
                AccountType = userRequest.AccountType,
            };
            _usersService.AddUserAsync(user);
            
               return true;
        }

        [HttpPut("{id}")]
        public bool UpdateUser(int id, [FromBody] UserRequest userRequest)
        {
            User? user = _usersService.GetUserByIdAsync(id).Result;
            if (user != null)
            {
                user.Login = userRequest.Login;
                user.Password = userRequest.Password;
                user.PhoneNumber = userRequest.PhoneNumber;
                user.Email = userRequest.Email;
                user.Firstname = userRequest.FirstName;
                user.Surname = userRequest.LastName;
                user.AccountType= userRequest.AccountType;  
                if(_usersService.UpdateUserAsync(user).IsCompletedSuccessfully)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpDelete("{id}")]
        public bool DeleteUser(int id)
        {
            if(_usersService.DeleteUserAsync(id).IsCompletedSuccessfully)
                return true;
            return false;
        }
    }
}
