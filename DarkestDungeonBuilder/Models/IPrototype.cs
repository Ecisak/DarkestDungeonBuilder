namespace DarkestDungeonBuilder.Models;

public interface IPrototype<out T>
{
    T Clone();
}