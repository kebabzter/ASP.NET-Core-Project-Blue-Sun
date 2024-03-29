﻿namespace BlueSun.Data
{
    public class DataConstants
    {
        public class User
        {
            public const int FullNameMinLength = 3;
            public const int FullNameMaxLength = 35;
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;

        }

        public class NFTCollection
        {
            public const int NameMaxLength = 30;
            public const int NameMinLength = 2;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 10000;
        }

        public class NFT
        {
            public const int NameMaxLength = 20;
            public const int NameMinLength = 2;

            public const int MinPrice = 0;
            public const int MaxPrice = 10000;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 10000;
        }
        
        public class Category
        {
            public const int NameMaxLength = 20;
        }

        public class Artist
        {
            public const int NameMinLength = 2; 
            public const int NameMaxLength = 30;
            public const int PhoneNumberMinLength = 6;
            public const int PhoneNumberMaxLength = 25;
        }
    }
}
