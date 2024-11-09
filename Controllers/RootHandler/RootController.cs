using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using writings_backend_dotnet.Controllers.Validation;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.DTOs;
using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.Controllers.RootHandler
{

    [ApiController, Route("root/")]
    public class RootController(ApplicationDBContext db) : ControllerBase
    {
        private readonly ApplicationDBContext _db = db;

        [HttpGet("{ScriptureNumber}/{RootLatin}")]
        public async Task<IActionResult> GetRoot([FromRoute] RootValidatedDTO dto)
        {

            string requestPath = Request.Path.ToString();
            Cache? cache = await _db.Cache.FirstOrDefaultAsync(c => c.Key == requestPath);

            if (cache != null)  //Checking cache
            {
                string jsonString = cache.Data.RootElement.GetRawText();
                VerseSimpleDTO deserializedData = JsonSerializer.Deserialize<VerseSimpleDTO>(jsonString)!; //Since the cache is not null, this deserialization is unlikely to produce an object type other than VerseSimpleDTO.

                return Ok(new { data = deserializedData });
            }

            Root? root = await _db.Root.AsNoTracking()
                                        .Include(r => r.Words).ThenInclude(w => w.Verse).ThenInclude(v => v.Chapter).ThenInclude(c => c.Section).ThenInclude(s => s.Scripture)
                                        .AsSingleQuery() //With the purpose of prevent Cartesian explosion. Reference : https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries
                                        .Include(r => r.Words).ThenInclude(w => w.Verse)
                                        .FirstOrDefaultAsync(r => r.Latin == dto.RootLatin && r.Scripture.Number == dto.ScriptureNumber);

            if (root == null) return NotFound("There is no root matches with this information.");

            RootExpandedDTO rootExpanded = root.ToRootExpandedDTO();

            var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(rootExpanded);
            var JSON = JsonDocument.Parse(jsonBytes); //Caching

            await _db.Cache.AddAsync(new Cache { Key = requestPath, Data = JSON });
            await _db.SaveChangesAsync();

            var result = new
            {
                data = new
                {
                    root = rootExpanded
                }
            };
            return Ok(result);
        }
    }
}