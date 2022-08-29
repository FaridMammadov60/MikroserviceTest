using AutoMapper;
using CardService.Data;
using CardService.DTOs;
using CardService.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CardService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CardController(AppDbContext context)
        {
            _context = context;           
        }

        /// <summary>
        /// Cardların yaradılması
        /// </summary>
        /// <param name="cardCreateDto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(CardCreateDto cardCreateDto)
        {
            Card newCard = new Card
            {
                Name = cardCreateDto.Name                              

            };
            _context.Cards.Add(newCard);
            _context.SaveChanges();
            return StatusCode(201, "Card yaradıldı");
        }
    }
}
