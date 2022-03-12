﻿namespace BlueSun.Models.NFTCollections
{
    using System.ComponentModel.DataAnnotations;

    public class AllNFTCollectionsQueryModel
    {
        public const int CollectionsPerPage = 6;

        public string Category { get; init; }

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public CollectionSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalCollections { get; set; }

        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<NFTCollectionListingViewModel> Collections { get; set; }
    }
}