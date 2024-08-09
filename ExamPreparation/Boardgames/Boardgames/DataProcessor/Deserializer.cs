namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Boardgames.XmlHelpers;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            var creatorsDto = XmlSerializationHelper.Deserialize<List<ImportCreatorDto>>(xmlString, "Creators");

            var result = new StringBuilder();

            List<Creator> creators = new List<Creator>();

            foreach (var creatorDto in creatorsDto)
            {
                if (!IsValid(creatorDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                Creator creator = new Creator()
                {
                    FirstName = creatorDto.FirstName,
                    LastName = creatorDto.LastName
                };

                foreach (var boardgameDto in creatorDto.BoardgamesDto)
                {
                    if (!IsValid(boardgameDto))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame boardgame = new Boardgame()
                    {
                        Name = boardgameDto.Name,
                        Rating = boardgameDto.Rating,
                        YearPublished = boardgameDto.YearPublished,
                        Mechanics = boardgameDto.Mechanics,
                        CategoryType = (CategoryType)boardgameDto.CategoryType,
                    };

                    creator.Boardgames.Add(boardgame);
                }

                creators.Add(creator);
                result.AppendLine(string.Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName, creator.Boardgames.Count));
            }

            context.Creators.AddRange(creators);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            var sellersDto = JsonConvert.DeserializeObject<List<ImportSellerDto>>(jsonString);

            StringBuilder result = new StringBuilder();

            List<Seller> sellers = new List<Seller>();

            foreach (var sellerDto in sellersDto)
            {
                if (!IsValid(sellerDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                Seller seller = new Seller()
                {
                    Name = sellerDto.Name,
                    Address = sellerDto.Address,
                    Website = sellerDto.Website,
                    Country = sellerDto.Country
                };

                foreach (int boardgameId in sellerDto.BoardgamesId.Distinct())
                {
                    if (context.Boardgames.Any(b => b.Id == boardgameId))
                    {
                        seller.BoardgamesSellers.Add(new BoardgameSeller()
                        {
                            BoardgameId = boardgameId,
                            Seller = seller
                        });
                    }
                    else
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }
                }

                sellers.Add(seller);
                result.AppendLine(string.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count));
            }

            context.Sellers.AddRange(sellers);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
