namespace GamePlatform.WebUI.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Popularity { get; set; }
        public decimal? UnitPrice { get; set; }
        public string Storage { get; set; }
        public string ImagePath { get; set; }
       
        public string CategoryName { get; set; }
    }
}
