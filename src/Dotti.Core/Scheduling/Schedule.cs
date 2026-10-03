using System;
using System.Collections.Generic;
using System.Text;

namespace Dotti;

/// <summary>
/// Describes when a job runs: a rule that, given an instant, produces the next instant the job is due.
/// </summary>
/// <remarks>
/// <para>
/// Create schedules with <see cref="Every(TimeSpan)"/> or <see cref="Cron(string, TimeZoneInfo?)"/>.
/// Schedules are immutable and thread-safe, and they never read the current time; the caller always
/// supplies the instant to search from.
/// </para>
/// <para>
/// All occurrences are absolute instants returned in UTC. Time zones only affect how cron expressions
/// are interpreted, never the returned values.
/// </para>
/// </remarks>
public abstract class Schedule
{
    /// <summary>
    /// Creates a schedule that occurs at a fixed elapsed interval.
    /// </summary>
    /// <param name="interval">The time between occurrences. Must be ⟨greater than zero / at least one second⟩.</param>
    /// <returns>A schedule that occurs every <paramref name="interval"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="interval"/> is ⟨zero or negative / less than one second⟩.
    /// </exception>
    /// <remarks>
    /// <para>
    /// Occurrences are aligned to ⟨the Unix epoch (1970-01-01T00:00:00Z)⟩, so a five-minute interval
    /// occurs at :00, :05, :10 and so on, regardless of when the application started.
    /// </para>
    /// <para>
    /// Intervals measure elapsed time and are unaffected by time zones or daylight saving time:
    /// occurrences are always exactly <paramref name="interval"/> apart.
    /// </para>
    /// </remarks>
    public static Schedule Every(TimeSpan interval) => throw new NotImplementedException();

    /// <summary>
    /// Creates a schedule from a cron expression, evaluated in the given time zone.
    /// </summary>
    /// <param name="expression">
    /// A cron expression: ⟨five fields (minute, hour, day of month, month, day of week), or six
    /// fields with a leading seconds field⟩.
    /// </param>
    /// <param name="zone">
    /// The time zone in which to interpret the expression. Defaults to <see cref="TimeZoneInfo.Utc"/>
    /// when <see langword="null"/>. The local time zone is never used implicitly.
    /// </param>
    /// <returns>A schedule that occurs whenever <paramref name="expression"/> matches.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="expression"/> is <see langword="null"/>.</exception>
    /// <exception cref="FormatException"><paramref name="expression"/> is not a valid cron expression.</exception>
    /// <remarks>
    /// <para>The expression is validated immediately, so an invalid expression fails at startup.</para>
    /// <para>Daylight saving time transitions in <paramref name="zone"/> are handled as follows:</para>
    /// <list type="bullet">
    ///   <item><description>
    ///   When clocks spring forward and a matching local time does not exist, the schedule occurs once,
    ///   at the next valid instant.
    ///   </description></item>
    ///   <item><description>
    ///   When clocks fall back and a matching local time occurs twice, the schedule occurs once, at the
    ///   first instance. Expressions that repeat within the hour (for example <c>*/30 * * * *</c>) occur
    ///   in both passes.
    ///   </description></item>
    /// </list>
    /// </remarks>
    public static Schedule Cron(string expression, TimeZoneInfo? zone = null) => throw new NotImplementedException();

    /// <summary>
    /// Returns the first occurrence of this schedule that is strictly later than <paramref name="after"/>.
    /// </summary>
    /// <param name="after">
    /// The instant to search from. Its offset does not affect the result; only the absolute instant matters.
    /// </param>
    /// <returns>
    /// The next occurrence as a UTC <see cref="DateTimeOffset"/> (offset zero), or <see langword="null"/>
    /// if the schedule has no further occurrences.
    /// </returns>
    /// <remarks>
    /// The search is exclusive: if <paramref name="after"/> falls exactly on an occurrence, the following
    /// occurrence is returned. Calling this repeatedly with each returned value enumerates the schedule
    /// without duplicates.
    /// </remarks>
    public abstract DateTimeOffset? GetNextOccurrence(DateTimeOffset after);
}
