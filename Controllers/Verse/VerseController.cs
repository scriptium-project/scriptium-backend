using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using scriptium_backend_dotnet.Controllers.Validation;
using scriptium_backend_dotnet.DB;
using scriptium_backend_dotnet.DTOs;
using scriptium_backend_dotnet.Models;
using scriptium_backend_dotnet.Services;

namespace scriptium_backend_dotnet.Controllers.VerseHandler
{
    [ApiController, Route("verse"), EnableRateLimiting(policyName: "StaticControllerRateLimiter")]
    public class VerseController(ApplicationDBContext db, ICacheService cacheService, ILogger<VerseController> logger) : ControllerBase
    {
        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        private readonly ILogger<VerseController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        [HttpGet("{ScriptureNumber}/{SectionNumber}/{ChapterNumber}/{VerseNumber}")]
        public async Task<IActionResult> GetVerse([FromRoute] VerseValidatedModel model)
        {
            string requestPath = Request.Path.ToString();

            VerseDTO? cache = await _cacheService.GetCachedDataAsync<VerseDTO>(requestPath);

            VerseDTO data;

            if (cache != null)
            {
                _logger.LogInformation($"Cache data with URL {requestPath} is found. Sending.");
                data = cache;
            }
            else
            {
                Verse? verse = await _db.Verse
                    .IgnoreAutoIncludes()
                    .AsNoTracking()
                    .Where(v => v.Number == model.VerseNumber &&
                                v.Chapter.Number == model.ChapterNumber &&
                                v.Chapter.Section.Number == model.SectionNumber &&
                                v.Chapter.Section.Scripture.Number == model.ScriptureNumber)
                    .Include(v => v.Chapter)
                        .ThenInclude(c => c.Section)
                            .ThenInclude(c => c.Scripture)
                                .ThenInclude(c => c.Meanings)
                                    .ThenInclude(m => m.Language)
                    .Include(v => v.Chapter)
                        .ThenInclude(c => c.Section.Meanings)
                            .ThenInclude(m => m.Language)
                    .Include(v => v.Chapter)
                        .ThenInclude(c => c.Meanings)
                            .ThenInclude(m => m.Language)
                    .Include(v => v.Words)
                        .ThenInclude(w => w.Roots)
                    .Include(v => v.Transliterations)
                        .ThenInclude(t => t.Language)
                    .Include(v => v.TranslationTexts)
                        .ThenInclude(t => t.Translation)
                            .ThenInclude(t => t.Language)
                    .Include(v => v.TranslationTexts)
                        .ThenInclude(t => t.Translation)
                            .ThenInclude(t => t.TranslatorTranslations)
                                .ThenInclude(tt => tt.Translator)
                                    .ThenInclude(t => t.Language)
                    .Include(v => v.TranslationTexts)
                        .ThenInclude(t => t.FootNotes)
                            .ThenInclude(f => f.FootNoteText)
                    .AsSplitQuery() //With the purpose of prevent Cartesian explosion. Reference : https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries
                    .FirstOrDefaultAsync();

                if (verse == null)
                {
                    _logger.LogCritical($"An verse flaw is found. Verse: [ScriptureNumber: {model.ScriptureNumber}, SectionNumber: {model.SectionNumber}, ChapterNumber: {model.ChapterNumber}, VerseNumber: {model.VerseNumber}] ");
                    return NotFound("There is no verse matches with this information.");
                }

                data = verse.ToVerseDTO();

                await _cacheService.SetCacheDataAsync(requestPath, data);
                _logger.LogInformation($"Cache data for URL {requestPath} is renewing");
            }

            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId != null && await _db.CollectionVerse.AnyAsync(c => c.Collection.UserId.ToString() == UserId && c.VerseId == data.Id))
                data.IsSaved = true;


            return Ok(new { data });
        }


        [HttpGet("{ScriptureNumber}/{SectionNumber}/{ChapterNumber}")]
        public async Task<IActionResult> GetChapter([FromRoute] ChapterValidatedModel model)
        {
            ChapterConfinedDTO? data;

            string requestPath = Request.Path.ToString();

            ChapterConfinedDTO? cache = await _cacheService.GetCachedDataAsync<ChapterConfinedDTO>(requestPath);

            if (cache != null)  //Checking cache
            {
                _logger.LogInformation($"Cache data with URL {requestPath} is found. Sending.");
                return Ok(new { data = cache });
            }


            data = await _db.Chapter
                .Where(c => c.Number == model.ChapterNumber && c.Section.Number == model.SectionNumber && c.Section.Scripture.Number == model.ScriptureNumber).AsNoTracking()
                .Select(c => new ChapterConfinedDTO
                {
                    ScriptureName = c.Section.Scripture.Name,
                    ScriptureMeanings = c.Section.Scripture.Meanings.Select(m => new ScriptureMeaningDTO
                    {
                        Language = new LanguageDTO
                        {
                            LangCode = m.Language.LangCode,
                            LangOwn = m.Language.LangOwn,
                            LangEnglish = m.Language.LangEnglish
                        },
                        Meaning = m.Meaning
                    }).ToList(),
                    SectionName = c.Section.Name,
                    SectionMeanings = c.Section.Meanings.Select(m => new SectionMeaningDTO
                    {
                        Language = new LanguageDTO
                        {
                            LangCode = m.Language.LangCode,
                            LangOwn = m.Language.LangOwn,
                            LangEnglish = m.Language.LangEnglish
                        },
                        Meaning = m.Meaning
                    }).ToList(),
                    ChapterName = c.Name,
                    ChapterNumber = c.Number,
                    Verses = c.Verses.Select(v => new ConfinedVerseDTO
                    {
                        Id = v.Id,
                        Text = v.Text,
                        TextSimplified = v.TextSimplified,
                        TextWithoutVowel = v.TextWithoutVowel,
                        Number = v.Number,
                        Transliterations = v.Transliterations.Select(t => new TransliterationDTO
                        {
                            Transliteration = t.Text,
                            Language = new LanguageDTO
                            {
                                LangCode = t.Language.LangCode,
                                LangOwn = t.Language.LangOwn,
                                LangEnglish = t.Language.LangEnglish
                            },
                        }).ToList()
                    }).ToList(),
                    Translations = c.Section.Scripture.Translations.Select(t => new TranslationWithMultiTextDTO
                    {
                        Translation = new TranslationDTO
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Language = new LanguageDTO
                            {
                                LangCode = t.Language.LangCode,
                                LangOwn = t.Language.LangOwn,
                                LangEnglish = t.Language.LangEnglish
                            },
                            Translators = t.TranslatorTranslations.Select(e => new TranslatorDTO
                            {
                                Name = e.Translator.Name,
                                URL = e.Translator.Url,
                                Language = new LanguageDTO
                                {
                                    LangCode = e.Translator.Language.LangCode,
                                    LangOwn = e.Translator.Language.LangOwn,
                                    LangEnglish = e.Translator.Language.LangEnglish
                                }
                            }).ToList(),
                            IsEager = t.EagerFrom.HasValue
                        },
                        TranslationTexts = t.TranslationTexts.Where(tx => tx.Verse.ChapterId == c.Id).Select(tx => new TranslationTextSimpleDTO
                        {
                            FootNotes = tx.FootNotes.Select(ftn => new FootNoteDTO { Index = ftn.Index, Text = ftn.FootNoteText.Text }).ToList(),
                            Text = tx.Text
                        }).ToList(),
                    }).ToList(),
                }
                ).FirstOrDefaultAsync();


