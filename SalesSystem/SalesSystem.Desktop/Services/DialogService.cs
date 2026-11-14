using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services;

public sealed class DialogService : IDialogService
{
    public bool Confirm(string message, string title = "تأكيد")
    {
        return clsDialogs.Confirm(message);
    }
}

