using DBL.Entities;

namespace DBL.Models
{
    public static class ItemsMapper
    {
        public static List<FaceBookItems> MapFromRangeData(IList<IList<object>> values)
        {
            var items = new List<FaceBookItems>();

            foreach (var value in values)
            {
                FaceBookItems item = new()
                {
                    Id = value[0].ToString(),
                    Title = value[1].ToString(),
                    Description = value[2].ToString(),
                    Availability = value[3].ToString(),
                    Condition = value[4].ToString(),
                    Price = value[5].ToString(),
                    Link = value[6].ToString(),
                    Image_link = value[7].ToString(),
                    Brand = value[8].ToString(),
                    Google_product_category = value[9].ToString(),
                    Fb_product_category = value[10].ToString(),
                    Quantity_to_sell_on_facebook = value[11].ToString(),
                    Sale_price = value[12].ToString(),
                    Sale_price_effective_date = value[13].ToString(),
                    Item_group_id = value[14].ToString(),
                    Gender = value[15].ToString(),
                    Color = value[16].ToString(),
                    Size = value[17].ToString(),
                    Age_group = value[18].ToString(),
                    Material = value[19].ToString(),
                    Pattern = value[20].ToString(),
                    Shipping = value[21].ToString(),
                    Shipping_weight = value[22].ToString(),
                };

                items.Add(item);
            }

            return items;
        }

        public static IList<IList<object>> MapToRangeData(FaceBookItems item)
        {
            var objectList = new List<object>() {
                item.Id,
                item.Title,
                item.Description,
                item.Availability,
                item.Condition,
                item.Price,
                item.Link,
                item.Image_link,
                item.Brand,
                item.Google_product_category,
                item.Fb_product_category,
                item.Quantity_to_sell_on_facebook,
                item.Sale_price,
                item.Sale_price_effective_date,
                item.Item_group_id,
                item.Gender,
                item.Color,
                item.Size,
                item.Age_group,
                item.Material,
                item.Pattern,
                item.Shipping,
                item.Shipping_weight};
            var rangeData = new List<IList<object>> { objectList };
            return rangeData;
        }
    }
}
