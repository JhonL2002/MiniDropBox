﻿namespace MiniDropBox.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        bool IsInRole(string role);
    }
}
