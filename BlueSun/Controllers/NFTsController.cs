﻿namespace BlueSun.Controllers
{
    using AspNetCoreHero.ToastNotification.Abstractions;
    using AutoMapper;
    using BlueSun.Infrastructure.Extensions;
    using BlueSun.Models.NFTs;
    using BlueSun.Services.NFTs;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static WebConstants;
    public class NFTsController : Controller
    {
        private readonly INFTsService nfts;
        private readonly IMapper mapper;
        private readonly INotyfService notyf;

        public NFTsController(
            INFTsService nfts,
            IMapper mapper, INotyfService notyf)
        {
            this.nfts = nfts;
            this.mapper = mapper;
            this.notyf = notyf;
        }

        [Authorize]
        public IActionResult Add(int id)
        {
            string collectionName = nfts.GetCollectionName(id);

            return View(new AddNFTFormModel
               {
                   CollectionName = collectionName
               });
        }

        [HttpPost]
        public IActionResult ForSale(int id, decimal price)
        {
            if (price < 1 || price > 10000)
            {
                notyf.Warning("Price must be between 1 and 10000!");
                return RedirectToAction(nameof(NFTsController.Details), "NFTs", new { id });
            }

            nfts.ForSale(id, price);
            notyf.Success("Your successfully put your item on the market!");

            return RedirectToAction(nameof(Details), new { id });
        }

        public IActionResult TakeFromMarket(int id)
        {
            nfts.TakeFromMarket(id);

            notyf.Success("Your successfully took your item off the market!");
            return RedirectToAction(nameof(Details), new { id });
        }

        public IActionResult All()
        {
            var nftsData = nfts.All();

            return View(nftsData);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddNFTFormModel nft, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(nft);
            }

            var userId = this.User.Id();

            nfts.Add(nft, id, userId);

            return RedirectToAction(nameof(NFTCollectionsController.MyCollections), "NFTCollections");
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var userId = this.User.Id();

            var nftDetails = nfts.Details(id, userId);

            return View(nftDetails);
        }

        [Authorize]
        public IActionResult ConnectWallet(int id)
        {
            var userId = this.User.Id();

            var canConnect = nfts.ConnectWallet(userId);

            if (!canConnect)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
