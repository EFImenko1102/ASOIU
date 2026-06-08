using System;

namespace SEM_6;

public class LibraryItemValidator
{
    public void ValidateBook(string title, string author, int year)
    {
        ValidateTitle(title);
        if (string.IsNullOrWhiteSpace(author))
        {
            throw new ArgumentException("Автор не может быть пустым");
        }
        if (year < 1000 || year > DateTime.Now.Year)
        {
            throw new ArgumentException("Некорректный год издания");
        }
    }

    public void ValidateMagazine(string title, int issueNumber)
    {
        ValidateTitle(title);
        if (issueNumber <= 0)
        {
            throw new ArgumentException("Номер выпуска должен быть положительным");
        }
    }

    private void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Название не может быть пустым");
        }
    }
}
