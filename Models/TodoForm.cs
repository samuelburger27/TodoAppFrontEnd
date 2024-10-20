using System.ComponentModel.DataAnnotations;

namespace WebAss.Models;

public class TodoForm
{
    [Required, MinLength(3, ErrorMessage = "Please use a Name longer than 3 letters."),
     MaxLength(100, ErrorMessage = "Please use a Name shorter than 100 letters")]
    public string Name { get; set; } = "";

    public string Notes { get; set; } = "";
    
    public bool IsFinished { get; set; }
    
    public DateTime Deadline { get; set; } = DateTime.Today;

    public bool SendNotification { get; set; } = true;

    public int NotifyBefore { get; set; } = 15;

    public void copyTodo(TodoItem todo)
    {
        Name = todo.title;
        Notes = todo.body;
        IsFinished = todo.is_complete;
        Deadline = todo.deadline;
        if (todo.notify_before_minutes == null)
            SendNotification = false;
        else
        {
            SendNotification = true;
            NotifyBefore = (int) todo.notify_before_minutes;
        }
    }
}