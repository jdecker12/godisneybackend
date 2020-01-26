﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoDisneyBlog2.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GoDisneyBlog2.Data
{
    public class GoDisneyRepository : IGoDisneyRepository
    {
        private GoDisneyContext _context;
        private ILogger<GoDisneyRepository> _logger;

        public GoDisneyRepository(GoDisneyContext context, ILogger<GoDisneyRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddEntity(object model)
        {
            _context.Add(model);
        }

        public void DeleteEntity(object model)
        {
            _context.Remove(model);
        }

        public async Task<IEnumerable<Card>> GetCard()
        {
            try
            {
                return  await _context.Cards
                    .Include(c => c.CardContents)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Cards {ex}");
                return null;
            }
        }

        public async Task<Card> GetCardById(int id)
        {
            try
            {
                return await _context.Cards
                        .Include(c => c.CardContents)
                        .Where(i => i.Id == id)
                        .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get card data by id {ex}");
                return null;
            }
        }

        public async Task<Card> GetCardByName(string name)
        {
            try
            {
                return await  _context.Cards
                        .Include(c => c.CardContents)
                        .Where(n => n.CardTitle == name)
                        .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get card by name {ex}");
                return null;
            }
        }

        public async Task<IEnumerable<Card>> GetCardsByCat(string cat)
        {
            try
            {
                return await _context.Cards
                        .Include(c => c.CardContents)
                        .Where(n => n.Category == cat)
                        .OrderByDescending(c => c.Id)
                        .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get card by name {ex}");
                return null;
            }
        }

        public async Task<IEnumerable<Card>> GetCardsLinkData(string cat)
        {
            try
            {
                return await _context.Cards
                        .Include(c => c.CardContents)
                        .Where(n => n.Category == cat)
                        .OrderByDescending(c => c.Id)
                        .Take(3)
                        .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get card by name {ex}");
                return null;
            }
        }

        public async Task<bool> SaveAllAsync()
        {
            try
            {
                return (await _context.SaveChangesAsync()) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save all cahnges {ex}");
                return false;
            }
        }
    }
}
