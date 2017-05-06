using Assets.Scripts.Data;
using Assets.Scripts.Managers;
using System;

public interface IGameManager
{
    event UnhandledExceptionEventHandler OnException;
    ManagerStatus Status { get; }
    void Startup(DataService dataService);
}