namespace AccountAPI.Models
{
    public class Total_search_form
    {
        public int Id { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.MinValue;
        public int Category_id { get; set; } = 0;
        public int Subcategory_id { get; set; } = 0;
        public int SubCount { get; set; } = 0;
        public int SubAmount { get; set; } = 0;

    }
}
