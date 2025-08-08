using System.Linq.Expressions;

namespace SharedKernel.Extensions.Linq;

/// <summary>
/// Provides extension methods for LINQ-related operations on collections.
/// </summary>
public static class LinqExtensions
{
    /// <summary>
    /// Determines whether the specified <see cref="IEnumerable{T}"/> is <c>null</c> or contains no elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="source">The collection to check for <c>null</c> or emptiness.</param>
    /// <returns>
    /// <c>true</c> if the collection is <c>null</c> or contains no elements; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source) => source is null || !source.Any();

    /// <summary>
    /// Determines whether the specified <see cref="ICollection{T}"/> is <c>null</c> or contains no elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="source">The collection to check for <c>null</c> or emptiness.</param>
    /// <returns>
    /// <c>true</c> if the collection is <c>null</c> or contains no elements; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullOrEmpty<T>(this ICollection<T>? source) => source is null || !source.Any();

    /// <summary>
    /// Applies a filter to the queryable source if the specified condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source.</typeparam>
    /// <param name="source">The queryable data source.</param>
    /// <param name="condition">A boolean value indicating whether to apply the predicate.</param>
    /// <param name="predicate">The filter expression to apply if the condition is true.</param>
    /// <returns>
    /// A queryable containing elements that satisfy the predicate if the condition is true;
    /// otherwise, the original unfiltered queryable.
    /// </returns>
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> source,
        bool condition,
        Expression<Func<T, bool>> predicate) => condition ? source.Where(predicate) : source;
}
