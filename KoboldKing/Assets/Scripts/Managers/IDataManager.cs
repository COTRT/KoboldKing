using Assets.Scripts.Data;
using Assets.Scripts.Managers;

public interface IGameManager
{
    ManagerStatus Status { get; }
    void Startup(DataService dataService);
}