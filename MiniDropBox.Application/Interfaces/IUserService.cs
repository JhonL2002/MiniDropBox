using MiniDropBox.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniDropBox.Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserDTO>> CreateUserAsync(UserDTO userDTO);
    }
}
