﻿namespace SoftGym.Services.Data
{
    using Microsoft.EntityFrameworkCore;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Services.Mapping;
    using System.Linq;
    using System.Threading.Tasks;

    public class CardsService : ICardsService
    {
        private readonly IDeletableEntityRepository<Card> cardRepository;
        private readonly IQrCodeService qrCodeService;

        public CardsService(
            IDeletableEntityRepository<Card> cardRepository,
            IQrCodeService qrCodeService)
        {
            this.cardRepository = cardRepository;
            this.qrCodeService = qrCodeService;
        }

        public async Task<Card> GenerateCardAsync(ApplicationUser user)
        {
            Card card = new Card
            {
                Visits = 0,
                User = user,
                UserId = user.Id,
            };
            card.PictureUrl = await this.qrCodeService.GenerateQrCodeAsync(card.Id);
            await this.cardRepository.AddAsync(card);

            return card;
        }

        public async Task<T> GetCardViewModelAsync<T>(string userId)
        {
            var result = await this.cardRepository
                .All()
                .Where(x => x.UserId == userId)
                .To<T>()
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
