using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;
using GreenPoint.Dotnet.Contracts.Parameters;
using GreenPoint.Dotnet.Contracts.ViewModels;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenPoint.Dotnet.WebApi.Controllers
{
    [Route("spots")]
    public class SpotsController : ControllerBase
    {
        private readonly SpotProvider _spotProvider;
        private readonly SpotImageProvider _spotImageProvider;

        public SpotsController(SpotProvider spotProvider, SpotImageProvider spotImageProvider)
        {
            _spotProvider = spotProvider;
            _spotImageProvider = spotImageProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var spots = await _spotProvider.GetAll();

            var spotsViewModel =
                spots.Select(spot => new SpotShortViewModel
                {
                    Id = spot.Id,
                    Latitude = spot.Latitude,
                    Longitude = spot.Langitude
                }).ToList();

            return Ok(spotsViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var spot = await _spotProvider.GetById(id);
                
                var spotImages = await _spotImageProvider.GetBySpotId(id);
                var urlImages = spotImages.Select(spotImage => spotImage.Url).ToList();

                var spotViewModel = new SpotViewModel
                {
                    Id = spot.Id,
                    Longitude = spot.Langitude,
                    Latitude = spot.Latitude,
                    Details = spot.Details,
                    Title = spot.Title,
                    Images = urlImages
                };

                return Ok(spotViewModel);
            }
            catch (ArgumentException e)
            {
                return BadRequest();
            }
        }

        [HttpGet("nearest")]
        public async Task<IActionResult> GetNearestSpot(SpotParameter parameter)
        {   
            var geoSelect = new GeoCoordinate(parameter.Latitude, parameter.Longitude);
            var spotNearest = new Spot
            {
                Id = Guid.Empty
            };
            
            foreach (var s in 
                from spot in await _spotProvider.GetAll() 
                let spotGeo = new GeoCoordinate(spot.Latitude, spot.Langitude) 
                let minGeoSpot = new GeoCoordinate(spotNearest.Latitude, spotNearest.Langitude) 
                let distanceTo = geoSelect.GetDistanceTo(spotGeo) 
                let distance = minGeoSpot.GetDistanceTo(spotGeo) 
                where distanceTo < 200 && distanceTo < distance 
                select spot)
            {
                spotNearest = s;
                spotNearest.Id = s.Id;
                spotNearest.CreationDate = s.CreationDate;
            }

            if (spotNearest.Id == Guid.Empty)
            {
                return NotFound("Not found");
            }
            
            var urlImages = (await _spotImageProvider.GetBySpotId(spotNearest.Id))
                .Select(spotImage => spotImage.Url).ToList();
            
            var spotViewModel = new SpotViewModel
            {
                Id = spotNearest.Id,
                Longitude = spotNearest.Langitude,
                Latitude = spotNearest.Latitude,
                Details = spotNearest.Details,
                Title = spotNearest.Title,
                Images = urlImages
            };

            return Ok(spotViewModel);
        }
    }
}