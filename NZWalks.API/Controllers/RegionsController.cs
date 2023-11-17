using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #region " Read Operation "
        [HttpGet]
        public IActionResult GetAll()
        {
            //Get data from database - Domain Model
            var regionDomainModel = dbContext.Regions.ToList();
            //Convert/Map domain model to DTO
            var regionDTO = new List<RegionDTO>();

            foreach (var regionDomain in regionDomainModel)
            {
                regionDTO.Add(
                    new RegionDTO
                    {
                        Id = regionDomain.Id,
                        Name = regionDomain.Name,
                        Code = regionDomain.Code,
                        RegionImageUrl = regionDomain.RegionImageUrl,
                    });

            }
            return Ok(regionDTO);

        }

        #endregion

        #region " Get By Id operation "
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomainModel is null)
            {
                return NotFound();
            }
            var regionDTO = new RegionDTO()
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDTO);
        }
        #endregion

        #region " Create Operation "
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            //Get data from DTO - addRegionRequestDTO
            //Map/Convert addRegionRequestDTO to DomainModel
            var regiondomainModel = new Region()
            {
                Name = addRegionRequestDTO.Name,
                Code = addRegionRequestDTO.Code,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl,
            };
            dbContext.Regions.Add(regiondomainModel);
            dbContext.SaveChanges();
            var regionDTO = new RegionDTO
            {
                Id = regiondomainModel.Id,
                Name = regiondomainModel.Name,
                Code = regiondomainModel.Code,
                RegionImageUrl = regiondomainModel.RegionImageUrl,
            };
            return CreatedAtAction(nameof(GetById), new { id = regionDTO.Id }, regionDTO);
        }
        #endregion
    }
}
