using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoDisneyBlog2.Data;
using GoDisneyBlog2.Data.Entities;
using Microsoft.Extensions.Logging;
using GoDisneyBlog2.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace GoDisneyBlog2.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CardsController: Controller
    {
        private IGoDisneyRepository _repository;
        private ILogger<CardsController> _logger;
        private IMapper _mapper;
        private UserManager<StoreUser> _userManager;
        private SignInManager<StoreUser> _signInManager;

        public CardsController(IGoDisneyRepository repository,  
                               ILogger<CardsController> logger,
                               IMapper mapper,
                               UserManager<StoreUser> userManager,
                               SignInManager<StoreUser> signInManager)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;

        }

        [HttpGet]
        [Route("GetAllCards")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCards()
        {
            try
            {
                var cards = await _repository.GetCard();
                return Ok(_mapper.Map<IEnumerable<Card>, IEnumerable<CardViewModel>>(cards));
            }
            catch(Exception ex)
            {
                _logger.LogError("Failed to get card data");
                return BadRequest($"Failed to get card data {ex}");
            }
        }
        [AllowAnonymous]
        [Route("GetCardById/{id:int}")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCardById(int id)
        {
            try
            {
                var card = await _repository.GetCardById(id);
                if (card != null)
                {
                    return Ok(_mapper.Map<Card, CardViewModel>(card));
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Failed to get card data by id");
                return BadRequest($"Failed to get card data by id {ex}");
            }
        }

        [HttpGet("{name}")]
        [Route("GetCardByName/{name}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCardByName(string name)
        {
            try
            {
                var card = await  _repository.GetCardByName(name);
                if (card != null)
                {
                    return Ok(_mapper.Map<Card, CardViewModel>(card));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get card data by id");
                return BadRequest($"Failed to get card data by id {ex}");
            }
        }



        [HttpGet("{category}")]
        [Route("GetByCategory/{category}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCategory(string category)
        {
            try
            {
                var cards = await _repository.GetCardsByCat(category);
                if (cards != null)
                {
                    return Ok(_mapper.Map<IEnumerable<Card>, IEnumerable<CardViewModel>>(cards));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get card data by category");
                return BadRequest($"Failed to get card data by category {ex}");
            }
        }



        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CardViewModel model)
        {

            var newCard = _mapper.Map<CardViewModel, Card>(model);

           
            try
            {

                    _repository.AddEntity(newCard);
                    if (await _repository.SaveAllAsync())
                    {

                        return Created($"/api/cards/{newCard.Id}", _mapper.Map<Card, CardViewModel>(newCard));
                    }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to save card info. {ex}");
               
            }
            return BadRequest(ModelState);

        }

        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string name, [FromBody]CardViewModel model)
        {
            try
            {
                // if(ModelState.IsValid)
                // {
                 var oldCard = await _repository.GetCardByName(name);
                    if (oldCard == null) return NotFound($"Could not find a card with a name of {name}");
                    _mapper.Map(model, oldCard);

                    if (await _repository.SaveAllAsync())
                    {
                        return Ok(_mapper.Map<CardViewModel>(oldCard));
                    }
                //}
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to update card. {ex}");
               
            }

            return BadRequest($"Failed to update card.");
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            try
            {
                var oldCard = await _repository.GetCardByName(name);
                if (oldCard == null) return NotFound($"Could not find Card with an id of {name}");

                _repository.DeleteEntity(oldCard);

                if (await _repository.SaveAllAsync())
                {
                    return Ok();
                }

            }
            catch(Exception ex)
            {
                _logger.LogError($"Could not delete old card {ex}");
            }
            return BadRequest($"Failed to update old card"); 
        }

    }
}
