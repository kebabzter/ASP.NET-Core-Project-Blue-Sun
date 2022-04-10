namespace BlueSun.Services.NFTs.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.NFT;
    public class NFTDetailsServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public decimal Price { get; init; }

        public string OwnerId { get; set; }

        public string OwnerName { get; set; }

        public bool UserHasWallet { get; set; }

        public decimal UserBalanceAfterPurchase { get; set; }

        public bool IsForSale { get; set; }

        public bool UserIsOwner { get; set; }

        public string ArtistName { get; init; }

        public int ArtistId { get; init; }

        public string ArtistUserId { get; init; }

        public string Description { get; init; }

        public string ImageUrl { get; init; }

        public int NFTCollectionId { get; init; }

        public string NFTCollectionName { get; init; }
    }
}
