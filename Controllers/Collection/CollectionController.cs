using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using writings_backend_dotnet.Controllers.Validation;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.Controllers.CollectionHandler
{

    [ApiController, Route("collection"), Authorize]

    public class CollectionController(ApplicationDBContext db, UserManager<User> userManager) : ControllerBase
    {

        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

        [HttpGet, Route("")]
        public async Task<IActionResult> GetCollections()
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            var data = await _db.Collection.Where(c => c.UserId == UserRequested.Id).ToListAsync();

            return Ok(new { data });

        }

        [HttpPost, Route("create")]
        public async Task<IActionResult> CreateCollection([FromBody] CollectionCreateModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            try
            {
                Collection CollectionCreated = new()
                {
                    Name = model.CollectionName,
                    Description = model.Description,
                    UserId = UserRequested.Id
                };

                _db.Collection.Add(CollectionCreated);
                await _db.SaveChangesAsync();

                return Ok(new { Message = "Collection is successfully created!" });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }

        }

        [HttpPost, Route("update")]
        public async Task<IActionResult> UpdateCollection([FromBody] CollectionUpdateModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            Collection? CollectionUpdated = await _db.Collection.FirstOrDefaultAsync(c => c.Name == model.OldCollectionName && c.UserId == UserRequested.Id);

            if (CollectionUpdated == null)
                return NotFound(new { Message = "There is no collection with this name." });


            try
            {
                CollectionUpdated.Name = model.NewCollectionName;
                CollectionUpdated.Description = model.NewDescription;

                _db.Collection.Update(CollectionUpdated);
                await _db.SaveChangesAsync();

                return Ok(new { Message = "Collection is successfully updated!" });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }

        }

        [HttpPost, Route("delete")]
        public async Task<IActionResult> DeleteCollection([FromBody] CollectionDeleteModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            Collection? CollectionDeleted = await _db.Collection.FirstOrDefaultAsync(c => c.Name == model.CollectionName && c.UserId == UserRequested.Id);

            if (CollectionDeleted == null)
                return NotFound(new { Message = "This collection might be already deleted or never been exist." });

            try
            {
                _db.Collection.Remove(CollectionDeleted);
                await _db.SaveChangesAsync();

                return Ok(new { Message = "Collection is successfully deleted!" });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }

        }
    }
}