using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ctor.Models;
using Ctor.Data;
using Ctor.Dtos;

namespace Ctor.Controllers;

[Route("[controller]")]
public class UsersController : Controller<User>
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context) : base(context)
    {
        _context = context;
    }

    // PATCH: users/{uuid}
    [HttpPatch("{uuid}")]
    public async Task<IActionResult> Update(Guid uuid, [FromBody] UpdateUserDto patchData)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Uuid == uuid);
        if (user is null) return NotFound();

        if (patchData.Name != null) user.Name = patchData.Name;
        if (patchData.AvatarUrl != null) user.AvatarUrl = patchData.AvatarUrl;
        if (patchData.Username != null) user.Username = patchData.Username;
        if (patchData.Email != null) user.Email = patchData.Email;
        if (patchData.Password != null) user.Password = patchData.Password;

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
