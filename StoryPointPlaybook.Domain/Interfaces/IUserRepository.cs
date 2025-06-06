using StoryPointPlaybook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User story);
    Task UpdateAsync(User story);
    Task<List<User>> GetByRoomIdAsync(Guid roomId);

}
