namespace AccountAPI.Models
{
    public class Records_search_form
    {
        public int Id { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.MinValue;
        public int Category_id { get; set; } = 0;
        public int SubCategory_id { get; set; } = 0;
        public int User_id { get; set; } = 0;
        public int Amount { get; set; } = 0;
    }
}
