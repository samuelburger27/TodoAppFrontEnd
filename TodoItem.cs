namespace WebAss;

public class TodoItem
{
    public int? id { get; set; }
    public string user_id { get; set; }
    public string title { get; set; }
    public string body { get; set; }
    public bool is_complete { get; set; }
    
    public DateTime deadline { get; set; }

    public string GetMaxLenBodyText(int length = 20)
    {
        if (body.Length > length)
        {
            return body.Substring(0, length) + "...";
        }
        return body;
    }
}