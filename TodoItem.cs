using WebAss.Models;

namespace WebAss;

public class TodoItem
{
    public int id { get; set; } = 0;
    public string user_id { get; set; } = "";
    public string title { get; set; } = "";
    public string body { get; set; } = "";
    public bool is_complete { get; set; }

    public DateTime deadline { get; set; }
    
    public int? notify_before_minutes { get; set; }

    public bool send_notification { get; set; }
    
    public static int Compare(TodoItem a, TodoItem b)
    {
        if (a.is_complete && !b.is_complete)
            return 1;
        if (!a.is_complete && b.is_complete)
            return -1;
        if (DateTime.Compare(a.deadline, b.deadline) == 0)
            return String.Compare(a.title, b.title, StringComparison.Ordinal);
        return DateTime.Compare(b.deadline, a.deadline);
    }

    public string GetMaxLenBodyText(int length = 20)
    {
        if (body.Length > length)
        {
            return body.Substring(0, length) + "...";
        }
        return body;
    }

    public void CopyFromForm(TodoForm form)
    {
        title = form.Name;
        body = form.Notes;
        is_complete = form.IsFinished;
        deadline = form.Deadline;
        if (form.SendNotification)
            notify_before_minutes = form.NotifyBefore;
        else
            notify_before_minutes = null;
    }
}