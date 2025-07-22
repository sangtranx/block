public static class EnumName
{
    public static string ShopCategoryName(ShopCategory type)
    {
        switch (type)
        {
            case ShopCategory.BLOCK:
                return "Block";
            case ShopCategory.BACKGROUND:
                return "Background";
            case ShopCategory.GAMEPLAY:
                return "Frame";
            default:
                return "Unknown";
        }
    }
}