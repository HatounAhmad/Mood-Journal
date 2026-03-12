public class MoodEntry
{
    public int Id { get; set; }
    public string Mood { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public bool IsSpecialOccasion { get; set; }
    public string OccasionName { get; set; } = string.Empty;
    public int MoodScore { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}