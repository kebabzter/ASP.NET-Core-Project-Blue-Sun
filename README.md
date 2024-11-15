# BlueSun

BlueSun is a project created with educational pursposes, using ASP.NET Core (ASP.NET + Entity Framework Core). The database of choice here is Microsft SQL Server.

## Information about the project

* The idea behind this application is to give users a platform to showcase and trade NFTs.
* Guests can visit the Home, All NFTs, Explore(Catalog), Login and Register pages.
* Logged in users have the ability to sign up for a wallet which allows them to purchase and sell NFTs.
  * Every user has the ability to become an Artist by registering with a phone number. The Artist can create entire NFT Collections and NFTs.
  * Every user has a personal collection of his NFTs which can be viewed when you click on personal collection in the taskbar dropdown.
* The Explore pages showcases all NFT collections where you can filter them by Category and sort them based on different criterias.
* All NFTs page shows every single NFT that has been created.
* Every single NFT and NFT collection has to be approved by an Admin first.
  * The Admin can either make a collection visible, invisible, edit the collection/nft himself so that it is suitable or delete the collection/nft.
* You can see other people's collections as well.

## Technologies
* Entity Framework Core
* ASP.NET
* Microsoft SQL Server

## Setup
1. You have to download the whole repository and open the .sln file.
2. You have to set the Program.cs as startup option if it is not already set.
3. Run the application and it will start running locally.
