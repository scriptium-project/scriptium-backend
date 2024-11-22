using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.DTOs
{
    public enum CollectionStatus
    {
        Error = 0,
        NotFound = 1,
        AlreadyDone = 2,
        Succeed = 3
    }
    public class CollectionProcessResultDTO
    {
        public required string CollectionName { get; set; }
        public required int Code { get; set; }
        public string? Message { get; set; }
    }

    public static class CollectionInsertResultDTOExtension
    {

        public static CollectionProcessResultDTO GetCollectionProcessResultDTO(this Collection? collection, string CollectionName)
        {
            return new CollectionProcessResultDTO
            {
                CollectionName = CollectionName,
                Code = 404,
                Message = $"Collection named '{CollectionName}' not found."
            };

        }

        public static CollectionProcessResultDTO GetCollectionProcessResultDTO(this Collection collection, CollectionStatus status)
        {
            if (status == CollectionStatus.Succeed)
                return new CollectionProcessResultDTO
                {
                    CollectionName = collection.Name,
                    Code = 200,
                    Message = $"The verse has been successfully removed from collection named '{collection.Name}'."
                };

            else if (status == CollectionStatus.AlreadyDone)
                return new CollectionProcessResultDTO
                {
                    CollectionName = collection.Name,
                    Code = 409,
                    Message = $"Collection '{collection.Name}' has already conform demanded situation."

                };

            else if (status == CollectionStatus.NotFound)
                return new CollectionProcessResultDTO
                {
                    CollectionName = collection.Name,
                    Code = 409,
                    Message = $"Content not found in collection '{collection.Name}'."
                };

            else //Error case, which is more likely to not be.
                return new CollectionProcessResultDTO
                {
                    CollectionName = collection.Name,
                    Code = 500,
                    Message = $"An unexpected error has been occurred while saving to collection '{collection.Name}'."
                };



        }

    }
}