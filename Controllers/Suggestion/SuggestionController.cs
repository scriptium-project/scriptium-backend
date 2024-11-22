using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.Models;
using writings_backend_dotnet.Validation;

namespace writings_backend_dotnet.Controllers.SuggestionHandler
{

    [ApiController, Route("suggestion"), Authorize]
    public class SuggestionController(ApplicationDBContext db, UserManager<User> userManager) : ControllerBase
    {

        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));


        [HttpPost, Route("create")]
        public async Task<IActionResult> CreateSuggestion([FromBody] SuggestionCreateModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            TranslationText? TranslationTextBeAttached = await _db.TranslationText.FirstOrDefaultAsync(tt => tt.Id == model.TranslationTextId && tt.Translation.EagerFrom.HasValue);

            if (TranslationTextBeAttached == null)
                return NotFound(new { Message = "Translation is not found." });

            Suggestion? ExistingSuggestion = await _db.Suggestion.FirstOrDefaultAsync(s => s.UserId == UserRequested.Id && s.TranslationTextId == TranslationTextBeAttached.Id);

            if (ExistingSuggestion != null)
                return Conflict(new { Message = "There is already a Suggestion belongs to you attached this Translation" });

            Suggestion SuggestionCreated = new()
            {
                SuggestionText = model.SuggestionText,
                UserId = UserRequested.Id,
                TranslationTextId = TranslationTextBeAttached.Id
            };

            try
            {
                await _db.SaveChangesAsync();

                return Ok(new { Message = "The suggestion is successfully attached." });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }

        }

        [HttpPost, Route("delete")]
        public async Task<IActionResult> DeleteSuggestion([FromBody] TranslationTextIdentifierModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            TranslationText? TranslationTextBeDeAttached = await _db.TranslationText.FirstOrDefaultAsync(tt => tt.Id == model.TranslationTextId && tt.Translation.EagerFrom.HasValue);

            if (TranslationTextBeDeAttached == null)
                return NotFound(new { Message = "Translation is not found." });

            Suggestion? SuggestionDeleted = await _db.Suggestion.FirstOrDefaultAsync(s => s.UserId == UserRequested.Id && s.TranslationTextId == TranslationTextBeDeAttached.Id);

            if (SuggestionDeleted == null)
                return NotFound(new { Message = "There is no a Suggestion belongs to you attached this Translation" });

            _db.Suggestion.Remove(SuggestionDeleted);

            try
            {
                await _db.SaveChangesAsync();

                return Ok(new { Message = "The suggestion is successfully removed." });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }
        }
    }
}