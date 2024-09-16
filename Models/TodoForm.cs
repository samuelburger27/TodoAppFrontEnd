using System.ComponentModel.DataAnnotations;

namespace WebAss.Models;

public class TodoForm
{
    [Required, MinLength(3, ErrorMessage = "Please use a Name longer than 3 letters."), 
     MaxLength(100, ErrorMessage = "Please use a Name shorter than 100 letters")]
    public string Name { get; set; }
    
    public string Notes { get; set; }
    
    public bool IsFinished { get; set; }
    
    
    public DateTime Deadline { get; set; } = DateTime.Today;

    public void copyTodo(TodoItem Todo)
    {
        Name = Todo.title;
        Notes = Todo.body;
        IsFinished = Todo.is_complete;
        Deadline = Todo.deadline;
    }
}