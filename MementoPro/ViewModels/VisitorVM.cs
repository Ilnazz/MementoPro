using MementoPro.Database.Models;

namespace MementoPro.ViewModels;

public class VisitorVM
{
    public Visitor Visitor { get; init; }

    public VisitorVM(Visitor visitor)
    {
        Visitor = visitor;
    }
}
