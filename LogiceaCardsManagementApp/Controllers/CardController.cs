using Asp.Versioning;
using LogiceaCardsManagementApp2.CQRS.Card_.Commands;
using LogiceaCardsManagementApp2.CQRS.Card_.Queries;
using LogiceaCardsManagementApp2.CQRS.User_.Queries;
using LogiceaCardsManagementApp2.DTOs;
using LogiceaCardsManagementApp2.Models;
using LogiceaCardsManagementApp2.Util;
using LogiceaCardsManagementApp2.Util.Filter;
using LogiceaCardsManagementApp2.Util.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace LogiceaCardsManagementApp2.Controllers
{
    [Authorize]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private IMediator mediator;
        private ILogger<CardController> _logger;
        
        public CardController(IMediator mediator, ILogger<CardController> logger)
        {
            this.mediator = mediator;
            _logger = logger;
        }

        //GetAll controller with pagination and filtering
        // GET: CardController

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetAll([FromQuery] SearchParams searchParam)
        {
            //Build mediatr query
            FindCardsQuery mQuery = new FindCardsQuery()
            {
                Id = 0,
                loggedInUser = getLoggedInUserAsync().Result
            };
            List<Card> cards = await mediator.Send(mQuery);

            List<ColumnFilter> columnFilters = new List<ColumnFilter>();
            if (!String.IsNullOrEmpty(searchParam.ColumnFilters))
            {
                try
                {
                    columnFilters.AddRange(JsonSerializer.Deserialize<List<ColumnFilter>>(searchParam.ColumnFilters));
                }
                catch (Exception)
                {
                    columnFilters = new List<ColumnFilter>();
                }
            }

            List<ColumnSorting> columnSorting = new List<ColumnSorting>();
            if (!String.IsNullOrEmpty(searchParam.OrderBy))
            {
                try
                {
                    columnSorting.AddRange(JsonSerializer.Deserialize<List<ColumnSorting>>(searchParam.OrderBy));
                }
                catch (Exception)
                {
                    columnSorting = new List<ColumnSorting>();
                }
            }

            Expression<Func<Card, bool>> filters = null;
            //First, we are checking our SearchTerm. If it contains information we are creating a filter.
            var searchTerm = "";
            if (!string.IsNullOrEmpty(searchParam.SearchTerm))
            {
                searchTerm = searchParam.SearchTerm.Trim().ToLower();
                filters = x => x.Name.ToLower().Contains(searchTerm);
            }

            // Then we are overwriting a filter if columnFilters has data.
            if (columnFilters.Count > 0)
            {
                var customFilter = CustomExpressionFilter<Card>.CustomFilter(columnFilters, "cards");
                filters = customFilter;
            }

            var query = cards.AsQueryable().CustomQuery(filters);

            var count = query.Count();
            var filteredData = query.CustomPagination(searchParam.PageNumber, searchParam.PageSize).ToList();

            var pagedList = new PagedList<Card>(filteredData, count, searchParam.PageNumber, searchParam.PageSize);

            if (pagedList != null)
            {
                Response.AddPaginationHeader(pagedList.MetaData);
            }

            return Ok(pagedList);
        }
        
        // GET: CardController/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            //Build mediatr query
            FindCardsQuery mQuery = new FindCardsQuery()
            {
                Id = id,
                loggedInUser = getLoggedInUserAsync().Result
            };
            List<Card> cards = await mediator.Send(mQuery);
            if(cards.Count > 0)
                return Ok(cards.First());
            return NotFound(JSONSerializer(404, "Card Not found"));
        }

      
        // POST: CardController/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateCardCommand command)
        {
            if (ModelState.IsValid)
            {
                
                command.CreatedBy = getLoggedInUserAsync().Result.Id;
                
                var card = new Card();
                try
                {
                    card = await mediator.Send(command);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Create Card error :: {ex.Message}");
                }           

                return Ok(card);
            }
            return BadRequest(ModelState);
        }



        // PUT: CardController/Edit/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, EditCardDto cardDto)
        {
            if (id != cardDto.Id)
            {
                return BadRequest(JSONSerializer(400, "Entity ID not match URI ID"));
            }
            EditCardCommand command = new EditCardCommand()
            {
                cardDto = cardDto,
                loggedInUer = getLoggedInUserAsync().Result
            };

            Card card = await mediator.Send(command);
            if (card != null && card.Id > 0)
                return Ok(card);

            if (card != null && card.Id == 0)
                return Unauthorized(JSONSerializer(401, "The opertaion was not permitted"));

            if (card != null && card.Id == -1)
                return BadRequest(JSONSerializer(400, "Entered Card Status does not exist. Please use [ToDo, InProgress, Done]"));

            return NotFound(JSONSerializer(404, "Card not found"));
        }

        // DELETE: CardController/Delete/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            DeleteCardCommand command = new DeleteCardCommand()
            {
                Id = id,
                loggedInUser = getLoggedInUserAsync().Result
            };
            var result = await mediator.Send(command);
            if (result == 1)
                return Ok(JSONSerializer(200, $"Card {command.Id} was deleted successfully"));
            else if(result == 2)
                return Unauthorized(JSONSerializer(401, "Action not authorized"));
            else
                return NotFound(JSONSerializer(404, "Card Not found"));
        }
        //Util function for getting logged in user details from JWT token
        private async Task<User> getLoggedInUserAsync()
        {
            var userEmail = "";
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;

                userEmail = claims.Where(t => t.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").First().Value;
                
                _logger.LogInformation($"User Email from Token {userEmail}");
            }
            FindUserByEmailQuery query = new FindUserByEmailQuery() { Password = "", email = userEmail };
            User loggedInUser = await mediator.Send(query);
            return loggedInUser;
        }
        private string JSONSerializer(int statusCode, string message)
        {
            return JsonSerializer.Serialize(new GenericResponse() { statusCode = statusCode, message = message });
        }
    }
}
