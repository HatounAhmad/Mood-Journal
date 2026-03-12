using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Handles all mood journal API endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MoodsController : ControllerBase
{
    // temp storage
    private static List<MoodEntry> _moods = new List<MoodEntry>();
    private static int _nextId = 1;

    private static readonly string[] ValidMoods = 
        { "happy", "sad", "anxious", "calm", "excited", "tired", "angry" };

    [HttpGet]
    public ActionResult<List<MoodEntry>> GetAll([FromQuery] string? mood)
    {
        if (!string.IsNullOrEmpty(mood))
        {
            var filtered = _moods
                .Where(m => m.Mood.ToLower() == mood.ToLower())
                .ToList();
            return Ok(filtered);
        }
        return Ok(_moods);
    }

    // GET by id
    [HttpGet("{id}")]
    public ActionResult<MoodEntry> GetById(int id)
    {
        var mood = _moods.FirstOrDefault(m => m.Id == id);
        if (mood == null) return NotFound("Mood entry not found");
        return Ok(mood);
    }

    // POST 
    [HttpPost]
    public ActionResult<MoodEntry> Create([FromBody] MoodEntry entry)
    {
        // Validation as mood cannot be empty
        if (string.IsNullOrWhiteSpace(entry.Mood))
            return BadRequest("Mood cannot be empty");

        // Validation as mood must be a valid value
        if (!ValidMoods.Contains(entry.Mood.ToLower()))
            return BadRequest($"Invalid mood. Valid moods are: {string.Join(", ", ValidMoods)}");

        entry.Id = _nextId++;
        entry.Date = DateTime.Now;
        _moods.Add(entry);
        return CreatedAtAction(nameof(GetById), new { id = entry.Id }, entry);
    }

    // PUT 
    [HttpPut("{id}")]
    public ActionResult<MoodEntry> Update(int id, [FromBody] MoodEntry updated)
    {
        var mood = _moods.FirstOrDefault(m => m.Id == id);
        if (mood == null) return NotFound("Mood entry not found");

        if (string.IsNullOrWhiteSpace(updated.Mood))
            return BadRequest("Mood cannot be empty");

        if (!ValidMoods.Contains(updated.Mood.ToLower()))
            return BadRequest($"Invalid mood. Valid moods are: {string.Join(", ", ValidMoods)}");

        mood.Mood = updated.Mood;
        mood.Note = updated.Note;

        return Ok(mood);
    }

    // DELETE 
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var mood = _moods.FirstOrDefault(m => m.Id == id);
        if (mood == null) return NotFound("Mood entry not found");
        _moods.Remove(mood);
        return NoContent();
    }
}