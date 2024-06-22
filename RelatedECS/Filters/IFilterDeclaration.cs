using RelatedECS.Filters.Conditions;

namespace RelatedECS.Filters;

/// <summary>
/// Интерфейс объявления фильтра сущностей.
/// </summary>
public interface IFilterDeclaration
{
    /// <summary>
    /// Добавить включаемый тип компоненты.
    /// </summary>
    /// <remarks>Будут отбираться сущности, содержащие данный тип компоненты.</remarks>
    /// <typeparam name="T">Тип компоненты. Ограничение - структура.</typeparam>
    /// <returns>Декларация.</returns>
    public IFilterDeclaration With<T>() where T : struct;

    /// <summary>
    /// Добавить исключаемый тип компоненты.
    /// </summary>
    /// <remarks>Будут отбираться сущности, <b>не</b> содержащие данный тип компоненты.</remarks>
    /// <typeparam name="T">Тип компоненты. Ограничение - структура.</typeparam>
    /// <returns>Декларация.</returns>
    public IFilterDeclaration Without<T>() where T : struct;

    /// <summary>
    /// Обозначить как фильтр одиночной сущности.
    /// </summary>
    /// <remarks>Означает, что сущность описываемая в декларации будет только одна.</remarks>
    /// <returns>Декларация.</returns>
    public IFilterDeclaration AsSingleton();

    /// <summary>
    /// Добавить включаемые типы компонент.
    /// </summary>
    /// <param name="types">Включаемые типы комопонент.</param>
    public void WithTypes(params Type[] types);

    /// <summary>
    /// Добавить исключаемые типы компонент.
    /// </summary>
    /// <param name="types">Исключаемые типы компонент.</param>
    public void WithoutTypes(params Type[] types);

    /// <summary>
    /// Получить все включаемые типы, добавленные в декларацию.
    /// </summary>
    /// <returns>Включаемые типы.</returns>
    public Type[] GetWithTypes();

    /// <summary>
    /// Получить все исключаемые типы, добавленные в декларацию.
    /// </summary>
    /// <returns>Исключаемые типы.</returns>
    public Type[] GetWithoutTypes();

    /// <summary>
    /// Содержит ли декларация указанный включаемый тип.
    /// </summary>
    /// <param name="type">Включаемый тип.</param>
    /// <returns><c>True</c> - если содержит, <c>False</c> - если не содержит.</returns>
    public bool HasWithType(Type type);

    /// <summary>
    /// Содержит ли декларация указанный исключаемый тип.
    /// </summary>
    /// <param name="type">Исключаемый тип.</param>
    /// <returns><c>True</c> - если содержит, <c>False</c> - если не содержит.</returns>
    public bool HasWithoutType(Type type);

    /// <summary>
    /// Собрать фильтр.
    /// </summary>
    /// <returns>Фильтр.</returns>
    public EntitiesFilter Build();

    /// <summary>
    /// Собрать фильтр, как фильтр с условием.
    /// </summary>
    /// <returns>Фильтр с условием.</returns>
    public EntitiesConditionedFilter BuildAsConditioned();
}