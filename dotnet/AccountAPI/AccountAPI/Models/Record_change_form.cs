namespace AccountAPI.Models
{
    public class Record_change_form
    {
        public int target_id { get; set; } = 0;
        public RecordForm content { get; set; } = new RecordForm();
    }
}
