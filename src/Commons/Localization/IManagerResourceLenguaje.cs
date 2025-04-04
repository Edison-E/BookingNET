namespace BookPro.Common.Localization;

public interface IManagerResourceLenguaje
{
    string GetMessage<TEnum>(TEnum key);
}
