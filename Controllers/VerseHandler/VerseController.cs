using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using writings_backend_dotnet.Controllers.Validation;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.DTOs;
using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.Controllers.VerseHandler
{
    [ApiController, Route("verse/")]
    public class VerseController(ApplicationDBContext db) : ControllerBase
    {
        private readonly ApplicationDBContext _db = db;

        [HttpGet("{ScriptureNumber}/{SectionNumber}/{ChapterNumber}/{VerseNumber}")]
        public async Task<IActionResult> GetVerse([FromRoute] VerseValidatedDTO dto)
        {
            string requestPath = Request.Path.ToString();
            Cache? cache = await _db.Cache.FirstOrDefaultAsync(c => c.Key == requestPath);

            if (cache != null)  //Checking cache
            {
                string jsonString = cache.Data.RootElement.GetRawText();
                VerseSimpleDTO deserializedData = JsonSerializer.Deserialize<VerseSimpleDTO>(jsonString)!; //Since the cache is not null, this deserialization is unlikely to produce an object type other than VerseSimpleDTO.

                return Ok(new { data = deserializedData });
            }

            //Dynamic including, via .Select() can also be used.
            Verse? verse = await _db.Verse.AsNoTracking().IgnoreAutoIncludes() //AsNoTracking is used for avoid auto inclusion. 
                                            .Include(v => v.Chapter).ThenInclude(c => c.Section).ThenInclude(c => c.Scripture).ThenInclude(c => c.Meanings).ThenInclude(c => c.Language)
                                            .AsSingleQuery() //With the purpose of prevent Cartesian explosion. Reference : https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries
                                            .Include(v => v.Chapter).ThenInclude(c => c.Section.Meanings).ThenInclude(c => c.Language)
                                            .AsSingleQuery()
                                            .Include(v => v.Chapter).ThenInclude(c => c.Meanings).ThenInclude(c => c.Language)
                                            .AsSingleQuery()
                                            .Include(v => v.Words).ThenInclude(w => w.Root).ThenInclude(r => r.Scripture)
                                            .AsSingleQuery()
                                            .Include(v => v.Transliterations).ThenInclude(t => t.Language)
                                            .AsSingleQuery()
                                            .Include(v => v.TranslationTexts).ThenInclude(t => t.Translation).ThenInclude(t => t.Language)
                                            .Include(v => v.TranslationTexts).ThenInclude(t => t.Translation).ThenInclude(t => t.TranslatorTranslations).ThenInclude(tt => tt.Translator).ThenInclude(t => t.Language)
                                            .AsSingleQuery()
                                            .Include(v => v.TranslationTexts).ThenInclude(t => t.FootNotes).ThenInclude(f => f.FootNoteText)
                                            .FirstOrDefaultAsync(v => v.Number == dto.VerseNumber);

            if (verse == null) return NotFound("There is no verse matches with this information.");

            VerseSimpleDTO existedVerse = verse.ToVerseSimpleDTO();

            var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(existedVerse);
            var JSON = JsonDocument.Parse(jsonBytes); //Caching

            await _db.Cache.AddAsync(new Cache { Key = requestPath, Data = JSON });
            await _db.SaveChangesAsync();
            var result = new
            {
                data = new
                {
                    verse = existedVerse
                }
            };

            return Ok(result);
        }


        [HttpGet]
        public IActionResult GetVerse1()
        {
            return Ok(new Dictionary<string, string> { { "sa", "sa" } });
        }
    }
}