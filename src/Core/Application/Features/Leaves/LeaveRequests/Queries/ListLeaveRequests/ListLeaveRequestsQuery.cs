using Application.Abstractions.Messaging;
using Application.DTOs.Leaves;

namespace Application.Features.Leaves.LeaveRequests.Queries.ListLeaveRequests;
/// <param name="Status">"Pending" | "Approved" | "Rejected" | null (=all)</param>
public sealed record ListLeaveRequestsQuery(string? Status = null)
    : IQuery<IReadOnlyCollection<LeaveRequestDto>>;
