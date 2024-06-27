using System.Diagnostics.CodeAnalysis;

namespace RelatedECS.Filters;

/// <summary>
/// Интерфейс, описывающий фильтр сущностей.
/// </summary>
/// <typeparam name="TReturnType">Возвращаемый тип.</typeparam>
public interface IFilter<TReturnType>
{
    /// <summary>
    /// Количество сущностей в фильтре.
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Получить сущности фильтра.
    /// </summary>
    /// <returns>Список сущностей.</returns>
    public IEnumerable<TReturnType> Entities();

    /// <summary>
    /// Получить первую сущность в фильтре. Если фильтр пустой - возвращается <c>null</c>.
    /// </summary>
    /// <param name="entity">Первая сущность в фильтре.</param>
    /// <returns>Есть ли в фильтре хотя бы одна сущность.</returns>
    public bool TryGetFirst([MaybeNullWhen(false)] out TReturnType entity);
}