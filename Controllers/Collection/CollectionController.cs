using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using writings_backend_dotnet.Controllers.Validation;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.Controllers.CollectionHandler
{

    [ApiController, Route("collection"), Authorize, EnableRateLimiting(policyName: "InteractionControllerRateLimit")]

    public class CollectionController(ApplicationDBContext db, UserManager<User> userManager, ILogger<CollectionController> logger) : ControllerBase
    {

        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly ILogger<CollectionController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        [HttpGet, Route("")]
        public async Task<IActionResult> GetCollections()
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            List<Collection> data = await _db.Collection.Where(c => c.UserId == UserRequested.Id).ToListAsync();

            _logger.LogInformation($"Operation completed: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has demanded his collection records. {data.Count} row has ben returned.");

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

            if (UserRequested.Collections?.Any(c => c.Name == model.CollectionName) ?? false)
            {
                _logger.LogWarning($"Conflict occurred, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has tried to create a Collection named: {model.CollectionName}, User has a collection with same name.");

                return Conflict("You have already the collection with same name.");
            }

            Collection CollectionCreated = new()
            {
                Name = model.CollectionName,
                Description = model.Description,
                UserId = UserRequested.Id
            };

            _db.Collection.Add(CollectionCreated);

            try
            {
                await _db.SaveChangesAsync();

                _logger.LogInformation($"Operation completed. User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has created a Collection: [Id: {CollectionCreated.Id}, CollectionName: {CollectionCreated.Name}]");
                return Ok(new { Message = "Collection is successfully created!" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has tried to create the collection. Collection: [Id: {CollectionCreated.Id}, CollectionName: {CollectionCreated.Name}], Error Details: {ex}");
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }

        }

        [HttpPut, Route("update")]
        public async Task<IActionResult> UpdateCollection([FromBody] CollectionUpdateModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            Collection? CollectionUpdated = null!;

            try
            {

                CollectionUpdated = await _db.Collection.FirstOrDefaultAsync(c => c.Name == model.OldCollectionName && c.UserId == UserRequested.Id);

                if (CollectionUpdated == null)
                {
                    _logger.LogWarning($"Not Found, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] is trying to update Non-existing Collection: [model.OldCollectionName: {model.OldCollectionName}] model.NewCollectionName: {model.NewCollectionName}");
                    return NotFound(new { Message = "There is no collection with this name." });
                }

                CollectionUpdated.Name = model.NewCollectionName;
                CollectionUpdated.Description = model.NewDescription;

                _db.Collection.Update(CollectionUpdated);


                await _db.SaveChangesAsync();
                _logger.LogInformation($"Operation completed. User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has changed his collection named: {model.OldCollectionName} to {CollectionUpdated.Name}");
                return Ok(new { Message = "Collection is successfully updated!" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has tried to create the collection. Collection: [Id: {CollectionUpdated?.Id}, CollectionName: {CollectionUpdated?.Name}], Error Details: {ex}");
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }

        }

        [HttpDelete, Route("delete")]
        public async Task<IActionResult> DeleteCollection([FromBody] CollectionDeleteModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            Collection? CollectionDeleted = null;

            try
            {
                await _db.Collection.FirstOrDefaultAsync(c => c.Name == model.CollectionName && c.UserId == UserRequested.Id);

                if (CollectionDeleted == null)
                {
                    _logger.LogWarning($"Not Found, while. User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] trying to delete Collection : [model.CollectionName: {model.CollectionName}]");
                    return NotFound(new { Message = "This collection might be already deleted or never been exist." });
                }

                _db.Collection.Remove(CollectionDeleted);
                await _db.SaveChangesAsync();

                _logger.LogInformation($"Operation completed. User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has deleted his Collection: [Id: {CollectionDeleted.Id}, CollectionName: {CollectionDeleted.Name}]");

                return Ok(new { Message = "Collection is successfully deleted!" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred, while. User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] trying to delete his Collection: [Id: {CollectionDeleted?.Id}, CollectionName: {CollectionDeleted?.Name}]. Error Details: {ex}");

                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }

        }
    }
}