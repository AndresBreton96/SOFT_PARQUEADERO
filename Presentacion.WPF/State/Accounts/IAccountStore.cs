using System;
using Transversales.Modelos;

namespace Presentacion.WPF.State.Accounts
{
    public interface IAccountStore
    {
        SystemUsers CurrentUser { get; set; }
        event Action StateChanged;
    }
}
