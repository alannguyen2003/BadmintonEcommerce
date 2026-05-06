namespace BadmintonEcommerce.Infrastructure.Persistence.Abstraction;

public static class CatalogPreData
{
    public static class Category
    {
        public static class Root
        {
            public const string Badminton = "Badminton";
        }

        public static class ChildCategory
        {
            public const string BadmintonRacquets = "Racquests";
            public const string BadmintonShoes = "Shoes";
            public const string BadmintonAccessories = "Accessories";
            public const string BadmintonApparel = "Apparel";
            public const string BadmintonShuttlecocks = "Shuttlecocks";
            public const string BadmintonStrings = "Strings";
        }
    }

    public static class Product
    {
        public static class Brand
        {
            public const string Yonex = "Yonex";
            public const string Lining = "Lining";
        }

        public static class Option
        {
            public static class RacquetOption
            {
                public const string Grip = "Grip";
                public const string Weight = "Weight";

                public static class GripValues
                {
                    public const string GripG5 = "G5";
                    public const string GripG6 = "G6";
                }

                public static class WeightValues
                {
                    public const string Weight2U = "2U";
                    public const string Weight3U = "3U";
                    public const string Weight4U = "4U";
                    public const string Weight5U = "5U";
                }
            }

            public static class ShoeOption
            {
                public const string Size = "Size";
                public const string Model = "Model";

                public static class SizeValues
                {
                    public const string Size35 = "35";
                    public const string Size36 = "36";
                    public const string Size37 = "37";
                    public const string Size38 = "38";
                    public const string Size39 = "39";
                    public const string Size40 = "40";
                    public const string Size41 = "41";
                    public const string Size42 = "42";
                    public const string Size43 = "43";
                    public const string Size44 = "44";
                    public const string Size45 = "45";
                    public const string Size46 = "46";
                    public const string Size47 = "47";
                }

                public static class ModelValues
                {
                    public const string AerusZWideNavyBlue = "Navy Blue";
                    public const string AerusZWideFlashGreen = "Flash Green";
                    public const string AerusZWideIndigo = "Indigo";
                    public const string AerusZWideWhiteGreen = "White Green";

                    public const string AerusZMenNavyBlue = "Navy Blue";
                    public const string AerusZMenIndigo = "Indigo";
                    public const string AerusZMenWhiteGreen = "White Green";
                    public const string AerusZMenFlashGreen = "Flash Green";

                    public const string AerusZWomenNavyBlue = "Navy Blue";
                    public const string AerusZWomenIndigo = "Indigo";
                    public const string AerusZWomenFlashGreen = "Flash Green";
                    public const string AerusZWomenWhiteGreen = "White Green";

                    public const string EclipsonZ3MenNavyBlue = "Navy Blue";
                    public const string EclipsonZ3MenWhiteGold = "White Gold";
                    public const string EclipsonZ3MenLightBlue = "Light Blue";

                    public const string EclipsonZ3WomenWhite = "White";
                    public const string EclipsonZ3WomenWhitePurple = "White Purple";

                    public const string CascadeDriveOrangeBlack = "Orange Black";
                    public const string CascadeDriveOcean = "Ocean";
                    public const string CascadeDriveGreyLightGreen = "Grey Light Green";
                    public const string CascadeDriveBlackBlue = "Black Blue";
                    public const string CascadeDriveGraphite = "Graphite";
                    public const string CascadeDriveClearBlue = "Clear Blue";
                    public const string CascadeDriveMatteWhite = "Matte White";
                    public const string CascadeDriveWhiteGreen = "White Green";

                    public const string Dial88Version3Black = "Black";
                    public const string Dial88Version3White = "White";
                    public const string Dial88Version3LightBlue = "Light Blue";

                    public const string SHB65Z3MenWhite = "White";
                    public const string SHB65Z3MenWhiteBlue = "White Blue";
                    public const string SHB65Z3MenBlack = "Black";
                    public const string SHB65Z3MenRedBlack = "Red Black";

                    public const string ComfortZ3WideBLKMIN = "BLKMIN";
                    public const string ComfortZ3WideOFWTRD = "OFWTRD";

                    public const string ComfortZ3MenDKRD = "DKRD";
                    public const string ComfortZ3MenDKGY = "DKGY";
                }
            }

            public static class ApparelOption
            {
                public const string Size = "Size";

                public static class SizeValues
                {
                    public const string SizeS = "S";
                    public const string SizeM = "M";
                    public const string SizeL = "L";
                    public const string SizeXL = "XL";
                    public const string Size2XL = "2XL";
                    public const string Size3XL = "3XL";
                    public const string Size4XL = "4XL";
                }
            }
        }
    }
}