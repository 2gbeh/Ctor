using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ctor.Models;
using Ctor.Data;
using Ctor.Dtos;

namespace Ctor.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
  private readonly AppDbContext _context;

  public UsersController(AppDbContext context)
  {
    _context = context;
  }

  // GET: api/users
  [HttpGet]
  public async Task<ActionResult<IEnumerable<User>>> GetAll()
  {
    return await _context.Users.ToListAsync();
  }

  // GET: api/users/{id}|{uuid}
  [HttpGet("{id}")]
  public async Task<ActionResult<User>> GetById(string value)
  {
    User? user = null;

    if (int.TryParse(value, out var id))
    {
      user = await _context.Users.FirstOrDefaultAsync(u => EF.Property<int>(u, "Id") == id);
    }
    else if (Guid.TryParse(value, out var uuid))
    {
      user = await _context.Users.FirstOrDefaultAsync(u => u.Uuid == uuid);
    }

    return user is null ? NotFound() : Ok(user);
  }

  // POST: api/users
  [HttpPost]
  public async Task<ActionResult<User>> Create(User user)
  {
    _context.Users.Add(user);
    await _context.SaveChangesAsync();
    return CreatedAtAction(nameof(GetById), new { uuid = user.Uuid }, user);
  }

  // PATCH: api/users/{uuid}
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

    await _context.SaveChangesAsync(); // UpdatedAt auto-set
    return NoContent();
  }

  // PATCH: api/users/{uuid}/trash
  [HttpPatch("{uuid}/trash")]
  public async Task<IActionResult> Trash(Guid uuid)
  {
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Uuid == uuid);
    if (user is null) return NotFound();

    user.DeletedAt = DateTime.UtcNow;
    _context.Users.Update(user);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  // PATCH: api/users/{uuid}/restore
  [HttpPatch("{uuid}/restore")]
  public async Task<IActionResult> Restore(Guid uuid)
  {
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Uuid == uuid && u.DeletedAt != null);
    if (user is null) return NotFound();

    user.DeletedAt = null;
    _context.Users.Update(user);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  // DELETE: api/users/{uuid}
  [HttpDelete("{uuid}")]
  public async Task<IActionResult> Delete(Guid uuid)
  {
    var user = await _context.Users
        .IgnoreQueryFilters() // Optional: if global filter is set
        .FirstOrDefaultAsync(u => u.Uuid == uuid);

    if (user is null) return NotFound();

    _context.Users.Remove(user);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}