            if (data == null)
            {

                _logger.LogCritical($"A chapter flaw is found. Chapter: [ScriptureNumber: {model.ScriptureNumber}, SectionNumber: {model.SectionNumber}, ChapterNumber: {model.ChapterNumber}] ");

                return NotFound("There is no scripture matches with this information.");
            }


            await _cacheService.SetCacheDataAsync(requestPath, data);
            _logger.LogInformation($"Cache data for URL {requestPath} is renewing");

            return Ok(new { data });
        }


        [HttpGet("{ScriptureNumber}/{SectionNumber}")]
        public async Task<IActionResult> GetSection([FromRoute] SectionValidatedModel model)
        {
            SectionSimpleDTO? data;

            string requestPath = Request.Path.ToString();

            SectionSimpleDTO? cache = await _cacheService.GetCachedDataAsync<SectionSimpleDTO>(requestPath);

            if (cache != null)
            {
                _logger.LogInformation($"Cache data with URL {requestPath} is found. Sending.");
                return Ok(new { data = cache });
            }


            data = await _db.Section.Include(s => s.Chapters)
                .Where(s => s.Number == model.SectionNumber && s.Scripture.Number == model.ScriptureNumber)
                .AsNoTracking()
                .Select(s => new SectionSimpleDTO
                {
                    ScriptureName = s.Scripture.Name,
                    ScriptureMeanings = s.Scripture.Meanings.Select(m => new ScriptureMeaningDTO { Language = m.Language.ToLanguageDTO(), Meaning = m.Meaning }).ToList(),
                    Name = s.Name,
                    SectionMeanings = s.Meanings.Select(m => new SectionMeaningDTO { Language = m.Language.ToLanguageDTO(), Meaning = m.Meaning }).ToList(),
                    ChapterCount = s.ChapterCount,
                }
                ).FirstOrDefaultAsync();


            if (data == null)
            {

                _logger.LogCritical($"A section flaw is found. Section: [ScriptureNumber: {model.ScriptureNumber}, SectionNumber: {model.SectionNumber}] ");

                return NotFound("There is no scripture matches with this information.");
            }


            await _cacheService.SetCacheDataAsync(requestPath, data);
            _logger.LogInformation($"Cache data for URL {requestPath} is renewing");

            return Ok(new { data });
        }


        [HttpGet("{ScriptureNumber}")]
        public async Task<IActionResult> GetScripture([FromRoute] ScriptureValidatedModel model)
        {
            ScriptureDTO? data;

            string requestPath = Request.Path.ToString();

            ScriptureDTO? cache = await _cacheService.GetCachedDataAsync<ScriptureDTO>(requestPath);

            if (cache != null)  //Checking cache
            {
                _logger.LogInformation($"Cache data with URL {requestPath} is found. Sending.");
                return Ok(new { data = cache });
            }


            //Link queries make it impossible to fetch.
            data = await _db.Scripture
                .Where(s => s.Number == model.ScriptureNumber)
                .AsNoTracking()
                .Select(s => new ScriptureDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Code = s.Code,
                    Number = s.Number,
                    Meanings = s.Meanings.Select(m => new ScriptureMeaningDTO { Language = m.Language.ToLanguageDTO(), Meaning = m.Meaning }).ToList(),
                    Sections = s.Sections.Select(s => new SectionWithMeaningDTO
                    {
                        Name = s.Name,
                        Meanings = s.Meanings.Select(m => new SectionMeaningDTO { Language = m.Language.ToLanguageDTO(), Meaning = m.Meaning }).ToList(),

                    }).ToList(),
                }
                ).FirstOrDefaultAsync();


            if (data == null)
            {

                _logger.LogCritical($"An scripture flaw is found. Verse: [ScriptureNumber: {model.ScriptureNumber}] ");

                return NotFound("There is no scripture matches with this information.");
            }


            await _cacheService.SetCacheDataAsync(requestPath, data);
            _logger.LogInformation($"Cache data for URL {requestPath} is renewing");

            return Ok(new { data });
        }
    }
}