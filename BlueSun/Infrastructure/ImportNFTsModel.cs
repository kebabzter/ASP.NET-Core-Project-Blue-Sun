namespace BlueSun.Infrastructure
{
    public class ImportNFTsModel
    {
       
        public string Name { get; set; }
       
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
       
        public string Description { get; set; }

        public string OwnerId { get; set; }
        
        public int NFTCollectionId { get; set; }

        public string ImageUrl { get; set; }

        public bool IsForSale { get; set; }
    }
}
