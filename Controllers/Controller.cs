using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ctor.Models;
using Ctor.Data;

namespace Ctor.Controllers;

[ApiController]
public abstract class Controller<T> : ControllerBase where T : Model
{
  private readonly AppDbContext _context;
  private readonly DbSet<T> _db;

  protected Controller(AppDbContext context)
  {
    _context = context;
    _db = context.Set<T>();
  }

  [HttpGet]
  public virtual async Task<ActionResult<IEnumerable<T>>> GetAll()
  {
    return await _db.ToListAsync();
  }

  [HttpGet("{id}")]
  public virtual async Task<ActionResult<T>> GetById(string id)
  {
    T? entity = null;

    if (int.TryParse(id, out var intId))
    {
      entity = await _db.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == intId);
    }
    else if (Guid.TryParse(id, out var guidId))
    {
      entity = await _db.FirstOrDefaultAsync(e => e.Uuid == guidId);
    }

    return entity is null ? NotFound() : Ok(entity);
  }

  [HttpPost]
  public virtual async Task<ActionResult<T>> Create(T entity)
  {
    _db.Add(entity);
    await _context.SaveChangesAsync();
    return CreatedAtAction(nameof(GetById), new { id = entity.Uuid }, entity);
  }

  [HttpPatch("{uuid}/trash")]
  public virtual async Task<IActionResult> Trash(Guid uuid)
  {
    var entity = await _db.FirstOrDefaultAsync(e => e.Uuid == uuid);
    if (entity is null) return NotFound();

    entity.DeletedAt = DateTime.UtcNow;
    await _context.SaveChangesAsync();
    return NoContent();
  }

  [HttpPatch("{uuid}/restore")]
  public virtual async Task<IActionResult> Restore(Guid uuid)
  {
    var entity = await _db
        .IgnoreQueryFilters()
        .FirstOrDefaultAsync(e => e.Uuid == uuid && e.DeletedAt != null);

    if (entity is null) return NotFound();

    entity.DeletedAt = null;
    await _context.SaveChangesAsync();
    return NoContent();
  }

  [HttpDelete("{uuid}")]
  public virtual async Task<IActionResult> Delete(Guid uuid)
  {
    var entity = await _db
        .IgnoreQueryFilters()
        .FirstOrDefaultAsync(e => e.Uuid == uuid);

    if (entity is null) return NotFound();

    _db.Remove(entity);
    await _context.SaveChangesAsync();
    return NoContent();
  }
}
