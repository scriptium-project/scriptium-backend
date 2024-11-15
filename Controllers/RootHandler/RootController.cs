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
using writings_backend_dotnet.Services;

namespace writings_backend_dotnet.Controllers.RootHandler
{

    [ApiController, Route("root/")]
    public class RootController(ApplicationDBContext db, ICacheService cacheService) : ControllerBase
    {
        private readonly ApplicationDBContext _db = db;
        private readonly ICacheService _cacheService = cacheService;


        [HttpGet("{ScriptureNumber}/{RootLatin}")]
        public async Task<IActionResult> GetRoot([FromRoute] RootValidatedDTO dto)
        {
            RootExpandedDTO data;

            string requestPath = Request.Path.ToString();

            RootExpandedDTO? cache = await _cacheService.GetCachedDataAsync<RootExpandedDTO>(requestPath);

            if (cache != null)  //Checking cache
                return Ok(new { data = cache });


            Root? root = await _db.Root.AsNoTracking()
                                        .Include(r => r.Words).ThenInclude(w => w.Verse).ThenInclude(v => v.Chapter).ThenInclude(c => c.Section).ThenInclude(s => s.Scripture)
                                        .AsSingleQuery() //With the purpose of prevent Cartesian explosion. Reference : https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries
                                        .Include(r => r.Words).ThenInclude(w => w.Verse)
                                        .FirstOrDefaultAsync(r => r.Latin == dto.RootLatin && r.Scripture.Number == dto.ScriptureNumber);

            if (root == null) return NotFound("There is no root matches with this information.");

            data = root.ToRootExpandedDTO();

            await _cacheService.SetCacheDataAsync(requestPath, data);

            var result = new
            {
                data
            };
            
            return Ok(result);
        }
    }
}